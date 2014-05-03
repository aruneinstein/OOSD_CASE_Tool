using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Debug = System.Diagnostics.Debug;
using Visio = Microsoft.Office.Interop.Visio;

namespace OOSD_CASE_Tool
{
    class RelationEditor
    {
        Visio.Application app;
        Visio.Page relPage;
        Visio.Shapes relShapes;
        Visio.Document stencil;
        Visio.Master connector, rect;
        private const double OFFSET = 1.5;

        public RelationEditor()
        {
            this.app = Globals.ThisAddIn.Application;
        }

        private void loadStencil()
        {
            this.stencil = Utilities.getStencil(app.Documents, CaseTypes.OOSD_GENERAL_STENCIL, Visio.VisOpenSaveArgs.visOpenHidden);
            this.connector = this.stencil.Masters[CaseTypes.OBJECT_HIERARCHY_CONNECTOR];
            this.rect = this.stencil.Masters[CaseTypes.OBJECT_HIERARCHY_RECT];
        }

        private void closeStencil()
        {
            this.stencil.Close();
            this.connector = null;
            this.rect = null;
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
                    
                    if (ts.Master.Name.Equals(CaseTypes.IS_A_STENCIL_MASTER, StringComparison.Ordinal))
                    {
                        nodes[sh] = new List<Visio.Shape>();
                        IEnumerable<int> subclass = ts.GluedShapes(Visio.VisGluedShapesFlags.visGluedShapesIncoming2D, "").Cast<int>();

                        foreach (int shid in subclass)
                        {
                            Visio.Shape sub = this.relShapes.get_ItemFromID(shid);
                            ((List<Visio.Shape>) nodes[sh]).Add(sub);
                        }
                    }
                        }
                    }
            List<Visio.Shape> treeRoots = constructForest(nodes);
            drawObjHierarchy(treeRoots, nodes);
        }

        /// <summary>
        /// Get to know the roots of each trees.
        /// </summary>
        /// <param name="nodes"> Node connections. (Adjacency list) </param>
        /// <returns> List of tree roots. </returns>
        private List<Visio.Shape> constructForest(Hashtable nodes)
        {
            List<Visio.Shape> treeRoots = new List<Visio.Shape>();
            HashSet<Visio.Shape> tparent = new HashSet<Visio.Shape>();
            
            foreach (Visio.Shape s in nodes.Keys)
            {
                tparent.UnionWith((List<Visio.Shape>) nodes[s]);
                }

            foreach (Visio.Shape nd in nodes.Keys)
            {
                if (!tparent.Contains(nd))
                {
                    treeRoots.Add((Visio.Shape)nd);
            }
        }

            return treeRoots;
        }

        /// <summary>
        /// Draws the object onto screen.
        /// </summary>
        /// <param name="trList"> Tree root list. </param>
        /// <param name="nd"> Node connections. (Adjacency list) </param>
        private void drawObjHierarchy(List<Visio.Shape> trList, Hashtable nd)
        {
            Visio.Page pg = Utilities.getDrawingPage(app, CaseTypes.OBJECT_DIAGRAM_PAGE);
            app.ActiveWindow.Page = pg;
            loadStencil();

            foreach (Visio.Shape tree in trList)
            {
                double[] d = getVBBox(pg);
                double height = d[3];
                double sibling = OFFSET;
                traverseTree(pg, tree, nd, ref height, ref sibling);
                height = 0;
                sibling = 0;
            }
            closeStencil();
            pg.LayoutIncremental(Visio.VisLayoutIncrementalType.visLayoutIncrAlign | Visio.VisLayoutIncrementalType.visLayoutIncrSpace,
                Visio.VisLayoutHorzAlignType.visLayoutHorzAlignCenter, Visio.VisLayoutVertAlignType.visLayoutVertAlignMiddle, 1.5, 1.5, Visio.VisUnitCodes.visPageUnits);
        }

        private double[] getVBBox(Visio.Page pg)
            {
            double[] dir = new double[4];
            pg.VisualBoundingBox((short) Visio.VisBoundingBoxArgs.visBBoxDrawingCoords, out dir[0], out dir[1], out dir[2], out dir[3]);
            return dir;
        }

        private void traverseTree(Visio.Page pg, Visio.Shape tree, Hashtable nd, ref double height, ref double sibling)
        {
            if (!nd.ContainsKey(tree))
            {
                return;
            }

            Visio.Shape parent = drawObject(pg, tree, ref height, ref sibling);
            height += OFFSET;
            
            var s = (List<Visio.Shape>) nd[tree];
                foreach (var child in s)
                {
                Visio.Shape ch = drawObject(pg, child, ref height, ref sibling);
                sibling += OFFSET;
                ch.AutoConnect(parent, Visio.VisAutoConnectDir.visAutoConnectDirUp, this.connector);
                Debug.WriteLine(String.Format("Connecting {0} to {1}", parent.Text, ch.Text));
            }

            height += OFFSET;
            sibling = OFFSET;
            foreach (var i in s)
            {
                traverseTree(pg, i, nd, ref height, ref sibling);
                }
            
            }

        private Visio.Shape drawObject(Visio.Page pg, Visio.Shape item, ref double height, ref double sibling)
        {
            string text = "";
            text += (item.Text + "\r\n_________________\r\n" + rectangleToObjectBox(pg, item));
            Visio.Shape sh = pg.Drop(this.rect, sibling, height);
            sh.Text = text;
            pg.AutoSizeDrawing();
            return sh;
        }

        private string rectangleToObjectBox(Visio.Page pg, Visio.Shape inp)
        {
            string attributeSet = "";
            // All attribute rows are stored in the form: 
            // at_[attribute_name]_[attribute_property] in the Label Cell
            // Get number of rows in c object shape data section
            short numRows = inp.get_RowCount(CaseTypes.SHAPE_DATA_SECTION);
            // Loop through each row of shape data section
            for (short r = 0; r < numRows; ++r)
            {
                Visio.Cell labelCell = inp.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION, r, CaseTypes.DS_LABEL_CELL);
                
                string labelCellValue = labelCell.get_ResultStr(Visio.VisUnitCodes.visUnitsString);

                if (labelCellValue.StartsWith("at_") && labelCellValue.EndsWith("_name"))
        {
                    // We are only interested in the attribute name
                    int startIndex = labelCellValue.IndexOf('_') + 1;
                    int endIndex = labelCellValue.LastIndexOf('_');
                    int atNameLen = endIndex - startIndex;
                    string atName = labelCellValue.Substring(startIndex, atNameLen);
                    attributeSet += (Utilities.underscoreToSpace(atName) + "\r\n");
                }
            }

            return attributeSet;
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
            
            foreach (Visio.Shape con in dataModelConnectors)
            {
                // Get both shapes connected to it
                int[] shapeIDs = (int[]) con.GluedShapes(Visio.VisGluedShapesFlags.visGluedShapesIncoming2D, "");
                Visio.Shape gluedShape = allShapes.get_ItemFromID(shapeIDs[0]);
                drawXPos = gluedShape.get_Cells("PinX").get_Result("inches");
                drawYPos = gluedShape.get_Cells("PinY").get_Result("inches");

                Visio.Shape beginDroppedShape = getShapeByName(drawnShapes, gluedShape.Text);
                if (beginDroppedShape == null)
                {
                    Visio.Shape droppedShape = outputPage.Drop(rect, drawXPos, drawYPos);
                    droppedShape.Text = gluedShape.Text;
                    beginDroppedShape = droppedShape;
                    drawnShapes.Add(droppedShape);
                    drawXPos = drawXPos + padX;
                }

                shapeIDs = (int[])con.GluedShapes(Visio.VisGluedShapesFlags.visGluedShapesOutgoing2D, "");
                gluedShape = allShapes.get_ItemFromID(shapeIDs[0]);
                drawXPos = gluedShape.get_Cells("PinX").get_Result("inches");
                drawYPos = gluedShape.get_Cells("PinY").get_Result("inches");

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

                // connection point
                PinLoc pinLocs = calculatePinLoc(beginDroppedShape, endDroppedShape);

                string conMasterName = con.Master.Name;
                // is-a relationships are connected by a "is-a" connector
                if (conMasterName == CaseTypes.IS_A_MASTER)
                {
                    Visio.Shape rel = Utilities.glueShapesWithConnector(outputPage,
                        beginDroppedShape, endDroppedShape, pinLocs.FromPinX, pinLocs.FromPinY, 
                        pinLocs.ToPinX, pinLocs.ToPinY, CaseTypes.OOSD_ONE_ONE_CONNECTOR);
                    rel.Text = "is-a";
                } 
                else if (conMasterName == CaseTypes.ONE_ONEC_MASTER)
                {
                    // 1:1 has-a relationship
                    Visio.Shape rel = Utilities.glueShapesWithConnector(outputPage,
                        beginDroppedShape, endDroppedShape, pinLocs.FromPinX, pinLocs.FromPinY, 
                        pinLocs.ToPinX, pinLocs.ToPinY, CaseTypes.OOSD_ONE_ONE_CONNECTOR);
                    rel.Text = "has " + endDroppedShape.Text;
                } 
                else if (conMasterName == CaseTypes.ONE_MC_MASTER)
                {
                    // 1:Mc has-a relationship
                    Visio.Shape rel = Utilities.glueShapesWithConnector(outputPage,
                        beginDroppedShape, endDroppedShape, pinLocs.FromPinX, pinLocs.FromPinY,
                        pinLocs.ToPinX, pinLocs.ToPinY, CaseTypes.OOSD_ONE_N_CONNECTOR);
                    rel.Text = "has " + endDroppedShape.Text + "s";
                } 
                else if (conMasterName == CaseTypes.M_MC_MASTER)
                {
                    Visio.Shape rel = Utilities.glueShapesWithConnector(outputPage,
                        beginDroppedShape, endDroppedShape, pinLocs.FromPinX, pinLocs.FromPinY,
                        pinLocs.ToPinX, pinLocs.ToPinY, CaseTypes.OOSD_ONE_N_CONNECTOR);
                    rel.Text = "has " + endDroppedShape.Text + "s";

                    //rel = Utilities.glueShapesWithConnector(outputPage,
                    //    endDroppedShape, beginDroppedShape, pinLocs.ToPinX, pinLocs.ToPinY, 
                    //    pinLocs.FromPinX, pinLocs.FromPinY, CaseTypes.OOSD_ONE_N_CONNECTOR);
                    //rel.Text = "is in " + beginDroppedShape.Text + "s";
                }

                // resets drawing point to left edge of page
                if (drawXPos >= pageW)
                {
                    drawXPos = leftEdge;
                    drawYPos = drawYPos - padY;
                }
                
            }

            // Layout
            //outputPage.LayoutIncremental(Visio.VisLayoutIncrementalType.visLayoutIncrAlign | Visio.VisLayoutIncrementalType.visLayoutIncrSpace,
            //    Visio.VisLayoutHorzAlignType.visLayoutHorzAlignCenter, Visio.VisLayoutVertAlignType.visLayoutVertAlignMiddle, 1.5, 1.5, Visio.VisUnitCodes.visPageUnits);
        }

        /// <summary>
        /// Returns location (in percentage) to pin X & pin Y
        /// </summary>
        private PinLoc calculatePinLoc(Visio.Shape fromShape, Visio.Shape toShape)
        {
            PinLoc pinLoc = new PinLoc();

            // gets the x, y position of from shape
            double fromPinX = fromShape.get_Cells("PinX").get_Result("inches");
            double fromPinY = fromShape.get_Cells("PinY").get_Result("inches");

            // gets x, y position of the to shape
            double toPinX = toShape.get_Cells("PinX").get_Result("inches");
            double toPinY = toShape.get_Cells("PinY").get_Result("inches");


            if (fromPinX < toPinX && Math.Abs(fromPinY - toPinY) < 1.0)
            {
                pinLoc.FromPinX = 1.0;
                pinLoc.ToPinX = 0.0;
                pinLoc.FromPinY = .5;
                pinLoc.ToPinY = .5;
            }
            else if (fromPinX > toPinX && Math.Abs(fromPinY - toPinY) < 1.0)
            {
                pinLoc.FromPinX = 0.0;
                pinLoc.ToPinX = 1.0;
                pinLoc.FromPinY = .5;
                pinLoc.ToPinY = .5;
            }
            else if (fromPinY > toPinY)
            {
                pinLoc.FromPinX = .5;
                pinLoc.ToPinX = 0.5;
                pinLoc.FromPinY = 0.0;
                pinLoc.ToPinY = 1.0;
            }
            else 
            {
                pinLoc.FromPinX = .5;
                pinLoc.ToPinX = 0.5;
                pinLoc.FromPinY = 1.0;
                pinLoc.ToPinY = 0;
            }

            return pinLoc;
        }

        private class PinLoc
        {
            public double FromPinX { get; set; }
            public double FromPinY { get; set; }
            public double ToPinX { get; set; }
            public double ToPinY { get; set; }

            public PinLoc(double fromX, double fromY, double toX, double toY)
            {
                FromPinX = fromX;
                FromPinY = fromY;
                ToPinX = toX;
                ToPinY = toY;
            }

            public PinLoc()
            {
                FromPinX = 0;
                FromPinY = 0;
                ToPinX = 0;
                ToPinY = 0;
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
                if (s.Master.Name != CaseTypes.ONE_ONE_MASTER &&
                    s.Master.Name != CaseTypes.ONE_M_MASTER &&
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
