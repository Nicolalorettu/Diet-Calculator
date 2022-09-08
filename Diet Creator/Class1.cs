using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

namespace Diet_Creator
{
    internal class DataCreationDirectory
    {
        public void creationdb()
        {
            String str;
            String file = @"C:\Diet Creator\Database\databasevalorinutritivi.mdf";
            String path = @"C:\Diet Creator\Database\";
            SqlConnection myConn = new SqlConnection("Server =localhost;Integrated security=SSPI;database=master");

            str = "CREATE DATABASE DBVN ON PRIMARY " +
                 "(NAME = DatabaseValoriNutritivi_Data, " +
                 "FILENAME = 'C:\\Diet Creator\\Database\\databasevalorinutritivi.mdf', " +
                 "SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)" +
                 "LOG ON (NAME = DatabaseValoriNutritivi_Log, " +
                 "FILENAME = 'C:\\Diet Creator\\Database\\databasevalorinutritiviLog.ldf', " +
                 "SIZE = 1MB, " +
                 "MAXSIZE = 5MB, " +
                 "FILEGROWTH = 10%)";
            if (!File.Exists(file))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                SqlCommand myCommand = new SqlCommand(str, myConn);
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Diet Creator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
            }

        }
        
    }
    internal class DataTableFeederDB
    {
        public void DataFeeder(DataTable excelfile)
        {
            String strcreatetable;
            String strchecktabletrue;


            SqlConnection myConn = new SqlConnection("Server =localhost;Integrated security=SSPI;database=DBVN");
            strchecktabletrue = "IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME= 'ValoriNutritivi') SELECT 1 ELSE SELECT 0";
            strcreatetable = "CREATE TABLE ValoriNutritivi(ID int PRIMARY KEY, Nome varchar(255), Categoria varchar(255), Unità_di_riferimento varchar(255), Energia_calorie_kcal varchar(255), Lipidi_totali_gr varchar(255), Acidi_grassi_saturi_gr varchar(255), Acidi_grassi_monoinsaturi_gr varchar(255), Acidi_grassi_polinsaturi_gr varchar(255), Glucidi_disponibili_gr varchar(255), Zuccheri_gr varchar(255), Fibra_alimentare_gr varchar(255), Proteine_gr varchar(255), Sale_NaCl_gr varchar(255), Data varchar(255))";
            string qry = "SELECT * FROM ValoriNutritivi";
            string drptbl = "DROP TABLE ValoriNutritivi";
            myConn.Open();
            SqlCommand checktable = new SqlCommand(strchecktabletrue, myConn);
            int x = Convert.ToInt32(checktable.ExecuteScalar());
            //if (x == 0)
            //{
                try
                {
                    if (x == 1)
                    {
                    SqlCommand droptable = new SqlCommand(drptbl, myConn);
                    droptable.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand createtable = new SqlCommand(strcreatetable, myConn);
                        createtable.ExecuteNonQuery();
                        SqlDataAdapter dadpt2 = new SqlDataAdapter(qry, myConn);
                        SqlDataAdapter dataadptr = new SqlDataAdapter();
                        DataSet dataset = new DataSet();
                        string cleaner;
                        dadpt2.Fill(dataset, "ValoriNutritivi");
                        DataTable cleanexcelfile = dataset.Tables["ValoriNutritivi"];
                        cleanexcelfile.PrimaryKey = new DataColumn[] { cleanexcelfile.Columns["ID"] };
                        dataadptr.SelectCommand = new SqlCommand(qry, myConn);
                        SqlCommandBuilder cmdbldr = new SqlCommandBuilder(dataadptr);

                        string[] ColumnsToBeSaved = { "Nome", "Categoria", "Unità di riferimento", "Energia, calorie (kcal)", "Lipidi, totali (g)", "Acidi grassi, saturi (g)", "Acidi grassi, monoinsaturi (g)", "Acidi grassi, polinsaturi (g)", "Glucidi, disponibili (g)", "Zuccheri (g)", "Fibra alimentare (g)", "Proteine (g)", "Sale (NaCl) (g)" };
                        string[] tempdata = new string[15];
                        dataadptr.Fill(dataset, "ValoriNutritivi");
                        tempdata[0] = excelfile.Columns[129].ColumnName.ToString().Remove(0, 67);
                        cleaner = tempdata[0].Remove(10, 1);
                        tempdata[0] = cleaner;
                        //excelfile.Rows.RemoveAt(0);
                        int countersetter = 0;

                        for (int i = 0; i < excelfile.Rows.Count; i++)
                        {
                            int k = 1;
                            for (int j = 0; j < excelfile.Columns.Count; j++)
                            {
                                try
                                {
                                    if (String.Equals(excelfile.Columns[j].ColumnName, ColumnsToBeSaved[countersetter]) is true) //excelfile.Rows[0][j].ToString()
                                {
                                        tempdata[k] = excelfile.Rows[i][j].ToString();
                                        k++;
                                        countersetter++;
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                break;
                                }

                            }
                            cleanexcelfile.Rows.Add(i, tempdata[1], tempdata[2], tempdata[3], tempdata[4], tempdata[5], tempdata[6], tempdata[7], tempdata[8], tempdata[9], tempdata[10], tempdata[11], tempdata[12], tempdata[13], tempdata[0]);
                            countersetter = 0;
                        }

                    dataadptr.Update(dataset, "ValoriNutritivi");
                    }
                }
                    
                    
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Diet Creator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
            myConn.Close();
        }
    }
    internal class checkclockDB
    {
        public string clocktaker()
        {
            SqlConnection myConn = new SqlConnection("Server =localhost;Integrated security=SSPI;database=DBVN");
            string strclockcheck = "SELECT Data FROM ValoriNutritivi WHERE ID = 1";
            string clock = "";
            SqlCommand clockCommand = new SqlCommand(strclockcheck, myConn);
            try
            {
                myConn.Open();
                using (SqlDataReader reader = clockCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clock = reader[0].ToString();
                    }
                }
                 
            }
            catch (System.Exception ex)
            {
                
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return clock;
        }
    }
    internal class Showfoodcombobox
    {
        public string[] foodviewer()
        {
            SqlConnection myConn = new SqlConnection("Server =localhost;Integrated security=SSPI;database=DBVN");
            string strfoodlist = "SELECT Nome FROM ValoriNutritivi";
            string foodcounter = "SELECT COUNT(Nome) FROM ValoriNutritivi"; 
            int count = 0;
            SqlCommand foodCount = new SqlCommand(foodcounter, myConn);
            myConn.Open();
            int size = (Int32)foodCount.ExecuteScalar();
            myConn.Close();
            SqlCommand foodCommand = new SqlCommand(strfoodlist, myConn);
            string[] foodviewer = new string[size];
            try
            {
                myConn.Open();
                
                using (SqlDataReader reader = foodCommand.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                       foodviewer[count] = reader[0].ToString();  
                       count++;    
                    }
                    reader.Close();
                }

            }
            catch (System.Exception ex)
            {

            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return foodviewer;
        }
    }

    internal class catchparameterscombobox
    {
        public string[] parameterscombobox(string combo)
        {
            SqlConnection myConn = new SqlConnection("Server =localhost;Integrated security=SSPI;database=DBVN");
            string strparameterfinder = "SELECT Energia_calorie_kcal, Lipidi_totali_gr, Glucidi_disponibili_gr, Fibra_alimentare_gr, Proteine_gr FROM ValoriNutritivi WHERE Nome = @combo";
            SqlCommand queryparameters = new SqlCommand(strparameterfinder, myConn);
            queryparameters.Parameters.AddWithValue("@combo",combo);
            string[] queryresult = new string[5];
            try
            {
                myConn.Open();

                using (SqlDataReader reader = queryparameters.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        queryresult[0] = reader.GetString(reader.GetOrdinal("Energia_calorie_kcal"));
                        queryresult[1] = reader.GetString(reader.GetOrdinal("Lipidi_totali_gr"));
                        queryresult[2] = reader.GetString(reader.GetOrdinal("Glucidi_disponibili_gr"));
                        queryresult[3] = reader.GetString(reader.GetOrdinal("Fibra_alimentare_gr"));
                        queryresult[4] = reader.GetString(reader.GetOrdinal("Proteine_gr"));

                    }
                    reader.Close();
                }

            }
            catch (System.Exception ex)
            {

            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }

            return queryresult;
        }
    }
    internal class proportionalfooddata
    {
        public string[] setwithrightproportion(string[] values, string weight)
        {
            double mathcalc = 0;
            int i = 0;
            string[] resultproportion = new string[5];
            char[] charsToTrim = { '<', '>', 't','r','.' };
            foreach (string value in values)
            {
                string cleanvalue;
                cleanvalue = value.Trim(charsToTrim);
                if (cleanvalue.Length == 0) { cleanvalue = "0"; };
                mathcalc = (Convert.ToDouble(cleanvalue) * Convert.ToDouble(weight)) / 100;
                resultproportion[i] = Math.Round(mathcalc,2).ToString();
                i++;
            }
            return resultproportion;
        }
    }
    internal class deletefoodfromlist
    {
        public string[,] deletefoodlist(string[,] list)
        {
            string[,] listpurify = new string[90, 7];

            return listpurify;
        }
    }
     
}
