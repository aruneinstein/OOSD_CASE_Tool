using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;

namespace OOSD_CASE_Tool
{
    public partial class OOSDRibbon
    {

        /// <summary>
        /// Loads this custom ribbon.
        /// </summary>
        /// <param name="sender">
        /// An object that represents the control that raised the event.
        /// Passed to every callback handler in this class.
        /// </param>
        /// <param name="e">
        /// A RibbonControlEventArgs that contains a Microsoft.Office.Core.IRibbonControl.
        /// Use this control to access any property that is not available in the Ribbon 
        /// object model provided by the Visual Studio Tools for Office runtime.
        /// Passed to every callback handler in this class.
        /// </param>
        private void OOSDRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            
        }

        /// <summary>
        /// Displays the Object Stencil in the stencil dock in Visio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openObjStencilBtn_Click(object sender, RibbonControlEventArgs e)
        {
        
        }
    }
}
