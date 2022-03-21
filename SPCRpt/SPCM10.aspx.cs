using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Reporting.WebForms;

namespace SPCRpt
{
    public partial class SPCM10 : System.Web.UI.Page
    {
        DateTimeFormatInfo dateTimeFormatterProvider = DateTimeFormatInfo.CurrentInfo.Clone() as DateTimeFormatInfo;
        string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["Sqlcon"].ToString();
        SqlConnection sqlCon = new SqlConnection();
        SqlCommand sqlCmd = new SqlCommand();
        SqlDataAdapter sqlDap = new SqlDataAdapter();
        DataTable dtMachine = new DataTable();
        DataTable dtProductIds = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                GetMachineadnPeoductInfo();
                DataTable dt = (DataTable)ViewState["dtMachine"];
                if (dtMachine.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[Convert.ToInt32(ViewState["A"])];
                    this.lblMachineId.Text = dr["MachineID"].ToString();
                    this.lblProductId.Text = dr["Product_Id"].ToString();
                    this.lblProductType.Text = dr["Product_Type"].ToString();
                    List<ReportParameter> paramList = new List<ReportParameter>();
                    paramList.Add(new ReportParameter("MachineId", this.lblMachineId.Text, true));
                    paramList.Add(new ReportParameter("ProductID", this.lblProductId.Text, true));
                    paramList.Add(new ReportParameter("ProductType", this.lblProductType.Text, true));
                    this.rptTableView.LocalReport.SetParameters(paramList);
                    this.rptTableView.LocalReport.Refresh();
                }
            }
        }
        public void GetMachineadnPeoductInfo()
        {
            try
            {
                string[] strShift = GetShiftName();
                this.lblFrom.Text = strShift[1];
                this.lblShift.Text = strShift[0];
                this.lblFrom.Text = "2019-05-09";
                dtMachine = GetDistinctProductId(this.lblFrom.Text, DateTime.Now.ToString(), "SPCM10");
                ViewState["dtMachine"] = dtMachine;
                ViewState["B"] = dtMachine.Rows.Count;
                ViewState["A"] = 0;

            }
            catch (Exception)
            {
                //throw;
            }
        }
        protected void Timer2_Tick(object sender, EventArgs e)
        {
            GetMachineadnPeoductInfo();
        }
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if ((Convert.ToInt32(ViewState["A"]) + 1) >= Convert.ToInt32(ViewState["B"]))
            {
                ViewState["A"] = 0;
            }
            else
            {
                ViewState["A"] = Convert.ToInt32(ViewState["A"]) + 1;
            }
            string[] strShift = GetShiftName();
            this.lblFrom.Text = strShift[1];
            this.lblShift.Text = strShift[0];
            DataTable dt = (DataTable)ViewState["dtMachine"];
            DataRow dr = dt.Rows[Convert.ToInt32(ViewState["A"])];
            this.lblMachineId.Text = dr["MachineID"].ToString();
            this.lblProductId.Text = dr["Product_Id"].ToString();
            this.lblProductType.Text = dr["Product_Type"].ToString();
            List<ReportParameter> paramList = new List<ReportParameter>();
            paramList.Add(new ReportParameter("MachineId", this.lblMachineId.Text, true));
            paramList.Add(new ReportParameter("ProductID", this.lblProductId.Text, true));
            paramList.Add(new ReportParameter("ProductType", this.lblProductType.Text, true));
            this.rptTableView.LocalReport.SetParameters(paramList);
            this.rptTableView.LocalReport.Refresh();
        }

        public string[] GetShiftName()
        {
            DataTable dtTiming = GetShiftTimings();
            string[] strShift = { string.Empty, string.Empty };
            try
            {
                DataRow drShiftA = dtTiming.Rows[0];
                DataRow drShiftB = dtTiming.Rows[1];
                DataRow drShiftC = dtTiming.Rows[2];
                Int64 presentTime = Convert.ToInt64(DateTime.Now.ToString("HHmmss"));
                string strShiftAStartTime = drShiftA["Shift_Start_Time"].ToString().Replace(":", "");
                Int64 intAStartTime = Convert.ToInt64(strShiftAStartTime.ToString().Replace(":", ""));
                string strShiftAEndTime = drShiftA["Shift_End_Time"].ToString().Replace(":", "");
                Int64 intAEndTime = Convert.ToInt64(strShiftAEndTime.ToString().Replace(":", ""));
                string strShiftBStartTime = drShiftB["Shift_Start_Time"].ToString().Replace(":", "");
                Int64 intBStartTime = Convert.ToInt64(strShiftBStartTime.ToString().Replace(":", ""));
                string strShiftBEndTime = drShiftB["Shift_End_Time"].ToString().Replace(":", "");
                Int64 intBEndTime = Convert.ToInt64(strShiftBEndTime.ToString().Replace(":", ""));
                dateTimeFormatterProvider.ShortDatePattern = "dd/MM/yyyy hh:mm:ss";
                // strDate = DateTime.Parse(strDate, dateTimeFormatterProvider).ToString("yyyy-MM-dd");
                //  strTime = DateTime.Parse(strTime, dateTimeFormatterProvider).ToString("yyyy-MM-dd");
                strShift[1] = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), dateTimeFormatterProvider).ToString("yyyy-MM-dd");
                // if (Enumerable.Range(6000, 14000).Contains(presentTime))
                if (presentTime >= intAStartTime && intAEndTime >= presentTime)
                {
                    strShift[0] = "Shift A";
                }
                else
                {
                    if (presentTime >= intBStartTime && intBEndTime >= presentTime)
                    {
                        strShift[0] = "Shift B";
                    }
                    else
                    {
                        if ((presentTime >= 000000 && presentTime <= 005959) || (presentTime >= 010000 && presentTime <= 060000))
                        {
                            strShift[1] = (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), dateTimeFormatterProvider).AddDays(-1)).ToString("yyyy-MM-dd");
                        }
                        strShift[0] = "Shift C";
                    }
                    // ClsGlobal.strShiftName = strShift[1];
                }
            }
            catch (Exception ex)
            {
                strShift[0] = string.Empty;
                strShift[1] = string.Empty;
                //InsertIntoErrorDetails(ex.ToString(), "SplitInputString-ShiftName", InputString);
                //return;
            }
            return strShift;
        }

        public DataTable GetShiftTimings()
        {
            DataTable dtTiming = new DataTable();
            try
            {
                sqlCon = new SqlConnection(strCon);
                sqlCon.Open();
                sqlCmd = new SqlCommand("SELECT Shift_Start_Time, Shift_End_Time FROM Shift_Master(nolock)", sqlCon);
                sqlDap = new SqlDataAdapter(sqlCmd);
                sqlDap.Fill(dtTiming);
                //return dtTiming;
            }
            catch (Exception ex)
            {
                dtTiming = null;
                //InsertIntoErrorDetails(ex.ToString(), "SplitInputString-SelectShifName", InputString);
                //return;
            }
            finally
            {
                sqlCon.Close();
            }
            return dtTiming;
        }

        public DataTable GetMachineID(string strURLName)
        {
            DataTable dtMachine = new DataTable();
            try
            {
                sqlCon = new SqlConnection(strCon);
                sqlCon.Open();
                sqlCmd = new SqlCommand("select MachineId from tbl_SPC_Report_Settings where URLName=@URLName", sqlCon);
                sqlCmd.Parameters.AddWithValue("@URLName", strURLName);
                sqlDap = new SqlDataAdapter(sqlCmd);
                sqlDap.Fill(dtMachine);
                //return dtTiming;
            }
            catch (Exception ex)
            {
                dtMachine = null;
                //InsertIntoErrorDetails(ex.ToString(), "SplitInputString-SelectShifName", InputString);
                //return;
            }
            finally
            {
                sqlCon.Close();
            }
            return dtMachine;
        }

        public DataTable GetDistinctProductId(string strFromdate, string strTodate, string URLName)
        {
            DataTable dtProductId = new DataTable();
            try
            {
                sqlCon = new SqlConnection(strCon);
                sqlCon.Open();
                sqlCmd = new SqlCommand("select DISTINCT RS.ID,RS.URLName,RS.MachineId,WTM.Product_Type,WTM.Product_Id from tbl_SPC_Report_Settings(NOLOCK) RS left join tbl_Weight_Thickness_Measuring (NOLOCK) WTM ON RS.MachineId=WTM.Machine_Id WHERE RS.URLName=@URLName AND WTM.Date between @FromDate and @Todate ", sqlCon);
                sqlCmd.Parameters.AddWithValue("@URLName", URLName);
                sqlCmd.Parameters.AddWithValue("@FromDate", strFromdate + " 00:00:00.001");
                sqlCmd.Parameters.AddWithValue("@Todate", strTodate);
                sqlDap = new SqlDataAdapter(sqlCmd);
                sqlDap.Fill(dtProductId);
                return dtProductId;
            }
            catch (SqlException sqlExc)
            {
                dtProductId = null;
                return dtProductId;
            }
            finally
            {
                sqlCon.Close();
            }
        }
    }
}