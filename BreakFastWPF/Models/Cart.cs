using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BreakFastWPF.Models
{
    class Cart : DependencyObject
    {
        public int ItemCount
        {
            get { return (int)GetValue(ItemCountProperty); }
            set { SetValue(ItemCountProperty, value); }
        }

        public static readonly DependencyProperty ItemCountProperty =
             DependencyProperty.Register("ItemCount", typeof(int),
             typeof(Cart), new PropertyMetadata(0));
    }
    public class ItemType
    {
        public string Itemkey { get; set; }
        public String Description { get; set; }
        public double Price { get; set; }

        public ItemType(string itemkey, string description, double price)
        {
            Itemkey = itemkey;
            Description = description;
            Price = price;
        }

        public override string ToString()
        {
            return Description;
        }

    }

    public class ItemBase : INotifyPropertyChanged
    {
        private string aPhoto;
        private ItemType aItemType;
        private int aQuantity;

        public ItemBase(string photo, ItemType itemtype, int quantity)
        {
            Photo = photo;
            ItemType = itemtype;
            Quantity = quantity;
        }

        public ItemBase(string photo, string itemkey, string description, double cost)
        {
            Photo = photo;
            ItemType = new ItemType(itemkey, description, cost);
            Quantity = 0;
        }

        public string Photo
        {
            set { aPhoto = value; OnPropertyChanged("Photo"); }
            get { return aPhoto; }
        }

        public ItemType ItemType
        {
            set { aItemType = value; OnPropertyChanged("ItemType"); }
            get { return aItemType; }
        }

        public int Quantity
        {
            set { aQuantity = value; OnPropertyChanged("Quantity"); }
            get { return aQuantity; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public override string ToString()
        {
            return ItemType.ToString();
        }
    }

    public class CartItem : ItemBase
    {
        public CartItem(string photo, string itemkey, string desp, double cost) : base(photo, itemkey, desp, cost) { }
    }

    public class CartList : ObservableCollection<ItemBase> { }
    public class ShipList : ObservableCollection<ItemBase> { }

}
