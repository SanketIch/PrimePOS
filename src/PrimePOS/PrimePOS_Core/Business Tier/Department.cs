using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
//using POS.Resources;
using Resources;
namespace POS_Core.BusinessRules
{
 
    /// <summary>
    /// This is business object of Department. It contains business rules of department.
    /// </summary>
    public class Department : IDisposable
    {

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating Departments data. 
        /// </summary>
        /// <param name="updates">It is department type dataset class. It contains all information of departments.</param>

        public void Persist(DepartmentData updates, ref System.Int32 DeptID)    //Sprint-22 20-Oct-2015 JY Added DeptID
        {
            try
            {
                using (DepartmentSvr dao = new DepartmentSvr())
                {
                    checkIsValidData(updates);
                    dao.Persist(updates, ref DeptID);   //Sprint-22 20-Oct-2015 JY Added DeptID
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool DeleteRow(string CurrentID)
        {
            try
            {
                using (DepartmentSvr oDepartmentSvr = new DepartmentSvr())
                {
                    return oDepartmentSvr.DeleteRow(CurrentID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region Get Methods
        /// <summary>
        /// Get typed dataset (DepartmentData) according to department code
        /// </summary>
        /// <param name="DeptCode">This code represent each department.</param>
        /// <returns>Department type dataset.</returns>
        public DepartmentData Populate(System.String DeptCode)
        {
            try
            {
                using (DepartmentSvr dao = new DepartmentSvr())
                {
                    return dao.Populate(DeptCode);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// Get departments type data set according to department code.
        /// </summary>
        /// <param name="DeptId">This code represent each department.</param>
        /// <returns>Department type dataset</returns>
        public DepartmentData Populate(System.Int32 DeptId)
        {
            try
            {
                Customer cus = new Customer();
                using (DepartmentSvr dao = new DepartmentSvr())
                {
                    return dao.Populate(DeptId);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// Get department type dataset with respect to condition.
        /// </summary>
        /// <param name="whereClause">Provide SQL where clause.</param>
        /// <returns>Department type dataset.</returns>
        public DepartmentData PopulateList(string whereClause)
        {
            try
            {
                using (DepartmentSvr dao = new DepartmentSvr())
                {
                    return dao.PopulateList(whereClause);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        //Added By Shitaljit(QuicSolv) 12 May 2011
        /// <summary>
        /// Get System Dataset with respect to condition.
        /// </summary>
        
        /// <retuurn> System Dataset.</returns>
        /// 
        public DataSet PopulateList()
        {
            try
            {
                using (DepartmentSvr dao = new DepartmentSvr())
                {
                    return dao.PopulateList();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public DataSet GetCLExcludedDeptData()
        {
            try
            {
                using (DepartmentSvr dao = new DepartmentSvr())
                {
                    return dao.PopulateListWithIdName(" where ExcludeFromCL=1");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public DataSet GetCLCouponExcludedDeptData()
        {
            try
            {
                using (DepartmentSvr dao = new DepartmentSvr())
                {
                    return dao.PopulateListWithIdName(" where ExcludeFromCL=1 and EXCLUDEFROMCLCouponPay=1 ");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #region PRIMEPOS-2500 02-Apr-2018 JY Added logic to check taxable department
        public bool IsTaxable(int DepartmentID)
        {
            try
            {
                using (DepartmentSvr oDepartmentSvr = new DepartmentSvr())
                {
                    return oDepartmentSvr.IsTaxable(DepartmentID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #endregion

        #region Validation Methods
        /// <summary>
        /// Check the departments data edited, added or unchanged is valid. Check code, sale price nad name.
        /// </summary>
        /// <param name="updates">Department type dataset.</param>
        public void checkIsValidData(DepartmentData updates)
        {
            DepartmentTable table;

            DepartmentRow oRow;

            oRow = (DepartmentRow)updates.Tables[0].Rows[0];

            table = (DepartmentTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (DepartmentTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((DepartmentTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((DepartmentTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (DepartmentTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((DepartmentTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((DepartmentTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

            foreach (DepartmentRow row in table.Rows)
            {
                //if (row.DeptCode.Trim()== "" || row.DeptCode==null)  
                //	ErrorHandler.throwCustomError(POSErrorENUM.Department_PrimaryKeyVoilation); 
                if (row.DeptCode.Trim() == "" || row.DeptCode == null)
                    ErrorHandler.throwCustomError(POSErrorENUM.Department_CodeCanNotBeNULL);
                if (row.SalePrice.ToString() == "")
                    ErrorHandler.throwCustomError(POSErrorENUM.Department_SalePriceCanNotBeNULL);
                if (row.DeptName.ToString() == "")
                    ErrorHandler.throwCustomError(POSErrorENUM.Department_NameCanNotBeNULL);
            }
        }

        /// <summary>
        /// It checks the voilation of primary key rules for department.
        /// </summary>
        /// <param name="updates">Department type dataset.</param>
        public virtual void checkIsValidPrimaryKey(DepartmentData updates)
        {
            DepartmentTable table = (DepartmentTable)updates.Tables[clsPOSDBConstants.Department_tbl];
            foreach (DepartmentRow row in table.Rows)
            {
                if (this.Populate(row.DeptCode).Tables[clsPOSDBConstants.Department_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for Department ");
                }
            }
        }

        #endregion
        /// <summary>
        /// Dispose all resources of department.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
