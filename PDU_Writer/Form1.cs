using System;
using System.ComponentModel;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace PDU_Writer
{

    public partial class Form1 : Form
    {
        private string excel_file;
        private string[] pdus;

        public Form1()
        {
            InitializeComponent();
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button2.Click += new System.EventHandler(this.button2_Click);

        }


        private void button1_Click(object sender, EventArgs e)
        {
            pdus = textBox1.Text.Split(',');
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Select Excel File";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                excel_file = open.FileName;
                button1.Text = excel_file;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var excel =new Excel.Application();
            excel.Visible = true;
            excel.Workbooks.Open(Filename:excel_file, ReadOnly:false);
            Excel.Workbook workbook = excel.ActiveWorkbook;
            Excel.Worksheet sheet = workbook.ActiveSheet;
            //Finding current row to add onto for IT,PDU,OUTlet,Rack
            Excel.Range last = sheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            int last_row = last.Row-1;
            //last row for pdus
            Excel.Range line = sheet.Rows[last_row];
            line.Insert();
            int last_rack = Int32.Parse(sheet.Cells[5][last_row-1].Value.Split('-')[2].Trim());
            int last_room = last_rack + 15;
            int last_it_position = 0;
            int last_it = 0;
            int last_outlet = 0;
            int last_outlet_position = 0;
            for(int i = 44016; i < last_row; i++)
            {              
                if (sheet.Cells[1][i].Value2.Equals("OUTLET")) 
                {
                    last_it_position = i - 2;
                    last_it = Int32.Parse(sheet.Cells[2][i-3].Value.Split('-')[2].Trim());
                    break;
                }
                
            }
            for(int i = last_row-1; i > 0; i--)
            {
                if (sheet.Cells[1][i].Value2.Equals("OUTLET"))
                {
                    last_outlet_position = i + 1;
                    break;
                }
            }
            //i=pdu index in pdus
            for(int i = 0; i < pdus.Length; i++)
            {
                string pdu = pdus[i].Trim();
                string [] pdu_split = pdu.Split('.');
                int q = Int32.Parse(pdu_split[3]);
                //j=pdu suffix
                for(int j = q; j <= 254; j++)
                {
                    if (j % 2 == 0)
                    {
                        last_rack++;
                        sheet.Rows[last_room].Insert();
                        last_row++;
                        last_it++;
                        last_outlet++;
                        Excel.Range cell = sheet.Cells;
                        cell[1][last_room].Value = "Rack";
                        cell[2][last_room].Value = "Rack -- " + last_rack;
                        cell[3][last_room].Value = "R" + last_rack;
                        cell[4][last_room].Value = "ROOM";
                        cell[5][last_room].Value = "Room -- 1";
                        cell[6][last_room].Value = "Room -- 1";
                        cell[7][last_room].Value = "2";
                        cell[8][last_room].Value = "75";
                        cell[9][last_room].Value = "65";
                    }
                    pdu_split[3] = j.ToString();
                    Excel.Range cells = sheet.Cells;
                    //create 27 IT devices for each PDU
                    for(int a = 0; a < 27; a++)
                    {
                        last_it++;
                        for(int b = 0; b < 2; b++)
                        {
                            sheet.Rows[last_outlet_position].Insert();
                            last_row++;
                            cells[1][last_outlet_position].Value = "OUTLET";
                            cells[2][last_outlet_position].Value = String.Join(".", pdu_split);
                            cells[4][last_outlet_position].Value = (a * 2) + b + 1;
                            cells[5][last_outlet_position].Value = "DEVICE";
                            cells[6][last_outlet_position].Value = "IT Device -- " + last_it;
                        }
                        sheet.Rows[last_it_position].Insert();
                        last_row++;
                        last_outlet_position++;
                        cells[1][last_it_position].Value = "DEVICE";
                        cells[2][last_it_position].Value = "IT Device -- " + last_it;
                        cells[3][last_it_position].Value = "IT Device -- " + last_it;
                        cells[4][last_it_position].Value = "RACK";
                        cells[5][last_it_position].Value = "Rack -- " + last_rack;
                        cells[6][last_it_position].Value = "Unknown";
                        cells[7][last_it_position].Value = "Default Generated Device";
                        cells[9][last_it_position].Value = "FALSE";
                    }
                    cells[1][last_row].Value="PDU";
                    cells[2][last_row].Value = String.Join(".", pdu_split);
                    cells[4][last_row].Value = "RACK";
                    cells[5][last_row].Value = "Rack -- " + last_rack;
                    last_row++;
                    sheet.Rows[last_row].Insert();
                    
                }
            }
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void helloWorldLabel_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            MessageBox.Show("file");
        }
    }
}
