using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Workbin
    {
        [DataMember(Name = "ID")]
        public String workbinID { get; set; }

        [DataMember(Name = "Title")]
        public String workbinTitle { get; set; }

        [DataMember(Name = "Creator")]
        public Member workbinCreator { get; set; }

        [DataMember(Name = "Folders")]
        public List<Folder> workbinFolders { get; set; }

        public Workbin(String id, String title, Member creator, List<Folder> folders)
        {
            workbinID = id;
            workbinTitle = title;
            workbinCreator = creator;
            workbinFolders = folders;
        }
    }
}
