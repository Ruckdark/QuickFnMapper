using System.Drawing;
using System.Windows.Forms; // Cho SystemColors

namespace QuickFnMapper.WinForms.Themes
{
    public struct ColorPalette // Nên implement IEquatable<ColorPalette> nếu muốn so sánh chính xác
    {
        public string Name { get; set; } // THÊM THUỘC TÍNH NÀY
        public Color FormBackground { get; set; }
        public Color TextColor { get; set; }        // Màu chữ chính
        public Color HeaderTextColor { get; set; }  // Màu chữ cho tiêu đề, header (có thể giống TextColor)
        public Color ControlBackground { get; set; } // Nền cho TextBox, ListView, ComboBox
        public Color ControlBorder { get; set; }
        public Color ButtonBackground { get; set; }
        public Color ButtonText { get; set; }
        public Color MenuStripBackground { get; set; } // Nền riêng cho MenuStrip, ToolStrip, StatusStrip
        public Color MenuStripText { get; set; }       // Chữ riêng cho MenuStrip, ToolStrip, StatusStrip
        public Color DropDownBackground { get; set; }  // Nền cho các menu con xổ xuống
        public Color DropDownText { get; set; }        // Chữ cho các menu con xổ xuống
        public Color SelectedItemBackground { get; set; } // Nền khi item được chọn/hover (nếu có thể set)
        public Color SelectedItemText { get; set; }     // Chữ khi item được chọn/hover
        public Color TextBoxFocusedBackground { get; set; } // THÊM: Màu nền khi TextBox được focus (dùng trong RuleEditorControl)
    }

    public static class AppThemes
    {
        public static ColorPalette LightTheme { get; } = new ColorPalette
        {
            Name = "LightTheme", // GÁN TÊN CHO THEME
            FormBackground = SystemColors.Control,
            TextColor = SystemColors.ControlText,
            HeaderTextColor = SystemColors.ControlText,
            ControlBackground = SystemColors.Window,
            ControlBorder = SystemColors.ControlDark,
            ButtonBackground = SystemColors.Control,
            ButtonText = SystemColors.ControlText,
            MenuStripBackground = SystemColors.Control,
            MenuStripText = SystemColors.ControlText,
            DropDownBackground = SystemColors.Window,
            DropDownText = SystemColors.ControlText,
            SelectedItemBackground = SystemColors.Highlight,
            SelectedItemText = SystemColors.HighlightText,
            TextBoxFocusedBackground = Color.FromArgb(255, 255, 220) // Vàng nhạt
        };

        public static ColorPalette DarkTheme { get; } = new ColorPalette
        {
            Name = "DarkTheme", // GÁN TÊN CHO THEME
            FormBackground = Color.FromArgb(45, 45, 48),
            TextColor = Color.White,
            HeaderTextColor = Color.FromArgb(220, 220, 220),
            ControlBackground = Color.FromArgb(30, 30, 30),
            ControlBorder = Color.FromArgb(80, 80, 80),
            ButtonBackground = Color.FromArgb(63, 63, 70),
            ButtonText = Color.White,
            MenuStripBackground = Color.FromArgb(50, 50, 55),
            MenuStripText = Color.FromArgb(220, 220, 220),
            DropDownBackground = Color.FromArgb(40, 40, 45),
            DropDownText = Color.FromArgb(220, 220, 220),
            SelectedItemBackground = Color.FromArgb(80, 80, 90),
            SelectedItemText = Color.White,
            TextBoxFocusedBackground = Color.FromArgb(50, 50, 0) // Vàng đậm tối
        };

        // SystemDefault có thể phức tạp hơn, hiện tại sẽ trỏ về LightTheme
        public static ColorPalette SystemDefaultTheme
        {
            get
            {
                // Có thể thêm logic để phát hiện theme hệ thống ở đây nếu muốn
                // Hiện tại, trả về LightTheme với tên là "SystemDefault"
                var palette = LightTheme;
                palette.Name = "SystemDefault";
                return palette;
            }
        }
    }
}