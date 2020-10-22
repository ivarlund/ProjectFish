using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFish.Models
{
    public class AccountMethods
    {
        public int getAccount(Account account, out string errormsg)
        {
            SqlConnection conn = new SqlConnection();

            conn.ConnectionString = "Server = (localdb)\\mssqllocaldb; Database = ProjectFish; Trusted_Connection = True;";

            String sqlString = "SELECT AccountId FROM Account WHERE Mail = @mail AND Password = @password";

            SqlCommand cmd = new SqlCommand(sqlString, conn);

            cmd.Parameters.Add("Mail", SqlDbType.NVarChar, 100).Value = account.Mail;
            cmd.Parameters.Add("password", SqlDbType.NVarChar, 100).Value = account.Password;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet dataSet = new DataSet();

            int accountId = 0;

            try
            {
                conn.Open();
                adapter.Fill(dataSet, "user");

                int i = 0;
                i = dataSet.Tables["user"].Rows.Count;

                if (i == 1)
                {
                    errormsg = "";
                    accountId = Convert.ToInt32(dataSet.Tables["user"].Rows[0]["AccountId"]);
                    return accountId;
                } else
                {
                    errormsg = "Could not find user";
                    return accountId;
                }

            } 
            catch (Exception e)
            {
                errormsg = e.Message;
                return accountId;

            } 
            finally
            {
                conn.Close();
            }

        }

        public int CreateAccount(Account account, out string errormsg)
        {
            //Skapa sqlConnection
            SqlConnection conn = new SqlConnection();

            //Koppling mot server
            conn.ConnectionString = "Server = (localdb)\\mssqllocaldb; Database = ProjectFish; Trusted_Connection = True;";

            String sqlstring = "INSERT INTO Account ( Mail, Password ) VALUES ( @mail, @password )";
            SqlCommand cmd = new SqlCommand(sqlstring, conn);

            cmd.Parameters.Add("Mail", SqlDbType.NVarChar, 100).Value = account.Mail;
            cmd.Parameters.Add("password", SqlDbType.NVarChar, 100).Value = account.Password;

            try
            {
                conn.Open();

                int i = 0;
                i = cmd.ExecuteNonQuery();

                if (i == 1)
                {
                    errormsg = "";
                }
                else
                {
                    errormsg = "Could not add Account";
                }

                return i;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
