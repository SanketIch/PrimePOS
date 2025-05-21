// ----------------------------------------------------------------
// Library: Data Access
// Author: Adeel Shehzad.
// Company: D-P-S, Inc. (www.d-p-s.com)
//
// ----------------------------------------------------------------

using System;
using System.Data.SqlClient;
using System.Data;

namespace POS_Core.ErrorLogging
{
	public class POSExceptions : Exception
	{
		private long m_ErrNumber;
		private string m_ErrMessage;

		public POSExceptions(string errMessage, long errNumber):base(errMessage)
		{
			this.m_ErrNumber = errNumber;
			this.m_ErrMessage = errMessage;
		}

		public long ErrNumber
		{
			get 
			{ 
					return this.m_ErrNumber;
			} 
			set { this.m_ErrNumber = value; }
		}

		public string ErrMessage
		{
			get 
			{ 
				return this.m_ErrMessage;
			} 
			set { this.m_ErrMessage = value; }
		}
	}
}
