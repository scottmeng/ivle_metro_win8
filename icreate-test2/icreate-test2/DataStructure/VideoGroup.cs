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
        public List<VideoGroup> videoInnerGroups { get; set; }

        [DataMember(Name = "Files")]
        public List<VideoFile> videoFiles { get; set; }

        public List<VideoFile> allVideoFiles { get; set; }

        public VideoGroup(string groupTitle, List<VideoGroup> innerGroups, List<VideoFile> files)
        {
            this.videoGroupTitle = groupTitle;
            this.videoInnerGroups = innerGroups;
            this.videoFiles = files;

            allVideoFiles = new List<VideoFile>();
        }

        public void GenerateAllVideoFiles()
        {
            if (this.videoInnerGroups != null)
            {
                foreach (VideoGroup videoGroup in this.videoInnerGroups)
                {
                    videoGroup.GenerateAllVideoFiles();

                    foreach (VideoFile videoFile in videoGroup.allVideoFiles)
                    {
                        this.allVideoFiles.Add(videoFile);
                    }
                }
            }

            foreach (VideoFile videoFile in this.videoFiles)
            {
                this.allVideoFiles.Add(videoFile);
            }
        }
    }
}
