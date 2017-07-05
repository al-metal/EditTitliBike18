using Bike18;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditTitliBike18
{
    public partial class Form1 : Form
    {
        string fileUrlsAllProducts;

        nethouse nethouse = new nethouse();


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
            Properties.Settings.Default.login = tbLogin.Text;
            Properties.Settings.Default.password = tbPasswords.Text;
            Properties.Settings.Default.Save();

            if (ofdOpenFile.FileName == "openFileDialog1" || ofdOpenFile.FileName == "")
            {
                MessageBox.Show("Ошибка при выборе файла", "Ошибка файла");
                return;
            }
            fileUrlsAllProducts = ofdOpenFile.FileName.ToString();

            CookieContainer cookie = nethouse.CookieNethouse(tbLogin.Text, tbPasswords.Text);
            if (cookie.Count == 1)
            {
                MessageBox.Show("Логин или пароль для сайта введены не верно", "Ошибка логина/пароля");
                return;
            }

            FileInfo file = new FileInfo(fileUrlsAllProducts);
            ExcelPackage p = new ExcelPackage(file);
            ExcelWorksheet w = p.Workbook.Worksheets[1];
            int q = w.Dimension.Rows;
            int edits = 0;

            for (int i = 2; q > i; i++)
            {
                string url = (string)w.Cells[i, 2].Value;
                string title = (string)w.Cells[i, 11].Value;

                if (url.Contains("category"))
                    continue;

                if (title.Contains("Купить ТОВАР"))
                {
                    #region BIKE18

                    List<string> tovarB18 = nethouse.GetProductList(cookie, url);
                    if (tovarB18 == null)
                        continue;

                    string titleSEO = tovarB18[13];
                    string nameTovar = tovarB18[4];
                    titleSEO = titleSEO.Replace("ТОВАР", nameTovar);
                    tovarB18[13] = titleSEO;
                    nethouse.SaveTovar(cookie, tovarB18);
                    edits++;

                    #endregion
                }
                else if(title.Contains("MP_"))
                {
                    List<string> tovarB18 = nethouse.GetProductList(cookie, url);
                    if (tovarB18 == null)
                        continue;
                }
                else
                {
                    //List<string> tovarB18 = nethouse.GetProductList(cookie, url);
                    //if (tovarB18 == null)
                    //    continue;

                    //string article = tovarB18[6];
                    //#region Racer
                    //if (article.Contains("R00"))
                    //{
                    //    edits++;
                    //    string titleSEO = tovarB18[13];

                    //    if (titleSEO.Contains(tovarB18[45]))
                    //        continue;

                    //    titleSEO = titleSEO + " Bike18 " + tovarB18[45];
                    //    titleSEO = titleSEO.Replace(".", "");
                    //    if (titleSEO.Length > 255)
                    //    {

                    //        titleSEO = titleSEO.Remove(255);
                    //        titleSEO = titleSEO.Remove(titleSEO.LastIndexOf(" "));
                    //    }
                    //    tovarB18[13] = titleSEO;
                    //    nethouse.SaveTovar(cookie, tovarB18);

                    //}
                    //#endregion
                }
            }
            MessageBox.Show("Удалено товаров " + edits);


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbLogin.Text = Properties.Settings.Default.login;
            tbPasswords.Text = Properties.Settings.Default.password;
        }
    }
}
