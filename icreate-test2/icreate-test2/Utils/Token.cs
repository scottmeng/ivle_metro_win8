using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.Utils
{
    [DataContract]
    class Token
    {
        [DataMember(Name = "Token")]
        public string TokenContent { get; set; }

        [DataMember(Name = "Success")]
        public bool TokenSuccess { get; set; }

        [DataMember(Name = "ValidTill_js")]
        public DateTime TokenValidTill { get; set; }

        public Token(string content, bool success, DateTime validTill)
        {
            TokenContent = content;
            TokenSuccess = success;
            TokenValidTill = validTill;
        }
    }
}
