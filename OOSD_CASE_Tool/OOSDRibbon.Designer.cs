namespace OOSD_CASE_Tool
{
    partial class OOSDRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public OOSDRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.oosdTab = this.Factory.CreateRibbonTab();
            this.objectEditorGroup = this.Factory.CreateRibbonGroup();
            this.openObjStencilBtn = this.Factory.CreateRibbonButton();
            this.objToDictBtn = this.Factory.CreateRibbonButton();
            this.relationEditorGroup = this.Factory.CreateRibbonGroup();
            this.openRelationStencilBtn = this.Factory.CreateRibbonButton();
            this.erToObjHierBtn = this.Factory.CreateRibbonButton();
            this.button1 = this.Factory.CreateRibbonButton();
            this.flowEditorGroup = this.Factory.CreateRibbonGroup();
            this.openFlowStencilBtn = this.Factory.CreateRibbonButton();
            this.convertToArchChartBtn = this.Factory.CreateRibbonButton();
            this.oosdTab.SuspendLayout();
            this.objectEditorGroup.SuspendLayout();
            this.relationEditorGroup.SuspendLayout();
            this.flowEditorGroup.SuspendLayout();
            // 
            // oosdTab
            // 
            this.oosdTab.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.oosdTab.Groups.Add(this.objectEditorGroup);
            this.oosdTab.Groups.Add(this.relationEditorGroup);
            this.oosdTab.Groups.Add(this.flowEditorGroup);
            this.oosdTab.Label = "OOSD CASE TOOL";
            this.oosdTab.Name = "oosdTab";
            // 
            // objectEditorGroup
            // 
            this.objectEditorGroup.Items.Add(this.openObjStencilBtn);
            this.objectEditorGroup.Items.Add(this.objToDictBtn);
            this.objectEditorGroup.Label = "Object Editor";
            this.objectEditorGroup.Name = "objectEditorGroup";
            // 
            // openObjStencilBtn
            // 
            this.openObjStencilBtn.Image = global::OOSD_CASE_Tool.Properties.Resources.files;
            this.openObjStencilBtn.Label = "Open Stencil";
            this.openObjStencilBtn.Name = "openObjStencilBtn";
            this.openObjStencilBtn.ShowImage = true;
            this.openObjStencilBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.openObjStencilBtn_Click);
            // 
            // objToDictBtn
            // 
            this.objToDictBtn.Image = global::OOSD_CASE_Tool.Properties.Resources.play;
            this.objToDictBtn.Label = "Generate Data Dictionary";
            this.objToDictBtn.Name = "objToDictBtn";
            this.objToDictBtn.ShowImage = true;
            // 
            // relationEditorGroup
            // 
            this.relationEditorGroup.Items.Add(this.openRelationStencilBtn);
            this.relationEditorGroup.Items.Add(this.erToObjHierBtn);
            this.relationEditorGroup.Items.Add(this.button1);
            this.relationEditorGroup.Label = "Relation Editor";
            this.relationEditorGroup.Name = "relationEditorGroup";
            // 
            // openRelationStencilBtn
            // 
            this.openRelationStencilBtn.Image = global::OOSD_CASE_Tool.Properties.Resources.files;
            this.openRelationStencilBtn.Label = "Open Stencil";
            this.openRelationStencilBtn.Name = "openRelationStencilBtn";
            this.openRelationStencilBtn.ShowImage = true;
            this.openRelationStencilBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.openRelationStencilBtn_Click);
            // 
            // erToObjHierBtn
            // 
            this.erToObjHierBtn.Image = global::OOSD_CASE_Tool.Properties.Resources.play;
            this.erToObjHierBtn.Label = "Convert to Object Hierarchy";
            this.erToObjHierBtn.Name = "erToObjHierBtn";
            this.erToObjHierBtn.ShowImage = true;
            // 
            // button1
            // 
            this.button1.Image = global::OOSD_CASE_Tool.Properties.Resources.play;
            this.button1.Label = "Convert to Object Hierarchy";
            this.button1.Name = "button1";
            this.button1.ShowImage = true;
            // 
            // flowEditorGroup
            // 
            this.flowEditorGroup.Items.Add(this.openFlowStencilBtn);
            this.flowEditorGroup.Items.Add(this.convertToArchChartBtn);
            this.flowEditorGroup.Label = "Flow Editor";
            this.flowEditorGroup.Name = "flowEditorGroup";
            // 
            // openFlowStencilBtn
            // 
            this.openFlowStencilBtn.Image = global::OOSD_CASE_Tool.Properties.Resources.files;
            this.openFlowStencilBtn.Label = "Open Stencil";
            this.openFlowStencilBtn.Name = "openFlowStencilBtn";
            this.openFlowStencilBtn.ShowImage = true;
            this.openFlowStencilBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.openFlowStencilBtn_Click);
            // 
            // convertToArchChartBtn
            // 
            this.convertToArchChartBtn.Image = global::OOSD_CASE_Tool.Properties.Resources.play;
            this.convertToArchChartBtn.Label = "Convert to Chart";
            this.convertToArchChartBtn.Name = "convertToArchChartBtn";
            this.convertToArchChartBtn.ShowImage = true;
            this.convertToArchChartBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.convertToArchChartBtn_Click);
            // 
            // OOSDRibbon
            // 
            this.Name = "OOSDRibbon";
            this.RibbonType = "Microsoft.Visio.Drawing";
            this.Tabs.Add(this.oosdTab);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.OOSDRibbon_Load);
            this.oosdTab.ResumeLayout(false);
            this.oosdTab.PerformLayout();
            this.objectEditorGroup.ResumeLayout(false);
            this.objectEditorGroup.PerformLayout();
            this.relationEditorGroup.ResumeLayout(false);
            this.relationEditorGroup.PerformLayout();
            this.flowEditorGroup.ResumeLayout(false);
            this.flowEditorGroup.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab oosdTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup objectEditorGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton openObjStencilBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup relationEditorGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton openRelationStencilBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup flowEditorGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton openFlowStencilBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton convertToArchChartBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton objToDictBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton erToObjHierBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button1;
    }

    partial class ThisRibbonCollection
    {
        internal OOSDRibbon Ribbon1
        {
            get { return this.GetRibbon<OOSDRibbon>(); }
        }
    }
}
