using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using MongoDB.Driver;

namespace MyWindowsFormsProject
{
    public partial class saleForm : Form
    {
        protected ChromeDriver _driver = null;
        List<Products> _products = new List<Products>();
        IMongoDatabase _database = null;

        public saleForm(IWebElement allProduct, ChromeDriver driver, IMongoDatabase database)
        {
            InitializeComponent();

            _database = database;
            _driver = driver;

            IList<IWebElement> liElements = allProduct.FindElements(By.CssSelector("li"));
            
            int count = 0;
            foreach (IWebElement liElement in liElements)
            {
                PictureBox p1 = new PictureBox();
                p1.Left = 10 + (count % 4 * 160);
                p1.Top = 10 + (count / 4 * 200);
                p1.Width = 150;
                p1.Height = 150;
                p1.SizeMode = PictureBoxSizeMode.StretchImage;

                IWebElement urlElement = liElement.FindElement(By.TagName("a"));
                string url = urlElement.GetAttribute("href");


                IWebElement imageElement = liElement.FindElement(By.TagName("img"));
                string imageUrl = imageElement.GetAttribute("src");
               
                IWebElement textElement = liElement.FindElement(By.ClassName("prod-list__txt"));
                string imageName = textElement.GetAttribute("innerHTML");

                IWebElement priceElement = liElement.FindElement(By.ClassName("prod-list__price"));

                IWebElement numElement = priceElement.FindElement(By.ClassName("num"));
                string price = numElement.GetAttribute("innerHTML");
                string rate = "";
                if (priceElement.FindElements(By.TagName("span")).Count >= 2)
                {
                    IWebElement discountElement = priceElement.FindElement(By.ClassName("discount"));
                    IWebElement rateElement = discountElement.FindElement(By.ClassName("rate"));
                    rate = rateElement.GetAttribute("innerHTML");
                }
                else
                {
                    rate = "0";
                }

                // 이미지 다운로드 및 출력 코드
                WebClient webClient = new WebClient();
                byte[] data = webClient.DownloadData(imageUrl);
                webClient.Dispose();

                using (MemoryStream ms = new MemoryStream(data))
                {
                    Image img = Image.FromStream(ms);

                    // PictureBox에 이미지 출력
                    p1.Image = img;
                }
                    
                System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                label.Location = new Point(30 + (count % 4 * 160), 170 + (count / 4 * 200));
                label.Tag = count.ToString();
                label.AutoSize = true; // 자동 크기 조정 비활성화
                label.Size = new System.Drawing.Size(200, 100);
                string name = imageName.Replace("<br>", "\n").Trim();
                label.Text = name + "\n" + rate + "% " + price + "원";
                label.Click += Label_click;

                this.Controls.Add(p1);
                this.Controls.Add(label);

                Products product = new Products {
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
                string a =  clickedLabel.Tag as string;
                int num = int.Parse(a);
                string additionalData = _products[num].url;

                _driver.Navigate().GoToUrl(additionalData);


                Search1Form.inputList(_products[num]);

                new itemDetail(_driver, _database, additionalData).Show();
            }
        }
    }
}
