using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDevice
{
    public interface iForm
    {
        /// <summary>
        /// Represents the Name of the form
        /// </summary>
        string FormName { set; get; }

        /// <summary>
        /// List of form elemets and the data it holds
        /// </summary>
        List<FormElement> FormItems { set; get; }

        /// <summary>
        /// if the Form Has LineDisplayItem
        /// </summary>
        bool HasLineDisplay { set; get; }
        
        /// <summary>
        /// Data to Be diplayed in LIneDisplay
        /// </summary>
        List<string> LineItemData { set; get; }

        /// <summary>
        /// If the Form has a sigBox
        /// </summary>
        bool HasSigBox { set; get; }


        /// <summary>
        /// name of the Signature Box
        /// </summary>
        string SigBoxName { set; get; }


    }
}
