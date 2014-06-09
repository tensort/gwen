﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Police
{
    public class Crime
    {
        /// <summary>
        /// Crime catagory
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 64-character unique identifier for that crime. 
        /// (This is different to the existing 'id' attribute, which is not guaranteed to always stay the same for each crime.)
        /// </summary>
        public string PersistentID { get; set; }

        /// <summary>
        /// Approximate location of the crime
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Extra information about the crime
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// ID of the crime.
        /// This ID only relates to the API, it is NOT a police identifier
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Location type of the crime
        /// </summary>
        public LOCATION_TYPE LocationType { get; set; }

        /// <summary>
        /// Outcome of the crime
        /// </summary>
        public OUTCOME_TYPE OutcomeStatus { get; set; }

        /// <summary>
        /// Date of the outcome
        /// </summary>
        public string OutcomeDate { get; set; }
    }
}