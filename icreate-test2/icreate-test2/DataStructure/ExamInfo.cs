using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class ExamInfo
    {
        [DataMember(Name = "CourseID")]
        public string examCourseId { get; set; }

        [DataMember(Name = "AcadYear")]
        public string examAcadYear { get; set; }

        [DataMember(Name = "Semester")]
        public string examSemester { get; set; }

        [DataMember(Name = "ExamDate_js")]
        public DateTime examDate { get; set; }

        [DataMember(Name = "ExamSession")]
        public string examSession { get; set; }

        [DataMember(Name = "ExamInfo")]
        public string examInfo { get; set; }

        [DataMember(Name = "ModuleCode")]
        public string examModuleCode { get; set; }

        public ExamInfo(string courseId, string acadYear, string semester, DateTime date, string session, string info, string moduleCode)
        {
            examCourseId = courseId;
            examAcadYear = acadYear;
            examSemester = semester;
            examDate = date;
            examSession = session;
            examInfo = info;
            examModuleCode = moduleCode;
        }
    }
}
