using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Police
{
    public class Neighbourhood
    {
        /// <summary>
        /// Police force specific team identifier.
        /// Note: this identifier is not unique and may also be used by a different force
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Name of this Neighbourhood
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Creates a new Neighbourhood
        /// </summary>
        /// <param name="id">The ID of this Neighbourhood</param>
        /// <param name="name">The name of this Neighbourhood</param>
        public Neighbourhood(string id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
