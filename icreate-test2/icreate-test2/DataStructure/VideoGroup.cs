using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class VideoGroup
    {
        [DataMember(Name = "ItemGroupTitle")]
        public string videoGroupTitle { get; set; }

        [DataMember(Name = "ItemGroups")]
        public VideoGroup[] videoInnerGroups { get; set; }

        [DataMember(Name = "Files")]
        public List<VideoFile> videoFiles { get; set; }

        public VideoGroup(string groupTitle, VideoGroup[] innerGroups, List<VideoFile> files)
        {
            this.videoGroupTitle = groupTitle;
            this.videoInnerGroups = innerGroups;
            this.videoFiles = files;
        }
    }
}
