using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Member
    {
        [DataMember(Name = "UserId")]
        public String memberId { get; set; }

        [DataMember(Name = "Name")]
        public String memberName { get; set; }

        [DataMember(Name = "Email")]
        public String memberEmail { get; set; }

        [DataMember(Name = "UserGuid")]
        public String memberGuid { get; set; }

        public Member(String id, String name, String email, String guid)
        {
            memberId = id;
            memberName = name;
            memberEmail = email;
            memberGuid = guid;
        }
    }
}
