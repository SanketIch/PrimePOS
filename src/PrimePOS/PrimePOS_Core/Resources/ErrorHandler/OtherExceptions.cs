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
	public class OtherExceptions : Exception
	{

		public OtherExceptions(Exception ex):base(ex.Message,ex)
		{
		}
	}
}
