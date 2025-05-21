using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
namespace EDevice
{
	/// <summary>
	/// Form Properties that need to be set 
	/// </summary>
	public class FormProperties
	{
		/// <summary>
		/// Form Properties use when passing each form to display
		/// </summary>
		public struct Properties
		{
			/// <summary>
			/// Element Type Text or Button
			/// </summary>
			public enum ElementType
			{
				/// <summary>
				/// The type of Element, Text
				/// </summary>
				Text,
				/// <summary>
				/// The teype of Element, Button
				/// </summary>
				Button,
                /// <summary>
                /// No Text or Button Element
                /// </summary>
                None

			}
			/// <summary>
			/// Name of the form to display.  No need to pass the form extension
			/// </summary>
			public string FormName;
			/// <summary>
			/// Form Element Type is either a Text or a Button
			/// </summary>
			public FormProperties.Properties.ElementType FormElementType;
			/// <summary>
			/// Form Element ID is the ID that is assign to Property when the form was created.
			/// </summary>
			public string FormElementID;
			/// <summary>
			/// Data that you want to display on the Form
			/// </summary>
			public string FormElementData;

            /// <summary>
            /// Update form Element
            /// </summary>
            public struct updateForm
            {
                /// <summary>
                /// List of Element and value to be update on the current form.
                /// Array 1 Name of Element, Array 2 Value for the element in Array 1.
                /// </summary>
                public List<Tuple<string[], string[]>> Update;
            }
            /// <summary>
            /// Update Elements on Form
            /// </summary>
            public updateForm UpdateFormElement;
		}
		/// <summary>
		/// Form Settings use to set the static setting for all forms and signature settings
		/// </summary>
		public struct FormSettings
		{
			/// <summary>
			/// Signature Information
			/// </summary>
			public struct Sign
			{
				public bool CropSignature
				{
					internal get;
					set;
				}
                /// <summary>
                /// Return format of the signature
                /// </summary>
                public SigFormat SignReturnFormat;
			}
			/// <summary>
			/// Device forms
			/// </summary>
			public struct cForms
			{
				/// <summary>
				/// Form list is the custom forms for the device.  
				/// Format: FormName, ElementID(control ID). 
				/// Each form may have diferent properties e.g Label LineDisplay each of these 
				/// will have a unique ID.
				/// </summary>
				public List<Tuple<string, string[]>> FormList;
			}
			/// <summary>
			/// Signature Information
			/// </summary>
			public FormProperties.FormSettings.Sign Signature;
			/// <summary>
			/// List of Custom forms to display with Element ID
			/// </summary>
			public FormProperties.FormSettings.cForms CustomForms;
		}
	}
}
