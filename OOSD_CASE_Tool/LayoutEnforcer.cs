using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vso = Microsoft.Office.Interop.Visio;
using System.Windows.Forms;

namespace OOSD_CASE_Tool
{
    class LayoutEnforcer
    {
        Vso.Page lPage;
        Vso.Application ap;
        Dictionary<Vso.Shape, Vso.Page> btnLinks;

        public LayoutEnforcer(Vso.Page pg)
        {
            this.lPage = pg;
            this.ap = Globals.ThisAddIn.Application;
        }

        public void checkConstraints(Vso.Shape shp)
        {
            if (this.lPage.Shapes.Count < 5)
            {
                MessageBox.Show("You don't have enough objects to prepare ER diagram. You need atleast 5 objects!");
                this.ap.ActiveWindow.Page = this.ap.ActiveDocument.Pages[CaseTypes.OBJECT_PAGE];
                return;
            }
            else if (this.lPage.Shapes.Count > 9)
            {
                
            }
        }
    }
}
