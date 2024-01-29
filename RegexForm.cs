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

using System.Windows.Forms;

namespace RDBEd
{
    public class RegexForm : Form
    {
        public RegexForm()
        {
            InitializeComponent();
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }

        public TextBox txtRegex;
        public ComboBox cmbField;
        public Button btnCancel;
        public Button btnOK;
        public Label outLabel;
        private Label label2;
        public ComboBox cmbPreset;
        private Label label1;
        public CheckBox chkNegate;

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
            this.txtRegex = new System.Windows.Forms.TextBox();
            this.outLabel = new System.Windows.Forms.Label();
            this.cmbField = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cmbPreset = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkNegate = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Field:";
            // 
            // txtRegex
            // 
            this.txtRegex.AcceptsReturn = true;
            this.txtRegex.AcceptsTab = true;
            this.txtRegex.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRegex.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRegex.Location = new System.Drawing.Point(72, 59);
            this.txtRegex.Multiline = true;
            this.txtRegex.Name = "txtRegex";
            this.txtRegex.Size = new System.Drawing.Size(815, 190);
            this.txtRegex.TabIndex = 3;
            this.txtRegex.WordWrap = false;
            // 
            // outLabel
            // 
            this.outLabel.AutoSize = true;
            this.outLabel.Location = new System.Drawing.Point(5, 62);
            this.outLabel.Name = "outLabel";
            this.outLabel.Size = new System.Drawing.Size(61, 26);
            this.outLabel.TabIndex = 0;
            this.outLabel.Text = "Regular\r\nExpression:";
            // 
            // cmbField
            // 
            this.cmbField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbField.FormattingEnabled = true;
            this.cmbField.Location = new System.Drawing.Point(72, 32);
            this.cmbField.Name = "cmbField";
            this.cmbField.Size = new System.Drawing.Size(815, 21);
            this.cmbField.TabIndex = 2;
            this.cmbField.ValueMember = "ColId";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(812, 255);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(731, 255);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // cmbPreset
            // 
            this.cmbPreset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPreset.FormattingEnabled = true;
            this.cmbPreset.Location = new System.Drawing.Point(72, 5);
            this.cmbPreset.Name = "cmbPreset";
            this.cmbPreset.Size = new System.Drawing.Size(815, 21);
            this.cmbPreset.TabIndex = 1;
            this.cmbPreset.ValueMember = "ColId";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Preset:";
            // 
            // chkNegate
            // 
            this.chkNegate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkNegate.AutoSize = true;
            this.chkNegate.Location = new System.Drawing.Point(631, 259);
            this.chkNegate.Name = "chkNegate";
            this.chkNegate.Size = new System.Drawing.Size(94, 17);
            this.chkNegate.TabIndex = 14;
            this.chkNegate.Text = "Negate Match";
            this.chkNegate.UseVisualStyleBackColor = true;
            // 
            // RegexForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(892, 286);
            this.Controls.Add(this.chkNegate);
            this.Controls.Add(this.cmbPreset);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outLabel);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cmbField);
            this.Controls.Add(this.txtRegex);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(180, 160);
            this.Name = "RegexForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Validate Field";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
