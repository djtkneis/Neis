using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neis.Core.DataModel
{
    public partial class GymMember
    {
        /// <summary>
        /// Constructor for the <see cref="GymMember"/> class
        /// </summary>
        /// <param name="gymMemberId">Identifier for the gym member</param>
        public GymMember(int gymMemberId)
        {
            Id = gymMemberId;
        }
        /// <summary>
        /// Constructor for the <see cref="GymMember"/> class
        /// </summary>
        /// <param name="person">Information about the gym member</param>
        public GymMember(Person person)
        {
            Person = person;
        }
    }
}