using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Folder
    {
        [DataMember(Name = "ID")]
        public string folderId { get; set; }

        [DataMember(Name = "FolderName")]
        public string folderName { get; set; }

        [DataMember(Name = "OpenDate_js")]
        public DateTime folderOpenDate { get; set; }

        [DataMember(Name = "CloseDate_js")]
        public DateTime folderCloseDate { get; set; }

        [DataMember(Name = "FileCount")]
        public int folderFileCount { get; set; }

        [DataMember(Name = "Folders")]
        public Folder[] folderInnerFolders { get; set; }

        [DataMember(Name = "Files")]
        public File[] folderFiles { get; set; }

        public string folderModuleCode { get; set; }

        public Folder(string id, string name, DateTime openDate, DateTime closeDate, int count, Folder[] folders, File[] files)
        {
            folderId = id;
            folderName = name;
            folderOpenDate = openDate;
            folderCloseDate = closeDate;
            folderFileCount = count;
            folderInnerFolders = folders;
            folderFiles = files;
        }

        public void recordModuleCode(string moduleCode)
        {
            folderModuleCode = moduleCode;
        }
    }
}
