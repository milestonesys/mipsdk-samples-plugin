using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Property.Client
{
    partial class PropertyViewItemUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyViewItemUserControl));
            this.contextMenuStripcopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.textBoxSharedUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxShareGlobal = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxPropValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStripcopy
            // 
            this.contextMenuStripcopy.Name = "contextMenuStripcopy";
            resources.ApplyResources(this.contextMenuStripcopy, "contextMenuStripcopy");
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.label1.Click += new System.EventHandler(this.OnClick);
            this.label1.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // labelName
            // 
            resources.ApplyResources(this.labelName, "labelName");
            this.labelName.Name = "labelName";
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.Silver;
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Controls.Add(this.labelName);
            resources.ApplyResources(this.panelHeader, "panelHeader");
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseClick);
            this.panelHeader.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseDoubleClick);
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.textBoxSharedUser);
            this.panelMain.Controls.Add(this.label2);
            this.panelMain.Controls.Add(this.textBoxShareGlobal);
            this.panelMain.Controls.Add(this.label4);
            this.panelMain.Controls.Add(this.textBoxPropValue);
            this.panelMain.Controls.Add(this.label3);
            resources.ApplyResources(this.panelMain, "panelMain");
            this.panelMain.Name = "panelMain";
            this.panelMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseClick);
            this.panelMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseDoubleClick);
            // 
            // textBoxSharedUser
            // 
            resources.ApplyResources(this.textBoxSharedUser, "textBoxSharedUser");
            this.textBoxSharedUser.Name = "textBoxSharedUser";
            this.textBoxSharedUser.TextChanged += new System.EventHandler(this.textBoxSharedUser_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.label2.Click += new System.EventHandler(this.OnClick);
            this.label2.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // textBoxShareGlobal
            // 
            resources.ApplyResources(this.textBoxShareGlobal, "textBoxShareGlobal");
            this.textBoxShareGlobal.Name = "textBoxShareGlobal";
            this.textBoxShareGlobal.TextChanged += new System.EventHandler(this.textBoxShareGlobal_TextChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            this.label4.Click += new System.EventHandler(this.OnClick);
            this.label4.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // textBoxPropValue
            // 
            resources.ApplyResources(this.textBoxPropValue, "textBoxPropValue");
            this.textBoxPropValue.Name = "textBoxPropValue";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.label3.Click += new System.EventHandler(this.OnClick);
            this.label3.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // PropertyViewItemUserControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DarkKhaki;
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelHeader);
            resources.ApplyResources(this, "$this");
            this.Name = "PropertyViewItemUserControl";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseDoubleClick);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal ContextMenuStrip contextMenuStripcopy;
        internal ToolStripMenuItem copyToolStripMenuItem;
        private Label label1;
        private Label labelName;
        private Panel panelHeader;
        private Panel panelMain;
        private TextBox textBoxPropValue;
        private Label label3;
        private TextBox textBoxSharedUser;
        private Label label2;
        private TextBox textBoxShareGlobal;
        private Label label4;

    }
}
