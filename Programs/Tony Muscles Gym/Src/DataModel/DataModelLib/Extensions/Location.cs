using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neis.Core.DataModel
{
    public partial class Location
    {
        /// <summary>
        /// Constructor for the <see cref="Location"/> class
        /// </summary>
        /// <param name="locationId">Identifier for the location</param>
        public Location(int locationId)
        {
            Id = locationId;
        }
        /// <summary>
        /// Constructor for the <see cref="Location"/> class
        /// </summary>
        /// <param name="number">Street address number</param>
        /// <param name="street">Street name, type and direction</param>
        /// <param name="city">City</param>
        /// <param name="state">State</param>
        public Location(string number, string street, string city, CodeInformation state)
        {
            Number = number;
            Street = street;
            City = city;
            State = state;
        }
        /// <summary>
        /// Constructor for the <see cref="Location"/> class
        /// </summary>
        /// <param name="number">Street address number</param>
        /// <param name="street">Street name, type and direction</param>
        /// <param name="city">City</param>
        /// <param name="state">State</param>
        public Location(string number, string street, string city, CodeInformation state, int zip)
            : this(number, street, city, state)
        {
            Zip = zip;
        }
    }
}