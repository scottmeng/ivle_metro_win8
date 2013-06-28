using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Gradebook
    {
        [DataMember(Name = "ID")]
        public String gradebookId { get; set; }

        [DataMember(Name = "CategoryTitle")]
        public String gradebookTitle { get; set; }

        [DataMember(Name = "Items")]
        public Grade[] gradebookGrades { get; set; }

        public Gradebook(String id, String title, Grade[] grades)
        {
            gradebookId = id;
            gradebookTitle = title;
            gradebookGrades = grades;
        }
    }
}
