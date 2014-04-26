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
    /// Contains utility methods used elsewhere in the project.
    /// </summary>
    public sealed class Utilities
    {

        /// <summary>
        /// Inserts a Shape Data section into a Shape's Shapesheet, if it doesn't exist.
        /// </summary>
        /// <param name="Shape"></param>
        public static void insertShapeDataSection(Visio.Shape Shape)
        {
            // Only insert Shape Data section into the Shape's Shapesheet if it doesn't exist
            // return value of 0 means section doesn't exists
            short sectionStatus = Shape.get_SectionExists(CaseTypes.SHAPE_DATA_SECTION, 0);
            if (sectionStatus == 0)
            {
                Shape.AddSection(CaseTypes.SHAPE_DATA_SECTION);   
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
                rowIndex = Shape.AddNamedRow(CaseTypes.SHAPE_DATA_SECTION, rowName,
                    (short)Visio.VisRowTags.visTagDefault);
            } else {
                rowIndex = Shape.get_CellsRowIndex(cellName);
            }

            Visio.Cell valueCell = Shape.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION,
                rowIndex, CaseTypes.DS_VALUE_CELL);

            valueCell.Formula = '"' + value + '"';

            Visio.Cell labelCell = Shape.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION,
                rowIndex, CaseTypes.DS_LABEL_CELL);

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
            short numRows = Shape.get_RowCount(CaseTypes.SHAPE_DATA_SECTION);

            rowName = Utilities.spaceToUnderscore(rowName);

            // Iterate through the list of Data Section rows in reverse (since upon 
            // deletion of a row, all row indexes after the deleted row gets shifted
            // up by 1) to safely delete any row that starts with the given rowName.
            short startIndex = --numRows;
            for (short r = startIndex; r >= 0; --r)
            {
                Visio.Cell labelCell = Shape.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION, r, CaseTypes.DS_LABEL_CELL);

                string labelValue = labelCell.get_ResultStr(Visio.VisUnitCodes.visUnitsString);
                if (labelValue.StartsWith(rowName))
                {
                    Shape.DeleteRow(CaseTypes.SHAPE_DATA_SECTION, r);
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

        /// <summary>
        /// Returns all the Shapes currently on the given Page.
        /// </summary>
        /// <param name="page">Page with the Shapes.</param>
        /// <returns>List of Shapes.</returns>
        public static List<Visio.Shape> getAllShapesOnPage(Visio.Page page)
        {
            List<Visio.Shape> shapesList = new List<Visio.Shape>();

            Visio.Shapes shapes = page.Shapes;
            foreach (Visio.Shape s in shapes)
            {
                shapesList.Add(s);
            }

            return shapesList;
        }

        /// <summary>
        /// Returns the list of text inside of all Shapes on the given Page.
        /// </summary>
        /// <param name="page">Page to get Shapes.</param>
        /// <returns>List of Shape text.</returns>
        public static List<string> getAllShapeNames(Visio.Page page)
        {
            List<Visio.Shape> shapes = getAllShapesOnPage(page);
            return shapes.ConvertAll<string>(x => x.Text);
        }

        /// <summary>
        /// Returns the list of text inside of all Shapes in which the given Shape
        /// is also on the same Page.
        /// </summary>
        /// <param name="shape">Shape to get the Page.</param>
        /// <returns>List of Shape text.</returns>
        public static List<string> getAllShapeNames(Visio.Shape shape)
        {
            List<Visio.Shape> shapeList = getAllShapesOnPage(shape.ContainingPage);
            return shapeList.ConvertAll<string>(x => x.Text);
        }

        /// <summary>
        /// Returns the first Shape whose text matches the given text.
        /// </summary>
        /// <param name="shapes">List of shapes.</param>
        /// <param name="text">Text that needs to match.</param>
        /// <returns>The Shape with matching text, else null.</returns>
        public static Visio.Shape getShapeFromText(List<Visio.Shape> shapes, string text)
        {
            foreach (Visio.Shape s in shapes)
            {
                if (s.Text == text)
                {
                    return s;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a list of values in the Label Cell of the Shape Data Section
        /// for all rows with a prefix that match the rowNamePrefix. Results are
        /// stripped of the rowNamePrefix.
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="rowNamePrefix"></param>
        /// <returns></returns>
        public static HashSet<string> getDSLabelCells(Visio.Shape shape, string rowNamePrefix)
        {
            HashSet<string> labelCellsList = new HashSet<string>();

            // All operation rows are stored in the form: 
            // op_[operation_name]_[operation_property] in the Label Cell
            short numRows = shape.get_RowCount(CaseTypes.SHAPE_DATA_SECTION);
            for (short r = 0; r < numRows; ++r)
            {
                Visio.Cell labelCell = shape.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION,
                    r, CaseTypes.DS_LABEL_CELL);

                string labelCellValue = labelCell.get_ResultStr(Visio.VisUnitCodes.visUnitsString);

                // we are only interested in rows with prefix
                if (labelCellValue.StartsWith(rowNamePrefix))
                {
                    // we are only interested in the Label Cell value without
                    // prefix and any postfix
                    int startIndex = labelCellValue.IndexOf('_') + 1;
                    int endIndex = labelCellValue.LastIndexOf('_');
                    int opNameLen = endIndex - startIndex;
                    string opName = labelCellValue.Substring(startIndex, opNameLen);

                    labelCellsList.Add(Utilities.underscoreToSpace(opName));
                }
            }

            return labelCellsList;
        }
    }
}
