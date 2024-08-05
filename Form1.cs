using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodingTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // loads all the data from the csv into the grid view on init, change file name to use different data set
            LoadData("values.csv");

            // used to test if error log was being properly used
            // ErrorLogger("this is a test");
        }


        private void LoadData(string path)
        {
            try
            {
                var lines = File.ReadAllLines(path);
                var dataTable = new DataTable();

                if (lines.Length > 0)
                {
                    // makes columns and adds them to the data table
                    var head = lines[0].Split(',');
                    foreach (var number in head)
                    {
                        dataTable.Columns.Add("Column" + number);
                    }

                    // populates rows and adds them to the data table
                    foreach (var line in lines)
                    {
                        var data = line.Split(',');
                        dataTable.Rows.Add(data);
                    }
                }
                // adds the data table to the grid view
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                ErrorLogger(ex.Message);
                MessageBox.Show("Error loading data from CSV: check error log");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // checks for and ignores illegal inputs
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }

                // on double click, opens a message box to show the selected cell
                var cellValue = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                string title = $"Cell [{e.ColumnIndex}, {e.RowIndex}]";
                MessageBox.Show(cellValue, title);
            }
            catch (Exception ex)
            {
                ErrorLogger(ex.Message);
                MessageBox.Show($"Error reading data from grid view: check error log");
            }
        }

        // logs all errors and saves them to the desktop in a file names "error_log.txt"
        private void ErrorLogger(string message)
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string logFilePath = Path.Combine(desktopPath, "error_log.txt");

                File.Create(logFilePath).Close();

                StreamWriter sw = new StreamWriter(logFilePath, true);

                sw.WriteLine($"{DateTime.Now}: {message}");
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error logging to file: {ex.Message}");
            }
        }
    }
}
