
namespace QuickFnMapper.WinForms.Views
{
    partial class HomeControl
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
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// <para>Required method for Designer support - do not modify </para>
        /// <para>the contents of this method with the code editor.</para>
        /// <para>Phương thức bắt buộc cho hỗ trợ Designer - không sửa đổi</para>
        /// <para>nội dung của phương thức này bằng trình soạn thảo mã.</para>
        /// </summary>
        private void InitializeComponent()
        {
            this.lblWelcomeMessage = new System.Windows.Forms.Label();
            this.lblServiceStatus = new System.Windows.Forms.Label();
            this.btnManageRules = new System.Windows.Forms.Button();
            this.btnToggleServiceOnHome = new System.Windows.Forms.Button(); // Đổi tên để phân biệt với nút trên MainForm nếu cần
            this.SuspendLayout();
            // 
            // lblWelcomeMessage
            // 
            this.lblWelcomeMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWelcomeMessage.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcomeMessage.Location = new System.Drawing.Point(20, 20);
            this.lblWelcomeMessage.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10); // Thêm margin dưới
            this.lblWelcomeMessage.Name = "lblWelcomeMessage";
            this.lblWelcomeMessage.Size = new System.Drawing.Size(460, 40); // Điều chỉnh kích thước
            this.lblWelcomeMessage.TabIndex = 0;
            this.lblWelcomeMessage.Text = "Welcome to QuickFn Mapper!";
            this.lblWelcomeMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblServiceStatus
            // 
            this.lblServiceStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblServiceStatus.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceStatus.Location = new System.Drawing.Point(20, 70); // Cách lblWelcomeMessage
            this.lblServiceStatus.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblServiceStatus.Name = "lblServiceStatus";
            this.lblServiceStatus.Size = new System.Drawing.Size(460, 25);
            this.lblServiceStatus.TabIndex = 1;
            this.lblServiceStatus.Text = "Service Status: Unknown";
            this.lblServiceStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnManageRules
            // 
            this.btnManageRules.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnManageRules.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageRules.Location = new System.Drawing.Point(150, 115); // Vị trí ví dụ
            this.btnManageRules.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.btnManageRules.Name = "btnManageRules";
            this.btnManageRules.Size = new System.Drawing.Size(200, 35); // Kích thước ví dụ
            this.btnManageRules.TabIndex = 2;
            this.btnManageRules.Text = "&Manage Rules";
            this.btnManageRules.UseVisualStyleBackColor = true;
            this.btnManageRules.Click += new System.EventHandler(this.btnManageRules_Click); // Đảm bảo handler này có trong HomeControl.cs
            // 
            // btnToggleServiceOnHome
            // 
            this.btnToggleServiceOnHome.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnToggleServiceOnHome.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleServiceOnHome.Location = new System.Drawing.Point(150, 160); // Vị trí ví dụ
            this.btnToggleServiceOnHome.Name = "btnToggleServiceOnHome";
            this.btnToggleServiceOnHome.Size = new System.Drawing.Size(200, 35);
            this.btnToggleServiceOnHome.TabIndex = 3;
            this.btnToggleServiceOnHome.Text = "Enable Service"; // Text này sẽ được cập nhật
            this.btnToggleServiceOnHome.UseVisualStyleBackColor = true;
            this.btnToggleServiceOnHome.Click += new System.EventHandler(this.btnToggleServiceOnHome_Click); // Đảm bảo handler này có trong HomeControl.cs
            // 
            // HomeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F); // Sử dụng Font Segoe UI 9pt
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight; // Màu nền sáng hơn
            this.Controls.Add(this.btnToggleServiceOnHome);
            this.Controls.Add(this.btnManageRules);
            this.Controls.Add(this.lblServiceStatus);
            this.Controls.Add(this.lblWelcomeMessage);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))); // Font chung cho control
            this.Name = "HomeControl";
            this.Padding = new System.Windows.Forms.Padding(10); // Thêm padding cho control
            this.Size = new System.Drawing.Size(500, 220); // Kích thước ví dụ cho UserControl
            this.Load += new System.EventHandler(this.HomeControl_Load); // Đảm bảo handler này có trong HomeControl.cs
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblWelcomeMessage;
        private System.Windows.Forms.Label lblServiceStatus;
        private System.Windows.Forms.Button btnManageRules;
        private System.Windows.Forms.Button btnToggleServiceOnHome;
    }
}