using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class VideoFile
    {
        [DataMember(Name = "ID")]
        public string videoId { get; set; }

        [DataMember(Name = "FileName")]
        public string videoFileName { get; set; }

        [DataMember(Name = "MP4")]
        public string videoMP4 { get; set; }

        [DataMember(Name = "MP3")]
        public string videoMP3 { get; set; }

        [DataMember(Name = "FileTitle")]
        public string videoTitle { get; set; }

        [DataMember(Name = "MediaFormat")]
        public string videoFormat { get; set; }

        [DataMember(Name = "CreateDate_js")]
        public DateTime videoCreateDate { get; set; }

        [DataMember(Name = "Creator")]
        public Member videoCreator { get; set; }

        [DataMember(Name = "isRead")]
        public bool videoIsViewed { get; set; }

        public VideoFile(string id, string fileName, string mp4, string mp3, string title, string format,
            DateTime createDate, Member creator, bool isViewed)
        {
            this.videoId = id;
            this.videoFileName = fileName;
            this.videoMP4 = mp4;
            this.videoMP3 = mp3;
            this.videoTitle = title;
            this.videoFormat = format;
            this.videoCreateDate = createDate;
            this.videoCreator = creator;
            this.videoIsViewed = isViewed;
        }
    }
}
