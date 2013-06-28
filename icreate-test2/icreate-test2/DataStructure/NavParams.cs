using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace icreate_test2.DataStructure
{
    class NavParams
    {
        public int moduleIndex { get; set; }
        public int announcementIndex { get; set; }

        public NavParams(int mi, int ai)
        {
            this.moduleIndex = mi;
            this.announcementIndex = ai;
        }
    }
}
