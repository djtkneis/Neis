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
	/// The information for a location
	/// </summary>
	public partial class Location : DataObject
	{
		/// <summary>
		/// The street address number
		/// </summary>
		public virtual string Number
		{
			get;
			set;
		}

		/// <summary>
		/// The city
		/// </summary>
		public virtual string City
		{
			get;
			set;
		}

		/// <summary>
		/// The street name
		/// </summary>
		public virtual string Street
		{
			get;
			set;
		}

		/// <summary>
		/// The zip code
		/// </summary>
		public virtual int Zip
		{
			get;
			set;
		}

		/// <summary>
		/// The state
		/// </summary>
		public virtual CodeInformation State
		{
			get;
			set;
		}

		/// <summary>
		/// The country
		/// </summary>
		public virtual CodeInformation Country
		{
			get;
			set;
		}

	}
}

