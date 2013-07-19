using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace icreate_test2.DataStructure
{
    class Searchable
    {
        public string resultTitle { get; set; }
        public string resultContent { get; set; }
        public string moduleId { get; set; }
        public string announcementId { get; set; }


        public Searchable(string title, string content, string mId, string aId)
        {
            this.resultTitle = title;
            this.resultContent = content;
            this.moduleId = mId;
            this.announcementId = aId;
        }
    }
}