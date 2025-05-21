using System;
using System.Collections.Generic;
namespace EDevice
{
	/// <summary>
	/// Interface to the Device Functionality
	/// </summary>
	public interface IEDevice
	{
		/// <summary>
		/// Event for Signature
		/// </summary>
        event Action<SigFormat, SigStatus, object> SignatureEvent;
        /// <summary>
        /// Button Click Event.  Return the Form name, ID of the button click and Button State (Radio, checkbox)
        /// </summary>
        event Action<string, string, string> ButtonEvent;
        /// <summary>
        /// Result Event.  Return the result from a transaction.
        /// </summary>
        event Action<Dictionary<string, string>> Result;
        /// <summary>
        /// Event for any user input using the static forms. UserInputForm()
        /// </summary>
        event Action<string> UserInputEvent;
		/// <summary>
		/// Get the RBA Version
		/// </summary>
        string AppVersion { get; }
		/// <summary>
		/// Get the Current Form that is loaded on the device display
		/// </summary>
        string GetCurrentForm { get; } 
		/// <summary>
		/// Connection to the device. Parameter accepted CommSettings
		/// </summary>
		/// <param name="com"></param>
		/// <returns></returns>
		int Connect(CommSettings com);
		/// <summary>
		/// Disconnect from the device not from the serial port
		/// </summary>
		/// <returns></returns>
		int Disconnect();
		/// <summary>
		/// Shutdown from the system, disconnect from the serial port
		/// </summary>
		/// <returns></returns>
		int ShutDown();
		/// <summary>
		/// This is a Hard Reset message.  The Terminal clear out the previous action.
		/// </summary>
		/// <returns></returns>
		int Reset();
		/// <summary>
		/// Display form on the device screen. 
		/// Parameter accepted is FormProperties
		/// </summary>
		/// <param name="fp"></param>
		/// <returns>FormProperties</returns>
		int ShowForm(FormProperties.Properties fp);

        /// <summary>
        /// Update all form elements at once
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        int UpdateFormElement(FormProperties.Properties.updateForm Data);
		/// <summary>
		/// Clear the complete form of all data
		/// </summary>
		/// <param name="formName"></param>
		/// <returns></returns>
		int ClearForm(string formName);
		/// <summary>
		/// Clear the Line Display
		/// </summary>
		/// <param name="formName"></param>
		/// <returns></returns>
		int ClearLineDisplay(string formName);
		/// <summary>
		/// Remove a specific Line from the Line Display
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		int RemoveFromLineDisplay(int index);
        /// <summary>
        /// Update an Item in the LineDisplay
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        int UpdateLineDisplay(int index, string data);
        /// <summary>
        /// Allow for any kind of input that you might want from the user
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        bool UserInputForm(ISC.InputTags.PromptTags prompt, ISC.InputTags.FormatTags format);
        /// <summary>
        /// Number of Item in the LineDisplay
        /// </summary>
        int LineItemNumber { get; }
		/// <summary>
		/// Activate Device for Card Processing.
		/// </summary>
		/// <returns></returns>
		/////int ActivateTransaction();
		/// <summary>
		/// Process the Transaction with the valid tags for Payment.
		/// </summary>
		/// <param name="tags"></param>
		/// <returns></returns>
		int ProcessTransaction(PaymentTags tags);
        int SetGatewayParms(GatewayTags Tags);
		/// <summary>
		/// Cancel Transaction
		/// </summary>
		/// <returns></returns>
		int CancelTransaction();
		/// <summary>
		/// Upload a file to the Device
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		int UploadFile(string fileName);
		/// <summary>
		/// Upload a Package to the Device
		/// </summary>
		/// <param name="PackageName"></param>
		/// <returns></returns>
		int UploadPackage(string PackageName);

        int ActivateSigBox(string FormName, string FormElementName);


        int LoadForm(string FormName);

        int WriteToLineDisplay(string FormData);


        int LoadForm(iForm form);

        int ClearForm(iForm form);
    }
}
