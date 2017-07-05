using OfficeOpenXml;
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

namespace EditTitliBike18
{
    public partial class Form1 : Form
    {
        string fileUrlsAllProducts;


        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            fileUrlsAllProducts = "";
            ofdOpenFile.ShowDialog();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (ofdOpenFile.FileName == "openFileDialog1" || ofdOpenFile.FileName == "")
            {
                MessageBox.Show("Ошибка при выборе файла", "Ошибка файла");
                return;
            }
            fileUrlsAllProducts = ofdOpenFile.FileName.ToString();

            FileInfo file = new FileInfo(fileUrlsAllProducts);
            ExcelPackage p = new ExcelPackage(file);
            ExcelWorksheet w = p.Workbook.Worksheets[1];
            int q = w.Dimension.Rows;

            for (int i = 2; q > i; i++)
            {
                string url = (string)w.Cells[i, 2].Value;
            }


        }
    }
}
