using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Visio = Microsoft.Office.Interop.Visio;
using System.Windows.Forms;

namespace OOSD_CASE_Tool
{
    public partial class OOSDRibbon
    {
        /// <summary>
        /// The Application hosting this addin.
        /// </summary>
        Visio.Application app;
        RelationEditor relEditor;

        /// <summary>
        /// Loads this custom ribbon.
        /// </summary>
        /// <param name="sender">
        /// An object that represents the control that raised the event.
        /// Passed to every callback handler in this class.
        /// </param>
        /// <param name="e">
        /// A RibbonControlEventArgs that contains a Microsoft.Office.Core.IRibbonControl.
        /// Use this control to access any property that is not available in the Ribbon 
        /// object model provided by the Visual Studio Tools for Office runtime.
        /// Passed to every callback handler in this class.
        /// </param>
        private void OOSDRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            app = Globals.ThisAddIn.Application;
            relEditor = new RelationEditor();
        }

        /// <summary>
        /// On click of Open Stencil button, open and display the Object Stencil
        /// in the stencil dock in Visio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openObjStencilBtn_Click(object sender, RibbonControlEventArgs e)
        {
            app.Documents.OpenEx(CaseTypes.stencilPath() + CaseTypes.OBJECT_STENCIL, 
                (short) Visio.VisOpenSaveArgs.visOpenDocked);
        }

        /// <summary>
        /// Converts the selected Flow Diagram to an Architecture Chart.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void convertToArchChartBtn_Click(object sender, RibbonControlEventArgs e)
        {
            // The user needs to select one or more Transform Center node of a Transform
            // Center Diagram to start the conversion.
            Visio.Selection selection = app.ActiveWindow.Selection;

            if (selection.Count == 0)
            {
                MessageBox.Show("Select a Data Flow Diagram to Convert.");
            }
            else
            {
                // Outputs the resulting Chart on the Architecture Chart Page.
                Visio.Page outputPage = Utilities.getDrawingPage(app, CaseTypes.ARCHITECTURE_PAGE);
                FlowSystem flowEditor = new FlowSystem();
                flowEditor.convertToArchitectureChart(selection, outputPage);
            }
        }

        private void erToObjHierBtn_Click(object sender, RibbonControlEventArgs e)
        {
            this.relEditor.generateObjectHierarchy();
        }

        public static void printProperties(Visio.Shapes shapes)
        {
            string res = "";
            // Look at each shape in the collection.
            foreach (Visio.Shape shape in shapes)
            {
                // Use this index to look at each row in the properties 
                // section.
                short iRow = (short)Visio.VisRowIndices.visRowFirst;

                // While there are stil rows to look at.
                while (shape.get_CellsSRCExists(
                    (short)Visio.VisSectionIndices.visSectionProp,
                    iRow,
                    (short)Visio.VisCellIndices.visCustPropsValue,
                    (short)0) != 0)
                {
                    // Get the label and value of the current property.
                    string label = shape.get_CellsSRC(
                            (short)Visio.VisSectionIndices.visSectionProp,
                            iRow,
                            (short)Visio.VisCellIndices.visCustPropsLabel
                        ).get_ResultStr(Visio.VisUnitCodes.visNoCast);

                    string value = shape.get_CellsSRC(
                            (short)Visio.VisSectionIndices.visSectionProp,
                            iRow,
                            (short)Visio.VisCellIndices.visCustPropsValue
                        ).get_ResultStr(Visio.VisUnitCodes.visNoCast);

                    // Print the results.
                    res += (string.Format(
                        "Shape={0} Label={1} Value={2}\r\n",
                        shape.Name, label, value));

                    // Move to the next row in the properties section.
                    iRow++;
                }

                // Now look at child shapes in the collection.
                if (shape.Master == null && shape.Shapes.Count > 0)
                    printProperties(shape.Shapes);
            }
            MessageBox.Show(res);
        }

        private void shapeInfoButton_Click(object sender, RibbonControlEventArgs e)
        {
            printProperties(app.ActivePage.Shapes);
        }

        private void openRelationStencilBtn_Click(object sender, RibbonControlEventArgs e)
        {
            app.Documents.OpenEx(CaseTypes.stencilPath() + CaseTypes.RELATION_STENCIL,
                (short)Visio.VisOpenSaveArgs.visOpenDocked);
        }

        private void openFlowStencilBtn_Click(object sender, RibbonControlEventArgs e)
        {
            app.Documents.OpenEx(CaseTypes.stencilPath() + CaseTypes.FLOW_STENCIL,
                (short)Visio.VisOpenSaveArgs.visOpenDocked);
        }

        private void stateDiagramToTable_Click(object sender, RibbonControlEventArgs e)
        {
            // The user needs to select one or more State Transiton Diagram Node
            // to start the conversion.
            Visio.Selection selection = app.ActiveWindow.Selection;

            if (selection.Count == 0)
            {
                MessageBox.Show("Select a State Transition Diagram to Convert.");
            }
            else
            {
                // Outputs the resulting State transition table to the State Transition Page
                Visio.Page outputPage = Utilities.getDrawingPage(app, CaseTypes.STATE_TABLE_PAGE);
                FlowSystem flowEditor = new FlowSystem();
                flowEditor.stateDiagramToTable(selection, outputPage);
            }
        }

        private void genDBButton_Click(object sender, RibbonControlEventArgs e)
        {

        }
    }
}
