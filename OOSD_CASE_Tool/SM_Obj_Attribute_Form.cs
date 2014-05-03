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
    public partial class SM_Obj_Attribute_Form : Form
    {
        /// <summary>
        /// Reference to the Shape that owns (called) this form and whose shape
        /// data is defined using this form.
        /// </summary>
        private Visio.Shape ownerShape;

        /// <summary>
        /// List of shapes on this Page.
        /// </summary>
        private List<Visio.Shape> pageShapes;

        [XmlElement("SM_Obj_Name")]
        public string SM_Object_Name
        {
            get
            {
                return OOSDRibbon.printProperties(ownerShape);
            }
            set
            {
                if (ownerShape.Name.StartsWith("sm_"))
                    sm_object_name = ownerShape.Name;
            }
        }

        [XmlElement("SM_Obj_Operation")]
        public string SM_Object_Operation
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
                    var operation = s.Split(' ');
                    if (ownerShape.Name.StartsWith("sm_") && String.Equals((String)operation[2], "Operation"))
                        sm_object_operation = (String)operation[3];
                }
            }
        }

        [XmlElement("SM_Obj_State")]
        public string SM_Object_State
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
                    var state = s.Split(' ');
                    if (ownerShape.Name.StartsWith("sm_") && String.Equals((String)state[2], "State"))
                        sm_object_state = (String)state[3];
                }
            }
        }

        [XmlElement("SM_Obj_Event")]
        public string SM_Object_Event
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
                    var eve = s.Split(' ');
                    if (ownerShape.Name.StartsWith("sm_") && String.Equals((String)eve[2], "Event"))
                        sm_object_event = (String)eve[3];
                }
            }
        }

        [XmlElement("SM_Obj_Control")]
        public string SM_Object_Control
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
                    var control = s.Split(' ');
                    if (ownerShape.Name.StartsWith("sm_") && String.Equals((String)control[2], "Control"))
                        sm_object_control = (String)control[3];
                }
            }
        }

        public SM_Obj_Attribute_Form(Visio.Shape Shape)
        {
            // Initialize shape for visio
            InitializeComponent();
            ownerShape = Shape;
            // Get content from visio page
            pageShapes = Utilities.getAllShapesOnPage(Shape.ContainingPage);

            // Shape Data section stores all attributes for the Shape
            // as defined by the user through this form.
            Utilities.insertShapeDataSection(ownerShape);
        }

        static public void SerializeToXML(SM_Obj_Attribute_Form SM_Obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SM_Obj_Attribute_Form));
            TextWriter textWriter = new StreamWriter("SM_Obj.xml");
            serializer.Serialize(textWriter, SM_Obj);
            textWriter.Close();
        }

        private void SM_Obj_Attribute_Form_Load(object sender, EventArgs e)
        {
            loadObjNameTextBox();
            // Loads all of the page's list of objects into objListListBox.
            loadObjListListBox();

            // Loads all of the Shape's current list of objects from its Shape
            // Data Section into the objNameListbox
            loadObjNameListBox();

            // Loads all of the Shape's list of operations from its Shapesheet Data Section.
            loadOperationNameList();

            // Sets the first operation in the list, if there is any, as the selected item
            // in the ListBox and displays its properties in the Operation Properties.
            if (operationNameListBox.Items.Count > 0)
            {
                operationNameListBox.SetSelected(0, true);
                string opName = operationNameListBox.SelectedItem.ToString();
                displayOperationProperties(opName);
            }
        }
        private void loadObjNameTextBox()
        {
            // Get the number of rows from shape data section of the object
            short numRows = ownerShape.get_RowCount(CaseTypes.SHAPE_DATA_SECTION);
            // Loop throught each row
            for (short r = 0; r < numRows; ++r)
            {
                // Initialize label cell from shape data section
                Visio.Cell labelCell = ownerShape.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION,
                    r, CaseTypes.DS_LABEL_CELL);
                // Get the lavel cell's value
                string labelCellValue = labelCell.get_ResultStr(Visio.VisUnitCodes.visUnitsString);
                // Get object name first from shape data section
                if (labelCellValue.StartsWith("sm_"))
                {
                    // Get start index of object name
                    int startIndex = labelCellValue.IndexOf('_') + 1;
                    // Get end index of object name
                    int endIndex = labelCellValue.LastIndexOf('_');
                    // Get the length of object name
                    int smNameLen = endIndex - startIndex;
                    // Get the object name
                    string smObjName = labelCellValue.Substring(startIndex, smNameLen);
                    // Display the object name in the editor
                    smObjectNameTextBox.Text = smObjName;
                    ownerShape.Text = smObjName;
                    ownerShape.Name = smObjName;
                }
            }
        }
        /// <summary>
        /// Loads the list of names tied to this Shape from its Shape Data Section.
        /// </summary>
        private void loadObjNameListBox()
        {
            // Get list of objects from shape data section
            HashSet<string> objList = Utilities.getDSLabelCells(ownerShape, "obj_");
            // And display the names of objects in the object name list box of editor
            objNameListBox.Items.AddRange(objList.ToArray());
        }

        /// <summary>
        /// Loads the list of Objects found on the Object Editor Page into the
        /// objListListBox. Only load C & ADT Object names.
        /// </summary>
        private void loadObjListListBox()
        {
            // Go through each object that is in visio page
            foreach (Visio.Shape s in pageShapes)
            {
                // Get name of object
                string shapeMaster = s.Master.Name;
                // If the object is not SM Object
                if (shapeMaster != CaseTypes.SM_OBJ_MASTER)
                {
                    // Add the object to object list box of editor
                    objListListBox.Items.Add(s.Text);
                }
            }
        }


        /// <summary>
        /// Displays the operation's properties in the operationPropertiesGroupBox.
        /// </summary>
        /// <param name="operationName">Name of operation to display.</param>
        private void displayOperationProperties(string operationName)
        {
            // Get name of the operation to display it in editor
            operationNameTextBox.Text = operationName;
            // Format the name of operation to store in shape data
            string rowName = "op_" + operationName + "_";
            // Store operation state
            nextStateTextBox.Text = Utilities.getDataSectionValueCell(ownerShape, rowName + "state");
            // Store operation event
            eventTextBox.Text = Utilities.getDataSectionValueCell(ownerShape, rowName + "event");
            // Store operation control
            controlTextBox.Text = Utilities.getDataSectionValueCell(ownerShape, rowName + "control");
        }

        /// <summary>
        /// Retrieves the list of operations (if any) associated with this Shape
        /// from the Data Section and loads the operationNameListBox with the names.
        /// </summary>
        private void loadOperationNameList()
        {
            // Get operations from shape data section
            HashSet<string> opsList = Utilities.getDSLabelCells(ownerShape, "op_");
            // And list them in operation list box in editor
            operationNameListBox.Items.AddRange(opsList.ToArray());
        }

        /// <summary>
        /// Closes the form. Does not save any un-applied changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitBtn_Click(object sender, EventArgs e)
        {
            // If object is not modified
            if (ownerShape.get_RowCount(CaseTypes.SHAPE_DATA_SECTION) == 0)
            {
                // Delete empty object
                ownerShape.Delete();
            }
            // And close editor
            this.Close();
        }

        private void operationNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Saves any new/updated form data to the Shapesheet Data Section.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void applyBtn_Click(object sender, EventArgs e)
        {
            saveObjectName();
            // Save operation when apply button is pressed
            saveOperation();
        }
        private void saveObjectName()
        {
            // Shape Data section format
            // Row Name                     ::  Value Cell
            // sm_[object name]_            :: [object name]
            string smName =  smObjectNameTextBox.Text.Trim();
            // Must have an operation name
            if (smName == "")
            {
                // Send message to enter operation name
                MessageBox.Show("Must enter an SM Object Name before proceeding.");
            }
            else // There exists object name in editor
            {
                // Format string the way we want to store it in shape data
                string rowName = "sm_" + smName + "_";
                // Store object name in shape data
                Utilities.setDataSectionValueCell(ownerShape, rowName, smName);
            }
            ownerShape.Name = smName;
            ownerShape.Text = smName;
        }
        /// <summary>
        /// Saves an operation and its properties, taken from the operationPropertiesGroupBox
        /// input text boxes, in the Shapesheet Data Section.
        /// </summary>
        private void saveOperation()
        {
            // Shape Data section format
            // Row Name                    ::  Value Cell
            // op_[operation name]_        :: [operation name]
            // op_[operation name]_state   :: [state name]
            // op_[operation name]_event   :: [event]
            // op_[operation name]_control :: [control]
            
            // Get operation name from text box in editor
            string opName = operationNameTextBox.Text.Trim();
            // Must have an operation name
            if (opName == "")
            {
                // Send message to enter operation name
                MessageBox.Show("Must enter an Operation Name.");
            }
            else // There exists operation name in editor
            {
                // Format string the way we want to store it in shape data
                string rowName = "op_" + opName + "_";
                // Store operation name in shape data
                Utilities.setDataSectionValueCell(ownerShape, rowName, opName);
                // Get operation state name
                string stateName = nextStateTextBox.Text;
                // Store operaion state value in shape data
                Utilities.setDataSectionValueCell(ownerShape, rowName + "state", stateName);
                // Get operation event value
                string eventName = eventTextBox.Text;
                // Store operation event value in shape data
                Utilities.setDataSectionValueCell(ownerShape, rowName + "event", eventName);
                // Get operation control value
                string controlName = controlTextBox.Text;
                // Store operation control value in shape data
                Utilities.setDataSectionValueCell(ownerShape, rowName + "control", controlName);
                // Update operation list
                updateOperationsList(operationNameTextBox.Text);
            }
        }

        /// <summary>
        /// If the given operationName doesn't exist, add it to the operationNameListBox
        /// and set the newly added item as the SelectedItem in the ListBox.
        /// </summary>
        /// <param name="operationName">
        /// Name of the operation to add to the operationNameListBox.
        /// </param>
        private void updateOperationsList(string operationName)
        {
            // Get list of operation names from operation list box
            //  of the object editor
            ListBox.ObjectCollection opNames = operationNameListBox.Items;
            // Only add the operation name if it doesn't already exist
            int itemIndex = Utilities.itemExists(opNames, operationName);
            // If operation does not exist
            if (itemIndex < 0)
            {
                // Add it to operation name list
                opNames.Add(operationName);
                // Decrement index
                itemIndex = opNames.Count - 1;
            }
            // Select the new operation in the operation list box
            //  in the object editor
            operationNameListBox.SetSelected(itemIndex, true);
        }

        /// <summary>
        /// Clears out the input text boxes in the Operation Properties Group Box 
        /// to let user enter information for the new operation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newOperationBtn_Click(object sender, EventArgs e)
        {
            // Clear text fields associated with operation in the editor
            Utilities.clearTextBoxInGroupBox(operationPropertiesGroupBox);
            // Clear operation list box selected item
            operationNameListBox.ClearSelected();
        }

        /// <summary>
        /// Removes the currently selected operation name entry from the
        /// operationNameListBox and from the Shapesheet Data Section.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteOperationBtn_Click(object sender, EventArgs e)
        {
            // Get selected operation from operation list box in editor
            Object selectedItem = operationNameListBox.SelectedItem;
            // If an valid operation is selected
            if (selectedItem != null)
            {
                // Get the name of the operation
                string selectedValue = selectedItem.ToString();

                // Removes the item from the ListBox
                operationNameListBox.Items.Remove(selectedItem);

                // Removes the operation and its properties from the Shapesheet
                // All operation rows are prefixed with 'op_' in its name
                Utilities.deleteDataSectionRow(ownerShape, "op_" + selectedValue);
                Utilities.clearTextBoxInGroupBox(operationPropertiesGroupBox);
            } 
            else // No Operation was selected
            {
                MessageBox.Show("Select an Operation to delete.");
            }
        }

        /// <summary>
        /// When user selects an item in the operationNameListBox, display the
        /// operation's properties in the Operation Properties GroupBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void operationNameListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Operation selected in the operation name list box in the editor
            Object item = operationNameListBox.SelectedItem;
            // If the operation exists
            if (item != null)
            {
                // Get the name of the operation
                string opName = operationNameListBox.SelectedItem.ToString();
                // And display its properties in the editor
                displayOperationProperties(opName);
            }
        }

        private void objNameListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void objListListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Adds an item from the objListListBox to the objNameListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addObjBtn_Click(object sender, EventArgs e)
        {
            // Must name sm object before proceeding
            saveObjectName();
            // An object is selected in the object list box in the editor
            object selected = objListListBox.SelectedItem;
            // If the object exists
            if (selected != null)
            {
                // Add object to the object name list box in the editor
                objNameListBox.Items.Add(selected);
                // Adds the name of this item to this Shape's Data Section
                string objName = selected.ToString();
                // Format string the way we want to store in shape data
                string rowName = "obj_" + objName + "_";
                // Update shape data
                Utilities.setDataSectionValueCell(ownerShape, rowName, objName);
            }
        }

        /// <summary>
        /// Removes an item from the objNameListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeObjBtn_Click(object sender, EventArgs e)
        {
            // An object is selected for removal
            object selected = objNameListBox.SelectedItem;
            // The object is a valid object
            if (selected != null)
            {
                // Remove object from object list box in the editor
                objNameListBox.Items.Remove(selected);
                // Removes the name of this item from the Shape's Data Section.
                string rowName = "obj_" + selected.ToString();
                Utilities.deleteDataSectionRow(ownerShape, rowName);
            }
        }

        private void smObjectNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }


        public string sm_object_name { get; set; }

        public string sm_object_operation { get; set; }

        public string sm_object_state { get; set; }

        public string sm_object_event { get; set; }

        public string sm_object_control { get; set; }
    }
}
