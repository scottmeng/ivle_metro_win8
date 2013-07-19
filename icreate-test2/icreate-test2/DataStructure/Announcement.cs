using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Windows.UI;
using System.Net;
using System.ComponentModel;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Announcement : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember(Name = "ID")]
        public String announceID { get; set; }

        [DataMember(Name = "Title")]
        public String announceName { get; set; }

        [DataMember(Name = "Description")]
        public String announceContent { get; set; }

        [DataMember(Name = "CreatedDate_js")]
        public DateTime announceTime { get; set; }

        [DataMember(Name = "Creator")]
        public Member announceCreator { get; set; }

        [DataMember(Name = "isRead")]
        public bool announceIsRead { get; set; }

        private Color _announceShowColor;
        public Color announceColor 
        {
            get { return _announceShowColor; }
            set
            {
                if (value != _announceShowColor)
                {
                    _announceShowColor = value;
                    OnPropertyChanged("announceColor");
                }
            }
        }
        public Color announcePrimaryColor { get; set; }
        public Color announceSecondaryColor { get; set; }
        public String announceContentDisplay { get; set; }
        public String announceContentPreview { get; set; }
        public String announceNameDisplay { get; set; }
        public String announceCreatorDisplay { get; set; }
        public String announceModuleCode { get; set; }
        public String announceModuleId { get; set; }
        public String announceTimeDisplay { get; set; }
        private int _backgroundConverter;
        public int backgroundConverter 
        {
            get { return _backgroundConverter; }
            set
            {
                if (value != _backgroundConverter)
                {
                    _backgroundConverter = value;
                    OnPropertyChanged("backgroundConverter");
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

        public Announcement(String id, String name, String content, DateTime time, Member creator)
        {
            this.announceID = id;
            this.announceName = name;
            this.announceContent = content;
            this.announceTime = time;
            this.announceCreator = creator;
        }

        public void SetAnnouncementColors(Color primaryColor, Color secondaryColor)
        {
            this.announcePrimaryColor = primaryColor;
            this.announceColor = this.announcePrimaryColor;
            this.announceSecondaryColor = secondaryColor;
            this.backgroundConverter = 1;
        }

        public void GenerateDisplayContent(Module module)
        {
            this.announceContentDisplay = WebUtility.HtmlDecode(announceContent);
            this.announceContentDisplay = Regex.Replace(announceContentDisplay, "<.+?>", string.Empty);
            this.announceContentDisplay = Regex.Replace(announceContentDisplay, "&nbsp;", string.Empty);

            this.announceContentPreview = WebUtility.HtmlDecode(announceContent);
            this.announceContentPreview = Regex.Replace(announceContentPreview, "<.+?>", string.Empty);
            this.announceContentPreview = announceContentPreview.Replace(System.Environment.NewLine, " ");
            this.announceContentPreview = announceContentPreview.Replace("\t", string.Empty);
            this.announceContentPreview = Regex.Replace(announceContentPreview, "&nbsp;", string.Empty);

            // remove the header
            this.announceNameDisplay = announceName.Replace("IVLE: ", string.Empty);
            this.announceNameDisplay = announceNameDisplay.Replace(module.moduleCode + ": ", string.Empty);

            this.announceCreatorDisplay = announceCreator.memberName;
            this.announceModuleCode = module.moduleCode;
            this.announceModuleId = module.moduleId;
            this.announceTimeDisplay = announceTime.Day + "/" + announceTime.Month;
        }
    }
}
