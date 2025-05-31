using QuickFnMapper.Core.Models;
using QuickFnMapper.Core.Services;
using QuickFnMapper.WinForms.Themes;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace QuickFnMapper.WinForms.Utils
{
    public class ThemeManager
    {
        private readonly IAppSettingsService _appSettingsService;
        private ColorPalette _currentPalette;
        private string _currentThemeName = string.Empty;
        private bool _isInitialThemeApplied = false;

        public event EventHandler? ThemeChanged;

        public ThemeManager(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService ?? throw new ArgumentNullException(nameof(appSettingsService));
            // Load theme lần đầu khi khởi tạo
            ForceLoadAndApplyTheme();
        }

        public ColorPalette CurrentPalette => _currentPalette;
        public string CurrentThemeName => _currentThemeName;

        // Gọi khi muốn chắc chắn theme được load và áp dụng, bất kể tên có đổi hay không
        public void ForceLoadAndApplyTheme()
        {
            AppSettings settings = _appSettingsService.LoadSettings();
            string newThemeName = settings.ApplicationTheme ?? "SystemDefault";
            _currentThemeName = newThemeName; // Cập nhật tên theme hiện tại

            Debug.WriteLine($"[ThemeManager] Force loading and applying theme: {_currentThemeName}");

            switch (_currentThemeName.ToUpperInvariant())
            {
                case "DARK":
                    _currentPalette = AppThemes.DarkTheme;
                    break;
                case "LIGHT":
                    _currentPalette = AppThemes.LightTheme;
                    break;
                case "SYSTEMDEFAULT":
                default:
                    _currentPalette = AppThemes.SystemDefaultTheme;
                    break;
            }
            _isInitialThemeApplied = true;
            OnThemeChanged();
        }

        // Gọi khi có khả năng theme đã thay đổi (ví dụ sau khi settings được lưu)
        public void CheckAndApplyTheme()
        {
            AppSettings settings = _appSettingsService.LoadSettings();
            string newThemeName = settings.ApplicationTheme ?? "SystemDefault";

            if (_currentThemeName != newThemeName || !_isInitialThemeApplied)
            {
                ForceLoadAndApplyTheme(); // Nếu tên theme thay đổi hoặc chưa áp dụng lần nào, thì load và áp dụng
            }
            else
            {
                Debug.WriteLine($"[ThemeManager] Theme '{newThemeName}' already set. No change needed.");
            }
        }


        protected virtual void OnThemeChanged()
        {
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ApplyThemeToFormOrUserControl(Control rootControl)
        {
            if (rootControl == null || (_currentPalette.FormBackground == Color.Empty && _currentPalette.TextColor == Color.Empty)) // Kiểm tra palette có vẻ "trống" không
            {
                Debug.WriteLine($"[ThemeManager] ApplyThemeToFormOrUserControl skipped: rootControl is null or CurrentPalette seems uninitialized. Current Theme: '{_currentThemeName}'");
                return;
            }

            rootControl.BackColor = _currentPalette.FormBackground;
            rootControl.ForeColor = _currentPalette.TextColor;
            ApplyThemeToNestedControls(rootControl.Controls, _currentPalette);
        }

        public void ApplyThemeToNestedControls(Control.ControlCollection controls, ColorPalette palette)
        {
            if (controls == null) return;

            foreach (Control control in controls)
            {
                control.ForeColor = palette.TextColor;

                if (control is Button button)
                {
                    button.BackColor = palette.ButtonBackground;
                    button.ForeColor = palette.ButtonText;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = palette.ControlBorder;
                    button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(palette.ButtonBackground, 0.2f);
                    button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(palette.ButtonBackground, 0.2f);
                }
                else if (control is Label || control is CheckBox || control is RadioButton)
                {
                    control.BackColor = Color.Transparent;
                }
                else if (control is GroupBox gb)
                {
                    gb.ForeColor = palette.HeaderTextColor;
                    // Để GroupBox có nền trong suốt với UserControl cha
                    // gb.BackColor = Color.Transparent; 
                    // Hoặc nếu muốn có nền riêng:
                    // gb.BackColor = palette.FormBackground; // Hoặc một màu panel
                }
                else if (control is TextBoxBase textBox) // Bao gồm TextBox, RichTextBox
                {
                    textBox.BackColor = palette.ControlBackground;
                    textBox.ForeColor = palette.TextColor;
                    if (textBox is TextBox txtBox) txtBox.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (control is ListBox listBox)
                {
                    listBox.BackColor = palette.ControlBackground;
                    listBox.ForeColor = palette.TextColor;
                }
                else if (control is ListView listView)
                {
                    listView.BackColor = palette.ControlBackground;
                    listView.ForeColor = palette.TextColor;
                    Debug.WriteLine($"[ThemeManager] Applied theme to ListView '{listView.Name}'. BackColor: {palette.ControlBackground}, ForeColor: {palette.TextColor}");

                    // Nếu muốn header có màu đồng bộ (cần OwnerDraw = true và xử lý sự kiện DrawColumnHeader)
                    // listView.OwnerDraw = true; 
                    // if (!this.eventHandlersAttachedToListView.Contains(listView.Name + "_DrawColumnHeader")) // Tránh gắn nhiều lần
                    // {
                    //    listView.DrawColumnHeader += ListView_DrawColumnHeader_Dynamic;
                    //    this.eventHandlersAttachedToListView.Add(listView.Name + "_DrawColumnHeader");
                    // }

                    listView.Refresh();
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.BackColor = palette.ControlBackground;
                    comboBox.ForeColor = palette.TextColor;
                    comboBox.FlatStyle = FlatStyle.Flat;
                }
                else if (control is NumericUpDown numericUpDown)
                {
                    numericUpDown.BackColor = palette.ControlBackground;
                    numericUpDown.ForeColor = palette.TextColor;
                }
                else if (control is MenuStrip menuStrip)
                {
                    menuStrip.BackColor = palette.MenuStripBackground;
                    menuStrip.ForeColor = palette.MenuStripText;
                    // Gán Renderer nếu muốn tùy chỉnh sâu màu sắc hover/selected
                    // menuStrip.Renderer = new ToolStripProfessionalRenderer(new ThemeColorTable(palette));
                    foreach (ToolStripItem item in menuStrip.Items)
                    {
                        ApplyThemeToToolStripItem(item, palette, menuStrip.Renderer);
                    }
                }
                else if (control is ToolStrip toolStrip)
                {
                    toolStrip.BackColor = palette.MenuStripBackground;
                    toolStrip.ForeColor = palette.MenuStripText;
                    // toolStrip.Renderer = new ToolStripProfessionalRenderer(new ThemeColorTable(palette));
                    foreach (ToolStripItem item in toolStrip.Items)
                    {
                        ApplyThemeToToolStripItem(item, palette, toolStrip.Renderer);
                    }
                }
                else if (control is StatusStrip statusStrip)
                {
                    statusStrip.BackColor = palette.MenuStripBackground;
                    statusStrip.ForeColor = palette.MenuStripText;
                    // statusStrip.Renderer = new ToolStripProfessionalRenderer(new ThemeColorTable(palette));
                    foreach (ToolStripItem item in statusStrip.Items)
                    {
                        ApplyThemeToToolStripItem(item, palette, statusStrip.Renderer);
                    }
                }
                else if (control is Panel panel)
                {
                    // Panel thường là container, để nó có màu nền của form cha là hợp lý
                    panel.BackColor = palette.FormBackground;
                    // Nếu panel cần màu riêng (vd: pnlSideBar) thì phải xử lý riêng.
                }
                else
                {
                    // Các control ít phổ biến hơn có thể cần xử lý màu riêng ở đây
                    // Mặc định, thử set BackColor theo ControlBackground
                    control.BackColor = palette.ControlBackground;
                }

                if (control.HasChildren)
                {
                    ApplyThemeToNestedControls(control.Controls, palette);
                }
            }
        }

        private void ApplyThemeToToolStripItem(ToolStripItem item, ColorPalette palette, ToolStripRenderer renderer)
        {
            if (item == null) return;

            item.ForeColor = palette.DropDownText; // Màu chữ cho item trong dropdown

            // Set màu nền cho item nếu nó không dùng hình ảnh (để tránh ghi đè icon)
            // if (item.DisplayStyle != ToolStripItemDisplayStyle.Image && item.DisplayStyle != ToolStripItemDisplayStyle.ImageAndText)
            // {
            //     item.BackColor = palette.DropDownBackground; // Màu nền cho từng item
            // }


            if (item is ToolStripMenuItem menuItem)
            {
                menuItem.ForeColor = palette.MenuStripText; // Màu chữ cho item cấp 1 của MenuStrip
                if (menuItem.HasDropDown)
                {
                    menuItem.DropDown.BackColor = palette.DropDownBackground;
                    if (menuItem.DropDown is ToolStripDropDown dropDownMenu)
                    {
                        dropDownMenu.Renderer = renderer; // Đồng bộ renderer
                        dropDownMenu.ForeColor = palette.DropDownText; // Đảm bảo chữ trong dropdown có màu
                    }
                    foreach (ToolStripItem dropDownItem in menuItem.DropDownItems)
                    {
                        // Đệ quy cho các item con trong dropdown, truyền renderer của parent
                        ApplyThemeToToolStripItem(dropDownItem, palette, renderer);
                    }
                }
            }
            else if (item is ToolStripButton tsButton)
            {
                tsButton.ForeColor = palette.MenuStripText;
            }
            else if (item is ToolStripStatusLabel tsLabel)
            {
                tsLabel.ForeColor = palette.MenuStripText;
            }
            else if (item is ToolStripLabel tsGenLabel) // Cho các ToolStripLabel khác
            {
                tsGenLabel.ForeColor = palette.MenuStripText;
            }
        }
    }

    // Nếu muốn tùy chỉnh sâu hơn màu sắc hover, selected cho MenuStrip/ToolStrip,
    // có thể tạo một class ThemeColorTable : ProfessionalColorTable
    // public class ThemeColorTable : ProfessionalColorTable { /* ... override các thuộc tính màu ... */ }
}