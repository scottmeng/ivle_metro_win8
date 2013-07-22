using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Webcast
    {
        [DataMember(Name = "ID")]
        public String webcastId { get; set; }

        [DataMember(Name = "Title")]
        public String webcastTitle { get; set; }

        [DataMember(Name = "Creator")]
        public Member webcastCreator { get; set; }

        [DataMember(Name = "ItemGroups")]
        public List<VideoGroup> webcastVideoGroups { get; set; }

        public Webcast(String id, String title, Member creator, List<VideoGroup> videoGroups)
        {
            this.webcastId = id;
            this.webcastTitle = title;
            this.webcastCreator = creator;
            this.webcastVideoGroups = videoGroups;
        }
    }
}
