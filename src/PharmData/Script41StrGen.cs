using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

namespace PharmData
{
	/// <summary>
	/// Summary description for Script41StrGen.
	/// </summary>
	/// 

	internal enum ERxTransType
	{
		/*
			 * This enumeration holds all the possible erx transactions.
			 * */			
		PASCHG, //password change
		REFREQ, //refill requests
		RXCHG, //rx change
		RXFILL, //rxfill.	
		NEWRX, //new rx
	
	};

	internal class Script41StrGen
	{
		
		public  DataSet dsScrDataStruct=null;
		public DateTime oCreated=System.DateTime.MinValue;
		
		private string sPassword="";		
		public bool bScriptXML=true;
		public DataSet ERxRequestData=null;
		public ERxTransType ERxTrans;
			
		static Script41StrGen()
		{			
			
		}

		public Script41StrGen()
		{
			SetDataTable();
		}

		private void SetDataTable()
		{
			/*
			 * The datatable will hold the data to pass on to the string creator
			 * so the string creator will work with this datatable.
			 * */
			
			dsScrDataStruct = new DataSet();
			
			DataTable dtScrDataStruct = new DataTable("UIB");			
			dtScrDataStruct.Columns.Add("UIBSEGMENTCODE");
			dtScrDataStruct.Columns.Add("UIBSYNTAXIDENTIFIER");			
			dtScrDataStruct.Columns.Add("UIBSYNTAXVERSNUMBER");
			dtScrDataStruct.Columns.Add("UIBTRANSCONTROLREF");
			dtScrDataStruct.Columns.Add("UIBINITIATORREFIDENTIFIER");
			dtScrDataStruct.Columns.Add("UIBCONTROLLINGAGENCY");	
			dtScrDataStruct.Columns.Add("UIBSENDERID1");
			dtScrDataStruct.Columns.Add("UIBSENDERQUALIFIER1");
			dtScrDataStruct.Columns.Add("UIBPASSWORD");
			dtScrDataStruct.Columns.Add("UIBSENDERID3");	
			dtScrDataStruct.Columns.Add("UIBRECIPIENTID1");
			dtScrDataStruct.Columns.Add("UIBRECIDQUALIFIER1");
			dtScrDataStruct.Columns.Add("UIBRECIPIENTID2");
			dtScrDataStruct.Columns.Add("UIBRECIPIENTID3");
			dtScrDataStruct.Columns.Add("UIBINITDATE");
			dtScrDataStruct.Columns.Add("UIBEVENTTIME");
			dsScrDataStruct.Tables.Add(dtScrDataStruct);

			dtScrDataStruct=new DataTable("UIH");
			dtScrDataStruct.Columns.Add("UIHSEGMENTCODE");									
			dtScrDataStruct.Columns.Add("UIHMSGTYPE");
			dtScrDataStruct.Columns.Add("UIHMSGVERSNUM");
			dtScrDataStruct.Columns.Add("UIHMSGRELEASENUM");
			dtScrDataStruct.Columns.Add("UIHMSGFUNCTION");
			dtScrDataStruct.Columns.Add("UIHASSOCIATIONASSIGNEDCODE");									
			dtScrDataStruct.Columns.Add("UIHMSGREFNUM");
			dtScrDataStruct.Columns.Add("UIHINITCTRLREFNUM");
			dtScrDataStruct.Columns.Add("UIHINITREFIDENTIFIER");
			dtScrDataStruct.Columns.Add("UIHCTRLLINGAGENCY");
			dtScrDataStruct.Columns.Add("UIHRESPONDERCTRLREF");						
			dtScrDataStruct.Columns.Add("UIHINITDATE");
			dtScrDataStruct.Columns.Add("UIHEVENTTIME");
			dtScrDataStruct.Columns.Add("UIHTESTINDICATOR");						
			dsScrDataStruct.Tables.Add(dtScrDataStruct);


			dtScrDataStruct=new DataTable("REQ");
			dtScrDataStruct.Columns.Add("REQSEGMENTCODE");
			dtScrDataStruct.Columns.Add("REQMSGFUNCCODED");
			dtScrDataStruct.Columns.Add("REQRETURNRECEIPT");
			dtScrDataStruct.Columns.Add("REQREFNUMBER");
			dtScrDataStruct.Columns.Add("REQSENDERID2OLD");
			dtScrDataStruct.Columns.Add("REQSENDERID2NEW");
			dsScrDataStruct.Tables.Add(dtScrDataStruct);

			dtScrDataStruct=new DataTable("RES");
			dtScrDataStruct.Columns.Add("RESSEGMENTCODE");			
			dtScrDataStruct.Columns.Add("RESRESPTYPECODED");			
			dtScrDataStruct.Columns.Add("RESCODELISTQUALIFIER");
			dtScrDataStruct.Columns.Add("RESREFNUM");			
			dsScrDataStruct.Tables.Add(dtScrDataStruct);

			/*
			dtScrDataStruct.Columns.Add("STSSEGMENTCODE");
			dtScrDataStruct.Columns.Add("STSSTATUSTYPE");
			dtScrDataStruct.Columns.Add("STSRESPONSIBILITYQLF");
			dtScrDataStruct.Columns.Add("STSSTATUSDESCR");
			*/
			dtScrDataStruct=new DataTable("PVD");
			dtScrDataStruct.Columns.Add("PVDSEGMENTCODE");
			dtScrDataStruct.Columns.Add("PVDPROVIDERCODED");
			dtScrDataStruct.Columns.Add("PVDREFERENCENUMBER");
			dtScrDataStruct.Columns.Add("PVDREFERENCEQUALIFIER");
			dtScrDataStruct.Columns.Add("PVDAGENCYQUALIFIERCODED");
			dtScrDataStruct.Columns.Add("PVDPROVIDERSPECIALTYCODED");
			dtScrDataStruct.Columns.Add("PVDNAME");
			dtScrDataStruct.Columns.Add("PVDLNAME");
			dtScrDataStruct.Columns.Add("PVDDNAME");
			
			//dtScrDataStruct.Columns.Add("PVDCOUNTRYIDENTIFICATION");
			//dtScrDataStruct.Columns.Add("PVDZIPCODE");
			//dtScrDataStruct.Columns.Add("PVDPLACELOCQUALIFIER");
			//dtScrDataStruct.Columns.Add("PVDPLACELOC");

			dtScrDataStruct.Columns.Add("PVDADDRSTR");
			dtScrDataStruct.Columns.Add("PVDADDRCT");
			dtScrDataStruct.Columns.Add("PVDADDRST");
			dtScrDataStruct.Columns.Add("PVDADDRZP");
			
			dtScrDataStruct.Columns.Add("PVDCOMMNUMBER");
			dtScrDataStruct.Columns.Add("PVDCOMMNUMBERQUALIFIER");
			dtScrDataStruct.Columns.Add("PVDOTHNAME");
			dtScrDataStruct.Columns.Add("PVDOTHLNAME");
			dsScrDataStruct.Tables.Add(dtScrDataStruct);
			/*
						dtScrDataStruct.Columns.Add("PVDPCSEGMENTCODE");	//setup the segment for prescriber loop
						dtScrDataStruct.Columns.Add("PVDPCPROVIDERCODED");
						dtScrDataStruct.Columns.Add("PVDPCREFERENCENUMBER");
						dtScrDataStruct.Columns.Add("PVDPCREFERENCEQUALIFIER");
						dtScrDataStruct.Columns.Add("PVDPCAGENCYQUALIFIERCODED");
						dtScrDataStruct.Columns.Add("PVDPCPROVIDERSPECIALTYCODED");
						dtScrDataStruct.Columns.Add("PVDPCNAME");
						dtScrDataStruct.Columns.Add("PVDPCLNAME");
						dtScrDataStruct.Columns.Add("PVDPCDNAME");
						dtScrDataStruct.Columns.Add("PVDPCCOUNTRYIDENTIFICATION");
						dtScrDataStruct.Columns.Add("PVDPCZIPCODE");
						dtScrDataStruct.Columns.Add("PVDPCPLACELOCQUALIFIER");
						dtScrDataStruct.Columns.Add("PVDPCPLACELOC");
						dtScrDataStruct.Columns.Add("PVDPCCOMMNUMBER");
						dtScrDataStruct.Columns.Add("PVDPCCOMMNUMBERQUALIFIER");
						dtScrDataStruct.Columns.Add("PVDPCOTHNAME");
						dtScrDataStruct.Columns.Add("PVDPCOTHLNAME");
			*/
			
			dtScrDataStruct=new DataTable("PTT");
			dtScrDataStruct.Columns.Add("PTTSEGMENTCODE");
			dtScrDataStruct.Columns.Add("PTTRELATIONSHIP");
			dtScrDataStruct.Columns.Add("PTTDOB");
			dtScrDataStruct.Columns.Add("PTTNAME");
			dtScrDataStruct.Columns.Add("PTTLNAME");
			dtScrDataStruct.Columns.Add("PTTGENDER");
			dtScrDataStruct.Columns.Add("PTTPATIENTID");
			dtScrDataStruct.Columns.Add("PTTPATIDQLF");
			dtScrDataStruct.Columns.Add("PTTADDRESS");
			dtScrDataStruct.Columns.Add("PTTADDRESS2");
			dtScrDataStruct.Columns.Add("PTTADDRESS3");
			dtScrDataStruct.Columns.Add("PTTZIPCODE");
			dtScrDataStruct.Columns.Add("PTTADDRESS4");
			dtScrDataStruct.Columns.Add("PTTADDRESS5");								
			dtScrDataStruct.Columns.Add("PTTCOMMUNICATIONNUMBER");
			dtScrDataStruct.Columns.Add("PTTCOMMUNICATIONNUMBERQUALIFIER");																		
			dsScrDataStruct.Tables.Add(dtScrDataStruct);

			/*
			dtScrDataStruct.Columns.Add("COOSEGMENTCODE");
			dtScrDataStruct.Columns.Add("COOPAYERID");
			dtScrDataStruct.Columns.Add("COOPAYERIDQUALIFIER");
			dtScrDataStruct.Columns.Add("COOPAYERNAME");
			dtScrDataStruct.Columns.Add("COOCARDHOLDERID");
			dtScrDataStruct.Columns.Add("COOCARDHOLDERNAME");
			dtScrDataStruct.Columns.Add("COOGROUPID");			
			dtScrDataStruct.Columns.Add("COOGROUPNAME");
			dtScrDataStruct.Columns.Add("COOPAYERSADDRESS");						
			dtScrDataStruct.Columns.Add("COOCARDHOLDERSADDRESS");
			*/
			
			dtScrDataStruct=new DataTable("DRU");
			dtScrDataStruct.Columns.Add("DRUSEGMENTCODE");			
			dtScrDataStruct.Columns.Add("DRUITMDESCIDT");
			dtScrDataStruct.Columns.Add("DRUITMDESC");
			dtScrDataStruct.Columns.Add("DRUITMNUM");
			dtScrDataStruct.Columns.Add("DRUITMNUMQLF");
			dtScrDataStruct.Columns.Add("DRUFORM");
			dtScrDataStruct.Columns.Add("DRUSTRONG");
			dtScrDataStruct.Columns.Add("DRUSTRONGQLF");
			dtScrDataStruct.Columns.Add("DRUREFNUM");		
			dtScrDataStruct.Columns.Add("DRUREFNUMQLF");					
			dtScrDataStruct.Columns.Add("DRUUM");
			dtScrDataStruct.Columns.Add("DRUQTY_ORD");
			dtScrDataStruct.Columns.Add("DRUQTY_ORDQLF");			
			dtScrDataStruct.Columns.Add("DRUDOSAGEIDENT");
			dtScrDataStruct.Columns.Add("DRUDOSAGE");						
			dtScrDataStruct.Columns.Add("DRUDAYTIMEQLF");
			dtScrDataStruct.Columns.Add("DRUDAYTIME");
			dtScrDataStruct.Columns.Add("DRUDAYTIMEFMTQLF");
			dtScrDataStruct.Columns.Add("DRUPRDSERVSUBSTCODED");
			dtScrDataStruct.Columns.Add("DRUREFQTY_ORD");
			dtScrDataStruct.Columns.Add("DRUREFQTY_ORDQLF");				
			dtScrDataStruct.Columns.Add("DRUCLINICINFOQLF");
			dtScrDataStruct.Columns.Add("DRUCLINICINFOPRIMARY");
			dtScrDataStruct.Columns.Add("DRUDIAGNOSISQLF");
			dtScrDataStruct.Columns.Add("DRUSECDIAGNOSISQLF");
			dtScrDataStruct.Columns.Add("DRUPRIORAUTHSAMPRXNUM");
			dtScrDataStruct.Columns.Add("DRUPRIORAUTHSAMPRXNUMQLF");									
			dtScrDataStruct.Columns.Add("DRUSPCMESSAGE");
			dtScrDataStruct.Columns.Add("DRUDUEREASONSERVCODE");
			dtScrDataStruct.Columns.Add("DRUDUEPROFSERVCD");
			dtScrDataStruct.Columns.Add("DRUDUERESULTOFSERVCD");
			dtScrDataStruct.Columns.Add("DRUDUECOAGENTID");
			dtScrDataStruct.Columns.Add("DRUDUECOAGENTIDQLF");
			dtScrDataStruct.Columns.Add("DRUDRGCOVSTATUSCD");
			dtScrDataStruct.Columns.Add("DRUREFQTY_ORDQLF2");
			
			dsScrDataStruct.Tables.Add(dtScrDataStruct);
			
			/*
			dtScrDataStruct.Columns.Add("DRURSEGMENTCODE"); //drug request segment.
			dtScrDataStruct.Columns.Add("DRURITMDESCIDT");
			dtScrDataStruct.Columns.Add("DRURITMDESC");
			dtScrDataStruct.Columns.Add("DRURITMNUM");
			dtScrDataStruct.Columns.Add("DRURITMNUMQLF");
			dtScrDataStruct.Columns.Add("DRURFORM");
			dtScrDataStruct.Columns.Add("DRURSTRONG");
			dtScrDataStruct.Columns.Add("DRURSTRONGQLF");
			dtScrDataStruct.Columns.Add("DRURREFNUM");		
			dtScrDataStruct.Columns.Add("DRURREFNUMQLF");					
			dtScrDataStruct.Columns.Add("DRURUM");
			dtScrDataStruct.Columns.Add("DRURQTY_ORD");
			dtScrDataStruct.Columns.Add("DRURQTY_ORDQLF");
			dtScrDataStruct.Columns.Add("DRURDOSAGEIDENT");
			dtScrDataStruct.Columns.Add("DRURDOSAGE");
			dtScrDataStruct.Columns.Add("DRURDAYTIMEQLF");
			dtScrDataStruct.Columns.Add("DRURDAYTIME");
			dtScrDataStruct.Columns.Add("DRURDAYTIMEFMTQLF");
			dtScrDataStruct.Columns.Add("DRURPRDSERVSUBSTCODED");
			dtScrDataStruct.Columns.Add("DRURREFQTY_ORD");
			dtScrDataStruct.Columns.Add("DRURREFQTY_ORDQLF");
			dtScrDataStruct.Columns.Add("DRURCLINICINFOQLF");
			dtScrDataStruct.Columns.Add("DRURCLINICINFOPRIMARY");
			dtScrDataStruct.Columns.Add("DRURDIAGNOSISQLF");
			dtScrDataStruct.Columns.Add("DRURSECDIAGNOSISQLF");
			dtScrDataStruct.Columns.Add("DRURPRIORAUTHSAMPRXNUM");
			dtScrDataStruct.Columns.Add("DRURPRIORAUTHSAMPRXNUMQLF");			
			dtScrDataStruct.Columns.Add("DRURDUEREASONSERVCODE");
			dtScrDataStruct.Columns.Add("DRURDUEPROFSERVCD");
			dtScrDataStruct.Columns.Add("DRURDUERESULTOFSERVCD");
			dtScrDataStruct.Columns.Add("DRURDUECOAGENTID");
			dtScrDataStruct.Columns.Add("DRURDUECOAGENTIDQLF");
			dtScrDataStruct.Columns.Add("DRURDRGCOVSTATUSCD");
			*/

			/*
			dtScrDataStruct.Columns.Add("OBSSEGMENTCODE");
			dtScrDataStruct.Columns.Add("OBSMEASUREMENTDIMENSIONCODED");
			dtScrDataStruct.Columns.Add("OBSMEASUREMENTUNITQUALIFIER");
			dtScrDataStruct.Columns.Add("OBSPERIODFORMATQLF");			
			*/

			dtScrDataStruct=new DataTable("UIT");
			dtScrDataStruct.Columns.Add("UITSEGMENTCODE");
			dtScrDataStruct.Columns.Add("UITMSGREFNUM");			
			dtScrDataStruct.Columns.Add("UITNUMOFSEGS");		
			dsScrDataStruct.Tables.Add(dtScrDataStruct);

			dtScrDataStruct=new DataTable("UIZ");
			dtScrDataStruct.Columns.Add("UIZSEGMENTCODE");
			dtScrDataStruct.Columns.Add("UIZINTERCHANGECTRLCOUNT");
			dsScrDataStruct.Tables.Add(dtScrDataStruct);
			

		}

	
		
		private void SetData()
		{
			
			/*
			 *Sets the datatable with data to pass into the string
			 *creation.			 
			* */						
			
		//	Script41StrGen.dsScrDataStruct.Clear();			
			
			DataRow oNewRow;
			
			ArrayList[] aTransStructs=Script41Structure.GetTransactionStructure();

			/*
			 * I didnt pass in business logic objects
			 * because that would have been more time consuming. 
			 * It was better to create a dataset and have
			 * multiple tables in that dataset.
			 * */
			DataRow oDRowData=this.ERxRequestData.Tables[0].Rows[0];
	


		//	oCreated=DateTime.Now;
		//	string sERxRefNo=new Guid(oDRowERxInfo["ERXREFNO"].ToString()).ToString("N");
			
			//SHA1Hash oSha1Hash=new SHA1Hash();
			
			//oSha1Hash.sData = sERxRefNo+Script41StrGen.GetScriptDateTime(oCreated)+oDRowMailBox["PASSWORD"].ToString().TrimEnd();						
			//sPassword=Convert.ToBase64String(oSha1Hash.ComputeHash());

			/*
			 * Set the UIB segment.
			 * */
			oNewRow=dsScrDataStruct.Tables["UIB"].NewRow();
			oNewRow["UIBSEGMENTCODE"]="UIB";
			oNewRow["UIBSYNTAXIDENTIFIER"]="UNOA";			
			oNewRow["UIBSYNTAXVERSNUMBER"]="0";
			oNewRow["UIBTRANSCONTROLREF"]=""; //use the trace number
			oNewRow["UIBINITIATORREFIDENTIFIER"]="";
			oNewRow["UIBCONTROLLINGAGENCY"]="";	
			
			oNewRow["UIBSENDERID1"]="";
			oNewRow["UIBSENDERQUALIFIER1"]="P";			
			//oNewRow["UIBPASSWORD"]=oDRowMailBox["PASSWORD"].ToString().TrimEnd();			
			
			//oNewRow["UIBSENDERID1"]=sPassword;
			

			oNewRow["UIBSENDERID3"]="";	
			
			oNewRow["UIBRECIPIENTID1"]="";	
			oNewRow["UIBRECIDQUALIFIER1"]="";
			
			oNewRow["UIBRECIPIENTID2"]="";
			oNewRow["UIBRECIPIENTID3"]="";
			
			oNewRow["UIBINITDATE"]=oCreated.ToString("yyyyMMdd");
			oNewRow["UIBEVENTTIME"]=oCreated.ToString("HHmmss");
			dsScrDataStruct.Tables["UIB"].Rows.Add(oNewRow);



			oNewRow=dsScrDataStruct.Tables["UIH"].NewRow();			
			oNewRow["UIHSEGMENTCODE"]="UIH";												
			oNewRow["UIHMSGTYPE"]="SCRIPT";			
			oNewRow["UIHMSGVERSNUM"]="004";
			oNewRow["UIHMSGRELEASENUM"]="002";			
		
			if(this.ERxTrans == ERxTransType.PASCHG)
				oNewRow["UIHMSGFUNCTION"]="PASCHG";
			else if(this.ERxTrans == ERxTransType.REFREQ)
				oNewRow["UIHMSGFUNCTION"]="REFREQ";
			else if(this.ERxTrans == ERxTransType.RXCHG)
				oNewRow["UIHMSGFUNCTION"]="RXCHG";
			else if(this.ERxTrans == ERxTransType.RXFILL)
				oNewRow["UIHMSGFUNCTION"]="RXFILL";
			else if(this.ERxTrans == ERxTransType.NEWRX)
				oNewRow["UIHMSGFUNCTION"]="NEWRX";
			else
			{ //unknown msg function
			}			
			
			oNewRow["UIHASSOCIATIONASSIGNEDCODE"]="";												
			
			oNewRow["UIHMSGREFNUM"]=""; //msg reference number 
			//in case of response this will be
			//the original message number
			//in case of request this will be our assigned number.			
			oNewRow["UIHINITCTRLREFNUM"]=""; //if the initiator had a number here i must response back with it.
			oNewRow["UIHINITREFIDENTIFIER"]="";
			oNewRow["UIHCTRLLINGAGENCY"]="";
			oNewRow["UIHRESPONDERCTRLREF"]="";									
			oNewRow["UIHINITDATE"]=DateTime.Today.ToString("yyyyMMdd");	//in the case of response this must be
			//the senders time and date of message
			//in case of request this must be
			//our time and date of message.			
			
			oNewRow["UIHEVENTTIME"]=DateTime.Now.ToString("HHmmss");	//same as uihinitdate		
			oNewRow["UIHTESTINDICATOR"]="";		 				
			dsScrDataStruct.Tables["UIH"].Rows.Add(oNewRow);
			
			
			//will do req segment for change transactions
			//when front end is determined.
		//	oNewRow=dsScrDataStruct.Tables["REQ"].NewRow();
		//	oNewRow["REQSEGMENTCODE"]="REQ";
		//	oNewRow["REQMSGFUNCCODED"]="";
		//	oNewRow["REQRETURNRECEIPT"]="";
		//	oNewRow["REQREFNUMBER"]="";
		//	oNewRow["REQSENDERID2OLD"]="";
		//	oNewRow["REQSENDERID2NEW"]="";
		//	dsScrDataStruct.Tables["REQ"].Rows.Add(oNewRow);
			
			/*
			 * When doing a rxfill transaction
			 * we have to put AH etc qualifier for
			 * rescodelistqualifier based on fill information			 
			 * */
			
	//		oNewRow=dsScrDataStruct.Tables["RES"].NewRow();			
	//		oNewRow["RESSEGMENTCODE"]="RES";			
	//		oNewRow["RESRESPTYPECODED"]="";			
			
	//		if(this.ERxTrans == ERxTransType.RXFILL)
	//			oNewRow["RESCODELISTQUALIFIER"]="";
			
	//		oNewRow["RESREFNUM"]="";	
	//		dsScrDataStruct.Tables["RES"].Rows.Add(oNewRow);

			/*
			oNewRow["STSSEGMENTCODE"];
			oNewRow["STSSTATUSTYPE"];
			oNewRow["STSRESPONSIBILITYQLF"];
			oNewRow["STSSTATUSDESCR"];
			*/

			if(aTransStructs[((int)this.ERxTrans)].IndexOf("PVDP1")!=-1)
			{
				oNewRow=dsScrDataStruct.Tables["PVD"].NewRow();
				oNewRow["PVDSEGMENTCODE"]="PVD";
				oNewRow["PVDPROVIDERCODED"]="P2";			
				oNewRow["PVDREFERENCENUMBER"]="";
				oNewRow["PVDREFERENCEQUALIFIER"]="D3"; //qualifier for nabp 
				oNewRow["PVDAGENCYQUALIFIERCODED"]="";
				oNewRow["PVDPROVIDERSPECIALTYCODED"]="";
				oNewRow["PVDNAME"]="";
				oNewRow["PVDLNAME"]="";
				oNewRow["PVDDNAME"]="";
			

				oNewRow["PVDADDRSTR"]="";
				oNewRow["PVDADDRCT"]="";
				oNewRow["PVDADDRST"]="";
				oNewRow["PVDADDRZP"]="";


				//oNewRow["PVDCOUNTRYIDENTIFICATION"]="";
				//oNewRow["PVDZIPCODE"]="";
				//oNewRow["PVDPLACELOCQUALIFIER"]="";
				//oNewRow["PVDPLACELOC"]="";
			
				
				oNewRow["PVDCOMMNUMBER"]="";
				oNewRow["PVDCOMMNUMBERQUALIFIER"]="TE";
				oNewRow["PVDOTHNAME"]="";
				oNewRow["PVDOTHLNAME"]="";
				dsScrDataStruct.Tables["PVD"].Rows.Add(oNewRow);			
			
			}
			
			if(aTransStructs[((int)this.ERxTrans)].IndexOf("PVDPC")!=-1)
			{
				oNewRow=dsScrDataStruct.Tables["PVD"].NewRow();
				oNewRow["PVDSEGMENTCODE"]="PVD";	//setup the segment for prescriber loop
				oNewRow["PVDPROVIDERCODED"]="PC";
				
				if(oDRowData["PRESLIC"]!=null)
				{
					//oNewRow["PVDREFERENCENUMBER"]=oDRowPrs["PRESLIC"].ToString().TrimEnd();
					//oNewRow["PVDREFERENCEQUALIFIER"]="0B";				
					oNewRow["PVDREFERENCENUMBER"]=oDRowData["PRESLIC"].ToString().Trim();
					oNewRow["PVDREFERENCEQUALIFIER"]="SPI";					
				
				}
				else
				{
				
					oNewRow["PVDREFERENCENUMBER"]="";
					oNewRow["PVDREFERENCEQUALIFIER"]="";
				
				}
												
				oNewRow["PVDAGENCYQUALIFIERCODED"]="";
				oNewRow["PVDPROVIDERSPECIALTYCODED"]="";
				oNewRow["PVDNAME"]=oDRowData["PRESFNM"].ToString().TrimEnd();
				oNewRow["PVDLNAME"]=oDRowData["PRESLNM"].ToString().TrimEnd();
				
				
				oNewRow["PVDDNAME"]="";
				
				//		oNewRow["PVDCOUNTRYIDENTIFICATION"]="";
				
					
				
				//oNewRow["PVDPLACELOCQUALIFIER"]="";
				
				//if(oDRowData["ADDRSTR"]==null)
				//	oNewRow["PVDADDRSTR"]="";
				//else
				
				oNewRow["PVDADDRSTR"]="";
				
				
				oNewRow["PVDADDRCT"]="";


				oNewRow["PVDADDRST"]=oDRowData["PRESSTATE"].ToString().TrimEnd();

				//if(oDRowPrs["ADDRZP"]==null)
				//	oNewRow["PVDADDRZP"]="";
				//else
				
				oNewRow["PVDADDRZP"]="";



				if(oDRowData["PRESPHONE"]!=null)
				{
					oNewRow["PVDCOMMNUMBER"]=oDRowData["PRESPHONE"].ToString().TrimEnd();
					oNewRow["PVDCOMMNUMBERQUALIFIER"]="TE";	
				}
				else
				{
					oNewRow["PVDCOMMNUMBER"]="";
					oNewRow["PVDCOMMNUMBERQUALIFIER"]="";
				}
				
				oNewRow["PVDOTHNAME"]="";
				oNewRow["PVDOTHLNAME"]="";
				
				dsScrDataStruct.Tables["PVD"].Rows.Add(oNewRow);			
			
			}
			
			//patient segment 
			oNewRow=dsScrDataStruct.Tables["PTT"].NewRow();			
			oNewRow["PTTSEGMENTCODE"]="PTT";						

			/*
			if(oDRowPat["RELATION"]!=null)
			{
				if(oDRowPat["RELATION"].ToString()=="H")
					oNewRow["PTTRELATIONSHIP"]="1";
				else if(oDRowPat["RELATION"].ToString()=="S")
					oNewRow["PTTRELATIONSHIP"]="2";
				else if(oDRowPat["RELATION"].ToString()=="C")
					oNewRow["PTTRELATIONSHIP"]="3";
				else
					oNewRow["PTTRELATIONSHIP"]="4";
			}
			else*/

			oNewRow["PTTRELATIONSHIP"]="";

			if(oDRowData["DOB"]!=null)
			{				
				oNewRow["PTTDOB"]=Convert.ToDateTime(oDRowData["DOB"]).ToString("yyyyMMdd");
			}						
			
			oNewRow["PTTNAME"]=oDRowData["FNAME"].ToString().TrimEnd();
			oNewRow["PTTLNAME"]=oDRowData["LNAME"].ToString().TrimEnd();
			oNewRow["PTTGENDER"]=oDRowData["SEX"].ToString();
			oNewRow["PTTPATIENTID"]=oDRowData["PATIENTNO"].ToString().TrimEnd();
			oNewRow["PTTPATIDQLF"]="94"; //this states its payer id number.			
			

			if(oDRowData["ADDRSTR"]!=null)
				oNewRow["PTTADDRESS"]=oDRowData["ADDRSTR"].ToString();
			else
				oNewRow["PTTADDRESS"]="";
			
										
			if(oDRowData["ADDRCT"]!=null)
				oNewRow["PTTADDRESS2"]=oDRowData["ADDRCT"].ToString();
			else
				oNewRow["PTTADDRESS2"]="";
			
			if(oDRowData["ADDRST"]!=null)
				oNewRow["PTTADDRESS3"]=oDRowData["ADDRST"].ToString();
			else 
				oNewRow["PTTADDRESS3"]="";
			
			if(oDRowData["ADDRZP"]!=null)
				oNewRow["PTTZIPCODE"]=oDRowData["ADDRZP"].ToString();						
			else
				oNewRow["PTTZIPCODE"]="";
			
			oNewRow["PTTADDRESS4"]="";
			oNewRow["PTTADDRESS5"]="";											
			
			if(oDRowData["PHONE"]!=null)
			{
				oNewRow["PTTCOMMUNICATIONNUMBER"]=oDRowData["PHONE"].ToString();
				oNewRow["PTTCOMMUNICATIONNUMBERQUALIFIER"]="TE";																		
			}
			else
			{
				oNewRow["PTTCOMMUNICATIONNUMBER"]="";
				oNewRow["PTTCOMMUNICATIONNUMBERQUALIFIER"]="";	
			}

			dsScrDataStruct.Tables["PTT"].Rows.Add(oNewRow);

			/*
			oNewRow["COOSEGMENTCODE"];
			oNewRow["COOPAYERID"];
			oNewRow["COOPAYERIDQUALIFIER"];
			oNewRow["COOPAYERNAME"];
			oNewRow["COOCARDHOLDERID"];
			oNewRow["COOCARDHOLDERNAME"];
			oNewRow["COOGROUPID"];			
			oNewRow["COOGROUPNAME"];
			oNewRow["COOPAYERSADDRESS"];						
			oNewRow["COOCARDHOLDERSADDRESS"];
			*/

			if(aTransStructs[((int)this.ERxTrans)].IndexOf("DRUP")!=-1)
			{
				oNewRow=dsScrDataStruct.Tables["DRU"].NewRow();
				
				oNewRow["DRUSEGMENTCODE"]="DRU";

				oNewRow["DRUITMDESCIDT"]="P";
				
				oNewRow["DRUITMDESC"]=oDRowData["DRGNAME"].ToString().TrimEnd()+" "+oDRowData["STRONG"].ToString().TrimEnd()+" "+oDRowData["FORM"].ToString().TrimEnd();
				
				oNewRow["DRUITMNUM"]=oDRowData["DRGNDC"].ToString();
				
				oNewRow["DRUITMNUMQLF"]="ND";
				
				oNewRow["DRUFORM"]="";
		
				
				oNewRow["DRUSTRONG"]="";
				oNewRow["DRUSTRONGQLF"]="";				
																				
				oNewRow["DRUREFNUM"]="";		
				oNewRow["DRUREFNUMQLF"]="";									
								
				oNewRow["DRUSPCMESSAGE"]="";
				
				if(oDRowData["COMMENT"].ToString().Trim().Length > 0)
					oNewRow["DRUSPCMESSAGE"]=oDRowData["COMMENT"].ToString().Trim()+"\n";

				if(oDRowData["DELIVTO"].ToString().Trim().Length > 0)
					oNewRow["DRUSPCMESSAGE"]=oNewRow["DRUSPCMESSAGE"].ToString()+"Delivery To: "+oDRowData["DELIVTO"].ToString()+"\n";
			
				if(oDRowData["DELIVPH"].ToString().Trim().Length > 0)
					oNewRow["DRUSPCMESSAGE"]=oNewRow["DRUSPCMESSAGE"].ToString()+"Delivery Phone: "+oDRowData["DELIVPH"].ToString();


				//if(oDRowDru["UM"]==null || oDRowDru["UM"].ToString().TrimEnd().Length == 0)
					oNewRow["DRUUM"]="EA";
			//	else
					//oNewRow["DRUUM"]=oDRowDru["UM"].ToString();
				
				
				oNewRow["DRUQTY_ORD"]=oDRowData["QUANT"].ToString().TrimEnd();
				oNewRow["DRUQTY_ORDQLF"]="38";			//code for original qty
				
				oNewRow["DRUDOSAGEIDENT"]="";
				
				oNewRow["DRUDOSAGE"]=oDRowData["SIGLINE1"].ToString().TrimEnd()+" "+oDRowData["SIGLINE2"].ToString().TrimEnd()+" "+oDRowData["SIGLINE3"].ToString().TrimEnd()+" "+oDRowData["SIGLINE4"].ToString().TrimEnd();						
				
				oNewRow["DRUDAYTIMEQLF"]="85";
				oNewRow["DRUDAYTIME"]=Convert.ToDateTime(oDRowData["DATE_ORD"]).ToString("yyyyMMdd");
				oNewRow["DRUDAYTIMEFMTQLF"]="102"+((char)31).ToString()+"ZDS"+((char)28).ToString()+oDRowData["DAYS"].ToString().TrimEnd()+((char)28).ToString()+"804";
												
				string sDaw=oDRowData["BRAND"].ToString().TrimEnd();
				if(sDaw=="N")
					sDaw="0";
				else if(sDaw=="Y")
					sDaw="1";
				else {}
				
				oNewRow["DRUPRDSERVSUBSTCODED"]=sDaw;
				
				oNewRow["DRUREFQTY_ORDQLF"]="R";
				
				oNewRow["DRUREFQTY_ORD"]=oDRowData["REFILLSNUM"].ToString().TrimEnd();								
				


				oNewRow["DRUREFQTY_ORDQLF2"]="R";
				
				oNewRow["DRUCLINICINFOQLF"]="";
				oNewRow["DRUCLINICINFOPRIMARY"]="";
				oNewRow["DRUDIAGNOSISQLF"]="";
				oNewRow["DRUSECDIAGNOSISQLF"]="";
				oNewRow["DRUPRIORAUTHSAMPRXNUM"]="";
				oNewRow["DRUPRIORAUTHSAMPRXNUMQLF"]="";									
				oNewRow["DRUDUEREASONSERVCODE"]="";
				oNewRow["DRUDUEPROFSERVCD"]="";
				oNewRow["DRUDUERESULTOFSERVCD"]="";
				oNewRow["DRUDUECOAGENTID"]="";
				oNewRow["DRUDUECOAGENTIDQLF"]="";
				oNewRow["DRUDRGCOVSTATUSCD"]="";			
				


				dsScrDataStruct.Tables["DRU"].Rows.Add(oNewRow);
			
			}



			if(aTransStructs[((int)this.ERxTrans)].IndexOf("DRUR")!=-1)
			{
				
				DataTable oDrugReqs=this.ERxRequestData.Tables["DRUGSEL"];
				
				DataRow oDrTemp=null;
				
				for(int i = 0 ; i < oDrugReqs.Rows.Count; i++)
				{					
				
					oNewRow=dsScrDataStruct.Tables["DRU"].NewRow();					
					
					oDrTemp=oDrugReqs.Rows[i];
					
					oNewRow["DRUSEGMENTCODE"]="DRU"; //drug request segment.
					oNewRow["DRUITMDESCIDT"]="R";
					oNewRow["DRUITMDESC"]=oDrTemp["DRGNAME"].ToString().TrimEnd();
					oNewRow["DRUITMNUM"]=oDrTemp["DRGNDC"].ToString().TrimEnd();
					oNewRow["DRUITMNUMQLF"]="ND";
					oNewRow["DRUFORM"]="";
				
					if(oDrTemp["STRONG"]!=null)
					{
						oNewRow["DRUSTRONG"]=oDrTemp["STRONG"].ToString();
						oNewRow["DRUSTRONGQLF"]="";				
					}
					else
					{
						oNewRow["DRUSTRONG"]="";
						oNewRow["DRUSTRONGQLF"]="";
					}								

					
					
					oNewRow["DRUSTRONGQLF"]="";
					oNewRow["DRUREFNUM"]="";		
					oNewRow["DRUREFNUMQLF"]="";					
					oNewRow["DRUUM"]="";
					oNewRow["DRUQTY_ORD"]="";
					oNewRow["DRUQTY_ORDQLF"]="";
					oNewRow["DRUDOSAGEIDENT"]="";
					oNewRow["DRUDOSAGE"]="";
					oNewRow["DRUDAYTIMEQLF"]="";
					oNewRow["DRUDAYTIME"]="";
					oNewRow["DRUDAYTIMEFMTQLF"]="";
					oNewRow["DRUPRDSERVSUBSTCODED"]="";
					oNewRow["DRUREFQTY_ORD"]="";
					oNewRow["DRUREFQTY_ORDQLF"]="";
					oNewRow["DRUCLINICINFOQLF"]="";
					oNewRow["DRUCLINICINFOPRIMARY"]="";
					oNewRow["DRUDIAGNOSISQLF"]="";
					oNewRow["DRUSECDIAGNOSISQLF"]="";
					oNewRow["DRUPRIORAUTHSAMPRXNUM"]="";
					oNewRow["DRUPRIORAUTHSAMPRXNUMQLF"]="";			
					oNewRow["DRUDUEREASONSERVCODE"]="";
					oNewRow["DRUDUEPROFSERVCD"]="";
					oNewRow["DRUDUERESULTOFSERVCD"]="";
					oNewRow["DRUDUECOAGENTID"]="";
					oNewRow["DRUDUECOAGENTIDQLF"]="";
					oNewRow["DRUDRGCOVSTATUSCD"]="";
					
					dsScrDataStruct.Tables["DRU"].Rows.Add(oNewRow);				

				}
			}
			
			/*
			oNewRow["OBSSEGMENTCODE"];
			oNewRow["OBSMEASUREMENTDIMENSIONCODED"];
			oNewRow["OBSMEASUREMENTUNITQUALIFIER"];
			oNewRow["OBSPERIODFORMATQLF"];			
			*/
			
			oNewRow=dsScrDataStruct.Tables["UIT"].NewRow();
			
			oNewRow["UITSEGMENTCODE"]="UIT";
			
			oNewRow["UITMSGREFNUM"]="";			//this will hold the msg ref num.			
						
			/*
			 * The number of segments
			 * in the message including UIT and UIH			 
			 * but not including UIZ,UNA and UIB
			 * */
			oNewRow["UITNUMOFSEGS"]=Convert.ToString((aTransStructs[((int)this.ERxTrans)].Count-2));		
			dsScrDataStruct.Tables["UIT"].Rows.Add(oNewRow);
			

			//UIZ Segment Setup.
			oNewRow=dsScrDataStruct.Tables["UIZ"].NewRow();
			
			oNewRow["UIZSEGMENTCODE"]="UIZ";
			oNewRow["UIZINTERCHANGECTRLCOUNT"]="1";
			
			dsScrDataStruct.Tables["UIZ"].Rows.Add(oNewRow);
			

			
		}

		private static  string GetScriptDateTime(DateTime oDateTime)
		{
			return oDateTime.ToString("yyyy-MM-dd")+"T"+oDateTime.ToString("hh:mm:ss")+"Z";
			//return System.Xml.XmlConvert.ToDateTime(oDateTime.ToShortDateString()).ToString();

		}


		public string CreateString()
		{
			/*
			 * This function will use segmentstring generator
			 * to generator the string. First it will mold the data
			 * set according to its own structure.
			 * */			
			string sRetVal="";
			string sUNA="";			
			SegmentStringGenerator oSegStrGen=new SegmentStringGenerator();			
			string[] asDelimeters=Script41Structure.GetDelimeters();
			this.SetData();
			oSegStrGen.Delimeters = asDelimeters;			
			oSegStrGen.DsStrData = this.dsScrDataStruct;
			
			oSegStrGen.SegStruct = Script41Structure.GetSegmentStructure();
			
			oSegStrGen.TransStruct = Script41Structure.GetTransactionStructure()[((int)ERxTrans)];						

			sRetVal=oSegStrGen.CreateString();									

			//append the UNA segment. UNA segment is structured a little differently.
			sUNA="UNA";
			for(int i = 0; i < asDelimeters.Length;i++)
			{	
				sUNA+=asDelimeters[i];
			}
			


			sRetVal=(sUNA+sRetVal);

			////Script requires data sent to them as XML.
		//	if(bScriptXML)
			//{
			//	ScriptXMLCreator oSCreator=new ScriptXMLCreator();
			//	sRetVal=oSCreator.Create(sRetVal,this.ERxRequestData,Script41StrGen.oCreated,sPassword).OuterXml;					
			//}
						
			return sRetVal;		
		
		}

	}
}
