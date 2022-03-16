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
    public partial class SPCM9 : System.Web.UI.Page
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
                dtMachine = GetMachineID("SPCM9");
                ViewState["dtMachine"] = dtMachine;
                ViewState["B"] = dtMachine.Rows.Count;
                ViewState["A"] = 0;
                string[] strShift = GetShiftName();
                this.lblFrom.Text = strShift[1];
                this.lblShift.Text = strShift[0];
                if (dtMachine.Rows.Count > 0)
                {
                    DataTable dt = (DataTable)ViewState["dtMachine"];
                    DataRow dr = dt.Rows[Convert.ToInt32(ViewState["A"])];
                    this.lblMachineId.Text = dr["MachineID"].ToString();
                    dtProductIds = GetDistinctProductId(this.lblFrom.Text, DateTime.Now.ToString(), "ALL", this.lblMachineId.Text);
                    ViewState["ProductID"] = dtProductIds;
                    List<ReportParameter> paramList = new List<ReportParameter>();
                    paramList.Add(new ReportParameter("MachineId", this.lblMachineId.Text, true));
                    this.rptTableView.LocalReport.SetParameters(paramList);
                    this.rptTableView.LocalReport.Refresh();

                    // ViewState["A"] = Convert.ToInt32(ViewState["A"]) + 1;
                }
            }

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
            DataTable dtProductId = (DataTable)ViewState["ProductID"];
            if (dtProductId != null && dtProductId.Rows.Count > 0)
            {
                Random rnd = new Random();
                int RandomRow = rnd.Next(1, dtProductId.Rows.Count);
                DataRow drProductId = dt.Rows[RandomRow];
                this.ddlProductId.Text = drProductId["Product_Id"].ToString();
                this.ddlProductType.Text = drProductId["Product_Type"].ToString();
            }
            List<ReportParameter> paramList = new List<ReportParameter>();
            paramList.Add(new ReportParameter("MachineId", this.lblMachineId.Text, true));
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
                sqlCmd = new SqlCommand("SELECT Shift_Start_Time,Shift_End_Time FROM Shift_Master", sqlCon);
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

        public DataTable GetDistinctProductId(string strFromdate, string strTodate, string strProductType, string MachineId)
        {
            DataTable dtProductId = new DataTable();
            try
            {
                sqlCon = new SqlConnection(strCon);
                sqlCon.Open();
                sqlCmd = new SqlCommand("select distinct Product_Id,Product_Type from tbl_Weight_Thickness_Measuring where Machine_Id=@MachineID and date between @FromDate and @Todate and Product_Type=(CASE WHEN @ProductType='ALL'then Product_Type ELSE @ProductType END) Order by Product_Id", sqlCon);
                sqlCmd.Parameters.AddWithValue("@MachineID", MachineId);
                sqlCmd.Parameters.AddWithValue("@FromDate", strFromdate + " 00:00:00.001");
                sqlCmd.Parameters.AddWithValue("@Todate", strTodate );
                sqlCmd.Parameters.AddWithValue("@ProductType", strProductType);
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