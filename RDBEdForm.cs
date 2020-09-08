/*  RDBEd - Retro RDB & DAT Editor
 *  Copyright (C) 2020 - Bernhard Schelling
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
    class RDBEdForm : Form
    {
        public string OriginalText;
        public RDBEdForm()
        {
            InitializeComponent();
            this.OriginalText = this.Text;
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }

        public DataGridView gridMain;
        public WebBrowser webView;
        private MenuStrip menuStrip1;
        public ToolStripMenuItem menuOpen;
        public SplitContainer splitContainer;
        public TextBox txtFilter;
        private ToolStripSeparator toolStripSeparator1;
        public ToolStripMenuItem menuExit;
        public ToolStripMenuItem menuExportDAT;
        public ToolStripMenuItem menuExportRDB;
        private ToolStripSeparator toolStripSeparator3;
        public ToolStripMenuItem menuImport;
        public ToolStripMenuItem menuToolUnify;
        public ToolStripMenuItem menuToolDeltaDAT;
        public ToolStripStatusLabel statusChanges;
        public ToolStripStatusLabel statusWarnings;
        public ToolStripMenuItem menuSave;
        public ToolStripMenuItem menuExportModifications;
        public ToolStripMenuItem menuValidateField;
        public ToolStripStatusLabel statusChangesFiltered;
        public ToolStripStatusLabel statusWarningsFiltered;
        public ToolStripMenuItem menuValidateUnique;
        public ToolStripMenuItem menuNew;
        public ToolStripMenuItem menuToolGenerateTags;
        public ToolStripMenuItem menuAbout;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel3;
        private ToolStripStatusLabel toolStripStatusLabel2;
        public Button btnClearFilter;
        public ToolStripStatusLabel statusEntries;
        private ToolStripStatusLabel toolStripStatusLabel4;
        public ToolStripStatusLabel statusFiltered;
        public ToolStripMenuItem menuValidateClear;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem validateToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private Label label1;
        private StatusStrip statusStrip1;
        private ToolStripMenuItem editToolStripMenuItem;
        public ToolStripMenuItem menuEditCut;
        public ToolStripMenuItem menuEditCopy;
        public ToolStripMenuItem menuEditPaste;
        public ToolStripMenuItem menuEditDelete;
        public ToolStripMenuItem menuSaveAs;

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

        public Func<bool> OnCopy, OnPaste, OnDelete;
        public Func<bool, bool> OnCtrlUpDown;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData & (Keys.Control | Keys.Shift)) == Keys.Control)
            {
                if (keyData == (Keys.Control | Keys.Down  ) && OnCtrlUpDown(true)    ) return true;
                if (keyData == (Keys.Control | Keys.Up    ) && OnCtrlUpDown(false)   ) return true;
                if (keyData == (Keys.Control | Keys.C     ) && OnCopy()              ) return true;
                if (keyData == (Keys.Control | Keys.Insert) && OnCopy()              ) return true;
                if (keyData == (Keys.Control | Keys.V     ) && OnPaste()             ) return true;
                if (keyData == (Keys.Control | Keys.X     ) && OnCopy() && OnDelete()) return true;
            }
            if ((keyData & (Keys.Control | Keys.Shift)) == Keys.Shift)
            {
                if (keyData == (Keys.Shift   | Keys.Delete) && OnCopy() && OnDelete()) return true;
                if (keyData == (Keys.Shift   | Keys.Insert) && OnPaste()             ) return true;
            }
            else if (keyData == Keys.Delete && OnDelete()) return true;
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuImport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExportModifications = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExportDAT = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExportRDB = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuToolUnify = new System.Windows.Forms.ToolStripMenuItem();
            this.menuToolGenerateTags = new System.Windows.Forms.ToolStripMenuItem();
            this.menuToolDeltaDAT = new System.Windows.Forms.ToolStripMenuItem();
            this.validateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuValidateUnique = new System.Windows.Forms.ToolStripMenuItem();
            this.menuValidateField = new System.Windows.Forms.ToolStripMenuItem();
            this.menuValidateClear = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusEntries = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusFiltered = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusChanges = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusChangesFiltered = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusWarnings = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusWarningsFiltered = new System.Windows.Forms.ToolStripStatusLabel();
            this.webView = new System.Windows.Forms.WebBrowser();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.gridMain = new System.Windows.Forms.DataGridView();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMain)).BeginInit();
            this.SuspendLayout();
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNew,
            this.menuOpen,
            this.menuImport,
            this.toolStripSeparator1,
            this.menuSave,
            this.menuSaveAs,
            this.menuExportModifications,
            this.menuExportDAT,
            this.menuExportRDB,
            this.toolStripSeparator3,
            this.menuExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // menuNew
            // 
            this.menuNew.Name = "menuNew";
            this.menuNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuNew.Size = new System.Drawing.Size(217, 22);
            this.menuNew.Text = "&New File";
            // 
            // menuOpen
            // 
            this.menuOpen.Name = "menuOpen";
            this.menuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuOpen.Size = new System.Drawing.Size(217, 22);
            this.menuOpen.Text = "&Open File...";
            // 
            // menuImport
            // 
            this.menuImport.Name = "menuImport";
            this.menuImport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.menuImport.Size = new System.Drawing.Size(217, 22);
            this.menuImport.Text = "&Import File...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(214, 6);
            // 
            // menuSave
            // 
            this.menuSave.Name = "menuSave";
            this.menuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuSave.Size = new System.Drawing.Size(217, 22);
            this.menuSave.Text = "&Save";
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.Name = "menuSaveAs";
            this.menuSaveAs.Size = new System.Drawing.Size(217, 22);
            this.menuSaveAs.Text = "Save &As...";
            // 
            // menuExportModifications
            // 
            this.menuExportModifications.Name = "menuExportModifications";
            this.menuExportModifications.Size = new System.Drawing.Size(217, 22);
            this.menuExportModifications.Text = "&Export Modifications DAT...";
            // 
            // menuExportDAT
            // 
            this.menuExportDAT.Name = "menuExportDAT";
            this.menuExportDAT.Size = new System.Drawing.Size(217, 22);
            this.menuExportDAT.Text = "Export &DAT...";
            // 
            // menuExportRDB
            // 
            this.menuExportRDB.Name = "menuExportRDB";
            this.menuExportRDB.Size = new System.Drawing.Size(217, 22);
            this.menuExportRDB.Text = "Export &RDB...";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(214, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(217, 22);
            this.menuExit.Text = "E&xit";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolUnify,
            this.menuToolGenerateTags,
            this.menuToolDeltaDAT});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // menuToolUnify
            // 
            this.menuToolUnify.Name = "menuToolUnify";
            this.menuToolUnify.Size = new System.Drawing.Size(275, 22);
            this.menuToolUnify.Text = "&Unify Meta Data with Equal Field...";
            // 
            // menuToolGenerateTags
            // 
            this.menuToolGenerateTags.Name = "menuToolGenerateTags";
            this.menuToolGenerateTags.Size = new System.Drawing.Size(275, 22);
            this.menuToolGenerateTags.Text = "&Generate Region and Tags from Name";
            // 
            // menuToolDeltaDAT
            // 
            this.menuToolDeltaDAT.Name = "menuToolDeltaDAT";
            this.menuToolDeltaDAT.Size = new System.Drawing.Size(275, 22);
            this.menuToolDeltaDAT.Text = "Create &Delta DAT from Two Files...";
            // 
            // validateToolStripMenuItem
            // 
            this.validateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuValidateUnique,
            this.menuValidateField,
            this.menuValidateClear});
            this.validateToolStripMenuItem.Name = "validateToolStripMenuItem";
            this.validateToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.validateToolStripMenuItem.Text = "&Validate";
            // 
            // menuValidateUnique
            // 
            this.menuValidateUnique.Name = "menuValidateUnique";
            this.menuValidateUnique.Size = new System.Drawing.Size(193, 22);
            this.menuValidateUnique.Text = "Validate &Unique Field...";
            // 
            // menuValidateField
            // 
            this.menuValidateField.Name = "menuValidateField";
            this.menuValidateField.Size = new System.Drawing.Size(193, 22);
            this.menuValidateField.Text = "Validate &Field Values...";
            // 
            // menuValidateClear
            // 
            this.menuValidateClear.Name = "menuValidateClear";
            this.menuValidateClear.Size = new System.Drawing.Size(193, 22);
            this.menuValidateClear.Text = "&Clear Warning Flags";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAbout});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(107, 22);
            this.menuAbout.Text = "&About";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Filter:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.statusEntries,
            this.toolStripStatusLabel4,
            this.statusFiltered,
            this.toolStripStatusLabel1,
            this.statusChanges,
            this.statusChangesFiltered,
            this.toolStripStatusLabel3,
            this.statusWarnings,
            this.statusWarningsFiltered});
            this.statusStrip1.Location = new System.Drawing.Point(0, 468);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1063, 24);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(74, 19);
            this.toolStripStatusLabel2.Text = "Total Entries:";
            // 
            // statusEntries
            // 
            this.statusEntries.ActiveLinkColor = System.Drawing.Color.Red;
            this.statusEntries.AutoSize = false;
            this.statusEntries.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusEntries.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.statusEntries.Name = "statusEntries";
            this.statusEntries.Size = new System.Drawing.Size(50, 19);
            this.statusEntries.Text = "99999";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(53, 19);
            this.toolStripStatusLabel4.Text = "Filtered:";
            // 
            // statusFiltered
            // 
            this.statusFiltered.ActiveLinkColor = System.Drawing.Color.Red;
            this.statusFiltered.AutoSize = false;
            this.statusFiltered.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusFiltered.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.statusFiltered.Name = "statusFiltered";
            this.statusFiltered.Size = new System.Drawing.Size(50, 19);
            this.statusFiltered.Text = "99999";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(60, 19);
            this.toolStripStatusLabel1.Text = "Changes:";
            // 
            // statusChanges
            // 
            this.statusChanges.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.statusChanges.AutoSize = false;
            this.statusChanges.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusChanges.IsLink = true;
            this.statusChanges.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.statusChanges.Name = "statusChanges";
            this.statusChanges.Size = new System.Drawing.Size(50, 19);
            this.statusChanges.Text = "99999";
            this.statusChanges.ToolTipText = "Click to filter";
            // 
            // statusChangesFiltered
            // 
            this.statusChangesFiltered.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusChangesFiltered.Name = "statusChangesFiltered";
            this.statusChangesFiltered.Size = new System.Drawing.Size(54, 19);
            this.statusChangesFiltered.Text = "(Filtered)";
            this.statusChangesFiltered.Visible = false;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(64, 19);
            this.toolStripStatusLabel3.Text = "Warnings:";
            // 
            // statusWarnings
            // 
            this.statusWarnings.AutoSize = false;
            this.statusWarnings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusWarnings.IsLink = true;
            this.statusWarnings.LinkColor = System.Drawing.Color.Red;
            this.statusWarnings.Name = "statusWarnings";
            this.statusWarnings.Size = new System.Drawing.Size(50, 19);
            this.statusWarnings.Text = "99999";
            this.statusWarnings.ToolTipText = "Click to filter";
            // 
            // statusWarningsFiltered
            // 
            this.statusWarningsFiltered.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusWarningsFiltered.Name = "statusWarningsFiltered";
            this.statusWarningsFiltered.Size = new System.Drawing.Size(54, 19);
            this.statusWarningsFiltered.Text = "(Filtered)";
            this.statusWarningsFiltered.Visible = false;
            // 
            // webView
            // 
            this.webView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView.Location = new System.Drawing.Point(0, 0);
            this.webView.MinimumSize = new System.Drawing.Size(20, 20);
            this.webView.Name = "webView";
            this.webView.ScriptErrorsSuppressed = true;
            this.webView.Size = new System.Drawing.Size(150, 46);
            this.webView.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Window;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.validateToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1063, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEditCut,
            this.menuEditCopy,
            this.menuEditPaste,
            this.menuEditDelete});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // menuEditCut
            // 
            this.menuEditCut.Name = "menuEditCut";
            this.menuEditCut.Size = new System.Drawing.Size(107, 22);
            this.menuEditCut.Text = "Cut";
            // 
            // menuEditCopy
            // 
            this.menuEditCopy.Name = "menuEditCopy";
            this.menuEditCopy.Size = new System.Drawing.Size(107, 22);
            this.menuEditCopy.Text = "Copy";
            // 
            // menuEditPaste
            // 
            this.menuEditPaste.Name = "menuEditPaste";
            this.menuEditPaste.Size = new System.Drawing.Size(107, 22);
            this.menuEditPaste.Text = "Paste";
            // 
            // menuEditDelete
            // 
            this.menuEditDelete.Name = "menuEditDelete";
            this.menuEditDelete.Size = new System.Drawing.Size(107, 22);
            this.menuEditDelete.Text = "Delete";
            // 
            // splitContainer
            // 
            this.splitContainer.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(0, 24);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer.Panel1.Controls.Add(this.btnClearFilter);
            this.splitContainer.Panel1.Controls.Add(this.txtFilter);
            this.splitContainer.Panel1.Controls.Add(this.label1);
            this.splitContainer.Panel1.Controls.Add(this.gridMain);
            this.splitContainer.Panel1MinSize = 80;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer.Panel2.Controls.Add(this.webView);
            this.splitContainer.Panel2Collapsed = true;
            this.splitContainer.Panel2MinSize = 10;
            this.splitContainer.Size = new System.Drawing.Size(1063, 444);
            this.splitContainer.SplitterDistance = 80;
            this.splitContainer.SplitterWidth = 3;
            this.splitContainer.TabIndex = 1;
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearFilter.Location = new System.Drawing.Point(1007, 3);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(52, 22);
            this.btnClearFilter.TabIndex = 2;
            this.btnClearFilter.Text = "Clear";
            this.btnClearFilter.UseVisualStyleBackColor = true;
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(36, 4);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(966, 20);
            this.txtFilter.TabIndex = 1;
            // 
            // gridMain
            // 
            this.gridMain.AllowUserToAddRows = false;
            this.gridMain.AllowUserToDeleteRows = false;
            this.gridMain.AllowUserToOrderColumns = true;
            this.gridMain.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.gridMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMain.BackgroundColor = System.Drawing.Color.White;
            this.gridMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMain.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridMain.Location = new System.Drawing.Point(0, 28);
            this.gridMain.Name = "gridMain";
            this.gridMain.RowHeadersVisible = false;
            this.gridMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridMain.ShowCellErrors = false;
            this.gridMain.ShowEditingIcon = false;
            this.gridMain.ShowRowErrors = false;
            this.gridMain.Size = new System.Drawing.Size(1063, 416);
            this.gridMain.TabIndex = 3;
            // 
            // RDBEdForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1063, 492);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RDBEdForm";
            this.Text = "RDBEd";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}
