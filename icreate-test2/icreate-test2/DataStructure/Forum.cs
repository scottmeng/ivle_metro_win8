using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Forum : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember(Name = "ID")]
        public String forumId { get; set; }

        [DataMember(Name = "Title")]
        public String forumTitle { get; set; }

        [DataMember(Name = "WelcomeMessage")]
        public String forumMessage { get; set; }

        [DataMember(Name = "BadgeTool")]
        public int forumBadge { get; set; }

        [DataMember(Name = "Headings")]
        public Heading[] forumHeadings { get; set; }

        private ObservableCollection<PostTitle> _forumAllTitles;
        public ObservableCollection<PostTitle> forumAllTitles 
        {
            get { return this._forumAllTitles; }
            set
            {
                if (value != this._forumAllTitles)
                {
                    this._forumAllTitles = value;
                    OnPropertyChanged("forumAllTitles");
                }
            }
        }

        public Forum(String id, String title, String message, int badge, Heading[] headings)
        {
            forumId = id;
            forumTitle = title;
            forumMessage = message;
            forumBadge = badge;
            forumHeadings = headings;

            forumAllTitles = new ObservableCollection<PostTitle>();
            forumAllTitles.CollectionChanged += _forumAllTitles_CollectionChanged;
        }

        void _forumAllTitles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            string test = null;
            //throw new NotImplementedException();
        }

        // generate the title list to be displayed on the left-
        public void GenerateAllTitles()
        {
            foreach (Heading heading in forumHeadings)
            {
                heading.GenerateAllTitles();
                foreach (PostTitle postTitle in heading.headingAllTiles)
                {
                    if (!this.forumAllTitles.Contains(postTitle, new PostTitleEqualityComparer()))
                    {
                        this.forumAllTitles.Add(postTitle);
                    }
                }
            }
        }

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    class PostTitleEqualityComparer : IEqualityComparer<DataStructure.PostTitle>
    {
        public bool Equals(DataStructure.PostTitle p1, DataStructure.PostTitle p2)
        {
            if (p1.isPostHeading == p2.isPostHeading && p1.headingId == p2.headingId && p1.threadId == p2.threadId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(DataStructure.PostTitle postTitle)
        {
            int hCode = postTitle.postTitle.Length ^ postTitle.isPostHeading.GetHashCode();
            return hCode.GetHashCode();
        }
    }
}
