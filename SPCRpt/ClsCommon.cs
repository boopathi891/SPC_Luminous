using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SPCRpt
{
    public class ClsCommon
    {
        string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["Sqlcon"].ToString();
        SqlConnection sqlCon = new SqlConnection();
        SqlCommand sqlCmd = new SqlCommand();
        SqlDataAdapter sqlDap = new SqlDataAdapter();
        readonly object _lockObject = new object();
        
        public void FillGroupByCloumn(string FromDate, string ToDate, string strProductID, string strProductType, string strShiftName, string strMachineId)
        {
            lock (_lockObject)
            {
                FromDate = "2021-03-11";
                ToDate = "2021-03-11";
                int intGroupSize = 1, i = 0, j = 1;
                int N = 5;
                bool boolDelete = DeleteTemp();
                DataTable dtWTM = WieghtThicknessMeasuring(FromDate, ToDate, strProductID, strProductType, strShiftName, strMachineId, "ALL", "ALL");
                for (i = 0; i < dtWTM.Rows.Count; i++)
                {
                    if (j > N)
                    {
                        intGroupSize++;
                        j = 1;
                    }
                    DataRow dr = dtWTM.Rows[i];
                    bool bolInsert = InsertTemp(intGroupSize, Convert.ToDecimal(dr["Weight"].ToString()));
                    j++;
                }
            }
        }
        private bool DeleteTemp()
        {
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                sqlCon = new SqlConnection(strCon);
                sqlCon.Open();
                SqlCommand sqlcmd = new SqlCommand("delete from tbl_temp_SPCDashboard", sqlCon);
                sqlcmd.ExecuteNonQuery();
                return true;
            }
            finally
            {
                sqlCon.Close();
            }
        }
        private DataTable WieghtThicknessMeasuring(string strFromDate, string strToDate, string strProductId, string strProductType, string strShift, string strMachineId, string strMouldId, string strEmployee)
        {
            DataTable dtWTM = new DataTable();
            
            try
            {
                sqlCon = new SqlConnection(strCon);
                sqlCon.Open();
                sqlCmd = new SqlCommand("SP_SPC_Data", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@FromDate", strFromDate);
                sqlCmd.Parameters.AddWithValue("@ToDate", strToDate);
                sqlCmd.Parameters.AddWithValue("@ProductId", strProductId);
                sqlCmd.Parameters.AddWithValue("@ProductType", strProductType);
                sqlCmd.Parameters.AddWithValue("@Shift", strShift);
                sqlCmd.Parameters.AddWithValue("@MachineId", strMachineId);
                sqlCmd.Parameters.AddWithValue("@MouldId", strMouldId);
                sqlCmd.Parameters.AddWithValue("@Employee", strEmployee);
                sqlDap = new SqlDataAdapter(sqlCmd);
                sqlDap.Fill(dtWTM);
                return dtWTM;
            }
            catch (SqlException sqlExc)
            {
                return dtWTM;
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private bool InsertTemp(int intGroupNo, decimal strWeight)
        {
            try
            {
                sqlCon = new SqlConnection(strCon);
                sqlCon.Open();
                sqlCmd = new SqlCommand("INSERT INTO tbl_temp_SPCDashboard(GroupNo, Weight_Temp) VALUES (@GroupNo, @Weight)", sqlCon);
                sqlCmd.Parameters.AddWithValue("@GroupNo", intGroupNo);
                sqlCmd.Parameters.AddWithValue("@Weight", strWeight);
                sqlCmd.ExecuteNonQuery();
                return true;
            }
            finally
            {
                sqlCon.Close();
            }
        }
    }
}