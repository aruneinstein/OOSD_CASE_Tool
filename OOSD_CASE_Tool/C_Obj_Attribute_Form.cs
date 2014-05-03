using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
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

        [XmlElement("C_Obj_Name")]
        public string C_Object_Name
        {
            get
            {
                return OOSDRibbon.printProperties(ownerShape);
            }
            set
            {
                if (ownerShape.Name.StartsWith("c_"))
                    c_object_name = ownerShape.Name;
            }
        }

        [XmlElement("C_Obj_Attribute")]
        public string C_Object_Attribute
        {
            get
            {
                return OOSDRibbon.printProperties(ownerShape);
            }
            set
            {
                string Result = OOSDRibbon.printProperties(ownerShape);
                var obj = Result.Split('\n');
                foreach (string s in obj)
                {
                    var attribute = s.Split(' ');
                    if (ownerShape.Name.StartsWith("c_") && String.Equals((String)attribute[2], "Attribute"))
                        c_object_attribute = (String)attribute[3];
                }
            }
        }

        public C_Obj_Attribute_Form(Visio.Shape Shape)
        {
            // Initialize shape in visio
            InitializeComponent();
            ownerShape = Shape;
            // Shape Data section stores all attributes for the Shape
            // as defined by the user through this form.
            Utilities.insertShapeDataSection(ownerShape);
        }

        static public void SerializeToXML(C_Obj_Attribute_Form C_Obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(C_Obj_Attribute_Form));
            TextWriter textWriter = new StreamWriter("C_Obj.xml");
            serializer.Serialize(textWriter, C_Obj);
            textWriter.Close();
        }

        private void C_Obj_Attribute_Form_Load(object sender, EventArgs e)
        {
            // Load C Object Name
            loadCObjectName();

            // Loads all of the Shape's list of attribution from its Shapesheet Data Section.
            loadAttributeNameList();

            // Sets the first attribute in the list, if there is any, as the selected item
            // in the ListBox and displays its properties in the Attribute Properties.
            if (attributeListBox.Items.Count > 0)
            {
                attributeListBox.SetSelected(0, true);
                string atName = attributeListBox.SelectedItem.ToString();
                displayAttributeProperties(atName);
            }
        }
        private void displayAttributeProperties(string attributeName)
        {
            // Get name of selected attribute
            attributeListBox.Text = attributeName;
            // Build string to search shape data
            string rowName = "at_" + attributeName + "_"; 
            // Get name of attribute
            attributeNameText.Text = Utilities.getDataSectionValueCell(ownerShape, rowName + "name");
            // Get escription of attribute
            attributeDiscriptionText.Text = Utilities.getDataSectionValueCell(ownerShape, rowName + "discription");
            // Get domain of attribute
            attributeDomainText.Text = Utilities.getDataSectionValueCell(ownerShape, rowName + "domain");
        }
        private void loadCObjectName()
        {
            // Get number of rows in shape data section of object
            short numRows = ownerShape.get_RowCount(CaseTypes.SHAPE_DATA_SECTION);
            // Initialize an empty string
            string cName = "";
            // Loop created to search through each row of shape data section
            for (short r = 0; r < numRows; ++r)
            {
                // Initialize shape data cell
                Visio.Cell labelCell = ownerShape.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION,
                    r, CaseTypes.DS_LABEL_CELL);
                // Get string from shape data row label
                string labelCellValue = labelCell.get_ResultStr(Visio.VisUnitCodes.visUnitsString);
                // We are only interested in c object name
                if (labelCellValue.StartsWith("c_"))
                {
                    // We are only interested in the object name
                    // Get start index of c object name
                    int startIndex = labelCellValue.IndexOf('_') + 1;
                    // Get end index of c object name
                    int endIndex = labelCellValue.LastIndexOf('_');
                    // Get length of c object name
                    int cNameLen = endIndex - startIndex;
                    // Get c object name
                    cName = labelCellValue.Substring(startIndex, cNameLen);
                }
            }
            // Update c object name in c object editor
            cObjectNameText.Text = cName;
            // Update c object name
            ownerShape.Text = cName;
        }
        private void loadAttributeNameList()
        {
            // Initialize empty HashSet
            HashSet<string> attributeSet = new HashSet<string>();
            // All attribute rows are stored in the form: 
            // at_[attribute_name]_[attribute_property] in the Label Cell
            // Get number of rows in c object shape data section
            short numRows = ownerShape.get_RowCount(CaseTypes.SHAPE_DATA_SECTION);
            // Loop through each row of shape data section
            for (short r = 0; r < numRows; ++r)
            {
                // Initialize shape data label cell
                Visio.Cell labelCell = ownerShape.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION,
                    r, CaseTypes.DS_LABEL_CELL);
                // Get shape data label cell value
                string labelCellValue = labelCell.get_ResultStr(Visio.VisUnitCodes.visUnitsString);
                // Ee are only interested in attribute-related rows
                if (labelCellValue.StartsWith("at_"))
                {
                    // We are only interested in the attribute name
                    // Get start index of attribute name
                    int startIndex = labelCellValue.IndexOf('_') + 1;
                    // Get end index of attribute name
                    int endIndex = labelCellValue.LastIndexOf('_');
                    // Get length of attribute name
                    int atNameLen = endIndex - startIndex;
                    // Get attribute name
                    string atName = labelCellValue.Substring(startIndex, atNameLen);
                    // And add it to HashSet
                    attributeSet.Add(Utilities.underscoreToSpace(atName));
                }
            }
            // Add attribute names to attribute list box of c object editor
            attributeListBox.Items.AddRange(attributeSet.ToArray());
        }
        private void applyBtn_Click(object sender, EventArgs e)
        {
            // Save object name when apply button is pressed
            saveObjectName();
            // Save attributes when apply button is presed
            saveAttribute();
            this.attributeNameText.Text = "";
            this.attributeDomainText.Text = "";
            this.attributeDiscriptionText.Text = "";
        }
        private void saveObjectName()
        {
            // Shape Data section format
            // Row Name                         :: Value Cell
            // c_[object name]_                 :: [c object name]

            // Get c object name from object editor
            string cObjectName = cObjectNameText.Text.Trim();
            // Must have an object name
            // If empty
            if (cObjectName == "")
            {
                // Send message to user
                MessageBox.Show("Must enter an Object Name.");
            }
            else // There exists an object name
            {
                // Initialize string format for saving to shape data
                string rowName = "c_" + cObjectName + "_";
                // Save object name to shape data section
                Utilities.setDataSectionValueCell(ownerShape, rowName, cObjectName);
                // Change object shape's name
                ownerShape.Text = cObjectName;
            }
        }
        private void saveAttribute()
        {
            // Shape Data section format
            // Row Name                            ::  Value Cell
            // at_[attribute name]_                :: [attribute name]
            // at_[attribute name]_name            :: [attribute name]
            // at_[attribute name]_discription     :: [attribute discription]
            // at_[attribute name]_domain          :: [attribute domain]

            // Get attribute name
            string atName = attributeNameText.Text.Trim();
            // Must have an attribute name
            // If empty
            if (atName == "")
            {
                // Send message to the user
                MessageBox.Show("Must enter an Attribute Name.");
            }
            else // There exists an attribute name
            {
                // Initialize attribute name string in the format we want
                string rowName = "at_" + atName + "_";
                // Save attribute name into shape data section
                Utilities.setDataSectionValueCell(ownerShape, rowName + "name", atName);
                // Initialize attribute discription string in the format we want
                string discName = attributeDiscriptionText.Text;
                // Save attribute discription into shape data section
                Utilities.setDataSectionValueCell(ownerShape, rowName + "discription", discName);
                // Initialize attribute domain string in the format we want
                string domainName = attributeDomainText.Text;
                // Save attribute domain into shape data section
                Utilities.setDataSectionValueCell(ownerShape, rowName + "domain", domainName);
                // Update editor's attribute list
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
            // Collect list of names from attribute list box of the editor
            ListBox.ObjectCollection atNames = attributeListBox.Items;
            // Only add the attribute name if it doesn't already exist
            int itemIndex = Utilities.itemExists(atNames, attributeName);
            // Attribute does not exists
            if (itemIndex < 0)
            {
                // Add attribute
                atNames.Add(attributeName);
                // Decrement index
                itemIndex = atNames.Count - 1;
            }
            // In the editor, the latest attribute is selected
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
            // Close the object editor
            this.Close();
        }

        private void newAttributeBtn_Click(object sender, EventArgs e)
        {
            // When new attribute button is pressed
            // Clear text in attribute fields
            Utilities.clearTextBoxInGroupBox(attributePropGrpBox);
            // Clear old selection from attribute list box
            attributeListBox.ClearSelected();
        }

        private void delAttributeBtn_Click(object sender, EventArgs e)
        {
            // When delete attribute button is pressed
            Object selectedItem = attributeListBox.SelectedItem;
            // If an attribute is selected
            if (selectedItem != null)
            {
                // Get name of the selected attribute
                string selectedValue = selectedItem.ToString();
                // Removes the item from the ListBox
                attributeListBox.Items.Remove(selectedItem);
                // Removes the attribute and its properties from the Shapesheet
                // All attribute rows are prefixed with 'at_' in its name
                Utilities.deleteDataSectionRow(ownerShape, "at_" + selectedValue);
                //  Finaly clear attribute fields
                Utilities.clearTextBoxInGroupBox(attributePropGrpBox);
            }
            else // User did not select an attribute from atrribute list box
            {
                // Send message to the user
                MessageBox.Show("Select an Attribute to delete.");
            }
        }

        private void attributeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get item that the user selected from attribute list box
            Object item = attributeListBox.SelectedItem;
            // If an item exists
            if (item != null)
            {
                // Get the name of the attribute
                string atName = attributeListBox.SelectedItem.ToString();
                // And displays its properties
                displayAttributeProperties(atName);
            }
        }

        private void cObjectNameText_TextChanged(object sender, EventArgs e)
        {

        }


        public string c_object_name { get; set; }

        public string c_object_attribute { get; set; }
    }
}
