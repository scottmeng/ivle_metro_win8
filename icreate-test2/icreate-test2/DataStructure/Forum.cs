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

        public ObservableCollection<PostTitle> forumAllTitles { get; set; }

        public Forum(String id, String title, String message, int badge, Heading[] headings)
        {
            forumId = id;
            forumTitle = title;
            forumMessage = message;
            forumBadge = badge;
            forumHeadings = headings;

            forumAllTitles = new ObservableCollection<PostTitle>();
        }

        // generate the title list to be displayed on the left-
        public void GenerateAllTitles()
        {
            foreach (Heading heading in forumHeadings)
            {
                heading.GenerateAllTitles();
                foreach(PostTitle postTitle in heading.headingAllTiles)
                {
                    forumAllTitles.Add(postTitle);
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
}
