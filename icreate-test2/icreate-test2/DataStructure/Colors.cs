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
        private static Color[] _moduleColors = new Color[12]{
            //Color.FromArgb(255, 0, 69, 0),
            //Color.FromArgb(255, 187, 27, 27),
            //Color.FromArgb(255, 186, 27, 107),
            //Color.FromArgb(255, 100, 26, 183),
            //Color.FromArgb(255, 26, 85, 175),
            //Color.FromArgb(255, 226, 107, 27),

            Color.FromArgb(255, 16, 148, 165),
            Color.FromArgb(255, 214, 85, 107),
            Color.FromArgb(255, 128,191,33),
            Color.FromArgb(255, 222,82,180),
            Color.FromArgb(255, 42, 98, 160),
            Color.FromArgb(255, 246,163,0),

            
            Color.FromArgb(255, 17, 155, 43),
            
            
            Color.FromArgb(255, 28, 4, 102),
            Color.FromArgb(255, 4, 76, 88),
            Color.FromArgb(255, 1, 174, 170),
            Color.FromArgb(255, 217, 100, 179),
            Color.FromArgb(255, 134, 184, 27)
        };

        private static Color[] _moduleSecondayrColors = new Color[12]{
            //Color.FromArgb(255, 22, 154, 8),
            //Color.FromArgb(255, 255, 43, 19),
            //Color.FromArgb(255, 255, 28, 117),
            //Color.FromArgb(255, 167, 64, 255),
            //Color.FromArgb(255, 28, 175, 255),
            //Color.FromArgb(255, 255, 152, 31),

            Color.FromArgb(255, 115, 206, 214),
            Color.FromArgb(255, 231, 151, 160),
            Color.FromArgb(255, 190,203,73),
            Color.FromArgb(255, 229,132,201),
            Color.FromArgb(255, 154, 200, 243),
            Color.FromArgb(255, 242,203,87),

            
            Color.FromArgb(255, 0, 195, 65),
            
            
            Color.FromArgb(255, 69, 24, 177),
            Color.FromArgb(255, 1, 131, 131),
            Color.FromArgb(255, 5, 215, 202),
            Color.FromArgb(255, 253, 119, 188),
            Color.FromArgb(255, 147, 209, 4)
        };

        public static Color GetModuleColor(int index)
        {
            return _moduleColors[index];
        }

        public static Color GetSecondaryColor(int index)
        {
            return _moduleSecondayrColors[index];
        }
    }
}
