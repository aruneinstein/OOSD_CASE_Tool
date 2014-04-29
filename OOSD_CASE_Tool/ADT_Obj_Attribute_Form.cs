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
        private List<Operation> operationList;
        private List<string> axiomList;

        [XmlElement("ADT_Obj_Name")]
        public string ADT_Object_Name
        { get; set; }

        [XmlElement("ADT_Obj_Operation")]
        public string ADT_Object_Operation
        { get; set; }

        [XmlElement("ADT_Obj_Axiom")]
        public string ADT_Object_Axiom
        { get; set; }

        public ADT_Obj_Attribute_Form(Visio.Shape shape)
        {
            InitializeComponent();
            ownerShape = shape;
            operationList = new List<Operation>();
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
           loadObject();
        }
        private void loadObject()
        {
            HashSet<string> operationSet = new HashSet<string>();
            short numRows = ownerShape.get_RowCount(CaseTypes.SHAPE_DATA_SECTION);
            for (short r = 0; r < numRows; ++r)
            {
                Visio.Cell labelCell = ownerShape.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION,
                    r, CaseTypes.DS_LABEL_CELL);

                string labelCellValue = labelCell.get_ResultStr(Visio.VisUnitCodes.visUnitsString);

                if (labelCellValue.StartsWith("adt_"))
                {
                    int startIndex = labelCellValue.IndexOf('_') + 1;
                    int endIndex = labelCellValue.LastIndexOf('_');
                    int opNameLen = endIndex - startIndex;
                    string adtObjName = labelCellValue.Substring(startIndex, opNameLen);
                    adtObjectNameTextBox.Text = adtObjName;
                }
                if (labelCellValue.EndsWith("name"))
                {
                    int startIndex = labelCellValue.IndexOf('_') + 1;
                    int endIndex = labelCellValue.LastIndexOf('_');
                    int opNameLen = endIndex - startIndex;
                    string opName = labelCellValue.Substring(startIndex, opNameLen);
                    Operation opObj = new Operation();
                    string rowName = "op_" + opName + "_";
                    opObj.name = Utilities.getDataSectionValueCell(ownerShape, rowName + "name");
                    opObj.domain = Utilities.getDataSectionValueCell(ownerShape, rowName + "domain");
                    opObj.range = Utilities.getDataSectionValueCell(ownerShape, rowName + "range");
                    opObj.purpose = Utilities.getDataSectionValueCell(ownerShape, rowName + "purpose");
                    opObj.effects = Utilities.getDataSectionValueCell(ownerShape, rowName + "effects");
                    if (Utilities.getDataSectionValueCell(ownerShape, rowName + "exceptions_list").Any())
                    {
                        opObj.exceptions = Utilities.getDataSectionValueCell(ownerShape, rowName + "exceptions_list").Split(',').Select(a => a.Trim()).ToList();
                    }
                    this.operationList.Add(opObj);
                    operationListBox.Items.Add(opObj.name);
                }
                if (labelCellValue.StartsWith("axiom"))
                {
                    string rowName = "axiom_list";
                    axiomList = Utilities.getDataSectionValueCell(ownerShape, rowName).Split(',').Select(a => a.Trim()).ToList();
                    axiomListBox.DataSource = axiomList;
                }
            }
            //operationListBox.Items.AddRange(operationSet.ToArray());
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (ownerShape.get_RowCount(CaseTypes.SHAPE_DATA_SECTION) == 0)
            {
                ownerShape.Delete();
            }

            this.Close();
        }

        private void addOpButton_Click(object sender, EventArgs e)
        {
            exceptionListBox.Items.Clear();
            Operation opObj = new Operation();
            opObj.name = nameTextBox.Text.Trim().ToString();
            opObj.domain = domainTextBox.Text.Trim().ToString();
            opObj.range = rangeTextBox.Text.Trim().ToString();
            opObj.purpose = purposeTextBox.Text.Trim().ToString();
            opObj.effects = effectsTextBox.Text.Trim().ToString();
            opObj.exceptions = getListOfExceptions();

            if (opObj.name.Equals("",StringComparison.Ordinal) ||
                opObj.domain.Equals("", StringComparison.Ordinal) ||
                opObj.range.Equals("", StringComparison.Ordinal) ||
                opObj.purpose.Equals("", StringComparison.Ordinal) ||
                opObj.effects.Equals("", StringComparison.Ordinal))
            {
                MessageBox.Show("One/All of the required fields is/are not filled! (Including exceptions list)");
                return;
            }

            exceptionListBox.Items.Clear();
            nameTextBox.Clear();
            domainTextBox.Clear();
            rangeTextBox.Clear();
            purposeTextBox.Clear();
            effectsTextBox.Clear();
            exceptTextBox.Clear();

            var optn = this.operationList.Find(x => x.name.Equals(opObj.name, StringComparison.Ordinal));

            if (optn == null)
            {
                this.operationList.Add(opObj);
                operationListBox.Items.Add(opObj.name);
            }
            else
            {
                optn.name = opObj.name;
                optn.domain = opObj.domain;
                optn.range = opObj.range;
                optn.purpose = opObj.purpose;
                optn.effects = opObj.effects;
                optn.exceptions = opObj.exceptions;
            }
            exceptionListBox.Items.Clear();
            nameTextBox.Clear();
            domainTextBox.Clear();
            rangeTextBox.Clear();
            purposeTextBox.Clear();
            effectsTextBox.Clear();
            exceptTextBox.Clear();
        }

        private List<string> getListOfExceptions()
        {
            var lOfExc = new List<string>();
            ListBox.ObjectCollection excptns = exceptionListBox.Items;
            foreach (var item in excptns)
            {
                lOfExc.Add(item.ToString());
            }

            return lOfExc;
        }

        private void delOpButton_Click(object sender, EventArgs e)
        {
            if (operationListBox.SelectedItem == null)
            {
                MessageBox.Show("No object selected for deletion!");
                return;
            }
            string opr = operationListBox.SelectedItem.ToString();

            if (opr.Equals("", StringComparison.Ordinal))
            {
                MessageBox.Show("No object selected for deletion!");
            }
            
            Operation opObj = null;

            foreach (var item in operationList)
            {
                if (item.name.Equals(opr, StringComparison.Ordinal))
                {
                    opObj = item;
                }
            }

            if (opObj != null)
            {
                operationList.Remove(opObj);
                operationListBox.Items.Remove(operationListBox.SelectedItem);
            }
        }

        private void operationListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            exceptionListBox.Items.Clear();
            Operation op = operationList.Find(x => x.name == operationListBox.SelectedItem.ToString());
            exceptionListBox.Items.AddRange(op.exceptions.ToArray<string>());
            
            nameTextBox.Clear();
            nameTextBox.AppendText(op.name);
            
            domainTextBox.Clear();
            domainTextBox.AppendText(op.domain);
            
            rangeTextBox.Clear();
            rangeTextBox.AppendText(op.range);
            
            purposeTextBox.Clear();
            purposeTextBox.AppendText(op.purpose);
            
            effectsTextBox.Clear();
            effectsTextBox.AppendText(op.effects);
        }

        private void addExceptionButton_Click(object sender, EventArgs e)
        {
            var exc = exceptTextBox.Text.ToString().Trim();
            
            if (exceptionListBox.Items.Contains(exc))
            {
                return;
            }

            if (exc.Equals("", StringComparison.Ordinal))
            {
                MessageBox.Show("Please enter the exception information!");
                return;
            }

            if (operationListBox.SelectedItem == null)
            {
                MessageBox.Show("No operation to associate the exception with! Please select an operation.");
                return;
            }

            var op = getOperationFromOpListBox(operationListBox.SelectedItem.ToString());
            if (!op.exceptions.Contains<string>(exc))
            {
                op.exceptions.Add(exc);
                exceptionListBox.Items.Add(exc);
            }
            exceptTextBox.Clear();
            nameTextBox.Clear();
            domainTextBox.Clear();
            rangeTextBox.Clear();
            purposeTextBox.Clear();
            effectsTextBox.Clear();
            exceptTextBox.Clear();
        }

        private void delExceptionButton_Click(object sender, EventArgs e)
        {
            if (exceptionListBox.SelectedItem == null)
            {
                MessageBox.Show("No object selected for deletion!");
                return;
            }
            string exc = exceptionListBox.SelectedItem.ToString();
            
            if (exc.Equals("", StringComparison.Ordinal))
            {
                MessageBox.Show("No object selected for deletion!");
                return;
            }

            var op = getOperationFromOpListBox(operationListBox.SelectedItem.ToString());
            op.exceptions.Remove(exc);
            exceptionListBox.Items.Remove(exceptionListBox.SelectedItem);
            //exceptionListBox.SetSelected(0, true);
            exceptTextBox.Clear();
        }

        private Operation getOperationFromOpListBox(string selop)
        {
            return operationList.Find(x => x.name.Equals(selop, StringComparison.Ordinal));
        }

        private void addAxiomButton_Click(object sender, EventArgs e)
        {
            var axm = axiomTextBox.Text.ToString().Trim();
            if (axiomListBox.Items.Contains(axm) || axm.Equals("", StringComparison.Ordinal))
            {
                MessageBox.Show("Please enter axiom information!");
                return;
            }

            axiomList.Add(axm);
            axiomListBox.Items.Add(axm);
            axiomTextBox.Clear();
        }

        private void delAxiomButton_Click(object sender, EventArgs e)
        {
            if (axiomListBox.SelectedItem == null)
            {
                MessageBox.Show("No object selected for deletion!");
                return;
            }


            string axm = axiomListBox.SelectedItem.ToString();
            if (axm.Equals("", StringComparison.Ordinal))
            {
                MessageBox.Show("No object selected for deletion!");
                return;
            }

            axiomList.Remove(axm);
            axiomListBox.Items.Remove(axiomListBox.SelectedItem);
            //axiomListBox.SetSelected(0, true);
            axiomTextBox.Clear();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveObjectName();
            saveOperations();
            //saveAxioms();
        }
        private void saveObjectName()
        {
            // Shape Data section format
            //    row name                         :: Value cell
            // adt_[object name]_                  :: [adt object name]

            string adtObjectName = adtObjectNameTextBox.Text.Trim();
            // Must have an object name
            if (adtObjectName == "")
            {
                MessageBox.Show("Must enter an Object Name.");
            }
            else
            {
                string rowName = "adt_" + adtObjectName + "_object";

                Utilities.setDataSectionValueCell(ownerShape, rowName, adtObjectName);
                ownerShape.Name = adtObjectName;
            }

        }
        private void saveOperations()
        {
            foreach (var o in operationList)
            {
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
                if (o.exceptions.Any())
                {
                    string exceptionsName = String.Join(", ", o.exceptions.ToArray());
                    Utilities.setDataSectionValueCell(ownerShape, rowName + "exceptions_list", exceptionsName);
                }
                string axiomName = "axiom_";
                Utilities.setDataSectionValueCell(ownerShape, axiomName + "list", String.Join(", ", axiomList.ToArray()));
            }
            this.Close();
        }
        private void axiomListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(axiomListBox.SelectedItem != null)
            {
                axiomTextBox.Clear();
                axiomTextBox.AppendText(axiomListBox.SelectedItem.ToString());
            }
        }

        private void exceptionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (exceptionListBox.SelectedItem != null)
            {
                exceptTextBox.Clear();
                exceptTextBox.AppendText(exceptionListBox.SelectedItem.ToString());
            }
        }

        private void adtObjectNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }


    class Operation
    {
        public string name;
        public string range;
        public string domain;
        public string purpose;
        public string effects;
        public List<string> exceptions;
    }

}
