using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Text;


namespace PhonNumberMatchingApp.Models
{
    public class Utility
    {

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            StringBuilder SB = new StringBuilder();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');

                    if (rows.Length >= 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                            SB.Append(dr[i].ToString().Substring(0, 4) + ",");
                        }

                        dt.Rows.Add(dr);
                    }
                }

            }


            return dt;
        }


        public List<PhoneData> FetchPhoneNumberData(string strFilePath)
        {
            List<PhoneCode> arr = new List<PhoneCode>();
  StringBuilder cods = new StringBuilder();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
              
                while (!sr.EndOfStream)
                {
                    string rows = sr.ReadLine();
                    cods.Append(rows.Substring(0, 4) + ",");
                    arr.Add(new PhoneCode {PhoneNumber= rows, Code=rows.Substring(0,4)});
                    //arr

                }
            }
            var phoneData = fetchPhoneNumberData(arr);

            return phoneData;
        }






        private List<PhoneData> fetchPhoneNumberData(List<PhoneCode> phNumber)
        {
            List<PhoneData> phoneData = new List<PhoneData>();
            using (MachingAppEntities DB = new MachingAppEntities())
            {
                // string ph = phNumber.Substring(0, 4);
                phoneData = (from lc in DB.LeadCollection
                             join no in DB.NetworkOperator on lc.OperatorCode equals no.Code
                             join c in DB.Circle on lc.CircleCode equals c.Code
                             //where phNumber.Contains(lc.PhoneNumber)
                             select new PhoneData
                             {
                                 PhoneNumber = lc.PhoneNumber,
                                 NetworkName = no.NetworkName,
                                 CircleName = c.CircleName,
                                 City = c.City
                             }).ToList();

                phoneData.ForEach(
                    z =>
                    {
                        phNumber.ForEach(c =>
                        {
                            if (z.PhoneNumber == c.Code)
                            {
                                z.PhoneNumber = c.PhoneNumber;
                            }
                        });
                    });

            }

            return phoneData;

        }


        public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {

                oledbConn.Open();
                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);

                    dt = ds.Tables[0];
                }
            }
            catch
            {
            }
            finally
            {

                oledbConn.Close();
            }

            return dt;

        }
    }
}