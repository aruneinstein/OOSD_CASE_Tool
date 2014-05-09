namespace OOSD_CASE_Tool
{
    partial class SM_Obj_Attribute_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.removeObjBtn = new System.Windows.Forms.Button();
            this.addObjBtn = new System.Windows.Forms.Button();
            this.objListLbl = new System.Windows.Forms.Label();
            this.objNameListBox = new System.Windows.Forms.ListBox();
            this.deleteOperationBtn = new System.Windows.Forms.Button();
            this.operationPropertiesGroupBox = new System.Windows.Forms.GroupBox();
            this.controlTextBox = new System.Windows.Forms.TextBox();
            this.controlLbl = new System.Windows.Forms.Label();
            this.eventTextBox = new System.Windows.Forms.TextBox();
            this.eventLbl = new System.Windows.Forms.Label();
            this.nextStateTextBox = new System.Windows.Forms.TextBox();
            this.nextStateLbl = new System.Windows.Forms.Label();
            this.operationNameTextBox = new System.Windows.Forms.TextBox();
            this.opNameLbl = new System.Windows.Forms.Label();
            this.newOperationBtn = new System.Windows.Forms.Button();
            this.operationsListLbl = new System.Windows.Forms.Label();
            this.operationNameListBox = new System.Windows.Forms.ListBox();
            this.applyBtn = new System.Windows.Forms.Button();
            this.exitBtn = new System.Windows.Forms.Button();
            this.separatorLine1 = new System.Windows.Forms.GroupBox();
            this.objListListBox = new System.Windows.Forms.ListBox();
            this.smObjectNameLbl = new System.Windows.Forms.Label();
            this.smObjectNameTextBox = new System.Windows.Forms.TextBox();
            this.operationPropertiesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // removeObjBtn
            // 
            this.removeObjBtn.Location = new System.Drawing.Point(185, 110);
            this.removeObjBtn.Name = "removeObjBtn";
            this.removeObjBtn.Size = new System.Drawing.Size(86, 23);
            this.removeObjBtn.TabIndex = 5;
            this.removeObjBtn.Text = "---> Remove";
            this.removeObjBtn.UseVisualStyleBackColor = true;
            this.removeObjBtn.Click += new System.EventHandler(this.removeObjBtn_Click);
            // 
            // addObjBtn
            // 
            this.addObjBtn.Location = new System.Drawing.Point(185, 77);
            this.addObjBtn.Name = "addObjBtn";
            this.addObjBtn.Size = new System.Drawing.Size(86, 23);
            this.addObjBtn.TabIndex = 4;
            this.addObjBtn.Text = "Add <---";
            this.addObjBtn.UseVisualStyleBackColor = true;
            this.addObjBtn.Click += new System.EventHandler(this.addObjBtn_Click);
            // 
            // objListLbl
            // 
            this.objListLbl.AutoSize = true;
            this.objListLbl.Location = new System.Drawing.Point(15, 52);
            this.objListLbl.Name = "objListLbl";
            this.objListLbl.Size = new System.Drawing.Size(65, 13);
            this.objListLbl.TabIndex = 2;
            this.objListLbl.Text = "Objects List:";
            // 
            // objNameListBox
            // 
            this.objNameListBox.FormattingEnabled = true;
            this.objNameListBox.Location = new System.Drawing.Point(15, 77);
            this.objNameListBox.Name = "objNameListBox";
            this.objNameListBox.Size = new System.Drawing.Size(159, 56);
            this.objNameListBox.TabIndex = 3;
            this.objNameListBox.SelectedIndexChanged += new System.EventHandler(this.objNameListBox_SelectedIndexChanged);
            // 
            // deleteOperationBtn
            // 
            this.deleteOperationBtn.Location = new System.Drawing.Point(312, 213);
            this.deleteOperationBtn.Name = "deleteOperationBtn";
            this.deleteOperationBtn.Size = new System.Drawing.Size(106, 23);
            this.deleteOperationBtn.TabIndex = 11;
            this.deleteOperationBtn.Text = "Delete Operation";
            this.deleteOperationBtn.UseVisualStyleBackColor = true;
            this.deleteOperationBtn.Click += new System.EventHandler(this.deleteOperationBtn_Click);
            // 
            // operationPropertiesGroupBox
            // 
            this.operationPropertiesGroupBox.Controls.Add(this.controlTextBox);
            this.operationPropertiesGroupBox.Controls.Add(this.controlLbl);
            this.operationPropertiesGroupBox.Controls.Add(this.eventTextBox);
            this.operationPropertiesGroupBox.Controls.Add(this.eventLbl);
            this.operationPropertiesGroupBox.Controls.Add(this.nextStateTextBox);
            this.operationPropertiesGroupBox.Controls.Add(this.nextStateLbl);
            this.operationPropertiesGroupBox.Controls.Add(this.operationNameTextBox);
            this.operationPropertiesGroupBox.Controls.Add(this.opNameLbl);
            this.operationPropertiesGroupBox.Location = new System.Drawing.Point(15, 254);
            this.operationPropertiesGroupBox.Name = "operationPropertiesGroupBox";
            this.operationPropertiesGroupBox.Size = new System.Drawing.Size(428, 98);
            this.operationPropertiesGroupBox.TabIndex = 12;
            this.operationPropertiesGroupBox.TabStop = false;
            this.operationPropertiesGroupBox.Text = "Operation Properties";
            this.operationPropertiesGroupBox.Enter += new System.EventHandler(this.operationPropertiesGroupBox_Enter);
            // 
            // controlTextBox
            // 
            this.controlTextBox.Location = new System.Drawing.Point(271, 62);
            this.controlTextBox.Name = "controlTextBox";
            this.controlTextBox.Size = new System.Drawing.Size(145, 20);
            this.controlTextBox.TabIndex = 7;
            // 
            // controlLbl
            // 
            this.controlLbl.AutoSize = true;
            this.controlLbl.Location = new System.Drawing.Point(225, 65);
            this.controlLbl.Name = "controlLbl";
            this.controlLbl.Size = new System.Drawing.Size(40, 13);
            this.controlLbl.TabIndex = 6;
            this.controlLbl.Text = "Control";
            // 
            // eventTextBox
            // 
            this.eventTextBox.Location = new System.Drawing.Point(48, 62);
            this.eventTextBox.Name = "eventTextBox";
            this.eventTextBox.Size = new System.Drawing.Size(145, 20);
            this.eventTextBox.TabIndex = 5;
            // 
            // eventLbl
            // 
            this.eventLbl.AutoSize = true;
            this.eventLbl.Location = new System.Drawing.Point(7, 65);
            this.eventLbl.Name = "eventLbl";
            this.eventLbl.Size = new System.Drawing.Size(35, 13);
            this.eventLbl.TabIndex = 4;
            this.eventLbl.Text = "Event";
            // 
            // nextStateTextBox
            // 
            this.nextStateTextBox.Location = new System.Drawing.Point(271, 27);
            this.nextStateTextBox.Name = "nextStateTextBox";
            this.nextStateTextBox.Size = new System.Drawing.Size(145, 20);
            this.nextStateTextBox.TabIndex = 3;
            // 
            // nextStateLbl
            // 
            this.nextStateLbl.AutoSize = true;
            this.nextStateLbl.Location = new System.Drawing.Point(233, 30);
            this.nextStateLbl.Name = "nextStateLbl";
            this.nextStateLbl.Size = new System.Drawing.Size(32, 13);
            this.nextStateLbl.TabIndex = 2;
            this.nextStateLbl.Text = "State";
            // 
            // operationNameTextBox
            // 
            this.operationNameTextBox.Location = new System.Drawing.Point(48, 27);
            this.operationNameTextBox.Name = "operationNameTextBox";
            this.operationNameTextBox.Size = new System.Drawing.Size(145, 20);
            this.operationNameTextBox.TabIndex = 1;
            this.operationNameTextBox.TextChanged += new System.EventHandler(this.operationNameTextBox_TextChanged);
            // 
            // opNameLbl
            // 
            this.opNameLbl.AutoSize = true;
            this.opNameLbl.Location = new System.Drawing.Point(7, 30);
            this.opNameLbl.Name = "opNameLbl";
            this.opNameLbl.Size = new System.Drawing.Size(35, 13);
            this.opNameLbl.TabIndex = 0;
            this.opNameLbl.Text = "Name";
            // 
            // newOperationBtn
            // 
            this.newOperationBtn.Location = new System.Drawing.Point(15, 389);
            this.newOperationBtn.Name = "newOperationBtn";
            this.newOperationBtn.Size = new System.Drawing.Size(106, 23);
            this.newOperationBtn.TabIndex = 13;
            this.newOperationBtn.Text = "Clear ";
            this.newOperationBtn.UseVisualStyleBackColor = true;
            this.newOperationBtn.Click += new System.EventHandler(this.newOperationBtn_Click);
            // 
            // operationsListLbl
            // 
            this.operationsListLbl.AutoSize = true;
            this.operationsListLbl.Location = new System.Drawing.Point(12, 155);
            this.operationsListLbl.Name = "operationsListLbl";
            this.operationsListLbl.Size = new System.Drawing.Size(80, 13);
            this.operationsListLbl.TabIndex = 8;
            this.operationsListLbl.Text = "Operations List:";
            // 
            // operationNameListBox
            // 
            this.operationNameListBox.FormattingEnabled = true;
            this.operationNameListBox.Location = new System.Drawing.Point(15, 180);
            this.operationNameListBox.Name = "operationNameListBox";
            this.operationNameListBox.Size = new System.Drawing.Size(251, 56);
            this.operationNameListBox.TabIndex = 9;
            this.operationNameListBox.SelectedIndexChanged += new System.EventHandler(this.operationNameListBox_SelectedIndexChanged);
            // 
            // applyBtn
            // 
            this.applyBtn.Location = new System.Drawing.Point(312, 180);
            this.applyBtn.Name = "applyBtn";
            this.applyBtn.Size = new System.Drawing.Size(106, 23);
            this.applyBtn.TabIndex = 10;
            this.applyBtn.Text = "Add Operation";
            this.applyBtn.UseVisualStyleBackColor = true;
            this.applyBtn.Click += new System.EventHandler(this.applyBtn_Click);
            // 
            // exitBtn
            // 
            this.exitBtn.Location = new System.Drawing.Point(368, 389);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(75, 23);
            this.exitBtn.TabIndex = 14;
            this.exitBtn.Text = "Exit";
            this.exitBtn.UseVisualStyleBackColor = true;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // separatorLine1
            // 
            this.separatorLine1.Location = new System.Drawing.Point(15, 139);
            this.separatorLine1.Name = "separatorLine1";
            this.separatorLine1.Size = new System.Drawing.Size(428, 10);
            this.separatorLine1.TabIndex = 7;
            this.separatorLine1.TabStop = false;
            // 
            // objListListBox
            // 
            this.objListListBox.FormattingEnabled = true;
            this.objListListBox.Location = new System.Drawing.Point(284, 77);
            this.objListListBox.Name = "objListListBox";
            this.objListListBox.Size = new System.Drawing.Size(159, 56);
            this.objListListBox.TabIndex = 6;
            this.objListListBox.SelectedIndexChanged += new System.EventHandler(this.objListListBox_SelectedIndexChanged);
            // 
            // smObjectNameLbl
            // 
            this.smObjectNameLbl.AutoSize = true;
            this.smObjectNameLbl.Location = new System.Drawing.Point(15, 21);
            this.smObjectNameLbl.Name = "smObjectNameLbl";
            this.smObjectNameLbl.Size = new System.Drawing.Size(91, 13);
            this.smObjectNameLbl.TabIndex = 0;
            this.smObjectNameLbl.Text = "SM Object Name:";
            // 
            // smObjectNameTextBox
            // 
            this.smObjectNameTextBox.Location = new System.Drawing.Point(113, 18);
            this.smObjectNameTextBox.Name = "smObjectNameTextBox";
            this.smObjectNameTextBox.Size = new System.Drawing.Size(158, 20);
            this.smObjectNameTextBox.TabIndex = 1;
            this.smObjectNameTextBox.TextChanged += new System.EventHandler(this.smObjectNameTextBox_TextChanged);
            // 
            // SM_Obj_Attribute_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 433);
            this.Controls.Add(this.smObjectNameTextBox);
            this.Controls.Add(this.smObjectNameLbl);
            this.Controls.Add(this.objListListBox);
            this.Controls.Add(this.separatorLine1);
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.applyBtn);
            this.Controls.Add(this.removeObjBtn);
            this.Controls.Add(this.addObjBtn);
            this.Controls.Add(this.objListLbl);
            this.Controls.Add(this.objNameListBox);
            this.Controls.Add(this.deleteOperationBtn);
            this.Controls.Add(this.operationPropertiesGroupBox);
            this.Controls.Add(this.newOperationBtn);
            this.Controls.Add(this.operationsListLbl);
            this.Controls.Add(this.operationNameListBox);
            this.Name = "SM_Obj_Attribute_Form";
            this.Text = "SM Object Attributes Editor";
            this.Load += new System.EventHandler(this.SM_Obj_Attribute_Form_Load);
            this.operationPropertiesGroupBox.ResumeLayout(false);
            this.operationPropertiesGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button removeObjBtn;
        private System.Windows.Forms.Button addObjBtn;
        private System.Windows.Forms.Label objListLbl;
        private System.Windows.Forms.ListBox objNameListBox;
        private System.Windows.Forms.Button deleteOperationBtn;
        private System.Windows.Forms.GroupBox operationPropertiesGroupBox;
        private System.Windows.Forms.TextBox controlTextBox;
        private System.Windows.Forms.Label controlLbl;
        private System.Windows.Forms.TextBox eventTextBox;
        private System.Windows.Forms.Label eventLbl;
        private System.Windows.Forms.TextBox nextStateTextBox;
        private System.Windows.Forms.Label nextStateLbl;
        private System.Windows.Forms.TextBox operationNameTextBox;
        private System.Windows.Forms.Label opNameLbl;
        private System.Windows.Forms.Button newOperationBtn;
        private System.Windows.Forms.Label operationsListLbl;
        private System.Windows.Forms.ListBox operationNameListBox;
        private System.Windows.Forms.Button applyBtn;
        private System.Windows.Forms.Button exitBtn;
        private System.Windows.Forms.GroupBox separatorLine1;
        private System.Windows.Forms.ListBox objListListBox;
        private System.Windows.Forms.Label smObjectNameLbl;
        private System.Windows.Forms.TextBox smObjectNameTextBox;

    }
}