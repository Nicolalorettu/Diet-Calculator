using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
//using IronXL;
using NPOI.XSSF.UserModel;
//using NPOI.HSSF.UserModel;
using System.Diagnostics;
using NPOI.SS.UserModel;
using System.Collections;
using System.Globalization;

namespace Diet_Creator
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 intro = new Form1();
            intro.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog(); 
            if (file.ShowDialog() == DialogResult.OK) 
            {
                string fileExt = Path.GetExtension(file.FileName); 
                if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                {
                    try
                    {
                        progressBar1.Visible = true;
                        button1.Enabled = false;
                        button2.Enabled = false;
                        DataTable dtExcel = ReadExcel(file.FileName);
                        foreach (DataRow row in dtExcel.Rows)
                        {
                            progressBar1.PerformStep();
                        }
                        DataTableFeederDB dataTableFeederDB = new DataTableFeederDB();
                        dataTableFeederDB.DataFeeder(dtExcel);
                        label1.Location = new Point(80,125);
                        label1.Text= ("Dati caricati con successo");
                        button2.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Please choose .xls or .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //custom messageBox to show error
                }
            }
            
            
            
        }
        private DataTable ReadExcel(string fileName)
        {
            int duplicatecolumn = 0;
            IWorkbook workbook;
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                workbook= new XSSFWorkbook(stream);
            };
            var sheet = workbook.GetSheetAt(0); // zero-based index of your target sheet
            var dataTable = new DataTable(sheet.SheetName);
            
            // write the header row

            var headerRow = sheet.GetRow(2);
            foreach (var headerCell in headerRow)
            {
                if (headerCell.ToString() == "Deduzione del valore" || headerCell.ToString() == "Fonte")
                {
                    dataTable.Columns.Add(headerCell.ToString()+ duplicatecolumn);
                    duplicatecolumn++;
                }
                else
                {
                    dataTable.Columns.Add(headerCell.ToString());
                }
                
            }
            headerRow = sheet.GetRow(0);
            foreach (var headerCell in headerRow)
            {
                dataTable.Columns.Add(headerCell.ToString());
            }

            // write the rest
            for (int i = 3; i < sheet.PhysicalNumberOfRows; i++)
            {
                var sheetRow = sheet.GetRow(i);
                var dtRow = dataTable.NewRow();
                dtRow.ItemArray = dataTable.Columns
                    
                    .Cast<DataColumn>()
                    .Select(c => sheetRow.GetCell(c.Ordinal, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString())
                    .ToArray();
                dataTable.Rows.Add(dtRow);
            }
            return dataTable;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://valorinutritivi.ch/it/downloads/");
        }
    }
}
