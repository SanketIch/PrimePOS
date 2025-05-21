
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

namespace MMS.DataFeed.Infrastructure
{

    public class FactoryPattern<K, T> where T : class, K, new()
    {
        public static K CreateInstance()
        {
            K objK;

            objK = new T();            
            return objK;
        }
    }

    public class FactoryC
    {
        public static IDataFeedSource CreateInstance(DataFeedSourceEnum enumModuleName)
        {
            IDataFeedSource objActivity = null;

            switch (enumModuleName)
            {
                case DataFeedSourceEnum.OutcomesMTM:
                    objActivity = FactoryPattern<IDataFeedSource, OutcomesMTMDataFeed>.CreateInstance();
                    break;
                default:
                    break;
            }
            return objActivity;
        }
    }
    public enum DataFeedSourceEnum
    {
        OutcomesMTM = 1,
    }
    public interface IDataFeedSource : IDisposable
    {
        IDataFeed GetDataFeed(IDataFeedParam param);
    }
    public class OutcomesMTMDataFeed : IDataFeedSource, IDisposable
    {
        // List<DataFeed> dFeeds = new List<DataFeed>();
        public IDataFeed GetDataFeed(IDataFeedParam param)
        {
            //Here write your logic to load data from database and send back;



            //DataFeed dFeed = new DataFeed.MTMDataFeed();
            // dFeeds.Add(dFeed);
            // return dFeeds;       
            return new MTMDataFeed() { PatientId="1"};
        }
        #region IDisposable
        public void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                DisposeManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            DisposeUnManagedResources();
        }
        private void DisposeManagedResources()
        {

        }
        private void DisposeUnManagedResources()
        {

        }
        #endregion
    }
    public abstract class DataFeedFactory : IDisposable
    {
        public abstract IDataFeedSource GetDataFeedSource(DataFeedSourceEnum source);
        public abstract void Dispose();

    }
    public class MTMDataFeedFactory : DataFeedFactory, IDisposable
    {
        public override IDataFeedSource GetDataFeedSource(DataFeedSourceEnum source)
        {
            switch (source)
            {
                case DataFeedSourceEnum.OutcomesMTM:
                    return new OutcomesMTMDataFeed();
                default:
                    throw new NotImplementedException("This type of datafeed can't be created.");
            }
        }
        #region IDisposable
        public override void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                DisposeManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            DisposeUnManagedResources();
        }
        private void DisposeManagedResources()
        {

        }
        private void DisposeUnManagedResources()
        {

        }
        #endregion
    }
    public interface IDataFeedParam { }
    public class MTMDataFeedParam : IDataFeedParam { }
    public interface IDataFeed { }
    public class MTMDataFeed : IDataFeed
    {
        public char RecordIdentifier { get; set; }
        public string PharmacyNCPDP { get; set; }
        public string PatientId { get; set; }
        public int PolicyID { get; set; }
        public int ClientID { get; set; }
        public string PolicyName { get; set; }
        public string PlanType { get; set; }

        public string CMSContractNumber { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientMiddleInitial { get; set; }
        public string PatientLastName { get; set; }
        public DateTime PatientDOB { get; set; }
        public char PatientGender { get; set; }
        public string PatientAddress1 { get; set; }
        public string PatientAddress2 { get; set; }
        public string PatientCity { get; set; }
        public string PatientState { get; set; }
        public int PatientZip { get; set; }
        public string PatientHomePhone { get; set; }
        public bool PrimaryPatient { get; set; }
        public bool NeedsCMR { get; set; }
        public int TIPs { get; set; }
        public bool AdherenceCheckPoint { get; set; }
        public int UnfinishedClaims { get; set; }
        public int ResubmitClaims { get; set; }
        public int PatientPriority { get; set; }
        public string Filler1 { get; set; }
        public string Filler2 { get; set; }
        public string Filler3 { get; set; }
    }
    public class DataFeedController : IDisposable
    {
        public void GetDataFeed(DataFeedSourceEnum source)
        {
            using (DataFeedFactory factory = new MTMDataFeedFactory())
            {
                using (var outcomesMTMDatafeed = factory.GetDataFeedSource(DataFeedSourceEnum.OutcomesMTM))
                {
                    var datafeed = outcomesMTMDatafeed.GetDataFeed(new MTMDataFeedParam() { });

                    // return
                }
            }
        }
        public int Process()
        {
            IDataFeedSource objActivity;
            objActivity = FactoryC.CreateInstance(DataFeedSourceEnum.OutcomesMTM);
            MTMDataFeed feed=(MTMDataFeed)objActivity.GetDataFeed(new MTMDataFeedParam() { });
            return 1;
        }
        #region IDisposable
        public void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                DisposeManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            DisposeUnManagedResources();
        }
        private void DisposeManagedResources()
        {

        }
        private void DisposeUnManagedResources()
        {

        }
        #endregion
    }
}
