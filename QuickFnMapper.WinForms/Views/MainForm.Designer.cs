// File: QuickFnMapper.WinForms/Views/MainForm.Designer.cs

// Đảm bảo các using directives cần thiết đã có ở đầu file
using System.Windows.Forms;
using System.Drawing;

namespace QuickFnMapper.WinForms.Views
{
    partial class MainForm
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
            if (disposing && (_globalHookService != null)) // Giả sử _globalHookService được khai báo trong MainForm.cs
            {
                _globalHookService.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        // File: QuickFnMapper.WinForms/Views/MainForm.Designer.cs

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] { "Yes", "Sample Rule (Designer File)", "Ctrl + Shift + X", "Run Application: explorer.exe", "0", System.DateTime.Now.ToString("g") }, -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addRuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editRuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.tsbShowHome = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAddRule = new System.Windows.Forms.ToolStripButton();
            this.tsbEditRule = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteRule = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbToggleService = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSettings = new System.Windows.Forms.ToolStripButton();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.lblStatusInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusService = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlMainContent = new System.Windows.Forms.Panel();
            this.listViewRules = new System.Windows.Forms.ListView();
            this.colEnabled = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRuleName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOriginalKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTargetAction = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOrder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLastModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.trayNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItemTray = new System.Windows.Forms.ToolStripMenuItem();
            this.traySeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toggleServiceTrayMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItemTray = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuStrip.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.pnlMainContent.SuspendLayout();
            this.trayContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.mainMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.rulesToolStripMenuItem,
            this.serviceToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(884, 28);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // rulesToolStripMenuItem
            // 
            this.rulesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRuleToolStripMenuItem,
            this.editRuleToolStripMenuItem,
            this.deleteRuleToolStripMenuItem});
            this.rulesToolStripMenuItem.Name = "rulesToolStripMenuItem";
            this.rulesToolStripMenuItem.Size = new System.Drawing.Size(59, 24);
            this.rulesToolStripMenuItem.Text = "&Rules";
            // 
            // addRuleToolStripMenuItem
            // 
            this.addRuleToolStripMenuItem.Name = "addRuleToolStripMenuItem";
            this.addRuleToolStripMenuItem.Size = new System.Drawing.Size(176, 26); // VS Sẽ tự tính lại size
            this.addRuleToolStripMenuItem.Text = "&Add Rule...";
            this.addRuleToolStripMenuItem.Click += new System.EventHandler(this.tsbAddRule_Click);
            // 
            // editRuleToolStripMenuItem
            // 
            this.editRuleToolStripMenuItem.Name = "editRuleToolStripMenuItem";
            this.editRuleToolStripMenuItem.Size = new System.Drawing.Size(176, 26); // VS Sẽ tự tính lại size
            this.editRuleToolStripMenuItem.Text = "&Edit Rule...";
            this.editRuleToolStripMenuItem.Click += new System.EventHandler(this.tsbEditRule_Click);
            // 
            // deleteRuleToolStripMenuItem
            // 
            this.deleteRuleToolStripMenuItem.Name = "deleteRuleToolStripMenuItem";
            this.deleteRuleToolStripMenuItem.Size = new System.Drawing.Size(176, 26); // VS Sẽ tự tính lại size
            this.deleteRuleToolStripMenuItem.Text = "&Delete Rule";
            this.deleteRuleToolStripMenuItem.Click += new System.EventHandler(this.tsbDeleteRule_Click);
            // 
            // serviceToolStripMenuItem
            // 
            this.serviceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleServiceToolStripMenuItem});
            this.serviceToolStripMenuItem.Name = "serviceToolStripMenuItem";
            this.serviceToolStripMenuItem.Size = new System.Drawing.Size(70, 24);
            this.serviceToolStripMenuItem.Text = "&Service";
            // 
            // toggleServiceToolStripMenuItem
            // 
            this.toggleServiceToolStripMenuItem.CheckOnClick = true;
            this.toggleServiceToolStripMenuItem.Name = "toggleServiceToolStripMenuItem";
            this.toggleServiceToolStripMenuItem.Size = new System.Drawing.Size(188, 26); // VS Sẽ tự tính lại size
            this.toggleServiceToolStripMenuItem.Text = "&Enable Service";
            this.toggleServiceToolStripMenuItem.Click += new System.EventHandler(this.tsbToggleService_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(155, 26); // VS Sẽ tự tính lại size
            this.settingsToolStripMenuItem.Text = "&Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.tsbSettings_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(142, 26); // VS Sẽ tự tính lại size
            this.aboutToolStripMenuItem.Text = "&About...";
            // 
            // toolStripMain
            // 
            this.toolStripMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(32, 32); // *** Đặt kích thước icon mong muốn ***
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbShowHome,
            this.toolStripSeparator3,
            this.tsbAddRule,
            this.tsbEditRule,
            this.tsbDeleteRule,
            this.toolStripSeparator1,
            this.tsbToggleService,
            this.toolStripSeparator2,
            this.tsbSettings});
            this.toolStripMain.Location = new System.Drawing.Point(0, 28);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripMain.Size = new System.Drawing.Size(884, 31); // Chiều cao có thể tự điều chỉnh (ví dụ 24 + padding)
            this.toolStripMain.TabIndex = 1;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // tsbShowHome
            // 
            this.tsbShowHome.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image; // *** SỬA ***
            this.tsbShowHome.Image = global::QuickFnMapper.Properties.Resources.HomeIcon; // *** SỬA ***
            this.tsbShowHome.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbShowHome.Name = "tsbShowHome";
            this.tsbShowHome.Size = new System.Drawing.Size(29, 28); // Kích thước sẽ theo ImageScalingSize
            this.tsbShowHome.Text = ""; // *** SỬA ***
            this.tsbShowHome.ToolTipText = "Show Home Screen"; // *** SỬA ***
            this.tsbShowHome.Click += new System.EventHandler(this.tsbShowHome_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 31); // Chiều cao theo toolstrip
                                                                            // 
                                                                            // tsbAddRule
                                                                            // 
            this.tsbAddRule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image; // *** SỬA ***
            this.tsbAddRule.Image = global::QuickFnMapper.Properties.Resources.AddRuleIcon; // *** SỬA ***
            this.tsbAddRule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddRule.Name = "tsbAddRule";
            this.tsbAddRule.Size = new System.Drawing.Size(29, 28);
            this.tsbAddRule.Text = ""; // *** SỬA ***
            this.tsbAddRule.ToolTipText = "Add New Rule"; // *** SỬA ***
            this.tsbAddRule.Click += new System.EventHandler(this.tsbAddRule_Click);
            // 
            // tsbEditRule
            // 
            this.tsbEditRule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image; // *** SỬA ***
            this.tsbEditRule.Image = global::QuickFnMapper.Properties.Resources.EditRuleIcon; // *** SỬA ***
            this.tsbEditRule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEditRule.Name = "tsbEditRule";
            this.tsbEditRule.Size = new System.Drawing.Size(29, 28);
            this.tsbEditRule.Text = ""; // *** SỬA ***
            this.tsbEditRule.ToolTipText = "Edit Selected Rule"; // *** SỬA ***
            this.tsbEditRule.Click += new System.EventHandler(this.tsbEditRule_Click);
            // 
            // tsbDeleteRule
            // 
            this.tsbDeleteRule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image; // *** SỬA ***
            this.tsbDeleteRule.Image = global::QuickFnMapper.Properties.Resources.DeleteRuleIcon; // *** SỬA ***
            this.tsbDeleteRule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteRule.Name = "tsbDeleteRule";
            this.tsbDeleteRule.Size = new System.Drawing.Size(29, 28);
            this.tsbDeleteRule.Text = ""; // *** SỬA ***
            this.tsbDeleteRule.ToolTipText = "Delete Selected Rule"; // *** SỬA ***
            this.tsbDeleteRule.Click += new System.EventHandler(this.tsbDeleteRule_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbToggleService
            // 
            this.tsbToggleService.CheckOnClick = true;
            this.tsbToggleService.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image; // *** SỬA ***
            this.tsbToggleService.Image = global::QuickFnMapper.Properties.Resources.ServiceOnIcon; // Icon mặc định, sẽ đổi trong code
            this.tsbToggleService.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbToggleService.Name = "tsbToggleService";
            this.tsbToggleService.Size = new System.Drawing.Size(29, 28);
            this.tsbToggleService.Text = ""; // *** SỬA ***
            this.tsbToggleService.ToolTipText = "Enable Service"; // Sẽ được cập nhật trong code
            this.tsbToggleService.Click += new System.EventHandler(this.tsbToggleService_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbSettings
            // 
            this.tsbSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image; // *** SỬA ***
            this.tsbSettings.Image = global::QuickFnMapper.Properties.Resources.SettingsIcon; // *** SỬA ***
            this.tsbSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSettings.Name = "tsbSettings";
            this.tsbSettings.Size = new System.Drawing.Size(29, 28);
            this.tsbSettings.Text = ""; // *** SỬA ***
            this.tsbSettings.ToolTipText = "Application Settings"; // *** SỬA ***
            this.tsbSettings.Click += new System.EventHandler(this.tsbSettings_Click);
            // 
            // statusStripMain
            // 
            this.statusStripMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statusStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
    this.lblStatusInfo,
    this.lblStatusService});
            this.statusStripMain.Location = new System.Drawing.Point(0, 535); // ClientSize.Height - StatusStrip.Height
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(884, 26);
            this.statusStripMain.TabIndex = 3;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // lblStatusInfo
            // 
            this.lblStatusInfo.Name = "lblStatusInfo";
            this.lblStatusInfo.Size = new System.Drawing.Size(50, 20);
            this.lblStatusInfo.Text = "Ready";
            // 
            // lblStatusService
            // 
            this.lblStatusService.Name = "lblStatusService";
            this.lblStatusService.Size = new System.Drawing.Size(819, 20);
            this.lblStatusService.Spring = true;
            this.lblStatusService.Text = "Service: Unknown";
            this.lblStatusService.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlMainContent
            // 
            this.pnlMainContent.Controls.Add(this.listViewRules);
            this.pnlMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContent.Location = new System.Drawing.Point(0, 59); // MenuStrip.Height (28) + ToolStrip.Height (31)
            this.pnlMainContent.Name = "pnlMainContent";
            this.pnlMainContent.Padding = new System.Windows.Forms.Padding(5);
            this.pnlMainContent.Size = new System.Drawing.Size(884, 476); // ClientSize.Height - Location.Y - StatusStrip.Height
            this.pnlMainContent.TabIndex = 2;
            // 
            // listViewRules
            // 
            this.listViewRules.CheckBoxes = true;
            this.listViewRules.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
    this.colEnabled,
    this.colRuleName,
    this.colOriginalKey,
    this.colTargetAction,
    this.colOrder,
    this.colLastModified});
            this.listViewRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewRules.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listViewRules.FullRowSelect = true;
            this.listViewRules.GridLines = true;
            this.listViewRules.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            this.listViewRules.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
    listViewItem1});
            this.listViewRules.Location = new System.Drawing.Point(5, 5);
            this.listViewRules.MultiSelect = false;
            this.listViewRules.Name = "listViewRules";
            this.listViewRules.Size = new System.Drawing.Size(874, 466); // pnlMainContent.ClientHeight - Padding*2
            this.listViewRules.TabIndex = 0;
            this.listViewRules.UseCompatibleStateImageBehavior = false;
            this.listViewRules.View = System.Windows.Forms.View.Details;
            this.listViewRules.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewRules_ItemChecked);
            this.listViewRules.DoubleClick += new System.EventHandler(this.listViewRules_DoubleClick);
            // 
            // colEnabled
            // 
            this.colEnabled.Text = "Active";
            this.colEnabled.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colEnabled.Width = 50;
            // 
            // colRuleName
            // 
            this.colRuleName.Text = "Rule Name";
            this.colRuleName.Width = 220;
            // 
            // colOriginalKey
            // 
            this.colOriginalKey.Text = "Original Key";
            this.colOriginalKey.Width = 180;
            // 
            // colTargetAction
            // 
            this.colTargetAction.Text = "Target Action";
            this.colTargetAction.Width = 250;
            // 
            // colOrder
            // 
            this.colOrder.Text = "Order";
            this.colOrder.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colOrder.Width = 50;
            // 
            // colLastModified
            // 
            this.colLastModified.Text = "Last Modified";
            this.colLastModified.Width = 120;
            // 
            // trayNotifyIcon
            // 
            this.trayNotifyIcon.ContextMenuStrip = this.trayContextMenuStrip;
            // Icon sẽ được gán trong MainForm.cs, ví dụ:
            // this.trayNotifyIcon.Icon = global::QuickFnMapper.WinForms.Properties.Resources.AppTrayDisabledIcon; 
            this.trayNotifyIcon.Text = "QuickFn Mapper";
            // 
            // trayContextMenuStrip
            // 
            this.trayContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.trayContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
    this.showToolStripMenuItemTray,
    this.traySeparator1,
    this.toggleServiceTrayMenuItem,
    this.exitToolStripMenuItemTray});
            this.trayContextMenuStrip.Name = "trayContextMenuStrip";
            this.trayContextMenuStrip.Size = new System.Drawing.Size(235, 88); // Cập nhật Size để chứa item mới (VS tự tính)
                                                                               // 
                                                                               // showToolStripMenuItemTray
                                                                               // 
            this.showToolStripMenuItemTray.Name = "showToolStripMenuItemTray";
            this.showToolStripMenuItemTray.Size = new System.Drawing.Size(234, 26);
            this.showToolStripMenuItemTray.Text = "Hiển thị QuickFn Mapper";
            // 
            // traySeparator1
            // 
            this.traySeparator1.Name = "traySeparator1";
            this.traySeparator1.Size = new System.Drawing.Size(231, 6);
            // 
            // toggleServiceTrayMenuItem
            // 
            this.toggleServiceTrayMenuItem.CheckOnClick = true;
            this.toggleServiceTrayMenuItem.Name = "toggleServiceTrayMenuItem";
            this.toggleServiceTrayMenuItem.Size = new System.Drawing.Size(234, 26);
            this.toggleServiceTrayMenuItem.Text = "&Enable Service";
            // 
            // exitToolStripMenuItemTray
            // 
            this.exitToolStripMenuItemTray.Name = "exitToolStripMenuItemTray";
            this.exitToolStripMenuItemTray.Size = new System.Drawing.Size(234, 26);
            this.exitToolStripMenuItemTray.Text = "Thoát";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.pnlMainContent);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.mainMenuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.mainMenuStrip;
            this.MinimumSize = new System.Drawing.Size(700, 450);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QuickFn Mapper - Advanced Key Remapping";
            // this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon"))); 
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.pnlMainContent.ResumeLayout(false);
            this.trayContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rulesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addRuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editRuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleServiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton tsbShowHome;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbAddRule;
        private System.Windows.Forms.ToolStripButton tsbEditRule;
        private System.Windows.Forms.ToolStripButton tsbDeleteRule;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbToggleService;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbSettings;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusInfo;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusService;
        private System.Windows.Forms.Panel pnlMainContent;
        private System.Windows.Forms.ListView listViewRules;
        private System.Windows.Forms.ColumnHeader colEnabled;
        private System.Windows.Forms.ColumnHeader colRuleName;
        private System.Windows.Forms.ColumnHeader colOriginalKey;
        private System.Windows.Forms.ColumnHeader colTargetAction;
        private System.Windows.Forms.ColumnHeader colOrder;
        private System.Windows.Forms.ColumnHeader colLastModified;
        private System.Windows.Forms.NotifyIcon trayNotifyIcon;
        private System.Windows.Forms.ContextMenuStrip trayContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItemTray;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItemTray;
        private System.Windows.Forms.ToolStripSeparator traySeparator1;
        private System.Windows.Forms.ToolStripMenuItem toggleServiceTrayMenuItem;
    }
}
