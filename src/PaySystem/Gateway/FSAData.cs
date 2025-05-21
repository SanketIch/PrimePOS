using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gateway
{
    /// <summary>
    /// Handles the Fsa Data 
    /// </summary>
    public class FSAData : IDisposable
    {
        //PrismPay.FSA ofsa = new PrismPay.FSA()
        #region "Private Fields"
        bool _IsFSA = false;
        float _RxAmount = 0.0f;
        float _ClinicAmount = 0.0f;
        float _VisionAmount = 0.0f;
        float _DentalAmount = 0.0f;
        #endregion


        #region "Properties"
        /// <summary>
        /// Boolean flag indicating if it is a FSA Transaction
        /// </summary>
        public bool IsFSA
        {
            set { this._IsFSA = value; }
        }

        /// <summary>
        /// Qualified Prescription Amount in the format 0.00
        /// </summary>
        public float RxAmount
        {
            set { this._RxAmount = value; }
        }


        /// <summary>
        /// Qualified Medical Clinic Amount in the format 0.00
        /// </summary>
        public float ClinicAmount
        {
            set { this._ClinicAmount = value; }
        }

        /// <summary>
        /// Qualified Vision Amount in the format 0.00
        /// </summary>
        public float VisionAmount
        {
            set { this._VisionAmount = value; }
        }

        /// <summary>
        /// Qualified Dental Amount  in the format 0.00
        /// </summary>
        public float DentalAmount
        {
            set { this._DentalAmount = value; }
        }


        #endregion

        public FSAData()
        {
        }

        public void LoadFSAData(PrismPay.FSA ofsa)
        {
            if (this._IsFSA)
            {
                ofsa.healthcareflag = 1;
                ofsa.rxamount = _RxAmount;
                ofsa.clinicamount = _ClinicAmount;
                ofsa.visionamount = _VisionAmount;
                ofsa.dentalamount = _DentalAmount;
            }
        }
        public void LoadFSAData(WorldPay.FSA ofsa)
        {
            if (this._IsFSA)
            {
                ofsa.healthcareflag = 1;
                ofsa.rxamount = _RxAmount;
                ofsa.clinicamount = _ClinicAmount;
                ofsa.visionamount = _VisionAmount;
                ofsa.dentalamount = _DentalAmount;
            }
        }


        #region "iDisposible Members"
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.IsFSA = false;
                this.RxAmount = 0.0f;
                this.ClinicAmount = 0.0f;
                this.VisionAmount = 0.0f;
                this.DentalAmount = 0.0f;
                //oCardInfo = null;
                //OGatewayClient = null;


            }
            //GC.Collect();

        }
        #endregion

    }
}
