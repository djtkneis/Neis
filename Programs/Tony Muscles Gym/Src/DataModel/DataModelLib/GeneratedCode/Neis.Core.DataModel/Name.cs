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
	/// The information for a name
	/// </summary>
	public partial class Name : DataObject
	{
		/// <summary>
		/// Last name
		/// </summary>
		public virtual string Last
		{
			get;
			set;
		}

		/// <summary>
		/// First name
		/// </summary>
		public virtual string First
		{
			get;
			set;
		}

		/// <summary>
		/// Middle name
		/// </summary>
		public virtual string Middle
		{
			get;
			set;
		}

		/// <summary>
		/// Suffix
		/// </summary>
		public virtual string Suffix
		{
			get;
			set;
		}

	}
}

