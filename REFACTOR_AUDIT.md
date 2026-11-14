# REFACTOR AUDIT — SKY BLOCK PUZZLE

Ngày khảo sát: 13/11/2026. Repository: `C:/unity/BlockBlast`. Baseline HEAD: `7db7dd6` — Hoàn thành cơ bản về giao diện cho Mainmenu và Gameplay, chưa hoàn thành popup của các button.

## 1. Phạm vi, phương pháp và giới hạn

Đây là báo cáo chỉ đọc; file duy nhất chủ động tạo là `REFACTOR_AUDIT.md`. Không chạy installer, refresh/compile, test Play Mode, build, đổi scene, save scene, thay PlayerPrefs, sửa asset/package/meta hoặc Git. Yêu cầu audit của TASK 01 được ưu tiên so với hướng dẫn triển khai UI trong AGENTS.md. Chỉ tìm thấy AGENTS.md ở gốc trong lần kiểm kê.

Đã kiểm kê đệ quy Assets, đọc manifest/lockfile, cấu hình build và Git; quét GUID, hash nội dung ảnh/audio/model, cấu trúc YAML scene/prefab; đọc các luồng code chính và tìm kiếm trên code runtime. Unity MCP đã kết nối: MainMenu đang mở, Edit Mode, không compiling, scene không dirty tại thời điểm truy vấn. Các đoạn C# khảo sát chạy trong bộ nhớ và chỉ đọc AssetDatabase/SerializedObject. Không coi việc thực thi đoạn khảo sát là build/compile toàn bộ game.

Giới hạn: chưa duyệt từng animation frame bằng mắt; chưa mở GamePlay để kiểm tra tất cả serialized property trực tiếp; chưa kiểm tra build Android, native SDK, FPS, GC, RAM hoặc GPU overdraw thực tế. Static GUID tồn tại không chứng minh subasset fileID, animation binding và logic callback đều đúng. Reference null hợp lệ/tùy chọn không tự động là lỗi. Kết quả PASS từ phiên trước không được tính là test chạy trong audit này.

Git ban đầu có thay đổi chưa commit: `Assets/Scenes/GamePlay.unity`, `Assets/Scenes/MainMenu.unity`, `Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF - Fallback.asset`, `UserSettings/AI.Pbr/MaterialGeneratorSettings.asset`; ảnh mới và meta `Assets/_Project/_Assets/SkyIslandSetting/Bộ giao diện Sky Block Puzzle 3D.png`. Không hoàn nguyên các thay đổi này.

## 2. Tổng quan cấu trúc hiện tại

### Kiểm kê file (không tính .meta)

| Loại | Số lượng toàn Assets |
|---|---:|
| PNG / JPEG | 352 / 1 |
| C# | 116 |
| Asset serialized (.asset, không phải tất cả đều là ScriptableObject gameplay) | 62 |
| Prefab | 40 |
| Material / shader / shadergraph | 14 / 20 / 4 |
| AnimationClip / AnimatorController | 8 / 6 |
| MP3 / FBX | 5 / 4 |
| Scene / SpriteAtlas / asmdef | 5 / 1 / 2 |
| DLL / AAR | 15 / 1 |

`Assets/_Project` có 561 file không tính meta: `_Assets` 366, `Scripts` 56, `Materials` 34, `Editor` 30, `ScriptableObjects` 20, `Prefabs` 18, `Environment` 13, `Animations` 11, `Audio` 5, `Tests` 5, `Resources` 2, `Art` 1. Số file trong thư mục bao gồm nhiều loại; không đồng nhất số file với số object runtime.

Ngoài `_Project`: `GoogleMobileAds`, `ExternalDependencyManager`, `Plugins`, `TextMesh Pro`, `Settings`, `TutorialInfo`, `Scenes`. Đây có thể là vendor hoặc cấu hình hợp lệ; không di chuyển hàng loạt.

- Unity `6000.3.24f1`, URP `17.3.0`, UGUI `2.0.0`, Input System `1.20.0`, Test Framework `1.6.0` theo file cấu hình.
- Hai scene enabled trong EditorBuildSettings: `Assets/Scenes/MainMenu.unity`, `Assets/Scenes/GamePlay.unity`. Loading là overlay của `PlaySceneLoader`, không có Loading scene riêng. Không coi đó là thiếu scene.
- Các scene còn lại: `Assets/Scenes/TestQC.unity`, hai scene trong `Assets/_Project/Environment/Backups`.
- MainMenu YAML: 86 record GameObject, 2 PrefabInstance, 2 Canvas. GamePlay: 148 record GameObject, 66 PrefabInstance, 1 Canvas. Đây là số record YAML, không phải tổng hierarchy sau khi expand prefab/runtime.
- MainMenu live có 8 root: Main Camera, Directional Light, Background, Canvas, EventSystem, AdsManager, AudioManager, MainMenuEnvironment.
- GamePlay có HUD, SettingsPopup, DesignCoordinates, BoardRegion, TrayPanel, Flower và AuthoredCells. Board hiện dùng GridBoard 8×8 và danh sách 64 cell authored; tray có 3 slot. Không đề xuất thay luật board.
- Runtime code còn nằm trong `Assets/_Project/_Assets/SkyIslandMainMenu/Scripts`; tooling layout nằm trong `SkyIslandGameplay/Editor`, ngoài nhóm `Assets/_Project/Editor`.

### Dependency chính

`MainMenu → PlaySceneLoader → GamePlay`; `TrayManager → ShapeData/ShapeRuntimeData + HandSelector + DifficultyConfig`; `DraggableShape → GridBoard → BoardModel`; `GridBoard → GameEvents → GameManager/GameHUD/AudioManager/ClearPresentation/BubblePopEffectPlayer`; `GameOverController ↔ Tray/Board/HUD/Hint/Settings/RewardedGameplay`; `RewardedGameplay → RewardedProvider → AdsManager → GoogleMobileAds/UMP`; `SettingsPanel/MenuSettings → AudioManager + PlayerPrefs`.

AssetDatabase.GetDependencies của hai scene chính trả về hợp 244 đường dẫn (recursive, bao gồm dependency và root). Đây không phải toàn bộ tài nguyên cần ship: `Resources.Load`, bootstrap, plugin native, editor installers và cấu hình build tạo các đường sử dụng khác.

## 3. Bằng chứng và phân loại vấn đề

### 3.1 Kiểm tra lỗi đã thực hiện

- Không phát hiện GUID trùng hoặc meta mồ côi trong Assets ở lần quét filesystem.
- Một số file con trong `.xcframework` và `.androidlib` không có meta riêng; không quy kết lỗi vì đây là nội dung native bundle/plugin.
- Tất cả GUID không phải built-in được nhắc trong hai scene chính đều phân giải bằng Unity AssetDatabase. Quét chỉ Assets ban đầu báo nhiều GUID UGUI/URP/TMP thiếu; đã loại cách kết luận đó vì package chứa các GUID này.
- Live MainMenu: không có Missing Script; không thấy ObjectReference có instance ID khác 0 nhưng object null khi duyệt serialized components, gồm inactive.
- Prefab thuộc `_Project`: duyệt components cả inactive, không phát hiện Missing Script. Chưa khẳng định toàn bộ reference nội bộ các prefab hoặc mọi scene đều sạch.
- Console đọc 20 warning/error gần nhất trả về một entry `NoSubscription` từ `https://generators.ai.unity.com`. Đây là lỗi dịch vụ Unity AI đang tồn tại, chưa có bằng chứng do gameplay gây ra. Không sửa bằng cách gỡ package trong audit.
- Những lỗi HUD/Settings/Score của các phiên trước đã có sửa chữa trong lịch sử; audit hiện tại không tái hiện chúng, không liệt kê là bug đang xảy ra.

### 3.1b Phát hiện bổ sung sau quét package và xác minh live GUID

**Dữ liệu reference lỗi đã tồn tại; chưa sửa và chưa tái hiện triệu chứng runtime:**

- `Assets/_Project/_Assets/SkyIslandGameplay/Prefabs/GameplayPiece.prefab:63`: Image.m_Material trỏ GUID `a7013bf7fb3e5a94592bb5b61dd60191`, AssetDatabase.GUIDToAssetPath trả rỗng. Cùng GUID trong `Assets/_Project/Prefabs/UI/BlockTileUI.prefab` và `DraggableShape.prefab`. Đây là missing material serialized, không phải Missing Script; vì vậy phép quét script prefab vẫn trả sạch. Cần xác minh override trong scene và default-material fallback trước sửa.
- `Assets/Settings/DefaultVolumeProfile.asset:56,251,868,977` có m_Script fileID0 với tên/type test như CopyPasteTestComponent2, VolumeComponentSupportedEverywhere, CopyPasteTestComponent1. GUID `60f3b30c03e6ba64d9a27dc9dba8f28d` ở dòng78 (OutlineVolumeComponent), cùng `74955a4b0b4243bc87231e8b59ed9140`, `5a00a63fdd6bd2a45ab1f2d869305ffd`, `0fd9ee276a1023e439cf7a9c393195fa` không phân giải live. Cần kiểm tra profile được pipeline nào sử dụng; không xóa component hàng loạt chỉ từ tên test.
- `Assets/Settings/PC_Renderer.asset` còn 7 GUID không tìm thấy trong Assets/PackageCache; đã xác nhận live một GUID đại diện `03cfc4915c15d504a9ed85ecc404e607` trả rỗng. Sáu GUID còn lại cần AssetDatabase xác minh riêng; chưa kết luận tất cả là renderer feature đang active.
- Backup `Environment/Backups/GamePlay_BeforeTask01.unity` có GUID `cf4f0c80386bb4c4e96c48ff616ff4ed` không phân giải. Không xem backup là bản restore chắc chắn chạy được.
- AdMob Editor ConsentForm prefab có GUID `f795a5541737a4c09a571dc953a23ba5`; placeholder ads có `f70555f144d8491a825f0804e09c671c`, đều không phân giải live. Cần phân loại field và đối chiếu bản vendor trước repair; không patch plugin trực tiếp trong refactor game.

Bổ sung ưu tiên **P1, Trung bình, rủi ro Trung bình–Cao**: lập danh sách owner/field/dependency reachability cho các lỗi reference này; đề xuất sửa từng asset sau duyệt. Kiểm thử: Scene/Prefab Inspector không Missing, material/UI không đổi ngoài ý muốn, URP Android và Editor ads mock hoạt động. Validator TASK02 phải kiểm tra dangling references của prefab và asset cấu hình, không chỉ Missing Script.

### 3.2 Danh sách refactor theo thứ tự đơn giản → phức tạp

P0 = làm rõ baseline trước mọi thay đổi; P1 = ưu tiên sớm; P2 = sau khi có regression; P3 = chỉ sau đo đạc. Rủi ro bên dưới là rủi ro khi triển khai đề xuất, không phải khẳng định game đang lỗi.

| ID / Thứ tự | Hiện trạng, bằng chứng và đường dẫn | Giải pháp đề xuất | Khó / Rủi ro | Kiểm tra sau thực hiện | Ưu tiên |
|---|---|---|---|---|---|
| A01 / 1 | Working tree đang dirty; 13 file UserSettings đang tracked; `.gitignore` đã bỏ Library/Temp/Builds nhưng chưa bỏ UserSettings. Nhiều backup/Captures ngoài Assets; git ls-files nhóm này cộng UserSettings trả 220 đường dẫn. | Lập baseline diff được chủ dự án chấp thuận, phân loại source/tool output/backup; đề xuất chính sách Git và LFS riêng. Không tự untrack hoặc rewrite history. | Dễ / Thấp với báo cáo, Trung bình nếu đổi tracking | Clone sạch, import đúng phiên bản Unity; kiểm tra meta và diff, bảo đảm không mất file nguồn | P0 |
| A02 / 2 | Naming pha trộn `_Assets`, `_Menu`, `_Model3D`, `sprite`, tên tiếng Việt có khoảng trắng; scene chính GamePlay nhưng TestQC và code string có tên cố định. | Viết naming convention và bảng old→new; di chuyển qua Unity từng nhóm nhỏ, giữ GUID. Tách naming mỹ thuật khỏi schema dữ liệu. | Dễ–Trung bình / Trung bình | So sánh GUID trước/sau; tìm path string, animation binding, Resources key và build scene | P2 |
| A03 / 3 | UI authored và UI runtime cùng tồn tại. `SettingsPanel.cs:26–57,98` có BuildNavigation clone nút và đổi anchor nếu thiếu refs. `GameplayLayoutAuthoring.cs:12–40` tự quan sát toàn canvas mỗi 0.15s; `MenuDesignScale.cs:5` ExecuteAlways chỉnh scale. | Chốt scene/prefab là nguồn bố cục tĩnh; validator references; tách migration dựng UI khỏi Awake sau khi migrate đầy đủ. Giữ dynamic piece/VFX runtime. Preview editor cần chỉ xử lý thay đổi liên quan và Undo rõ ràng. | Trung bình / Cao nếu làm đồng loạt | Edit/Play tại cùng aspect; sửa Inspector rồi Play; Settings/Hint, 64 cell/3 slot; không duplicate UI | P1 |
| A04 / 4 | GameplayCell, GameplayPiece nằm trong SkyIslandGameplay/Prefabs; các prefab cũ nằm trong Prefabs/UI và Prefabs/Shapes. Scene có nhiều instance và override. | Kiểm kê ownership/override; prefab hóa từng khối ổn định (HUD, popup) sau khi xác nhận refs, không Apply All hoặc unpack hàng loạt. Flower giữ dưới nhóm thiết kế, dưới popup, không raycast. | Trung bình / Trung bình–Cao | Prefab variant/scene overrides không mất; popup luôn trên trang trí; drag mapping không đổi | P1 |
| A05 / 5 | Hash byte phát hiện nhiều ảnh trùng; model lớn; backup scene trong Assets; có candidate thiếu incoming GUID tĩnh. | Dùng dependency graph kết hợp Resources/path string/build report/animation/plugin để quyết định; trước mắt chỉ manifest candidate. Hợp nhất từng nhóm sau duyệt. | Trung bình / Cao nếu xóa nhầm | So ảnh và import settings, subasset IDs, all-scenes+build regression; có mapping/rollback | P2 |
| A06 / 6 | BoardModel, GameHUD, Difficulty và nhiều editor check có câu lệnh dồn một dòng. `GameHUD.cs` tên field score lại biểu diễn record, highScore biểu diễn điểm ván. | Format riêng một patch; làm rõ tên domain và comment. Khi đổi serialized field dùng migration/FormerlySerializedAs, không đổi cả hành vi. | Dễ / Thấp với format; Trung bình với rename | Compile + diff chỉ format; score tween/record sau rename vẫn đúng | P1 |
| A07 / 7 | `DragonMascotChecks.cs:37` tìm path `Canvas/SafeArea/HUD/ReferenceWoodHUD` và `Canvas/SafeArea/TrayPanel`, khác hierarchy hiện tại. Tests NUnit dùng reflection đến Assembly-CSharp-Editor (`GameplayUITests.cs:9`). | Cập nhật cơ chế discovery bằng component/serialized fixture; tách validator không mutation khỏi scenario Play tests; sau đó mới bổ sung asmdef runtime/editor/test. | Trung bình / Trung bình | Test tìm đúng object cả khi đổi hierarchy; không tự save/delete scene khi audit; tests chạy batch sau khi được duyệt | P1 |
| A08 / 8 | `ReferenceGameplayInstaller.cs:75–105` áp layout và SaveScene/SaveAssets; `ShapeLibraryRebuild.cs:89` DeleteAsset. Tool dựng một lần còn chung menu công cụ hàng ngày. | Nhóm Migration/Legacy rõ ràng, dry-run và precondition; idempotency, Undo/backup, thông báo phạm vi trước mutation. | Trung bình / Trung bình | Chạy dry-run không đổi hash; chạy lặp không tạo trùng; không vô tình reset UI do artist chỉnh | P1 |
| A09 / 9 | GridBoard vừa model binding, spawn authored/fallback, preview, placement, clear, events. BoardModel.PlaceAndClear và GridBoard.ProcessCompletedLines có hai nơi xử lý quy tắc clear. | Đóng băng contract placement-result và score; thêm characterization tests, rồi hợp nhất thuật toán clear vào model; view chỉ render snapshot. Không thay đổi 8×8, intersect dedup hay score order. | Khó / Cao | Row/column/cross/16-lines; invalid drop không mutate; preview và Hint đồng thuận; score/combo/perfect clear y hệt baseline | P2 |
| A10 / 10 | TrayManager phụ thuộc singleton/config; HandSelector clone board, tạo hand/path trong nhiều vòng; Hint/GameOver đều cần xét nước đi. | Tách service lựa chọn hand và query placement dùng chung dữ liệu immutable; RNG seed/replay trace; giữ weight và distribution. | Rất khó / Cao | Fixed-seed snapshots, 41 hướng, diagonal, no-move, 3-piece viability, thống kê độ khó; CPU trên máy yếu | P3 |
| A11 / 11 | SettingsPanel và MenuSettings lặp key UI.Music/UI.SFX/UI.Haptic; AudioManager thêm Enabled keys; GameManager đọc/ghi/save record trực tiếp mỗi record mới (`GameManager.cs:86–90`). | Preferences facade dùng nguyên key/default cũ; tách policy lưu khỏi UI; cân nhắc debounce chỉ sau đo và kiểm tra lifecycle. | Trung bình / Cao với save migration | Cài mới, save cũ, record đồng bộ, pause/quit/crash window, Home/Replay; không reset người dùng | P2 |
| A12 / 12 | AudioManager persistent, 2 source BGM/SFX, subscription có guard; đang là phần nền tảng hợp lý. Refs AudioSource bị dereference trực tiếp trong Awake. | Giữ singleton ownership; thêm validation refs, catalog clip/mixer chỉ nếu có nhu cầu, thống nhất setting UI. | Trung bình / Trung bình | Scene roundtrip không nhân đôi nhạc/listener; mute/slider, overlap SFX, pause/ads resume | P2 |
| A13 / 13 | AdsManager gộp bootstrap, consent, queue main thread, retry/deadline, banner, rewarded, scene lifecycle. Đã có generation token và RewardedRequest: không nên bỏ. `AdsConfiguration.asset` useTestAds=1, productionApproved=0; iOS units trống. | Sau test baseline, tách consent, banner và rewarded lifecycle từng phần sau facade hiện có; test adapter giữ semantics reward exactly-once và stale callback. Không bật production trong refactor. | Rất khó / Rất cao | Offline→online, load/show fail, close trước reward, callback muộn, scene unload, replay/revive, privacy, test device native | P3 |
| A14 / 14 | `Packages/manifest.json` unity-mcp theo #main, AI assistant prerelease; có navigation/multiplayer/visual scripting nhưng chưa chứng minh cần/không cần. GoogleMobileAds + EDM + Plugins nằm ngoài UPM. | Lập package ownership/version matrix; pin MCP revision sau thử ở bản sao; rà soát package bằng dependency/build. Không gỡ chỉ vì không thấy script gọi. | Trung bình / Cao | Clean import, resolver, Gradle, Android test ads, editor tooling; giữ lockfile đồng bộ | P2 |
| A15 / 15 | Ba FBX HUD/frame/Tray khoảng 65.87/63.44/61.56 MiB; frame color texture17.76MiB. Đây là kích thước nguồn, chưa phải RAM/build. Chỉ 1 Gameplay.spriteatlas. | Dùng BuildReport/Memory Profiler đo tài nguyên được ship/load; cân nhắc loại source3D khỏi build, atlas theo lifetime, Android texture format sau test chất lượng. | Khó / Trung bình–Cao | RAM peak, texture memory, cold load, text/alpha/sprite edges, máy GPU thấp; không lấy dung lượng PNG làm RAM | P3 |
| A16 / 16 | BubblePopEffectPlayer có pool nhưng mỗi clear tạo rows/cols arrays và HashSet (dòng66–68); IslandEnvironment đã giới hạn leaves≤12, dùng MaterialPropertyBlock. | Profile GC trước, reuse scratch buffers khi cần; giữ pool và bounds, tối ưu overdraw UI/VFX theo số đo; không thay hệ animation hàng loạt. | Khó / Trung bình | CPU/GPU frame-time p95, GC.Alloc, 16-line clear, pool reuse, 10 phút chơi và 20 lần restart | P3 |

## 4. Tài nguyên nghi dư thừa — chưa cho phép xóa

| Nhóm cụ thể | Bằng chứng | Điều phải xác minh |
|---|---|---|
| `_Assets/Black.png` và Black_Normal.png; tương tự Blue/Cyan/Green/Orange/Pink/Purple/Red/White/Yellow | SHA-256 nội dung file bằng nhau trong từng cặp | Meta/importer có thể khác, sprite subasset/material/state button khác; kiểm tra mọi consumer |
| `_Assets/cell.png` và cell_empty.png | Nội dung byte bằng nhau | Cell prefab, code fallback, texture type, pixels-per-unit |
| `SkyIslandGameplay/Panel.png`, SquareParticle.png, TraySlot.png và bản cùng tên trong `UI/AntigravityGen` | Nội dung byte bằng nhau | Renderer/UI/VFX reference và importer; không hợp nhất chỉ theo hash |
| `UI/AntigravityGen/BackgroundSoft.png` và `UI/Generated/BackgroundSoft.png` | Nội dung byte bằng nhau | Source generator/installer vẫn có thể cần cả path |
| `_Assets/Background.png`, BackgroundSoft.png, cell_invalid.png; `Animations/dragon_fly_spritesheet.png`, thac_nuoc_transparent.png, waterfall_spritesheet.png | Không có incoming GUID trong tập YAML .unity/.prefab/.asset/.mat/.controller/.anim đã quét | Scan meta spriteatlas/import dependencies, Resources, code AssetDatabase.LoadAssetAtPath, animation/controller và runtime before quyết định |
| `_Assets/_Model3D/HUD`, frame, Tray | File nguồn rất lớn | Cảnh/prefab/material dependencies, future art workflow, build inclusion; dung lượng lớn không có nghĩa không dùng |
| `Environment/Backups/*BeforeTask01.unity`, TestQC.unity | Không enabled trong EditorBuildSettings hiện tại | BuildProfile khác, editor tests, load path, nhu cầu archive; chuyển ngoài Assets cần duyệt riêng |
| Prefabs/UI/GameplayScreen, DraggableShape; Prefabs/Shapes/GridCell so với GameplayCell/GameplayPiece | Có nhiều thế hệ tên/vị trí | Chưa xác minh hình học tương đương; có thể fixtures/installer/fallback; không đánh dấu dead |

Các asset trong Resources phải coi là runtime roots: AdsConfiguration được load ở AdsManager.cs:72; DifficultyConfig có fallback ở TrayManager.cs:43. Plugin native và Editor/Resources của AdMob không áp dụng quy tắc xóa asset game thông thường.

## 5. Package, Android, animation và import

- GoogleMobileAdsDependencies.xml khai báo Android play-services-ads25.4.0 và iOS SDK ~>13.9. Đây là phiên bản khai báo local, không kết luận đã resolve/build thành công hoặc là bản mới nhất.
- AdsManager.cs:135–142 có cấu hình dành cho đối tượng trẻ em, max rating G và under-age consent. Chỉ ghi nhận code hiện có; không kết luận đáp ứng pháp lý/chính sách toàn cầu. Mọi thay đổi release/privacy cần task riêng.
- Android hiện MinSDK25, target SDK automatic, ARM config3, scriptingBackend1 (IL2CPP), minify release1; application identifier còn dạng template `com.UnityTechnologies.com.unity.template.urpblank`. Đây là hạng mục chuẩn bị release, không tự đổi trong refactor.
- TextureImporter đã đọc thực tế: Dragon_Idle 2048×1024, BoardCell78×78; cả hai Read/Write=false, mipmap=false, Android override=false, AutomaticCompressed. Chưa có bằng chứng hai texture này gây lãng phí CPU readable hoặc cần đổi ngay. Kiểm tra cả bộ artwork lớn ở task profiler.
- Animation tồn tại đồng thời clips/controllers và SpriteFrameAnimator/MenuDragonAnimation/DragonMascot. Khi tổ chức lại, lập ownership theo object: không để Animator và coroutine cùng ghi sprite/scale. Không kết luận hiện đang conflict nếu chưa quan sát runtime.
- Một atlas tại `_Assets/UI/Gameplay.spriteatlas`: cần kiểm tra packables, include-in-build, texture residency trước chia/gộp.

## 6. Lộ trình và nhiệm vụ độc lập

1. **Giai đoạn 0 — Baseline (P0):** phê duyệt diff hiện có, ghi phiên bản Unity/package/build, định nghĩa checklist và ảnh baseline. Không cần sửa runtime.
2. **Giai đoạn 1 — Guardrails (P1):** validator scene/prefab chỉ đọc; phân tách menu audit với installer; sửa test discovery cũ trong task riêng. Mỗi phần một thay đổi có thể review.
3. **Giai đoạn 2 — Hygiene:** format một module; naming manifest; quyết định Git/LFS; mỗi lần chỉ move một nhóm đã có dependency map, giữ meta/GUID.
4. **Giai đoạn 3 — UI ownership:** authoring vs runtime, prefab HUD/popup từng phần; camera/canvas/safearea hợp đồng rõ; không thay artwork. Test trước/sau từng prefab.
5. **Giai đoạn 4 — Domain:** BoardModel/placement-result, score và query; rồi difficulty. Không gộp đổi thuật toán cùng format/rename.
6. **Giai đoạn 5 — Services:** preferences, audio, cuối cùng Ads adapter/lifecycle. Giữ nguyên public facade và saved-data keys cho từng migration.
7. **Giai đoạn 6 — Android:** đo build/device baseline, tối ưu bottleneck thực tế; mỗi tối ưu kèm trước/sau. Tách chuẩn bị phát hành khỏi refactor.

### TASK 02 an toàn nhất đề xuất

**“Tạo validator Editor chỉ đọc cho scene/prefab và báo cáo references.”** Chỉ thêm một công cụ Editor sau khi được duyệt, không auto-fix, không save/open scene tự động, không run gameplay/Ads, không ApplyModifiedProperties. Kiểm tra scene đang mở và prefab đọc bằng AssetDatabase: missing scripts, dangling references, required refs HUD/Settings/Score/Tray/Board, 64 cell distinct, 3 tray slots, non-raycast decorations, popup ordering. Phân biệt required-null, optional-null và prefab overrides; xuất kết quả Console hoặc cửa sổ đọc, không tự tạo thêm asset. Với scene chưa mở, đọc serialized dependency và yêu cầu kiểm tra live riêng. Test tính không mutation bằng hash/status trước-sau. Độ khó Trung bình, rủi ro thấp; tạo hàng rào bảo vệ trước cleanup.

Các task độc lập tiếp theo: (a) chỉ format BoardModel; (b) naming/dependency manifest, chưa move; (c) audit prefab override HUD; (d) cập nhật DragonMascotChecks discovery; (e) catalog texture/import/platform; (f) profiler Android baseline; (g) migration preferences có backward compatibility. Mỗi task phải được chấp thuận trước triển khai.

## 7. Rủi ro và quy trình kiểm thử

- **Mất reference:** giữ GUID/meta, snapshot serialized fields/subassets; không Find theo tên vừa rename. Undo không thay thế backup Git/diff.
- **Mất bố cục:** Edit/Play cùng aspect9:16,19.5:9,20:9 và notch; inspector edits phải giữ; Settings/gameover/reward popup trên hoa và VFX; graphic trang trí không chặn raycast.
- **Đổi luật:** placement hợp lệ/không hợp lệ, rotation/diagonal, row/column/intersection, score/combo/perfectclear, Hint và no-move; deterministic fixtures trước khi tách model.
- **Save:** cài mới0, record cũ giữ, vượt record đồng bộ, replay reset round không xóa record, pause/relaunch.
- **Lifecycle:** Menu→loading overlay→GamePlay→Home nhiều lần; không duplicate Ads/Audio/events; reset hủy drag/preview/coroutine/callback cũ.
- **Ads:** fake provider deterministic trong Editor; native test IDs trên thiết bị, reward đúng một lần, đóng sớm không nhận, offline không kẹt input. Không bật production để test.
- **Hiệu suất:** cùng thiết bị/build/settings, đo frame-time p95, peak RAM, GC, cold start, clear tối đa; không tuyên bố tối ưu chỉ theo code smell.
- **Gate mỗi task:** baseline diff → đổi phạm vi nhỏ → compile → tests liên quan → visual/device khi cần → review diff → phê duyệt. Không chạy installer cũ để “sửa nhanh” sau migration.

## 8. Kết luận audit

Đã xác minh một số reference serialized không phân giải được ở prefab/material/volume và asset phụ; chưa xác minh triệu chứng gameplay runtime do chúng gây ra. Các vấn đề có bằng chứng mạnh nhất là ownership UI/editor tooling phân tán, path test cũ, naming/serialized semantics khó hiểu, duplicate byte assets, nguồn model lớn và services nhiều trách nhiệm. Những phần tốt nên giữ: authored cells validation, BoardModel riêng, ShapeRuntimeData, rewarded request/generation guards, VFX pool, giới hạn leaves và event unsubscribe guards. Refactor theo lớp nhỏ, không viết lại game.

Kiểm tra Git cuối audit: tập file modified/untracked có sẵn giữ nguyên theo status; chỉ thêm `REFACTOR_AUDIT.md`. Không có commit, stash hoặc reset.
