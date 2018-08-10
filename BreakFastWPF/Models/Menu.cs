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


    class Menu
    {
        public string MenuId { get; set; }
        public string MenuType { get; set; }    //Solid or liquid?
        public string Style { get; set; }       //Chinese, Japaness, or America style?
        public string Title { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public string ImageUri{ get; set; }
        public string Desp { get; set; }
    }

    class ObsMenu : ObservableCollection<Menu>
    {
        List<Menu> menulist;
        public ObsMenu()
        {
            string imgPath = "pack://siteoforigin:,,,./images/";

            LoadMenu();
            foreach(var item in menulist)
            {
                Console.WriteLine(item.MenuId);
                Add(new Menu() { MenuType=item.MenuType, Style=item.Style, MenuId = item.MenuId, Title = item.Title,
                    Price =item.Price, Discount=item.Discount, ImageUri = imgPath + item.ImageUri, Desp=item.Desp });
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
