using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Visio = Microsoft.Office.Interop.Visio;
using Office = Microsoft.Office.Core;
using System.Windows.Forms;

namespace OOSD_CASE_Tool
{
    public partial class ThisAddIn
    {
        /// <summary>
        /// Instance of the Application that this add-in belongs to.
        /// </summary>
        private Visio.Application app;

        /// <summary>
        /// Class that handles all Object Editor functionality.
        /// </summary>
        private ObjectSystem objectSystem;

        /// <summary>
        /// Loads this addin in Visio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            // Our layer will be called app
            app = this.Application;
            // Initialize new object system
            objectSystem = new ObjectSystem();

            // Register event handlers

            /////////// TODO: Fix bug ///////////
            app.ShapeAdded += app_ShapeAdded;
            /// Danny: this event handler raises an exception when I try to call
            /// page.Draw... because the Draw method only returns a reference to
            /// the created Shape after it's done, but the ShapeAdded handler is called
            /// before a reference to new Shape could be passed to it.

            // Adding new shape text edit
            app.BeforeShapeTextEdit += app_BeforeShapeTextEdit;
            // Adding document created
            app.DocumentCreated += app_DocumentCreated;
        }

        private void app_ShapeAdded(Visio.Shape Shape)
        {
            if (Shape != null)
            {
                string shapePageName = Shape.ContainingPage.Name;
                //Visio.Page shapePage = Shape.ContainingPage;
                // Don't ask for information when generating or copying objects, since info is already 
                // expected to be present in the shape at this point in time.
                if (shapePageName.Equals(CaseTypes.OBJECT_PAGE, StringComparison.Ordinal))
                {
                    app_BeforeShapeTextEdit(Shape);
                }
            }
        }

        /// <summary>
        /// Event handler called after a Document has been created.
        /// </summary>
        /// <param name="doc">The created Document.</param>
        private void app_DocumentCreated(Visio.Document doc)
        {
            // If the Document created is a drawing file, setup the initial set
            // of Pages. Create different Pages for each System (Object, ER, Flow).
            // Only done if this is a new Drawing file.
            if (doc.Type == Visio.VisDocumentTypes.visTypeDrawing)
            {
                Visio.Pages pages = doc.Pages;

                // Adds a different page for each remaining Subsystem.
                // Relationship editor page
                pages.Add().Name = CaseTypes.RELATION_PAGE;
                // Flow diagram editor page
                pages.Add().Name = CaseTypes.FLOW_PAGE;
                // Object diagram editor page
                pages.Add().Name = CaseTypes.OBJECT_DIAGRAM_PAGE;
                // Data modelling diagram editor page
                pages.Add().Name = CaseTypes.DATA_MODEL_DIAGRAM_PAGE;
                // Architecture diagram editor page
                pages.Add().Name = CaseTypes.ARCHITECTURE_PAGE;
                // State table editor page
                pages.Add().Name = CaseTypes.STATE_TABLE_PAGE;

                // By default, Visio opens with one page
                // Rename first page to be for the Object Editor
                // Pages Collection index starts at 1.
                Visio.Page firstPage = pages[1];
                firstPage.Name = CaseTypes.OBJECT_PAGE;
                app.ActiveWindow.Page = firstPage.Name;

                // Opens the Object Stencil & have it docked to the Stencil Window
                app.Documents.OpenEx(CaseTypes.stencilPath() + CaseTypes.OBJECT_STENCIL,
                (short)Visio.VisOpenSaveArgs.visOpenDocked);

                // Event handlers that loads the appropriate stencil for a particular
                // page and unloads all other stencils when the Active Page changes.
                app.BeforeWindowPageTurn += app_BeforeWindowPageTurn;
                app.WindowTurnedToPage += app_WindowTurnedToPage;
            }
        }

        /// <summary>
        /// Event handler called after a different page is activated. Loads the
        /// appropriate stencil for this page.
        /// </summary>
        /// <param name="Window"></param>
        private void app_WindowTurnedToPage(Visio.Window Window)
        {
            // Get name of active page
            string activePage = app.ActivePage.Name;
            // Get Stencil path of associated editor page
            string stencilPath = CaseTypes.stencilPath();

            // Not all pages has an associated stencil
            bool stencilExists = true;
            // Choose stencil to load
            switch (activePage)
            {
                    // Stencils for object editor page
                case CaseTypes.OBJECT_PAGE:
                    stencilPath += CaseTypes.OBJECT_STENCIL;
                    break;
                    // Stencils for relationship editor page
                case CaseTypes.RELATION_PAGE:
                    stencilPath += CaseTypes.RELATION_STENCIL;
                    // Create empty HashSet to store current page shapes
                    HashSet<string> currentPageShapes = new HashSet<string>();
                    // Iterate through each shape
                    foreach (Visio.Shape s in app.ActivePage.Shapes)
                    {
                        // Add shape name to HashSet
                        currentPageShapes.Add(s.NameU);
                    }
                    System.Threading.Thread.Sleep(1000);
                   
                    // Iterate through pages
                    foreach (Visio.Page p in Window.Document.Pages)
                    {
                        // If object name found
                        if (p.Name == CaseTypes.OBJECT_PAGE)
                        {
                            // For each item on page
                            foreach (var item in Utilities.getAllShapesOnPage(p))
                            {            
                                // If the item does not exists
		                        if (!currentPageShapes.Contains(item.NameU))
                                {
                                    // Copy item
                                    item.Copy(Visio.VisCutCopyPasteCodes.visCopyPasteNormal);
                                    // And Paste it on active page
                                    app.ActivePage.Paste(Visio.VisCutCopyPasteCodes.visCopyPasteNormal);
                                }
                            }
                            break;
                        }
                    }
                    //Visio.Page shapePage = Shape.ContainingPage;
                     if (app.ActivePage.Shapes.Count < 5)
                    {
                        MessageBox.Show("Relationship editor must contain atleast 5 objects. Add more objects.");
                        app.ActiveWindow.Page = app.ActiveDocument.Pages[CaseTypes.OBJECT_PAGE];
                    }
 
	            	/*if (app.ActivePage.Shapes.Count > 9)
                    {
                        int count = app.ActivePage.Shapes.Count - 5;
                        for (int i = 1; i < count+1; i++ )
                        {
                            //app.ActivePage.Drop(getShapeToMove(i), -1 + i, -1 + 1);
                            //app.ActivePage.Shapes[i].Delete();
                            getShapeToMove(i).Cut(Visio.VisCutCopyPasteCodes.visCopyPasteNormal);
                            app.ActivePage.PasteToLocation( -1 + i, -1 + i, 0);
                            app.ActivePage.ResizeToFitContents();
                            app.ActivePage.AutoSizeDrawing();
                        }
                    }  */

                    
                    break;
                    // Stencils for flow diagram editor
                case CaseTypes.FLOW_PAGE:
                    stencilPath += CaseTypes.FLOW_STENCIL;
                    break;
                    // Default to no stencils on page
                default:
                    stencilExists = false;
                    break;
            }
            // If stencil 
            if (stencilExists)
            {
                app.Documents.OpenEx(stencilPath,
                    (short)Visio.VisOpenSaveArgs.visOpenDocked);
            }
        }

        Visio.Shape getShapeToMove(int i)
        {
            return app.ActivePage.Shapes[i];
        }

        /// <summary>
        /// Event handler called before a different page is activated. Unloads
        /// all docked stencil windows.
        /// </summary>
        /// <param name="Window"></param>
        private void app_BeforeWindowPageTurn(Visio.Window Window)
        {
            Visio.Documents docs = app.Documents;

            foreach (Visio.Document d in docs)
            {
                if (d.Type == Visio.VisDocumentTypes.visTypeStencil)
                {
                    d.Close();
                }
            }
        }

        /// <summary>
        /// This event handler is called after user double-clicks on a Shape,
        /// but before they are allowed to edit the text inside the Shape.
        /// </summary>
        /// <param name="Shape">
        /// The Shape that is going to be opened for text editing.
        /// </param>
        private void app_BeforeShapeTextEdit(Visio.Shape Shape)
        {
            // If a Shape is part of a group, the Master Name is the same
            // name as the group's Master name
            try
            {
                string shapeMasterName;
                if (Shape.Master != null)
                {
                    shapeMasterName = Shape.Master.Name;

                    switch (shapeMasterName)
                    {
                        case CaseTypes.C_OBJ_MASTER:
                            objectSystem.getCObjAttributesForm(Shape);
                            break;
                        case CaseTypes.SM_OBJ_MASTER:
                            objectSystem.getSMObjAttributesForm(Shape);
                            break;
                        case CaseTypes.ADT_OBJ_MASTER:
                            objectSystem.getADTObjAttributesForm(Shape);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
            
        }


        /// <summary>
        /// Unloads this addin in Visio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        /// <summary>
        /// Creates custom ribbon and adds it to Visio UI.
        /// </summary>
        /// <returns>Custom ribbon.</returns>
        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return Globals.Factory.GetRibbonFactory().CreateRibbonManager(
                new Microsoft.Office.Tools.Ribbon.IRibbonExtension[] { new OOSDRibbon() });
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
