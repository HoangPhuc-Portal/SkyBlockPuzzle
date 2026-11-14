# Báo cáo loại bỏ Hint — 14/11/2026

## Kết quả
Đã tiếp tục các thay đổi từ phiên trước, không làm lại UI. Gỡ HintController, nút/counter/popup, cấu hình lượt/cooldown, hiệu ứng pulse riêng của Hint, callback quảng cáo nhận Hint. Cập nhật các installer để không tạo lại Hint và các regression check để không phụ thuộc tính năng đã xóa.

RewardedGameplay vẫn sử dụng provider hiện có cho Continue. Giữ nguyên AdsManager, AdMobRewardedProvider, tính điểm, kéo thả, ghost preview và highlight hàng/cột. BoardModel.CompletedBy được giữ vì còn được hệ thống độ khó sử dụng. Không đổi hành vi Replay hiện tại.

Các sprite đã xóa không có reference GUID trong asset/scene/prefab/animation/atlas, không được code nạp theo tên và không nằm trong Resources. Sprite atlas tham chiếu thư mục sẽ không còn đóng gói các sprite đã xóa. Không xóa atlas hoặc sprite sheet dùng chung.

## File đã xóa (bao gồm meta đi kèm)
- `Assets\_Project\Scripts\UI\HintController.cs`
- `Assets\_Project\Scripts\UI\HintController.cs.meta`
- `Assets\_Project\_Assets\Button_Help_Blue.png`
- `Assets\_Project\_Assets\Button_Help_Blue.png.meta`
- `Assets\_Project\_Assets\HintIcon.png`
- `Assets\_Project\_Assets\HintIcon.png.meta`
- `Assets\_Project\_Assets\UI\AntigravityGen\HintIcon.png`
- `Assets\_Project\_Assets\UI\AntigravityGen\HintIcon.png.meta`
- `Assets\_Project\_Assets\UI\Generated\HintIcon.png`
- `Assets\_Project\_Assets\UI\Generated\HintIcon.png.meta`
- `Assets\_Project\_Assets\UI\ReferenceArt\HintButton.png`
- `Assets\_Project\_Assets\UI\ReferenceArt\HintButton.png.meta`
- `Assets\_Project\_Assets\sprite\hint.png`
- `Assets\_Project\_Assets\sprite\hint.png.meta`

## File đã chỉnh sửa trong nhiệm vụ Hint
- `Assets\Scenes\GamePlay.unity`
- `Assets\_Project\Editor\DifficultyPlayChecks.cs`
- `Assets\_Project\Editor\GameOverInstaller.cs`
- `Assets\_Project\Editor\GameplayUIInstaller.cs`
- `Assets\_Project\Editor\OfflineAdsChecks.cs`
- `Assets\_Project\Editor\ReferenceGameplayArt.cs`
- `Assets\_Project\Editor\ReferenceGameplayInstaller.cs`
- `Assets\_Project\Editor\RewardedGameplayInstaller.cs`
- `Assets\_Project\Editor\ShapeLibraryPlayChecks.cs`
- `Assets\_Project\Editor\UIRegressionChecks.cs`
- `Assets\_Project\Environment\Backups\GamePlay_BeforeTask01.unity`
- `Assets\_Project\Prefabs\UI\GameplayScreen.prefab`
- `Assets\_Project\ScriptableObjects\UITheme.asset`
- `Assets\_Project\Scripts\Shape\DraggableShape.cs`
- `Assets\_Project\Scripts\UI\ButtonFeedback.cs`
- `Assets\_Project\Scripts\UI\GameOverController.cs`
- `Assets\_Project\Scripts\UI\RewardedGameplay.cs`
- `Assets\_Project\Scripts\UI\SettingsPanel.cs`
- `Assets\_Project\Scripts\UI\UITheme.cs`
- `Assets\_Project\_Assets\SkyIslandGameplay\Configs\GameplayTheme.asset`
- `Assets\_Project\_Assets\SkyIslandGameplay\Editor\GameplayArtInstaller.cs`
- `Assets\_Project\_Assets\SkyIslandGameplay\Editor\GameplayEditModeChecks.cs`
- `Assets\_Project\_Assets\SkyIslandGameplay\Editor\GameplayReferenceLayout.cs`

## Kiểm tra thực tế
- Compile Assembly-CSharp bằng Roslyn đi kèm Unity 6000.3.24f1 và response file/dependency của dự án: đạt, 0 lỗi.
- Compile Assembly-CSharp-Editor với assembly runtime vừa build: đạt, 0 lỗi.
- Có 2 warning ở code không chỉnh: ClearPresentation.camera che Component.camera (CS0108); MobileAds.RaiseAdEventsOnUnityMainThread obsolete (CS0618).
- Kiểm tra GamePlay.unity, GameplayScreen.prefab và scene backup: mỗi file loại 61 object/component; không còn local fileID trỏ đến đối tượng vừa xóa.
- Quét Assets: không còn GUID HintController, component/popup/counter và field callback Hint đã xóa. m_LocalEulerAnglesHint là dữ liệu Unity, được giữ nguyên.
- Có cập nhật OfflineAdsChecks để kiểm tra Continue khi ad chưa sẵn sàng, hủy, thất bại, nhận thưởng, callback trùng/muộn, Replay, Settings và scene loading. Chỉ mới compile; CHƯA chạy được bộ test Play Mode.
- Unity batch được gọi thực tế nhưng thoát mã 1 ngay sau khi nhận project path, trước khi thực thi test. Log chưa cho biết nguyên nhân cụ thể. Phiên này không có Unity MCP. Vì vậy chưa xác minh Console/runtime, thao tác chuột hoặc quảng cáo thiết bị thật.
- dotnet build --no-restore không chạy được do thiếu project.assets.json; kết quả compile ở trên dùng compiler của Unity, không dựa vào lệnh dotnet build thất bại.

## Kiểm tra còn cần trong Unity
1. Mở project, đợi import/compile. Nếu Editor vẫn mở từ phiên cũ và auto-refresh đang tắt, dùng Assets > Refresh (hoặc khởi động lại Editor).
2. Mở Assets/Scenes/GamePlay.unity: xác nhận không có Hint/counter/popup; kéo thả, preview, clear, Score/Best vẫn bình thường.
3. Play, chạy Block Blast > Tests > Offline Online Gameplay and Ads (Play Mode). Kiểm tra Continue; Replay; Settings Home; MainMenu → GamePlay.
4. Chạy Block Blast > Tests > Run Line Clear Play Checks. Kiểm tra thiết bị Android riêng cho quảng cáo native.
Không cần gán lại reference bằng tay cho phần Hint đã gỡ.

## Bảo vệ dữ liệu
Bản trước chỉnh sửa nằm tại Captures/HintRemoval/Assets, ngoài thư mục import Assets. Đây là bản sao phục hồi, không được game biên dịch/đóng gói. Giữ nguyên thay đổi trước đó của người dùng tại MainMenu, TMP, UserSettings, các asset mới và các file _Model3D người dùng đã xóa. Không commit/reset/stash. Script kiểm tra Unity tạm thời được giữ ngoài Assets, không thêm hệ thống runtime mới.
