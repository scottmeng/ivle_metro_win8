using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Heading
    {
        [DataMember(Name = "ID")]
        public String headingId { get; set; }

        [DataMember(Name = "Title")]
        public String headingTitle { get; set; }

        [DataMember(Name = "BadgeHeading")]
        public int headingBadge { get; set; }

        [DataMember(Name = "Threads")]
        public Thread[] headingThreads { get; set; }

        public List<PostTitle> headingPostTile { get; set; }

        public Heading(String id, String title, int badge, Thread[] threads)
        {
            headingId = id;
            headingTitle = title;
            headingBadge = badge;
            headingThreads = threads;

            headingPostTile = new List<PostTitle>();
        }

        public void GeneratePostTitle()
        {
                
        }
    }
}
