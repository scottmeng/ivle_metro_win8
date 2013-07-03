using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace icreate_test2.DataStructure
{
    class PostTitle
    {
        public String postTitle { get; set; }
        public bool isPostHeading { get; set; }
        public String threadId { get; set; }

        public PostTitle(String title, bool isHeading, String id)
        {
            postTitle = title;
            isPostHeading = isHeading;
            threadId = id;
        }
    }
}
