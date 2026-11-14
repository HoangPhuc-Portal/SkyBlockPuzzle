> **Cập nhật Unity MCP:** đã xác minh AssetDatabase; xem [09_UNITY_VERIFICATION_REPORT.md](09_UNITY_VERIFICATION_REPORT.md) và JSON 06–08. Phần dưới là baseline static trước khi kết nối; các giới hạn “chưa có MCP” chỉ áp dụng lần quét trước. Chưa chạy Play Mode.

# Audit Sky Block Puzzle — 14/11/2026

CHỈ ĐỌC: working tree hiện tại, không sửa/xóa Assets/meta/import/scene/prefab/code. Không có Unity MCP; chưa gọi AssetDatabase API hoặc chạy runtime. Không thêm công cụ Editor. Graph GUID/fileID text/meta + atlas folder + Resources + chuỗi C# không bao phủ hết binary/package/loader ghép chuỗi/reflection. CẦN KIỂM TRA THỦ CÔNG các trường hợp chưa rõ. A=có serialized reference, không khẳng định đang hiển thị; B=code/resource có khả năng dùng; C=chưa thấy reference, KHÔNG an toàn xóa mặc định. D trùng lặp là nhãn độc lập.

695 asset không tính meta; 350 nguồn ảnh/texture (128.09 MiB); A/B/C={'C': 161, 'A': 160, 'B': 29}. 57 nhóm trùng byte/114 file. 0 nhóm pixel giống khác byte; 352 cặp dHash gần chưa review thị giác.

Sprite-imported sources: 289; Multiple sheets: 52; internalID records: 1039. Không phải tổng Sprite sub-asset Unity đã resolve.

## Ứng viên C: 161 file / 62.91 MiB

Không thấy reference GUID/atlas hoặc code path/token/prefix cụ thể. Mọi file dưới đây còn thiếu Unity API/runtime/external-loader verification. Rủi ro: phá workflow art hoặc code động/binary. Giữ và chờ duyệt; 0 byte được phép xóa. Số dung lượng là nguồn, không phải APK/VRAM.

| File | Dung lượng | Đề xuất |
|---|---:|---|
| `Assets/_Project/_Assets/sprite/blockblast_assets/piece_tray_background.png` | 3.07 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/blockblast_assets/background_sky_fantasy.png` | 3.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Animations/dragon_fly_spritesheet.png` | 2.53 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandGameover/giao dien gmaeover .png` | 2.49 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/Bộ giao diện Sky Block Puzzle 3D.png` | 2.44 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/_Menu/Menu chính Sky Block Puzzle bay bổng.png` | 2.29 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/spriteBlocks/Bảng ý tưởng giao diện game xếp khối 3D 1.png` | 2.14 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/spriteBlocks/Bảng ý tưởng giao diện game xếp khối 3D.png` | 2.14 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/spriteBlocks/gpt-image-2-724bd9f2a28d0819f15fdd865a7a05c7 1.png` | 2.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/Wood.png` | 1.82 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/blockblast_assets/board_base.png` | 1.78 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/01_SettingsPanel/settings_panel_settings.png` | 1.66 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/01_SettingsPanel/settings_panel_empty.png` | 1.64 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/05_SpriteSheets/panel_transparent.png` | 1.64 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Animations/thac_nuoc_transparent.png` | 1.56 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/spriteBlocks/gpt-image-2-e3a3c8b32d75b6b5be1ba0bbce36c025 1.png` | 1.51 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/blockblast_assets/piece_slot_empty.png` | 1.50 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandGameover/icon.png` | 1.37 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/spriteBlocks/Bộ tài sản giao diện game puzzle 1.png` | 1.36 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/blockblast_assets/fantasy_lamp_left.png` | 1.26 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/blockblast_assets/fantasy_lamp_right.png` | 1.21 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Animations/waterfall_spritesheet.png` | 1.13 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/blockblast_assets/block_green_1x1.png` | 1.13 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/blockblast_assets/block_blue_1x1.png` | 1.10 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/blockblast_assets/block_purple_1x1.png` | 1.09 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/blockblast_assets/block_yellow_1x1.png` | 1.08 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/blockblast_assets/block_red_1x1.png` | 1.06 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/05_SpriteSheets/buttons_sheet_transparent.png` | 1.02 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/BlockBlast_Art_SpriteSheet.png` | 0.97 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/slider.png` | 0.96 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/icon.png` | 0.83 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/BlockBlast_Art_SpriteSheet.png` | 0.73 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/grass03.png` | 0.69 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/05_SpriteSheets/icons_sheet_transparent.png` | 0.59 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/05_SpriteSheets/sliders_toggles_sheet_transparent.png` | 0.51 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/TrayFrame.png` | 0.46 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_yellow_close.png` | 0.40 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/HUD_Banner.png` | 0.38 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/sprite/spriteBlocks/board-removebg-preview.png` | 0.37 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_blue_replay.png` | 0.31 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_blue_home.png` | 0.31 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_blue_navigation.png` | 0.30 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block3Highlighted.png` | 0.15 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Highlighted.png` | 0.15 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block1Normal.png` | 0.15 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_28_Normal.png` | 0.15 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block4Normal.png` | 0.15 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Normal.png` | 0.15 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block6Normal.png` | 0.15 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block7Normal.png` | 0.15 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block4Highlighted.png` | 0.15 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Highlighted.png` | 0.15 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block5Normal.png` | 0.15 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Normal.png` | 0.15 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block0Normal.png` | 0.14 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Normal.png` | 0.14 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block3Normal.png` | 0.14 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Normal.png` | 0.14 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block5Highlighted.png` | 0.13 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Highlighted.png` | 0.13 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block0Highlighted.png` | 0.13 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Highlighted.png` | 0.13 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block1Highlighted.png` | 0.12 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_28_Highlighted.png` | 0.12 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block2Normal.png` | 0.12 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Normal.png` | 0.12 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_round_blue.png` | 0.11 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block2Highlighted.png` | 0.10 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Highlighted.png` | 0.10 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_settings_gear.png` | 0.09 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/TrophyIcon.png` | 0.09 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/SettingsIcon.png` | 0.09 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_home.png` | 0.09 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_crystal.png` | 0.08 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/decoration_crystal_gold.png` | 0.08 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_restart.png` | 0.07 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/TrophyIcon.png` | 0.07 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block6Disabled.png` | 0.07 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block7Disabled.png` | 0.07 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block1Disabled.png` | 0.07 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_28_Disabled.png` | 0.07 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Prop_Lantern.png` | 0.07 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/CrownIcon.png` | 0.07 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Button_Settings_Blue.png` | 0.07 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block3Disabled.png` | 0.06 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Disabled.png` | 0.06 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block4Disabled.png` | 0.06 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Disabled.png` | 0.06 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/SettingsIcon.png` | 0.06 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block5Disabled.png` | 0.06 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Disabled.png` | 0.06 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block0Disabled.png` | 0.06 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Disabled.png` | 0.06 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block2Disabled.png` | 0.06 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Disabled.png` | 0.06 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/CrownIcon.png` | 0.06 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/GemCrest.png` | 0.05 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Prop_Daisy.png` | 0.05 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block6Highlighted.png` | 0.05 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/UI/AntigravityGen/Block7Highlighted.png` | 0.05 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/OctagonBrick1.png` | 0.04 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/OctagonBrick2.png` | 0.04 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Purple_Normal.png` | 0.04 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Pink_Normal.png` | 0.04 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Blue_Normal.png` | 0.04 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/cell_invalid.png` | 0.04 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Green_Normal.png` | 0.04 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Cyan_Normal.png` | 0.04 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/BrickBlock1.png` | 0.04 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Yellow_Normal.png` | 0.04 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Orange_Normal.png` | 0.04 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/BrickBlock2.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Green_Highlighted.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Pink_Disabled.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Purple_Disabled.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Cyan_Disabled.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Blue_Disabled.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/White_Normal.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Orange_Disabled.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Yellow_Disabled.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Green_Disabled.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Purple_Highlighted.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Pink_Highlighted.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Black_Highlighted.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Blue_Highlighted.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Black.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Black_Normal.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/White_Disabled.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Red_Normal.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Orange_Highlighted.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Cyan_Highlighted.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/SpeedBlock.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/cell_empty.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Red_Disabled.png` | 0.03 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Black_Disabled.png` | 0.02 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/Cylinder.png` | 0.02 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Yellow_Highlighted.png` | 0.02 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/Red_Highlighted.png` | 0.02 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/Flipboard.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/WorkBlock4.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/WorkBlock1.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/WorkBlock6.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/Star.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/WorkBlock5.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/_Assets/White_Highlighted.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/WorkBlock2.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/WorkBlock3.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/Lever.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/Fan.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/Checkpoint.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/OctagonWork4.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/OctagonWork1.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/OctagonWork5.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/OctagonWork6.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/OctagonWork2.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/OctagonWork3.png` | 0.01 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/BlueBlock1.png` | 0.00 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/BlueBlock2.png` | 0.00 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/GreenCube1.png` | 0.00 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/GreenCube2.png` | 0.00 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |
| `Assets/_Project/Materials/Textures/MovingBlock.png` | 0.00 MiB | CẦN KIỂM TRA THỦ CÔNG; xác nhận art nguồn rồi kiểm tra toàn scene/UI/gameplay trước đợt dọn dẹp |

## Nhóm B cần giữ

- `Assets/_Project/_Assets/Background.png`: Assets/_Project/Editor/GameplayUIInstaller.cs, Assets/_Project/Editor/IslandEnvironmentInstaller.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs, Assets/_Project/Editor/UIArtVariants.cs, Assets/_Project/Scripts/UI/SettingsPanel.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayEditModeChecks.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- `Assets/_Project/_Assets/BackgroundSoft.png`: Assets/_Project/Editor/UIArtVariants.cs
- `Assets/_Project/_Assets/Cyan.png`: Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- `Assets/_Project/_Assets/Green.png`: Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- `Assets/_Project/_Assets/Orange.png`: Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- `Assets/_Project/_Assets/Pink.png`: Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- `Assets/_Project/_Assets/Purple.png`: Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- `Assets/_Project/_Assets/Red.png`: Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- `Assets/_Project/_Assets/ScorePlate.png`: Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayEditModeChecks.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- `Assets/_Project/_Assets/StoneFrame.png`: Assets/_Project/Editor/GameplayUIInstaller.cs
- `Assets/_Project/_Assets/TraySlot.png`: Assets/_Project/Editor/GameplayUIInstaller.cs
- `Assets/_Project/_Assets/White.png`: Assets/_Project/Editor/ReferenceGameplayArt.cs
- `Assets/_Project/_Assets/Yellow.png`: Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- `Assets/_Project/_Assets/SkyIslandGameplay/CloudBank.png`: Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- `Assets/_Project/_Assets/SkyIslandGameplay/CloudCorner.png`: Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- `Assets/_Project/_Assets/SkyIslandGameplay/CloudCumulus.png`: Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- `Assets/_Project/_Assets/SkyIslandGameplay/CloudWisps.png`: Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- `Assets/_Project/_Assets/SkyIslandGameplay/gameplay.png`: Assets/_Project/Editor/IslandEnvironmentInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- `Assets/_Project/_Assets/SkyIslandGameplay/GrassTuft.png`: Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- `Assets/_Project/_Assets/SkyIslandMainMenu/menu_fix.png`: Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- `Assets/_Project/_Assets/UI/AntigravityGen/BackgroundSoft.png`: Assets/_Project/Editor/UIArtVariants.cs
- `Assets/_Project/_Assets/UI/AntigravityGen/BlockWhite.png`: Assets/_Project/Editor/GameplayUIInstaller.cs, Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- `Assets/_Project/_Assets/UI/AntigravityGen/CellBeveled.png`: Assets/_Project/Editor/GameplayUIInstaller.cs
- `Assets/_Project/_Assets/UI/AntigravityGen/Panel.png`: Assets/_Project/Editor/GameOverInstaller.cs, Assets/_Project/Editor/GameplayUIInstaller.cs
- `Assets/_Project/_Assets/UI/AntigravityGen/SquareParticle.png`: Assets/_Project/Editor/GameplayUIInstaller.cs
- `Assets/_Project/_Assets/UI/AntigravityGen/StoneFrame.png`: Assets/_Project/Editor/GameplayUIInstaller.cs
- `Assets/_Project/_Assets/UI/AntigravityGen/TraySlot.png`: Assets/_Project/Editor/GameplayUIInstaller.cs
- `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/AdImages/320x480.png`: 
- `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/AdImages/480x320.png`: 