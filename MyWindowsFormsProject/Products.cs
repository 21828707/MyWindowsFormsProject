using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWindowsFormsProject
{
    class Products
    {
        public ObjectId Id { get; set; }
        public string name { get; set; }
        public byte[] picture { get; set; }
        public string url { get; set; }
        public DateTime time { get; set; }
    }
}
