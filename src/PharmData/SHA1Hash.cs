using System;
using System.Security.Cryptography;

namespace PharmData
{
	/// <summary>
	/// Summary description for SHA1Hash.
	/// </summary>
	internal class SHA1Hash
	{
		public string sData="";

		public SHA1Hash()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	
		public byte[] ComputeHash()
		{
			/*
			 * This function is responsible for computing SHA1 hash. 			 
			 * */
			SHA1 oSha1=SHA1CryptoServiceProvider.Create();			
			System.Text.ASCIIEncoding oEnc=new System.Text.ASCIIEncoding();
			byte[] bData=oEnc.GetBytes(sData);
			byte[] bHashedData;
			bHashedData=oSha1.ComputeHash(bData);
			return bHashedData;
		
		}


	}
}
