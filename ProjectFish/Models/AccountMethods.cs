using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace ProjectFish.Models
{
    public class AccountMethods
    {
        public int getAccount(Account account, out string errormsg)
        {
            SqlConnection conn = new SqlConnection();

            conn.ConnectionString = "Server = (localdb)\\mssqllocaldb; Database = ProjectFish; Trusted_Connection = True;";

            String sqlString = "SELECT AccountId, Password FROM Account WHERE Mail = @mail";

            SqlCommand cmd = new SqlCommand(sqlString, conn);

            cmd.Parameters.Add("Mail", SqlDbType.NVarChar, 100).Value = account.Mail;
            
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet dataSet = new DataSet();

            int accountId = 0;

            try
            {
                conn.Open();
                adapter.Fill(dataSet, "user");

                int i = 0;
                i = dataSet.Tables["user"].Rows.Count;

                if (i == 1) //If a matching row is returned on the mail entered 
                {
                    string hashedpw = (string)dataSet.Tables["user"].Rows[0]["Password"]; // Get the encrypted password for that mail from DB

                    if (Crypto.VerifyHashedPassword(hashedpw, account.Password)) // Verify the encrypted password to the entered one
                    {
                        errormsg = "";
                        accountId = Convert.ToInt32(dataSet.Tables["user"].Rows[0]["AccountId"]);
                        return accountId; // Return AccountId if its a match.
                    }
                    else
                    {
                        errormsg = "Wrong username or password";
                        return 0;
                    }
                    
                }
                else
                {
                    errormsg = "Wrong username or password";
                    return 0;
                }

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

        public int CreateAccount(Account account, out string errormsg)
        {
            //Skapa sqlConnection
            SqlConnection conn = new SqlConnection();

            //Koppling mot server
            conn.ConnectionString = "Server = (localdb)\\mssqllocaldb; Database = ProjectFish; Trusted_Connection = True;";

            String sqlstring = "INSERT INTO Account ( Mail, Password ) VALUES ( @mail, @password )";
            SqlCommand cmd = new SqlCommand(sqlstring, conn);

            cmd.Parameters.Add("Mail", SqlDbType.NVarChar, 100).Value = account.Mail;
            cmd.Parameters.Add("password", SqlDbType.NVarChar, 100).Value = passwordEncrypter(account.Password); //account.Password; // encrypt pw

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

        private string passwordEncrypter(string password)
        {
            string hashed = Crypto.HashPassword(password);

            return hashed;
        }
    }
}
