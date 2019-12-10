using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neis.Core.DataModel
{
    public partial class PersonPhone
    {
        /// <summary>
        /// Constructor for the <see cref="PersonPhone"/> class
        /// </summary>
        /// <param name="personPhoneId">Identifier for the person/phone relationship</param>
        public PersonPhone(int personPhoneId)
        {
            Id = personPhoneId;
        }
        /// <summary>
        /// Constructor for the <see cref="PersonPhone"/> class
        /// </summary>
        /// <param name="personId">Identifier for the person</param>
        /// <param name="phoneId">Identifier for the phone</param>
        public PersonPhone(int personId, int phoneId)
        {
            PersonId = personId;
            Person = new Person(PersonId);

            PhoneId = phoneId;
            Phone = new Phone(phoneId);
        }
    }
}