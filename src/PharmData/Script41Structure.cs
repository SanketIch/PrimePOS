using System;
using System.Collections;
using System.Collections.Specialized;

namespace PharmData
{
	/// <summary>
	/// Summary description for Script41Structure.
	/// </summary>
	internal class Script41Structure
	{
	
		private static Hashtable aHashSegStruct=null;
		private static string[] asDelimeters=null;
		private static ArrayList[] aTransStructs=null;
		
		public enum Script41DelimeterPositions
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

		static Script41Structure()
		{
			SetSegmentStructure();
			SetDelimeters();
			SetTransactionStructure();
		}
		

		public static ArrayList[] GetTransactionStructure()
		{
			return Script41Structure.aTransStructs;
		}

		static void SetTransactionStructure()
		{
			/*
			 * This function will create the transaction
			 * structure for the four different type of
			 * transactions. The key can be used in the future
			 * as field overrides. If a particular 
			 * transaction wants a different set of field.
			 * */		

			aTransStructs=new ArrayList[5]; //each element holds the struct for that transaction.
			
			aTransStructs[0]=new ArrayList(); //password change structure.
			
			aTransStructs[0].Add("UIB");
			aTransStructs[0].Add("UIH");
			aTransStructs[0].Add("REQ");
			aTransStructs[0].Add("UIT");
			aTransStructs[0].Add("UIZ");

			aTransStructs[1]=new ArrayList(); //RefReq structure
			aTransStructs[1].Add("UIB");
			aTransStructs[1].Add("UIH");
			//aTransStructs[1].Add("REQ",null);
			aTransStructs[1].Add("PVDP1");
			aTransStructs[1].Add("PVDPC");
			aTransStructs[1].Add("PVD");
			aTransStructs[1].Add("PTT");			
			aTransStructs[1].Add("DRUP");	
			aTransStructs[1].Add("DRU");		
			//aTransStructs[1].Add("OBS",null);
			//aTransStructs[1].Add("COO",null);
			aTransStructs[1].Add("UIT");
			aTransStructs[1].Add("UIZ");

			aTransStructs[2]=new ArrayList(); //rxchg structure
			aTransStructs[2].Add("UIB");
			aTransStructs[2].Add("UIH");
			aTransStructs[2].Add("REQ");
			aTransStructs[2].Add("PVDPC");
			aTransStructs[2].Add("PVDP1");
			aTransStructs[2].Add("PTT");
			aTransStructs[2].Add("DRUP");						
			aTransStructs[2].Add("DRUR");			
			aTransStructs[2].Add("DRU");
			//aTransStructs[2].Add("OBS",null);
			//aTransStructs[2].Add("COO",null);						
			aTransStructs[2].Add("UIT");
			aTransStructs[2].Add("UIZ");			
			
		
			aTransStructs[3]=new ArrayList(); //rxfill structure			
			aTransStructs[3].Add("UIB");
			aTransStructs[3].Add("UIH");
			//aTransStructs[3].Add("REQ",null);
			aTransStructs[3].Add("RES");
			aTransStructs[3].Add("PVDP1");
			aTransStructs[3].Add("PVDPC");
			aTransStructs[3].Add("PVD");			
			aTransStructs[3].Add("PTT");
			aTransStructs[3].Add("DRUP");
			aTransStructs[3].Add("UIT");
			aTransStructs[3].Add("UIZ");

			aTransStructs[4]=new ArrayList();
			aTransStructs[4].Add("UIB");
			aTransStructs[4].Add("UIH");
			aTransStructs[4].Add("PVDPC"); //prescriber
			aTransStructs[4].Add("PVDP1"); //pharmacist
			aTransStructs[4].Add("PVD");
			aTransStructs[4].Add("PTT");	
			aTransStructs[4].Add("DRUP");	
			aTransStructs[4].Add("DRU");

			



		}


		static void SetDelimeters()
		{
			/*This will set the delimeters for the script41 protocol.
			 * */						
			
			
			asDelimeters=new string[6];
			
			/*
			asDelimeters[0]=":";
			asDelimeters[1]="+";
			asDelimeters[2]=".";
			asDelimeters[3]="/";
			asDelimeters[4]="*";
			asDelimeters[5]="'";
			*/
			
			asDelimeters[0]=((char)28).ToString(); //component data element sep.
			asDelimeters[1]=((char)29).ToString(); //data element sep
			asDelimeters[2]=((char)46).ToString(); //decimal notation
			asDelimeters[3]=((char)32).ToString(); //release indicator
			asDelimeters[4]=((char)31).ToString(); //repetition sep
			asDelimeters[5]=((char)30).ToString(); //segment sep		
			
		}
		
		public static string[] GetDelimeters()
		{
			return Script41Structure.asDelimeters;
		}

		public static Hashtable GetSegmentStructure()
		{
			return Script41Structure.aHashSegStruct;
		}

		static void SetSegmentStructure()
		{
			/*
			 * This will set the segment structure for the
			 * script41 protocol. The field names will
			 * point to the name in the dataset to look 
			 * for that field.
			 * */
			aHashSegStruct=new Hashtable();
	
			string[] asFields;
			
			/*
			 * UIB SEGMENT CREATION.
			 * */
			
			object []asUIB=new object[11];				
			
			asFields=new string[1];			
			asFields[0]="UIBSEGMENTCODE";
			asUIB[0]=asFields;			
			
			asFields=new string[2];
			asFields[0]="UIBSYNTAXIDENTIFIER";			
			asFields[1]="UIBSYNTAXVERSNUMBER";
			asUIB[1]=asFields;
			
			asUIB[2]=null;	//script doesnt use this field.														
			
			asFields=new string[3];
			asFields[0]="UIBTRANSCONTROLREF";
			asFields[1]="UIBINITIATORREFIDENTIFIER";
			asFields[2]="UIBCONTROLLINGAGENCY";			
			asUIB[3]=asFields; 
						
			asUIB[4]=null;
			asUIB[5]=null;

			asFields=new string[4];
			asFields[0]="UIBSENDERID1";
			asFields[1]="UIBSENDERQUALIFIER1";
			asFields[2]=null;
			asFields[3]=null;			
			
			//asFields[2]="UIBPASSWORD";
			//asFields[3]="UIBSENDERID3";			
			
			asUIB[6]=asFields;
			
			asFields=new string[4];
			asFields[0]="UIBRECIPIENTID1";
			asFields[1]="UIBRECIDQUALIFIER1";
			asFields[2]="UIBRECIPIENTID2";
			asFields[3]="UIBRECIPIENTID3";
			asUIB[7]=asFields;

			asFields=new string[2];
			asFields[0]="UIBINITDATE";
			asFields[1]="UIBEVENTTIME";
			asUIB[8]=asFields;
			
			asUIB[9]=null;
			
			asFields=new string[1];
			asFields[0]="UIBTESTINDICATOR";
			asUIB[10]=asFields;
		
			aHashSegStruct.Add("UIB",asUIB);			
			
			/*
			 * End of UIB SEG CREATOR
			 * */

			/*
			 * UIH Segment 
			 * */
			
			object [] asUIH=new object[7];			
			asFields=new string[1];
			
			asFields[0]="UIHSEGMENTCODE";
			asUIH[0]=asFields;
			
			asFields=new string[6];
			asFields[0]="UIHMSGTYPE";
			asFields[1]="UIHMSGVERSNUM";
			asFields[2]="UIHMSGRELEASENUM";
			asFields[3]="UIHMSGFUNCTION";
			asFields[4]=null; //this component is not being used by script.
			asFields[5]="UIHASSOCIATIONASSIGNEDCODE";
			asUIH[1]=asFields;
			
			asFields=new string[1];
			asFields[0]="UIHMSGREFNUM";
			asUIH[2]=asFields;

			asFields=new string[4];
			asFields[0]="UIHINITCTRLREFNUM";
			asFields[1]="UIHINITREFIDENTIFIER";
			asFields[2]="UIHCTRLLINGAGENCY";
			asFields[3]="UIHRESPONDERCTRLREF";
			asUIH[3]=asFields;
			
			asUIH[4]=null;

			asFields=new string[2];
			asFields[0]="UIHINITDATE";
			asFields[1]="UIHEVENTTIME";
			asUIH[5]=null;
				
			asFields=new string[1];
			asFields[0]="UIHTESTINDICATOR";
			asUIH[6]=null;

						
			aHashSegStruct.Add("UIH",asUIH);

			/*
			 * End of UIH Segment.
			 * */

			/*
			 * Create REQ Segment Structure
			 * */

			object[] asREQ=new object[6];
			asFields=new string[1];
			asFields[0]="REQSEGMENTCODE";
			asREQ[0]=asFields;
			asFields=new string[1];
			asFields[0]="REQMSGFUNCCODED";
			asREQ[1]=asFields;			
			asFields=new string[1];
			asFields[0]="REQRETURNRECEIPT";
			asREQ[2]=asFields;			
			asFields=new string[1];
			asFields[0]="REQREFNUMBER";
			asREQ[3]=asFields;			
			asFields=new string[1];
			asFields[0]="REQSENDERID2OLD";
			asREQ[4]=asFields;			
			asFields=new string[1];
			asFields[0]="REQSENDERID2NEW";
			asREQ[5]=asFields;			
			aHashSegStruct.Add("REQ",asREQ);

			/*
			 * End of REQ Segment Structure
			 * */

			object[] asRES=new object[5];		//response segment	
			asFields=new string[1];
			asFields[0]="RESSEGMENTCODE";
			asRES[0]=asFields;			
			asFields=new string[1];
			asFields[0]="RESRESPTYPECODED";
			asRES[1]=asFields;			
			asFields=new string[1];
			asFields[0]="RESCODELISTQUALIFIER";
			asRES[2]=asFields;			
			asFields=new string[1];
			asFields[0]="RESREFNUM";
			asRES[3]=asFields;			
			asFields=new string[1];
			asFields[0]="RESPMSG";
			asRES[4]=asFields;
			aHashSegStruct.Add("RES",asRES);
			
			object[] asSTS=new object[4]; //sts segment
			asFields=new string[1];			
			asFields[0]="STSSEGMENTCODE";
			asSTS[0]=asFields;
			asFields=new string[1];
			asFields[0]="STSSTATUSTYPE";
			asSTS[1]=asFields;			
			asFields=new string[1];
			asFields[0]="STSRESPONSIBILITYQLF";
			asSTS[2]=asFields;			
			asFields=new string[1];
			asFields[0]="STSSTATUSDESCR";
			asSTS[3]=asFields;
			aHashSegStruct.Add("STS",asSTS);


			/*
			object[] asPVD = new object[11];			 
			asFields=new string[1];				//setup the structure for pharmacist loop
			asFields[0]="PVDP1SEGMENTCODE";
			asPVD[0]=asFields;			
			asFields=new string[1];
			asFields[0]="PVDP1PROVIDERCODED";
			asPVD[1]=asFields;			
			asFields=new string[2];
			asFields[0]="PVDP1REFERENCENUMBER";
			asFields[1]="PVDP1REFERENCEQUALIFIER";
			asPVD[2]=asFields;
			asPVD[3]=null;
			asFields=new string[2];
			asFields[0]="PVDP1AGENCYQUALIFIERCODED";
			asFields[1]="PVDP1PROVIDERSPECIALTYCODED";
			asPVD[4]=asFields;
			asFields=new string[2];
			asFields[0]="PVDP1NAME";
			asFields[1]="PVDP1LNAME";
			asPVD[5]=asFields;			
			asPVD[6]=null;			
			asFields=new string[1];
			asFields[0]="PVDP1DNAME";
			asPVD[7]=asFields;
			asFields=new string[4];
			asFields[0]="PVDP1COUNTRYIDENTIFICATION";
			asFields[1]="PVDP1ZIPCODE";
			asFields[2]="PVDP1PLACELOCQUALIFIER";
			asFields[3]="PVDP1PLACELOC";
			asPVD[8]=asFields;			
			asFields=new string[2];
			asFields[0]="PVDP1COMMNUMBER";
			asFields[1]="PVDP1COMMNUMBERQUALIFIER";
			asPVD[9]=asFields;
			asFields=new string[2];
			asFields[0]="PVDP1OTHNAME";
			asFields[1]="PVDP1OTHLNAME";
			asPVD[10]=asFields;
			aHashSegStruct.Add("PVDP1",asPVD);

			asPVD = new object[11];						
			asFields=new string[1];
			asFields[0]="PVDPCSEGMENTCODE";	//setup the segment for prescriber loop
			asPVD[0]=asFields;			
			asFields=new string[1];
			asFields[0]="PVDPCPROVIDERCODED";
			asPVD[1]=asFields;			
			asFields=new string[2];
			asFields[0]="PVDPCREFERENCENUMBER";
			asFields[1]="PVDPCREFERENCEQUALIFIER";
			asPVD[2]=asFields;
			asPVD[3]=null;
			asFields=new string[2];
			asFields[0]="PVDPCAGENCYQUALIFIERCODED";
			asFields[1]="PVDPCPROVIDERSPECIALTYCODED";
			asPVD[4]=asFields;
			asFields=new string[2];
			asFields[0]="PVDPCNAME";
			asFields[1]="PVDPCLNAME";
			asPVD[5]=asFields;			
			asPVD[6]=null;			
			asFields=new string[1];
			asFields[0]="PVDPCDNAME";
			asPVD[7]=asFields;
			asFields=new string[4];
			asFields[0]="PVDPCCOUNTRYIDENTIFICATION";
			asFields[1]="PVDPCZIPCODE";
			asFields[2]="PVDPCPLACELOCQUALIFIER";
			asFields[3]="PVDPCPLACELOC";
			asPVD[8]=asFields;			
			asFields=new string[2];
			asFields[0]="PVDPCCOMMNUMBER";
			asFields[1]="PVDPCCOMMNUMBERQUALIFIER";
			asPVD[9]=asFields;
			asFields=new string[2];
			asFields[0]="PVDPCOTHNAME";
			asFields[1]="PVDPCOTHLNAME";
			asPVD[10]=asFields;
			aHashSegStruct.Add("PVDPC",asPVD);
*/

			object[]asPVD = new object[11];			
			asFields=new string[1];
			asFields[0]="PVDSEGMENTCODE";
			asPVD[0]=asFields;			
			asFields=new string[1];
			asFields[0]="PVDPROVIDERCODED";
			asPVD[1]=asFields;			
			asFields=new string[2];
			asFields[0]="PVDREFERENCENUMBER";
			asFields[1]="PVDREFERENCEQUALIFIER";
			asPVD[2]=asFields;
			asPVD[3]=null;
			asFields=new string[2];
			asFields[0]="PVDAGENCYQUALIFIERCODED";
			asFields[1]="PVDPROVIDERSPECIALTYCODED";
			asPVD[4]=asFields;
			asFields=new string[2];
			asFields[0]="PVDLNAME";
			asFields[1]="PVDNAME";
			asPVD[5]=asFields;			
			asPVD[6]=null;			
			asFields=new string[1];
			asFields[0]="PVDDNAME";
			asPVD[7]=asFields;
			asFields=new string[4];
			
			/*asFields[0]="PVDCOUNTRYIDENTIFICATION";
			asFields[1]="PVDZIPCODE";
			asFields[2]="PVDPLACELOCQUALIFIER";
			asFields[3]="PVDPLACELOC";*/

			asFields[0]="PVDADDRSTR";
			asFields[1]="PVDADDRCT";
			asFields[2]="PVDADDRST";
			asFields[3]="PVDADDRZP";

			
			
			asPVD[8]=asFields;			
			asFields=new string[2];
			asFields[0]="PVDCOMMNUMBER";
			asFields[1]="PVDCOMMNUMBERQUALIFIER";
			asPVD[9]=asFields;
			asFields=new string[2];
			asFields[0]="PVDOTHNAME";
			asFields[1]="PVDOTHLNAME";
			asPVD[10]=asFields;
			aHashSegStruct.Add("PVD",asPVD);
			
			
			
			
			//patient structure.
			object[] asPTT=new object[13];
			
			asFields=new string[1];
			asFields[0]="PTTSEGMENTCODE";
			asPTT[0]=asFields;
			
			asFields=new string[1];
			asFields[0]="PTTRELATIONSHIP";
			asPTT[1]=asFields;
			
			asFields=new string[1];
			asFields[0]="PTTDOB";
			asPTT[2]=asFields;

			asFields=new string[2];
			asFields[0]="PTTLNAME";
			asFields[1]="PTTNAME";
			asPTT[3]=asFields;
			
			asFields=new string[1];
			asFields[0]="PTTGENDER";
			asPTT[4]=asFields;

			asFields=new string[2];
			asFields[0]="PTTPATIENTID";
			asFields[1]="PTTPATIDQLF";
			asPTT[5]=asFields;
			
			asFields=new string[6];
			asFields[0]="PTTADDRESS";
			asFields[1]="PTTADDRESS2";
			asFields[2]="PTTADDRESS3";
			asFields[3]="PTTZIPCODE";
			asFields[4]="PTTADDRESS4";
			asFields[5]="PTTADDRESS5";
			asPTT[6]=asFields;

					
			asFields=new string[2];
			asFields[0]="PTTCOMMUNICATIONNUMBER";
			asFields[1]="PTTCOMMUNICATIONNUMBERQUALIFIER";
			asPTT[7]=asFields;
			
			aHashSegStruct.Add("PTT",asPTT);
			
			object[] asCOO=new object[12];			
			
			asFields=new string[1];
			asFields[0]="COOSEGMENTCODE";
			asCOO[0]=asFields;			
			asFields=new string[2];
			asFields[0]="COOPAYERID";
			asFields[1]="COOPAYERIDQUALIFIER";
			asCOO[1]=asFields;			
			asFields=new string[1];
			asFields[0]="COOPAYERNAME";
			asCOO[2]=asFields;			
			asCOO[3]=null;
			asFields=new string[1];
			asFields[0]="COOCARDHOLDERID";
			asCOO[4]=asFields;			
			asFields=new string[1];
			asFields[0]="COOCARDHOLDERNAME";
			asCOO[5]=asFields;			
			asFields=new string[1];
			asFields[0]="COOGROUPID";			
			asCOO[6]=asFields;			
			asFields=new string[1];
			asFields[0]="COOGROUPNAME";
			asCOO[7]=asFields;			
			asFields=new string[1];
			asFields[0]="COOPAYERSADDRESS";
			asCOO[8]=asFields;			
			
			asCOO[9]=null;
			asCOO[10]=null;
			
			asFields=new string[1];
			asFields[0]="COOCARDHOLDERSADDRESS";
			asCOO[11]=asFields;			
			
			aHashSegStruct.Add("COO",asCOO);

			/*

						object[] asDRU=new object[12];			
			
						asFields=new string[1];
						asFields[0]="DRUPSEGMENTCODE";
						asDRU[0]=asFields;

						asFields=new string[9];
						asFields[0]="DRUPITMDESCIDT";
						asFields[1]="DRUPITMDESC";
						asFields[2]="DRUPITMNUM";
						asFields[3]="DRUPITMNUMQLF";
						asFields[4]="DRUPFORM";
						asFields[5]="DRUPSTRONG";
						asFields[6]="DRUPSTRONGQLF";
						asFields[7]="DRUPREFNUM";		
						asFields[8]="DRUPREFNUMQLF";					
						asDRU[1]=asFields;

						asFields=new string[3];
						asFields[0]="DRUPUM";
						asFields[1]="DRUPQTY_ORD";
						asFields[2]="DRUPQTY_ORDQLF";
						asDRU[2]=asFields;

						asFields=new string[2];
						asFields[0]="DRUPDOSAGEIDENT";
						asFields[1]="DRUPDOSAGE";
						asDRU[3]=asFields;

						asFields=new string[3]; 
						asFields[0]="DRUPDAYTIMEQLF";
						asFields[1]="DRUPDAYTIME";
						asFields[2]="DRUPDAYTIMEFMTQLF";
						asDRU[4]=asFields;

						asFields=new string[1];
						asFields[0]="DRUPPRDSERVSUBSTCODED";
						asDRU[5]=asFields; 
			
						asFields=new string[2];
						asFields[1]="DRUPREFQTY_ORDQLF";
						asFields[0]="DRUPREFQTY_ORD";			
						asDRU[6]=asFields;
	
						asFields=new string[4];
						asFields[0]="DRUPCLINICINFOQLF";
						asFields[1]="DRUPCLINICINFOPRIMARY";
						asFields[2]="DRUPDIAGNOSISQLF";
						asFields[3]="DRUPSECDIAGNOSISQLF";
						asDRU[7]=asFields;
			
						asFields=new string[2];
						asFields[0]="DRUPPRIORAUTHSAMPRXNUM";
						asFields[1]="DRUPPRIORAUTHSAMPRXNUMQLF";
						asDRU[8]=asFields;									
			
						asDRU[9]=null;

						asFields=new string[5];			
						asFields[0]="DRUPDUEREASONSERVCODE";
						asFields[1]="DRUPDUEPROFSERVCD";
						asFields[2]="DRUPDUERESULTOFSERVCD";
						asFields[3]="DRUPDUECOAGENTID";
						asFields[4]="DRUPDUECOAGENTIDQLF";
						asDRU[10]=asFields;
			
						asFields=new string[1];
						asFields[0]="DRUPDRGCOVSTATUSCD";
						asDRU[11]=asFields;
			
						aHashSegStruct.Add("DRUP",asDRU);

						asDRU=new object[12];			
			
						asFields=new string[1];
						asFields[0]="DRURSEGMENTCODE"; //drug request segment.
						asDRU[0]=asFields;

						asFields=new string[9];
						asFields[0]="DRURITMDESCIDT";
						asFields[1]="DRURITMDESC";
						asFields[2]="DRURITMNUM";
						asFields[3]="DRURITMNUMQLF";
						asFields[4]="DRURFORM";
						asFields[5]="DRURSTRONG";
						asFields[6]="DRURSTRONGQLF";
						asFields[7]="DRURREFNUM";		
						asFields[8]="DRURREFNUMQLF";					
						asDRU[1]=asFields;

						asFields=new string[3];
						asFields[0]="DRURUM";
						asFields[1]="DRURQTY_ORD";
						asFields[2]="DRURQTY_ORDQLF";
						asDRU[2]=asFields;

						asFields=new string[2];
						asFields[0]="DRURDOSAGEIDENT";
						asFields[1]="DRURDOSAGE";
						asDRU[3]=asFields;

						asFields=new string[3]; 
						asFields[0]="DRURDAYTIMEQLF";
						asFields[1]="DRURDAYTIME";
						asFields[2]="DRURDAYTIMEFMTQLF";
						asDRU[4]=asFields;

						asFields=new string[1];
						asFields[0]="DRURPRDSERVSUBSTCODED";
						asDRU[5]=asFields; 
			
						asFields=new string[2];
						asFields[0]="DRURREFQTY_ORD";
						asFields[1]="DRURREFQTY_ORDQLF";
						asDRU[6]=asFields;
	
						asFields=new string[4];
						asFields[0]="DRURCLINICINFOQLF";
						asFields[1]="DRURCLINICINFOPRIMARY";
						asFields[2]="DRURDIAGNOSISQLF";
						asFields[3]="DRURSECDIAGNOSISQLF";
						asDRU[7]=asFields;
			
						asFields=new string[2];
						asFields[0]="DRURPRIORAUTHSAMPRXNUM";
						asFields[1]="DRURPRIORAUTHSAMPRXNUMQLF";
						asDRU[8]=asFields;									
			
						asDRU[9]=null;

						asFields=new string[5];			
						asFields[0]="DRURDUEREASONSERVCODE";
						asFields[1]="DRURDUEPROFSERVCD";
						asFields[2]="DRURDUERESULTOFSERVCD";
						asFields[3]="DRURDUECOAGENTID";
						asFields[4]="DRURDUECOAGENTIDQLF";
						asDRU[10]=asFields;
			
						asFields=new string[1];
						asFields[0]="DRURDRGCOVSTATUSCD";
						asDRU[11]=asFields;
			
						aHashSegStruct.Add("DRUR",asDRU);
			*/

			object []asDRU=new object[12];			
			
			asFields=new string[1];
			asFields[0]="DRUSEGMENTCODE";
			asDRU[0]=asFields;

			asFields=new string[9];
			asFields[0]="DRUITMDESCIDT";
			asFields[1]="DRUITMDESC";
			asFields[2]="DRUITMNUM";
			asFields[3]="DRUITMNUMQLF";
			asFields[4]="DRUFORM";
			asFields[5]="DRUSTRONG";
			asFields[6]="DRUSTRONGQLF";
			asFields[7]="DRUREFNUM";		
			asFields[8]="DRUREFNUMQLF";					
			asDRU[1]=asFields;

			asFields=new string[3];
			asFields[0]="DRUUM";
			asFields[1]="DRUQTY_ORD";
			asFields[2]="DRUQTY_ORDQLF";
			asDRU[2]=asFields;

			asFields=new string[3];
			asFields[0]="DRUDOSAGEIDENT";
			asFields[1]="DRUDOSAGE";
			asFields[2]="DRUDOSAGE2";
			asDRU[3]=asFields;

			asFields=new string[3];
			asFields[0]="DRUDAYTIMEQLF";
			asFields[1]="DRUDAYTIME";
			asFields[2]="DRUDAYTIMEFMTQLF";
			asDRU[4]=asFields;

			asFields=new string[1];
			asFields[0]="DRUPRDSERVSUBSTCODED";
			asDRU[5]=asFields;
			
			asFields=new string[3];
			
			asFields[0]="DRUREFQTY_ORDQLF";
			asFields[1]="DRUREFQTY_ORD";			
			asFields[2]="DRUREFQTY_ORDQLF2";
			
			asDRU[6]=asFields;
	
			asFields=new string[4];
			asFields[0]="DRUCLINICINFOQLF";
			asFields[1]="DRUCLINICINFOPRIMARY";
			asFields[2]="DRUDIAGNOSISQLF";
			asFields[3]="DRUSECDIAGNOSISQLF";
			asDRU[7]=asFields;
			
			asFields=new string[2];
			asFields[0]="DRUPRIORAUTHSAMPRXNUM";
			asFields[1]="DRUPRIORAUTHSAMPRXNUMQLF";
			asDRU[8]=asFields;									
			
			asFields=new string[1];
			asFields[0]="DRUSPCMESSAGE";
			asDRU[9]=asFields;

			asFields=new string[5];			
			asFields[0]="DRUDUEREASONSERVCODE";
			asFields[1]="DRUDUEPROFSERVCD";
			asFields[2]="DRUDUERESULTOFSERVCD";
			asFields[3]="DRUDUECOAGENTID";
			asFields[4]="DRUDUECOAGENTIDQLF";
			asDRU[10]=asFields;
			
			asFields=new string[1];
			asFields[0]="DRUDRGCOVSTATUSCD";
			asDRU[11]=asFields;

			aHashSegStruct.Add("DRU",asDRU);

			object[] asOBS=new object[2];			
			asFields=new string[1];
			asFields[0]="OBSSEGMENTCODE";
			asOBS[0]=asFields;						
			asFields=new string[5];
			asFields[0]="OBSMEASUREMENTDIMENSIONCODED";
			asFields[2]="OBSMEASUREMENTUNITQUALIFIER";
			asFields[4]="OBSPERIODFORMATQLF";
			asOBS[1]=asFields;
			aHashSegStruct.Add("OBS",asOBS);
			

			object[] asUIT=new object[3];			
			asFields=new string[1];
			asFields[0]="UITSEGMENTCODE";
			asUIT[0]=asFields;
			asFields=new string[1];
			asFields[0]="UITMSGREFNUM";			
			asUIT[1]=asFields;			
			asFields=new string[1];
			asFields[0]="UITNUMOFSEGS";
			asUIT[2]=asFields;
			aHashSegStruct.Add("UIT",asUIT);

			
			object[] asUIZ=new object[3];						
			asFields=new string[1];
			asFields[0]="UIZSEGMENTCODE";
			
			asUIZ[0]=asFields;			
			
			asUIZ[1]=null;
			
			asFields=new string[1];
			asFields[0]="UIZINTERCHANGECTRLCOUNT";
			asUIZ[2]=asFields;			
			
			aHashSegStruct.Add("UIZ",asUIZ);

		}
		
		
		public Script41Structure()
		{
			//
			// TODO: Add constructor logic here
			//
		}

	}
}
