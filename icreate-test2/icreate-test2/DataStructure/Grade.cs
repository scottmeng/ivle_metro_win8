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
        public String gradeId { get; set; }

        [DataMember(Name = "ItemName")]
        public String gradeName { get; set; }

        [DataMember(Name = "ItemDescription")]
        public String gradeDescription { get; set; }

        [DataMember(Name = "MarksObtained")]
        public String gradeObtainedMarks { get; set; }

        [DataMember(Name = "Remark")]
        public String gradeRemark { get; set; }

        [DataMember(Name = "MaxMarks")]
        public int gradeMaxMarks { get; set; }

        [DataMember(Name = "HighestLowestMarks")]
        public String gradeHighestLowestMarks { get; set; }

        [DataMember(Name = "AverageMedianMarks")]
        public String gradeAverageMedianMarks { get; set; }

        [DataMember(Name = "Percentile")]
        public String gradePercentile { get; set; }

        public Grade(String id, String name, String description, String obtainedMarks, String remark, int maxMarks, String highestLowestMarks, String averageMedianMarks,
                     String percentile)
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
        }
    }
}
