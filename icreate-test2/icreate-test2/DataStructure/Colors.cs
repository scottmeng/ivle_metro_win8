using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace icreate_test2.DataStructure
{
    static class Colors
    {
        private static Color[] _moduleColors = new Color[10]{
            Color.FromArgb(255, 162, 0, 255),
            Color.FromArgb(255, 255, 0, 151),
            Color.FromArgb(255, 0, 171, 169),
            Color.FromArgb(255, 140, 191, 38),
            Color.FromArgb(255, 160, 80, 0),
            Color.FromArgb(255, 230, 113, 184),
            Color.FromArgb(255, 240, 150, 9),
            Color.FromArgb(255, 27, 161, 226), 
            Color.FromArgb(255, 229, 20, 0),
            Color.FromArgb(255, 51, 153, 51)
        };

        public static Color GetModuleColor(int index)
        {
            return _moduleColors[index];
        }
    }
}
