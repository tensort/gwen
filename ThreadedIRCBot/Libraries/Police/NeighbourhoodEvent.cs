using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot.Libraries.Police
{
    public class NeighbourhoodEvent
    {
        /// <summary>
        /// Details for a method of contact
        /// </summary>
        public class NeighbourhoodEventContactDetail
        {
            /// <summary>
            /// The type of the contact (e.g. Twitter/EMail etc)
            /// </summary>
            public string Type { get; private set; }

            /// <summary>
            /// The contact information (e.g. http://twitter.com/graymalkinsblog)
            /// </summary>
            public string Contact { get; private set; }

            /// <summary>
            /// Creates a new NeighbourhoodEventContactDetail
            /// </summary>
            /// <param name="type">The type of contact</param>
            /// <param name="contact">The actual contact details</param>
            public NeighbourhoodEventContactDetail(string type, string contact)
            {
                Type = type;
                Contact = contact;
            }
        }

        /// <summary>
        /// Contact details for the event (if available)
        /// </summary>
        public IList<NeighbourhoodEventContactDetail> ContactDetails { get; set; }

        /// <summary>
        /// Description of the event
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Title of the event
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Address of the event
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Type of event
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Start of the event
        /// </summary>
        public DateTime StartDate { get; set; }
    }

}