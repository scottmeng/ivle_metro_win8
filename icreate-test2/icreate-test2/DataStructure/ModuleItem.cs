using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Windows.UI;

namespace icreate_test2.DataStructure
{
    class ModuleItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // represents the text to be shown on the tab
        public String itemName { get; set; }
        // represents data type and which flipview to present
        public ItemType itemType { get; set; }
        // represents the index of the item if there are
        // multiple entries of the same data type
        public int itemIndex { get; set; }

        private Color _itemShowColor;

        public Color itemShowColor
        {
            get { return this._itemShowColor; }
            set
            {
                if (value != this._itemShowColor)
                {
                    this._itemShowColor = value;
                    OnPropertyChanged("itemShowColor");
                }
            }
        }

        public Color itemPrimaryColor { get; set; }
        public Color itemSecondaryColor { get; set; }

        public ModuleItem(String name, ItemType type, int index, Color primaryColor, Color secondaryColor)
        {
            this.itemName = name;
            this.itemType = type;
            this.itemIndex = index;
            this.itemPrimaryColor = primaryColor;
            this.itemSecondaryColor = secondaryColor;
            this._itemShowColor = this.itemPrimaryColor;
        }

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
