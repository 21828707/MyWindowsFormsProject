using MongoDB.Driver;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MyWindowsFormsProject
{
    public partial class itemDetail : Form
    {
        protected ChromeDriver _driver = null;
        IMongoDatabase _database = null;
        IMongoCollection<Products> collection = null;
        string _url = null;
        byte[] _data = null;
        string _name = null;

        PictureBox p2 = null;
        Button buttonTrue = null;
        Button buttonFalse = null;

        public itemDetail(ChromeDriver driver, IMongoDatabase database, string url)
        {
            InitializeComponent();

            _driver = driver;
            _database = database;
            collection = _database.GetCollection<Products>("Wishs");
            _url = url;

            PictureBox p1 = new PictureBox();
            p1.Left = 30;
            p1.Top = 10;
            p1.Width = 300;
            p1.Height = 300;
            p1.SizeMode = PictureBoxSizeMode.StretchImage;

            p2 = new PictureBox();
            p2.Left = 35;
            p2.Top = 320;
            p2.Width = 60;
            p2.Height = 55;
            p2.SizeMode = PictureBoxSizeMode.StretchImage;
            p2.Paint += PictureBox1_Paint;

            IWebElement imageElement = driver.FindElement(By.XPath("//*[@id='baseImage']"));
            string imageUrl = imageElement.GetAttribute("src");

            WebClient webClient = new WebClient();
            byte[] data = webClient.DownloadData(imageUrl);
            _data = data;
            webClient.Dispose();
            
            p1.Image = getImage(data);
            
            data = File.ReadAllBytes(
                "C:\\Users\\test\\Downloads\\MyWindowsFormsProject\\MyWindowsFormsProject\\img\\star-removebg.png");
            p2.Image = getImage(data);


            buttonTrue = new Button();
            buttonTrue.Location = new Point(100, 330);
            buttonTrue.Width = 90;
            buttonTrue.Height = 40;
            buttonTrue.Text = "찜 목록 삭제";
            buttonTrue.Tag = "";
            buttonTrue.Click += ButtonTrue_Click;

            buttonFalse = new Button();
            buttonFalse.Location = new Point(100, 330);
            buttonFalse.Width = 90;
            buttonFalse.Height = 40;
            buttonFalse.Text = "찜 목록 넣기";
            buttonFalse.Tag = "";
            buttonFalse.Click += ButtonFalse_Click;

            //---------------------
            _name = driver.FindElement(By.XPath("//*[@id='blog_content']/div[2]/div[1]/h3/span")).GetAttribute("innerHTML");

            bool exist = checkData();
            LookStar(exist);

            //--------------------- 찜 버튼 누루면 생길 수 있도록 모듈화 ㄱㄱ
            string mini = "";
            for(int i = 5; i <= 8; i++)
            {
                IWebElement font = null;
                try
                {
                    font = driver.FindElement(By.XPath("//*[@id='graphAreaSmall']/div[" + i + "]"));
                }
                catch
                {
                    break;
                }
               
                string text = font.GetAttribute("innerHTML");
                if (!text.Equals(""))
                {
                    mini = text;
                }
            }

            System.Windows.Forms.Label l1 = new System.Windows.Forms.Label();
            l1.Location = new Point(200, 340);
            l1.AutoSize = true;
            if (mini.Equals(""))
            {
                l1.Text = "가격 추이가 없습니다.";
            }
            else
            {
                l1.Text = "1개월 최저가 추이\n" + mini + "원";
            }

            this.Controls.Add(p1);
            this.Controls.Add(l1);

            IWebElement tableElement = driver.FindElement(By.ClassName("high_list"));
            IList<IWebElement> trElements = tableElement.FindElements(By.CssSelector("tr"));
            int count = 0;
            foreach (IWebElement trElement in trElements)
            {
                if (!trElement.GetAttribute("class").Equals("product-pot"))
                {
                    string logo = "";
                    string money = "";
                    string delevery = "";

                    LinkLabel linkLabel = new LinkLabel();

                    IWebElement mall = trElement.FindElement(By.ClassName("mall"));
                    IWebElement over = mall.FindElement(By.ClassName("logo_over"));
                    IWebElement a = over.FindElement(By.CssSelector("a"));
                    
                    if (!a.GetAttribute("title").Equals(""))
                    {
                        System.Windows.Forms.Label l = new System.Windows.Forms.Label();
                        l.Location = new Point(350, 12 + 32 * count);
                        l.AutoSize = true;

                        logo = a.GetAttribute("title");

                        l.Text = logo;
                        this.Controls.Add(l);
                    }
                    else
                    {
                        PictureBox pp1 = new PictureBox();
                        pp1.Left = 350;
                        pp1.Top = 10 + 32 * count;
                        pp1.Width = 54;
                        pp1.Height = 22;
                        pp1.SizeMode = PictureBoxSizeMode.StretchImage;

                        IWebElement i = a.FindElement(By.CssSelector("img")); // 의미 없는 행이 숨겨져 있다. => productPot
                        logo = i.GetAttribute("src");

                        webClient = new WebClient();
                        data = webClient.DownloadData(logo);
                        webClient.Dispose();

                        pp1.Image = getImage(data);

                        this.Controls.Add(pp1);
                    }
                    

                    linkLabel.Text = "이동하기";
                    linkLabel.Links.Add(0, linkLabel.Text.Length, a.GetAttribute("href")); // 링크 추가
                    linkLabel.Left = 650;
                    linkLabel.Top = 15 + 32 * count;
                    linkLabel.LinkClicked += (sender1, e1) =>
                    {
                        // 링크가 클릭되었을 때의 동작
                        System.Diagnostics.Process.Start(e1.Link.LinkData.ToString());
                    };

                    IWebElement price = trElement.FindElement(By.ClassName("price"));
                    IWebElement em = price.FindElement(By.CssSelector("em"));

                    IWebElement ship = trElement.FindElement(By.ClassName("ship"));
                    IWebElement del = ship.FindElement(By.CssSelector("span"));

                    money = em.GetAttribute("innerHTML");
                    delevery = del.GetAttribute("innerHTML");


                    System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                    label.Location = new Point(450, 15 + 32 * count);
                    label.AutoSize = true;
                    label.Text = money + "원";

                    System.Windows.Forms.Label label1 = new System.Windows.Forms.Label();
                    label1.Location = new Point(530, 15 + 32 * count);
                    label1.AutoSize = true;
                    label1.Text = delevery;

                    this.Controls.Add(linkLabel);
                    this.Controls.Add(label);
                    this.Controls.Add(label1);
                    count++;
                }
                else
                {
                    continue;
                }
            }
        }

        private Image getImage(byte[] b)
        {
            Image image = null;

            using (MemoryStream ms = new MemoryStream(b))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBox1 = sender as PictureBox;

            // PictureBox에 테두리 그리기
            int borderWidth = 1;
            ControlPaint.DrawBorder(e.Graphics, pictureBox1.ClientRectangle,
                Color.Yellow, borderWidth, ButtonBorderStyle.Solid,
                Color.Yellow, borderWidth, ButtonBorderStyle.Solid,
                Color.Yellow, borderWidth, ButtonBorderStyle.Solid,
                Color.Yellow, borderWidth, ButtonBorderStyle.Solid);
        }

        private bool checkData()
        {
            bool exist = true;

            var filter = Builders<Products>.Filter.Eq("url", _url);
            try
            {
                Products product = collection.Find(filter).First();
            }catch
            {
                exist = false;
            }
            return exist;
        }

        private void LookStar(bool e)
        {
            if(e)
            {
                this.Controls.Add(p2);
                try
                {
                    this.Controls.Remove(buttonFalse);
                }
                catch { }
                this.Controls.Add(buttonTrue);
            }
            else
            {
                try
                {
                    this.Controls.Remove(p2);    
                }catch { }
                try
                {
                    this.Controls.Remove(buttonTrue);
                }
                catch { }
                this.Controls.Add(buttonFalse);
            }
        }

        private void ButtonTrue_Click(object sender, EventArgs e)
        {
            var filter = Builders<Products>.Filter.Eq("url", _url);
            collection.DeleteOne(filter);

            bool exist = checkData();
            LookStar(exist);
        }

        private void ButtonFalse_Click(object sender, EventArgs e)
        {
            Products p = new Products {
                name = _name,
                url = _url,
                picture = _data,
                time = DateTime.Now
            };
            collection.InsertOne(p);

            bool exist = checkData();
            LookStar(exist);
        }
    }
}
