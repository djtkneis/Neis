using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neis.Core.DataModel
{
    public partial class Name
    {
        /// <summary>
        /// Constructro for the <see cref="Name"/> class
        /// </summary>
        /// <param name="nameId"></param>
        public Name(int nameId)
        {
            Id = nameId;
        }
        /// <summary>
        /// Constructor for the <see cref="Name"/> class
        /// </summary>
        /// <param name="first">First name</param>
        /// <param name="last">Last name</param>
        public Name(string first, string last)
        {
            First = first;
            Last = last;
        }
    }
}