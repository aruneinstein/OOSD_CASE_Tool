namespace OOSD_CASE_Tool
{
    partial class C_Obj_Attribute_Form
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
            this.applyBtn = new System.Windows.Forms.Button();
            this.exitBtn = new System.Windows.Forms.Button();
            this.cObjectNameLbl = new System.Windows.Forms.Label();
            this.cObjectNameText = new System.Windows.Forms.TextBox();
            this.attributeListLbl = new System.Windows.Forms.Label();
            this.attributeListBox = new System.Windows.Forms.ListBox();
            this.newAttributeBtn = new System.Windows.Forms.Button();
            this.delAttributeBtn = new System.Windows.Forms.Button();
            this.attributePropertyLbl = new System.Windows.Forms.Label();
            this.attributeNameLbl = new System.Windows.Forms.Label();
            this.attributeDiscriptionLbl = new System.Windows.Forms.Label();
            this.attributeDomainLbl = new System.Windows.Forms.Label();
            this.attributeDiscriptionText = new System.Windows.Forms.TextBox();
            this.attributeDomainText = new System.Windows.Forms.TextBox();
            this.attributeNameText = new System.Windows.Forms.TextBox();
            this.attributePropGrpBox = new System.Windows.Forms.GroupBox();
            this.attributePropGrpBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // applyBtn
            // 
            this.applyBtn.Location = new System.Drawing.Point(9, 126);
            this.applyBtn.Name = "applyBtn";
            this.applyBtn.Size = new System.Drawing.Size(101, 31);
            this.applyBtn.TabIndex = 0;
            this.applyBtn.Text = "Add / Save";
            this.applyBtn.UseVisualStyleBackColor = true;
            this.applyBtn.Click += new System.EventHandler(this.applyBtn_Click);
            // 
            // exitBtn
            // 
            this.exitBtn.Location = new System.Drawing.Point(371, 393);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(105, 28);
            this.exitBtn.TabIndex = 1;
            this.exitBtn.Text = "Exit";
            this.exitBtn.UseVisualStyleBackColor = true;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // cObjectNameLbl
            // 
            this.cObjectNameLbl.AutoSize = true;
            this.cObjectNameLbl.Location = new System.Drawing.Point(9, 24);
            this.cObjectNameLbl.Name = "cObjectNameLbl";
            this.cObjectNameLbl.Size = new System.Drawing.Size(82, 13);
            this.cObjectNameLbl.TabIndex = 2;
            this.cObjectNameLbl.Text = "C Object Name:";
            this.cObjectNameLbl.Click += new System.EventHandler(this.label1_Click);
            // 
            // cObjectNameText
            // 
            this.cObjectNameText.Location = new System.Drawing.Point(97, 24);
            this.cObjectNameText.Name = "cObjectNameText";
            this.cObjectNameText.Size = new System.Drawing.Size(223, 20);
            this.cObjectNameText.TabIndex = 3;
            this.cObjectNameText.TextChanged += new System.EventHandler(this.cObjectNameText_TextChanged);
            // 
            // attributeListLbl
            // 
            this.attributeListLbl.AutoSize = true;
            this.attributeListLbl.Location = new System.Drawing.Point(9, 66);
            this.attributeListLbl.Name = "attributeListLbl";
            this.attributeListLbl.Size = new System.Drawing.Size(68, 13);
            this.attributeListLbl.TabIndex = 4;
            this.attributeListLbl.Text = "Attribute List:";
            // 
            // attributeListBox
            // 
            this.attributeListBox.FormattingEnabled = true;
            this.attributeListBox.HorizontalScrollbar = true;
            this.attributeListBox.Location = new System.Drawing.Point(12, 82);
            this.attributeListBox.Name = "attributeListBox";
            this.attributeListBox.Size = new System.Drawing.Size(311, 108);
            this.attributeListBox.TabIndex = 5;
            this.attributeListBox.SelectedIndexChanged += new System.EventHandler(this.attributeListBox_SelectedIndexChanged);
            // 
            // newAttributeBtn
            // 
            this.newAttributeBtn.Location = new System.Drawing.Point(358, 126);
            this.newAttributeBtn.Name = "newAttributeBtn";
            this.newAttributeBtn.Size = new System.Drawing.Size(97, 31);
            this.newAttributeBtn.TabIndex = 6;
            this.newAttributeBtn.Text = "Clear Form";
            this.newAttributeBtn.UseVisualStyleBackColor = true;
            this.newAttributeBtn.Click += new System.EventHandler(this.newAttributeBtn_Click);
            // 
            // delAttributeBtn
            // 
            this.delAttributeBtn.Location = new System.Drawing.Point(379, 82);
            this.delAttributeBtn.Name = "delAttributeBtn";
            this.delAttributeBtn.Size = new System.Drawing.Size(97, 23);
            this.delAttributeBtn.TabIndex = 7;
            this.delAttributeBtn.Text = "Delete Attribute";
            this.delAttributeBtn.UseVisualStyleBackColor = true;
            this.delAttributeBtn.Click += new System.EventHandler(this.delAttributeBtn_Click);
            // 
            // attributePropertyLbl
            // 
            this.attributePropertyLbl.AutoSize = true;
            this.attributePropertyLbl.Location = new System.Drawing.Point(10, 16);
            this.attributePropertyLbl.Name = "attributePropertyLbl";
            this.attributePropertyLbl.Size = new System.Drawing.Size(0, 13);
            this.attributePropertyLbl.TabIndex = 8;
            // 
            // attributeNameLbl
            // 
            this.attributeNameLbl.AutoSize = true;
            this.attributeNameLbl.Location = new System.Drawing.Point(6, 32);
            this.attributeNameLbl.Name = "attributeNameLbl";
            this.attributeNameLbl.Size = new System.Drawing.Size(38, 13);
            this.attributeNameLbl.TabIndex = 9;
            this.attributeNameLbl.Text = "Name:";
            // 
            // attributeDiscriptionLbl
            // 
            this.attributeDiscriptionLbl.AutoSize = true;
            this.attributeDiscriptionLbl.Location = new System.Drawing.Point(6, 74);
            this.attributeDiscriptionLbl.Name = "attributeDiscriptionLbl";
            this.attributeDiscriptionLbl.Size = new System.Drawing.Size(59, 13);
            this.attributeDiscriptionLbl.TabIndex = 10;
            this.attributeDiscriptionLbl.Text = "Discription:";
            // 
            // attributeDomainLbl
            // 
            this.attributeDomainLbl.AutoSize = true;
            this.attributeDomainLbl.Location = new System.Drawing.Point(255, 32);
            this.attributeDomainLbl.Name = "attributeDomainLbl";
            this.attributeDomainLbl.Size = new System.Drawing.Size(46, 13);
            this.attributeDomainLbl.TabIndex = 11;
            this.attributeDomainLbl.Text = "Domain:";
            // 
            // attributeDiscriptionText
            // 
            this.attributeDiscriptionText.Location = new System.Drawing.Point(66, 71);
            this.attributeDiscriptionText.Multiline = true;
            this.attributeDiscriptionText.Name = "attributeDiscriptionText";
            this.attributeDiscriptionText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.attributeDiscriptionText.Size = new System.Drawing.Size(389, 38);
            this.attributeDiscriptionText.TabIndex = 12;
            // 
            // attributeDomainText
            // 
            this.attributeDomainText.Location = new System.Drawing.Point(307, 29);
            this.attributeDomainText.Name = "attributeDomainText";
            this.attributeDomainText.Size = new System.Drawing.Size(148, 20);
            this.attributeDomainText.TabIndex = 13;
            // 
            // attributeNameText
            // 
            this.attributeNameText.Location = new System.Drawing.Point(66, 29);
            this.attributeNameText.Name = "attributeNameText";
            this.attributeNameText.Size = new System.Drawing.Size(166, 20);
            this.attributeNameText.TabIndex = 14;
            // 
            // attributePropGrpBox
            // 
            this.attributePropGrpBox.Controls.Add(this.attributePropertyLbl);
            this.attributePropGrpBox.Controls.Add(this.attributeDiscriptionText);
            this.attributePropGrpBox.Controls.Add(this.attributeDomainText);
            this.attributePropGrpBox.Controls.Add(this.attributeDiscriptionLbl);
            this.attributePropGrpBox.Controls.Add(this.attributeNameText);
            this.attributePropGrpBox.Controls.Add(this.newAttributeBtn);
            this.attributePropGrpBox.Controls.Add(this.attributeNameLbl);
            this.attributePropGrpBox.Controls.Add(this.applyBtn);
            this.attributePropGrpBox.Controls.Add(this.attributeDomainLbl);
            this.attributePropGrpBox.Location = new System.Drawing.Point(13, 215);
            this.attributePropGrpBox.Name = "attributePropGrpBox";
            this.attributePropGrpBox.Size = new System.Drawing.Size(463, 163);
            this.attributePropGrpBox.TabIndex = 15;
            this.attributePropGrpBox.TabStop = false;
            this.attributePropGrpBox.Text = "Attribute Property";
            this.attributePropGrpBox.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // C_Obj_Attribute_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 439);
            this.Controls.Add(this.attributePropGrpBox);
            this.Controls.Add(this.delAttributeBtn);
            this.Controls.Add(this.attributeListBox);
            this.Controls.Add(this.attributeListLbl);
            this.Controls.Add(this.cObjectNameText);
            this.Controls.Add(this.cObjectNameLbl);
            this.Controls.Add(this.exitBtn);
            this.Name = "C_Obj_Attribute_Form";
            this.Text = "C Object Attributes Editor";
            this.Load += new System.EventHandler(this.C_Obj_Attribute_Form_Load);
            this.attributePropGrpBox.ResumeLayout(false);
            this.attributePropGrpBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button applyBtn;
        private System.Windows.Forms.Button exitBtn;
        private System.Windows.Forms.Label cObjectNameLbl;
        private System.Windows.Forms.TextBox cObjectNameText;
        private System.Windows.Forms.Label attributeListLbl;
        private System.Windows.Forms.ListBox attributeListBox;
        private System.Windows.Forms.Button newAttributeBtn;
        private System.Windows.Forms.Button delAttributeBtn;
        private System.Windows.Forms.Label attributePropertyLbl;
        private System.Windows.Forms.Label attributeNameLbl;
        private System.Windows.Forms.Label attributeDiscriptionLbl;
        private System.Windows.Forms.Label attributeDomainLbl;
        private System.Windows.Forms.TextBox attributeDiscriptionText;
        private System.Windows.Forms.TextBox attributeDomainText;
        private System.Windows.Forms.TextBox attributeNameText;
        private System.Windows.Forms.GroupBox attributePropGrpBox;

    }
}