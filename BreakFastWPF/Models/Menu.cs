using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BreakFastWPF.Models
{


    public class Menu
    {
        public string MenuType { get; set; }
        public string Style { get; set; }
        public string MenuId { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        private string imgdir = "";
        private string newpath = "";
        public string ImageUri
        {
            get
            {
                    return @newpath;
            }
            set
            {
                newpath = imgdir + value;
            }
        }
        public string Desp { get; set; }
    }

    public class ObsMenu : ObservableCollection<Menu>
    {
        List<Menu> menulist;
        public ObsMenu()
        {
            LoadMenu();
            foreach(var item in menulist)
            {
                Console.WriteLine(item.MenuId);
                Add(new Menu() { MenuType=item.MenuType, Style=item.Style, MenuId = item.MenuId, Title = item.Title,
                    Price =item.Price, Discount=item.Discount, ImageUri =item.ImageUri, Desp=item.Desp });
            }
        }
        public void MyCommand()
        {
            Console.WriteLine("Here!!!");
        }

        public void LoadMenu()
        {
            using (StreamReader r = File.OpenText("menudata.json"))
            {
                string json = r.ReadToEnd();
                menulist = JsonConvert.DeserializeObject<List<Menu>>(json);
            }
        }

    }

}
