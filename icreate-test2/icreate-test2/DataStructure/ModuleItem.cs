using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace icreate_test2.DataStructure
{
    class ModuleItem
    {
        // represents the text to be shown on the tab
        public String itemName { get; set; }
        // represents data type and which flipview to present
        public ItemType itemType { get; set; }
        // represents the index of the item if there are
        // multiple entries of the same data type
        public int itemIndex { get; set; }

        public ModuleItem(String name, ItemType type, int index)
        {
            itemName = name;
            itemType = type;
            itemIndex = index;
        }
    }
}
