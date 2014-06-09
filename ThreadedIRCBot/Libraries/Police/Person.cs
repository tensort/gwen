using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Police
{
    public class Person
    {
        /// <summary>
        /// Details for a method of contact
        /// </summary>
        public class PersonalContactDetail
        {
            /// <summary>
            /// The type of the contact (e.g. Twitter/EMail etc)
            /// </summary>
            public string Type { get; private set; }

            /// <summary>
            /// The contact information (e.g. http://twitter.com/graymalkinsblog)
            /// </summary>
            public string Contact { get; private set; }

            public PersonalContactDetail(string type, string contact)
            {
                Type = type;
                Contact = contact;
            }
        }

        /// <summary>
        /// Officer's Name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Officer Rank
        /// </summary>
        public string Rank { get; private set; }

        /// <summary>
        /// Officer's Biography
        /// </summary>
        public string Biography { get; private set; }

        public IList<PersonalContactDetail> ContactDetails { get; private set; }
    }
}
