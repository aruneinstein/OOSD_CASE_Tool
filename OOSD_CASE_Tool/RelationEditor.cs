using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Visio = Microsoft.Office.Interop.Visio;

namespace OOSD_CASE_Tool
{
    class RelationEditor
    {
        Visio.Application app;
        Visio.Page relPage;
        Visio.Shapes relShapes;

        public RelationEditor()
        {
            app = Globals.ThisAddIn.Application;
        }

        public void refreshShapes()
        {
            this.relPage = app.ActiveDocument.Pages[CaseTypes.RELATION_PAGE];
            this.relShapes = this.relPage.Shapes;
        }

        #region Object Hierarchy Diagram

        public void generateObjectHierarchy()
        {
            refreshShapes();
            Hashtable nodes = new Hashtable();

            foreach (Visio.Shape sh in this.relShapes)
            {
                Array con = sh.GluedShapes(Visio.VisGluedShapesFlags.visGluedShapesIncoming1D, "", null);
                foreach (int cn in con)
                {
                    Visio.Shape ts = this.relShapes.get_ItemFromID(cn);
                    
                    if (ts.Master.Name.Equals("Is-A Relation"))
                    {
                        nodes[ts] = new List<Visio.Shape>();
                        IEnumerable<int> subclass = ts.GluedShapes(Visio.VisGluedShapesFlags.visGluedShapesIncoming2D, "").Cast<int>();

                        foreach (int shid in subclass)
                        {
                            Visio.Shape sub = this.relShapes.get_ItemFromID(shid);
                            ((List<Visio.Shape>) nodes[ts]).Add(sub);
                        }
                    }
                }

                drawObjHierarchy(nodes);
                nodes.Clear();
            }
        }

        private void drawObjHierarchy(Hashtable nd)
        {
            Visio.Page pg = Utilities.getDrawingPage(app, CaseTypes.OBJECT_DIAGRAM_PAGE);

            foreach (Visio.Shape item in nd.Keys)
            {
                Visio.Shape parent = drawObject(pg, item);
                var s = (List<Visio.Shape>) nd[item];
                foreach (var child in s)
                {
                    Visio.Shape ch = drawObject(pg, child);
                    Utilities.glueShapesWithDynamicConnector(pg, ch, parent, 0, 1, 0.5, 0);
                }
            }
        }

        private Visio.Shape drawObject(Visio.Page pg, Visio.Shape item)
        {
            return null;
        }

        #endregion


        #region Data Model Generation


        /// <summary>
        /// Converts a Relation Diagram to a Data Model Diagram (is-a, has).
        /// </summary>
        /// <param name="inputPage">Name of input page to get the diagram.</param>
        /// <param name="outputPage">Name of page to output data model.</param>
        public void toDataModel(Visio.Page inputPage, Visio.Page outputPage)
        {
            Visio.Shapes allShapes = inputPage.Shapes;
           
            // get all connectors: they show relationships between objects.
            List<Visio.Shape> connectors = getAll1DConnectors(inputPage);

            // The data model only needs relationships that are is-a or has.
            // these relationships are modeled using connectors that are NOT "is-a"
            // or does NOT have "c" in its name.
            List<Visio.Shape> dataModelConnectors = filterDataModelConnectors(connectors);

            // Draws all the objects for the Data Model
            double leftEdge = 0.5;
            double drawXPos = leftEdge, drawYPos = 10.5, padX = 2.0, padY = 2.0, pageW = 8.0;

            // IDs of Objects already drawn
            List<Visio.Shape> drawnShapes = new List<Visio.Shape>();

            Visio.Master rect = Utilities.getMasterFromStencil(app, CaseTypes.OOSD_GENERAL_STENCIL,
                CaseTypes.OOSD_RECTANGLE);
            
            foreach (Visio.Shape s in dataModelConnectors)
            {
                // Get both shapes connected to it
                int[] shapeIDs = (int[]) s.GluedShapes(Visio.VisGluedShapesFlags.visGluedShapesIncoming2D, "");
                Visio.Shape gluedShape = allShapes.get_ItemFromID(shapeIDs[0]);

                Visio.Shape beginDroppedShape = getShapeByName(drawnShapes, gluedShape.Text);
                if ( beginDroppedShape == null)
                {
                    Visio.Shape droppedShape = outputPage.Drop(rect, drawXPos, drawYPos);
                    droppedShape.Text = gluedShape.Text;
                    beginDroppedShape = droppedShape;
                    drawnShapes.Add(droppedShape);
                    drawXPos = drawXPos + padX;
                }

                shapeIDs = (int[])s.GluedShapes(Visio.VisGluedShapesFlags.visGluedShapesOutgoing2D, "");
                gluedShape = allShapes.get_ItemFromID(shapeIDs[0]);

                Visio.Shape endDroppedShape = getShapeByName(drawnShapes, gluedShape.Text);
                if (endDroppedShape == null)
                {
                    Visio.Shape droppedShape = outputPage.Drop(rect, drawXPos, drawYPos);
                    droppedShape.Text = gluedShape.Text;
                    endDroppedShape = droppedShape;
                    drawnShapes.Add(droppedShape);
                    drawXPos = drawXPos + padX;
                }

                // connects both shapes depending on its relationship
                Visio.Shape fromShape = null;
                Visio.Shape toShape = null;
                string connectorName = CaseTypes.OOSD_ONE_N_CONNECTOR;
                


                // resets drawing point to left edge of page
                if (drawXPos >= pageW)
                {
                    drawXPos = leftEdge;
                    drawYPos = drawYPos - padY;
                }
                
            }
        }

        private Visio.Shape getShapeByName(List<Visio.Shape> shapes, string name)
        {
            foreach (Visio.Shape s in shapes)
            {
                if (s.Text == name)
                {
                    return s;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns only Data Model Connectors.
        /// </summary>
        /// <param name="allConnectors"></param>
        /// <returns></returns>
        private List<Visio.Shape> filterDataModelConnectors(List<Visio.Shape> allConnectors)
        {
            List<Visio.Shape> dataModelConnectors = new List<Visio.Shape>();
            foreach (Visio.Shape s in allConnectors)
            {
                if (s.Master.Name != CaseTypes.ONE_ONE_MASTER ||
                    s.Master.Name != CaseTypes.ONE_M_MASTER ||
                    s.Master.Name != CaseTypes.M_M_MASTER)
                {
                    dataModelConnectors.Add(s);
                }
            }

            return dataModelConnectors;
        }
        /// <summary>
        /// Returns only 1D Connector Shapes.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private List<Visio.Shape> getAll1DConnectors(Visio.Page page)
        {
            List<Visio.Shape> all1DShapes = new List<Visio.Shape>();

            List<Visio.Shape> allShapes = Utilities.getAllShapesOnPage(page);
            foreach (Visio.Shape s in allShapes)
            {
                // OneD returns -1 if it's a 1D Shape
                if (s.OneD < 0)
                {
                    all1DShapes.Add(s);
                }
            }

            return all1DShapes;
        }

        #endregion
    }
}
