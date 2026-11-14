> **Cập nhật Unity MCP:** đã xác minh AssetDatabase; xem [09_UNITY_VERIFICATION_REPORT.md](09_UNITY_VERIFICATION_REPORT.md) và JSON 06–08. Phần dưới là baseline static trước khi kết nối; các giới hạn “chưa có MCP” chỉ áp dụng lần quét trước. Chưa chạy Play Mode.

# Audit Sky Block Puzzle — 14/11/2026

CHỈ ĐỌC: working tree hiện tại, không sửa/xóa Assets/meta/import/scene/prefab/code. Không có Unity MCP; chưa gọi AssetDatabase API hoặc chạy runtime. Không thêm công cụ Editor. Graph GUID/fileID text/meta + atlas folder + Resources + chuỗi C# không bao phủ hết binary/package/loader ghép chuỗi/reflection. CẦN KIỂM TRA THỦ CÔNG các trường hợp chưa rõ. A=có serialized reference, không khẳng định đang hiển thị; B=code/resource có khả năng dùng; C=chưa thấy reference, KHÔNG an toàn xóa mặc định. D trùng lặp là nhãn độc lập.

695 asset không tính meta; 350 nguồn ảnh/texture (128.09 MiB); A/B/C={'C': 161, 'A': 160, 'B': 29}. 57 nhóm trùng byte/114 file. 0 nhóm pixel giống khác byte; 352 cặp dHash gần chưa review thị giác.

Sprite-imported sources: 289; Multiple sheets: 52; internalID records: 1039. Không phải tổng Sprite sub-asset Unity đã resolve.

## Kế hoạch

1. P0: duyệt baseline Git, tách thay đổi có sẵn. Không tự commit/stash/reset.
2. P0: đề xuất công cụ Editor chỉ đọc GetDependencies/FindAssets/GUIDToAssetPath/LoadAllAssetsAtPath/EditorBuildSettings/atlas để xác minh graph/sub-assets. CHƯA tạo; chờ phê duyệt.
3. P1: review ảnh C lớn nhất báo cáo 02, xác nhận art nguồn cần giữ; chọn 3–5 file mỗi đợt sau runtime verification, không xóa thư mục.
4. P2: dedupe byte sau đối chiếu importer/PPU/pivot/border/slicing/platform/animation/atlas; remap rồi test trước khi xóa.
5. P3: cân nhắc backup/test/art cũ với chủ dự án, không mặc định bỏ.
6. P4: tối ưu Android là task riêng dựa trên profiler.

## Test mỗi đợt

- Import/compile/Console, missing script/reference và GUID/fileID; toàn scene/prefab/variant ngoài build.
- MainMenu/Settings slider/Loading fill/chuyển scene; Dragon Idle/Blink/PlayReaction; nhiều tỷ lệ dọc.
- Board/cell/Tray, drag/ghost/highlight, placement, clear/combo/VFX, Score/Best/Save.
- GameOver/Replay/Continue ads cancel/fail/earn, offline-online, Settings Home, Audio/haptic/môi trường.
- Android build/device; screenshot, Build Report/Memory Profiler trước/sau.
- Rollback chỉ file và meta của đợt từ baseline đã duyệt, không reset repo; commit từng đợt khi được phép.

## Giữ nguyên / rủi ro cao

Resources, AdsConfiguration/DifficultyConfig, GoogleMobileAds/plugin, UITheme/BlockSpriteSet, GameplayCell/GameplayScreen, Dragon sheets/animation, CloudLeafAtlas, atlas và mọi A. B cần loader verification. Sprite giống nhau không hoán đổi trực tiếp. Một số generator dùng đường dẫn _Project/UI hoặc _Project/sprite cũ trong khi art nằm ở _Assets; không chạy generator để sửa trong audit. Không thấy Addressables loader trong code Assets đã quét; module AssetBundle trong manifest không chứng minh đang sử dụng.

## Giới hạn

Không xác nhận ảnh đang hiển thị hay chỉ serialized/disabled. Không gọi Unity API/Play Mode, không xét runtime package binary/external loader đầy đủ. Chưa đo APK/VRAM. Nhóm C và thư mục ứng viên đều cần xác minh trước khi xóa.

PASS: SHA-256 1492 file Assets gồm meta trước/sau giống hệt; không thêm/xóa asset. Chỉ tạo 5 báo cáo docs/refactor.

## Ưu tiên xem xét cụ thể

- P1: `Assets/_Project/_Assets/sprite/blockblast_assets/piece_tray_background.png` — 3.07 MiB; xác nhận mục đích art nguồn; chưa xóa.
- P1: `Assets/_Project/_Assets/sprite/blockblast_assets/background_sky_fantasy.png` — 3.03 MiB; xác nhận mục đích art nguồn; chưa xóa.
- P1: `Assets/_Project/Animations/dragon_fly_spritesheet.png` — 2.53 MiB; xác nhận mục đích art nguồn; chưa xóa.
- P1: `Assets/_Project/_Assets/SkyIslandGameover/giao dien gmaeover .png` — 2.49 MiB; xác nhận mục đích art nguồn; chưa xóa.
- P1: `Assets/_Project/_Assets/SkyIslandSetting/Bộ giao diện Sky Block Puzzle 3D.png` — 2.44 MiB; xác nhận mục đích art nguồn; chưa xóa.
- P1: `Assets/_Project/_Assets/_Menu/Menu chính Sky Block Puzzle bay bổng.png` — 2.29 MiB; xác nhận mục đích art nguồn; chưa xóa.
- P1: `Assets/_Project/_Assets/sprite/spriteBlocks/Bảng ý tưởng giao diện game xếp khối 3D 1.png` — 2.14 MiB; xác nhận mục đích art nguồn; chưa xóa.
- P1: `Assets/_Project/_Assets/sprite/spriteBlocks/Bảng ý tưởng giao diện game xếp khối 3D.png` — 2.14 MiB; xác nhận mục đích art nguồn; chưa xóa.
- P1: `Assets/_Project/_Assets/sprite/spriteBlocks/gpt-image-2-724bd9f2a28d0819f15fdd865a7a05c7 1.png` — 2.01 MiB; xác nhận mục đích art nguồn; chưa xóa.
- P1: `Assets/_Project/Materials/Textures/Wood.png` — 1.82 MiB; xác nhận mục đích art nguồn; chưa xóa.

Tổng C 62.91 MiB; nguồn tiết kiệm lý thuyết, không phải APK/RAM. Không cộng với duplicate. Thư mục ứng viên: Assets/_Project/Materials/Textures, Assets/_Project/_Assets/SkyIslandSetting/01_SettingsPanel, Assets/_Project/_Assets/SkyIslandSetting/05_SpriteSheets.


## Cập nhật Play Mode

Đã chạy kiểm kê runtime; xem [11_PLAYMODE_VERIFICATION_REPORT.md](11_PLAYMODE_VERIFICATION_REPORT.md). Ghi nhận 20 Sprite FrameSlices .asset ngoài danh sách file ảnh nguồn; giữ nguyên. Không có nhóm C được phát hiện thêm trong phạm vi thử. Có cảnh báo Animator.GotoState cần điều tra. Chưa duyệt xóa asset.
