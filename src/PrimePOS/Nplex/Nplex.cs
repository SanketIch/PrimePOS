using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Services;
using System.Diagnostics;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Nplex
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "RetailServicesWSPortBinding", Namespace = "http://services.methcheck.appriss.com/")]
    public partial class RetailServicesWS : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback doInquiryOperationCompleted;

        private System.Threading.SendOrPostCallback doPurchaseOperationCompleted;

        private System.Threading.SendOrPostCallback doReturnOperationCompleted;

        private System.Threading.SendOrPostCallback doVoidOperationCompleted;

        private System.Threading.SendOrPostCallback doSignatureOperationCompleted;

        private System.Threading.SendOrPostCallback parseIdOperationCompleted;

        /// <remarks/>
        public RetailServicesWS()
        {
            //this.Url = "https://methcheck.prep.appriss.com/RetailServicesWS/RetailWebService?wsdl";   //This is testing URL
            this.Url = "https://methcheck.appriss.com/RetailServicesWS/RetailWebService?wsdl"; //This is Production URL
        }

        /// <remarks/>
        public event doInquiryCompletedEventHandler doInquiryCompleted;

        /// <remarks/>
        public event doPurchaseCompletedEventHandler doPurchaseCompleted;

        /// <remarks/>
        public event doReturnCompletedEventHandler doReturnCompleted;

        /// <remarks/>
        public event doVoidCompletedEventHandler doVoidCompleted;

        /// <remarks/>
        public event doSignatureCompletedEventHandler doSignatureCompleted;

        /// <remarks/>
        public event parseIdCompletedEventHandler parseIdCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://services.methcheck.appriss.com/", ResponseNamespace = "http://services.methcheck.appriss.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("inquiryResult", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public InquiryResponseType doInquiry([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] InquiryRequestType inquiryRequest)
        {
            object[] results = this.Invoke("doInquiry", new object[] {
                        inquiryRequest});
            return ((InquiryResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BegindoInquiry(InquiryRequestType inquiryRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("doInquiry", new object[] {
                        inquiryRequest}, callback, asyncState);
        }

        /// <remarks/>
        public InquiryResponseType EnddoInquiry(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((InquiryResponseType)(results[0]));
        }

        /// <remarks/>
        public void doInquiryAsync(InquiryRequestType inquiryRequest)
        {
            this.doInquiryAsync(inquiryRequest, null);
        }

        /// <remarks/>
        public void doInquiryAsync(InquiryRequestType inquiryRequest, object userState)
        {
            if ((this.doInquiryOperationCompleted == null))
            {
                this.doInquiryOperationCompleted = new System.Threading.SendOrPostCallback(this.OndoInquiryOperationCompleted);
            }
            this.InvokeAsync("doInquiry", new object[] {
                        inquiryRequest}, this.doInquiryOperationCompleted, userState);
        }

        private void OndoInquiryOperationCompleted(object arg)
        {
            if ((this.doInquiryCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.doInquiryCompleted(this, new doInquiryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://services.methcheck.appriss.com/", ResponseNamespace = "http://services.methcheck.appriss.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("purchaseResult", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PurchaseResponseType doPurchase([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] PurchaseRequestType purchaseRequest)
        {
            object[] results = this.Invoke("doPurchase", new object[] {
                        purchaseRequest});
            return ((PurchaseResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BegindoPurchase(PurchaseRequestType purchaseRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("doPurchase", new object[] {
                        purchaseRequest}, callback, asyncState);
        }

        /// <remarks/>
        public PurchaseResponseType EnddoPurchase(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((PurchaseResponseType)(results[0]));
        }

        /// <remarks/>
        public void doPurchaseAsync(PurchaseRequestType purchaseRequest)
        {
            this.doPurchaseAsync(purchaseRequest, null);
        }

        /// <remarks/>
        public void doPurchaseAsync(PurchaseRequestType purchaseRequest, object userState)
        {
            if ((this.doPurchaseOperationCompleted == null))
            {
                this.doPurchaseOperationCompleted = new System.Threading.SendOrPostCallback(this.OndoPurchaseOperationCompleted);
            }
            this.InvokeAsync("doPurchase", new object[] {
                        purchaseRequest}, this.doPurchaseOperationCompleted, userState);
        }

        private void OndoPurchaseOperationCompleted(object arg)
        {
            if ((this.doPurchaseCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.doPurchaseCompleted(this, new doPurchaseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://services.methcheck.appriss.com/", ResponseNamespace = "http://services.methcheck.appriss.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("returnResult", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ReturnResponseType doReturn([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] ReturnRequestType returnRequest)
        {
            object[] results = this.Invoke("doReturn", new object[] {
                        returnRequest});
            return ((ReturnResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BegindoReturn(ReturnRequestType returnRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("doReturn", new object[] {
                        returnRequest}, callback, asyncState);
        }

        /// <remarks/>
        public ReturnResponseType EnddoReturn(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((ReturnResponseType)(results[0]));
        }

        /// <remarks/>
        public void doReturnAsync(ReturnRequestType returnRequest)
        {
            this.doReturnAsync(returnRequest, null);
        }

        /// <remarks/>
        public void doReturnAsync(ReturnRequestType returnRequest, object userState)
        {
            if ((this.doReturnOperationCompleted == null))
            {
                this.doReturnOperationCompleted = new System.Threading.SendOrPostCallback(this.OndoReturnOperationCompleted);
            }
            this.InvokeAsync("doReturn", new object[] {
                        returnRequest}, this.doReturnOperationCompleted, userState);
        }

        private void OndoReturnOperationCompleted(object arg)
        {
            if ((this.doReturnCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.doReturnCompleted(this, new doReturnCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://services.methcheck.appriss.com/", ResponseNamespace = "http://services.methcheck.appriss.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("voidResult", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public VoidResponseType doVoid([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] VoidRequestType voidRequest)
        {
            object[] results = this.Invoke("doVoid", new object[] {
                        voidRequest});
            return ((VoidResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BegindoVoid(VoidRequestType voidRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("doVoid", new object[] {
                        voidRequest}, callback, asyncState);
        }

        /// <remarks/>
        public VoidResponseType EnddoVoid(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((VoidResponseType)(results[0]));
        }

        /// <remarks/>
        public void doVoidAsync(VoidRequestType voidRequest)
        {
            this.doVoidAsync(voidRequest, null);
        }

        /// <remarks/>
        public void doVoidAsync(VoidRequestType voidRequest, object userState)
        {
            if ((this.doVoidOperationCompleted == null))
            {
                this.doVoidOperationCompleted = new System.Threading.SendOrPostCallback(this.OndoVoidOperationCompleted);
            }
            this.InvokeAsync("doVoid", new object[] {
                        voidRequest}, this.doVoidOperationCompleted, userState);
        }

        private void OndoVoidOperationCompleted(object arg)
        {
            if ((this.doVoidCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.doVoidCompleted(this, new doVoidCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://services.methcheck.appriss.com/", ResponseNamespace = "http://services.methcheck.appriss.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("signatureResult", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SignatureResponseType doSignature([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] SignatureRequestType SignatureRequest)
        {
            object[] results = this.Invoke("doSignature", new object[] {
                        SignatureRequest});
            return ((SignatureResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BegindoSignature(SignatureRequestType SignatureRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("doSignature", new object[] {
                        SignatureRequest}, callback, asyncState);
        }

        /// <remarks/>
        public SignatureResponseType EnddoSignature(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SignatureResponseType)(results[0]));
        }

        /// <remarks/>
        public void doSignatureAsync(SignatureRequestType SignatureRequest)
        {
            this.doSignatureAsync(SignatureRequest, null);
        }

        /// <remarks/>
        public void doSignatureAsync(SignatureRequestType SignatureRequest, object userState)
        {
            if ((this.doSignatureOperationCompleted == null))
            {
                this.doSignatureOperationCompleted = new System.Threading.SendOrPostCallback(this.OndoSignatureOperationCompleted);
            }
            this.InvokeAsync("doSignature", new object[] {
                        SignatureRequest}, this.doSignatureOperationCompleted, userState);
        }

        private void OndoSignatureOperationCompleted(object arg)
        {
            if ((this.doSignatureCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.doSignatureCompleted(this, new doSignatureCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://services.methcheck.appriss.com/", ResponseNamespace = "http://services.methcheck.appriss.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("parseResult", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public IdParseResponseType parseId([System.Xml.Serialization.XmlElementAttribute("parseId", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] IdParseRequestType parseId1)
        {
            object[] results = this.Invoke("parseId", new object[] {
                        parseId1});
            return ((IdParseResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginparseId(IdParseRequestType parseId1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("parseId", new object[] {
                        parseId1}, callback, asyncState);
        }

        /// <remarks/>
        public IdParseResponseType EndparseId(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((IdParseResponseType)(results[0]));
        }

        /// <remarks/>
        public void parseIdAsync(IdParseRequestType parseId1)
        {
            this.parseIdAsync(parseId1, null);
        }

        /// <remarks/>
        public void parseIdAsync(IdParseRequestType parseId1, object userState)
        {
            if ((this.parseIdOperationCompleted == null))
            {
                this.parseIdOperationCompleted = new System.Threading.SendOrPostCallback(this.OnparseIdOperationCompleted);
            }
            this.InvokeAsync("parseId", new object[] {
                        parseId1}, this.parseIdOperationCompleted, userState);
        }

        private void OnparseIdOperationCompleted(object arg)
        {
            if ((this.parseIdCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.parseIdCompleted(this, new parseIdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class InquiryRequestType
    {

        private PersonIdentifierType personIdentifierField;

        private StoreType storeField;

        private ProductType[] productsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PersonIdentifierType personIdentifier
        {
            get
            {
                return this.personIdentifierField;
            }
            set
            {
                this.personIdentifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public StoreType store
        {
            get
            {
                return this.storeField;
            }
            set
            {
                this.storeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("product", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ProductType[] products
        {
            get
            {
                return this.productsField;
            }
            set
            {
                this.productsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class PersonIdentifierType
    {

        private object itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("idScan", typeof(string), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("personInfo", typeof(PersonInfoType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class PersonInfoType
    {

        private string idField;

        private string idTypeField;

        private string idIssuingAgencyField;

        private string idExpirationField;

        private string lastNameField;

        private string middleNameField;

        private string firstNameField;

        private string suffixField;

        private string birthDateField;

        private string addressLine1Field;

        private string addressLine2Field;

        private string cityField;

        private string stateField;

        private string postalCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string idType
        {
            get
            {
                return this.idTypeField;
            }
            set
            {
                this.idTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string idIssuingAgency
        {
            get
            {
                return this.idIssuingAgencyField;
            }
            set
            {
                this.idIssuingAgencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string idExpiration
        {
            get
            {
                return this.idExpirationField;
            }
            set
            {
                this.idExpirationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string lastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string middleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string firstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string suffix
        {
            get
            {
                return this.suffixField;
            }
            set
            {
                this.suffixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string birthDate
        {
            get
            {
                return this.birthDateField;
            }
            set
            {
                this.birthDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string addressLine1
        {
            get
            {
                return this.addressLine1Field;
            }
            set
            {
                this.addressLine1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string addressLine2
        {
            get
            {
                return this.addressLine2Field;
            }
            set
            {
                this.addressLine2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string city
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string postalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class IdParseResponseType
    {

        private PersonInfoType personField;

        private ResultCodeType trxStatusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PersonInfoType person
        {
            get
            {
                return this.personField;
            }
            set
            {
                this.personField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ResultCodeType trxStatus
        {
            get
            {
                return this.trxStatusField;
            }
            set
            {
                this.trxStatusField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class ResultCodeType
    {

        private int resultCodeField;

        private string errorMsgField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int resultCode
        {
            get
            {
                return this.resultCodeField;
            }
            set
            {
                this.resultCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string errorMsg
        {
            get
            {
                return this.errorMsgField;
            }
            set
            {
                this.errorMsgField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class IdParseRequestType
    {

        private object itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("idScan", typeof(string), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("store", typeof(StoreType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class StoreType
    {

        private string itemField;

        private ItemChoiceType itemElementNameField;

        private string siteIdField;

        private string storeTechField;

        private string pharmacistApprovalField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ncpdp", typeof(string), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("storeId", typeof(string), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string siteId
        {
            get
            {
                return this.siteIdField;
            }
            set
            {
                this.siteIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string storeTech
        {
            get
            {
                return this.storeTechField;
            }
            set
            {
                this.storeTechField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pharmacistApproval
        {
            get
            {
                return this.pharmacistApprovalField;
            }
            set
            {
                this.pharmacistApprovalField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/", IncludeInSchema = false)]
    public enum ItemChoiceType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":ncpdp")]
        ncpdp,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":storeId")]
        storeId,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class SignatureResponseType
    {

        private string pseTrxIdField;

        private string storeTrxIdField;

        private System.DateTime timeField;

        private bool timeFieldSpecified;

        private ResultCodeType trxStatusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer")]
        public string pseTrxId
        {
            get
            {
                return this.pseTrxIdField;
            }
            set
            {
                this.pseTrxIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string storeTrxId
        {
            get
            {
                return this.storeTrxIdField;
            }
            set
            {
                this.storeTrxIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool timeSpecified
        {
            get
            {
                return this.timeFieldSpecified;
            }
            set
            {
                this.timeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ResultCodeType trxStatus
        {
            get
            {
                return this.trxStatusField;
            }
            set
            {
                this.trxStatusField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class SignatureRequestType
    {

        private string itemField;

        private ItemChoiceType2 itemElementNameField;

        private signatureType signatureField;

        private StoreType storeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("pseTrxId", typeof(string), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("storeTrxId", typeof(string), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType2 ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public signatureType signature
        {
            get
            {
                return this.signatureField;
            }
            set
            {
                this.signatureField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public StoreType store
        {
            get
            {
                return this.storeField;
            }
            set
            {
                this.storeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/", IncludeInSchema = false)]
    public enum ItemChoiceType2
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":pseTrxId")]
        pseTrxId,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":storeTrxId")]
        storeTrxId,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class signatureType
    {

        private string mimeTypeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string mimeType
        {
            get
            {
                return this.mimeTypeField;
            }
            set
            {
                this.mimeTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class VoidResponseType
    {

        private System.DateTime storeVoidTimeField;

        private bool storeVoidTimeFieldSpecified;

        private System.DateTime timeField;

        private bool timeFieldSpecified;

        private ResultCodeType trxStatusField;

        private string resultField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime storeVoidTime
        {
            get
            {
                return this.storeVoidTimeField;
            }
            set
            {
                this.storeVoidTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool storeVoidTimeSpecified
        {
            get
            {
                return this.storeVoidTimeFieldSpecified;
            }
            set
            {
                this.storeVoidTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool timeSpecified
        {
            get
            {
                return this.timeFieldSpecified;
            }
            set
            {
                this.timeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ResultCodeType trxStatus
        {
            get
            {
                return this.trxStatusField;
            }
            set
            {
                this.trxStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class VoidRequestType
    {

        private string itemField;

        private ItemChoiceType1 itemElementNameField;

        private System.DateTime storeVoidTimeField;

        private StoreType storeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("pseTrxId", typeof(string), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer")]
        [System.Xml.Serialization.XmlElementAttribute("storeVoidId", typeof(string), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType1 ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime storeVoidTime
        {
            get
            {
                return this.storeVoidTimeField;
            }
            set
            {
                this.storeVoidTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public StoreType store
        {
            get
            {
                return this.storeField;
            }
            set
            {
                this.storeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/", IncludeInSchema = false)]
    public enum ItemChoiceType1
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":pseTrxId")]
        pseTrxId,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":storeVoidId")]
        storeVoidId,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class ReturnResponseType
    {

        private string pseRtrnIdField;

        private string storeRtrnIdField;

        private System.DateTime storeRtrnTimeField;

        private bool storeRtrnTimeFieldSpecified;

        private System.DateTime timeField;

        private bool timeFieldSpecified;

        private ResultCodeType trxStatusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer")]
        public string pseRtrnId
        {
            get
            {
                return this.pseRtrnIdField;
            }
            set
            {
                this.pseRtrnIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string storeRtrnId
        {
            get
            {
                return this.storeRtrnIdField;
            }
            set
            {
                this.storeRtrnIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime storeRtrnTime
        {
            get
            {
                return this.storeRtrnTimeField;
            }
            set
            {
                this.storeRtrnTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool storeRtrnTimeSpecified
        {
            get
            {
                return this.storeRtrnTimeFieldSpecified;
            }
            set
            {
                this.storeRtrnTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool timeSpecified
        {
            get
            {
                return this.timeFieldSpecified;
            }
            set
            {
                this.timeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ResultCodeType trxStatus
        {
            get
            {
                return this.trxStatusField;
            }
            set
            {
                this.trxStatusField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class ReturnIdentifierType
    {

        private object itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("personIdentifier", typeof(PersonIdentifierType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("pseTrxId", typeof(string), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class ReturnRequestType
    {

        private ReturnIdentifierType personIdentifierField;

        private string storeRtrnIdField;

        private System.DateTime storeRtrnTimeField;

        private StoreType storeField;

        private ProductType[] productsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ReturnIdentifierType personIdentifier
        {
            get
            {
                return this.personIdentifierField;
            }
            set
            {
                this.personIdentifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string storeRtrnId
        {
            get
            {
                return this.storeRtrnIdField;
            }
            set
            {
                this.storeRtrnIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime storeRtrnTime
        {
            get
            {
                return this.storeRtrnTimeField;
            }
            set
            {
                this.storeRtrnTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public StoreType store
        {
            get
            {
                return this.storeField;
            }
            set
            {
                this.storeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("product", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ProductType[] products
        {
            get
            {
                return this.productsField;
            }
            set
            {
                this.productsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class ProductType
    {

        private string upcField;

        private string nameField;

        private string ndcField;

        private float gramsField;

        private int pillsField;

        private bool pillsFieldSpecified;

        private int dosagesField;

        private bool dosagesFieldSpecified;

        private string typeField;

        private System.Nullable<bool> pediatricIndField;

        private int boxCountField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string upc
        {
            get
            {
                return this.upcField;
            }
            set
            {
                this.upcField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ndc
        {
            get
            {
                return this.ndcField;
            }
            set
            {
                this.ndcField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public float grams
        {
            get
            {
                return this.gramsField;
            }
            set
            {
                this.gramsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int pills
        {
            get
            {
                return this.pillsField;
            }
            set
            {
                this.pillsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pillsSpecified
        {
            get
            {
                return this.pillsFieldSpecified;
            }
            set
            {
                this.pillsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int dosages
        {
            get
            {
                return this.dosagesField;
            }
            set
            {
                this.dosagesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dosagesSpecified
        {
            get
            {
                return this.dosagesFieldSpecified;
            }
            set
            {
                this.dosagesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<bool> pediatricInd
        {
            get
            {
                return this.pediatricIndField;
            }
            set
            {
                this.pediatricIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int boxCount
        {
            get
            {
                return this.boxCountField;
            }
            set
            {
                this.boxCountField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class PurchaseResponseType
    {

        private string pseTrxIdField;

        private string storeTrxIdField;

        private System.DateTime pseTrxTimeField;

        private bool pseTrxTimeFieldSpecified;

        private System.DateTime timeField;

        private bool timeFieldSpecified;

        private ComplianceResultType pseResultField;

        private limitType[] limitsField;

        private warningType[] warningsField;

        private ResultCodeType trxStatusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer")]
        public string pseTrxId
        {
            get
            {
                return this.pseTrxIdField;
            }
            set
            {
                this.pseTrxIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string storeTrxId
        {
            get
            {
                return this.storeTrxIdField;
            }
            set
            {
                this.storeTrxIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime pseTrxTime
        {
            get
            {
                return this.pseTrxTimeField;
            }
            set
            {
                this.pseTrxTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pseTrxTimeSpecified
        {
            get
            {
                return this.pseTrxTimeFieldSpecified;
            }
            set
            {
                this.pseTrxTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool timeSpecified
        {
            get
            {
                return this.timeFieldSpecified;
            }
            set
            {
                this.timeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ComplianceResultType pseResult
        {
            get
            {
                return this.pseResultField;
            }
            set
            {
                this.pseResultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("limit", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public limitType[] limits
        {
            get
            {
                return this.limitsField;
            }
            set
            {
                this.limitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("warning", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public warningType[] warnings
        {
            get
            {
                return this.warningsField;
            }
            set
            {
                this.warningsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ResultCodeType trxStatus
        {
            get
            {
                return this.trxStatusField;
            }
            set
            {
                this.trxStatusField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class ComplianceResultType
    {

        private AgentType[] agentField;

        private string resultField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("agent", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AgentType[] agent
        {
            get
            {
                return this.agentField;
            }
            set
            {
                this.agentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class AgentType
    {

        private AgentCheckType[] agentCheckField;

        private string nameField;

        private string resultField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("agentCheck", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AgentCheckType[] agentCheck
        {
            get
            {
                return this.agentCheckField;
            }
            set
            {
                this.agentCheckField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class AgentCheckType
    {

        private string resultField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class limitType
    {

        private string unitsField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string units
        {
            get
            {
                return this.unitsField;
            }
            set
            {
                this.unitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class warningType
    {

        private string codeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class PurchaseRequestType
    {

        private PersonInfoType personField;

        private StoreType storeField;

        private string inquiryIdField;

        private string storeTrxIdField;

        private System.DateTime storeTrxTimeField;

        private bool postSaleIndField;

        private ProductType[] productsField;

        private signatureType signatureField;

        public PurchaseRequestType()
        {
            this.postSaleIndField = false;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PersonInfoType person
        {
            get
            {
                return this.personField;
            }
            set
            {
                this.personField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public StoreType store
        {
            get
            {
                return this.storeField;
            }
            set
            {
                this.storeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer")]
        public string inquiryId
        {
            get
            {
                return this.inquiryIdField;
            }
            set
            {
                this.inquiryIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string storeTrxId
        {
            get
            {
                return this.storeTrxIdField;
            }
            set
            {
                this.storeTrxIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime storeTrxTime
        {
            get
            {
                return this.storeTrxTimeField;
            }
            set
            {
                this.storeTrxTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool postSaleInd
        {
            get
            {
                return this.postSaleIndField;
            }
            set
            {
                this.postSaleIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("product", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ProductType[] products
        {
            get
            {
                return this.productsField;
            }
            set
            {
                this.productsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public signatureType signature
        {
            get
            {
                return this.signatureField;
            }
            set
            {
                this.signatureField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.methcheck.appriss.com/")]
    public partial class InquiryResponseType
    {

        private string inquiryIdField;

        private System.DateTime timeField;

        private bool timeFieldSpecified;

        private PersonInfoType personField;

        private limitType[] limitsField;

        private ComplianceResultType resultField;

        private warningType[] warningsField;

        private ResultCodeType trxStatusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer")]
        public string inquiryId
        {
            get
            {
                return this.inquiryIdField;
            }
            set
            {
                this.inquiryIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool timeSpecified
        {
            get
            {
                return this.timeFieldSpecified;
            }
            set
            {
                this.timeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PersonInfoType person
        {
            get
            {
                return this.personField;
            }
            set
            {
                this.personField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("limit", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public limitType[] limits
        {
            get
            {
                return this.limitsField;
            }
            set
            {
                this.limitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ComplianceResultType result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("warning", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public warningType[] warnings
        {
            get
            {
                return this.warningsField;
            }
            set
            {
                this.warningsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ResultCodeType trxStatus
        {
            get
            {
                return this.trxStatusField;
            }
            set
            {
                this.trxStatusField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void doInquiryCompletedEventHandler(object sender, doInquiryCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class doInquiryCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal doInquiryCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public InquiryResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((InquiryResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void doPurchaseCompletedEventHandler(object sender, doPurchaseCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class doPurchaseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal doPurchaseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public PurchaseResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((PurchaseResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void doReturnCompletedEventHandler(object sender, doReturnCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class doReturnCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal doReturnCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public ReturnResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((ReturnResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void doVoidCompletedEventHandler(object sender, doVoidCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class doVoidCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal doVoidCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public VoidResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((VoidResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void doSignatureCompletedEventHandler(object sender, doSignatureCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class doSignatureCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal doSignatureCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SignatureResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SignatureResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void parseIdCompletedEventHandler(object sender, parseIdCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class parseIdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal parseIdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public IdParseResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((IdParseResponseType)(this.results[0]));
            }
        }
    }

}
