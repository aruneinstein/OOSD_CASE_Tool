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
        private double drawXPos = 0.0;

        /// <summary>
        /// Y coordinate of where to draw a Shape on a Drawing Page.
        /// </summary>
        private double drawYPos = 0.0;

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
        /// Converts a Flow Diagram to an Architecture Chart. By Default, retrieves
        /// shapes from the FLOW_PAGE and outputs to ARCHITECTURE_PAGE.
        /// </summary>
        public void convertToArchitectureChart()
        {
            Visio.Page inputPage = Utilities.getDrawingPage(app, CaseTypes.FLOW_PAGE);
            Visio.Page outputPage = Utilities.getDrawingPage(app, CaseTypes.ARCHITECTURE_PAGE);

            // Grabs all shapes on the Flow Editor page and separate them by type flow diagram.
            List<Visio.Shape> allShapes = Utilities.getAllShapesOnPage(inputPage);

            // grabs the root node of each Flow Diagram
            // i.e. a Transform-Center shape for a Transform Center Diagram,
            // a Transaction-Center shape for a Transaction Driven Diagram
            filterRootNodes(allShapes);

            foreach (Visio.Shape s in transformCenters)
            {
                transformToArchChart(inputPage, outputPage, s);
            }

            // switches active window to display the architecture chart page
            app.ActiveWindow.Page = outputPage;
        }

        /// <summary>
        /// Converts a Transform-Center Diagram to an Architectural Chart.
        /// </summary>
        private void transformToArchChart(Visio.Page inputPage, Visio.Page outputPage, Visio.Shape root)
        {
            // Separates a Transform-Center into its different components
            // ie. a Process tree, an Input tree, and an Output tree. To get all these
            // components, start at the Root and retrieve all Shapes connected to Root.

            // Gets all shapes that are connected to the root shape through a connector
            // (such as through a 1-D Dynamic Connector)
            List<int> shapeIDs = new List<int>(
                (int[]) root.ConnectedShapes(Visio.VisConnectedShapesFlags.visConnectedShapesOutgoingNodes, ""));

            // Gets all shapes that are glued to the root shape (as in, it is connected
            // directly to the root shape.
            shapeIDs.AddRange(
                (int[]) root.GluedShapes(Visio.VisGluedShapesFlags.visGluedShapesAll1D, ""));

            List<Visio.Shape> inputs = new List<Visio.Shape>();
            List<Visio.Shape> process = new List<Visio.Shape>();
            List<Visio.Shape> outputs = new List<Visio.Shape>();

            Visio.Shapes allShapes = inputPage.Shapes;

            foreach (int id in shapeIDs)
            {
                Visio.Shape toShape = allShapes[id];

                if (toShape.Master.Name == CaseTypes.TRANSFORM_PROCESS_MASTER)
                {
                    process.Add(toShape);
                } else if (toShape.Master.Name == CaseTypes.TRANSFORM_INPUT_MASTER)
                {
                    inputs.Add(toShape);
                } else if (toShape.Master.Name == CaseTypes.TRANSFORM_OUTPUT_MASTER)
                {
                    outputs.Add(toShape);
                }
            }

            // Draws the Architecture Chart on the Architecture Chart Page.
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
            // Space to leave open between shapes
            double yPad = .5, xPad = .5;

            // sets the first open position to start dropping shapes onto the page
            // into the field variables drawXPos, drawYPos.
            setShapeDropPosition(outputPage);

            // Get a Rectangle Master from the OOSD General Stencil to serve
            // as the container for each Node in the chart.
            Visio.Master nodeMaster = Utilities.getMasterFromStencil(app, CaseTypes.OOSD_GENERAL_STENCIL,
                CaseTypes.OOSD_RECTANGLE);

            // Root of the Architecture Chart
            Visio.Shape transformRoot = outputPage.Drop(nodeMaster, drawXPos, drawYPos);
            transformRoot.Text = root.Text;

            // Use the root node width and height as the basis for performing layout
            double nodeHeight = transformRoot.Cells["Height"].Result["inches"];
            double nodeWidth = transformRoot.Cells["Width"].Result["inches"];

            // Root of the Process subtree, set it directly under transform center root.
            drawYPos -= nodeHeight / 2 + yPad;
            Visio.Shape processRoot = outputPage.Drop(nodeMaster, drawXPos, drawYPos);
            processRoot.Text = @"Process";
            
            // Root of the Input subtree, set to left of Process Root
            Visio.Shape inputRoot = outputPage.Drop(nodeMaster, drawXPos - (nodeWidth * 3), drawYPos);
            inputRoot.Text = @"Input";

            // Root of the Output subtree, set to right of Process Root
            Visio.Shape outputRoot = outputPage.Drop(nodeMaster, drawXPos + (nodeWidth * 3), drawYPos);
            outputRoot.Text = @"Output";

            // Calculate starting position of the first Input child node based on
            // the number of all children (input, process, output) and the nodeWidth
            // Set the start y position as the next level down
            drawYPos -= nodeHeight / 2 + yPad;
            int childrenCount = inputs.Count + process.Count + outputs.Count;
            double totalWidth = childrenCount * (nodeWidth + xPad);
            drawXPos = drawXPos - (totalWidth / 2) + nodeWidth;

            double pad = nodeWidth / 2 + xPad; // distance from one center of a shape to another center
            List<Visio.Shape> inputChildren = dropShapes(outputPage, nodeMaster, inputs, pad);
            List<Visio.Shape> processChildren = dropShapes(outputPage, nodeMaster, process, pad);
            List<Visio.Shape> outputChildren = dropShapes(outputPage, nodeMaster, outputs, pad);

            // Connects all shapes together to form tree
            glueRootToChildren(outputPage, transformRoot,
                    new List<Visio.Shape>() { inputRoot, processRoot, outputRoot });
            glueRootToChildren(outputPage, inputRoot, inputChildren);
            glueRootToChildren(outputPage, processRoot, processChildren);
            glueRootToChildren(outputPage, outputRoot, outputChildren);
        }

        /// <summary>
        /// Creates an instance of Master for every shape given in the shapes list
        /// and drops it onto the page. Uses & modifies the field variables drawXPos, drawYPos.
        /// </summary>
        /// <param name="page">The page to drop shapes on.</param>
        /// <param name="master">The master to derive instances of shapes.</param>
        /// <param name="shapes">The list of shapes to get properties from.</param>
        /// <param name="pad">Distance from one center of a shape to another center.</param>
        /// <returns>List of shapes dropped.</returns>
        private List<Visio.Shape> dropShapes(Visio.Page page, Visio.Master master, List<Visio.Shape> shapes, double pad)
        {
            List<Visio.Shape> shapesDropped = new List<Visio.Shape>();
            foreach (Visio.Shape s in shapes)
            {
                Visio.Shape dropped = page.Drop(master, drawXPos, drawYPos);
                dropped.Text = s.Text;

                drawXPos += pad;

                shapesDropped.Add(dropped);
            }

            return shapesDropped;
        }

        private void glueRootToChildren(Visio.Page page, Visio.Shape root, List<Visio.Shape> children)
        {
            // Since this is an Architecture Chart, all connection points of root
            // is on the bottom edge of its shape and all connection points of children
            // are on its top edge of its shape. Use the middle of its width for both.
            double xGluePoint = .5,
                   rootYGluePoint = 0.0,
                   childYGluePoint = 1.0;

            foreach (Visio.Shape child in children)
            {
                Utilities.glueShapesWithDynamicConnector(page, root, child,
                    xGluePoint, rootYGluePoint, xGluePoint, childYGluePoint);
            }
        }

        /// <summary>
        /// Sets the position to Drop a Shape into the drawX, drawY field variables.
        /// This is a position at the top center of the Page if there are no shapes in it.
        /// Else, it's the bottom center of the BoundingBox that surrounds all the 
        /// current shapes on the page.
        /// </summary>
        /// <param name="page"></param>
        private void setShapeDropPosition(Visio.Page page)
        {
            int shapeCount = page.Shapes.Count;
            if (shapeCount == 0)
            {
                double pageWidth = Utilities.getPageWidth(page);
                double pageHeight = Utilities.getPageHeight(page);
                drawXPos = pageWidth / 2;
                drawYPos = pageHeight;
            }
            else
            {
                BoundingBox box = Utilities.getBoundingBox(page);
                drawXPos = ((box.LowerRightX - box.UpperLeftX) / 2) + box.UpperLeftX;
                drawYPos = box.LowerRightY;
            }

            // Adjustment co-efficients to try to center the Point in the center top,
            // with a margin from the top.
            drawYPos -= 1;
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
