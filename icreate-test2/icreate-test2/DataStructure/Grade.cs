using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Grade
    {
        [DataMember(Name = "ID")]
        public string gradeId { get; set; }

        [DataMember(Name = "ItemName")]
        public string gradeName { get; set; }

        [DataMember(Name = "ItemDescription")]
        public string gradeDescription { get; set; }

        [DataMember(Name = "MarksObtained")]
        public string gradeObtainedMarks { get; set; }

        [DataMember(Name = "Remark")]
        public string gradeRemark { get; set; }

        [DataMember(Name = "MaxMarks")]
        public int gradeMaxMarks { get; set; }

        [DataMember(Name = "HighestLowestMarks")]
        public string gradeHighestLowestMarks { get; set; }

        [DataMember(Name = "AverageMedianMarks")]
        public string gradeAverageMedianMarks { get; set; }

        [DataMember(Name = "Percentile")]
        public string gradePercentile { get; set; }

        [DataMember(Name = "DateEntered")]
        public string gradeEnteredDate { get; set; }

        public string gradeCategory { get; set; }

        public Grade(string id, string name, string description, string obtainedMarks, string remark, int maxMarks, string highestLowestMarks, string averageMedianMarks,
                     string percentile, string enteredDate)
        {
            gradeId = id;
            gradeName = name;
            gradeDescription = description;
            gradeObtainedMarks = obtainedMarks;
            gradeRemark = remark;
            gradeMaxMarks = maxMarks;
            gradeHighestLowestMarks = highestLowestMarks;
            gradeAverageMedianMarks = averageMedianMarks;
            gradePercentile = percentile;
            gradeEnteredDate = enteredDate;
        }

        public void SetGradeCategory(string category)
        {
            gradeCategory = category;
        }
    }
}
