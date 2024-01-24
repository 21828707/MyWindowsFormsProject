using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace MyWindowsFormsProject
{
    public partial class Search1Form : Form
    {
        protected ChromeDriverService _driverService = null;
        protected ChromeOptions _options = null;
        protected ChromeDriver _driver = null;

        /*static List<Product> _products = new List<Product>();
        static List<string> urls = new List<string>();*/

        MongoClient client = new MongoClient();
        static IMongoDatabase database = null;

        public Search1Form()
        {
            InitializeComponent();

            database = client.GetDatabase("item");

            try
            {
                _driverService = ChromeDriverService.CreateDefaultService();
                _driverService.HideCommandPromptWindow = true;

                _options = new ChromeOptions();
                _options.AddArgument("disable-gpu");
                _options.AddArgument("--headless");

                _driver = new ChromeDriver(_driverService, _options);

                _driver.Navigate().GoToUrl("https://www.danawa.com/");

                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.Message);
            }
        }


        private void btnSearchItem_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Replace(" ", "") != "")
            {
                try
                {
                    _driver.Navigate().GoToUrl("https://www.danawa.com/");

                    IWebElement searchBox = _driver.FindElement(By.XPath("//*[@id='AKCSearch']"));
                    searchBox.SendKeys(textBox1.Text);

                    var element = _driver.FindElement(By.XPath("//*[@id='srchFRM_TOP']/fieldset/div[1]/button"));
                    element.Click();

                    new Search2Form(_driver, database).Show();
                }
                catch (Exception exc)
                {
                    Trace.WriteLine(exc.Message);
                }
            }
            else
            {
                MessageBox.Show("검색할 물품이 없습니다.", "검색 물품 없음 오류");
            }
        }

        private void btnDiscounted_Click(object sender, EventArgs e)
        {
            try
            {
                _driver.Navigate().GoToUrl("https://www.danawa.com/");

                IWebElement allProduct = _driver.FindElement(By.XPath("//*[@id='cmPickLayer']/div[2]/div[2]/div/ul"));

                new saleForm(allProduct, _driver, database).Show();
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.Message);
            }
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            new history(_driver, database).Show();
        }

        public static void inputList(object product)
        {
            Products p = product as Products;
            IMongoCollection<Products> collection = database.GetCollection<Products>("Products");

            try
            {
                var filter = Builders<Products>.Filter.Eq("url", p.url);

                Products ppp = collection.Find(filter).First();
            }
            catch
            {
                collection.InsertOne(p);
            }
            finally
            {
                if (collection.EstimatedDocumentCount() > 10)
                {
                    var sortDefinition = Builders<Products>.Sort.Ascending("time");
                    var filter = Builders<Products>.Filter.Empty; // 필요한 경우 추가적인 필터를 지정할 수 있습니다.

                    // 가장 오래된 문서 삭제
                    var oldestDocument = collection.Find(filter).Sort(sortDefinition).FirstOrDefault();

                    if (oldestDocument != null)
                    {
                        ObjectId objectIdToDelete = oldestDocument.Id;
                        var nameToDelete = oldestDocument.name;
                        var deleteFilter = Builders<Products>.Filter.Eq("Id", objectIdToDelete);

                        collection.DeleteOne(deleteFilter);

                        Console.WriteLine(nameToDelete);
                    }
                    else
                    {
                        Console.WriteLine("No documents to delete.");
                    }
                }
            }
        }

        private void btnWishlist_Click(object sender, EventArgs e)
        {
            new wishList(_driver, database).Show();
        }
    }
}
