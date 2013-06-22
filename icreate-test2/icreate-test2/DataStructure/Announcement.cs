using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Windows.UI;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Announcement
    {
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

        public Color annouceColor { get; set; }
        public String announceContentDisplay { get; set; }
        public String announceContentPreview { get; set; }
        public String announceNameDisplay { get; set; }
        public String announceCreatorDisplay { get; set; }
        public String announceModuleCode { get; set; }
        public String announceTimeDisplay { get; set; }

        public Announcement(String id, String name, String content, DateTime time, Member creator)
        {
            announceID = id;
            announceName = name;
            announceContent = content;
            announceTime = time;
            announceCreator = creator;

        }

        public void GenerateDisplayContent(String moduleCode)
        {
            announceContentDisplay = Regex.Replace(announceContent, "<.+?>", string.Empty);
            announceContentDisplay = Regex.Replace(announceContentDisplay, "&nbsp;", string.Empty);

            announceContentPreview = Regex.Replace(announceContent, "<.+?>", string.Empty);
            announceContentPreview = announceContentPreview.Replace(System.Environment.NewLine, " ");
            announceContentPreview = Regex.Replace(announceContentPreview, "&nbsp;", string.Empty);

            // remove the header
            announceNameDisplay = announceName.Replace("IVLE: ", string.Empty);
            announceNameDisplay = announceNameDisplay.Replace(moduleCode + ": ", string.Empty);

            announceCreatorDisplay = announceCreator.memberName;
            announceModuleCode = moduleCode;
            announceTimeDisplay = announceTime.Day + "/" + announceTime.Month;
        }
    }
}
