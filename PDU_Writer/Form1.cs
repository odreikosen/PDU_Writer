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
            button3.Click += new EventHandler(button3_Click);
            linkLabel1.Click += new EventHandler(linkLabel1Clicked);

        }
        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            pdus = textBox1.Text.Split(',');
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Select Excel File";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                excel_file = open.FileName;
                button3.Text = excel_file;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var excel =new Excel.Application();
            excel.Visible = true;
            Excel.Workbook workbook;
            Excel.Worksheet sheet;
            //The current amount of entities in PIQ
            int currentpdu;
            int currentoutlet;
            int currentit;
            int currentrack;
            //The current position of entity in spreadsheet
            int linepdu;
            int lineoutlet;
            int lineit;
            int linerack;
            if (!String.IsNullOrEmpty(excel_file))
            {
                excel.Workbooks.Open(Filename: excel_file, ReadOnly: false);
                 workbook = excel.ActiveWorkbook;
                 sheet = workbook.ActiveSheet;
                //Finding last row to determine current PDU
                Excel.Range last = sheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                double last_row = last.Row - 1;
                currentpdu = (int) Math.Floor(last_row / 82.5);
                currentoutlet = currentpdu * 54;
                currentit = currentpdu * 27;
                currentrack = currentpdu / 2;
                //find current lines of pdus
                linepdu = (int)last_row;
                lineoutlet = linepdu - currentpdu - 4;
                lineit = lineoutlet - currentoutlet - 2;
                linerack = lineit -currentit - 2;
            } else
            {
                workbook=excel.Workbooks.Add();
                sheet = workbook.ActiveSheet;
                //Find amount of pdus based off of relationships
                currentpdu = (int) numericUpDown2.Value;
                currentit = currentpdu * ((int)numericUpDown4.Value);
                currentoutlet = currentit * ((int)numericUpDown5.Value);
                currentrack = currentpdu / ((int)numericUpDown3.Value);
                //Set positions slightly apart to allow for copy and paste
                linerack = 1;
                lineit = 3;
                lineoutlet = 5;
                linepdu = 7;
            }                                
            //last row for pdus and insert empty line
            Excel.Range line = sheet.Rows[linepdu];
            line.Insert();
          
            //i=pdu index in pdus
            for(int i = 0; i < pdus.Length; i++)
            {
                string pdu = pdus[i].Trim();
                string [] pdu_split = pdu.Split('.');
                int q = Int32.Parse(pdu_split[3]);
                //j=pdu suffix
                for(int j = q; j <=(int)numericUpDown1.Value; j++)
                {
                    //add a new room for every 2 pdus and increment outlet,it,and pdu positions
                    if (j % (int)numericUpDown3.Value == 0)
                    {
                        currentrack++;
                        sheet.Rows[linerack].Insert();
                        linepdu++;
                        lineit++;
                        lineoutlet++;
                        Excel.Range cell = sheet.Cells;
                        cell[1][linerack].Value = "Rack";
                        cell[2][linerack].Value = "Rack -- " + currentrack;
                        cell[3][linerack].Value = "R" + currentrack;
                        cell[4][linerack].Value = "ROOM";
                        cell[5][linerack].Value = "Room -- 1";
                        cell[6][linerack].Value = "Room -- 1";
                        cell[7][linerack].Value = "2";
                        cell[8][linerack].Value = "75";
                        cell[9][linerack].Value = "65";
                        linerack++;
                    }
                    pdu_split[3] = j.ToString();
                    Excel.Range cells = sheet.Cells;
                    //create 27 IT devices for each PDU
                    for(int a = 0; a < (int) numericUpDown4.Value; a++)
                    {
                        currentit++;
                        //create two outlets for each IT devices
                        for(int b = 0; b < (int) numericUpDown5.Value; b++)
                        {
                            sheet.Rows[lineoutlet].Insert();
                            linepdu++;
                            cells[1][lineoutlet].Value = "OUTLET";
                            cells[2][lineoutlet].Value = String.Join(".", pdu_split);
                            cells[4][lineoutlet].Value = (a * 2) + b + 1;
                            cells[5][lineoutlet].Value = "DEVICE";
                            cells[6][lineoutlet].Value = "IT Device -- " + currentit;
                            lineoutlet++;
                        }
                        //create it device
                        sheet.Rows[lineit].Insert();
                        linepdu++;
                        lineoutlet++;
                        cells[1][lineit].Value = "DEVICE";
                        cells[2][lineit].Value = "IT Device -- " + currentit;
                        cells[3][lineit].Value = "IT Device -- " + currentit;
                        cells[4][lineit].Value = "RACK";
                        cells[5][lineit].Value = "Rack -- " + currentrack;
                        cells[6][lineit].Value = "Unknown";
                        cells[7][lineit].Value = "Default Generated Device";
                        cells[9][lineit].Value = "FALSE";
                        lineit++;
                    }
                    //create PDU
                    sheet.Rows[linepdu].Insert();
                    cells[1][linepdu].Value="PDU";
                    cells[2][linepdu].Value = String.Join(".", pdu_split);
                    cells[4][linepdu].Value = "RACK";
                    cells[5][linepdu].Value = "Rack -- " + currentrack;
                    linepdu++;
                    
                    
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

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
        private void linkLabel1Clicked(object sender,EventArgs e)
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://github.com/odreikosen/PDU_Writer");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void debugInstructionsLabel_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}
