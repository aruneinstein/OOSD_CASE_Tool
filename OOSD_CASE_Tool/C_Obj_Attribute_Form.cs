using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Visio = Microsoft.Office.Interop.Visio;

namespace OOSD_CASE_Tool
{
    public partial class C_Obj_Attribute_Form : Form
    {
        /// <summary>
        /// Reference to the Shape that owns (called) this form and whose shape
        /// data is defined using this form.
        /// </summary>
        private Visio.Shape ownerShape;

        public C_Obj_Attribute_Form(Visio.Shape Shape)
        {
            InitializeComponent();
            ownerShape = Shape;

            // Shape Data section stores all attributes for the Shape
            // as defined by the user through this form.
            Utilities.insertShapeDataSection(ownerShape);
        }
        private void C_Obj_Attribute_Form_Load(object sender, EventArgs e)
        {
            // Load C Object Name
            loadCObjectName();

            // Loads all of the Shape's list of attribution from its Shapesheet Data Section.
            loadAttributeNameList();

            // Sets the first attribute in the list, if there is any, as the selected item
            // in the ListBox and displays its properties in the Operation Properties.
            if (attributeListBox.Items.Count > 0)
            {
                attributeListBox.SetSelected(0, true);
                string atName = attributeListBox.SelectedItem.ToString();
                displayAttributeProperties(atName);
            }
        }
        private void displayAttributeProperties(string attributeName)
        {
            attributeListBox.Text = attributeName;

            string rowName = "at_" + attributeName + "_";

            attributeNameText.Text = Utilities.getDataSectionValueCell(ownerShape, rowName + "name");
            attributeDiscriptionText.Text = Utilities.getDataSectionValueCell(ownerShape, rowName + "discription");
            attributeDomainText.Text = Utilities.getDataSectionValueCell(ownerShape, rowName + "domain");
        }
        private void loadCObjectName()
        {
            short numRows = ownerShape.get_RowCount(CaseTypes.SHAPE_DATA_SECTION);
            string cName = "";
            for (short r = 0; r < numRows; ++r)
            {
                Visio.Cell labelCell = ownerShape.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION,
                    r, CaseTypes.DS_LABEL_CELL);

                string labelCellValue = labelCell.get_ResultStr(Visio.VisUnitCodes.visUnitsString);

                // we are only interested in c object name
                if (labelCellValue.StartsWith("c_"))
                {
                    // we are only interested in the object name
                    int startIndex = labelCellValue.IndexOf('_') + 1;
                    int endIndex = labelCellValue.LastIndexOf('_');
                    int cNameLen = endIndex - startIndex;
                    cName = labelCellValue.Substring(startIndex, cNameLen);
                }
            }
            cObjectNameText.Text = cName;
        }
        private void loadAttributeNameList()
        {
            HashSet<string> attributeSet = new HashSet<string>();

            // All attribute rows are stored in the form: 
            // at_[attribute_name]_[attribute_property] in the Label Cell
            short numRows = ownerShape.get_RowCount(CaseTypes.SHAPE_DATA_SECTION);
            for (short r = 0; r < numRows; ++r)
            {
                Visio.Cell labelCell = ownerShape.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION,
                    r, CaseTypes.DS_LABEL_CELL);

                string labelCellValue = labelCell.get_ResultStr(Visio.VisUnitCodes.visUnitsString);

                // we are only interested in attribute-related rows
                if (labelCellValue.StartsWith("at_"))
                {
                    // we are only interested in the attribute name
                    int startIndex = labelCellValue.IndexOf('_') + 1;
                    int endIndex = labelCellValue.LastIndexOf('_');
                    int atNameLen = endIndex - startIndex;
                    string atName = labelCellValue.Substring(startIndex, atNameLen);

                    attributeSet.Add(Utilities.underscoreToSpace(atName));
                }
            }

            attributeListBox.Items.AddRange(attributeSet.ToArray());
        }
       
        private void applyBtn_Click(object sender, EventArgs e)
        {
            saveObjectName();
            saveAttribute();
        }

        private void saveObjectName()
        {
            // Shape Data section format
            //    row name                         :: Value cell
            // c_[object name]_                    :: [c object name]

            string cObjectName = cObjectNameText.Text.Trim();
            // Must have an object name
            if (cObjectName == "")
            {
                MessageBox.Show("Must enter an Object Name.");
            }
            else
            {
                string rowName = "c_" + cObjectName + "_";

                Utilities.setDataSectionValueCell(ownerShape, rowName, cObjectName);
                ownerShape.Name = cObjectName;
            }

        }
        private void saveAttribute()
        {
            // Shape Data section format
            //    row name                         ::  Value cell
            // at_[attribute name]_                :: [attribute name]
            // at_[attribute name]_name            :: [attribute name]
            // at_[attribute name]_discription     :: [attribute discription]
            // at_[attribute name]_domain          :: [attribute domain]

            string atName = attributeNameText.Text.Trim();
            // Must have an attribute name
            if (atName == "")
            {
                MessageBox.Show("Must enter an Attribute Name.");
            }
            else
            {
                string rowName = "at_" + atName + "_";
                Utilities.setDataSectionValueCell(ownerShape, rowName + "name", atName);

                string discName = attributeDiscriptionText.Text;
                Utilities.setDataSectionValueCell(ownerShape, rowName + "discription", discName);

                string domainName = attributeDomainText.Text;
                Utilities.setDataSectionValueCell(ownerShape, rowName + "domain", domainName);

                updateAttributeList(attributeNameText.Text);
            }
        }
        /// <summary>
        /// If the given attributeName doesn't exist, add it to the attributeListBox
        /// and set the newly added item as the SelectedItem in the ListBox.
        /// </summary>
        /// <param name="attributeName">
        /// Name of the operation to add to the attributeListBox.
        /// </param>
        private void updateAttributeList(string attributeName)
        {
            ListBox.ObjectCollection atNames = attributeListBox.Items;

            // only add the attribute name if it doesn't already exist
            int itemIndex = Utilities.itemExists(atNames, attributeName);
            if (itemIndex < 0)
            {
                atNames.Add(attributeName);
                itemIndex = atNames.Count - 1;
            }
            attributeListBox.SetSelected(itemIndex, true);
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newAttributeBtn_Click(object sender, EventArgs e)
        {
            Utilities.clearTextBoxInGroupBox(attributePropGrpBox);
            attributeListBox.ClearSelected();
        }

        private void delAttributeBtn_Click(object sender, EventArgs e)
        {
            Object selectedItem = attributeListBox.SelectedItem;
            if (selectedItem != null)
            {
                string selectedValue = selectedItem.ToString();

                // Removes the item from the ListBox
                attributeListBox.Items.Remove(selectedItem);

                // Removes the attribute and its properties from the Shapesheet
                // All attribute rows are prefixed with 'at_' in its name
                Utilities.deleteDataSectionRow(ownerShape, "at_" + selectedValue);
                Utilities.clearTextBoxInGroupBox(attributePropGrpBox);
            }
            else
            {
                MessageBox.Show("Select an Attribute to delete.");
            }
        }

        private void attributeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Object item = attributeListBox.SelectedItem;

            if (item != null)
            {
                string atName = attributeListBox.SelectedItem.ToString();
                displayAttributeProperties(atName);
            }
        }

        private void cObjectNameText_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
