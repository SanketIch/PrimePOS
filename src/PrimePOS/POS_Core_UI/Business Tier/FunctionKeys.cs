
namespace POS_Core.BusinessRules
{
    using System;
    using System.Data;
    using POS_Core.DataAccess;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;
    using POS_Core.ErrorLogging;
    //using POS.UI;
    using POS.Resources;
    using System.Windows.Forms;
    using NLog;
    using POS_Core_UI;
    using Resources;

    /// <summary>
    /// This is business object class for function keys.
    /// Functionkeys is the collection of key combinations for buttons.
    /// It also contains button back and fore color properties description.
    /// </summary>
    public class FunctionKeys : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating FunctionKeys data. 
        /// </summary>
        /// <param name="updates">It is FunctionKeys type dataset class. It contains all information of FunctionKeys.</param>       
        public void Persist(FunctionKeysData updates)
        {
            try
            {
                using (FunctionKeysSvr dao = new FunctionKeysSvr())
                {
                    dao.Persist(updates);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(FunctionKeysData updates)");
                throw (ex);
            }
        }
        #endregion

        #region Get Methods
        /// <summary>
        /// Fills a FunctionKeys type DataSet with all FunctionKeys based on a Function Key.
        /// </summary>
        /// <param name="FunKey">Function key combination.</param>
        /// <returns>FunctionKeysData type dataset</returns>
        public FunctionKeysData Populate(System.String FunKey)
        {
            try
            {
                using (FunctionKeysSvr dao = new FunctionKeysSvr())
                {
                    return dao.Populate(FunKey);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.String FunKey)");
                throw (ex);
            }
        }
        /// <summary>
        /// Get collection of function keys on the bases of SQL WHERE CLUASE
        /// </summary>
        /// <param name="whereClause">Where clause description.</param>
        /// <returns>functionkeys type dataset.</returns>
        public FunctionKeysData PopulateList(string whereClause)
        {
            try
            {
                using (FunctionKeysSvr dao = new FunctionKeysSvr())
                {
                    return dao.PopulateList(whereClause);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList(string whereClause)");
                throw (ex);
            }
        }
        /// <summary>
        /// Author: Shitaljit 
        /// Will get all the Fucntions key define as Parent
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateParents()
        {
            try
            {
                using (FunctionKeysSvr dao = new FunctionKeysSvr())
                {
                    return dao.PopulateParents();
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateParents()");
                throw (ex);
            }
        }

        /// <summary>
        /// Author:Shitaljit created date: 18May13
        /// To delete a perticular row
        /// </summary>
        /// <param name="KeyId"></param>
        /// <returns></returns>
        public bool DeleteRow(int KeyId)
        {
            try
            {
                using (FunctionKeysSvr dao = new FunctionKeysSvr())
                {
                    return dao.DeleteRow(KeyId);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteRow(int KeyId)");
                throw (ex);
            }
        }
        /// <summary>
        /// Author:Shitaljit created date: 18May13
        /// To update a perticular row
        /// </summary>
        /// <param name="KeyId"></param>
        /// <returns></returns>
        public bool UpdateRow(string FuncKey)
        {
            try
            {
                using (FunctionKeysSvr dao = new FunctionKeysSvr())
                {
                    return dao.UpdateRow(FuncKey);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "UpdateRow(string FuncKey)");
                throw (ex);
            }
        }
        #endregion
        /// <summary>
        /// Author: Shitaljit
        /// To get next Position of Function Keys
        /// </summary>
        /// <param name="ParameterName"></param>
        /// <returns></returns>
        public Int32 GetNextKeyPosition(string ParameterName, Int32 ParentID)
        {
            try
            {
                using (FunctionKeysSvr dao = new FunctionKeysSvr())
                {
                    return dao.GetNextKeyPosition(ParameterName, ParentID);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetNextKeyPosition(string ParameterName, Int32 ParentID)");
                throw (ex);
            }
        }

        #region Validation Methods
        /*		public void checkIsValidData(FunctionKeysData updates) 
		{
			FunctionKeysTable table;

			FunctionKeysRow oRow ;
				
			oRow = (FunctionKeysRow)updates.Tables[0].Rows[0];

			table = (FunctionKeysTable)updates.Tables[0].GetChanges(DataRowState.Added);

			if (table == null)
				table= (FunctionKeysTable)updates.Tables[0].GetChanges(DataRowState.Modified);
			else if ((FunctionKeysTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
				table.MergeTable ((FunctionKeysTable)updates.Tables[0].GetChanges(DataRowState.Modified));
		  
			if (table == null)
				table= (FunctionKeysTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
			else if ((FunctionKeysTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
				table.MergeTable( (FunctionKeysTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

			foreach(FunctionKeysRow row in table.Rows)
			{ 
				//if (row.DeptCode.Trim()== "" || row.DeptCode==null)  
				//	ErrorHandler.throwCustomError(POSErrorENUM.FunctionKeys_PrimaryKeyVoilation); 
				if (row.DeptCode.Trim() == "" || row.DeptCode==null)
					ErrorHandler.throwCustomError(POSErrorENUM.FunctionKeys_CodeCanNotBeNULL); 
				if (row.SalePrice.ToString()=="")
					ErrorHandler.throwCustomError(POSErrorENUM.FunctionKeys_SalePriceCanNotBeNULL); 
				if (row.DeptName.ToString()=="")
					ErrorHandler.throwCustomError(POSErrorENUM.Customer_NameCanNotBeNULL); 
			}
		} 

		public  void checkIsValidPrimaryKey(FunctionKeysData updates) 
		{
			FunctionKeysTable table = (FunctionKeysTable)updates.Tables[clsPOSDBConstants.FunctionKeys_tbl];
			foreach(FunctionKeysRow row in table.Rows)
			{ 
				if (this.Populate(row.DeptCode).Tables[clsPOSDBConstants.FunctionKeys_tbl].Rows.Count != 0)
				{ 
					throw new Exception ("Primary key violation for FunctionKeys ");
				}		
			} 
		} */

        #endregion
        /// <summary>
        /// Free all resources of functionKeys object.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }

    public class FunKeyCommonOperations
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public static void RemoveKey(string CurrentKey)
        {
            FunctionKeys oFKeys = new FunctionKeys();
            bool isSsucess = false;
            try
            {
                if (clsUIHelper.isNumeric(CurrentKey))
                {
                    isSsucess = oFKeys.DeleteRow(Configuration.convertNullToInt(CurrentKey));
                }
                else
                {
                    isSsucess = oFKeys.UpdateRow(CurrentKey);
                }
                if (isSsucess == false)
                {
                    throw new Exception("Cannot remove function key.");
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "RemoveKey(string CurrentKey)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        public static void AddKeys(FunctionKeysRow oFKRow)
        {
            frmFunctionKeys ofrmFK = new frmFunctionKeys();
            try
            {
                ofrmFK.AddKey(oFKRow);
                ofrmFK.ShowDialog();
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "AddKeys(FunctionKeysRow oFKRow)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        public static void EditKeys(string CurrentKey)
        {
            frmFunctionKeys ofrmFK = new frmFunctionKeys();
            FunctionKeys oFKeys = new FunctionKeys();
            FunctionKeysData oFKData = new FunctionKeysData();
            string whereClause = string.Empty;
            try
            {
                if (clsUIHelper.isNumeric(CurrentKey) == true)
                {
                    whereClause = " where KeyId='" + CurrentKey + "'";
                }
                else
                {
                    whereClause = " where FunKey='" + CurrentKey + "'";
                }
                oFKData = oFKeys.PopulateList(whereClause);
                if (oFKData.FunctionKeys.Rows.Count > 0)
                {
                    ofrmFK.EditKey((FunctionKeysRow)oFKData.FunctionKeys.Rows[0]);
                    ofrmFK.ShowDialog();
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "EditKeys(string CurrentKey)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        public static string GetITemCode(string FKey)
        {
            FunctionKeys oFKeys = new FunctionKeys();
            string ItemCode = string.Empty;
            string whereClause = string.Empty;
            try
            {
                if (clsUIHelper.isNumeric(FKey) == true)
                {
                    whereClause = " where KeyId='" + FKey + "'";
                }
                else
                {
                    whereClause = " where FunKey='" + FKey + "'";
                }
                FunctionKeysData oFKData = oFKeys.PopulateList(whereClause);

                if (oFKData.FunctionKeys.Rows.Count > 0)
                {
                    if (oFKData.FunctionKeys.Rows[0][clsPOSDBConstants.FunctionKeys_Fld_Operation].ToString().Trim() != "")
                    {
                        if (oFKData.FunctionKeys.Rows[0][clsPOSDBConstants.FunctionKeys_Fld_FunctionType].ToString().Equals(clsPOSDBConstants.FunctionKeys_Type_Item))
                        {
                            string strKey = oFKData.FunctionKeys.Rows[0][clsPOSDBConstants.FunctionKeys_Fld_FunKey].ToString().Trim();
                            if (ItemCode.Trim() == "")
                            {
                                ItemCode = oFKData.FunctionKeys.Rows[0][clsPOSDBConstants.FunctionKeys_Fld_Operation].ToString().Trim();
                            }
                            else if (clsUIHelper.isNumeric(ItemCode))
                            {
                                ItemCode += "@" + oFKData.FunctionKeys.Rows[0][clsPOSDBConstants.FunctionKeys_Fld_Operation].ToString().Trim();
                            }
                            else if (strKey == "Shift+Q" || strKey == "Shift+W")
                            {
                                ItemCode = ItemCode.Substring(0, ItemCode.Length - 1);
                                ItemCode += "@" + oFKData.FunctionKeys.Rows[0][clsPOSDBConstants.FunctionKeys_Fld_Operation].ToString().Trim();
                            }
                            else
                            {
                                ItemCode += "@" + oFKData.FunctionKeys.Rows[0][clsPOSDBConstants.FunctionKeys_Fld_Operation].ToString().Trim();
                            }

                        }
                    }
                }
                return ItemCode;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetITemCode(string FKey)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
                return ItemCode;
            }

        }
        public static string getItemName(string itemId)
        {
            try
            {
                Item oItem = new Item();
                ItemData oItemData;
                ItemRow oItemRow;
                if (itemId == "")
                {
                    return "";
                }
                oItemData = oItem.Populate(itemId);
                if (oItemData.Tables[0].Rows.Count > 0)
                    oItemRow = oItemData.Item[0];
                else
                    return "";

                return oItemRow.Description.Trim();
            }
            catch (Exception)
            { return ""; }
        }
    }
}