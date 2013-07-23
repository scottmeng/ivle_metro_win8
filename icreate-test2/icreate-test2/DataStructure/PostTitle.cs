using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace icreate_test2.DataStructure
{
    class PostTitle : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _postTitle;
        private bool _isPostHeading;
        private string _threadId;
        private string _headingId;

        public string postTitle 
        {
            get { return _postTitle; }
            set
            {
                if (value != _postTitle)
                {
                    _postTitle = value;
                    OnPropertyChanged("postTitle");
                }
            }
        }

        public bool isPostHeading { get; set; }
        public String threadId { get; set; }
        public String headingId { get; set; }
        public Member threadPoster { get; set; }
        public string threadDatetime { get; set; }

        public PostTitle(String title, bool isHeading, String threadId, String headingId, 
            Member poster, string datetime)
        {
            this.postTitle = title;
            this.isPostHeading = isHeading;
            this.threadId = threadId;
            this.headingId = headingId;
            this.threadPoster = poster;
            this.threadDatetime = datetime;
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
