using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visio = Microsoft.Office.Interop.Visio;
using System.Windows.Forms;

namespace OOSD_CASE_Tool
{
    /// <summary>
    /// Class for working with the Object Editor CSC.
    /// </summary>
    internal class ObjectSystem
    {
        private Visio.Application app;

        public ObjectSystem()
        {
            app = Globals.ThisAddIn.Application;
        }

        /// <summary>
        /// Display a form where user can enter information for a C-Object
        /// attribute. Update the corresponding Shape with applicable info
        /// upon form close.
        /// </summary>
        /// <param name="Shape">
        /// The Shape to apply applicable info to.
        /// </param>
        internal void getCObjAttributesForm(Visio.Shape Shape)
        {
            Form attrEditorForm = new C_Obj_Attribute_Form(Shape);
            attrEditorForm.ShowDialog();
        }

        /// <summary>
        /// Display a form where user can enter information for a SM-Object
        /// attribute. Update the corresponding Shape with applicable info
        /// upon form close.
        /// </summary>
        /// <param name="Shape">
        /// The Shape to apply applicable info to.
        /// </param>
        internal void getSMObjAttributesForm(Visio.Shape Shape)
        {
            Form attrEditorForm = new SM_Obj_Attribute_Form(Shape);
            attrEditorForm.ShowDialog();
        }

      
    }
}
