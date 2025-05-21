using System;
using System.Collections;
using System.Data;
using CbDbf;

namespace PharmData
{
	/// <summary>
	/// Summary description for PharmFpDB.
	/// </summary>
	internal class PharmFpDB
	{
		private string sPatient="";
		private string sClaims="";
		private string sPrescrib="";
		private string sFAQ="";
		private string sFacility="";		
		private string sWebLogin="";
		private string sWebDrugs="";
		private string sDrug="";
		private string sNewRxOrd="";
		private string sWebSet="";
		private string sDf0001="";
		private string sPhUser="";
		private string sRxRefQue="";
		private string sConstant="";
		//private string sPatDoc="";
		private string sInsFile="";
		private string sWebContact="";							
		private string sPatPres="";
		private string sAccess="";
		private string sRxSig="";
		private string sAdvImg="";
		private string sWebInfo="";
		private string sPcLong="";
		private string sPcShort="";
		private string sExtrnPat="";


		private ArrayList oRxFields=null;
		private ArrayList oPatFields=null;
		private ArrayList oPresFields=null;
		private ArrayList oPatHistFields=null;

		public PharmFpDB()
		{
			//
			// TODO: Add constructor logic here
			//
			oPatFields=this.GetPatientFields();
			oPresFields=this.GetDoctorFields();
			this.oPatHistFields = this.GetPatHistFields();
			oRxFields=this.GetRxFields();
		}
			
		public ArrayList GetRxFields()
		{
			ArrayList oAl=new ArrayList();
			oAl.Add("PATIENTNO");
			oAl.Add("PATTYPE");
			oAl.Add("RXNO");
			oAl.Add("PRESNO");
			oAl.Add("NDC");
			oAl.Add("DRGNAME");
			oAl.Add("DATEO");
			oAl.Add("DATEF");
			oAl.Add("QUANT");
			oAl.Add("QTY_ORD");
			oAl.Add("DAYS");
			oAl.Add("TREFILLS");
			oAl.Add("NREFILL");
			return oAl;
		}


		public string Patient
		{
			get{return this.sPatient;}
			set{this.sPatient = value;}
		}

		public string Claims
		{	
			get{return this.sClaims;}
			set{this.sClaims = value;}
		}

		public string Prescrib
		{
			get{return this.sPrescrib;}
			set{this.sPrescrib = value;}		
		}
		
		public string Constant
		{
			get{return this.sConstant;}
			set{this.sConstant = value;}
		}

		public string ExtrnPat
		{
			get{return this.sExtrnPat;}
			set{this.sExtrnPat = value;}
		}

		public string FAQ
		{
			get{return this.sFAQ;}
			set{this.sFAQ = value;}
		}

		public string Facility
		{
			get{return this.sFacility;}
			set{this.sFacility = value;}
		}

		public string WebLogin
		{
			get{return this.sWebLogin;}
			set{this.sWebLogin = value;}
		}

		public string PatPres
		{
			get{return this.sPatPres;}
			set{this.sPatPres = value;}
		}

		public string WebDrugs
		{
			get{return this.sWebDrugs;}
			set{this.sWebDrugs = value;}
		}
		
		public string InsFile
		{
			get{return this.sInsFile;}
			set{this.sInsFile = value;}
		}

		public string Drug
		{
			get{return this.sDrug;}
			set{this.sDrug = value;}
		}

		public string NewRxOrd
		{
			get{return this.sNewRxOrd;}
			set{this.sNewRxOrd=value;}
		}

		public string WebSet
		{
			get{return this.sWebSet;}
			set{this.sWebSet=value;}
		}
	
		public string AdvImg
		{
			get{return this.sAdvImg;}
			set{this.sAdvImg = value;}
		}

		public string RxSig
		{
			get{return this.sRxSig;}
			set{this.sRxSig = value;}

		}

		public string WebInfo
		{
			get{return this.sWebInfo;}
			set{this.sWebInfo=value;}
		}

		public string Df0001
		{
			get{return this.sDf0001;}
			set{this.sDf0001 = value;}
		}

		public string PhUser
		{
			get{return this.sPhUser;}
			set{this.sPhUser = value;}
		}

		public string RxRefQue
		{
			get{return this.sRxRefQue;}
			set{this.sRxRefQue = value;}
		}

		public string WebContact
		{
			get{return this.sWebContact;}
			set{this.sWebContact = value;}
		}

		public string Access
		{
			get{return this.sAccess;}
			set{this.sAccess = value;}
		}

		public string PcLong
		{
			get{return this.sPcLong;}
			set{this.sPcLong= value;}
		}

		public string PcShort
		{
			get{return this.sPcShort;}
			set{this.sPcShort = value;}
		}

		public DataTable GetCounseling(string sNdc,string sType)
		{
			/*
			 * This function is responsible for returning the counseling
			 * it will return short if stype is "S" and long if
			 * stype is	"L"
			 * */
			try
			{
				
				DataTable oDtTemp=this.GetDrug(sNdc);
				
				string sTableName=this.sPcShort;
				
				if(oDtTemp==null || oDtTemp.Rows.Count == 0 || oDtTemp.Rows[0]["TXRCODE"].ToString().TrimEnd()=="")
					return null;
			
				if(sType=="L")
					sTableName=this.sPcLong;

				oDtTemp=this.GetRecs(sTableName,oDtTemp.Rows[0]["TXRCODE"].ToString(),"TXRCODE","TXRCODE",null);
				
				if(oDtTemp.Rows.Count==0)
					return null;

				return oDtTemp;
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}



		public DataTable GetDoctor(string sDocNo)
		{
			try
			{				
				return this.GetRecs(this.sPrescrib,sDocNo,"PRESNO","PRESNO",this.oPresFields);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public DataTable GetDrug(string sNdc)
		{
			try
			{
				return this.GetRecs(this.sDrug,sNdc,"DRGNDC","DRGNDC",null);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		
		public DataTable GetAdvImg(string sAdvImg)
		{
			try
			{
				if(sAdvImg=="-1")
					return this.GetAll(this.sAdvImg,null);
				else
					return this.GetRecs(this.sAdvImg,sAdvImg,"IMGID","IMGID",null);						
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		
	

		public DataTable GetWebDrug(string sNdc)
		{
			try
			{
				if(sNdc=="-1")
				{
					return this.GetAll(this.sWebDrugs,null);
				}
				else
				{
					return this.GetRecs(this.WebDrugs,sNdc,"DRGNDC","DRGNDC",null);
				}						
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public ArrayList GetPatHistFields()
		{
			ArrayList oAl=new ArrayList();
			oAl.Add("PATIENTNO");
			oAl.Add("PATTYPE");
			oAl.Add("BILLTYPE");
			oAl.Add("RXNO");
			oAl.Add("STATUS");
			oAl.Add("PRESNO");
			oAl.Add("NDC");
			oAl.Add("DRGNAME");
			oAl.Add("BRAND");
			oAl.Add("DATEO");
			oAl.Add("DATEF");
			oAl.Add("QUANT");
			oAl.Add("QTY_ORD");
			oAl.Add("DAYS");
			oAl.Add("ORDSTATUS");
			oAl.Add("TREFILLS");
			oAl.Add("NREFILL");
			oAl.Add("AMOUNT");
			oAl.Add("PFEE");
			oAl.Add("DISCOUNT");
			oAl.Add("COPAY");
			oAl.Add("TOTAMT");
			oAl.Add("SIG");
			oAl.Add("PHARMACIST");										
			return oAl;
		}



		public DataTable GetInsInfo(string sInsType)
		{
			/*
				 * This function will return the information of a given 
				 * insurance type. I am only returning back necessary fields.
				 * Feel free to return back more information about an
				 * insurance.
				 * */									
			DataTable oInsTable=new DataTable();
			CCbDbf oIns=null;
			bool bRetVal=false;						
			try
			{				
				if(OpenFile(out oIns,this.sInsFile))
				{
					//I am searching manually because inscar uses
					//an idx and i will research furthur how to
					//work with an idx.
					oIns.db.top();					
					//sInsType=sInsType.Trim();
					while(oIns.db.eof()==0)
					{
						if(oIns.db.deleted()==0)
						{
							if(oIns.GetValString("IC_CODE").Trim()==sInsType)
							{													
								bRetVal=true;
								break;
							}
						}
						oIns.db.skip();
					}
					if(bRetVal)
					{
						oInsTable.Columns.Add("MDREFILL");
						DataRow oRow=oInsTable.NewRow();
						oRow["MDREFILL"]=oIns.GetValString("MDREFILL").Trim();
						oInsTable.Rows.Add(oRow);						
					}																									
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Error In Getting Data "+ex.Message);
			}			
			finally
			{	
				CloseFile(oIns);
			}
			
			return oInsTable;
		}

		public void Hit()
		{
			ArrayList oAl=new ArrayList();
			oAl.Add("WEBHITS");				
			CCbDbf oFile=null;			
			try
			{
				if(this.OpenFile(out oFile,this.sWebInfo))
				{					
					oFile.db.top();
					oFile.db.lockFile();					
					string sHit=oFile.GetValString("WEBHITS");
					sHit=Convert.ToString(Convert.ToInt32(sHit)+1);					
					sHit=sHit.PadLeft(Convert.ToInt32(oFile.GetFieldLength("WEBHITS")),' ');							
					oFile.GetValField4("WEBHITS").assign(sHit);
					oFile.db.unlock();										
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				CloseFile(oFile);
			}		
		}

		public DataTable GetConstant()
		{
			try
			{
				return this.GetAll(this.sConstant,null);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}	

		public DataTable GetWebDrugName(string sName)
		{
			try
			{
				return this.GetName(this.sWebDrugs,"DRGNAME",sName,"","DRGNAME","",null,false,"","");
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public DataTable GetPhDrug(string sNdc)
		{
			try
			{
				return this.GetRecs(this.sDrug,sNdc,"DRGNDC","DRGNDC",null);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public DataTable GetRxsByPatient(string sPatNo)
		{
			try
			{
				string sTarget=sPatNo.PadLeft(12,' ');
				return this.GetRecs(this.sClaims,sTarget,"CLMMEDNO","PATIENTNO",null);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public DataTable GetPhDrugName(string sName)
		{
			try
			{
				return this.GetName(this.sDrug,"DRGNAME",sName,"","DRGNAME","",null,false,"","");
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}


		public DataTable GetFacility(string sFacCode)
		{
			try
			{
				if(sFacCode.Equals("-1"))
					return this.GetAll(this.sFacility,null);
				else
					return this.GetRecs(this.sFacility,sFacCode,"FACCODE","FACILITYCD",null);
						
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public DataTable GetPhInfo()
		{
			try
			{
				return this.GetAll(this.sDf0001,null);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public DataTable GetPhUser(string sPhInit)
		{
			try
			{
				return this.GetRecs(this.sPhUser,sPhInit,"PHINIT","PH_INIT",null);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		
		public DataTable GetWebLoginByLoginType(string sLoginType)
		{
			if(sLoginType==null)
			{
				return this.GetAll(this.sWebLogin,null);
			}
			else
			{
				return this.GetRecs(this.sWebLogin,sLoginType,"LOGINTYPE","LOGINTYPE",null);	
			}
		}



		public DataTable GetWebLoginByUserName(string sUserName,string sPassword)
		{
			try
			{
				
				string sTarget=sUserName;
				
				FieldValueCollection oFVC=new FieldValueCollection();
				FieldValue oFVUserName=new FieldValue("USERNAME",sUserName);
				oFVC.Add(oFVUserName);

				if(sPassword!=null)
				{
					FieldValue oFVPassword=new FieldValue("PASSWORD",sPassword);
					sTarget=sTarget.PadRight(15,' ')+sPassword;
					oFVC.Add(oFVPassword);
				}
												
				return this.GetRecs(this.WebLogin,sTarget,"USERNAME",oFVC,null);				
			
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public DataTable GetRxQueue(string sRxNo)
		{
			try
			{
				return this.GetRecs(this.sRxRefQue,sRxNo.PadLeft(7,' '),"RXNO","RXNO",null);

			}
			catch(Exception ex)
			{
				throw ex;
			}		
		}

	
		public DataTable GetWebSet(string sSetType)
		{
			try
			{
				DataTable oTable=null;
				if(sSetType=="-1")
				{
					oTable=this.GetAll(this.sWebSet,null);
				}
				else
				{
					oTable=this.GetRecs(this.sWebSet,sSetType,"SETTYPE","SETTYPE",null);	
				}
				return oTable;			
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public DataTable GetWebLoginByID(string sLoginId)
		{
			try
			{
				return this.GetRecs(this.sWebLogin,sLoginId,"LOGINID","LOGINID",null);
			}
			catch(Exception ex)
			{
				throw ex;
			}

		}
		
		public DataTable GetFaq(string sFaqNo)
		{
			try
			{
				if(sFaqNo=="-1")
				{
					return this.GetAll(this.sFAQ,null,true);
				}
				else
				{
					return this.GetRecs(this.sFAQ,sFaqNo,"FAQNO","FAQNO",null,true);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		private ArrayList GetDoctorFields()
		{
			ArrayList oAl=new ArrayList();
			oAl.Add("PRESLNM");
			oAl.Add("PRESFNM");
			oAl.Add("PRESNO");
			oAl.Add("PRESLIC");
			oAl.Add("PRESDEA");
			oAl.Add("PHONE");
			oAl.Add("ADDRSTR");
			oAl.Add("ADDRST");
			oAl.Add("ADDRCT");
			oAl.Add("ADDRZP");
			return oAl;
		}

		public DataTable GetDocPat(string sDocNo,string sFacNo, string sPatFname, string sPatLname)
		{
			/*
			 * This function will accept a doctor no and patient names,
			 * and return all matching patients that 
			 * belong to that doctor.
			 * */
			CCbDbf oNameFile=null;
			CCbDbf oDocPat=null;			
			try
			{
				DataTable oRetTable=new DataTable();
				DataRow oNewRow;
				string sTemp="";
				string sTarget="";
				string sModiDocNo=sDocNo.PadLeft(12,' ');
				bool bAdd=false;
				System.DateTime oDateTime;
				if (this.OpenFile(out oNameFile,this.sPatient) && OpenFile(out oDocPat,this.sPatPres))
				{
					if(this.Search(oNameFile,"PATNAME",sPatLname))
					{
						oDocPat.db.select("PATPRES");
						oDocPat.db.top();
						this.InsertFields(oRetTable,oNameFile.GetFields(),this.oPatFields);						
						while(oNameFile.db.eof()==0&&oNameFile.GetValString("LNAME").Substring(0,sPatLname.Length) == sPatLname)
						{
							if(oNameFile.db.deleted()==0)
							{							
								if (sPatFname=="" || oNameFile.GetValString("FNAME").Substring(0,sPatFname.Length)== sPatFname)	
								{																			
									
									if(sFacNo=="-1")
									{
										sTarget=oNameFile.GetValString("PATIENTNO").Trim().PadLeft(12,' ')+sModiDocNo;																		
										bAdd=oDocPat.db.seek(sTarget)==0;															
									}
									else
									{
										bAdd=oNameFile.GetValString("FACILITYCD").Trim()==sFacNo;
									}

									if(bAdd)
									{
										oNewRow=oRetTable.NewRow();																											
										for(int i = 0; i < oPatFields.Count; i++)
										{
											sTemp=oPatFields[i].ToString();
											if(oNameFile.IsDateTime(sTemp))
											{
												if(FPUtil.GetCShDate(oNameFile.GetValString(sTemp),out oDateTime))
													oNewRow[sTemp]=oDateTime.ToShortDateString();																								
											}
											else
												oNewRow[sTemp]=oNameFile.GetValString(sTemp).Trim();
										
										}																		
										oRetTable.Rows.Add(oNewRow);	
									}
								}
							}
							oNameFile.db.skip(1);
						}
					}
				}
				return oRetTable;							
			}
			catch(Exception ex)
			{
				throw ex;
			}	
			finally
			{
				CloseFile(oNameFile);
				CloseFile(oDocPat);
			}
		}

		public DataTable GetRxsALLFields(string sRxNo, string sRefNo)
		{
			return this.GetRxs(sRxNo,sRefNo,null);
		}

		public DataTable GetRxs(string sRxNo,string sRefNo)
		{
			return this.GetRxs(sRxNo,sRefNo,this.oRxFields);		
		}

		public DataTable GetRxs(string sRxNo,string sRefNo,ArrayList oFields)
		{			
			try
			{																		
				ArrayList oTempDgFields=new ArrayList();
				oTempDgFields.Add("STRONG");
				oTempDgFields.Add("FORM");
				oTempDgFields.Add("UNITS");
				DataTable oTable=this.GetRecs(this.sClaims,sRxNo.PadLeft(7,' '),"CLMRXN","RXNO",oFields); 				
				DataTable oTempDgTable;
				if(oTable.Rows.Count > 0 && sRefNo!="-1")
				{
					
					oTable.Columns.Add("SIGLINES");					
					oTable.Columns.Add("STRONG");
					oTable.Columns.Add("FORM");
					oTable.Columns.Add("UNITS");
		
					for(int i = 0; i < oTable.Rows.Count; i++)
					{
						if(oTable.Rows[i]["NREFILL"].ToString()!=sRefNo)
						{	
							oTable.Rows[i].Delete();
							i--;
						}
						else
						{							
							oTempDgTable=this.GetRecs(this.sDrug,oTable.Rows[i]["NDC"].ToString(),"DRGNDC","DRGNDC",oTempDgFields);								
							if(oTempDgTable.Rows.Count > 0)
							{
								oTable.Rows[i]["STRONG"]=oTempDgTable.Rows[0]["STRONG"];
								oTable.Rows[i]["FORM"]=oTempDgTable.Rows[0]["FORM"];
								oTable.Rows[i]["UNITS"]=oTempDgTable.Rows[0]["UNITS"];
							}
							oTable.Rows[i]["SIGLINES"]=this.GetRxSig(oTable.Rows[i]["RXNO"].ToString(),oTable.Rows[i]["NREFILL"].ToString());				
						}					
					}
					oTable.AcceptChanges();
				}
				
				
				return oTable;
			}
			catch(Exception ex)
			{
				throw ex;
			}	
		}
			
		public string GetRxSig(string sRxNo, string sNRefill)
		{
			string sTarget=sRxNo.PadLeft(7,' ')+sNRefill.PadLeft(2,' ');
			string sRxSig="";
			FieldValueCollection oFVC=new FieldValueCollection();
			FieldValue oFV=new FieldValue("RXNO",sRxNo);
			FieldValue oFV2=new FieldValue("REFILL_NO",sNRefill);
			oFVC.Add(oFV);
			oFVC.Add(oFV2);
			DataTable oTable=this.GetRecs(this.sRxSig,sTarget,"RXS",oFVC,null);
			if(oTable.Rows.Count > 0)
			{				
				for(int i = 0; i < oTable.Rows.Count; i++)
				{
					sRxSig+=oTable.Rows[i]["SIG"].ToString();
				}				
			}
			return sRxSig;
		}

		public DataTable GetContsByDate(DateTime FromDate, DateTime ToDate,string sPharmId,string sProcess)
		{
			
			string sTarget=sPharmId.PadRight(15,' ')+sProcess+this.GetFPDate(FromDate);
			CCbDbf oFile=null;
			int iResult=-1;
			DataTable oRetTable=new DataTable();
			DateTime oDateF;			
			DataRow oRow=null;	

			if(OpenFile(out oFile,this.sWebContact,true))
			{
				
				this.InsertFields(oRetTable,oFile.GetFields(),null);
				
				oFile.db.select("PHARMID");
				oFile.db.top();
				iResult=oFile.db.seek(sTarget);
			
				if(iResult==CodeBase.Code4.r4eof)
				{
					oFile.db.bottom();
				}

				//if the pharmid is not equal to the pharmid i am not on the correct record or if the process is not sprocess then i am not on correct record.
				if(!sPharmId.Equals(oFile.GetValString("PHARMID").TrimEnd()) || !sProcess.Equals(oFile.GetValString("PROCESS").TrimEnd()))
					return oRetTable;

			
				while(oFile.db.eof()==0)
				{
							
					if(oFile.db.deleted()==1)
					{
						oFile.db.skip(1);
						continue;
					}

					if((FPUtil.GetCShDate(oFile.GetValString("CONTDATE").Trim(),out oDateF)))
					{										
					
						if(oDateF >= FromDate && oDateF <= ToDate && sPharmId.Equals(oFile.GetValString("PHARMID").TrimEnd()) && sProcess.Equals(oFile.GetValString("PROCESS").TrimEnd()))
						{
							oRow=oRetTable.NewRow();
							CopyRec(oRow,oRetTable,oFile,true);
							oRetTable.Rows.Add(oRow);
						}
						else
						{
							break;
						}				
				
					}													
				
					oFile.db.skip(1);
						
				}
							

			}

			return oRetTable;
		
		}

		public void InsertExtPat(DataTable oTable)
		{
			this.ModiFile(this.sExtrnPat,"NEWPATID","NEWPATID",oTable,true,true);
		}
		
		public DataTable GetExtPatByDate(DateTime FromDate, DateTime ToDate,string sPharmId,string sProcess)
		{
			
			
			string sTarget=sPharmId.PadRight(15,' ');
			
			bool bDateSrch=false;					

			CCbDbf oFile=null;
			int iResult=-1;
			DataTable oRetTable=new DataTable();
			DateTime oDateF;			
			DataRow oRow=null;	

			if (sProcess!=null)
			{
	
				sTarget+=sProcess.PadRight(1,' ');				
				
				if(FromDate!=System.DateTime.MinValue && ToDate!=System.DateTime.MinValue)
				{
					sTarget+=this.GetFPDate(FromDate);
					bDateSrch=true;
				}
			
			}
									
			if(OpenFile(out oFile,this.sExtrnPat,true))
			{
				
				
				this.InsertFields(oRetTable,oFile.GetFields(),null);
				
				oFile.db.select("PHARMID");
				oFile.db.top();
				iResult=oFile.db.seek(sTarget);
			
				if(iResult==CodeBase.Code4.r4eof)
				{
					oFile.db.bottom();
				}

				//if the pharmid is not equal to the pharmid i am not on the correct record or if the process is not sprocess then i am not on correct record.
		//		if(!sPharmId.Equals(oFile.GetValString("PHARMID").TrimEnd()) || (sProcess!=null && !sProcess.Equals(oFile.GetValString("PROCESS").TrimEnd())))
		//			return oRetTable;								
			
				while(oFile.db.eof()==0)
				{
							
					if(oFile.db.deleted()==1)
					{
						oFile.db.skip(1);
						continue;
					}

					if(sPharmId.Equals(oFile.GetValString("PHARMID").TrimEnd()))
					{
						if(sProcess!=null)
						{
							if(sProcess.Equals(oFile.GetValString("PROCESSED").TrimEnd()))
							{
								if(bDateSrch)
								{
									if(!(FPUtil.GetCShDate(oFile.GetValString("REQDATE"),out oDateF) && oDateF >= FromDate && oDateF <= ToDate))
										break;									
								}
								
							}
							else
								break;
						}						
					}
					else
						break;


					
					oRow=oRetTable.NewRow();
					CopyRec(oRow,oRetTable,oFile,true);
					oRetTable.Rows.Add(oRow);
																
					oFile.db.skip(1);
				
				}													
				
				
						
			}
							
			return oRetTable;
		
		}

				
		public DataTable GetRxsByDate(DateTime FromDate,DateTime ToDate)
		{
		
			/*
			 * Returns rxs from date to date.
			 * */
			string sTarget=this.GetFPDate(FromDate);
			CCbDbf oFile=null;
			int iResult=-1;
			DataTable oRetTable=new DataTable();
			DateTime oDateF;			
			DataRow oRow=null;

			if(OpenFile(out oFile,this.sClaims))
			{				
				
				this.InsertFields(oRetTable,oFile.GetFields(),null);
				
				oFile.db.select("CLMDTF");
				oFile.db.top();
				iResult=oFile.db.seek(sTarget);
				
				if(iResult==CodeBase.Code4.r4eof)
				{
					oFile.db.bottom();
				}

				/*
				if(iResult==CodeBase.Code4.r4success || iResult==CodeBase.Code4.r4after)
				{				
					//	oFile.db.skip(-1);										
					bDateRange=FPUtil.GetCShDate(oFile.GetValString("DATEF").Trim(),out oDateF) && oDateF >= FromDate && oDateF <= ToDate;										
					if(!bDateRange)
						return oRetData;								
				}
				else if(iResult==CodeBase.Code4.r4eof)
				{				
					oFile.db.bottom();					
					bDateRange=FPUtil.GetCShDate(oFile.GetValString("DATEF").Trim(),out oDateF) && oDateF >= FromDate && oDateF <= ToDate;										
					if(!bDateRange)
						return oRetData;				
				}
				else {return oRetData;}								
				*/
				
			}				
			
			while(oFile.db.eof()==0)
			{
							
				if(oFile.db.deleted()==1)
				{
					oFile.db.skip(1);
					continue;
				}

				if((FPUtil.GetCShDate(oFile.GetValString("DATEF").Trim(),out oDateF)))
				{										
					
					if(oDateF >= FromDate && oDateF <= ToDate)
					{
						oRow=oRetTable.NewRow();
						CopyRec(oRow,oRetTable,oFile);
						oRetTable.Rows.Add(oRow);
					}
					else
					{
						break;
					}				
				
				}													
				
				oFile.db.skip(1);
						
			}
			
			return oRetTable;
		
		}

		public DataTable GetAccess(string sUserName, string sLogoutDt)
		{
			/*
			 * This function will return all users from access table.
			 * If logoutdt is null that means return all the username records.
			 * if logoutdt is empty string "" that means return username with blank logout dt
			 * if logoutdt is specified than return username with that logout dt.
			 * */
			
			FieldValueCollection oFVC=new FieldValueCollection();			
			FieldValue oFV=new FieldValue("USERNAME",sUserName);			
			string sTarget=sUserName;
			oFVC.Add(oFV);
			
			if(sLogoutDt!=null)
			{
				oFV=new FieldValue();
				oFV.sField="LOGOUT_DT";
				
				if(sLogoutDt.Equals(""))
				{
					oFV.sValue = "";	
					sTarget=sTarget.PadRight(23,' ');
				}
				else
				{
					oFV.sValue = this.GetFPDate(Convert.ToDateTime(sLogoutDt));
					sTarget=sTarget.PadRight(15,' ')+oFV.sValue;
				}
												
				oFVC.Add(oFV);
			
			}
			
			return this.GetRecs(this.sAccess,sTarget,"USERNAME",oFVC,null);
			
		}

		private string GetFPDate(System.DateTime a)
		{
			//returns back the fp format of a date which needs to be written to codebase
			string sMonth=a.Month.ToString();
			string sDay=a.Day.ToString();
			string sYear=a.Year.ToString();			
			if (sMonth.Trim().Length == 1)
				sMonth="0"+sMonth;			
			if (sDay.Trim().Length == 1)
				sDay="0"+sDay;
			return (sYear.Trim()+sMonth.Trim()+sDay.Trim());			
		}

		private DataTable GetReverseRecs(string sPatNo,string sFile,System.DateTime dtStartDate,System.DateTime odtEndDate,ArrayList oFields)
		{
			/*
			 * This is a generic function to get recs 
			 * that are in descending order.
			 * */			
			DataTable oRetData=new DataTable();			
			string sTemp;
			CCbDbf oFile=null; 							
			System.DateTime oDateTime;			
			System.DateTime oDateTimeTemp;
			int iResult;						
			try
			{				
				string sTarget=Convert.ToString((Convert.ToInt32(sPatNo)+1)).PadLeft(12,' ');
				if(OpenFile(out oFile,sFile))
				{									
					oFile.db.select("CLMMEDNO");
					iResult=oFile.db.seek(sTarget);																				
					if(iResult==CodeBase.Code4.r4success || iResult==CodeBase.Code4.r4after)
					{
						oFile.db.skip(-1);
						if(!(oFile.GetValString("PATIENTNO").Trim()==sPatNo))
							return oRetData;					
					}
					else if(iResult==CodeBase.Code4.r4eof)
					{
						oFile.db.bottom();
						if(!(oFile.GetValString("PATIENTNO").Trim()==sPatNo))
							return oRetData;
					}
					else {return oRetData;}					
					
					InsertFields(oRetData,oFile.GetFields(),oFields);												
					DataRow oNewRow;
					sTarget=sTarget.Trim(); //this is for if the key is formatted in a special way
					
					/*
					 * This part will search for the beginning end date
					 * */										
					while(oFile.db.eof()==0 && oFile.GetValString("PATIENTNO").Trim()==sPatNo)
					{
						if(FPUtil.GetCShDate(oFile.GetValString("DATEF"),out oDateTimeTemp))
						{
							if(oDateTimeTemp <= odtEndDate)
								break;
						}
						oFile.db.skip(-1);	
					}
										
					while(oFile.db.bof()==0 && oFile.GetValString("PATIENTNO").Trim()==sPatNo)
					{																						
						if (FPUtil.GetCShDate(oFile.GetValString("DATEF"),out oDateTimeTemp))
						{
							if(!(oDateTimeTemp >= dtStartDate))
								break;
						}
						else
							break;
												
						if(oFile.db.deleted()==0)
						{
							oNewRow=oRetData.NewRow();
							if(oFields==null)
							{								
								foreach(DataColumn oCol in oRetData.Columns)								
								{
									if(oFile.IsDateTime(oCol.ColumnName))
									{
										if(FPUtil.GetCShDate(oFile.GetValString(oCol.ColumnName),out oDateTime))
											oNewRow[oCol.ColumnName]=oDateTime.ToShortDateString();																								
									}										
									else
										oNewRow[oCol.ColumnName]=oFile.GetValString(oCol.ColumnName).Trim(); 																											
								}
							}
							else
							{
								for(int i = 0 ; i < oFields.Count ; i++)
								{
									sTemp=oFields[i].ToString();
									if(oFile.IsDateTime(sTemp))
									{
										if(FPUtil.GetCShDate(oFile.GetValString(sTemp),out oDateTime))
											oNewRow[sTemp]=oDateTime.ToShortDateString();																								
									}																				
									else
										oNewRow[sTemp]=oFile.GetValString(sTemp).Trim();									
								}
							}	
							oRetData.Rows.Add(oNewRow);
						}
						oFile.db.skip(-1);
					}
				
				}									
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				CloseFile(oFile);
			}
			return oRetData;
		}


		public DataTable GetPatHistory(string sPatNo, string sDocNo, System.DateTime odtStartDate,System.DateTime odtEndDate,bool bIncludeDiscontinued)
		{
			try
			{										
				DateTime oCurrDate=odtStartDate;				
				string sModiPatNo=sPatNo.PadLeft(12,' ');
				//string sTarget="";								
				DataTable oTable=null;
				DataTable oTempTable=null;
				DataRow oRow;
				DateTime oDtObsDate;
				double dAmt=0;
				double dPfee=0;
				double dDiscount=0;
				bool bDelete=false;	
				
				oTable=this.GetReverseRecs(sPatNo,this.sClaims,odtStartDate,odtEndDate,this.oPatHistFields);					
				
				if(oTable.Rows.Count == 0) return oTable;
			
				ArrayList oPatTempFields=new ArrayList();
				oPatTempFields.Add("FACILITYCD");

				ArrayList oDgFields=new ArrayList();
				oDgFields.Add("DRGNAME");
				oDgFields.Add("STRONG");
				oDgFields.Add("FORM");
				oDgFields.Add("OBSDATE");

				ArrayList oPrFields=new ArrayList();
				oPrFields.Add("PRESLNM");
				oPrFields.Add("PRESFNM");


				oTable.Columns.Add("DRGINFO");
				
				oTable.Columns.Add("PRSINFO");
				oTable.Columns.Add("TOTRXAMT");
	
				for(int i = 0; i < oTable.Rows.Count; i++)
				{
					oRow=oTable.Rows[i];						
					
					bDelete=false;
					
					if(sDocNo!="-1")
					{
						bDelete=(sDocNo!=oRow["PRESNO"].ToString());						
					}
					
					if(!bDelete)
					{
						if(!bIncludeDiscontinued)
						{
							if(oRow["ORDSTATUS"].ToString().Trim().Equals("D"))
								bDelete=true;
						}
					}
	
					if(bDelete)
					{
						oTable.Rows[i].Delete();
						i--;
						continue;
					}


					//get drug info
					if(!bDelete)
					{
					
						oTempTable=this.GetRecs(this.sDrug,oRow["NDC"].ToString(),"DRGNDC","DRGNDC",oDgFields);					
						
						if(oTempTable.Rows.Count > 0)
						{
							
							/*
							if(!bIncludeDiscontinued)
							{							
								if(Util.IsDate(oTempTable.Rows[0]["OBSDATE"].ToString()))
								{									
									oDtObsDate=Convert.ToDateTime(oTempTable.Rows[0]["OBSDATE"].ToString());
									
									if(System.DateTime.Today>=oDtObsDate)
									{										
										bDelete=true;
									}									
								
								}																															
							
							}						
							*/							
							
							oRow["DRGINFO"]=oTempTable.Rows[0]["DRGNAME"].ToString()+" "+oTempTable.Rows[0]["FORM"].ToString()+" "+oTempTable.Rows[0]["STRONG"].ToString();													

						}																				
					}
										
					oTempTable=this.GetRecs(this.sPrescrib,oRow["PRESNO"].ToString(),"PRESNO","PRESNO",oPrFields);					
					if(oTempTable.Rows.Count > 0)
					{						
						oRow["PRSINFO"]=oTempTable.Rows[0]["PRESLNM"].ToString()+", "+oTempTable.Rows[0]["PRESFNM"].ToString();
					}
					
					dAmt=FPUtil.GetCCDouble(oRow["AMOUNT"].ToString());
					dPfee=FPUtil.GetCCDouble(oRow["PFEE"].ToString());
					dDiscount=FPUtil.GetCCDouble(oRow["DISCOUNT"].ToString());

					oRow["TOTRXAMT"]=Convert.ToString(dAmt+dPfee-dDiscount);									

				}				
				
				oTable.AcceptChanges();
				return oTable;				
			
			}			
			catch(Exception ex)
			{
				throw ex;
			}		
		}

/*

		public DataTable GetPatHistory(string sPatNo,string sDocNo,System.DateTime odtStartDate)
		{
			try
			{							
			
				DateTime oCurrDate=odtStartDate;				
				string sModiPatNo=sPatNo.PadLeft(12,' ');
				string sTarget="";								
				DataTable oTable=null;
				DataTable oTempTable=null;
				DataRow oRow;
				double dAmt=0;
				double dPfee=0;
				double dDiscount=0;
				FieldValue oFV=new FieldValue("PATIENTNO",sPatNo);				
				FieldValueCollection oFVCol=new FieldValueCollection();
				oFVCol.Add(oFV);
				
							
				while(oCurrDate <= DateTime.Today)
				{
					sTarget=sModiPatNo+FPUtil.GetFPDate(oCurrDate);					
					
					oTable=this.GetRecs(this.sClaims,sTarget,"CLMMEDNO",oFVCol,this.oPatHistFields);
					
					if(oTable.Rows.Count > 0) break;					
					
					oCurrDate=oCurrDate.AddDays(1);					
				}
				
				if(oTable.Rows.Count == 0) return oTable;
			
				ArrayList oDgFields=new ArrayList();
				oDgFields.Add("DRGNAME");
				oDgFields.Add("STRONG");
				oDgFields.Add("FORM");

				ArrayList oPrFields=new ArrayList();
				oPrFields.Add("PRESLNM");
				oPrFields.Add("PRESFNM");


				oTable.Columns.Add("DRGINFO");
				oTable.Columns.Add("PRSINFO");
				oTable.Columns.Add("TOTRXAMT");
	
				for(int i = 0; i < oTable.Rows.Count; i++)
				{
					oRow=oTable.Rows[i];	
					
					if(sDocNo!="-1")
					{
						if(sDocNo!=oRow["PRESNO"].ToString())
						{	
							oTable.Rows[i].Delete();
							i--;
							continue;
						}
					}
					
					//get drug info
					oTempTable=this.GetRecs(this.sDrug,oRow["NDC"].ToString(),"DRGNDC","DRGNDC",oDgFields);

					if(oTempTable.Rows.Count > 0)
					{
						oRow["DRGINFO"]=oTempTable.Rows[0]["DRGNAME"].ToString()+" "+oTempTable.Rows[0]["FORM"].ToString()+" "+oTempTable.Rows[0]["STRONG"].ToString();
					}

					oTempTable=this.GetRecs(this.sPrescrib,oRow["PRESNO"].ToString(),"PRESNO","PRESNO",oPrFields);
					
					if(oTempTable.Rows.Count > 0)
					{						
						oRow["PRSINFO"]=oTempTable.Rows[0]["PRESLNM"].ToString()+", "+oTempTable.Rows[0]["PRESFNM"].ToString();
					}
					
					dAmt=FPUtil.GetCCDouble(oRow["AMOUNT"].ToString());
					dPfee=FPUtil.GetCCDouble(oRow["PFEE"].ToString());
					dDiscount=FPUtil.GetCCDouble(oRow["DISCOUNT"].ToString());

					oRow["TOTRXAMT"]=Convert.ToString(dAmt+dPfee-dDiscount);									

				}				
				oTable.AcceptChanges();
				return oTable;	
			
			}			
			catch(Exception ex)
			{
				throw ex;
			}
		}
*/
		private bool Search(CCbDbf oFile, string sIndex, string sTarget)
		{
			bool bRetVal=false;
			try
			{
				if(oFile.db.isValid()!=0)
				{
					oFile.db.select(sIndex);
					oFile.db.top();
					bRetVal=(oFile.db.seek(sTarget)==0);
				}								
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return bRetVal;
		}

		private DataTable GetName(string sFileName,string sIndex, string sLname, string sFname,string sLnameField, string sFnameField,ArrayList oFields,bool bRemoveWebLogin,string sLoginType,string sPharmIdField)
		{
			
			CCbDbf oNameFile=null;						
			CCbDbf oWebLoginFile=null;
			bool bDontInclude=false;

			try
			{											
			
				DataTable oRetTable=new DataTable();
				DataRow oNewRow;
				string sTemp="";
				System.DateTime oDateTime;
				string sWebLoginTarget="";

				if (this.OpenFile(out oNameFile,sFileName))
				{
					
					if(this.Search(oNameFile,sIndex,sLname))
					{
						
						this.InsertFields(oRetTable,oNameFile.GetFields(),oFields);						
						
						if(bRemoveWebLogin)
						{
						
							bRemoveWebLogin=this.OpenFile(out oWebLoginFile,this.sWebLogin);
							if(bRemoveWebLogin)
							{
								oWebLoginFile.db.select("LOGINTYPE");								
							}
						
						}
						
						while(oNameFile.db.eof()==0&&oNameFile.GetValString(sLnameField).Substring(0,sLname.Length) == sLname)
						{
							if(oNameFile.db.deleted()==0)
							{																															
								if (sFname=="" || oNameFile.GetValString(sFnameField).Substring(0,sFname.Length)== sFname)	
								{	
																	
									
									if(bRemoveWebLogin)
									{	
										
										/*
										 * This will check if the user is in the weblogin
										 * file, if the user is in the weblogin file then it will
										 * ignore the record and continue.
										 * */
									
										sWebLoginTarget=sLoginType+oNameFile.GetValString(sPharmIdField).Trim();
									
										if(oWebLoginFile.db.seek(sWebLoginTarget)==0)
										{
											
											bDontInclude=false;

											while(oWebLoginFile.db.eof()==0&&oWebLoginFile.GetValString("LOGINTYPE").Trim().Equals(sLoginType) && oWebLoginFile.GetValString("PHARMID").Trim().Equals(oNameFile.GetValString(sPharmIdField).Trim()))
											{
												
												if(oWebLoginFile.db.deleted()==0)
												{													
													bDontInclude=true;
													break;
												}									
												
												oWebLoginFile.db.skip(1);
											
											}
											
											if(bDontInclude)
											{
											
												oNameFile.db.skip(1);
												continue;
											
											}
										
										}									
									
									}
									
									oNewRow=oRetTable.NewRow();																											
									if(oFields==null)
									{
										foreach(DataColumn oCol in oRetTable.Columns)
										{
											if(oNameFile.IsDateTime(oCol.ColumnName))
											{
												if(FPUtil.GetCShDate(oNameFile.GetValString(oCol.ColumnName),out oDateTime))
													oNewRow[oCol.ColumnName]=oDateTime.ToShortDateString();																								
											}
											else
												oNewRow[oCol.ColumnName]=oNameFile.GetValString(oCol.ColumnName).Trim(); 																												
										}
									}
									else
									{
										for(int i = 0; i < oFields.Count; i++)
										{
											sTemp=oFields[i].ToString();
											if(oNameFile.IsDateTime(sTemp))
											{
												if(FPUtil.GetCShDate(oNameFile.GetValString(sTemp),out oDateTime))
													oNewRow[sTemp]=oDateTime.ToShortDateString();																								
											}											
											else
												oNewRow[sTemp]=oNameFile.GetValString(sTemp).Trim();										
										}
									}
									oRetTable.Rows.Add(oNewRow);	
								}
							}
							oNameFile.db.skip(1);
						}
					}
					//					CloseFile(oNameFile);
				}
				return oRetTable;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				CloseFile(oNameFile);
				CloseFile(oWebLoginFile);
			}
		}

		private ArrayList GetPatientFields()
		{
			ArrayList oAl=new ArrayList();
			oAl.Add("LNAME");
			oAl.Add("FNAME");
			oAl.Add("DOB");
			oAl.Add("PATIENTNO");
			oAl.Add("SEX");
			oAl.Add("PHONE");
			oAl.Add("ADDRSTR");
			oAl.Add("ADDRST");
			oAl.Add("ADDRCT");
			oAl.Add("ADDRZP");
			oAl.Add("PATTYPE");
			oAl.Add("EMAIL");
			oAl.Add("FACILITYCD");
			oAl.Add("VIEWHIST");
			oAl.Add("SIGACK");
			oAl.Add("SIGACKDATE");
			return oAl;
		}
	
		
		private ArrayList GetFacilityFields()
		{
			ArrayList oA1 = new ArrayList();
			oA1.Add("FACILITYNM");
			oA1.Add("ADDRESS1");
			oA1.Add("ADDRESS2");
			oA1.Add("CITY");
			oA1.Add("STATE");
			return oA1;
		}

		public DataTable GetFacilityByName(string sName,bool bRemoveWebLogin)
		{
		
			try
			{

				string sPharmIdField="";
				string sLoginType="";

				if(bRemoveWebLogin)
				{
					sPharmIdField="FACILITYCD";
					sLoginType="FA";
				}
				
				return this.GetName(this.sFacility,"FACNAME",sName,"","FACILITYNM","",null,bRemoveWebLogin,sLoginType,sPharmIdField);
			
			}
			catch(Exception ex)
			{
				throw ex;
			}
		
		}
		
		public DataTable GetDoctorsByName(string sLname,string sFname,bool bRemoveWebLogin)
		{
			try
			{

				string sPharmIdField="";
				string sLoginType="";

				if(bRemoveWebLogin)
				{
					sPharmIdField="PRESNO";
					sLoginType="DO";
				}
				
				return this.GetName(this.sPrescrib,"PRSNAME",sLname,sFname,"PRESLNM","PRESFNM",this.oPresFields,bRemoveWebLogin,sLoginType,sPharmIdField);
			
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public DataTable GetPatientByName(string sLname,string sFname,bool bRemoveWebLogin)
		{
			try
			{

				string sPharmIdField="";
				string sLoginType="";

				if(bRemoveWebLogin)
				{
					sPharmIdField="PATIENTNO";
					sLoginType="PA";
				}
				
				return this.GetName(this.sPatient,"PATNAME",sLname,sFname,"LNAME","FNAME",this.oPatFields,bRemoveWebLogin,sLoginType,sPharmIdField);
			
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}


		
		public DataTable GetPatient(string sPatNo)
		{
			try
			{
				if(sPatNo=="-1")
				{
					return this.GetAll(this.sPatient,this.oPatFields);				
				}
				else
				{
					return this.GetRecs(this.sPatient,sPatNo,"PATNO","PATIENTNO",this.oPatFields);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public DataTable GetUser(string username)
		{
			//this functions will return the details if there is a record for that user name
			try
			{
				return this.GetRecs(this.sWebLogin,username,"USERNAME","USERNAME",null);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		

		private void InsertFields(DataTable oTable, System.Collections.ICollection oFields,ArrayList oOthFields)
		{
			/*
			 * This function will add the columns into a table
			 * for the fields passed in on the right side.
			 * */			
				
			if(oOthFields==null)
			{
				System.Collections.IEnumerator oField=oFields.GetEnumerator();								
				while(oField.MoveNext())			
					oTable.Columns.Add(oField.Current.ToString());			
			}
			else
			{				
				for(int i = 0 ; i < oOthFields.Count ; i++)
				{
					oTable.Columns.Add(oOthFields[i].ToString());
				}
			}				
		}
		
		private void CopyRec(DataRow oNewRow,DataTable oRetData,CCbDbf oFile)
		{
			this.CopyRec(oNewRow,oRetData,oFile,false);
		}

		private void CopyRec(DataRow oNewRow,DataTable oRetData,CCbDbf oFile,bool bProcessMemo)
		{
			
			DateTime oDateTime;

			foreach(DataColumn oCol in oRetData.Columns)								
			{
				
				if(oFile.IsDateTime(oCol.ColumnName))			
				{
					if(FPUtil.GetCShDate(oFile.GetValString(oCol.ColumnName),out oDateTime))
						oNewRow[oCol.ColumnName]=oDateTime.ToShortDateString();																												
				}										
				else if(bProcessMemo && oFile.IsMemoField(oCol.ColumnName))
					oNewRow[oCol.ColumnName]=oFile.GetField4Memo(oCol.ColumnName).str().Trim();
				else
					oNewRow[oCol.ColumnName]=oFile.GetValString(oCol.ColumnName).Trim(); 																											
			
			}
		
		}

		public void SavePatientAck(long lPatNo,string sAck,DateTime dtAckDate)
		{
			try
			{
				DataTable oPat=this.GetPatient(lPatNo.ToString());
				if(oPat.Rows.Count > 0)
				{
										
					oPat.Rows[0]["SIGACK"]=sAck;				
					oPat.Rows[0]["SIGACKDATE"]=dtAckDate.ToShortDateString();
					
					this.ModiFile(this.sPatient,"PATIENTNO","PATNO",oPat,false,true);				
			
				}
			}
			catch(Exception ex)
			{

			}

		}

		public bool DoesRxExist(string sRxNo,string sRefNo)
		{
			CCbDbf oClaims=null;
			bool bRetVal=false;
			try
			{	string sError="";
				bRetVal=PositionRx2(sRxNo,sRefNo,out oClaims,out sError);
			}
			catch{}
			finally
			{
				CloseFile(oClaims);
			}
			return bRetVal;
			
		}


		public bool PositionRx(string sRxNo,string sRefNo,out CCbDbf oClaims)
		{
			oClaims=null;
			try
			{							
				if(this.OpenFile(out oClaims,this.sClaims) && this.Search(oClaims,"CLMRXN",sRxNo))
				{
					while(oClaims.db.eof()==0&&oClaims.GetValString("RXNO").Trim()==sRxNo){oClaims.db.skip(1);}					
					oClaims.db.skip(-1);				
					return true;
				}							
			}
			catch(Exception ex)
			{

			}				
		
			return false;
		
		}
		
		public bool PositionRx2(string sRxNo,string sRefNo,out CCbDbf oClaims,out string sError)
		{
			oClaims=null;
			bool bRefFound=false;
			sError="";

			try
			{							
				
				if(this.OpenFile(out oClaims,this.sClaims))
				{
					
					if (this.Search(oClaims,"CLMRXN",sRxNo))
					{
					
						while(oClaims.db.eof()==0&&oClaims.GetValString("RXNO").Trim()==sRxNo)
						{
						
						
							if(oClaims.GetValString("NREFILL").Trim().Equals(sRefNo) && oClaims.db.deleted()==0)
							{
								bRefFound=true;
								break;
							}
						
						
							oClaims.db.skip(1);
					
						}					
										
						if(sRefNo.Equals("-1"))
						{
							oClaims.db.skip(-1);
							return true;
						}
						else
						{
						
							if(bRefFound)
								return true;
							else
							{
								sError="Could Not Seek Rx# "+sRxNo+" RefNo: "+sRefNo+" In PositionRx2.";
								return false;
							}


						}
										

					}				
					else
					{
						
						sError="Could Not Seek Rx# "+sRxNo+" RefNo: "+sRefNo+" In PositionRx2. ";
					
					}
				
				
				}							
				else
				{
					
					sError="Could Not Open Claims File In PositionRx2";
				
				}

			}
			catch(Exception ex)
			{
				sError=ex.Message+"\n"+ex.StackTrace;
			}				
		
			return false;
		
		}

		public string CheckDDIDUP(string sDrgNdc,string sTxrCode,long lPatNo,int iDDIDays,out DataTable oDtInteracts,out bool bFoundDupDrugs,out string sDupDrugs)
		{
			
			oDtInteracts=new DataTable();
			bFoundDupDrugs=false;
			sDupDrugs="";
			return "false";

		}
		
		public bool CheckAllergy(string strTxrCode,string strNDC, string strPatAllergy, out string strRetInfo)
		{
			strRetInfo="";
			return false;
		}

		public void MarkDelivery(string sRxNo,string sRefNo,string sDelivery,string sPickedUp,DateTime PickUpDate,out string sError)
		{
		
			CCbDbf oClaims=null;
			
			sError="";

			try
			{
				if (this.PositionRx2(sRxNo,sRefNo,out oClaims,out sError))							
				{									
				
					oClaims.db.lockRecord(oClaims.db.recNo());
				
					if(sDelivery!=null)
					{
						oClaims.GetValField4("DELIVERY").assign(sDelivery);
					}
				
					if (sPickedUp!=null)
					{
						oClaims.GetValField4("PICKEDUP").assign(sPickedUp);																
					}
				
					if(!(PickUpDate.Equals(DateTime.MinValue)))
					{
						oClaims.GetValField4("PICKUPDATE").assign(this.GetFPDate(PickUpDate));
					}

					oClaims.db.write();	
					
					oClaims.db.unlock();

				}
			
			}
			catch(Exception ex)
			{
			
				sError+="\n Error In MarkDelivery For Rx# "+sRxNo+" Ref# "+sRefNo+" \n "+ex.Message+" \n "+ex.StackTrace;
			
			}
			finally
			{
			
				CloseFile(oClaims);
			
			
			}
		
		}

		private DataTable GetAll(string sFileName,ArrayList oFields)
		{
			return this.GetAll(sFileName,oFields,false);
		}

		private DataTable GetAll(string sFileName,ArrayList oFields,bool bProcessMemo)
		{			
			DataTable oRetData=new DataTable();						
			string sTemp;
			CCbDbf oFile=null; 											
			System.DateTime oDateTime;			
			try
			{
				//FieldValue temp;
				if(OpenFile(out oFile,sFileName,bProcessMemo))
				{																																		
					oFile.db.top();
					InsertFields(oRetData,oFile.GetFields(),oFields);						
					DataRow oNewRow;						
					while(oFile.db.eof()==0)
					{
						if(oFile.db.deleted()==0)
						{							
							oNewRow=oRetData.NewRow();
							if(oFields==null)
							{
								foreach(DataColumn oCol in oRetData.Columns)
								{	
									if(oFile.IsDateTime(oCol.ColumnName))
									{										
										if(FPUtil.GetCShDate(oFile.GetValString(oCol.ColumnName),out oDateTime))
											oNewRow[oCol.ColumnName]=oDateTime.ToShortDateString();											
										else
											oNewRow[oCol.ColumnName]="";																				
									}
									else if(bProcessMemo && oFile.IsMemoField(oCol.ColumnName))
									{
										oNewRow[oCol.ColumnName]=oFile.GetField4Memo(oCol.ColumnName).str().Trim();
									}
									else
										oNewRow[oCol.ColumnName]=oFile.GetValString(oCol.ColumnName).Trim(); 																																	
								}		
							}		
							else
							{
								for(int i = 0 ; i < oFields.Count; i++)
								{
									sTemp=oFields[i].ToString();									
									if(oFile.IsDateTime(sTemp))
									{
										if(FPUtil.GetCShDate(oFile.GetValString(sTemp),out oDateTime))
											oNewRow[sTemp]=oDateTime.ToShortDateString();						
									}
									else if(bProcessMemo && oFile.IsMemoField(sTemp))
									{
										oNewRow[sTemp]=oFile.GetField4Memo(sTemp).str().Trim();
									}
									else
										oNewRow[sTemp] = oFile.GetValString(sTemp).Trim();																
								}							
							}																							
							oRetData.Rows.Add(oNewRow);					
						}
						oFile.db.skip(1);
					}
					//CloseFile(oFile);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				CloseFile(oFile);
			}
			return oRetData;			 			
		}

		private DataTable GetRecs(string sFileName, string sTarget,string sIndex,FieldValueCollection oFieldValues,ArrayList oFields)
		{
			return this.GetRecs(sFileName,sTarget,sIndex,oFieldValues,oFields,false);
		}

		private DataTable GetRecs(string sFileName, string sTarget,string sIndex,FieldValueCollection oFieldValues,ArrayList oFields,bool bProcessMemo)
		{
			/*
			 * This function is very powerful in the sense
			 * it is the enhanced version of the other getrecs.
			 * For example if there is an index, which points to 2 fields or n fields.
			 * you can search that index with the target and it will only
			 * return those records which are in the fieldvalue collection. 
			 * I realized this approach after writing the other one
			 * so there are some functions which still use the old getrec
			 * method but feel free to change it to this method, this is more
			 * dynamic. This can handle multiple fields on a particular index
			 * unlike the other one.			  			  			  
			 * */
			
			DataTable oRetData=new DataTable();					
			CCbDbf oFile=null; 	
			try
			{																		
				bool bContinue=true;
				FieldValue temp;
				string sTemp;
				System.DateTime oDateTime;
				if(OpenFile(out oFile,sFileName,bProcessMemo))
				{
					if (Search(oFile,sIndex,sTarget))
					{																													
						InsertFields(oRetData,oFile.GetFields(),oFields);						
						DataRow oNewRow;						
						while(oFile.db.eof()==0 && bContinue)
						{
							if(oFile.db.deleted()==0)
							{
								
								
								for(int i=0;i<oFieldValues.Count && bContinue;i++)
								{								
									
									temp=(FieldValue)oFieldValues[i];									
									if(!(oFile.GetValString(temp.sField).Trim()==temp.sValue))
										bContinue=false;

								}																																
								
								if(bContinue)
								{
								
									oNewRow=oRetData.NewRow();									
									if(oFields==null)
									{
										foreach(DataColumn oCol in oRetData.Columns)
										{
											if(oFile.IsDateTime(oCol.ColumnName))
											{
												if(FPUtil.GetCShDate(oFile.GetValString(oCol.ColumnName),out oDateTime))
 													oNewRow[oCol.ColumnName]=oDateTime.ToShortDateString();																								
											}
											else if(bProcessMemo && oFile.IsMemoField(oCol.ColumnName))
											{
												oNewRow[oCol.ColumnName]=oFile.GetField4Memo(oCol.ColumnName).str().Trim();
											}
											else
												oNewRow[oCol.ColumnName]=oFile.GetValString(oCol.ColumnName).Trim(); 																																										
										}									
									}	
									else
									{
										for(int i = 0 ; i < oFields.Count; i++)
										{
											sTemp=oFields[i].ToString();
											if(oFile.IsDateTime(sTemp))
											{
												if(FPUtil.GetCShDate(oFile.GetValString(sTemp),out oDateTime))
													oNewRow[sTemp]=oDateTime.ToShortDateString();																								
											}
											else if(bProcessMemo && oFile.IsMemoField(sTemp))
											{
												oNewRow[sTemp]=oFile.GetField4Memo(sTemp).str().Trim();
											}									
											else
												oNewRow[sTemp]=oFile.GetValString(sTemp).Trim();																							
										}
									}									
									oRetData.Rows.Add(oNewRow);								
								}							
							}
							oFile.db.skip(1);
						}
					}				
					//CloseFile(oFile);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				CloseFile(oFile);
			}

			return oRetData;			 
		}
		
		private DataTable GetRecs(string sFileName, string sTarget, string sIndex,string sField,ArrayList oFields)
		{
			return this.GetRecs(sFileName,sTarget,sIndex,sField,oFields,false);
		}

		private DataTable GetRecs(string sFileName, string sTarget, string sIndex,string sField,ArrayList oFields,bool bProcessMemo)
		{									
			/*
			 * This function will return a datatable
			 * FileName: Accepts the file name for the dbf file
			 * sTarget: The Value to search
			 * sIndex: Resembles the index 
			 * sField: The field that the index points to.
			 * */			
			DataTable oRetData=new DataTable();			
			string sTemp;
			CCbDbf oFile=null; 							
			System.DateTime oDateTime;			
			try
			{									
				if(OpenFile(out oFile,sFileName,bProcessMemo))
				{
					if (Search(oFile,sIndex,sTarget))
					{																																		
						InsertFields(oRetData,oFile.GetFields(),oFields);												
						DataRow oNewRow;
						sTarget=sTarget.Trim(); //this is for if the key is formatted in a special way
						while(oFile.db.eof()==0 && oFile.GetValString(sField).Trim()==sTarget)
						{
							if(oFile.db.deleted()==0)
							{
								oNewRow=oRetData.NewRow();
								
								if(oFields==null)
								{								
									foreach(DataColumn oCol in oRetData.Columns)								
									{
										if(oFile.IsDateTime(oCol.ColumnName))
										{
											if(FPUtil.GetCShDate(oFile.GetValString(oCol.ColumnName),out oDateTime))
												oNewRow[oCol.ColumnName]=oDateTime.ToShortDateString();																								
										}										
										else if(bProcessMemo && oFile.IsMemoField(oCol.ColumnName))
										{
											oNewRow[oCol.ColumnName]=oFile.GetField4Memo(oCol.ColumnName).str().Trim();
										}
										else
											oNewRow[oCol.ColumnName]=oFile.GetValString(oCol.ColumnName).Trim(); 																											
									}
								}
								else
								{
									for(int i = 0 ; i < oFields.Count ; i++)
									{
										sTemp=oFields[i].ToString();
										if(oFile.IsDateTime(sTemp))
										{
											if(FPUtil.GetCShDate(oFile.GetValString(sTemp),out oDateTime))
												oNewRow[sTemp]=oDateTime.ToShortDateString();																								
										}																				
										else if(bProcessMemo && oFile.IsMemoField(sTemp))
										{
											oNewRow[sTemp]=oFile.GetField4Memo(sTemp).str().Trim();
										}
										else
											oNewRow[sTemp]=oFile.GetValString(sTemp).Trim();									
									}
								}	
								
								
								oRetData.Rows.Add(oNewRow);
							}
							oFile.db.skip(1);
						}
					}				
					//CloseFile(oFile);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				CloseFile(oFile);
			}
			return oRetData;
		}

		
		private string GetMinMax(CCbDbf oFile,string sField, string sIndex,bool bMin)
		{
			string sRetVal="";
			if(oFile.db.isValid()!=0)
			{
				oFile.db.select(sIndex);
				if(!bMin)
					oFile.db.bottom();
				else
					oFile.db.top();
				sRetVal=oFile.GetValString(sField);
			}
			return sRetVal;
		}

		public void InsertFAQ(DataTable oTable)
		{
			try
			{
				ModiFile(this.sFAQ,"FAQNO","FAQNO",oTable,true,false,true);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public string InsertNewRxOrd(DataTable oTable)
		{
			try
			{
				return ModiFile(this.sNewRxOrd,"NEWRXORDID","NEWRXORDID",oTable,true,false,true);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public void InsertContact(DataTable oTable)
		{
			try
			{
				ModiFile(this.sWebContact,"CONTACTID","CONTACTID",oTable,true,true,true);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		
		public void InsertAccess(DataTable oTable)
		{
			try
			{
				ModiFile(this.sAccess,"SES_NO","",oTable,true,false);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public void UpdateAccess(DataTable oTable)
		{
			/*
			 * this function updates the access table with the logout date and time
			 */
			try
			{
				ModiFile(this.sAccess,"SES_NO","",oTable,false,true);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public string InsertAccess1(DataTable oTable)
		{
			/*
			 * i am duplicating this function for the time being to make it return the ses_no
			 */
			try
			{
				return ModiFile(this.sAccess,"SES_NO","SES_NO",oTable,true,true);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public string InsertLogin(DataTable oTable)
		{
			/*
			 * this creates an entry inot the login table
			 */
			try
			{
				return ModiFile(this.sWebLogin,"LOGINID","LOGINID",oTable,true,true);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public void InsertAdvImg(DataTable oTable)
		{
			try
			{
				ModiFile(this.sAdvImg,"IMGID","IMGID",oTable,true,true);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public void InsertRxQue(DataTable oTable)
		{
			try
			{
				ModiFile(this.sRxRefQue,"","",oTable,false,false,true);								
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		
		private string ModiFile(string sFileName, string sField,string sIndex, System.Data.DataTable dtData,bool bGenerateMax,bool bFindFirst)
		{
			try
			{
				return ModiFile(sFileName,sField,sIndex, dtData, bGenerateMax, bFindFirst, false);
			}
			catch(Exception ex)
			{
				throw ex;
			}

		}
		
		private string ModiFile(string sFileName, string sField,string sIndex, System.Data.DataTable dtData,bool bGenerateMax,bool bFindFirst,bool ProcessMemo)
		{
			/*
			 * This function is very generic in the sense, any file that needs to 
			 * be modified, or a new rec needs to be created either based on 
			 * some new value which needs to be determined or just a plain record
			 * needs to be created, modi file can be used. The new record
			 * will be created by plus 1 of the max of the field.
			 * */			 
			CCbDbf oFile=null;			
			//bool bRetVal=false;
			string sMax="";
			bool bFound=false;			
			bool bGenCond1=  bGenerateMax && bFindFirst;
			bool bGenCond2= bGenerateMax && !bFindFirst;
			DateTime oDtStart;
			TimeSpan oTs;
			string sFieldVal;
			try
			{				
				if(this.OpenFile(out oFile,sFileName,ProcessMemo))
				{																																																																																
					
					oDtStart=DateTime.Now;
					while(oFile.db.lockFile()!=0)
					{
						oTs=DateTime.Now.Subtract(oDtStart);
						if(oTs.Seconds >= 15)
						{
							throw new Exception("Unable To Get Lock Of File "+sFileName);		
						}
						System.Threading.Thread.Sleep(100);
					}
										
					for(int i=0;i<dtData.Rows.Count;i++)
					{																																		
						if(bFindFirst)
						{								
							if (this.Search(oFile,sIndex,dtData.Rows[i][sField].ToString()))
							{									
								bFound=true;
							}															
						}
						
						if ((!bFound && bGenCond1) || (bGenCond2))
						{														
							if(oFile.db.recCount()==0)
								sMax="0";
							else
								sMax=this.GetMinMax(oFile,sField,sIndex,false);												
							
							if(Util.IsNumeric(sMax))
							{
								dtData.Rows[i][sField]=sMax=Convert.ToString((Convert.ToInt32(sMax)+1));																						
								//SessNo=Convert.ToString((Convert.ToInt32(sMax)+1));
							}
						}											
						
						if(!bFound)
							oFile.db.appendBlank();

						foreach(DataColumn oColumn in dtData.Columns)
						{																																																									
							if(!(System.DBNull.Equals(System.DBNull.Value,dtData.Rows[i][oColumn.ColumnName])))	
							{	
								sFieldVal="";
								if((oFile.IsDateTime(oColumn.ColumnName) || oColumn.DataType.ToString() == "System.DateTime") && Util.IsDate(dtData.Rows[i][oColumn.ColumnName].ToString()))
									sFieldVal=FPUtil.GetFPDate(Convert.ToDateTime(dtData.Rows[i][oColumn.ColumnName].ToString()));								
								else if(oFile.IsNumeric(oColumn.ColumnName))
								{
									sFieldVal=dtData.Rows[i][oColumn.ColumnName].ToString().PadLeft(Convert.ToInt32(oFile.GetFieldLength(oColumn.ColumnName)),' ');		
								}
								else
									sFieldVal=dtData.Rows[i][oColumn.ColumnName].ToString();																

								if(oFile.IsMemoField(oColumn.ColumnName))
								{
									oFile.GetField4Memo(oColumn.ColumnName).assign(sFieldVal);
								}
								else
									oFile.GetValField4(oColumn.ColumnName).assign(sFieldVal);							
							
							}							
						}																		
						oFile.db.write();
						bFound=false;
					}									
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}			 																	
			finally
			{
				CloseFile(oFile);					
			}		
			return sMax;
		}

		private bool OpenFile(out CCbDbf oFile, string sFileName)
		{
			try
			{
				return OpenFile(out oFile,sFileName, false);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		private bool OpenFile(out CCbDbf oFile, string sFileName,bool bProcessMemo)
		{
			/*
			 * 
			 * This is the open file method, it opens any file and returns
			 * back an instantiated cbdbf object which is pointing to a
			 * valid file.
			 * */			
			oFile=new CCbDbf();	
			oFile.ProcessMemo = bProcessMemo;
			if(!(System.IO.File.Exists(sFileName)))
				throw new Exception("Error Opening File. The File "+sFileName+" Does Not Exist");						
			try
			{				
				oFile.OpenFile(sFileName);					
			}
			catch(Exception ex)
			{
				throw ex;
			}						
			return (oFile.db.isValid()!=0);
		}		
		
		private void CloseFile(CCbDbf oFile)		
		{
			/*
			 * If the file is open it will close the file.
			 * */
			if(oFile !=null && oFile.db.isValid()!=0)								
				oFile.db.close();
		}	
	
	}
}
