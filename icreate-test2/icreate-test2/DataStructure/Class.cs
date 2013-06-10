using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Class
    {
        [DataMember(Name = "StartTime")]
        public string classStartTime { get; set; }

        [DataMember(Name = "EndTime")]
        public string classEndTime { get; set; }

        [DataMember(Name = "ModuleCode")]
        public string classModuleCode { get; set; }

        [DataMember(Name = "ClassNo")]
        public string classNo { get; set; }

        [DataMember(Name = "LessonType")]
        public string classLessonType { get; set; }

        [DataMember(Name = "Venue")]
        public string classVenue { get; set; }

        [DataMember(Name = "DayText")]
        public string classDayText { get; set; }

        [DataMember(Name = "DayCode")]
        public string classDayCode { get; set; }

        [DataMember(Name = "WeekText")]
        public string classWeek { get; set; }

        public int classTimePoint { get; set; }

        public Class(string startTime, string endTime, string moduleCode, string number, string lessonType, string venue, string dayCode, string dayText, string week)
        {
            classStartTime = startTime;
            classEndTime = endTime;
            classModuleCode = moduleCode;
            classNo = number;
            classLessonType = lessonType;
            classVenue = venue;
            classDayCode = dayCode;
            classDayText = dayText;
            classWeek = week;
        }

        public void GenerateDisplay()
        {
            if (classStartTime.Length == 3)
            {
                classStartTime = "0" + classStartTime;
            }

            if (classEndTime.Length == 3)
            {
                classEndTime = "0" + classEndTime;
            }

            classTimePoint = Int32.Parse(classStartTime) + 10000 * Int32.Parse(classDayCode);
        }
    }
}
