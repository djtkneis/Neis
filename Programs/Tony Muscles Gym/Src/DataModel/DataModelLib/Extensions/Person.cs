using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neis.Core.DataModel
{
    public partial class Person
    {
        /// <summary>
        /// Constructor for the <see cref="Person"/> class
        /// </summary>
        /// <param name="personId">Identifier for the person</param>
        public Person(int personId)
        {
            Id = personId;
        }
        /// <summary>
        /// Constructor for the <see cref="Person"/> class
        /// </summary>
        /// <param name="name">Name of the person</param>
        public Person(Name name)
        {
            Name = name;
        }
    }
}