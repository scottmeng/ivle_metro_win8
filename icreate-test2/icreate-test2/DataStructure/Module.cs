using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Windows.UI;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Module
    {
        [DataMember(Name = "Announcements")]
        public Announcement[] moduleAnnouncements { get; set; }

        [DataMember(Name = "Workbins")]
        public Workbin[] moduleWorkbins { get; set; }

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

        public Module(Announcement[] announces, Workbin[] workins, String id, String code, String name, String depart, String sem, String ay, String mc, Lecturer[] lecturers)
        {
            moduleAnnouncements = announces;
            moduleWorkbins = workins;
            moduleId = id;
            moduleCode = code;
            moduleName = name;
            moduleDepart = depart;
            moduleSemester = sem;
            moduleAcadYear = ay;
            moduleMc = mc;
            moduleLecturers = lecturers;
        }

        // set the color of this module and every announcement
        public void SetModuleColor(Color color)
        {
            this.moduleColor = color;

            foreach (Announcement announcement in this.moduleAnnouncements)
            {
                announcement.annouceColor = this.moduleColor;
            }
        }
    }
}
