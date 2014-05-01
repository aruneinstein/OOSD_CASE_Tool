using System;
using System.Collections.Generic;
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

            foreach (Visio.Shape sh in this.relShapes)
            {
                Array con = sh.GluedShapes(Visio.VisGluedShapesFlags.visGluedShapesIncoming1D, "", null);
                foreach (int cn in con)
                {
                    Visio.Shape ts = this.relShapes.get_ItemFromID(cn);
                    
                    if (ts.Master.Name.Equals("Is-A Relationship"))
                    {
                        IEnumerable<long> subclass = ts.GluedShapes(Visio.VisGluedShapesFlags.visGluedShapesIncoming2D, "").Cast<long>();

                        foreach (int shid in subclass)
                        {
                            MessageBox.Show( this.relShapes.get_ItemFromID(shid).Name );
                        }
                    }
                }

            }
        }

        #endregion

        #region Activity Diagram Generation

        #endregion
    }
}
