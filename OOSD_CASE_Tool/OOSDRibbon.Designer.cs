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
            this.relationEditorGroup = this.Factory.CreateRibbonGroup();
            this.openRelationStencilBtn = this.Factory.CreateRibbonButton();
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
            this.objectEditorGroup.Label = "Object Editor";
            this.objectEditorGroup.Name = "objectEditorGroup";
            // 
            // openObjStencilBtn
            // 
            this.openObjStencilBtn.Label = "Open Stencil";
            this.openObjStencilBtn.Name = "openObjStencilBtn";
            this.openObjStencilBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.openObjStencilBtn_Click);
            // 
            // relationEditorGroup
            // 
            this.relationEditorGroup.Items.Add(this.openRelationStencilBtn);
            this.relationEditorGroup.Label = "Relation Editor";
            this.relationEditorGroup.Name = "relationEditorGroup";
            // 
            // openRelationStencilBtn
            // 
            this.openRelationStencilBtn.Label = "Open Stencil";
            this.openRelationStencilBtn.Name = "openRelationStencilBtn";
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
            this.openFlowStencilBtn.Label = "Open Stencil";
            this.openFlowStencilBtn.Name = "openFlowStencilBtn";
            // 
            // convertToArchChartBtn
            // 
            this.convertToArchChartBtn.Label = "Convert to Chart";
            this.convertToArchChartBtn.Name = "convertToArchChartBtn";
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
    }

    partial class ThisRibbonCollection
    {
        internal OOSDRibbon Ribbon1
        {
            get { return this.GetRibbon<OOSDRibbon>(); }
        }
    }
}
