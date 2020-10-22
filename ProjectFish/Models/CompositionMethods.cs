using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFish.Models
{
    public class CompositionMethods
    {
        public List<Composition> GetCompositionList(out string errormsg)
        {
            SqlConnection conn = new SqlConnection();

            conn.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FishLureDB;Integrated Security=True";

            String sqlstring = "SELECT * FROM Species";

            SqlCommand cmd = new SqlCommand(sqlstring, conn);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet dataSet = new DataSet();

            List<Composition> speciesList = new List<Composition>();

            try
            {
                conn.Open();

                adapter.Fill(dataSet, "mySpecies");

                int count = 0;
                int i = 0;
                count = dataSet.Tables["mySpecies"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        Composition composition = new Composition();

                        //species.SpeciesId = Convert.ToInt16(dataSet.Tables["mySpecies"].Rows[i]["SpeciesId"]);
                        //species.Name = dataSet.Tables["mySpecies"].Rows[i]["Name"].ToString();
                        //species.Waters = dataSet.Tables["mySpecies"].Rows[i]["Waters"].ToString();

                        //i++;
                        //speciesList.Add(species);
                    }

                    errormsg = "";
                    return speciesList;
                }
                else
                {
                    errormsg = "Could not get species";
                    return speciesList;
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return speciesList;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
