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

        #region Activity Diagram Generation

        #endregion
    }
}
