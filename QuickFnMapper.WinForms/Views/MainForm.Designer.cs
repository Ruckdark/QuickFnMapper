namespace QuickFnMapper.WinForms.Views
{
    partial class MainForm
    {
        /// <summary>
        /// <para>Required designer variable.</para>
        /// <para>Biến thiết kế bắt buộc.</para>
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// <para>Clean up any resources being used.</para>
        /// <para>Dọn dẹp mọi tài nguyên đang được sử dụng.</para>
        /// </summary>
        /// <param name="disposing">
        /// <para>true if managed resources should be disposed; otherwise, false.</para>
        /// <para>true nếu các tài nguyên được quản lý nên được giải phóng; ngược lại, false.</para>
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            // Quan trọng: Giải phóng GlobalHookService nếu nó được MainForm sở hữu và khởi tạo
            if (disposing && (_globalHookService != null)) // _globalHookService là field trong MainForm.cs
            {
                _globalHookService.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// <para>Required method for Designer support - do not modify</para>
        /// <para>the contents of this method with the code editor.</para>
        /// <para>Phương thức bắt buộc cho hỗ trợ Designer - không sửa đổi</para>
        /// <para>nội dung của phương thức này bằng trình soạn thảo mã.</para>
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Yes",
            "Sample Rule 1",
            "Ctrl + Shift + A",
            "Run Application: notepad.exe",
            "0",
            "29/05/2025 10:00"}, -1);
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
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
            this.menuStripMain.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.pnlMainContent.SuspendLayout(); // Panel sẽ chứa ListView hoặc các UserControl khác
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.rulesToolStripMenuItem,
            this.serviceToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(784, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
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
            this.rulesToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.rulesToolStripMenuItem.Text = "&Rules";
            // 
            // addRuleToolStripMenuItem
            // 
            this.addRuleToolStripMenuItem.Name = "addRuleToolStripMenuItem";
            this.addRuleToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.addRuleToolStripMenuItem.Text = "&Add Rule...";
            this.addRuleToolStripMenuItem.Click += new System.EventHandler(this.tsbAddRule_Click); // Dùng lại handler của ToolStripButton
            // 
            // editRuleToolStripMenuItem
            // 
            this.editRuleToolStripMenuItem.Name = "editRuleToolStripMenuItem";
            this.editRuleToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.editRuleToolStripMenuItem.Text = "&Edit Rule...";
            this.editRuleToolStripMenuItem.Click += new System.EventHandler(this.tsbEditRule_Click); // Dùng lại handler
            // 
            // deleteRuleToolStripMenuItem
            // 
            this.deleteRuleToolStripMenuItem.Name = "deleteRuleToolStripMenuItem";
            this.deleteRuleToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.deleteRuleToolStripMenuItem.Text = "&Delete Rule";
            this.deleteRuleToolStripMenuItem.Click += new System.EventHandler(this.tsbDeleteRule_Click); // Dùng lại handler
            // 
            // serviceToolStripMenuItem
            // 
            this.serviceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleServiceToolStripMenuItem});
            this.serviceToolStripMenuItem.Name = "serviceToolStripMenuItem";
            this.serviceToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.serviceToolStripMenuItem.Text = "&Service";
            // 
            // toggleServiceToolStripMenuItem
            // 
            this.toggleServiceToolStripMenuItem.Name = "toggleServiceToolStripMenuItem";
            this.toggleServiceToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.toggleServiceToolStripMenuItem.Text = "&Enable Service"; // Text sẽ được cập nhật
            this.toggleServiceToolStripMenuItem.Click += new System.EventHandler(this.tsbToggleService_Click); // Dùng lại handler
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.settingsToolStripMenuItem.Text = "&Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.tsbSettings_Click); // Dùng lại handler
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            // this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click); // Cần tạo handler nếu có
            // 
            // toolStripMain
            // 
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
            this.toolStripMain.Location = new System.Drawing.Point(0, 24);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(784, 25);
            this.toolStripMain.TabIndex = 1;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // tsbShowHome
            // 
            this.tsbShowHome.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image; // Hoặc Text
            // this.tsbShowHome.Image = ((System.Drawing.Image)(resources.GetObject("tsbShowHome.Image"))); // Cần hình ảnh
            this.tsbShowHome.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbShowHome.Name = "tsbShowHome";
            this.tsbShowHome.Size = new System.Drawing.Size(23, 22);
            this.tsbShowHome.Text = "Show Home Screen";
            this.tsbShowHome.Click += new System.EventHandler(this.tsbShowHome_Click); // Cần tạo handler
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbAddRule
            // 
            this.tsbAddRule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            // this.tsbAddRule.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddRule.Image")));
            this.tsbAddRule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddRule.Name = "tsbAddRule";
            this.tsbAddRule.Size = new System.Drawing.Size(23, 22);
            this.tsbAddRule.Text = "Add New Rule";
            this.tsbAddRule.Click += new System.EventHandler(this.tsbAddRule_Click);
            // 
            // tsbEditRule
            // 
            this.tsbEditRule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            // this.tsbEditRule.Image = ((System.Drawing.Image)(resources.GetObject("tsbEditRule.Image")));
            this.tsbEditRule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEditRule.Name = "tsbEditRule";
            this.tsbEditRule.Size = new System.Drawing.Size(23, 22);
            this.tsbEditRule.Text = "Edit Selected Rule";
            this.tsbEditRule.Click += new System.EventHandler(this.tsbEditRule_Click);
            // 
            // tsbDeleteRule
            // 
            this.tsbDeleteRule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            // this.tsbDeleteRule.Image = ((System.Drawing.Image)(resources.GetObject("tsbDeleteRule.Image")));
            this.tsbDeleteRule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteRule.Name = "tsbDeleteRule";
            this.tsbDeleteRule.Size = new System.Drawing.Size(23, 22);
            this.tsbDeleteRule.Text = "Delete Selected Rule";
            this.tsbDeleteRule.Click += new System.EventHandler(this.tsbDeleteRule_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbToggleService
            // 
            this.tsbToggleService.CheckOnClick = true; // Quan trọng
            this.tsbToggleService.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text; // Hoặc ImageAndText
            // this.tsbToggleService.Image = ((System.Drawing.Image)(resources.GetObject("tsbToggleService.Image")));
            this.tsbToggleService.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbToggleService.Name = "tsbToggleService";
            this.tsbToggleService.Size = new System.Drawing.Size(87, 22);
            this.tsbToggleService.Text = "Enable Service"; // Sẽ được cập nhật
            this.tsbToggleService.Click += new System.EventHandler(this.tsbToggleService_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSettings
            // 
            this.tsbSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            // this.tsbSettings.Image = ((System.Drawing.Image)(resources.GetObject("tsbSettings.Image")));
            this.tsbSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSettings.Name = "tsbSettings";
            this.tsbSettings.Size = new System.Drawing.Size(23, 22);
            this.tsbSettings.Text = "Application Settings";
            this.tsbSettings.Click += new System.EventHandler(this.tsbSettings_Click);
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusInfo,
            this.lblStatusService});
            this.statusStripMain.Location = new System.Drawing.Point(0, 539);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(784, 22);
            this.statusStripMain.TabIndex = 2;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // lblStatusInfo
            // 
            this.lblStatusInfo.Name = "lblStatusInfo";
            this.lblStatusInfo.Size = new System.Drawing.Size(39, 17);
            this.lblStatusInfo.Text = "Ready";
            // 
            // lblStatusService
            // 
            this.lblStatusService.Name = "lblStatusService";
            this.lblStatusService.Size = new System.Drawing.Size(730, 17); // Spring = true sẽ làm nó co giãn
            this.lblStatusService.Spring = true;
            this.lblStatusService.Text = "Service: Unknown";
            this.lblStatusService.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlMainContent
            // 
            this.pnlMainContent.Controls.Add(this.listViewRules); // Ban đầu có thể chứa ListView
            this.pnlMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContent.Location = new System.Drawing.Point(0, 49); // Sau MenuStrip và ToolStrip
            this.pnlMainContent.Name = "pnlMainContent";
            this.pnlMainContent.Size = new System.Drawing.Size(784, 490);
            this.pnlMainContent.TabIndex = 3;
            // 
            // listViewRules
            // 
            this.listViewRules.CheckBoxes = true; // Nếu muốn bật/tắt rule từ ListView
            this.listViewRules.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colEnabled,
            this.colRuleName,
            this.colOriginalKey,
            this.colTargetAction,
            this.colOrder,
            this.colLastModified});
            this.listViewRules.Dock = System.Windows.Forms.DockStyle.Fill; // Để ListView chiếm hết pnlMainContent ban đầu
            this.listViewRules.FullRowSelect = true;
            this.listViewRules.GridLines = true;
            this.listViewRules.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            this.listViewRules.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1}); // Ví dụ item, sẽ bị xóa bởi DisplayRules
            this.listViewRules.Location = new System.Drawing.Point(0, 0); // Nằm trong pnlMainContent
            this.listViewRules.MultiSelect = false;
            this.listViewRules.Name = "listViewRules";
            this.listViewRules.Size = new System.Drawing.Size(784, 490);
            this.listViewRules.TabIndex = 0;
            this.listViewRules.UseCompatibleStateImageBehavior = false;
            this.listViewRules.View = System.Windows.Forms.View.Details;
            this.listViewRules.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewRules_ItemChecked);
            this.listViewRules.DoubleClick += new System.EventHandler(this.listViewRules_DoubleClick);
            // 
            // colEnabled
            // 
            this.colEnabled.Text = "Enabled";
            this.colEnabled.Width = 70;
            // 
            // colRuleName
            // 
            this.colRuleName.Text = "Rule Name";
            this.colRuleName.Width = 200;
            // 
            // colOriginalKey
            // 
            this.colOriginalKey.Text = "Original Key";
            this.colOriginalKey.Width = 150;
            // 
            // colTargetAction
            // 
            this.colTargetAction.Text = "Target Action";
            this.colTargetAction.Width = 200;
            // 
            // colOrder
            // 
            this.colOrder.Text = "Order";
            this.colOrder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colOrder.Width = 50;
            // 
            // colLastModified
            // 
            this.colLastModified.Text = "Last Modified";
            this.colLastModified.Width = 120;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561); // Kích thước ví dụ
            this.Controls.Add(this.pnlMainContent);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "MainForm";
            this.Text = "QuickFn Mapper";
            // Sự kiện Load và FormClosing sẽ được đăng ký trong MainForm.cs constructor
            // hoặc Designer cũng có thể tự tạo nếu Đại ca double click vào Form trong trình thiết kế.
            // this.Load += new System.EventHandler(this.MainForm_Load);
            // this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.pnlMainContent.ResumeLayout(false); // Panel chứa ListView
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStripMain;
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
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleServiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rulesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addRuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editRuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tsbShowHome;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}