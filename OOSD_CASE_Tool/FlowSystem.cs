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

        /// <summary>
        /// X coordinate of where to draw a Shape on a Drawing Page.
        /// </summary>
        private static double drawXPos = 4;

        /// <summary>
        /// Y coordinate of where to draw a Shape on a Drawing Page.
        /// </summary>
        private static double drawYPos = 10;

        /// <summary>
        /// List of root nodes (Shape) for different Transform Center Diagrams.
        /// </summary>
        private List<Visio.Shape> transformCenters;


        public FlowSystem()
        {
            app = Globals.ThisAddIn.Application;
            transformCenters = new List<Visio.Shape>();
        }

        /// <summary>
        /// Converts a Flow Diagram to an Architecture Chart.
        /// </summary>
        public void convertToArchitectureChart()
        {
            // Grabs all shapes on the Flow Editor page and separate them by type flow diagram.
            Visio.Pages drawingPages = Utilities.getPages(app, Visio.VisDocumentTypes.visTypeDrawing);
            Visio.Page flowEditorPage = Utilities.getPage(drawingPages, CaseTypes.FLOW_PAGE);
            List<Visio.Shape> allShapes = Utilities.getAllShapesOnPage(flowEditorPage);

            // grabs the root node of each Flow Diagram
            // i.e. a Transform-Center shape for a Transform Center Diagram,
            // a Transaction-Center shape for a Transaction Driven Diagram
            filterRootNodes(allShapes);

            foreach (Visio.Shape s in transformCenters)
            {
                transformToArchChart(s);
            }
        }

        /// <summary>
        /// Converts a Transform-Center Diagram to an Architectural Chart.
        /// </summary>
        private void transformToArchChart(Visio.Shape root)
        {
            // Separates a Transform-Center into its different components
            // ie. a Process tree, an Input tree, and an Output tree. To get all these
            // components, start at the Root and retrieve all Shapes connected to Root.
            Visio.Connects branches = root.FromConnects;
            List<Visio.Shape> inputs = new List<Visio.Shape>();
            List<Visio.Shape> process = new List<Visio.Shape>();
            List<Visio.Shape> outputs = new List<Visio.Shape>();
            foreach (Visio.Connect c in branches)
            {
                Visio.Shape toShape = c.ToSheet;
                if (toShape.Master.Name == CaseTypes.TRANSFORM_PROCESS_MASTER)
                {
                    process.Add(toShape);
                }
            }

            // Draws the Architecture Chart on the Architecture Chart Page.
            Visio.Page outputPage = Utilities.getDrawingPage(app, CaseTypes.ARCHITECTURE_PAGE);
            outputChart(outputPage, root, inputs, process, outputs);
        }

        /// <summary>
        /// Draws an Architecture Chart on the given outputPage from the nodes given.
        /// </summary>
        /// <param name="outputPage">Page to draw the chart.</param>
        /// <param name="root">Root node of the chart.</param>
        /// <param name="inputs">Inputs for the Input Node.</param>
        /// <param name="process">Processes for the Process Node.</param>
        /// <param name="outputs">Outputs for the Output Node.</param>
        private void outputChart(Visio.Page outputPage, Visio.Shape root, List<Visio.Shape> inputs,
            List<Visio.Shape> process, List<Visio.Shape> outputs)
        {
            // Get a Rectangle Master from the OOSD General Stencil to serve
            // as the container for each Node in the chart.
            Visio.Master nodeMaster = Utilities.getMasterFromStencil(app, CaseTypes.OOSD_GENERAL_STENCIL,
                CaseTypes.OOSD_RECTANGLE);
            
        }

        /// <summary>
        /// Retrieves only the root Shape for each Flow Diagram system from a 
        /// list of shapes.
        /// </summary>
        /// <param name="shapes">List of shapes to search and filter.</param>
        private void filterRootNodes(List<Visio.Shape> shapes)
        {
            foreach (Visio.Shape s in shapes)
            {
                if (s.Master.Name == CaseTypes.TRANSFORM_CENTER_MASTER)
                {
                    transformCenters.Add(s);
                }

                // TODO: Add branches for Transaction-Driven Diagram
            }
        }
    }
}
