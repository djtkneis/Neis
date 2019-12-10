using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neis.Core.DataModel
{
    public partial class Phone
    {
        /// <summary>
        /// Constructor for the <see cref="Phone"/> class
        /// </summary>
        /// <param name="phoneId">Identifier for this phone object</param>
        public Phone(int phoneId)
        {
            Id = phoneId;
        }
        /// <summary>
        /// Constructor for the <see cref="Phone"/> class
        /// </summary>
        /// <param name="areaCode">Area code</param>
        /// <param name="prefix">Prefix</param>
        /// <param name="suffix">Suffix</param>
        public Phone(int areaCode, int prefix, int suffix)
        {
            this.AreaCode = areaCode;
            this.Prefix = prefix;
            this.Suffix = suffix;
        }
    }
}
