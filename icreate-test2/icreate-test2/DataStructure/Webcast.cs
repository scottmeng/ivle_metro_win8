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

        public List<VideoFile> webcastAllVideoFiles { get; set; }

        public Webcast(String id, String title, Member creator, List<VideoGroup> videoGroups)
        {
            this.webcastId = id;
            this.webcastTitle = title;
            this.webcastCreator = creator;
            this.webcastVideoGroups = videoGroups;

            this.webcastAllVideoFiles = new List<VideoFile>();
        }

        public void GenerateVideoFileList()
        {
            foreach (VideoGroup videoGroup in this.webcastVideoGroups)
            {
                videoGroup.GenerateAllVideoFiles();

                foreach (VideoFile videoFile in videoGroup.allVideoFiles)
                {
                    this.webcastAllVideoFiles.Add(videoFile);
                }
            }
        }
    }
}
