﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Neis.Core.DataModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// Phone number specific to a person
	/// </summary>
	public partial class PersonPhone : DataObject
	{
		/// <summary>
		/// The identifier for the Person object
		/// </summary>
		protected virtual int PersonId
		{
			get;
			set;
		}

		/// <summary>
		/// The person this phone number is associated to
		/// </summary>
		public virtual Person Person
		{
			get;
			set;
		}

		/// <summary>
		/// The phone number for this relationship
		/// </summary>
		public virtual Phone Phone
		{
			get;
			set;
		}

		/// <summary>
		/// The identifier for the Phone object
		/// </summary>
		protected virtual int PhoneId
		{
			get;
			set;
		}

	}
}

