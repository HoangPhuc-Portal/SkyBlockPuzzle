# Kiểm kê runtime Play Mode — 14/11/2026

Đã thực sự vào và thoát Play Mode qua Unity MCP. Không sửa/xóa asset hoặc thêm script Editor. Thu thập bằng callback EditorApplication.update chạy trong bộ nhớ, giới hạn 180 giây, lấy SpriteRenderer/UI.Image/RawImage và texture của sharedMaterial trên component active/enabled. Đây là reference runtime, không phải phép chứng minh pixel đang nhìn thấy. Không dùng Resources.FindObjectsOfTypeAll vì Editor đã tải nhiều asset trong kiểm kê trước.

## Kết quả

- 232 observation khác nhau theo scene/object/path/fileID; 69 đường dẫn asset không rỗng.
- 0 đường dẫn nhóm C được phát hiện sử dụng trong phạm vi chạy này: không có.
- Nhóm theo baseline: {'A': 49, 'other': 20}.

## Luồng đã thực hiện

1. GamePlay khởi tạo Board/Tray/môi trường; mở và đóng Settings qua listener thực tế.
2. Tạo trạng thái hàng 0 còn một ô trống trong bộ nhớ runtime; gọi preview và TryPlaceShape thành công, kích hoạt clear/score. Không giả định đây là thao tác chuột thủ công.
3. ConfirmGameOver; xác nhận popup IsPopupVisible=true sau delay. Replay đã gọi trước popup sẵn sàng nên không kết luận Replay đã hoàn tất.
4. Load MainMenu runtime; mở SettingsButton, đóng Close; gọi PlaySceneLoader.Play để chạy reaction/loading/chuyển scene. Kết thúc ở GamePlay.
5. Dừng collector, thoát Play; khôi phục high score trước test, runInBackground và bỏ network override; không lưu scene.

## Giới hạn

Collector lấy mẫu Editor update, có thể bỏ sót frame animation/VFX ngắn. Không thử tất cả màu/shape/combo, Continue/rewarded ads, mọi kích thước màn hình hoặc Android. NetworkOverrideForTests=false bật sau khi vào Play nên không khẳng định ngăn mọi khởi tạo SDK ban đầu; không yêu cầu xem quảng cáo. Không chỉnh slider/toggle hay setting lưu. Một lần gọi SettingsPanel.Open trực tiếp bị compile lỗi vì method private; đã chuyển sang listener Button và mở thành công, không sửa script.

Console trước test có lỗi Unity AI NoSubscription; không xem đó là lỗi gameplay mới. Chưa có cơ sở kết luận 161 ảnh C có thể xóa: vẫn phải giữ art nguồn và xác minh các luồng chưa phủ.

## Asset quan sát theo scene

### GamePlay

- `Assets/_Project/Environment/Art/CloudLeafAtlas.png`
- `Assets/_Project/_Assets/SkyIslandGameover/button.png`
- `Assets/_Project/_Assets/SkyIslandGameover/frame.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/BestPlate.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/BlockBlue.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/BlockCoral.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/BlockPurple.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/BlockWhite.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/BoardCell.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/BoardTraySteps.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/ButtercupPatch.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/DaisyBorder.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/Dragon.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/DragonPerch.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_0.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_1.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_2.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_3.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_4.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_5.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_6.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_1_0.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_1_6.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_2_0.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_2_6.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_3_0.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_3_6.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_0.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_1.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_2.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_3.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_4.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_5.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_6.asset`
- `Assets/_Project/_Assets/SkyIslandGameplay/IslandBackground.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/Panel.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/PieceTray.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/PinkPurpleFlowers.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/ScorePlate.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/Settings.png`
- `Assets/_Project/_Assets/SkyIslandGameplay/cell_highlight.png`
- `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button.png`
- `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_blue_home_with_icon.png`
- `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_blue_replay_with_icon.png`
- `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/frame.png`
- `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/tile_icon_blue.png`
- `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_music.png`
- `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_sound.png`
- `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_vibration.png`
- `Assets/_Project/_Assets/SkyIslandSetting/04_Sliders_Toggles/slider_fill_green.png`
- `Assets/_Project/_Assets/SkyIslandSetting/04_Sliders_Toggles/slider_handle_crystal.png`
- `Assets/_Project/_Assets/SkyIslandSetting/04_Sliders_Toggles/slider_track_blue.png`
- `Assets/_Project/_Assets/SkyIslandSetting/04_Sliders_Toggles/toggle_off.png`
- `Assets/_Project/_Assets/SkyIslandSetting/04_Sliders_Toggles/toggle_on.png`
- `Assets/_Project/_Assets/UI/Generated/BackgroundSoft.png`
- `Assets/_Project/_Assets/UI/Generated/BlockWhite.png`

### MainMenu

- `Assets/_Project/Environment/Art/CloudLeafAtlas.png`
- `Assets/_Project/_Assets/SkyIslandLoading/loading_title_gold.png`
- `Assets/_Project/_Assets/SkyIslandLoading/progress_fill_gold.png`
- `Assets/_Project/_Assets/SkyIslandLoading/progress_track_empty.png`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Dragon_Blink.png`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Dragon_Idle.png`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Dragon_Play_Reaction.png`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Explorer.png`
- `Assets/_Project/_Assets/SkyIslandMainMenu/MenuIsland.png`
- `Assets/_Project/_Assets/SkyIslandMainMenu/PlayButton.png`
- `Assets/_Project/_Assets/SkyIslandMainMenu/SettingsButton.png`
- `Assets/_Project/_Assets/SkyIslandMainMenu/TitleLogo.png`
- `Assets/_Project/_Assets/SkyIslandMainMenu/VFX/SoftLight.png`
- `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_yellow_close_labeled.png`
- `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/frame.png`
- `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/tile_icon_blue.png`
- `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_music.png`
- `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_sound.png`
- `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_vibration.png`
- `Assets/_Project/_Assets/SkyIslandSetting/04_Sliders_Toggles/slider_fill_green.png`
- `Assets/_Project/_Assets/SkyIslandSetting/04_Sliders_Toggles/slider_handle_crystal.png`
- `Assets/_Project/_Assets/SkyIslandSetting/04_Sliders_Toggles/slider_track_blue.png`
- `Assets/_Project/_Assets/SkyIslandSetting/04_Sliders_Toggles/toggle_off.png`
- `Assets/_Project/_Assets/SkyIslandSetting/04_Sliders_Toggles/toggle_on.png`

## Phát hiện bổ sung quan trọng

20 đường dẫn ngoài danh sách ảnh nguồn là Sprite .asset tại SkyIslandGameplay/FrameSlices/Segment_*.asset, được renderer sử dụng runtime: phải giữ. Con số 719 Sprite trước đó chỉ là Sprite lấy từ các đường dẫn FindAssets(t:Texture), không bao gồm đầy đủ Sprite standalone trong .asset; không dùng 719 như tổng toàn bộ Sprite dự án.

Console sau test còn cảnh báo Animator.GotoState: State could not be found. Chưa xác định nguồn/state cụ thể trong đợt audit này; không tự sửa animation. Cần điều tra trước khi dọn clip/controller hoặc sprite sheet. Lỗi NoSubscription đã có trước test.

Kết thúc ở Edit Mode, GamePlay không dirty; HighScore đã khôi phục 210. Không lưu scene hay thay đổi tài nguyên có chủ đích.

## Hoàn tất kiểm kê bổ sung sau gián đoạn

FindAssets(t:Sprite) + LoadAllAssetsAtPath hiện trả 309 đường dẫn và 747 Sprite, bao gồm 20 Sprite standalone FrameSlices. So với snapshot trước: SkyIslandGameover/icon.png hiện có 9 Sprite thay vì 1 (tăng 8); đây là trạng thái nhập asset hiện tại, không do audit chỉnh importer. Vì thế không cộng đơn giản 719 + 20 để mô tả workspace hiện tại.

Console hiện không còn bản ghi Animator.GotoState từ lần chạy trước; không lấy được stack để quy nguyên nhân. Code MenuDragonAnimation sử dụng Idle/Blink/PlayReaction và controller MenuDragon có đủ tên state đó, nên chưa có bằng chứng state bị thiếu ở controller này. Không tự sửa hoặc xóa animation dựa vào cảnh báo. Cần tái hiện riêng nếu muốn xử lý lỗi.

Unity xác nhận hiện ở Edit Mode, GamePlay không dirty. Audit runtime hoàn tất trong phạm vi mô tả; các luồng chưa phủ vẫn là giới hạn, không phải bảo đảm xóa an toàn.
