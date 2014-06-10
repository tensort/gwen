using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Police
{
    public class CrimeCategory
    {
        /// <summary>
        /// Unique identifier for this category
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Name of this category
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Creates a new CrimeCategory
        /// </summary>
        /// <param name="id">The unique ID of this category</param>
        /// <param name="name">The name of this category</param>
        public CrimeCategory(string id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
