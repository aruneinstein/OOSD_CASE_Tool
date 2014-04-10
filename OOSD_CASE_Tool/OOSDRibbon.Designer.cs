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
            this.Object = this.Factory.CreateRibbonGroup();
            this.openObjStencilBtn = this.Factory.CreateRibbonButton();
            this.oosdTab.SuspendLayout();
            this.Object.SuspendLayout();
            // 
            // oosdTab
            // 
            this.oosdTab.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.oosdTab.Groups.Add(this.Object);
            this.oosdTab.Label = "OOSD CASE TOOL";
            this.oosdTab.Name = "oosdTab";
            // 
            // Object
            // 
            this.Object.Items.Add(this.openObjStencilBtn);
            this.Object.Label = "Object Editor";
            this.Object.Name = "Object";
            // 
            // openObjStencilBtn
            // 
            this.openObjStencilBtn.Label = "Open Stencil";
            this.openObjStencilBtn.Name = "openObjStencilBtn";
            this.openObjStencilBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
            // 
            // OOSDRibbon
            // 
            this.Name = "OOSDRibbon";
            this.RibbonType = "Microsoft.Visio.Drawing";
            this.Tabs.Add(this.oosdTab);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.OOSDRibbon_Load);
            this.oosdTab.ResumeLayout(false);
            this.oosdTab.PerformLayout();
            this.Object.ResumeLayout(false);
            this.Object.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab oosdTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup Object;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton openObjStencilBtn;
    }

    partial class ThisRibbonCollection
    {
        internal OOSDRibbon Ribbon1
        {
            get { return this.GetRibbon<OOSDRibbon>(); }
        }
    }
}
