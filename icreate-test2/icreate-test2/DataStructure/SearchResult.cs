using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace icreate_test2.DataStructure
{
    class SearchResult
    {
        public string resultTitle { get; set; }
        public string resultContent { get; set;}

        public SearchResult(string title, string content)
        {
            resultTitle = title;
            resultContent = content;
        }
    }
}
