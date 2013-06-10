using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Lecturer
    {
        [DataMember(Name = "ID")]
        public String lecturerId { get; set; }

        [DataMember(Name = "User")]
        public Member lecturerMember { get; set; }

        [DataMember(Name = "Role")]
        public String lecturerRole { get; set; }

        public Lecturer(String id, Member member, String role)
        {
            lecturerId = id;
            lecturerMember = member;
            lecturerRole = role;
        }
    }
}
