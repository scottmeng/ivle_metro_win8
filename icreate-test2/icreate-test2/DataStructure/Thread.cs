using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Net;
using Windows.UI;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Thread
    {
        [DataMember(Name = "ID")]
        public String threadId { get; set; }

        [DataMember(Name = "PostTitle")]
        public String threadTitle { get; set; }

        [DataMember(Name = "PostBody")]
        public String threadBody { get; set; }

        [DataMember(Name = "PostDate_js")]
        public DateTime threadDate { get; set; }

        [DataMember(Name = "Poster")]
        public Member threadPoster { get; set; }

        [DataMember(Name = "Threads")]
        public List<Thread> threadInnerThreads { get; set; }

        [DataMember(Name = "isPosterStaff")]
        public bool threadIsPosterStaff { get; set; }

        [DataMember(Name = "isRead")]
        public bool threadIsRead { get; set; }

        // hold all threads data
        public ObservableCollection<Thread> threadAllThreads { get; set; }

        public Thread(String id, String title, String body, DateTime date, Member poster, List<Thread> innerThreads, bool isPosterStaff, bool isRead)
        {
            threadId = id;
            threadTitle = title;
            threadBody = body;
            threadDate = date;
            threadPoster = poster;
            threadInnerThreads = innerThreads;
            threadIsPosterStaff = isPosterStaff;
            threadIsRead = isRead;           
        }

        public void GenerateAllThread()
        {
            threadBody = WebUtility.HtmlDecode(threadBody);
            threadBody = Regex.Replace(threadBody, "<.+?>", string.Empty);
            threadBody = Regex.Replace(threadBody, "&nbsp;", string.Empty);

            threadAllThreads = new ObservableCollection<Thread>();
            foreach (Thread thread in threadInnerThreads)
            {
                thread.GenerateAllThread();
                foreach (Thread innerThread in thread.threadAllThreads)
                {
                    threadAllThreads.Add(innerThread);
                }
            }
            threadAllThreads.Insert(0, this);
        }
    }
}
