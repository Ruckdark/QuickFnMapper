#region Using Directives
using QuickFnMapper.Core.Models;
using QuickFnMapper.Core.Services;
using QuickFnMapper.WinForms.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms; // For Keys
using System.Diagnostics; // For Debug
#endregion

namespace QuickFnMapper.WinForms.Controllers
{
    /// <summary>
    /// <para>Controller for the <see cref="IRuleEditorView"/>.</para>
    /// <para>Manages the logic for creating and editing key mapping rules.</para>
    /// <para>Controller cho <see cref="IRuleEditorView"/>.</para>
    /// <para>Quản lý logic để tạo và chỉnh sửa các quy tắc ánh xạ phím.</para>
    /// </summary>
    public class RuleEditorController
    {
        #region Fields
        private readonly IRuleEditorView _view;
        private readonly IKeyMappingService _keyMappingService;
        private KeyMappingRule? _currentRule; // Sửa: Cho phép nullable, sẽ được gán trong StartInitialization
        private bool _isNewRule;
        private readonly MainController _mainController;
        #endregion

        #region Constructors
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RuleEditorController"/> class.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="RuleEditorController"/>.</para>
        /// </summary>
        public RuleEditorController(IRuleEditorView view, IKeyMappingService keyMappingService, MainController mainController)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _keyMappingService = keyMappingService ?? throw new ArgumentNullException(nameof(keyMappingService));
            _mainController = mainController ?? throw new ArgumentNullException(nameof(mainController));

            // Subscribe to view events
            _view.ViewInitialized += OnViewInitializedByView;
            _view.SaveRuleClicked += OnSaveRuleClicked;
            _view.CancelClicked += OnCancelClicked; // OnCancelClicked sẽ gọi _view.CloseEditor()
            _view.SelectedActionTypeChanged += OnSelectedActionTypeChanged;
            Debug.WriteLine("[DEBUG] RuleEditorController: Subscribed to view events.");
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// <para>Prepares the controller with a rule to edit, or for creating a new rule.</para>
        /// <para>The actual data loading to the view happens when the view raises the ViewInitialized event.</para>
        /// <para>Chuẩn bị controller với một quy tắc để chỉnh sửa, hoặc để tạo một quy tắc mới.</para>
        /// <para>Việc tải dữ liệu thực tế vào view xảy ra khi view kích hoạt sự kiện ViewInitialized.</para>
        /// </summary>
        public void PrepareForEditing(KeyMappingRule? ruleToEdit) // Cho phép ruleToEdit là null
        {
            if (ruleToEdit == null)
            {
                _isNewRule = true;
                _currentRule = new KeyMappingRule();
                _currentRule.OriginalKey = new OriginalKeyData(Keys.None);
                _currentRule.TargetActionDetails = new TargetAction();
            }
            else
            {
                _isNewRule = false;
                // Tạo một bản sao để chỉnh sửa
                _currentRule = new KeyMappingRule
                {
                    Id = ruleToEdit.Id,
                    Name = ruleToEdit.Name,
                    IsEnabled = ruleToEdit.IsEnabled,
                    OriginalKey = ruleToEdit.OriginalKey != null ? new OriginalKeyData
                    {
                        Key = ruleToEdit.OriginalKey.Key,
                        Ctrl = ruleToEdit.OriginalKey.Ctrl,
                        Shift = ruleToEdit.OriginalKey.Shift,
                        Alt = ruleToEdit.OriginalKey.Alt,
                        Win = ruleToEdit.OriginalKey.Win
                    } : new OriginalKeyData(Keys.None),
                    TargetActionDetails = ruleToEdit.TargetActionDetails != null ? new TargetAction
                    {
                        Type = ruleToEdit.TargetActionDetails.Type,
                        ActionParameter = ruleToEdit.TargetActionDetails.ActionParameter,
                        ActionParameterSecondary = ruleToEdit.TargetActionDetails.ActionParameterSecondary
                    } : new TargetAction(),
                    Order = ruleToEdit.Order,
                    CreatedDate = ruleToEdit.CreatedDate,
                    LastModifiedDate = ruleToEdit.LastModifiedDate
                };
            }
            // Logic điền dữ liệu vào View sẽ được thực hiện trong OnViewInitializedByView
            // khi View đã sẵn sàng và kích hoạt event ViewInitialized.
        }
        #endregion

        #region Event Handlers from View

        // This method is called when THE VIEW raises its ViewInitialized event
        // Phương thức này được gọi khi VIEW kích hoạt sự kiện ViewInitialized của nó
        private void OnViewInitializedByView(object? sender, EventArgs e) // Sửa: object? sender
        {
            if (_currentRule == null) // Kiểm tra phòng trường hợp PrepareForEditing chưa được gọi hoặc lỗi
            {
                Debug.WriteLine("[WARN] RuleEditorController: _currentRule is null in OnViewInitializedByView. Initializing a new one.");
                _currentRule = new KeyMappingRule();
                _currentRule.OriginalKey = new OriginalKeyData(Keys.None);
                _currentRule.TargetActionDetails = new TargetAction();
                _isNewRule = true;
            }

            PopulateViewWithRuleData();
            _view.PopulateActionTypes(Enum.GetValues(typeof(ActionType)).Cast<ActionType>());
            // Gọi ConfigureActionParameterControls sau khi SelectedActionType đã được gán trong PopulateViewWithRuleData
            _view.ConfigureActionParameterControls(_view.SelectedActionType);
        }

        private void OnSaveRuleClicked(object? sender, EventArgs e)
        {
            if (_currentRule == null)
            {
                _view.ShowErrorMessage("Cannot save rule: current rule data is missing.", "Internal Error");
                return;
            }

            if (!ValidateInput())
            {
                return;
            }

            // Cập nhật _currentRule từ _view
            _currentRule.Name = _view.RuleName;
            _currentRule.IsEnabled = _view.IsRuleEnabled;
            // Quan trọng: Phải tạo OriginalKeyData và TargetAction mới nếu chúng null
            // để tránh lỗi NullReferenceException khi truy cập các thuộc tính con.
            if (_currentRule.OriginalKey == null) _currentRule.OriginalKey = new OriginalKeyData();
            if (_currentRule.TargetActionDetails == null) _currentRule.TargetActionDetails = new TargetAction();

            OriginalKeyData? viewSelectedOriginalKey = _view.SelectedOriginalKey; // Lấy giá trị từ view một lần
            _currentRule.OriginalKey.Key = viewSelectedOriginalKey?.Key ?? Keys.None;
            _currentRule.OriginalKey.Ctrl = viewSelectedOriginalKey?.Ctrl ?? false;
            _currentRule.OriginalKey.Shift = viewSelectedOriginalKey?.Shift ?? false;
            _currentRule.OriginalKey.Alt = viewSelectedOriginalKey?.Alt ?? false;
            _currentRule.OriginalKey.Win = viewSelectedOriginalKey?.Win ?? false;

            _currentRule.TargetActionDetails.Type = _view.SelectedActionType;
            _currentRule.TargetActionDetails.ActionParameter = _view.ActionParameterValue;
            _currentRule.TargetActionDetails.ActionParameterSecondary = _view.ActionParameterSecondaryValue;
            _currentRule.Order = _view.RuleOrder;
            _currentRule.LastModifiedDate = DateTime.UtcNow;

            string paramFromView = _view.ActionParameterValue; // Lấy giá trị từ view
            Debug.WriteLine($"[DEBUG] RuleEditorController.OnSaveRuleClicked: _view.ActionParameterValue BEFORE assignment = '{paramFromView}'");

            _currentRule.TargetActionDetails.ActionParameter = paramFromView; // Gán vào rule
            Debug.WriteLine($"[DEBUG] RuleEditorController.OnSaveRuleClicked: _currentRule.TargetActionDetails.ActionParameter AFTER assignment = '{_currentRule.TargetActionDetails.ActionParameter}'");

            try
            {
                // Sao chép _currentRule ra một object mới để Add hoặc Update,
                // đảm bảo _currentRule không bị thay đổi bởi service nếu có tham chiếu.
                // Hoặc đảm bảo service không giữ tham chiếu đến rule được truyền vào sau khi xử lý.
                // Hiện tại, KeyMappingService.AddRule và UpdateRule có vẻ không giữ tham chiếu.

                if (_isNewRule)
                {
                    _currentRule.Id = Guid.NewGuid(); // Đảm bảo ID mới cho rule mới
                    _currentRule.CreatedDate = DateTime.UtcNow;
                    Debug.WriteLine($"[DEBUG] RuleEditorController: Attempting to Add new rule. ID: {_currentRule.Id}, Name: '{_currentRule.Name}'");
                    _keyMappingService.AddRule(_currentRule); // Chỉ gọi AddRule
                }
                else
                {
                    // Nếu là edit, _currentRule.Id đã có từ PrepareForEditing
                    Debug.WriteLine($"[DEBUG] RuleEditorController: Attempting to Update rule. ID: {_currentRule.Id}, Name: '{_currentRule.Name}'");
                    _keyMappingService.UpdateRule(_currentRule); // Chỉ gọi UpdateRule
                }

                // Sau khi lưu thành công, _isNewRule nên được reset nếu cần,
                // nhưng vì RuleEditorController có thể được tạo mới mỗi lần,
                // nên trạng thái này sẽ tự reset.

                _mainController.RuleSuccessfullySaved(_currentRule); // Thông báo cho MainController
                _view.CloseEditor();                                 // Đóng editor
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine($"[ERROR] RuleEditorController.OnSaveRuleClicked (InvalidOperationException): {ex.Message}");
                _view.ShowErrorMessage(ex.Message, "Save Error");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] RuleEditorController.OnSaveRuleClicked (Exception): {ex.ToString()}");
                _view.ShowErrorMessage($"An unexpected error occurred while saving the rule: {ex.Message}", "Save Error");
            }
        }

        private void OnCancelClicked(object? sender, EventArgs e)
        {
            Debug.WriteLine("[INFO] RuleEditorController.OnCancelClicked: Editor cancel requested by view. Closing editor.");
            _view.CloseEditor(); // Yêu cầu view tự đóng/ẩn đi
                                 // MainController sẽ xử lý việc điều hướng dựa trên event EditorCancelled của View.
        }

        private void OnSelectedActionTypeChanged(object? sender, ActionType selectedType) // Sửa: object? sender
        {
            _view.ConfigureActionParameterControls(selectedType);
            if (selectedType == ActionType.SendMediaKey)
            {
                var mediaKeyNames = new List<string> {
                    "VolumeMute", "VolumeDown", "VolumeUp",
                    "MediaNextTrack", "MediaPreviousTrack",
                    "MediaStop", "MediaPlayPause"
                };
                _view.PopulateMediaKeyParameters(mediaKeyNames);
            }
        }
        public void UnsubscribeViewEvents()
        {
            if (_view != null)
                _view.ViewInitialized -= OnViewInitializedByView;
            _view.SaveRuleClicked -= OnSaveRuleClicked;
            _view.CancelClicked -= OnCancelClicked;
            _view.SelectedActionTypeChanged -= OnSelectedActionTypeChanged;
            Debug.WriteLine("[DEBUG] RuleEditorController: Unsubscribed from view events.");
        }
        #endregion

        #region Private Helper Methods

        private void PopulateViewWithRuleData()
        {
            if (_currentRule == null) return; // Kiểm tra null

            _view.RuleName = _currentRule.Name ?? string.Empty; // Xử lý Name có thể null từ rule
            _view.IsRuleEnabled = _currentRule.IsEnabled;
            _view.SelectedOriginalKey = _currentRule.OriginalKey ?? new OriginalKeyData(Keys.None);
            _view.SelectedActionType = _currentRule.TargetActionDetails?.Type ?? ActionType.None;
            _view.ActionParameterValue = _currentRule.TargetActionDetails?.ActionParameter ?? string.Empty;
            _view.ActionParameterSecondaryValue = _currentRule.TargetActionDetails?.ActionParameterSecondary; // Cho phép null
            _view.RuleOrder = _currentRule.Order;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(_view.RuleName))
            {
                _view.ShowErrorMessage("Rule name cannot be empty.", "Validation Error");
                return false;
            }

            if (_view.SelectedOriginalKey == null || _view.SelectedOriginalKey.Key == Keys.None)
            {
                _view.ShowErrorMessage("An original key must be selected or captured.", "Validation Error");
                return false;
            }

            if (_view.SelectedActionType == ActionType.None)
            {
                _view.ShowErrorMessage("An action type must be selected.", "Validation Error");
                return false;
            }

            if (_view.SelectedActionType == ActionType.RunApplication || _view.SelectedActionType == ActionType.OpenUrl)
            {
                if (string.IsNullOrWhiteSpace(_view.ActionParameterValue))
                {
                    _view.ShowErrorMessage("Action parameter (e.g., path or URL) cannot be empty for the selected action type.", "Validation Error");
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}