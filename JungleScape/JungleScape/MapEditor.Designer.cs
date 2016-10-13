namespace LevelEditor
{
    partial class MapEditor
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
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.instructionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.setbutton = new System.Windows.Forms.Button();
            this.TileSize = new System.Windows.Forms.Label();
            this.Width = new System.Windows.Forms.Label();
            this.Height = new System.Windows.Forms.Label();
            this.Layer = new System.Windows.Forms.Label();
            this.zValue = new System.Windows.Forms.Label();
            this.WidthPicker = new System.Windows.Forms.NumericUpDown();
            this.HeightPicker = new System.Windows.Forms.NumericUpDown();
            this.SizePicker = new System.Windows.Forms.NumericUpDown();
            this.zValuePicker = new System.Windows.Forms.NumericUpDown();
            this.LayerPicker = new System.Windows.Forms.ComboBox();
            this.isVisible = new System.Windows.Forms.CheckBox();
            this.isSolid = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WidthPicker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightPicker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizePicker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zValuePicker)).BeginInit();
            this.SuspendLayout();
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 22);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.instructionsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // instructionsToolStripMenuItem
            // 
            this.instructionsToolStripMenuItem.Name = "instructionsToolStripMenuItem";
            this.instructionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.instructionsToolStripMenuItem.Text = "Instructions";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(853, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.comboBox1);
            this.splitContainer1.Panel1.Controls.Add(this.isSolid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.isVisible);
            this.splitContainer1.Panel2.Controls.Add(this.LayerPicker);
            this.splitContainer1.Panel2.Controls.Add(this.zValuePicker);
            this.splitContainer1.Panel2.Controls.Add(this.SizePicker);
            this.splitContainer1.Panel2.Controls.Add(this.HeightPicker);
            this.splitContainer1.Panel2.Controls.Add(this.WidthPicker);
            this.splitContainer1.Panel2.Controls.Add(this.zValue);
            this.splitContainer1.Panel2.Controls.Add(this.Layer);
            this.splitContainer1.Panel2.Controls.Add(this.setbutton);
            this.splitContainer1.Panel2.Controls.Add(this.TileSize);
            this.splitContainer1.Panel2.Controls.Add(this.Width);
            this.splitContainer1.Panel2.Controls.Add(this.Height);
            this.splitContainer1.Size = new System.Drawing.Size(853, 655);
            this.splitContainer1.SplitterDistance = 283;
            this.splitContainer1.TabIndex = 1;
            // 
            // setbutton
            // 
            this.setbutton.Location = new System.Drawing.Point(310, 44);
            this.setbutton.Name = "setbutton";
            this.setbutton.Size = new System.Drawing.Size(105, 23);
            this.setbutton.TabIndex = 3;
            this.setbutton.Text = "Set";
            this.setbutton.UseVisualStyleBackColor = true;
            // 
            // TileSize
            // 
            this.TileSize.AutoSize = true;
            this.TileSize.Location = new System.Drawing.Point(432, 23);
            this.TileSize.Name = "TileSize";
            this.TileSize.Size = new System.Drawing.Size(47, 13);
            this.TileSize.TabIndex = 2;
            this.TileSize.Text = "Tile Size";
            // 
            // Width
            // 
            this.Width.AutoSize = true;
            this.Width.Location = new System.Drawing.Point(22, 23);
            this.Width.Name = "Width";
            this.Width.Size = new System.Drawing.Size(35, 13);
            this.Width.TabIndex = 1;
            this.Width.Text = "Width";
            // 
            // Height
            // 
            this.Height.AutoSize = true;
            this.Height.Location = new System.Drawing.Point(164, 23);
            this.Height.Name = "Height";
            this.Height.Size = new System.Drawing.Size(38, 13);
            this.Height.TabIndex = 0;
            this.Height.Text = "Height";
            this.Height.Click += new System.EventHandler(this.Height_Click);
            // 
            // Layer
            // 
            this.Layer.AutoSize = true;
            this.Layer.Location = new System.Drawing.Point(22, 98);
            this.Layer.Name = "Layer";
            this.Layer.Size = new System.Drawing.Size(33, 13);
            this.Layer.TabIndex = 4;
            this.Layer.Text = "Layer";
            // 
            // zValue
            // 
            this.zValue.AutoSize = true;
            this.zValue.Location = new System.Drawing.Point(307, 98);
            this.zValue.Name = "zValue";
            this.zValue.Size = new System.Drawing.Size(44, 13);
            this.zValue.TabIndex = 5;
            this.zValue.Text = "Z-Value";
            // 
            // WidthPicker
            // 
            this.WidthPicker.Location = new System.Drawing.Point(25, 47);
            this.WidthPicker.Name = "WidthPicker";
            this.WidthPicker.Size = new System.Drawing.Size(105, 20);
            this.WidthPicker.TabIndex = 6;
            this.WidthPicker.ValueChanged += new System.EventHandler(this.WidthPicker_ValueChanged);
            // 
            // HeightPicker
            // 
            this.HeightPicker.Location = new System.Drawing.Point(167, 47);
            this.HeightPicker.Name = "HeightPicker";
            this.HeightPicker.Size = new System.Drawing.Size(105, 20);
            this.HeightPicker.TabIndex = 7;
            this.HeightPicker.ValueChanged += new System.EventHandler(this.HeightPicker_ValueChanged);
            // 
            // SizePicker
            // 
            this.SizePicker.Location = new System.Drawing.Point(435, 44);
            this.SizePicker.Name = "SizePicker";
            this.SizePicker.Size = new System.Drawing.Size(105, 20);
            this.SizePicker.TabIndex = 8;
            this.SizePicker.ValueChanged += new System.EventHandler(this.SizePicker_ValueChanged);
            // 
            // zValuePicker
            // 
            this.zValuePicker.Location = new System.Drawing.Point(310, 114);
            this.zValuePicker.Name = "zValuePicker";
            this.zValuePicker.Size = new System.Drawing.Size(105, 20);
            this.zValuePicker.TabIndex = 9;
            this.zValuePicker.ValueChanged += new System.EventHandler(this.zValuePicker_ValueChanged);
            // 
            // LayerPicker
            // 
            this.LayerPicker.FormattingEnabled = true;
            this.LayerPicker.Location = new System.Drawing.Point(25, 114);
            this.LayerPicker.Name = "LayerPicker";
            this.LayerPicker.Size = new System.Drawing.Size(247, 21);
            this.LayerPicker.TabIndex = 10;
            this.LayerPicker.SelectedIndexChanged += new System.EventHandler(this.LayerPicker_SelectedIndexChanged);
            // 
            // isVisible
            // 
            this.isVisible.AutoSize = true;
            this.isVisible.Location = new System.Drawing.Point(435, 114);
            this.isVisible.Name = "isVisible";
            this.isVisible.Size = new System.Drawing.Size(56, 17);
            this.isVisible.TabIndex = 11;
            this.isVisible.Text = "Visible";
            this.isVisible.UseVisualStyleBackColor = true;
            this.isVisible.CheckedChanged += new System.EventHandler(this.isVisible_CheckedChanged);
            // 
            // isSolid
            // 
            this.isSolid.AutoSize = true;
            this.isSolid.Location = new System.Drawing.Point(211, 19);
            this.isSolid.Name = "isSolid";
            this.isSolid.Size = new System.Drawing.Size(49, 17);
            this.isSolid.TabIndex = 12;
            this.isSolid.Text = "Solid";
            this.isSolid.UseVisualStyleBackColor = true;
            this.isSolid.CheckedChanged += new System.EventHandler(this.isSolid_CheckedChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 20);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(178, 21);
            this.comboBox1.TabIndex = 12;
            // 
            // MapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 679);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MapEditor";
            this.Text = "MapEditor";
            this.Load += new System.EventHandler(this.MapEditor_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WidthPicker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightPicker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizePicker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zValuePicker)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem instructionsToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label TileSize;
        private System.Windows.Forms.Label Width;
        private System.Windows.Forms.Label Height;
        private System.Windows.Forms.Button setbutton;
        private System.Windows.Forms.Label zValue;
        private System.Windows.Forms.Label Layer;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox isSolid;
        private System.Windows.Forms.CheckBox isVisible;
        private System.Windows.Forms.ComboBox LayerPicker;
        private System.Windows.Forms.NumericUpDown zValuePicker;
        private System.Windows.Forms.NumericUpDown SizePicker;
        private System.Windows.Forms.NumericUpDown HeightPicker;
        private System.Windows.Forms.NumericUpDown WidthPicker;
    }
}