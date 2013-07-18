using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Heading
    {
        [DataMember(Name = "ID")]
        public string headingId { get; set; }

        [DataMember(Name = "Title")]
        public string headingTitle { get; set; }

        [DataMember(Name = "BadgeHeading")]
        public int headingBadge { get; set; }

        [DataMember(Name = "Threads")]
        public Thread[] headingThreads { get; set; }

        public ObservableCollection<PostTitle> headingAllTiles { get; set; }

        public Heading(string id, string title, int badge, Thread[] threads)
        {
            headingId = id;
            headingTitle = title;
            headingBadge = badge;
            headingThreads = threads;

            headingAllTiles = new ObservableCollection<PostTitle>();
        }

        public void GenerateAllTitles()
        {
            headingAllTiles.Add(new PostTitle(headingTitle, true, null, this.headingId));

            foreach (Thread thread in headingThreads)
            {
                headingAllTiles.Add(new PostTitle(thread.threadTitle, false, thread.threadId, null));
            }
        }
    }
}
