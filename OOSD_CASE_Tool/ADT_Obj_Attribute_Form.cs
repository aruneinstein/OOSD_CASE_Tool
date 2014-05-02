using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Visio = Microsoft.Office.Interop.Visio;

namespace OOSD_CASE_Tool
{
    public partial class ADT_Obj_Attribute_Form : Form
    {
        /// <summary>
        /// Reference to the Shape that owns (called) this form and whose shape
        /// data is defined using this form.
        /// </summary>
        private Visio.Shape ownerShape;
        // List to store operations
        private List<Operation> operationList;
        // List to store axioms
        private List<string> axiomList;

        [XmlElement("ADT_Obj_Name")]
        public string ADT_Object_Name
        {
            get
            {
                return OOSDRibbon.printProperties(ownerShape);
            }
            set
            {
                if (ownerShape.Name.StartsWith("adt_"))
                    ADT_Object_Name = ownerShape.Name;
            }
        }

        [XmlElement("ADT_Obj_Operation")]
        public string ADT_Object_Operation
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
                    if (ownerShape.Name.StartsWith("adt_") && String.Equals((String)operation[2], "Operation"))
                    ADT_Object_Operation = (String)operation[3];
                }
            }
        }

        [XmlElement("ADT_Obj_Axiom")]
        public string ADT_Object_Axiom
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
                    var axiom = s.Split(' ');
                    if (ownerShape.Name.StartsWith("adt_") && String.Equals((String)axiom[2], "Axiom"))
                        ADT_Object_Axiom = (String)axiom[3];
                }
            }
        }

        public ADT_Obj_Attribute_Form(Visio.Shape shape)
        {
            // Initialize shape in visio
            InitializeComponent();
            ownerShape = shape;
            // Initialize empty operation list
            operationList = new List<Operation>();
            // Initialize empty axiom list
            axiomList = new List<string>();
            // Shape Data section stores all attributes for the Shape
            // as defined by the user through this form.
            Utilities.insertShapeDataSection(ownerShape);
        }

        static public void SerializeToXML(ADT_Obj_Attribute_Form ADT_Obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ADT_Obj_Attribute_Form));
            TextWriter textWriter = new StreamWriter("ADT_Obj.xml");
            serializer.Serialize(textWriter, ADT_Obj);
            textWriter.Close();
        }

        private void ADT_Obj_Attribute_Form_Load(object sender, EventArgs e)
        {
            // When an object is double clicked, load properties if any
            loadObject();
        }
        private void loadObject()
        {
            // Create an empty HasSet to store operations
            HashSet<string> operationSet = new HashSet<string>();
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
                if (labelCellValue.StartsWith("adt_"))
                {
                    // Get start index of object name
                    int startIndex = labelCellValue.IndexOf('_') + 1;
                    // Get end index of object name
                    int endIndex = labelCellValue.LastIndexOf('_');
                    // Get the length of object name
                    int opNameLen = endIndex - startIndex;
                    // Get the object name
                    string adtObjName = labelCellValue.Substring(startIndex, opNameLen);
                    // Display the object name in the editor
                    adtObjectNameTextBox.Text = adtObjName;
                    ownerShape.Name = adtObjName;
                    ownerShape.Text = adtObjName;
                }
                // Get the operation name from the shape data section
                if (labelCellValue.EndsWith("name"))
                {
                    // Get start index of operation name
                    int startIndex = labelCellValue.IndexOf('_') + 1;
                    // Get end index of operation name
                    int endIndex = labelCellValue.LastIndexOf('_');
                    // Get the length of the operation name
                    int opNameLen = endIndex - startIndex;
                    // Get the operation name
                    string opName = labelCellValue.Substring(startIndex, opNameLen);
                    // Initialize an object
                    Operation opObj = new Operation();
                    // Format string to the way we want to stored the operation values
                    string rowName = "op_" + opName + "_";
                    // Get the operation name and store it in operation object
                    opObj.name = Utilities.getDataSectionValueCell(ownerShape, rowName + "name");
                    // Get the domain value and store it in operation object
                    opObj.domain = Utilities.getDataSectionValueCell(ownerShape, rowName + "domain");
                    // Get the range value and store it in operation object
                    opObj.range = Utilities.getDataSectionValueCell(ownerShape, rowName + "range");
                    // Get the purpose value and store it in operation object
                    opObj.purpose = Utilities.getDataSectionValueCell(ownerShape, rowName + "purpose");
                    // Get the effects value and store it in operation object
                    opObj.effects = Utilities.getDataSectionValueCell(ownerShape, rowName + "effects");
                    // If there exists an exception list associated with an operation
                    if (Utilities.getDataSectionValueCell(ownerShape, rowName + "exceptions_list").Any())
                    {
                        // Get the exceptions and store them in operation object
                        opObj.exceptions = Utilities.getDataSectionValueCell(ownerShape, 
                            rowName + "exceptions_list").Split(',').Select(a => a.Trim()).ToList();
                    }
                    // Add the operation to the list of operations
                    this.operationList.Add(opObj);
                    // And add it to the editor's opeartion list box
                    operationListBox.Items.Add(opObj.name);
                }
                // Get the axioms from the shape data section
                if (labelCellValue.StartsWith("axiom"))
                {
                    // Format string to search in shape data section
                    string rowName = "axiom_list";
                    // Get the list of axioms and store them in list of axioms
                    axiomList = Utilities.getDataSectionValueCell(ownerShape, rowName).Split(',').Select(a => a.Trim()).ToList();
                    // Update the editor with list of axioms
                    axiomListBox.DataSource = axiomList;
                }
            }
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            // If the user cancels editing the object and the object has no data
            //  associated with it
            if (ownerShape.get_RowCount(CaseTypes.SHAPE_DATA_SECTION) == 0)
            {
                // Delete the object
                ownerShape.Delete();
            }
            // And close the editor
            this.Close();
        }

        private void addOpButton_Click(object sender, EventArgs e)
        {
            // Clear the exceptions list box in the editor
            exceptionListBox.Items.Clear();
            // Initialize new operation
            Operation opObj = new Operation();
            // Get operation name from editor
            opObj.name = nameTextBox.Text.Trim().ToString();
            // Get operation domain from editor
            opObj.domain = domainTextBox.Text.Trim().ToString();
            // Get operation range from editor
            opObj.range = rangeTextBox.Text.Trim().ToString();
            // Get operation purpose from editor
            opObj.purpose = purposeTextBox.Text.Trim().ToString();
            // Get operation effects from editor
            opObj.effects = effectsTextBox.Text.Trim().ToString();
            // Get operation exceptions from editor
            opObj.exceptions = getListOfExceptions();
            // If any field from the editor associated with operation
            //  fields is empty
            if (opObj.name.Equals("",StringComparison.Ordinal) ||
                opObj.domain.Equals("", StringComparison.Ordinal) ||
                opObj.range.Equals("", StringComparison.Ordinal) ||
                opObj.purpose.Equals("", StringComparison.Ordinal) ||
                opObj.effects.Equals("", StringComparison.Ordinal))
            {
                // Send user message to add data
                MessageBox.Show("One/All of the required fields is/are not filled! (Including exceptions list)");
                return;
            }
            // Clear exception list box in the editor
            exceptionListBox.Items.Clear();
            // Also clear all operation fields in the editor
            nameTextBox.Clear();
            domainTextBox.Clear();
            rangeTextBox.Clear();
            purposeTextBox.Clear();
            effectsTextBox.Clear();
            exceptTextBox.Clear();
            // Look for operation in the operation list
            var optn = this.operationList.Find(x => x.name.Equals(opObj.name, StringComparison.Ordinal));
            // If the operation does not exist
            if (optn == null)
            {
                // Add the operation to the operation list
                this.operationList.Add(opObj);
                // And add the operation name to operation list box in the editor
                operationListBox.Items.Add(opObj.name);
            }
            else // Operation exists
            {
                // Update the operation with new values
                optn.name = opObj.name;
                optn.domain = opObj.domain;
                optn.range = opObj.range;
                optn.purpose = opObj.purpose;
                optn.effects = opObj.effects;
                optn.exceptions = opObj.exceptions;
            }
            // Clear exception list box in the editor
            exceptionListBox.Items.Clear();
            // Clear fileds associated with operations in the editor
            nameTextBox.Clear();
            domainTextBox.Clear();
            rangeTextBox.Clear();
            purposeTextBox.Clear();
            effectsTextBox.Clear();
            exceptTextBox.Clear();
        }

        private List<string> getListOfExceptions()
        {
            // Initialize new list for exceptions
            var lOfExc = new List<string>();
            // Add exceptions from the exceptions list box of the editor
            //  to object collection
            ListBox.ObjectCollection excptns = exceptionListBox.Items;
            // Go through the collection
            foreach (var item in excptns)
            {
                // Adding each item to exception list
                lOfExc.Add(item.ToString());
            }
            // Return the list of exceptions
            return lOfExc;
        }

        private void delOpButton_Click(object sender, EventArgs e)
        {
            // If the operation list box is empty when deleting
            if (operationListBox.SelectedItem == null)
            {
                // Send user message to select an item
                MessageBox.Show("No object selected for deletion!");
                return;
            }
            // Else, the user selected an operation from the
            //  operation list box of the editor
            // Get name of the operation
            string opr = operationListBox.SelectedItem.ToString();
            // Check if the operation is not an empty string
            if (opr.Equals("", StringComparison.Ordinal))
            {
                // Send message to user that no object was selected
                MessageBox.Show("No object selected for deletion!");
            }
            // Initialize empty operation
            Operation opObj = null;
            // Go through the list of operations to
            //  look for user selected operation
            foreach (var item in operationList)
            {
                // If the operation was found
                if (item.name.Equals(opr, StringComparison.Ordinal))
                {
                    // Get the operation
                    opObj = item;
                }
            }
            // Operation was found
            if (opObj != null)
            {
                // Delete the operation
                operationList.Remove(opObj);
                // And clear it from operation list box of the editor
                operationListBox.Items.Remove(operationListBox.SelectedItem);
            }
        }

        private void operationListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear the exceptions list box in the object editor
            exceptionListBox.Items.Clear();
            // Get the selected operation
            Operation op = operationList.Find(x => x.name == operationListBox.SelectedItem.ToString());
            // Get the associated exceptions list and display it in editor
            exceptionListBox.Items.AddRange(op.exceptions.ToArray<string>());
            // Clear old operatio name
            nameTextBox.Clear();
            // Display new operation name
            nameTextBox.AppendText(op.name);
            // Clear old operation domain
            domainTextBox.Clear();
            // Display new operation domain
            domainTextBox.AppendText(op.domain);
            // Clear old operation range
            rangeTextBox.Clear();
            // Display new operation range
            rangeTextBox.AppendText(op.range);
            // Clear old operation purpose
            purposeTextBox.Clear();
            // Display new operation purpose
            purposeTextBox.AppendText(op.purpose);
            // Clear old operation effects
            effectsTextBox.Clear();
            // Display new operation effects
            effectsTextBox.AppendText(op.effects);
        }

        private void addExceptionButton_Click(object sender, EventArgs e)
        {
            // Get name of new exception to add
            var exc = exceptTextBox.Text.ToString().Trim();
            // If the exceptions already exists in the exception list
            if (exceptionListBox.Items.Contains(exc))
            {
                // Do nothing
                return;
            }
            // Else, if the text box is empty
            if (exc.Equals("", StringComparison.Ordinal))
            {
                // Send user a message to enter data
                MessageBox.Show("Please enter the exception information!");
                return;
            }
            // User must select an operation to associate exception with
            if (operationListBox.SelectedItem == null)
            {
                // Send user a message if no operation was selected
                MessageBox.Show("No operation to associate the exception with! Please select an operation.");
                return;
            }
            // Get the operation from the operation list box of the editor
            var op = getOperationFromOpListBox(operationListBox.SelectedItem.ToString());
            // If the exception does not exist in the operation
            if (!op.exceptions.Contains<string>(exc))
            {
                // Add the exception
                op.exceptions.Add(exc);
                // And update the exception list box in the editor
                exceptionListBox.Items.Add(exc);
            }
            // Clear the exception text box in the editor
            exceptTextBox.Clear();
            // Clear all operation fields int eh editor
            nameTextBox.Clear();
            domainTextBox.Clear();
            rangeTextBox.Clear();
            purposeTextBox.Clear();
            effectsTextBox.Clear();
            exceptTextBox.Clear();
        }

        private void delExceptionButton_Click(object sender, EventArgs e)
        {
            // If no exception was selected and delete button pressed
            if (exceptionListBox.SelectedItem == null)
            {
                // Send user message to select an exception
                MessageBox.Show("No object selected for deletion!");
                return;
            }
            // An exception selected for deletion
            string exc = exceptionListBox.SelectedItem.ToString();
            // If the exception name is an empty string
            if (exc.Equals("", StringComparison.Ordinal))
            {
                // Send user a message to select proper exception
                MessageBox.Show("No object selected for deletion!");
                return;
            }
            // Get the associated operation for the selected exception
            var op = getOperationFromOpListBox(operationListBox.SelectedItem.ToString());
            // Remove the exception from the operation
            op.exceptions.Remove(exc);
            // And remove the exception from the exception list box of the editor
            exceptionListBox.Items.Remove(exceptionListBox.SelectedItem);
            // Clear the exception text box of the editor
            exceptTextBox.Clear();
        }

        private Operation getOperationFromOpListBox(string selop)
        {
            // Look for an operation in operation list and return it
            return operationList.Find(x => x.name.Equals(selop, StringComparison.Ordinal));
        }

        private void addAxiomButton_Click(object sender, EventArgs e)
        {
            // Get axiom name form axiom text box of the editor
            var axm = axiomTextBox.Text.ToString().Trim();
            // If the axiom already exists in the axiom list box or
            //  if the user tried to save an empty axiom
            if (axiomListBox.Items.Contains(axm) || axm.Equals("", StringComparison.Ordinal))
            {
                // Send user message to enter proper information
                MessageBox.Show("Please enter axiom information!");
                return;
            }
            // Add the axiom to the axiom list
            axiomList.Add(axm);
            // Also, add it to axiom ist box in the editor
            axiomListBox.Items.Add(axm);
            // And clear the axiom text box in the editor
            axiomTextBox.Clear();
        }

        private void delAxiomButton_Click(object sender, EventArgs e)
        {
            // Delete axiom button pressed without
            //  first selecting an axiom
            if (axiomListBox.SelectedItem == null)
            {
                // Send user message to select an axiom
                MessageBox.Show("No object selected for deletion!");
                return;
            }
            // Get the name of selected axiom
            string axm = axiomListBox.SelectedItem.ToString();
            // If the name contain empty string
            if (axm.Equals("", StringComparison.Ordinal))
            {
                // Send user message to select axiom
                MessageBox.Show("No object selected for deletion!");
                return;
            }
            // When proper axiom is selected, delete it from axiom list
            axiomList.Remove(axm);
            // And remove it from axiom list box from 
            axiomListBox.Items.Remove(axiomListBox.SelectedItem);
            // Clear axiom text box in editor
            axiomTextBox.Clear();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // Save object name
            saveObjectName();
            // Save associated operations
            saveOperations();
        }
        private void saveObjectName()
        {
            // Shape Data section format
            // Row Name                         :: Value Cell
            // adt_[object name]_               :: [adt object name]
            // Get object name from text box of editor
            string adtObjectName = adtObjectNameTextBox.Text.Trim();
            // Must have an object name
            if (adtObjectName == "")
            {
                // Send user message to enter object name
                MessageBox.Show("Must enter an Object Name.");
            }
            else // User entered proper name
            {
                // For string to save the way we want on the shape data section
                string rowName = "adt_" + adtObjectName + "_object";
                // Save object name
                Utilities.setDataSectionValueCell(ownerShape, rowName, adtObjectName);
                // Change object name
                ownerShape.Name = adtObjectName;
                ownerShape.Text = adtObjectName;
            }

        }
        private void saveOperations()
        {
            // To save operaion onto shape data section
            // For every operation in the operation list
            foreach (var o in operationList)
            {
                // Save name, domain, range, purpose, effects, and exception list
                //  of the operation to shape data section
                // Format the way we want to save operation in shape data
                string rowName = "op_" + o.name + "_";
                Utilities.setDataSectionValueCell(ownerShape, rowName + "name", o.name);
                string damainName = o.domain;
                Utilities.setDataSectionValueCell(ownerShape, rowName + "domain", damainName);
                string rangeName = o.range;
                Utilities.setDataSectionValueCell(ownerShape, rowName + "range", rangeName);
                string purposeName = o.purpose;
                Utilities.setDataSectionValueCell(ownerShape, rowName + "purpose", purposeName);
                string effectsName = o.effects;
                Utilities.setDataSectionValueCell(ownerShape, rowName + "effects", effectsName);
                // If there exists an any exceptions associated with an operation
                if (o.exceptions.Any())
                {
                    // Get name of exception and join it with delimiter ","
                    string exceptionsName = String.Join(", ", o.exceptions.ToArray());
                    // Store the exception list
                    Utilities.setDataSectionValueCell(ownerShape, rowName + "exceptions_list", exceptionsName);
                }
                // Save acioms associated with an object
                // Format axiom name
                string axiomName = "axiom_";
                // And store it in shape data section with the delimiter ","
                Utilities.setDataSectionValueCell(ownerShape, axiomName + "list", String.Join(", ", axiomList.ToArray()));
            }
            // Close the object editor window
            this.Close();
        }
        private void axiomListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If user clicked on axiom from axiom list box
            if(axiomListBox.SelectedItem != null)
            {
                // Clear axiom text box in editor
                axiomTextBox.Clear();
                // And display axiom name of selected axiom from axiom list box
                axiomTextBox.AppendText(axiomListBox.SelectedItem.ToString());
            }
        }

        private void exceptionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If user selected an exception in the exception list box
            //  of the object editor
            if (exceptionListBox.SelectedItem != null)
            {
                // Clear exception text box
                exceptTextBox.Clear();
                // And display exception name of exception selected from exception list box
                exceptTextBox.AppendText(exceptionListBox.SelectedItem.ToString());
            }
        }
        private void adtObjectNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }


    class Operation
    {
        // Name of an operation
        public string name;
        // Range of an operation
        public string range;
        // Domain of an operation
        public string domain;
        // Purpose of an operation
        public string purpose;
        // Effects of an operation
        public string effects;
        // List of exceptions for an operation
        public List<string> exceptions;
    }

}
