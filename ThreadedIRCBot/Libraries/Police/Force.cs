using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Police
{
    /// <summary>
    /// Represents a specific Police Force
    /// </summary>
    public class Force
    {
        #region Engagement Methods
        public class Engagement_Method
        {
            /// <summary>
            /// URL for the Engagement
            /// </summary>
            public string URL { get; private set; }

            /// <summary>
            /// Description
            /// </summary>
            public string Description { get; private set; }

            /// <summary>
            /// Title
            /// </summary>
            public string Title { get; private set; }

            /// <summary>
            /// Creates a new Engagement Method
            /// </summary>
            /// <param name="url">URL</param>
            /// <param name="description">Description</param>
            /// <param name="title">Title</param>
            public Engagement_Method(string url, string description, string title)
            {
                URL = url;
                Description = description;
                Title = title;
            }
        }
        #endregion

        #region Compulsary Information
        /// <summary>
        /// The Unique ID of this Force
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// The Name of this Force
        /// </summary>
        public string Name { get; private set; }
        #endregion

        #region Optional Information
        /// <summary>
        /// Force description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Force telephone number
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// A list of the community engagement methods from this force
        /// </summary>
        public IList<Engagement_Method> EngagementMethods { get; set; }

        /// <summary>
        /// The Force's web URL
        /// </summary>
        public string URL { get; set; }
        #endregion

        /// <summary>
        /// Creates a new Force object
        /// </summary>
        /// <param name="id">Force ID</param>
        /// <param name="name">Force Name</param>
        public Force(string id, string name)
        {
            ID = id;
            Name = name;
            EngagementMethods = new List<Engagement_Method>();
        }
    }
}
