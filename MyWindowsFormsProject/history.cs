using MongoDB.Driver;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWindowsFormsProject
{
    public partial class history : Form
    {
        
        ChromeDriver _driver = null;
        List<Products> _products = null;
        IMongoDatabase _database = null;

        public history(ChromeDriver driver, IMongoDatabase database)
        {
            InitializeComponent();
            _driver = driver;
            _database = database;
            IMongoCollection<Products> collection = _database.GetCollection<Products>("Products");
            _products = collection.AsQueryable().ToList<Products>();


            if (_products.Count != 0)
            {
                int count = 0;
                foreach (Products product in _products)
                {
                    PictureBox p1 = new PictureBox();
                    p1.Left = 20;
                    p1.Top = 10 + count * 160;
                    p1.Width = 150;
                    p1.Height = 150;
                    p1.SizeMode = PictureBoxSizeMode.StretchImage;

                    using (MemoryStream ms = new MemoryStream(product.picture))
                    {
                        Image img = Image.FromStream(ms);

                        // PictureBox에 이미지 출력
                        p1.Image = img;
                    }

                    System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                    label.Location = new Point(250, 30 + (count * 160));
                    label.Font = new Font(label.Font.Name, 10);
                    label.AutoSize = true;
                    label.Size = new System.Drawing.Size(200, 100);
                    label.Text = product.name;

                    Button button1 = new Button();
                    button1.Location = new Point(350, 90 + (count * 160));
                    button1.Width = 90;
                    button1.Height = 40;
                    button1.Text = "찜 목록 넣기";
                    button1.Tag = count.ToString();
                    button1.Click += Button1_Click;

                    Button button2 = new Button();
                    button2.Location = new Point(250, 90 + (count * 160));
                    button2.Width = 70;
                    button2.Height = 40;
                    button2.Text = "상품 보기";
                    button2.Tag = product.url;
                    button2.Click += Button2_Click;

                    this.Controls.Add(p1);
                    this.Controls.Add(label);
                    this.Controls.Add(button1);
                    this.Controls.Add(button2);

                    count++;
                }
            }
            else
            {
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                // Tag 속성을 통해 라벨에 연결된 보이지 않는 데이터 가져오기
                string additionalData = clickedButton.Tag as string;

                _driver.Navigate().GoToUrl(additionalData);

                new itemDetail(_driver, _database, additionalData).Show();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            IMongoCollection<Products> collection = _database.GetCollection<Products>("Wishs");
           
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                // Tag 속성을 통해 라벨에 연결된 보이지 않는 데이터 가져오기
                string string_num = clickedButton.Tag as string;
                int num = int.Parse(string_num);

                Products product = _products[num];

                try
                {
                    var filter = Builders<Products>.Filter.Eq("url", product.url);

                    Products ppp = collection.Find(filter).First();
                }
                catch
                {
                    collection.InsertOne(product);
                }
            }
        }
    }
}
