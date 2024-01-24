using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;
using System.IO;
using System.Net;
using MongoDB.Driver;

namespace MyWindowsFormsProject
{
    public partial class Search2Form : Form
    {
        ChromeDriver _driver = null;
        List<Products> _products = new List<Products> ();
        IMongoDatabase _database = null;

        public Search2Form(ChromeDriver driver, IMongoDatabase database)
        {
            InitializeComponent();
            _driver = driver;
            _database = database;

            /*string str = _driver.FindElement(By.XPath("//*[@id='productListArea']/div[1]/div[1]/ul/li[1]/a/span")).GetAttribute("innerHTML");
            string str1 = str.Replace(",", "");
            string str2 = str1.Substring( 1, str1.Length - 2);
            int num = int.Parse(str2);*/

            IWebElement table = null;
            List<IWebElement> webElements = null;
            int type = 0;

            try
            {
                table = _driver.FindElement(By.XPath("//*[@id='productListArea']/div[3]/ul"));
                webElements = table.FindElements(By.ClassName("prod_item")).ToList();
            }
            catch
            {
                type = 1;
                table = _driver.FindElement(By.XPath("//*[@id='productListArea']/div[5]/ul"));
                webElements = table.FindElements(By.ClassName("prod_main_info")).ToList();
                webElements.RemoveAt(webElements.Count - 1);
            }

            MessageBox.Show(webElements.Count.ToString());

            int count = 0;
            foreach (IWebElement webElement in webElements)
            {
                if (type == 0)
                {
                    if (webElement.GetAttribute("id").Substring(0, 2).Equals("ad")) { continue; }
                }
                else if(type == 1)
                {
                }
                

                IWebElement nameTag = webElement.FindElement(By.ClassName("prod_name")).FindElement(By.TagName("a"));
                string url = nameTag.GetAttribute("href");
                string name  = nameTag.GetAttribute("innerHTML");

                System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                label.Location = new Point(200, 30 + (count * 160));
                label.Tag = count.ToString();
                label.AutoSize = true; // 자동 크기 조정 비활성화
                label.Size = new System.Drawing.Size(200, 100);
                label.Text = name;
                label.Click += Label_click;

                IWebElement imgTag = webElement.FindElement(By.ClassName("thumb_image")).FindElement(By.TagName("img"));
                string imgUrl = imgTag.GetAttribute("src");

                PictureBox p1 = new PictureBox();
                p1.Left = 10;
                p1.Top = 10 + (count * 160);
                p1.Width = 150;
                p1.Height = 150;
                p1.SizeMode = PictureBoxSizeMode.StretchImage;

                WebClient webClient = new WebClient();
                byte[] data = webClient.DownloadData(imgUrl);
                webClient.Dispose();

                using (MemoryStream ms = new MemoryStream(data))
                {
                    Image img = Image.FromStream(ms);

                    // PictureBox에 이미지 출력
                    p1.Image = img;
                }

                this.Controls.Add(label);
                this.Controls.Add(p1);
                // 지연된 로드로 인해 이미지가 전부 불러와지지 않는다.

                Products product = new Products() {
                    name = name,
                    picture = data,
                    url = url,
                    time = DateTime.Now
                };
                
                _products.Add(product);

                count++;
            }
        }
        private void Label_click(object sender, EventArgs e)
        {
            // 클릭된 라벨 객체 가져오기
            System.Windows.Forms.Label clickedLabel = sender as System.Windows.Forms.Label;

            // 라벨이 null이 아닌 경우에만 Tag 속성에서 데이터 가져오기
            if (clickedLabel != null)
            {
                // Tag 속성을 통해 라벨에 연결된 보이지 않는 데이터 가져오기
                string a = clickedLabel.Tag as string;
                int num = int.Parse(a);
                string additionalData = _products[num].url;

                _driver.Navigate().GoToUrl(additionalData);


                Search1Form.inputList(_products[num]);

                new itemDetail(_driver, _database, additionalData).Show();
            }
        }
    }
}
