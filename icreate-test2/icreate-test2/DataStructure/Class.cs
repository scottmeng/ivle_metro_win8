﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Windows.UI;

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

        public int classDayCodeInt { get; set; }
        public int classStartTimeInt { get; set; }
        public int classEndTimeInt { get; set; }
        public int classDurationInt { get; set; }
        public int classTimePoint { get; set; }
        public Color classModuleColor { get; set; }

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

            if (classStartTime.Length == 3)
            {
                classStartTime = "0" + classStartTime;
            }

            if (classEndTime.Length == 3)
            {
                classEndTime = "0" + classEndTime;
            }

            try
            {
                classStartTimeInt = Int32.Parse(classStartTime);
                classEndTimeInt = Int32.Parse(classEndTime);
                classDurationInt = classEndTimeInt - classStartTimeInt;
                classDayCodeInt = Int32.Parse(classDayCode);
                classTimePoint = classStartTimeInt + 10000 * classDayCodeInt;
            }
            catch 
            {
                classStartTimeInt = 0;
                classEndTimeInt = 0;
                classDurationInt = classEndTimeInt - classStartTimeInt;
                classDayCodeInt = -1;
                classTimePoint = classStartTimeInt + 10000 * classDayCodeInt;
            }
        }
    }
}
