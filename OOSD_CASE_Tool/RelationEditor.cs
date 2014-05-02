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
            pg.Layout();
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

               // Visio.Shape connector = pg.Drop(this.connector, sibling, height);
#if false
                Visio.Cell beginX = connector.get_CellsSRC( (short) Visio.VisSectionIndices.visSectionObject, 
                    (short) Visio.VisRowIndices.visRowXForm1D, 
                    (short) Visio.VisCellIndices.vis1DBeginX);

                beginX.GlueTo(ch.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject,
                                                (short)Visio.VisRowIndices.visRowXFormOut,
                                                (short)Visio.VisCellIndices.visXFormPinX));

                Visio.Cell endX = connector.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject,
                    (short)Visio.VisRowIndices.visRowXForm1D,
                    (short)Visio.VisCellIndices.vis1DEndX);

                endX.GlueTo(parent.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject,
                                                (short)Visio.VisRowIndices.visRowXFormOut,
                                                (short)Visio.VisCellIndices.visXFormPinX));
#else
                ch.AutoConnect(parent, Visio.VisAutoConnectDir.visAutoConnectDirUp, this.connector);
#endif

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
            Visio.Shape sh = pg.Drop(item, sibling, height);
            pg.AutoSizeDrawing();
            sh.Text += ("\r\n_________________\r\n" + rectangleToObjectBox(pg, sh));
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

        #region Activity Diagram Generation

        #endregion
    }
}
