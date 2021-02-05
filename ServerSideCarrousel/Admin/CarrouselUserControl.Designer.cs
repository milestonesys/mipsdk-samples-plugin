namespace ServerSideCarrousel.Admin
{
	partial class CarrouselUserControl
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
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxName = new System.Windows.Forms.TextBox();
			this.treeViewSelected = new System.Windows.Forms.TreeView();
			this.treeViewAvailable = new System.Windows.Forms.TreeView();
			this.buttonRemove = new System.Windows.Forms.Button();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.buttonUp = new System.Windows.Forms.Button();
			this.buttonDown = new System.Windows.Forms.Button();
			this.textBoxSeconds = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxDefaultSeconds = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name";
			// 
			// textBoxName
			// 
			this.textBoxName.Location = new System.Drawing.Point(84, 17);
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new System.Drawing.Size(279, 20);
			this.textBoxName.TabIndex = 1;
			this.textBoxName.TextChanged += new System.EventHandler(this.OnUserChange);
			// 
			// treeViewSelected
			// 
			this.treeViewSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.treeViewSelected.Location = new System.Drawing.Point(400, 75);
			this.treeViewSelected.Name = "treeViewSelected";
			this.treeViewSelected.ShowLines = false;
			this.treeViewSelected.ShowPlusMinus = false;
			this.treeViewSelected.ShowRootLines = false;
			this.treeViewSelected.Size = new System.Drawing.Size(288, 392);
			this.treeViewSelected.Sorted = true;
			this.treeViewSelected.TabIndex = 6;
			this.treeViewSelected.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterSelect);
			// 
			// treeViewAvailable
			// 
			this.treeViewAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.treeViewAvailable.Location = new System.Drawing.Point(25, 75);
			this.treeViewAvailable.Name = "treeViewAvailable";
			this.treeViewAvailable.Size = new System.Drawing.Size(286, 392);
			this.treeViewAvailable.TabIndex = 4;
			this.treeViewAvailable.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.OnBeforeExpand);
			this.treeViewAvailable.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeMouseClick);
			// 
			// buttonRemove
			// 
			this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRemove.Enabled = false;
			this.buttonRemove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.buttonRemove.Location = new System.Drawing.Point(317, 202);
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.Size = new System.Drawing.Size(75, 23);
			this.buttonRemove.TabIndex = 5;
			this.buttonRemove.Text = "Remove";
			this.buttonRemove.UseVisualStyleBackColor = true;
			this.buttonRemove.Click += new System.EventHandler(this.OnRemove);
			// 
			// buttonAdd
			// 
			this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAdd.Enabled = false;
			this.buttonAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.buttonAdd.Location = new System.Drawing.Point(317, 164);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(75, 23);
			this.buttonAdd.TabIndex = 3;
			this.buttonAdd.Text = "Add  ->";
			this.buttonAdd.UseVisualStyleBackColor = true;
			this.buttonAdd.Click += new System.EventHandler(this.OnAdd);
			// 
			// buttonUp
			// 
			this.buttonUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonUp.Enabled = false;
			this.buttonUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.buttonUp.Location = new System.Drawing.Point(552, 484);
			this.buttonUp.Name = "buttonUp";
			this.buttonUp.Size = new System.Drawing.Size(60, 23);
			this.buttonUp.TabIndex = 7;
			this.buttonUp.Text = "Up";
			this.buttonUp.UseVisualStyleBackColor = true;
			this.buttonUp.Click += new System.EventHandler(this.OnUp);
			// 
			// buttonDown
			// 
			this.buttonDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonDown.Enabled = false;
			this.buttonDown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.buttonDown.Location = new System.Drawing.Point(632, 484);
			this.buttonDown.Name = "buttonDown";
			this.buttonDown.Size = new System.Drawing.Size(56, 23);
			this.buttonDown.TabIndex = 8;
			this.buttonDown.Text = "Down";
			this.buttonDown.UseVisualStyleBackColor = true;
			this.buttonDown.Click += new System.EventHandler(this.OnDown);
			// 
			// textBoxSeconds
			// 
			this.textBoxSeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxSeconds.Location = new System.Drawing.Point(474, 486);
			this.textBoxSeconds.Name = "textBoxSeconds";
			this.textBoxSeconds.Size = new System.Drawing.Size(54, 20);
			this.textBoxSeconds.TabIndex = 9;
			this.textBoxSeconds.TextChanged += new System.EventHandler(this.OnSecondsChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(397, 489);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(52, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "Seconds:";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(32, 489);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(87, 13);
			this.label3.TabIndex = 12;
			this.label3.Text = "Default seconds:";
			// 
			// textBoxDefaultSeconds
			// 
			this.textBoxDefaultSeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textBoxDefaultSeconds.Location = new System.Drawing.Point(154, 487);
			this.textBoxDefaultSeconds.Name = "textBoxDefaultSeconds";
			this.textBoxDefaultSeconds.Size = new System.Drawing.Size(54, 20);
			this.textBoxDefaultSeconds.TabIndex = 11;
			this.textBoxDefaultSeconds.Text = "10";
			this.textBoxDefaultSeconds.TextChanged += new System.EventHandler(this.OnDefaultSecondsChanged);
			// 
			// CarrouselUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textBoxDefaultSeconds);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxSeconds);
			this.Controls.Add(this.buttonDown);
			this.Controls.Add(this.buttonUp);
			this.Controls.Add(this.treeViewSelected);
			this.Controls.Add(this.treeViewAvailable);
			this.Controls.Add(this.buttonRemove);
			this.Controls.Add(this.buttonAdd);
			this.Controls.Add(this.textBoxName);
			this.Controls.Add(this.label1);
			this.Name = "CarrouselUserControl";
			this.Size = new System.Drawing.Size(718, 534);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion


		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxName;
		private System.Windows.Forms.TreeView treeViewSelected;
		private System.Windows.Forms.TreeView treeViewAvailable;
		private System.Windows.Forms.Button buttonRemove;
		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.Button buttonUp;
		private System.Windows.Forms.Button buttonDown;
		private System.Windows.Forms.TextBox textBoxSeconds;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxDefaultSeconds;
	}
}
