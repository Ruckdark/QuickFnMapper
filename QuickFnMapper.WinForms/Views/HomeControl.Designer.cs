namespace QuickFnMapper.WinForms.Views // Đã sửa namespace
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
            // Các dòng này là mặc định hoặc do Designer tạo khi UserControl được tạo
            // components = new System.ComponentModel.Container(); // Dòng này có thể đã có sẵn hoặc không tùy thuộc vào project template
            // this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; // Dòng này cũng vậy

            // --- BẮT ĐẦU PHẦN MINH HỌA THÊM CONTROL ---
            // Đại ca sẽ sử dụng trình thiết kế để thêm các control này.
            // Code dưới đây chỉ để minh họa những gì Designer có thể tạo ra.

            this.lblWelcomeMessage = new System.Windows.Forms.Label();
            this.lblServiceStatus = new System.Windows.Forms.Label();
            this.btnManageRules = new System.Windows.Forms.Button();
            this.btnToggleService = new System.Windows.Forms.Button(); // Nút tùy chọn
            this.SuspendLayout();
            // 
            // lblWelcomeMessage
            // 
            this.lblWelcomeMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWelcomeMessage.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcomeMessage.Location = new System.Drawing.Point(15, 15);
            this.lblWelcomeMessage.Name = "lblWelcomeMessage";
            this.lblWelcomeMessage.Size = new System.Drawing.Size(470, 30); // Kích thước ví dụ, điều chỉnh bằng Anchor
            this.lblWelcomeMessage.TabIndex = 0;
            this.lblWelcomeMessage.Text = "Welcome to QuickFn Mapper!";
            this.lblWelcomeMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblServiceStatus
            // 
            this.lblServiceStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblServiceStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceStatus.Location = new System.Drawing.Point(15, 55);
            this.lblServiceStatus.Name = "lblServiceStatus";
            this.lblServiceStatus.Size = new System.Drawing.Size(470, 23); // Kích thước ví dụ
            this.lblServiceStatus.TabIndex = 1;
            this.lblServiceStatus.Text = "Service Status: Unknown";
            this.lblServiceStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnManageRules
            // 
            this.btnManageRules.Anchor = System.Windows.Forms.AnchorStyles.Top; // Ví dụ anchor
            this.btnManageRules.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageRules.Location = new System.Drawing.Point(150, 100); // Vị trí ví dụ
            this.btnManageRules.Name = "btnManageRules";
            this.btnManageRules.Size = new System.Drawing.Size(200, 30); // Kích thước ví dụ
            this.btnManageRules.TabIndex = 2;
            this.btnManageRules.Text = "Manage Key Mapping Rules";
            this.btnManageRules.UseVisualStyleBackColor = true;
            // Dòng Click event sẽ được Designer thêm khi Đại ca double-click vào button trong trình thiết kế
            // và tạo phương thức xử lý trong HomeControl.cs
            this.btnManageRules.Click += new System.EventHandler(this.btnManageRules_Click);
            // 
            // btnToggleService
            // 
            this.btnToggleService.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnToggleService.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleService.Location = new System.Drawing.Point(150, 140); // Vị trí ví dụ
            this.btnToggleService.Name = "btnToggleService";
            this.btnToggleService.Size = new System.Drawing.Size(200, 30); // Kích thước ví dụ
            this.btnToggleService.TabIndex = 3;
            this.btnToggleService.Text = "Enable Service"; // Text này có thể được Controller thay đổi
            this.btnToggleService.UseVisualStyleBackColor = true;
            this.btnToggleService.Click += new System.EventHandler(this.btnToggleService_Click);
            // 
            // HomeControl
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F); // Dòng này có thể Designer đã tạo
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; // Dòng này có thể Designer đã tạo
            this.Controls.Add(this.btnToggleService);
            this.Controls.Add(this.btnManageRules);
            this.Controls.Add(this.lblServiceStatus);
            this.Controls.Add(this.lblWelcomeMessage);
            this.Name = "HomeControl";
            this.Size = new System.Drawing.Size(500, 200); // Kích thước ví dụ cho UserControl
            // Dòng Load event sẽ được Designer thêm khi Đại ca double-click vào UserControl trong trình thiết kế
            // và tạo phương thức xử lý trong HomeControl.cs
            this.Load += new System.EventHandler(this.HomeControl_Load);
            this.ResumeLayout(false);
            // this.PerformLayout(); // Dòng này có thể Designer đã tạo hoặc không cần thiết tùy thuộc vào controls

            // --- KẾT THÚC PHẦN MINH HỌA THÊM CONTROL ---
        }

        #endregion

        // --- KHAI BÁO BIẾN CHO CÁC CONTROL ---
        // Visual Studio Designer sẽ tự động thêm các khai báo này khi Đại ca đặt tên cho controls
        private System.Windows.Forms.Label lblWelcomeMessage;
        private System.Windows.Forms.Label lblServiceStatus;
        private System.Windows.Forms.Button btnManageRules;
        private System.Windows.Forms.Button btnToggleService; // Nút tùy chọn

    }
}