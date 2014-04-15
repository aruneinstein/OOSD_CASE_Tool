using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Visio = Microsoft.Office.Interop.Visio;
using Office = Microsoft.Office.Core;
using System.Windows.Forms;

namespace OOSD_CASE_Tool
{
    public partial class ThisAddIn
    {
        /// <summary>
        /// Loads this addin in Visio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            // Register event handlers
            this.Application.BeforeShapeTextEdit += Application_BeforeShapeTextEdit;
        }

        /// <summary>
        /// This event handler is called after user double-clicks on a Shape,
        /// but before they are allowed to edit the text inside the Shape.
        /// </summary>
        /// <param name="Shape">
        /// The Shape that is going to be opened for text editing.
        /// </param>
        private void Application_BeforeShapeTextEdit(Visio.Shape Shape)
        {
            // If a Shape is part of a group, the Master Name is the same
            // name as the group's Master name
            string shapeMasterName = Shape.Master.Name;

            switch (shapeMasterName)
            {
                case Utilities.C_OBJ_MASTER_NAME:
                    MessageBox.Show(shapeMasterName);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Unloads this addin in Visio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        /// <summary>
        /// Creates custom ribbon and adds it to Visio UI.
        /// </summary>
        /// <returns>Custom ribbon.</returns>
        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return Globals.Factory.GetRibbonFactory().CreateRibbonManager(
                new Microsoft.Office.Tools.Ribbon.IRibbonExtension[] { new OOSDRibbon() });
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
