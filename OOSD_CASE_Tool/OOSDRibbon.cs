using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Visio = Microsoft.Office.Interop.Visio;
using System.Windows.Forms;

namespace OOSD_CASE_Tool
{
    public partial class OOSDRibbon
    {
        /// <summary>
        /// The Application hosting this addin.
        /// </summary>
        Visio.Application app;

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
            app = Globals.ThisAddIn.Application;
        }

        /// <summary>
        /// On click of Open Stencil button, open and display the Object Stencil
        /// in the stencil dock in Visio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openObjStencilBtn_Click(object sender, RibbonControlEventArgs e)
        {
            app.Documents.OpenEx(CaseTypes.stencilPath() + CaseTypes.OBJECT_STENCIL, 
                (short) Visio.VisOpenSaveArgs.visOpenDocked);
        }

        /// <summary>
        /// Converts a Flow Diagram to an Architecture Chart.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void convertToArchChartBtn_Click(object sender, RibbonControlEventArgs e)
        {
            FlowSystem flowEditor = new FlowSystem();
            flowEditor.convertToArchitectureChart();
        }
    }
}
