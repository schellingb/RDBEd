/*  RDBEd - Retro RDB & DAT Editor
 *  Copyright (C) 2020-2023 - Bernhard Schelling
 *
 *  RDBEd is free software: you can redistribute it and/or modify it under the terms
 *  of the GNU General Public License as published by the Free Software Found-
 *  ation, either version 3 of the License, or (at your option) any later version.
 *
 *  RDBEd is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
 *  PURPOSE.  See the GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License along with RDBEd.
 *  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace RDBEd
{
    public class MultiForm : Form
    {
        public MultiForm()
        {
            Application.SetDefaultFont(new Font(new FontFamily("Microsoft Sans Serif"), 8f));
            InitializeComponent();
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }

        public TextBox in1Path;
        public Button in1Button;
        public ComboBox mergeKey;
        public Button btnCancel;
        public Button btnOK;
        public Label in2Label;
        public Button in2Button;
        public TextBox in2Path;
        public Label outLabel;
        public TextBox outPath;
        public Button outButton;
        public Label in1Label;
        public TableLayoutPanel tableLayout;
        public Label lblInfo;
        public CheckBox option2Check;
        public CheckBox option1Check;
        private Label label2;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel7;
        private Panel panel6;
        private Panel panel5;
        private Label label3;

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
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.in1Label = new System.Windows.Forms.Label();
            this.in1Button = new System.Windows.Forms.Button();
            this.in1Path = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.in2Label = new System.Windows.Forms.Label();
            this.in2Button = new System.Windows.Forms.Button();
            this.in2Path = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.outLabel = new System.Windows.Forms.Label();
            this.outPath = new System.Windows.Forms.TextBox();
            this.outButton = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.mergeKey = new System.Windows.Forms.ComboBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.option2Check = new System.Windows.Forms.CheckBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.option1Check = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tableLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Merge Key:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.in1Label);
            this.panel1.Controls.Add(this.in1Button);
            this.panel1.Controls.Add(this.in1Path);
            this.panel1.Location = new System.Drawing.Point(5, 28);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(673, 26);
            this.panel1.TabIndex = 1;
            // 
            // in1Label
            // 
            this.in1Label.AutoSize = true;
            this.in1Label.Location = new System.Drawing.Point(0, 5);
            this.in1Label.Name = "in1Label";
            this.in1Label.Size = new System.Drawing.Size(26, 13);
            this.in1Label.TabIndex = 0;
            this.in1Label.Text = "File:";
            // 
            // in1Button
            // 
            this.in1Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.in1Button.Location = new System.Drawing.Point(648, 0);
            this.in1Button.Name = "in1Button";
            this.in1Button.Size = new System.Drawing.Size(26, 22);
            this.in1Button.TabIndex = 2;
            this.in1Button.Text = "...";
            this.in1Button.UseVisualStyleBackColor = true;
            // 
            // in1Path
            // 
            this.in1Path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.in1Path.Location = new System.Drawing.Point(66, 1);
            this.in1Path.Name = "in1Path";
            this.in1Path.Size = new System.Drawing.Size(576, 20);
            this.in1Path.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.in2Label);
            this.panel2.Controls.Add(this.in2Button);
            this.panel2.Controls.Add(this.in2Path);
            this.panel2.Location = new System.Drawing.Point(5, 54);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(673, 26);
            this.panel2.TabIndex = 2;
            // 
            // in2Label
            // 
            this.in2Label.AutoSize = true;
            this.in2Label.Location = new System.Drawing.Point(0, 5);
            this.in2Label.Name = "in2Label";
            this.in2Label.Size = new System.Drawing.Size(26, 13);
            this.in2Label.TabIndex = 0;
            this.in2Label.Text = "File:";
            // 
            // in2Button
            // 
            this.in2Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.in2Button.Location = new System.Drawing.Point(648, 0);
            this.in2Button.Name = "in2Button";
            this.in2Button.Size = new System.Drawing.Size(26, 22);
            this.in2Button.TabIndex = 4;
            this.in2Button.Text = "...";
            this.in2Button.UseVisualStyleBackColor = true;
            // 
            // in2Path
            // 
            this.in2Path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.in2Path.Location = new System.Drawing.Point(67, 1);
            this.in2Path.Name = "in2Path";
            this.in2Path.Size = new System.Drawing.Size(575, 20);
            this.in2Path.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.outLabel);
            this.panel3.Controls.Add(this.outPath);
            this.panel3.Controls.Add(this.outButton);
            this.panel3.Location = new System.Drawing.Point(5, 80);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(673, 26);
            this.panel3.TabIndex = 3;
            // 
            // outLabel
            // 
            this.outLabel.AutoSize = true;
            this.outLabel.Location = new System.Drawing.Point(0, 5);
            this.outLabel.Name = "outLabel";
            this.outLabel.Size = new System.Drawing.Size(61, 13);
            this.outLabel.TabIndex = 0;
            this.outLabel.Text = "Output File:";
            // 
            // outPath
            // 
            this.outPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outPath.Location = new System.Drawing.Point(67, 1);
            this.outPath.Name = "outPath";
            this.outPath.Size = new System.Drawing.Size(575, 20);
            this.outPath.TabIndex = 5;
            // 
            // outButton
            // 
            this.outButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.outButton.Location = new System.Drawing.Point(648, 0);
            this.outButton.Name = "outButton";
            this.outButton.Size = new System.Drawing.Size(26, 22);
            this.outButton.TabIndex = 6;
            this.outButton.Text = "...";
            this.outButton.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.Controls.Add(this.mergeKey);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Location = new System.Drawing.Point(5, 106);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(673, 26);
            this.panel4.TabIndex = 4;
            // 
            // mergeKey
            // 
            this.mergeKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mergeKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mergeKey.FormattingEnabled = true;
            this.mergeKey.Location = new System.Drawing.Point(67, 1);
            this.mergeKey.Name = "mergeKey";
            this.mergeKey.Size = new System.Drawing.Size(606, 21);
            this.mergeKey.TabIndex = 7;
            this.mergeKey.ValueMember = "ColId";
            // 
            // panel7
            // 
            this.panel7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel7.Controls.Add(this.btnCancel);
            this.panel7.Controls.Add(this.btnOK);
            this.panel7.Location = new System.Drawing.Point(5, 184);
            this.panel7.Margin = new System.Windows.Forms.Padding(0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(673, 26);
            this.panel7.TabIndex = 7;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(595, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(514, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel6.Controls.Add(this.option2Check);
            this.panel6.Location = new System.Drawing.Point(5, 158);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(673, 26);
            this.panel6.TabIndex = 6;
            // 
            // option2Check
            // 
            this.option2Check.AutoSize = true;
            this.option2Check.Location = new System.Drawing.Point(67, 4);
            this.option2Check.Name = "option2Check";
            this.option2Check.Size = new System.Drawing.Size(57, 17);
            this.option2Check.TabIndex = 9;
            this.option2Check.Text = "Option";
            this.option2Check.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.Controls.Add(this.option1Check);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Location = new System.Drawing.Point(5, 132);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(673, 26);
            this.panel5.TabIndex = 5;
            // 
            // option1Check
            // 
            this.option1Check.AutoSize = true;
            this.option1Check.Location = new System.Drawing.Point(67, 4);
            this.option1Check.Name = "option1Check";
            this.option1Check.Size = new System.Drawing.Size(57, 17);
            this.option1Check.TabIndex = 8;
            this.option1Check.Text = "Option";
            this.option1Check.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Options:";
            // 
            // tableLayout
            // 
            this.tableLayout.AutoSize = true;
            this.tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayout.ColumnCount = 1;
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout.Controls.Add(this.lblInfo, 0, 0);
            this.tableLayout.Controls.Add(this.panel1, 0, 1);
            this.tableLayout.Controls.Add(this.panel2, 0, 2);
            this.tableLayout.Controls.Add(this.panel3, 0, 3);
            this.tableLayout.Controls.Add(this.panel4, 0, 4);
            this.tableLayout.Controls.Add(this.panel5, 0, 5);
            this.tableLayout.Controls.Add(this.panel6, 0, 6);
            this.tableLayout.Controls.Add(this.panel7, 0, 7);
            this.tableLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayout.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayout.Location = new System.Drawing.Point(0, 0);
            this.tableLayout.Name = "tableLayout";
            this.tableLayout.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayout.RowCount = 8;
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayout.Size = new System.Drawing.Size(683, 215);
            this.tableLayout.TabIndex = 0;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(8, 5);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(25, 13);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Info";
            // 
            // MultiForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(683, 215);
            this.Controls.Add(this.tableLayout);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(5000, 254);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(192, 254);
            this.Name = "MultiForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Title";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.tableLayout.ResumeLayout(false);
            this.tableLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
