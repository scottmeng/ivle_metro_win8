using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Windows.UI;

namespace icreate_test2.DataStructure
{
    // accessible to all data structures
    enum ItemType { MODULE_INFO, ANNOUNCEMENT, FORUM, WORKBIN, GRADEBOOK, WEBCAST}

    [DataContract]
    class Module
    {
        [DataMember(Name = "Announcements")]
        public Announcement[] moduleAnnouncements { get; set; }

        [DataMember(Name = "Forums")]
        public List<Forum> moduleForums { get; set; }

        [DataMember(Name = "Workbins")]
        public Workbin[] moduleWorkbins { get; set; }

        [DataMember(Name = "Webcasts")]
        public Webcast[] moduleWebcasts { get; set; }

        [DataMember(Name = "Gradebooks")]
        public Gradebook[] moduleGradebooks { get; set; }

        [DataMember(Name = "ID")]
        public String moduleId { get; set; }

        [DataMember(Name = "CourseCode")]
        public String moduleCode { get; set; }

        [DataMember(Name = "CourseName")]
        public String moduleName { get; set; }

        [DataMember(Name = "CourseDepartment")]
        public String moduleDepart { get; set; }

        [DataMember(Name = "CourseSemester")]
        public String moduleSemester { get; set; }

        [DataMember(Name = "CourseAcadYear")]
        public String moduleAcadYear { get; set; }

        [DataMember(Name = "CourseMC")]
        public String moduleMc { get; set; }

        [DataMember(Name = "Lecturers")]
        public Lecturer[] moduleLecturers { get; set; }

        public Color moduleColor { get; set; }
        public List<ModuleItem> moduleItems { get; set; }
        public bool isAnnouncementAvailable { get; set; }
        public bool isWorkbinAvailable { get; set; }
        public bool isGradebookAvailable { get; set; }
        public bool isForumAvailable { get; set; }
        public bool isWebcastAvailable { get; set; }

        public Module(Announcement[] announces, List<Forum> forums, Workbin[] workins, Webcast[] webcasts, Gradebook[] gradebooks, String id, String code, 
                      String name, String depart, String sem, String ay, String mc, Lecturer[] lecturers)
        {
            moduleAnnouncements = announces;
            moduleForums = forums;
            moduleWorkbins = workins;
            moduleWebcasts = webcasts;
            moduleGradebooks = gradebooks;
            moduleId = id;
            moduleCode = code;
            moduleName = name;
            moduleDepart = depart;
            moduleSemester = sem;
            moduleAcadYear = ay;
            moduleMc = mc;
            moduleLecturers = lecturers;

            moduleItems = new List<ModuleItem>();
            isAnnouncementAvailable = false;
            isWorkbinAvailable = false;
            isGradebookAvailable = false;
            isForumAvailable = false;
            isWebcastAvailable = false;
        }

        // set the color of this module and every announcement in it
        public void SetModuleColor(Color color)
        {
            this.moduleColor = color;

            foreach (Announcement announcement in this.moduleAnnouncements)
            {
                announcement.annouceColor = this.moduleColor;
            }
        }

        public void GenerateModuleItemList()
        {
            int index;

            // module info is always available
            moduleItems.Add(new ModuleItem("Module Info", ItemType.MODULE_INFO, 0));

            if (moduleAnnouncements.Count() > 0)
            {
                moduleItems.Add(new ModuleItem("Announcements", ItemType.ANNOUNCEMENT, 0));
                isAnnouncementAvailable = true;
            }

            if (moduleForums.Count() > 0)
            {
                index = 0;
                foreach (Forum forum in moduleForums)
                {
                    moduleItems.Add(new ModuleItem(forum.forumTitle, ItemType.FORUM, index));
                    index++;
                }
                isForumAvailable = true;
            }

            if (moduleWorkbins.Count() > 0)
            {
                index = 0;
                foreach (Workbin workbin in moduleWorkbins)
                {
                    moduleItems.Add(new ModuleItem(workbin.workbinTitle, ItemType.WORKBIN, index));
                    index++;
                }
                isWorkbinAvailable = true;
            }

            if (moduleGradebooks.Count() > 0)
            {
                index = 0;
                foreach (Gradebook gradebook in moduleGradebooks)
                {
                    moduleItems.Add(new ModuleItem("Gradebook", ItemType.GRADEBOOK, index));
                    index++;
                }
                isGradebookAvailable = true;
            }

            if (moduleWebcasts.Count() > 0)
            {
                index = 0;
                foreach (Webcast webcast in moduleWebcasts)
                {
                    moduleItems.Add(new ModuleItem(webcast.webcastTitle, ItemType.WEBCAST, index));
                    index++;
                }
                isWebcastAvailable = true;
            }
        }
    }
}
