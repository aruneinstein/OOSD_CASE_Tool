using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visio = Microsoft.Office.Interop.Visio;

namespace OOSD_CASE_Tool
{
    /// <summary>
    /// Class for working with the Flow Editor Subsystem.
    /// </summary>
    internal class FlowSystem
    {
        /// <summary>
        /// Instance of the Application that owns this AddIn.
        /// </summary>
        private Visio.Application app;


        private FlowSystem()
        {
            app = Globals.ThisAddIn.Application;
        }

        /// <summary>
        /// Converts a Flow Diagram to an Architecture Chart.
        /// </summary>
        public static void convertToArchitectureChart()
        {
            
        }
    }
}
