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
using Database;

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
        public ObsMenu()
        {
            /* Here has two methods to load menu, JSON file and databse */
            /* Load menu from JSON file */
            //List<Menu> menulist;
            //string imgPath = "pack://siteoforigin:,,,./images/";
            //using (StreamReader r = File.OpenText("menudata.json"))
            //{
            //    string json = r.ReadToEnd();
            //    menulist = JsonConvert.DeserializeObject<List<Menu>>(json);
            //}
            //foreach(var item in menulist)
            //{
            //    Console.WriteLine(item.MenuId);
            //    Add(new Menu() { MenuType=item.MenuType, Style=item.Style, MenuId = item.MenuId, Title = item.Title,
            //        Price =item.Price, Discount=item.Discount, ImageUri = imgPath + item.ImageUri, Desp=item.Desp });
            //}

            /* Load menu from SQLite DB */
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                var query = from it in dbContext.BFMenus
                            orderby it.MenuId
                            select it;
                foreach (BFMenu item in query)
                    //Console.WriteLine("{0} | {1} | {2}", item.MenuId, item.Title, item.Price);
                    Add(new Menu()
                    {
                        MenuType = item.MenuType,
                        Style = item.Style,
                        MenuId = item.MenuId,
                        Title = item.Title,
                        Price = item.Price,
                        Discount = item.Discount,
                        ImageUri = item.ImageUri,
                        Desp = item.Desp
                    });

                //List<BFMenu> mlist = dbContext.BFMenus
                //    .OrderBy(m => m.MenuId)
                //    .ToList();
                //foreach (var item in mlist)
                //{
                //    Add(new Menu()
                //    {
                //        MenuType = item.MenuType,
                //        Style = item.Style,
                //        MenuId = item.MenuId,
                //        Title = item.Title,
                //        Price = item.Price,
                //        Discount = item.Discount,
                //        ImageUri = item.ImageUri,
                //        Desp = item.Desp
                //    });
                //}
            }

        }
        public void MyCommand()
        {
            Console.WriteLine("Here!!!");
        }

    }

}
