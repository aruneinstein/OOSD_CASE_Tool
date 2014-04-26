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
    public partial class ADT_Obj_Attribute_Form : Form
    {
        /// <summary>
        /// Reference to the Shape that owns (called) this form and whose shape
        /// data is defined using this form.
        /// </summary>
        private Visio.Shape ownerShape;
        private string objectName;
        private List<Operation> operationList;
        private List<string> axiomList;

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

        private void ADT_Obj_Attribute_Form_Load(object sender, EventArgs e)
        {
            if (ownerShape.get_RowCount(CaseTypes.SHAPE_DATA_SECTION) > 0)
            {
                // load all operations and exceptions into lists and onto list boxes.
            }
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
            Operation opObj = new Operation();
            opObj.name = nameTextBox.Text.Trim().ToString();
            opObj.domain = domainTextBox.Text.Trim().ToString();
            opObj.range = rangeTextBox.Text.Trim().ToString();
            opObj.purpose = purposeTextBox.Text.Trim().ToString();
            opObj.exceptions = getListOfExceptions();

            if (opObj.name.Equals("",StringComparison.Ordinal) ||
                opObj.domain.Equals("", StringComparison.Ordinal) ||
                opObj.range.Equals("", StringComparison.Ordinal) ||
                opObj.purpose.Equals("", StringComparison.Ordinal) ||
                opObj.exceptions.Count == 0)
            {
                MessageBox.Show("One/All of the required fields is/are not filled!");
                return;
            }

            exceptionListBox.Items.Clear();
            nameTextBox.Clear();
            domainTextBox.Clear();
            rangeTextBox.Clear();
            purposeTextBox.Clear();
            exceptTextBox.Clear();

            this.operationList.Add(opObj);
            operationListBox.Items.Add(opObj.name);
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
            string opr;
            try
            {
                opr = operationListBox.SelectedItem.ToString();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("No object selected for deletion!");
                return;
            }
            
            Operation opObj = null;

            if (opr.Equals("", StringComparison.Ordinal))
            {
                MessageBox.Show("No object selected for deletion!");
            }

            foreach (var item in operationList)
            {
                if (item.name.Equals(opr, StringComparison.Ordinal))
                {
                    opObj = item;
                }
            }

            operationList.Remove(opObj);
            operationListBox.Items.Remove(operationListBox.SelectedItem);
        }

        private void operationListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            exceptionListBox.Items.Clear();
            Operation op = operationList.Find(x => x.name == operationListBox.SelectedItem.ToString());
            exceptionListBox.Items.AddRange(op.exceptions.ToArray<string>());
        }

        private void addExceptionButton_Click(object sender, EventArgs e)
        {
            var exc = exceptTextBox.Text.ToString().Trim();
            if (exceptionListBox.Items.Contains(exc) || exc.Equals("", StringComparison.Ordinal))
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
            op.exceptions.Add(exc);
            exceptionListBox.Items.Add(exc);
        }

        private void delExceptionButton_Click(object sender, EventArgs e)
        {
            string exc;
            try
            {
                exc = exceptionListBox.SelectedItem.ToString();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("No object selected for deletion!");
                return;
            }
                

            if (exc.Equals("", StringComparison.Ordinal))
            {
                MessageBox.Show("No object selected for deletion!");
                return;
            }

            var op = getOperationFromOpListBox(operationListBox.SelectedItem.ToString());
            op.exceptions.Remove(exc);
            exceptionListBox.Items.Remove(exceptionListBox.SelectedItem);
            exceptionListBox.SetSelected(0, true);
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
        }

        private void delAxiomButton_Click(object sender, EventArgs e)
        {
            string axm;
            try
            {
                axm = axiomListBox.SelectedItem.ToString();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("No object selected for deletion!");
                return;
            }
            
            if (axm.Equals("", StringComparison.Ordinal))
            {
                MessageBox.Show("No object selected for deletion!");
                return;
            }

            axiomList.Remove(axm);
            axiomListBox.Items.Remove(axiomListBox.SelectedItem);
            axiomListBox.SetSelected(0, true);
        }

        private void saveButton_Click(object sender, EventArgs e)
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
