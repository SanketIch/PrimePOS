using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

namespace PharmData
{
	/// <summary>
	/// Summary description for SegmentStringGenerator.
	/// </summary>
	internal class SegmentStringGenerator
	{
		private enum DelimeterPositions
		{
			/*
			 * Each element in this enumeration represents
			 * the position in the delimeter array. So componentDataElement
			 * is in position 0 in the delimeter array.
			 * */			
			ComponentDataElement,
			DataElement,
			DecimalNotation,
			ReleaseIndicator,
			Repetition,
			Segment
		};


		//holds the delimeters
		private string[] asDelimeters=null;
		
		//holds the segment structure
		private Hashtable htSegStruct=null;
		
		//holds the transaction structure
		private ArrayList htTransStruct=null;

		private DataSet oDsData=null;

		public SegmentStringGenerator()
		{
			
			//
			// TODO: Add constructor logic here
			//
		}
	
		//delimeters
		public string[] Delimeters
		{
			get{return this.asDelimeters;}
			set{this.asDelimeters = value;}
		}

		//segment structure
		public Hashtable SegStruct
		{
			get{return this.htSegStruct;}
			set{this.htSegStruct = value;}

		}

		//the particular transaction structure
		public ArrayList TransStruct
		{
			get{return this.htTransStruct;}
			set{this.htTransStruct = value;}
		}
		
		//holds the data to be used in the dataset.
		public DataSet DsStrData
		{
			get{return this.oDsData;}
			set{this.oDsData = value;}
		}
	
		public string CreateString()
		{
						
			/*This method is responsible for creating the string using the
			 * three structures that are set to the class.
			 * */
			string sRetVal="";
			string sSegVal="";
			string sSegment="";
			System.Data.DataRow oDrRow=null;
			object[] oaSegStruct=null;
			string[] asFieldStruct=null;
			string sFieldVal="";
			int i;
			int j;
			DataTable oDtSegment=null;	
			System.Collections.IEnumerator oEnumerator=this.htTransStruct.GetEnumerator();

			//foreach(string sSegment in this.htTransStruct.Keys)
			while(oEnumerator.MoveNext())
			{
				
				/*
				 * Create the segment.
				 * Please note i am creating the segments backwards
				 * so that the problem of position holders are
				 * automatically resolved.				  
				 * */												
								
				
				//sSegVal=this.asDelimeters[((int)DelimeterPositions.Segment)]; //append the segment seperator.												
				
				sSegment=oEnumerator.Current.ToString();
				
				oaSegStruct=(object[])this.htSegStruct[sSegment];																
				
				if(!oDsData.Tables.Contains(sSegment))
					continue;

				oDtSegment=oDsData.Tables[sSegment];
			
				for(int irow = 0; irow < oDtSegment.Rows.Count; irow++)
				{

					oDrRow=oDtSegment.Rows[irow];
					sSegVal="";

					for(i = oaSegStruct.Length - 1; i >= 0; i--)
					{					
					
						asFieldStruct=(string[])oaSegStruct[i];				
					
						if(asFieldStruct!=null)
						{
							for(j = asFieldStruct.Length - 1; j >= 0; j--)
							{										
						
								if(sFieldVal.Length > 0) //this means there are multiple components.
								{							
									sFieldVal=this.asDelimeters[((int)DelimeterPositions.ComponentDataElement)]+sFieldVal;							
								}																	
							
								if(asFieldStruct[j]!=null && oDtSegment.Columns.Contains(asFieldStruct[j]))
									sFieldVal=oDrRow[asFieldStruct[j]].ToString().Trim()+sFieldVal;																				
					
							}																							
						}
					
					
						//append the field to the segment
						sSegVal=sFieldVal+sSegVal;					
					
						sFieldVal="";				
					
						//if its not the final iteration then append the field seperator.
						if(i>0)
						{						
							if(sSegVal.Length > 0)
							{
								sSegVal=this.asDelimeters[((int)DelimeterPositions.DataElement)]+sSegVal; //put the field seperator
							}				
						}									
																
					}															
					
					sRetVal=sRetVal+sSegVal+this.asDelimeters[((int)DelimeterPositions.Segment)]; //append the segment seperator;
				
				}			
			
			}
			
			return sRetVal;
		
		}						

	}
}
