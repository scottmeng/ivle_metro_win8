// Token.cs
// class to hold data for web request
// token
// Created by Kaizhi Meng on 9th June 2013
// Last edited on 14th June 2013

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
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

        public Token() { }

        // constructer for use when restoring 
        // from local setting data
        public Token(string tokenContent)
        {
            this.TokenContent = tokenContent;
        }

        // constructer for use when creating
        // token from json parser
        public Token(string content, bool success, DateTime validTill)
        {
            TokenContent = content;
            TokenSuccess = success;
            TokenValidTill = validTill;
        }
    }
}
