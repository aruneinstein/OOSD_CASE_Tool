using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visio = Microsoft.Office.Interop.Visio;
using System.Windows.Forms;

namespace OOSD_CASE_Tool
{
    /// <summary>
    /// Contains utility methods/variables used elsewhere in the project.
    /// </summary>
    public sealed class Utilities
    {
        /// <summary>
        /// Name of the Concrete Object Master Shape as defined in the Object Stencil.
        /// </summary>
        public const string C_OBJ_MASTER_NAME = "C-Object";

        /// <summary>
        /// Name of the Abstract Data Type Object Master Shape as defined in the Object Stencil.
        /// </summary>
        public const string ADT_OBJ_MASTER_NAME = "ADT-Object";

        /// <summary>
        /// Name of the State Machine Object Master Shape as defined in the Object Stencil.
        /// </summary>
        public const string SM_OBJ_MASTER_NAME = "SM-Object";

        /// <summary>
        /// The name of the stencil that contains Object shapes.
        /// </summary>
        public const string OBJECT_STENCIL_NAME = "Object Stencil.vssx";

        /// <summary>
        /// The name of the stencil that contains Flow Diagram shapes.
        /// </summary>
        public const string FLOW_STENCIL_NAME = "Flow Diagram Stencil.vssx";

        /// <summary>
        /// The name of the stencil that contains Relation Editor shapes.
        /// </summary>
        public const string RELATION_STENCIL_NAME = "Relation Editor Stencil.vssx";

        /// <summary>
        /// Index of the Shape Data Section in a Shapesheet.
        /// </summary>
        public const short SHAPE_DATA_SECTION = (short)Visio.VisSectionIndices.visSectionProp;

        /// <summary>
        /// Index of the Value Cell in the Data Section of a Shapesheet.
        /// </summary>
        public const short DATA_SECTION_VALUE_CELL = (short)Visio.VisCellIndices.visCustPropsValue;

        /// <summary>
        /// Index of the Label Cell in the Data Section of a Shapesheet.
        /// </summary>
        public const short DATA_SECTION_LABEL_CELL = (short)Visio.VisCellIndices.visCustPropsLabel;

        /// <summary>
        /// Returns the path to the Stencils folder.
        /// </summary>
        /// <returns></returns>
        public static string getStencilPath()
        {
            // Get the assembly information, which has runtime info
            System.Reflection.Assembly assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();

            // CodeBase is the location of the ClickOnce deployment files
            Uri uriCodeBase = new Uri(assemblyInfo.CodeBase);
            string clickOnceLocation = System.IO.Path.GetDirectoryName(uriCodeBase.LocalPath.ToString());

            return clickOnceLocation += @"\Stencils\";
        }

        /// <summary>
        /// Inserts a Shape Data section into a Shape's Shapesheet, if it doesn't exist.
        /// </summary>
        /// <param name="Shape"></param>
        public static void insertShapeDataSection(Visio.Shape Shape)
        {
            // Only insert Shape Data section into the Shape's Shapesheet if it doesn't exist
            // return value of 0 means section doesn't exists
            short sectionStatus = Shape.get_SectionExists(SHAPE_DATA_SECTION, 0);
            if (sectionStatus == 0)
            {
                Shape.AddSection(SHAPE_DATA_SECTION);   
            }
        }

        /// <summary>
        /// Sets the Value Cell of a Shapesheet's Data Section at the given row
        /// name to the specified value. Also sets the Label Cell to given rowName.
        /// </summary>
        /// <param name="Shape">Shape to change.</param>
        /// <param name="rowIndex">Name of row within the Data Section.</param>
        /// <param name="value">Value to change the Value Cell to.</param>
        public static void setDataSectionValueCell(Visio.Shape Shape, string rowName, string value)
        {
            // Row Names can only contain a-z, A-Z, 0-9, or _
            rowName = spaceToUnderscore(rowName);

            // Name of the Value Cell in the Shapesheet
            string cellName = "Prop." + rowName + ".Value";

            short rowIndex;
            // Return value of == 0 means cell (and thus, the row) doesn't exist.
            short cellExists = Shape.get_CellExists(cellName, 0);
            if (cellExists == 0)
            {
                rowIndex = Shape.AddNamedRow(SHAPE_DATA_SECTION, rowName,
                    (short)Visio.VisRowTags.visTagDefault);
            } else {
                rowIndex = Shape.get_CellsRowIndex(cellName);
            }

            Visio.Cell valueCell = Shape.get_CellsSRC(SHAPE_DATA_SECTION, 
                rowIndex, DATA_SECTION_VALUE_CELL);

            valueCell.Formula = '"' + value + '"';

            Visio.Cell labelCell = Shape.get_CellsSRC(SHAPE_DATA_SECTION,
                rowIndex, DATA_SECTION_LABEL_CELL);

            labelCell.Formula = '"' + rowName + '"';
        }

        /// <summary>
        /// Retrieves the value of the Value Cell in a Shape's Data Section.
        /// </summary>
        /// <param name="Shape">Shape to get cell value for.</param>
        /// <param name="rowName">Name of the row to find the Value Cell.</param>
        /// <returns></returns>
        public static string getDataSectionValueCell(Visio.Shape Shape, string rowName)
        {
            string cellName = "Prop." + spaceToUnderscore(rowName) + ".Value";
            Visio.Cell valueCell = Shape.get_Cells(cellName);

            return valueCell.get_ResultStr(Visio.VisUnitCodes.visUnitsString);
        }

        /// <summary>
        /// Delete all rows in the Data Section that starts with the given row name.
        /// </summary>
        /// <param name="Shape">
        /// The Shape to delete the row(s) from.
        /// </param>
        /// <param name="rowName">The name of the row to delete.</param>
        public static void deleteDataSectionRow(Visio.Shape Shape, string rowName)
        {
            short numRows = Shape.get_RowCount(SHAPE_DATA_SECTION);

            rowName = Utilities.spaceToUnderscore(rowName);

            // Iterate through the list of Data Section rows in reverse (since upon 
            // deletion of a row, all row indexes after the deleted row gets shifted
            // up by 1) to safely delete any row that starts with the given rowName.
            short startIndex = --numRows;
            for (short r = startIndex; r >= 0; --r)
            {
                Visio.Cell labelCell = Shape.get_CellsSRC(SHAPE_DATA_SECTION, r, DATA_SECTION_LABEL_CELL);

                string labelValue = labelCell.get_ResultStr(Visio.VisUnitCodes.visUnitsString);
                if (labelValue.StartsWith(rowName))
                {
                    Shape.DeleteRow(SHAPE_DATA_SECTION, r);
                }
            }
        }


        /// <summary>
        /// Replaces all instances of a space within a string to an underscore.
        /// </summary>
        /// <param name="value">String to work on.</param>
        /// <returns>String with all spaces replaced by underscores.</returns>
        public static string spaceToUnderscore(string value)
        {
            return value.Replace(' ', '_');
        }

        /// <summary>
        /// Replaces all instances of an underscore within a string to a space.
        /// </summary>
        /// <param name="value">String to work on.</param>
        /// <returns>String with all underscores replaced by space.</returns>
        public static string underscoreToSpace(string value)
        {
            return value.Replace('_', ' ');
        }

        /// <summary>
        /// Returns the index if a string value exists in a ListBox list of items,
        /// else return -1.
        /// </summary>
        /// <param name="list">List that contains all of a ListBox's entries.</param>
        /// <param name="value">Value to check existence of.</param>
        /// <returns>Index of found item or -1.</returns>
        public static int itemExists(ListBox.ObjectCollection list, string value)
        {
            int numItems = list.Count;
            for (int i = 0; i < numItems; ++i)
            {
                if (list[i].ToString() == value)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Clears all text in all TextBox belonging to a GroupBox.
        /// </summary>
        /// <param name="groupBox">GroupBox containing TextBox(s) to clear.</param>
        public static void clearTextBoxInGroupBox(GroupBox groupBox)
        {
            Control.ControlCollection inputBoxes = groupBox.Controls;

            foreach (Control c in inputBoxes)
            {
                if (c is TextBox)
                {
                    c.Text = "";
                }
            }
        }
    }
}
