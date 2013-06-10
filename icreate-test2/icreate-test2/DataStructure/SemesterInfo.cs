using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class SemesterInfo
    {
        public string AcademicYear { get; set; }
        public string Semester { get; set; }

        public SemesterInfo(string ay, string sem)
        {
            AcademicYear = ay;
            Semester = sem;
        }
    }
}
