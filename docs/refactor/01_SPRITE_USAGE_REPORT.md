> **Cập nhật Unity MCP:** đã xác minh AssetDatabase; xem [09_UNITY_VERIFICATION_REPORT.md](09_UNITY_VERIFICATION_REPORT.md) và JSON 06–08. Phần dưới là baseline static trước khi kết nối; các giới hạn “chưa có MCP” chỉ áp dụng lần quét trước. Chưa chạy Play Mode.

# Audit Sky Block Puzzle — 14/11/2026

CHỈ ĐỌC: working tree hiện tại, không sửa/xóa Assets/meta/import/scene/prefab/code. Không có Unity MCP; chưa gọi AssetDatabase API hoặc chạy runtime. Không thêm công cụ Editor. Graph GUID/fileID text/meta + atlas folder + Resources + chuỗi C# không bao phủ hết binary/package/loader ghép chuỗi/reflection. CẦN KIỂM TRA THỦ CÔNG các trường hợp chưa rõ. A=có serialized reference, không khẳng định đang hiển thị; B=code/resource có khả năng dùng; C=chưa thấy reference, KHÔNG an toàn xóa mặc định. D trùng lặp là nhãn độc lập.

695 asset không tính meta; 350 nguồn ảnh/texture (128.09 MiB); A/B/C={'C': 161, 'A': 160, 'B': 29}. 57 nhóm trùng byte/114 file. 0 nhóm pixel giống khác byte; 352 cặp dHash gần chưa review thị giác.

Sprite-imported sources: 289; Multiple sheets: 52; internalID records: 1039. Không phải tổng Sprite sub-asset Unity đã resolve.

## Scenes

- Assets/Scenes/GamePlay.unity [BUILD]
- Assets/Scenes/MainMenu.unity [BUILD]
- Assets/Scenes/TestQC.unity [ngoài build]
- Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity [ngoài build]
- Assets/_Project/Environment/Backups/MainMenu_BeforeTask01.unity [ngoài build]

Không có Loading.unity; Loading được điều khiển qua UI/code.

## Atlas

- Assets/_Project/_Assets/UI/Gameplay.spriteatlas: Assets/_Project/_Assets/Blue.png, Assets/_Project/_Assets/SkyIslandGameplay/cell_highlight.png, Assets/_Project/_Assets/UI/Generated, Assets/_Project/_Assets/UI/Generated/BackgroundSoft.png, Assets/_Project/_Assets/UI/Generated/Block0Disabled.png, Assets/_Project/_Assets/UI/Generated/Block0Highlighted.png, Assets/_Project/_Assets/UI/Generated/Block0Normal.png, Assets/_Project/_Assets/UI/Generated/Block1Disabled.png, Assets/_Project/_Assets/UI/Generated/Block1Highlighted.png, Assets/_Project/_Assets/UI/Generated/Block1Normal.png, Assets/_Project/_Assets/UI/Generated/Block2Disabled.png, Assets/_Project/_Assets/UI/Generated/Block2Highlighted.png, Assets/_Project/_Assets/UI/Generated/Block2Normal.png, Assets/_Project/_Assets/UI/Generated/Block3Disabled.png, Assets/_Project/_Assets/UI/Generated/Block3Highlighted.png, Assets/_Project/_Assets/UI/Generated/Block3Normal.png, Assets/_Project/_Assets/UI/Generated/Block4Disabled.png, Assets/_Project/_Assets/UI/Generated/Block4Highlighted.png, Assets/_Project/_Assets/UI/Generated/Block4Normal.png, Assets/_Project/_Assets/UI/Generated/Block5Disabled.png, Assets/_Project/_Assets/UI/Generated/Block5Highlighted.png, Assets/_Project/_Assets/UI/Generated/Block5Normal.png, Assets/_Project/_Assets/UI/Generated/Block6Disabled.png, Assets/_Project/_Assets/UI/Generated/Block6Highlighted.png, Assets/_Project/_Assets/UI/Generated/Block6Normal.png, Assets/_Project/_Assets/UI/Generated/Block7Disabled.png, Assets/_Project/_Assets/UI/Generated/Block7Highlighted.png, Assets/_Project/_Assets/UI/Generated/Block7Normal.png, Assets/_Project/_Assets/UI/Generated/BlockBlast_Art_SpriteSheet.png, Assets/_Project/_Assets/UI/Generated/BlockWhite.png, Assets/_Project/_Assets/UI/Generated/CellBeveled.png, Assets/_Project/_Assets/UI/Generated/CrownIcon.png, Assets/_Project/_Assets/UI/Generated/SettingsIcon.png, Assets/_Project/_Assets/UI/Generated/StoneFrame.png, Assets/_Project/_Assets/UI/Generated/TrophyIcon.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_27_Disabled.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_27_Highlighted.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_27_Normal.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_28_Disabled.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_28_Highlighted.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_28_Normal.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_29_Disabled.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_29_Highlighted.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_29_Normal.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_30_Disabled.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_30_Highlighted.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_30_Normal.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_31_Disabled.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_31_Highlighted.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_31_Normal.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_32_Disabled.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_32_Highlighted.png, Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_32_Normal.png, Assets/_Project/_Assets/UI/ReferenceArt, Assets/_Project/_Assets/UI/ReferenceArt/BackgroundCliffs.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockBlue.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockBlueDisabled.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockBlueHighlighted.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockCyan.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockCyanDisabled.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockCyanHighlighted.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockGreen.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockGreenDisabled.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockGreenHighlighted.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockOrange.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockOrangeDisabled.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockOrangeHighlighted.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockPink.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockPinkDisabled.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockPinkHighlighted.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockPurple.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockPurpleDisabled.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockPurpleHighlighted.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockRed.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockRedDisabled.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockRedHighlighted.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockWhiteDisabled.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockWhiteHighlighted.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockYellow.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockYellowDisabled.png, Assets/_Project/_Assets/UI/ReferenceArt/BlockYellowHighlighted.png, Assets/_Project/_Assets/UI/ReferenceArt/BoardCrest.png, Assets/_Project/_Assets/UI/ReferenceArt/BoardStone.png, Assets/_Project/_Assets/UI/ReferenceArt/CellBlue.png, Assets/_Project/_Assets/UI/ReferenceArt/Crown.png, Assets/_Project/_Assets/UI/ReferenceArt/HUDWood.png, Assets/_Project/_Assets/UI/ReferenceArt/SettingsButton.png, Assets/_Project/_Assets/UI/ReferenceArt/TrayWood.png, Assets/_Project/_Assets/UI_game_nen_trong_suot.png

### `Assets/_Project/Animations/dragon_fly_spritesheet.png`

- .png; (2560, 1280); 2649012 byte; GUID 6eb671c1f06931a4ea82c7a316eddbd8; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4684669211038146883, 1839122967, 1646365737, 889797077, -1929660232, 994171263, -239784079, -866407220, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Animations/thac_nuoc_transparent.png`

- .png; (1536, 1024); 1637822 byte; GUID 4a56a5f11f1dfae48835ecaac988d0a2; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5526641554447052263, 1050254089, 992786427, 1703968605, 318130074, -1703863386, -1553730208, 1271330728, -924048243, 1145040175, 144121424, -1517026368, 176974791, 1436134125, -633983087, -1087363348, 719548095, 1359203061, 10548402, -1912710927, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Animations/waterfall_sheet_transparent.png`

- .png; (1358, 682); 735270 byte; GUID 5cfa6049c9957a54185cddaab6055111; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -3449423529669082991, -2093460893, -1646301987, 670692166, 224258617, -909947050, -865504932, 200914846, -458039223, 606490692, -1256685279, -684545658, -1635811161, 277934076, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/Animations/Warter_2.anim:24: value: {fileID: -2093460893, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`
  - `Assets/_Project/Animations/Warter_2.anim:26: value: {fileID: 224258617, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`
  - `Assets/_Project/Animations/Warter_2.anim:28: value: {fileID: -909947050, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`
  - `Assets/_Project/Animations/Warter_2.anim:30: value: {fileID: -865504932, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`
  - `Assets/_Project/Animations/Warter_2.anim:32: value: {fileID: -458039223, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`
  - `Assets/_Project/Animations/Warter_2.anim:34: value: {fileID: -684545658, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`
  - `Assets/_Project/Animations/Warter_2.anim:36: value: {fileID: -1635811161, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`
  - `Assets/_Project/Animations/Warter_2.anim:38: value: {fileID: 277934076, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`
  - `Assets/_Project/Animations/Warter_2.anim:61: - {fileID: -2093460893, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`
  - `Assets/_Project/Animations/Warter_2.anim:62: - {fileID: 224258617, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`
  - `Assets/_Project/Animations/Warter_2.anim:63: - {fileID: -909947050, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`
  - `Assets/_Project/Animations/Warter_2.anim:64: - {fileID: -865504932, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`
  - `Assets/_Project/Animations/Warter_2.anim:65: - {fileID: -458039223, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`
  - `Assets/_Project/Animations/Warter_2.anim:66: - {fileID: -684545658, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`
  - `Assets/_Project/Animations/Warter_2.anim:67: - {fileID: -1635811161, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`
  - `Assets/_Project/Animations/Warter_2.anim:68: - {fileID: 277934076, guid: 5cfa6049c9957a54185cddaab6055111, type: 3}`

### `Assets/_Project/Animations/waterfall_spritesheet.png`

- .png; (2280, 1680); 1182322 byte; GUID 11662b4143621aa4ca4beb5e894ae172; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6487130751631252196, 91555242, 1757580581, 1567630753, 822305867, -987854895, 777609998, 2126039787, -940861869, -1043552270, -1115471784, 955413015, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Background.png`

- .png; (1080, 1920); 2461730 byte; GUID 4015b740920fa9e4299a4c165aeb15eb; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -1955448553306655712, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs, Assets/_Project/Editor/IslandEnvironmentInstaller.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs, Assets/_Project/Editor/UIArtVariants.cs, Assets/_Project/Scripts/UI/SettingsPanel.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayEditModeChecks.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/BackgroundSoft.png`

- .png; (540, 960); 282150 byte; GUID da10bcfb5dec0b8204dc9302060e8d2e; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -5210908662597136490, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/UIArtVariants.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Black.png`

- .png; (384, 384); 31453 byte; GUID 777c0ffeca267e84696b796314fd1385; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4457835537561387818, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Black_Disabled.png`

- .png; (384, 384); 26201 byte; GUID 081719ef20a0afc362da47c9ad3ec857; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2842438174572793013, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Black_Highlighted.png`

- .png; (384, 384); 33622 byte; GUID dc5c233974617d43e4d73c5bfe16f38d; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 1698428014722011960, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Black_Normal.png`

- .png; (384, 384); 31453 byte; GUID ae076ab0688c2893677a465625ff0605; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6033161406235078333, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/BlockBlast_Art_SpriteSheet.png`

- .png; (2048, 2048); 766598 byte; GUID c17c0c8a65393692391bc0245647cdad; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6547413204877522291, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Blue.png`

- .png; (384, 384); 39347 byte; GUID cd2e075c926835d459c3bf48b22306df; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6429493399158520243, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas:38: - {fileID: 21300000, guid: cd2e075c926835d459c3bf48b22306df, type: 3}`
  - `Assets/_Project/Prefabs/Shapes/BlockCell2D.prefab:85: m_Sprite: {fileID: -6429493399158520243, guid: cd2e075c926835d459c3bf48b22306df, type: 3}`

### `Assets/_Project/_Assets/Blue_Disabled.png`

- .png; (384, 384); 34685 byte; GUID 810d2237cd1bd76c0fd76af1ca75673d; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -4574097796107097778, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Blue_Highlighted.png`

- .png; (384, 384); 32566 byte; GUID cb24853d2c3245ae652b298daca8085b; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 3064788442520437839, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Blue_Normal.png`

- .png; (384, 384); 39347 byte; GUID 16004759de5e3acc8ae934aa369f7ad8; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 1023492407749295439, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Button_Settings_Blue.png`

- .png; (256, 256); 69635 byte; GUID 00359b484c34ff28176ca96c48c6c9e9; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6966269806489202353, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/cell.png`

- .png; (384, 384); 27784 byte; GUID 57aabd969a32f4c4c88ee31ffc3d5c6b; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 36, y: 36, z: 36, w: 36}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -319469171194908686, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/Prefabs/Shapes/Square.prefab:85: m_Sprite: {fileID: -319469171194908686, guid: 57aabd969a32f4c4c88ee31ffc3d5c6b, type: 3}`

### `Assets/_Project/_Assets/cell_empty.png`

- .png; (384, 384); 27784 byte; GUID 065550f6cf6aab848a8de6c86bb61ca4; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 36, y: 36, z: 36, w: 36}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -4013881256306030524, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/cell_invalid.png`

- .png; (384, 384); 39291 byte; GUID 127c8ea8c5bcafe48bc77e33b289392b; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 36, y: 36, z: 36, w: 36}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7295551760566684311, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/CrownIcon.png`

- .png; (256, 256); 59229 byte; GUID c173613bce44bce951c337758a3bb70b; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6777162303297155294, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Cyan.png`

- .png; (384, 384); 38536 byte; GUID 2ca598dccbacb5741943858d964e2d14; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -4003223316803723154, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Cyan_Disabled.png`

- .png; (384, 384); 34732 byte; GUID a62568b73e0dd41802e2a7a0a6202b4b; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5436489924568911226, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Cyan_Highlighted.png`

- .png; (384, 384); 30103 byte; GUID c0daeb190d458c38e4f3aa6b1055e813; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8162384655625054109, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Cyan_Normal.png`

- .png; (384, 384); 38536 byte; GUID 0c1943686672c70dd145f8c0c6136e6e; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 1407129663672141859, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/GemCrest.png`

- .png; (256, 256); 54987 byte; GUID 3cabb2eebaa9f090e75671bff9f7b5ae; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -3444188000909812828, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Green.png`

- .png; (384, 384); 38864 byte; GUID a17df85c2c195344481e5ff9b9daa938; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -4987337320017983735, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Green_Disabled.png`

- .png; (384, 384); 34011 byte; GUID 00ef4ee0ac52e2a92ab7ec340935283b; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6767064771563918700, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Green_Highlighted.png`

- .png; (384, 384); 36546 byte; GUID 34747a28e33f72f14aa3f9b42d7347ea; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7370184018439363424, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Green_Normal.png`

- .png; (384, 384); 38864 byte; GUID 94075f495cd3f37095951310d59a9b75; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -4725367401054891196, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/HUD_Banner.png`

- .png; (1024, 256); 399408 byte; GUID c2cc3c7eb6c5d713bcfb4c020a7baa53; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "200", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 90, y: 40, z: 90, w: 40}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -7293490496272486393, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Orange.png`

- .png; (384, 384); 36782 byte; GUID 812a8aa6864b6504a8c157c120127540; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4121880872312635667, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Orange_Disabled.png`

- .png; (384, 384); 34604 byte; GUID 1227f260f05dd05a6e01582a6c9658fe; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -91033288179824082, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Orange_Highlighted.png`

- .png; (384, 384); 30724 byte; GUID 59c088c03524bcc9946c82d5ccb79f5f; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -5433829232214511269, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Orange_Normal.png`

- .png; (384, 384); 36782 byte; GUID 5cd497760180f3afd2f5cff4f9fa4ecb; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5114776604485948167, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Pink.png`

- .png; (384, 384); 40391 byte; GUID 9980fcb503c89424a8c021b5873219fb; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -7381184880430962403, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Pink_Disabled.png`

- .png; (384, 384); 35084 byte; GUID 512f0af42e22bf8f7982b3cbfd15049f; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5858225575255994816, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Pink_Highlighted.png`

- .png; (384, 384); 33708 byte; GUID 525b08ae5c841915236832e6d99fae08; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 1250952452257296263, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Pink_Normal.png`

- .png; (384, 384); 40391 byte; GUID 32eebe7727bc9f11e825bbe53d690978; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 3598318700420422378, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Prop_Daisy.png`

- .png; (256, 256); 53196 byte; GUID 59995c2a03ee98818312a71780369b33; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 1075805873904803341, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Prop_Lantern.png`

- .png; (256, 256); 70491 byte; GUID a7387ec34973f83b7bb1e0dea1ba0c02; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -3485469161538210096, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Purple.png`

- .png; (384, 384); 40418 byte; GUID bcbf9e0cdeb1bb74bb74e554ca37c378; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8923911374754931897, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Purple_Disabled.png`

- .png; (384, 384); 34861 byte; GUID 5d904bad996eecb8b0a919daa95d1347; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8995909565172676533, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Purple_Highlighted.png`

- .png; (384, 384); 33724 byte; GUID afeb228f43a12e44923522354a0d95d8; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -3219964759802143785, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Purple_Normal.png`

- .png; (384, 384); 40418 byte; GUID f2db91bcfffc9a1eba2e396c0d97f13d; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 3955673075138181904, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Red.png`

- .png; (384, 384); 30851 byte; GUID cb4a82ab7e7a9c74ca42f3254a1b99ea; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6286595657586805340, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Red_Disabled.png`

- .png; (384, 384); 27136 byte; GUID 716f2f75047dddac6b7790d7f434f8cb; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -4878397694552371024, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Red_Highlighted.png`

- .png; (384, 384); 20220 byte; GUID 28c43d54e7d8e87dbe216aed43739fcd; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -4892821750988077366, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Red_Normal.png`

- .png; (384, 384); 30851 byte; GUID 4df2aac0f8a7321b3af8f7e1299963b5; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2303669911562647272, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/ScorePlate.png`

- .png; (512, 144); 95207 byte; GUID 1d309d5032e08ed17ba05fbcd1bfd29c; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "200", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 32, y: 32, z: 32, w: 32}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8499812842609837228, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayEditModeChecks.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SettingsIcon.png`

- .png; (256, 256); 65815 byte; GUID 7e67017ce094d00b30a37d51f3eb7963; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5904627132653970092, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SquareParticle.png`

- .png; (128, 128); 1564 byte; GUID 1a12701880cae55131134e5695a21b2f; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 4, y: 4, z: 4, w: 4}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 2571114976766217928, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:15680: glowSprite: {fileID: 21300000, guid: 1a12701880cae55131134e5695a21b2f, type: 3}`
  - `Assets/_Project/Environment/Prefabs/ComboEnvironment.prefab:50: glowSprite: {fileID: 21300000, guid: 1a12701880cae55131134e5695a21b2f, type: 3}`

### `Assets/_Project/_Assets/StoneFrame.png`

- .png; (1024, 1024); 1314199 byte; GUID 7ef51a7c5a5b2da143190e77b23c45a2; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "400", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 112, y: 112, z: 112, w: 112}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4756602434617575680, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/TrayFrame.png`

- .png; (1024, 384); 480610 byte; GUID 854c467f9870b66f80264d5e7826f320; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "200", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 80, y: 80, z: 80, w: 80}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -1839555883357385291, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/TraySlot.png`

- .png; (512, 512); 13449 byte; GUID 565c3b8436270b0b92bbcc3619f7da96; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "200", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 56, y: 56, z: 56, w: 56}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -9195402912914313453, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/TrophyIcon.png`

- .png; (256, 256); 92250 byte; GUID 591e2d356e91715a286fc542afe8c5b4; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8116845798535481512, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI_game_nen_trong_suot.png`

- .png; (1536, 1024); 2122729 byte; GUID 97b41ebb4203cff49947a7a74f04718b; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -7588745312150869981, -1445307881, -561266521, -593910626, 536285914, -1061504147, -1568885233, 1075435975, -117460364, 1589474471, 378565993, -212249816, 674187894, 1882130584, -1674330599, -546230661, 1767463933, 1132553181, -1154540100, -563556172, -1885538955, 1145828098, -1261212714, -247209638, -1934997368, 1398365254, 788183124, 2036559400, -753808245, -163357735, -422324737, -586681577, 1026209000, -1137345757, -861767217, -894675032, 1556409108, 1703729423, 696409887, -617563351, -511073385, 970046972, -635852736, -929507005, -918753262, 1282975795, 1013274685, -1485102904, 796141557, 2073259095, 1221045509, 2135771038, 2086981205, -1071082624, -410307568, 109132050, -1873251401, 556158438, -1177987914, -1311267651, 1871836095, -801540712, -798915564, 1313826052, 1868447129, -960760407, 209023404, 1887249523, -814311794, -373484836, -400342240, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/BlockSpriteSet.asset:16: - {fileID: 2036559400, guid: 97b41ebb4203cff49947a7a74f04718b, type: 3}`
  - `Assets/_Project/ScriptableObjects/BlockSpriteSet.asset:17: - {fileID: -753808245, guid: 97b41ebb4203cff49947a7a74f04718b, type: 3}`
  - `Assets/_Project/ScriptableObjects/BlockSpriteSet.asset:18: - {fileID: -163357735, guid: 97b41ebb4203cff49947a7a74f04718b, type: 3}`
  - `Assets/_Project/ScriptableObjects/BlockSpriteSet.asset:19: - {fileID: -422324737, guid: 97b41ebb4203cff49947a7a74f04718b, type: 3}`
  - `Assets/_Project/ScriptableObjects/BlockSpriteSet.asset:20: - {fileID: -586681577, guid: 97b41ebb4203cff49947a7a74f04718b, type: 3}`
  - `Assets/_Project/ScriptableObjects/BlockSpriteSet.asset:21: - {fileID: 1026209000, guid: 97b41ebb4203cff49947a7a74f04718b, type: 3}`
  - `Assets/_Project/ScriptableObjects/BubblePopConfig.asset:16: bubbleSprite: {fileID: 696409887, guid: 97b41ebb4203cff49947a7a74f04718b, type: 3}`
  - `Assets/_Project/ScriptableObjects/BubblePopConfig.asset:17: ringSprite: {fileID: -511073385, guid: 97b41ebb4203cff49947a7a74f04718b, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas:36: - {fileID: 2800000, guid: 97b41ebb4203cff49947a7a74f04718b, type: 3}`
  - `Assets/_Project/Prefabs/Blocks/UI_game_nen_trong_suot_32.prefab:84: m_Sprite: {fileID: 1026209000, guid: 97b41ebb4203cff49947a7a74f04718b, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:2386: m_Sprite: {fileID: -798915564, guid: 97b41ebb4203cff49947a7a74f04718b, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:6764: m_Sprite: {fileID: -546230661, guid: 97b41ebb4203cff49947a7a74f04718b, type: 3}`

### `Assets/_Project/_Assets/White.png`

- .png; (384, 384); 34633 byte; GUID e71c37570425c854aa16fabed0e276cc; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6601691336363857274, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/White_Disabled.png`

- .png; (384, 384); 31344 byte; GUID b6088ab22efead09862ccdec92cfb338; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 9109476828645456461, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/White_Highlighted.png`

- .png; (384, 384); 8532 byte; GUID 0a47fd7f001259b4af80921791893d64; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 925880233350935653, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/White_Normal.png`

- .png; (384, 384); 34633 byte; GUID d224edfbfa3fe82d837ad8ab4aa398a4; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 1454718160771148299, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Yellow.png`

- .png; (384, 384); 37329 byte; GUID a7cfb7a112aa6154284b87d5bf4d372a; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6358040702757662528, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Yellow_Disabled.png`

- .png; (384, 384); 34419 byte; GUID 884b01f5112666c3da03893d7e4b5d6c; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -7222191908666977829, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Yellow_Highlighted.png`

- .png; (384, 384); 22788 byte; GUID 7f6e1508dabeeaf9fe6ff81340fa8b7e; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7748272835898221366, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/Yellow_Normal.png`

- .png; (384, 384); 37329 byte; GUID 497bc57d16fc4acf79d147ed18974035; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4717153917656439754, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandGameover/button.png`

- .png; (1254, 1254); 1450162 byte; GUID e21d4efc3174a6f4bb0e16724baa6929; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 2530564911472380330, 263407725, 306372712, -1187579414, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:14483: m_Sprite: {fileID: 2530564911472380330, guid: e21d4efc3174a6f4bb0e16724baa6929, type: 3}`
  - `Assets/Scenes/GamePlay.unity:15120: m_Sprite: {fileID: 263407725, guid: e21d4efc3174a6f4bb0e16724baa6929, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameover/frame.png`

- .png; (1086, 1448); 1631443 byte; GUID c64a786ec99bfb849800b5b911440de2; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7501387830980884940, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:12641: m_Sprite: {fileID: 7501387830980884940, guid: c64a786ec99bfb849800b5b911440de2, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameover/giao dien gmaeover .png`

- .png; (1086, 1448); 2610589 byte; GUID f251032c5f7b5d44c8fe00487caf29c4; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandGameover/icon.png`

- .png; (1254, 1254); 1437659 byte; GUID 8298e4a184353fc4c8be43d68409e20b; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6934086728263231946, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandGameplay/BestPlate.png`

- .png; (380, 135); 67949 byte; GUID f524380a9b2a48919b4d9d06f2873e45; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8242093563612415161, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayEditModeChecks.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:7555: m_Sprite: {fileID: 21300000, guid: f524380a9b2a48919b4d9d06f2873e45, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/BlockBlue.png`

- .png; (297, 297); 88697 byte; GUID da2fd51b395a4750a219e3ef0ff6f598; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -5346708522279743761, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:64: - {fileID: 21300000, guid: da2fd51b395a4750a219e3ef0ff6f598, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:68: - {fileID: 21300000, guid: da2fd51b395a4750a219e3ef0ff6f598, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:74: - {fileID: 21300000, guid: da2fd51b395a4750a219e3ef0ff6f598, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:78: - {fileID: 21300000, guid: da2fd51b395a4750a219e3ef0ff6f598, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:83: - {fileID: 21300000, guid: da2fd51b395a4750a219e3ef0ff6f598, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:87: - {fileID: 21300000, guid: da2fd51b395a4750a219e3ef0ff6f598, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Prefabs/GameplayPiece.prefab:71: m_Sprite: {fileID: 21300000, guid: da2fd51b395a4750a219e3ef0ff6f598, type: 3}`
  - `Assets/_Project/Prefabs/Blocks/UI_game_nen_trong_suot_27.prefab:84: m_Sprite: {fileID: 21300000, guid: da2fd51b395a4750a219e3ef0ff6f598, type: 3}`
  - `Assets/_Project/Prefabs/UI/BlockTileUI.prefab:68: m_Sprite: {fileID: 21300000, guid: da2fd51b395a4750a219e3ef0ff6f598, type: 3}`
  - `Assets/_Project/Prefabs/UI/DraggableShape.prefab:71: m_Sprite: {fileID: 21300000, guid: da2fd51b395a4750a219e3ef0ff6f598, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/BlockCoral.png`

- .png; (297, 297); 91868 byte; GUID 397d729572444a93b27976a54a4f6319; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -5346708522279743761, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:66: - {fileID: 21300000, guid: 397d729572444a93b27976a54a4f6319, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:70: - {fileID: 21300000, guid: 397d729572444a93b27976a54a4f6319, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:76: - {fileID: 21300000, guid: 397d729572444a93b27976a54a4f6319, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:80: - {fileID: 21300000, guid: 397d729572444a93b27976a54a4f6319, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:85: - {fileID: 21300000, guid: 397d729572444a93b27976a54a4f6319, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:89: - {fileID: 21300000, guid: 397d729572444a93b27976a54a4f6319, type: 3}`
  - `Assets/_Project/Prefabs/Blocks/UI_game_nen_trong_suot_29.prefab:84: m_Sprite: {fileID: 21300000, guid: 397d729572444a93b27976a54a4f6319, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/BlockGold.png`

- .png; (296, 296); 93311 byte; GUID 1ac73e5e3791430db3dcc5e2e824c82c; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -1132638290841266987, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:65: - {fileID: 21300000, guid: 1ac73e5e3791430db3dcc5e2e824c82c, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:75: - {fileID: 21300000, guid: 1ac73e5e3791430db3dcc5e2e824c82c, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:84: - {fileID: 21300000, guid: 1ac73e5e3791430db3dcc5e2e824c82c, type: 3}`
  - `Assets/_Project/Prefabs/Blocks/UI_game_nen_trong_suot_28.prefab:84: m_Sprite: {fileID: 21300000, guid: 1ac73e5e3791430db3dcc5e2e824c82c, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/BlockGreen.png`

- .png; (296, 296); 98129 byte; GUID d85c868309b64852acbbdc891a309343; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2399699543274225224, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:67: - {fileID: 21300000, guid: d85c868309b64852acbbdc891a309343, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:77: - {fileID: 21300000, guid: d85c868309b64852acbbdc891a309343, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:86: - {fileID: 21300000, guid: d85c868309b64852acbbdc891a309343, type: 3}`
  - `Assets/_Project/Prefabs/Blocks/UI_game_nen_trong_suot_30.prefab:84: m_Sprite: {fileID: 21300000, guid: d85c868309b64852acbbdc891a309343, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/BlockPurple.png`

- .png; (297, 297); 93562 byte; GUID aa1ca215a70f496c9963e8e8eddd43e6; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -5346708522279743761, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:69: - {fileID: 21300000, guid: aa1ca215a70f496c9963e8e8eddd43e6, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:71: - {fileID: 21300000, guid: aa1ca215a70f496c9963e8e8eddd43e6, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:79: - {fileID: 21300000, guid: aa1ca215a70f496c9963e8e8eddd43e6, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:81: - {fileID: 21300000, guid: aa1ca215a70f496c9963e8e8eddd43e6, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:88: - {fileID: 21300000, guid: aa1ca215a70f496c9963e8e8eddd43e6, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:90: - {fileID: 21300000, guid: aa1ca215a70f496c9963e8e8eddd43e6, type: 3}`
  - `Assets/_Project/Prefabs/Blocks/UI_game_nen_trong_suot_31.prefab:84: m_Sprite: {fileID: 21300000, guid: aa1ca215a70f496c9963e8e8eddd43e6, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/BlockWhite.png`

- .png; (75, 78); 8749 byte; GUID 8f663cffe381366449aadb213f07649b; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -7041519908417514300, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs, Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:61: whiteBlock: {fileID: 21300000, guid: 8f663cffe381366449aadb213f07649b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:61: whiteBlock: {fileID: 21300000, guid: 8f663cffe381366449aadb213f07649b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Prefabs/GameplayCell.prefab:326: m_Sprite: {fileID: 21300000, guid: 8f663cffe381366449aadb213f07649b, type: 3}`
  - `Assets/_Project/Prefabs/Shapes/GridCell.prefab:326: m_Sprite: {fileID: 21300000, guid: 8f663cffe381366449aadb213f07649b, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/BoardCell.png`

- .png; (78, 78); 5119 byte; GUID 5004ff21564647dfb98095bde3d55422; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -4879917664986786212, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:233: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:742: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:955: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:1415: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:1703: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:1841: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:2449: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:2662: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:3012: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:3236: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:3528: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:4251: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:4692: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:5269: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:5657: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:6144: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:6282: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:6529: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:6667: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:6805: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:6943: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:7394: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:7684: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:7822: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:8172: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:8848: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:9061: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:9774: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:9912: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:10050: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:10727: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:11062: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:11245: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:11383: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:11705: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:11843: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:12107: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:12245: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:12383: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:12770: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:13257: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:14233: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:15440: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:15883: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:16021: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:16309: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:16447: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:16585: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:16723: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:16861: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:17317: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:17455: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:17593: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:18065: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:18203: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:19149: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:19521: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:19659: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:19888: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:20026: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:20164: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:20377: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:20731: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/Scenes/GamePlay.unity:20869: objectReference: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/_Project/ScriptableObjects/UITheme.asset:58: cellSprite: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:58: cellSprite: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Prefabs/GameplayCell.prefab:221: m_Sprite: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`
  - `Assets/_Project/Prefabs/Shapes/GridCell.prefab:130: m_Sprite: {fileID: 21300000, guid: 5004ff21564647dfb98095bde3d55422, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/BoardFrame.png`

- .png; (320, 327); 125691 byte; GUID fa5c26c5e3d04184be7e55a9edf3fa90; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -5672258373178653655, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:14875: m_Sprite: {fileID: 21300000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/ScriptableObjects/UITheme.asset:59: frameSprite: {fileID: 21300000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:59: frameSprite: {fileID: 21300000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_0.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_0.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_1.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_1.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_2.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_2.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_3.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_3.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_4.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_4.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_5.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_5.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_6.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_0_6.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_1_0.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_1_0.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_1_6.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_1_6.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_2_0.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_2_0.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_2_6.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_2_6.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_3_0.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_3_0.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_3_6.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_3_6.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_0.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_0.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_1.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_1.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_2.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_2.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_3.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_3.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_4.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_4.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_5.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_5.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_6.asset:29: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/FrameSlices/Segment_4_6.asset:120: texture: {fileID: 2800000, guid: fa5c26c5e3d04184be7e55a9edf3fa90, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/BoardTraySteps.png`

- .png; (443, 335); 239610 byte; GUID 2f44351869f9431eabf1a03a23fca318; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5662795425674271148, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:17789: m_Sprite: {fileID: 21300000, guid: 2f44351869f9431eabf1a03a23fca318, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/ButtercupPatch.png`

- .png; (416, 238); 170679 byte; GUID ac73037ec5ad4d19aa50147d81641a9c; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6286129954904556559, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:10842: m_Sprite: {fileID: 21300000, guid: ac73037ec5ad4d19aa50147d81641a9c, type: 3}`
  - `Assets/Scenes/GamePlay.unity:14942: m_Sprite: {fileID: 21300000, guid: ac73037ec5ad4d19aa50147d81641a9c, type: 3}`
  - `Assets/_Project/Prefabs/Flower/ButtercupPatch.prefab:84: m_Sprite: {fileID: 21300000, guid: ac73037ec5ad4d19aa50147d81641a9c, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/cell_highlight.png`

- .png; (384, 384); 44966 byte; GUID e2536ffae330e7941b08b00f97dd27be; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 36, y: 36, z: 36, w: 36}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7583463172941648353, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas:39: - {fileID: 21300000, guid: e2536ffae330e7941b08b00f97dd27be, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Prefabs/GameplayCell.prefab:84: m_Sprite: {fileID: 21300000, guid: e2536ffae330e7941b08b00f97dd27be, type: 3}`
  - `Assets/_Project/Prefabs/Shapes/GridCell.prefab:235: m_Sprite: {fileID: 21300000, guid: e2536ffae330e7941b08b00f97dd27be, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/CloudBank.png`

- .png; (617, 236); 169601 byte; GUID b1a0e18cfae04b91b76a993896d214f8; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 8106112578033804541, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandGameplay/CloudCorner.png`

- .png; (624, 423); 246041 byte; GUID 7ea36877c95d471195b4c4d1798febc9; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2870095538197106420, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandGameplay/CloudCumulus.png`

- .png; (574, 288); 156848 byte; GUID 7587225bc50e4bbfbc84b202ebf5b12a; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7707970739268529366, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandGameplay/CloudWisps.png`

- .png; (524, 139); 68155 byte; GUID 6e4c2ca51ba248558e0dda488b045576; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2631867351549360480, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandGameplay/DaisyBorder.png`

- .png; (471, 176); 155407 byte; GUID 87341b9072ef4d0e87d133349528d84b; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 3107486366796537349, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:3744: m_Sprite: {fileID: 21300000, guid: 87341b9072ef4d0e87d133349528d84b, type: 3}`
  - `Assets/Scenes/GamePlay.unity:14392: m_Sprite: {fileID: 21300000, guid: 87341b9072ef4d0e87d133349528d84b, type: 3}`
  - `Assets/Scenes/GamePlay.unity:15524: m_Sprite: {fileID: 21300000, guid: 87341b9072ef4d0e87d133349528d84b, type: 3}`
  - `Assets/Scenes/GamePlay.unity:16180: m_Sprite: {fileID: 21300000, guid: 87341b9072ef4d0e87d133349528d84b, type: 3}`
  - `Assets/_Project/Prefabs/Flower/DaisyBorder.prefab:84: m_Sprite: {fileID: 21300000, guid: 87341b9072ef4d0e87d133349528d84b, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/Dragon.png`

- .png; (284, 322); 150819 byte; GUID 9bb3657cf46846aab16f9d31269f8aff; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 1372335161102834277, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:3972: m_Sprite: {fileID: 21300000, guid: 9bb3657cf46846aab16f9d31269f8aff, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/DragonPerch.png`

- .png; (453, 371); 297054 byte; GUID 34b164b2abfb4115b601d073659d404e; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 192879034240686642, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:18509: m_Sprite: {fileID: 21300000, guid: 34b164b2abfb4115b601d073659d404e, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/gameplay.png`

- .png; (941, 1672); 2298952 byte; GUID e3f440d27548bc246acaf00e72ee1beb; nhóm B.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/IslandEnvironmentInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandGameplay/GrassTuft.png`

- .png; (464, 179); 140153 byte; GUID 56e32f0beded4ce5b5a2588a43e8db8e; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -7618035288382876292, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandGameplay/IslandBackground.png`

- .png; (941, 1672); 2644640 byte; GUID 49e3336fd88645a69d817ad3a3712eed; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -1769196586779859317, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:10289: m_Sprite: {fileID: 21300000, guid: 49e3336fd88645a69d817ad3a3712eed, type: 3}`
  - `Assets/Scenes/GamePlay.unity:19752: m_Sprite: {fileID: 21300000, guid: 49e3336fd88645a69d817ad3a3712eed, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/Panel.png`

- .png; (512, 512); 7140 byte; GUID f2038d2f3aa2e1347ad55cdecf6fc421; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "200", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 64, y: 64, z: 64, w: 64}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -1198477067713850731, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameOverInstaller.cs, Assets/_Project/Editor/GameplayUIInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:826: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/Scenes/GamePlay.unity:4923: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/Scenes/GamePlay.unity:10941: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/Scenes/GamePlay.unity:14643: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/Scenes/GamePlay.unity:15599: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/Scenes/GamePlay.unity:17892: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/ScriptableObjects/UITheme.asset:57: panelSprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Prefabs/MenuSettingsPopup.prefab:295: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Prefabs/MenuSettingsPopup.prefab:546: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Prefabs/MenuSettingsPopup.prefab:758: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Prefabs/MenuSettingsPopup.prefab:833: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Prefabs/MenuSettingsPopup.prefab:908: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Prefabs/MenuSettingsPopup.prefab:1059: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Prefabs/MenuSettingsPopup.prefab:1271: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Prefabs/MenuSettingsPopup.prefab:1663: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Prefabs/MenuSettingsPopup.prefab:1742: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Prefabs/MenuSettingsPopup.prefab:1886: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:57: panelSprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:68: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:1304: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:1762: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:2042: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:2117: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:3016: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:3310: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:3385: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:3460: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:3545: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:4656: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:4881: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:5623: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:5702: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:5921: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:6012: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:6087: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:6299: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:429: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:1458: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:1609: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:1921: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:2209: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:2625: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:3186: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:3668: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:4124: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:5215: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:5306: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:5466: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:5560: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:5793: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:5972: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:6059: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:6363: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:6715: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:6981: m_Sprite: {fileID: 21300000, guid: f2038d2f3aa2e1347ad55cdecf6fc421, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/PieceTray.png`

- .png; (394, 157); 90989 byte; GUID e24f4d3d0aa9455a9e13b618c1e8fd9a; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6885569818145921803, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:12536: m_Sprite: {fileID: 21300000, guid: e24f4d3d0aa9455a9e13b618c1e8fd9a, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/PinkPurpleFlowers.png`

- .png; (440, 259); 202582 byte; GUID 01ddcb4a3e7a4e85a49755f83b7905d1; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6500288083995007447, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:1039: m_Sprite: {fileID: 21300000, guid: 01ddcb4a3e7a4e85a49755f83b7905d1, type: 3}`
  - `Assets/Scenes/GamePlay.unity:1574: m_Sprite: {fileID: 21300000, guid: 01ddcb4a3e7a4e85a49755f83b7905d1, type: 3}`
  - `Assets/Scenes/GamePlay.unity:10598: m_Sprite: {fileID: 21300000, guid: 01ddcb4a3e7a4e85a49755f83b7905d1, type: 3}`
  - `Assets/Scenes/GamePlay.unity:11467: m_Sprite: {fileID: 21300000, guid: 01ddcb4a3e7a4e85a49755f83b7905d1, type: 3}`
  - `Assets/Scenes/GamePlay.unity:15017: m_Sprite: {fileID: 21300000, guid: 01ddcb4a3e7a4e85a49755f83b7905d1, type: 3}`
  - `Assets/_Project/Prefabs/Flower/PinkPurpleFlowers.prefab:84: m_Sprite: {fileID: 21300000, guid: 01ddcb4a3e7a4e85a49755f83b7905d1, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/ScorePlate.png`

- .png; (380, 201); 94482 byte; GUID 3aa3be512223409bbdb3462a2b136f24; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8499812842609837228, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayEditModeChecks.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:10448: m_Sprite: {fileID: 21300000, guid: 3aa3be512223409bbdb3462a2b136f24, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/Settings.png`

- .png; (248, 249); 85428 byte; GUID 35d9d604bc8a443e945437e693584467; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7886631795410796208, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayEditModeChecks.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:18347: m_Sprite: {fileID: 21300000, guid: 35d9d604bc8a443e945437e693584467, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/SquareParticle.png`

- .png; (128, 128); 2161 byte; GUID 40a1c646542ddda449e3cd9d892fde11; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 4, y: 4, z: 4, w: 4}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 2571114976766217928, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:62: squareParticle: {fileID: 21300000, guid: 40a1c646542ddda449e3cd9d892fde11, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:62: squareParticle: {fileID: 21300000, guid: 40a1c646542ddda449e3cd9d892fde11, type: 3}`

### `Assets/_Project/_Assets/SkyIslandGameplay/TraySlot.png`

- .png; (512, 512); 12610 byte; GUID cb4f653326f219140921bd302feb21e1; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "200", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 56, y: 56, z: 56, w: 56}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -9195402912914313453, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:6368: m_Sprite: {fileID: 21300000, guid: cb4f653326f219140921bd302feb21e1, type: 3}`
  - `Assets/Scenes/GamePlay.unity:11544: m_Sprite: {fileID: 21300000, guid: cb4f653326f219140921bd302feb21e1, type: 3}`
  - `Assets/Scenes/GamePlay.unity:18912: m_Sprite: {fileID: 21300000, guid: cb4f653326f219140921bd302feb21e1, type: 3}`
  - `Assets/_Project/ScriptableObjects/UITheme.asset:60: slotSprite: {fileID: 21300000, guid: cb4f653326f219140921bd302feb21e1, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:60: slotSprite: {fileID: 21300000, guid: cb4f653326f219140921bd302feb21e1, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:2462: m_Sprite: {fileID: 21300000, guid: cb4f653326f219140921bd302feb21e1, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:5025: m_Sprite: {fileID: 21300000, guid: cb4f653326f219140921bd302feb21e1, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:6451: m_Sprite: {fileID: 21300000, guid: cb4f653326f219140921bd302feb21e1, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:2285: m_Sprite: {fileID: 21300000, guid: cb4f653326f219140921bd302feb21e1, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:3736: m_Sprite: {fileID: 21300000, guid: cb4f653326f219140921bd302feb21e1, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:7374: m_Sprite: {fileID: 21300000, guid: cb4f653326f219140921bd302feb21e1, type: 3}`

### `Assets/_Project/_Assets/SkyIslandLoading/loading_title_gold.png`

- .png; (648, 120); 70546 byte; GUID aa0a604609cf9834aac22f8bdab3fd9f; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5257289788633876091, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/MainMenu.unity:7086: m_Sprite: {fileID: 5257289788633876091, guid: aa0a604609cf9834aac22f8bdab3fd9f, type: 3}`

### `Assets/_Project/_Assets/SkyIslandLoading/progress_fill_gold.png`

- .png; (1056, 182); 20890 byte; GUID 2b741c335f3d888408c5826c151d847c; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6757402031518685000, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/MainMenu.unity:6936: m_Sprite: {fileID: 6757402031518685000, guid: 2b741c335f3d888408c5826c151d847c, type: 3}`

### `Assets/_Project/_Assets/SkyIslandLoading/progress_track_empty.png`

- .png; (1056, 182); 29146 byte; GUID 6287d991b63e2ea4595b5e6703f3431d; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 2402613193211741223, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/MainMenu.unity:1950: m_Sprite: {fileID: 2402613193211741223, guid: 6287d991b63e2ea4595b5e6703f3431d, type: 3}`

### `Assets/_Project/_Assets/SkyIslandMainMenu/Dragon_Blink.png`

- .png; (1774, 887); 2427499 byte; GUID 62f89bc5808d4834684f81b10267d831; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 2326664460026951475, 1444801788, 1413881091, -908998132, -1041404544, 800176819, -1431038352, 129425062, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:114: value: {fileID: 2326664460026951475, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:116: value: {fileID: 1444801788, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:118: value: {fileID: 1413881091, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:120: value: {fileID: -908998132, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:122: value: {fileID: 129425062, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:124: value: {fileID: -1431038352, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:126: value: {fileID: 800176819, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:128: value: {fileID: -1041404544, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:130: value: {fileID: -1041404544, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:180: - {fileID: 2326664460026951475, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:181: - {fileID: 1444801788, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:182: - {fileID: 1413881091, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:183: - {fileID: -908998132, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:184: - {fileID: 129425062, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:185: - {fileID: -1431038352, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:186: - {fileID: 800176819, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:187: - {fileID: -1041404544, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonBlink.anim:188: - {fileID: -1041404544, guid: 62f89bc5808d4834684f81b10267d831, type: 3}`

### `Assets/_Project/_Assets/SkyIslandMainMenu/Dragon_Idle.png`

- .png; (3072, 1536); 3809639 byte; GUID 47d01a48de8d5ab46aa532618515fa1f; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4609173563387012312, -355127050, 177361730, -1136792054, -391398102, -752233971, -2045567293, 2050552617, -213360274, 1820458599, 93634826, 356436231, -1668344661, 2066976582, 1117202942, -921587470, -443571439, -773683508, -2101696836, 243879074, -1726118701, 52513866, -529234311, 1141033822, 734043391, -503812346, 1588965202, 1572796212, -723506763, -630029233, -552961186, -1141714026, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/MainMenu.unity:6008: m_Sprite: {fileID: 4609173563387012312, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:114: value: {fileID: 4609173563387012312, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:116: value: {fileID: -355127050, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:118: value: {fileID: 177361730, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:120: value: {fileID: -1136792054, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:122: value: {fileID: -391398102, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:124: value: {fileID: -752233971, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:126: value: {fileID: -2045567293, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:128: value: {fileID: 2050552617, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:130: value: {fileID: -213360274, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:132: value: {fileID: 1820458599, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:134: value: {fileID: 93634826, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:136: value: {fileID: 356436231, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:138: value: {fileID: -1668344661, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:140: value: {fileID: 2066976582, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:142: value: {fileID: 1117202942, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:144: value: {fileID: -921587470, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:146: value: {fileID: -443571439, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:148: value: {fileID: -773683508, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:150: value: {fileID: -2101696836, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:152: value: {fileID: 243879074, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:154: value: {fileID: -1726118701, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:156: value: {fileID: 52513866, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:158: value: {fileID: -529234311, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:160: value: {fileID: 1141033822, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:162: value: {fileID: 734043391, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:164: value: {fileID: -503812346, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:166: value: {fileID: 1588965202, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:168: value: {fileID: 1572796212, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:170: value: {fileID: -723506763, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:172: value: {fileID: -630029233, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:174: value: {fileID: -552961186, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:176: value: {fileID: -1141714026, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:178: value: {fileID: -1141714026, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:228: - {fileID: 4609173563387012312, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:229: - {fileID: -355127050, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:230: - {fileID: 177361730, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:231: - {fileID: -1136792054, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:232: - {fileID: -391398102, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:233: - {fileID: -752233971, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:234: - {fileID: -2045567293, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:235: - {fileID: 2050552617, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:236: - {fileID: -213360274, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:237: - {fileID: 1820458599, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:238: - {fileID: 93634826, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:239: - {fileID: 356436231, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:240: - {fileID: -1668344661, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:241: - {fileID: 2066976582, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:242: - {fileID: 1117202942, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:243: - {fileID: -921587470, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:244: - {fileID: -443571439, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:245: - {fileID: -773683508, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:246: - {fileID: -2101696836, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:247: - {fileID: 243879074, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:248: - {fileID: -1726118701, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:249: - {fileID: 52513866, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:250: - {fileID: -529234311, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:251: - {fileID: 1141033822, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:252: - {fileID: 734043391, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:253: - {fileID: -503812346, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:254: - {fileID: 1588965202, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:255: - {fileID: 1572796212, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:256: - {fileID: -723506763, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:257: - {fileID: -630029233, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:258: - {fileID: -552961186, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:259: - {fileID: -1141714026, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonIdle.anim:260: - {fileID: -1141714026, guid: 47d01a48de8d5ab46aa532618515fa1f, type: 3}`

### `Assets/_Project/_Assets/SkyIslandMainMenu/Dragon_Play_Reaction.png`

- .png; (2048, 1024); 1074166 byte; GUID 3ae8bb92d8570654fa6d0b31aedc8e9b; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6884971427691096721, 1481458454, -1958724078, -1490606008, 1181368280, 1473044041, -2047872257, -1372277367, 890996613, 2029400269, -505090017, -1633090887, -1952558105, -404768501, 1635764743, -323618943, -831057538, 395095097, 304985820, -1604075099, 1458316224, -2022278193, 276637185, 737100221, 1523910358, -249801792, -1886630301, -950363944, -240389476, 1605239295, -1491292503, -911712310, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:114: value: {fileID: -6884971427691096721, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:116: value: {fileID: 1481458454, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:118: value: {fileID: -1958724078, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:120: value: {fileID: -1490606008, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:122: value: {fileID: 1181368280, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:124: value: {fileID: 1473044041, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:126: value: {fileID: -2047872257, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:128: value: {fileID: -1372277367, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:130: value: {fileID: 890996613, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:132: value: {fileID: 2029400269, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:134: value: {fileID: -505090017, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:136: value: {fileID: -1633090887, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:138: value: {fileID: -1952558105, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:140: value: {fileID: -404768501, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:142: value: {fileID: 1635764743, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:144: value: {fileID: -323618943, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:146: value: {fileID: -831057538, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:148: value: {fileID: 395095097, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:150: value: {fileID: 304985820, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:152: value: {fileID: -1604075099, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:154: value: {fileID: 1458316224, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:156: value: {fileID: -2022278193, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:158: value: {fileID: 276637185, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:160: value: {fileID: 737100221, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:162: value: {fileID: 1523910358, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:164: value: {fileID: -249801792, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:166: value: {fileID: -1886630301, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:168: value: {fileID: -950363944, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:170: value: {fileID: -240389476, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:172: value: {fileID: 1605239295, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:174: value: {fileID: -1491292503, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:176: value: {fileID: -911712310, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:178: value: {fileID: -911712310, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:228: - {fileID: -6884971427691096721, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:229: - {fileID: 1481458454, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:230: - {fileID: -1958724078, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:231: - {fileID: -1490606008, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:232: - {fileID: 1181368280, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:233: - {fileID: 1473044041, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:234: - {fileID: -2047872257, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:235: - {fileID: -1372277367, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:236: - {fileID: 890996613, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:237: - {fileID: 2029400269, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:238: - {fileID: -505090017, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:239: - {fileID: -1633090887, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:240: - {fileID: -1952558105, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:241: - {fileID: -404768501, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:242: - {fileID: 1635764743, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:243: - {fileID: -323618943, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:244: - {fileID: -831057538, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:245: - {fileID: 395095097, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:246: - {fileID: 304985820, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:247: - {fileID: -1604075099, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:248: - {fileID: 1458316224, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:249: - {fileID: -2022278193, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:250: - {fileID: 276637185, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:251: - {fileID: 737100221, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:252: - {fileID: 1523910358, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:253: - {fileID: -249801792, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:254: - {fileID: -1886630301, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:255: - {fileID: -950363944, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:256: - {fileID: -240389476, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:257: - {fileID: 1605239295, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:258: - {fileID: -1491292503, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:259: - {fileID: -911712310, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandMainMenu/Animations/DragonPlayReaction.anim:260: - {fileID: -911712310, guid: 3ae8bb92d8570654fa6d0b31aedc8e9b, type: 3}`

### `Assets/_Project/_Assets/SkyIslandMainMenu/Explorer.png`

- .png; (1199, 1312); 1254512 byte; GUID 1d7c5db2909044cbade0b8fac6ffea27; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "4096", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 8998553830603943519, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/MainMenu.unity:768: m_Sprite: {fileID: 21300000, guid: 1d7c5db2909044cbade0b8fac6ffea27, type: 3}`

### `Assets/_Project/_Assets/SkyIslandMainMenu/MenuDragon.png`

- .png; (1315, 1196); 1142292 byte; GUID 92e7d15245424ed7a6ac4db1498990cd; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "4096", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -4070323590245985328, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/MainMenu.unity:2073: m_Sprite: {fileID: 21300000, guid: 92e7d15245424ed7a6ac4db1498990cd, type: 3}`

### `Assets/_Project/_Assets/SkyIslandMainMenu/MenuIsland.png`

- .png; (941, 1672); 2266005 byte; GUID 64e5242806b84454b9a85ef6139dab4c; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "4096", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -809850331762486293, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/MainMenu.unity:3043: m_Sprite: {fileID: 21300000, guid: 64e5242806b84454b9a85ef6139dab4c, type: 3}`
  - `Assets/Scenes/MainMenu.unity:3437: m_Sprite: {fileID: 21300000, guid: 64e5242806b84454b9a85ef6139dab4c, type: 3}`

### `Assets/_Project/_Assets/SkyIslandMainMenu/menu_fix.png`

- .png; (941, 1672); 2520861 byte; GUID 60f9ed1215a1e884b98d845538ff14fb; nhóm B.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandMainMenu/PlayButton.png`

- .png; (2172, 724); 871257 byte; GUID 6fc7fd25e81945d3b80ccdc65f159e2e; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "4096", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -1958676783162264248, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/MainMenu.unity:7457: m_Sprite: {fileID: 21300000, guid: 6fc7fd25e81945d3b80ccdc65f159e2e, type: 3}`

### `Assets/_Project/_Assets/SkyIslandMainMenu/SettingsButton.png`

- .png; (1261, 1247); 1051752 byte; GUID 20a2921ceb834558a94faa0fbff8da6b; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "4096", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -1655109142887154325, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/MainMenu.unity:5108: m_Sprite: {fileID: 21300000, guid: 20a2921ceb834558a94faa0fbff8da6b, type: 3}`

### `Assets/_Project/_Assets/SkyIslandMainMenu/TitleLogo.png`

- .png; (1312, 1199); 1574241 byte; GUID b6aa2be4647c460cb0c08ef51cae1c72; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "4096", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6944447031546875295, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/MainMenu.unity:179: m_Sprite: {fileID: 21300000, guid: b6aa2be4647c460cb0c08ef51cae1c72, type: 3}`

### `Assets/_Project/_Assets/SkyIslandSetting/Bộ giao diện Sky Block Puzzle 3D.png`

- .png; (1536, 1024); 2553364 byte; GUID f7f6c46d4d3d3db47a408a54c8c66bf5; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2291380761225334291, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/Bộ giao diện game xếp khối .png`

- .png; (1536, 1024); 2105246 byte; GUID 89a76962477551047aff600ae8e15064; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 1560264527552892675, -762148975, -1107584487, -681084021, -82543363, -2016584897, -1442208298, 1262480359, 556016974, -941007713, 2076971702, -478903258, -1262490006, -1165190008, 438347601, -1582494649, 1688287582, -229389224, -560899911, -917598469, 1091677059, 217877171, 1049038464, -1741220654, -2022341121, -1674367226, -2012256811, -2076821445, 597302083, 1415835396, -1807421979, -1147415791, -204287028, 2071921451, -2079209438, 279129906, 1407813267, -2009117824, 207953890, 1707565418, 1383232205, 1830441835, 764339751, -517340279, -1912329754, 1621472516, -1141919438, 1226331036, 1330315118, -1746648767, -1395330272, 788851870, 1780424989, -593721993, -823764096, -1949428499, -2119225261, 25143931, 115281290, -305393563, -1754354133, 1468226039, 681605393, 245852575, -269734629, 686332374, -1325145044, 361471841, -135199325, -583200866, -1820883991, -346427422, 1837966052, -218069837, -507247007, -767324406, 198733911, -1829594825, -1205422120, 1555535551, -1460743013, -376568513, -382332298, 1635155785, -696449262, 421733703, -386998810, 1769262730, 800744240, -2119871874, 1155362551, -816663934, -1713481509, 862166617, 1818306330, -1034303687, -303568758, -2039619628, -704326894, -2128465729, 1580923979, -811780192, -1738755528, 1604505511, 1280524204, -1269935577, 1923468819, -2080610035, -60554008, 1194752192, -1833175527, 1800685909, -1372024766, -842642632, -1373875604, 1311247647, 1507514718, -139020387, 1245496928, 1868478010, 540569960, 1148773421, -2126580965, 1053535935, 589700332, 746888292, 0
- Build graph: False; Resources graph: False; Scene dependency: Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:4930: m_Sprite: {fileID: 1155362551, guid: 89a76962477551047aff600ae8e15064, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:5698: m_Sprite: {fileID: 556016974, guid: 89a76962477551047aff600ae8e15064, type: 3}`

### `Assets/_Project/_Assets/sprite/setting.png`

- .png; (500, 500); 345043 byte; GUID 97669bdc6305a3e439b1c1200ee6d3a4; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6416704506416456542, 0
- Build graph: False; Resources graph: False; Scene dependency: Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:6894: m_Sprite: {fileID: -6416704506416456542, guid: 97669bdc6305a3e439b1c1200ee6d3a4, type: 3}`

### `Assets/_Project/_Assets/sprite/sky_block_sheet_transparent.png`

- .png; (1024, 1536); 1467203 byte; GUID 314d88485957a8b4a80a714d0aadc6fe; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 8109477017451902376, 507818571, -421806573, 358180719, -503876433, 384151217, 1387737086, -1936233587, 427207927, -959319807, -1937319139, -878297289, 1696983822, 813948030, -1665533468, 163026138, -1375303342, -1676402355, -691376138, -1501287553, -537604022, -1609204060, -989911125, -1639705777, 945745299, -1410127685, 630235341, 372044531, -752154111, 238920193, -1635136048, -1243930101, -1173172065, 1953436467, 2049829491, -1678139045, -854949343, -1639733910, 376098631, 623242498, 135741481, 1389975751, -673636947, 234547330, -1988746898, 800650588, -305187685, -2139757554, -1851779035, -37065596, 948292270, -1223096435, 882635559, -14270438, -2082038532, 2140452517, 421749998, -1126748063, 1968323380, -1815618408, -1100751786, -1742130483, 1110042492, -182967183, -955296342, -366977355, 241042430, 1371093606, -1676056784, 447287208, 337741417, -988595538, -935422106, 1934832259, -2128684186, 672687967, 1606028646, 858632986, -523780491, 564782883, -1940286736, 1909571247, 1769489285, -581270657, 0
- Build graph: False; Resources graph: False; Scene dependency: Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:4019: m_Sprite: {fileID: 1389975751, guid: 314d88485957a8b4a80a714d0aadc6fe, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:4336: m_Sprite: {fileID: 507818571, guid: 314d88485957a8b4a80a714d0aadc6fe, type: 3}`

### `Assets/_Project/_Assets/_Menu/background.jpeg`

- .jpeg; (2048, 2048); 7501843 byte; GUID 8abe757aed99a274f9990f85e154519b; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -7443756053782179069, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/MainMenu.unity, Assets/_Project/Environment/Backups/MainMenu_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/IslandEnvironmentChecks.cs, Assets/_Project/Editor/IslandEnvironmentInstaller.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayLayoutAuthoring.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/MainMenu.unity:7798: m_Sprite: {fileID: -7443756053782179069, guid: 8abe757aed99a274f9990f85e154519b, type: 3}`
  - `Assets/_Project/Environment/Backups/MainMenu_BeforeTask01.unity:2103: m_Sprite: {fileID: -7443756053782179069, guid: 8abe757aed99a274f9990f85e154519b, type: 3}`

### `Assets/_Project/_Assets/_Menu/Menu chính Sky Block Puzzle bay bổng.png`

- .png; (1536, 1024); 2401207 byte; GUID 4ea3e0e251155244986cafe5ceac10c7; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6699661299554106711, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/_Menu/Menu.png`

- .png; (1536, 1024); 2488944 byte; GUID 2deab30dbb2aebd48abb301633a47ffd; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -198938125430884413, -1763455421, -344178364, 1609805113, 972167472, -263393704, -330853525, 1786339151, -290440718, -1601235127, 491139523, -1948941312, -1841112339, -1894021237, 1705853438, 1102584571, -1929210856, -440887054, 1959564185, -282505922, 216539173, 1232188497, -1701874283, 664078257, 1522773883, -1671756406, 0
- Build graph: False; Resources graph: False; Scene dependency: Assets/_Project/Environment/Backups/MainMenu_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/Environment/Backups/MainMenu_BeforeTask01.unity:179: m_Sprite: {fileID: -198938125430884413, guid: 2deab30dbb2aebd48abb301633a47ffd, type: 3}`
  - `Assets/_Project/Environment/Backups/MainMenu_BeforeTask01.unity:505: m_Sprite: {fileID: -1894021237, guid: 2deab30dbb2aebd48abb301633a47ffd, type: 3}`
  - `Assets/_Project/Environment/Backups/MainMenu_BeforeTask01.unity:1032: m_Sprite: {fileID: 1705853438, guid: 2deab30dbb2aebd48abb301633a47ffd, type: 3}`
  - `Assets/_Project/Environment/Backups/MainMenu_BeforeTask01.unity:1990: m_Sprite: {fileID: -263393704, guid: 2deab30dbb2aebd48abb301633a47ffd, type: 3}`

### `Assets/_Project/_Assets/UI/AntigravityGen/BackgroundSoft.png`

- .png; (415, 430); 80111 byte; GUID 1f8a14108d8926c17b96cbba9d9ec45e; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -5210908662597136490, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/UIArtVariants.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block0Disabled.png`

- .png; (384, 384); 64727 byte; GUID 34f13837f5724189466b94ca2732d5cc; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8495966459991418233, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block0Highlighted.png`

- .png; (384, 384); 131969 byte; GUID 2a58438b67d1d38a0338125845305070; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8765929113565392776, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block0Normal.png`

- .png; (384, 384); 151633 byte; GUID f16d6ab6a9365357924b2ec3cf1f57ab; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -4738021195338925408, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block1Disabled.png`

- .png; (384, 384); 71247 byte; GUID cfdc4e7df29e7864902dbb38915ca969; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 9171629915536649711, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block1Highlighted.png`

- .png; (384, 384); 127507 byte; GUID 015f0f8af82a59c14b41664a388c5e51; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7125189894888741201, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block1Normal.png`

- .png; (384, 384); 155611 byte; GUID a50b5b94b900a378b71680355dda54f3; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 8873924949879006168, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block2Disabled.png`

- .png; (384, 384); 64346 byte; GUID a4f710fff85db92dc0568843450c9a9e; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7455638864653402289, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block2Highlighted.png`

- .png; (384, 384); 101959 byte; GUID fc3eb67d88dbf4b173ef9b26393ecf68; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -625785846316208143, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block2Normal.png`

- .png; (384, 384); 125447 byte; GUID c522e43bd0462062961b49ecd6ef0441; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 2944593789210980750, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block3Disabled.png`

- .png; (384, 384); 65852 byte; GUID 2bd7252932e8adfa4fec2a7cedcf474f; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 744143619153080078, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block3Highlighted.png`

- .png; (384, 384); 155916 byte; GUID 986c1446b0af78fbe8794c4000faf3ad; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4957633553924677500, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block3Normal.png`

- .png; (384, 384); 148892 byte; GUID 3784a0b5a5ca07cf3d9b5f8194c28ab4; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6386308040033041660, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block4Disabled.png`

- .png; (384, 384); 65835 byte; GUID 839957fbf07513477d2114a4cd679338; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6409559031009954441, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block4Highlighted.png`

- .png; (384, 384); 152659 byte; GUID 4d617aa8eb313c20ccad21103c29e579; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 8018499674757750637, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block4Normal.png`

- .png; (384, 384); 154618 byte; GUID c3c34511cd7b0f395116f5d30ed8a080; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4692950131324915142, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block5Disabled.png`

- .png; (384, 384); 65295 byte; GUID 4cd4af8aac76850b6dfc38800d024fcc; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5114307727998365036, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block5Highlighted.png`

- .png; (384, 384); 132974 byte; GUID 27141407182abed585837d621f35393f; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8143733837897426619, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block5Normal.png`

- .png; (384, 384); 152424 byte; GUID a1ecfc1831e9c49823f8cb97398e9b4d; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7204493747986274894, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block6Disabled.png`

- .png; (384, 384); 73617 byte; GUID 8ca751ae91326771e20767fff9533eed; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2035131612192785458, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block6Highlighted.png`

- .png; (384, 384); 50373 byte; GUID 2b0ce62f73b87100a0f69b351f12026b; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8928964709616841920, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block6Normal.png`

- .png; (384, 384); 153831 byte; GUID a90c0de9d2a7b1b2e9f659f5701e92bd; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8151066912651019718, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block7Disabled.png`

- .png; (384, 384); 73617 byte; GUID db19b781aae0de25acacfa7a1d8e420a; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 3929777981628782178, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block7Highlighted.png`

- .png; (384, 384); 50373 byte; GUID 2b73b91e1eb336ae485d2235fd7c5fcf; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 900884918216358690, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Block7Normal.png`

- .png; (384, 384); 152784 byte; GUID 1650412e6550142b63162e745acc88d4; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 1479292640288875141, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/BlockBlast_Art_SpriteSheet.png`

- .png; (2048, 2048); 1021272 byte; GUID 84e959c6c643688f127e333c4bfa0083; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6547413204877522291, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/BlockWhite.png`

- .png; (384, 384); 141971 byte; GUID 2779ebacb2f6073fc6303869c1697faf; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 45, y: 45, z: 45, w: 45}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -7041519908417514300, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs, Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/CellBeveled.png`

- .png; (384, 384); 14261 byte; GUID 1282e8bad9a8b588d7cb8118ebd45178; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 36, y: 36, z: 36, w: 36}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -666179557924137675, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/CrownIcon.png`

- .png; (256, 256); 69822 byte; GUID 900ba07f4ebf27c63ceb777051fcae15; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6777162303297155294, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/Panel.png`

- .png; (512, 512); 7140 byte; GUID 534a4efc7cbcc54ce2c05413bf8fcb65; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "200", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 64, y: 64, z: 64, w: 64}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -1198477067713850731, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameOverInstaller.cs, Assets/_Project/Editor/GameplayUIInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/SettingsIcon.png`

- .png; (256, 256); 91755 byte; GUID 7504f27f2aa1d94d6b723cc94ed34ef8; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5904627132653970092, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/SquareParticle.png`

- .png; (128, 128); 2161 byte; GUID 7e8f55943bc22a8fa58c40c0b38c3e28; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 4, y: 4, z: 4, w: 4}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 2571114976766217928, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/StoneFrame.png`

- .png; (1024, 1024); 1196889 byte; GUID dd82464d4a4d98c5393dd775389208d7; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "400", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 112, y: 112, z: 112, w: 112}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4756602434617575680, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/TraySlot.png`

- .png; (512, 512); 12610 byte; GUID 80bdc6a22c89d6629099506cdaceb4fe; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "200", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 56, y: 56, z: 56, w: 56}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -9195402912914313453, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/TrophyIcon.png`

- .png; (256, 256); 76585 byte; GUID 515f41b2f99fadc3e415bb1ba476c209; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8116845798535481512, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Disabled.png`

- .png; (384, 384); 64727 byte; GUID 4a777674a5e0e830e681d105620eceaf; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -364640900876226405, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Highlighted.png`

- .png; (384, 384); 131969 byte; GUID d55f92e07f0591e4eb09c6e731ee6087; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 9175017883495956034, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Normal.png`

- .png; (384, 384); 151633 byte; GUID cca9f0741c806bb31a12957bd7398acf; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2337570061654428930, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_28_Disabled.png`

- .png; (384, 384); 71247 byte; GUID 4836c01c3711650bfbf24974188a9839; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2093349269767040530, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_28_Highlighted.png`

- .png; (384, 384); 127507 byte; GUID 251b115a273f61a9fa10ff37d88e8acf; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 8994034169197929843, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_28_Normal.png`

- .png; (384, 384); 155611 byte; GUID 7d4e59d1625e849a07f6c7259a21ed73; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 3495419142862636717, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Disabled.png`

- .png; (384, 384); 64346 byte; GUID fb3ebea5bdc201b70992d6bc475fe02f; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -7546845627833400698, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Highlighted.png`

- .png; (384, 384); 101959 byte; GUID 360cfd30e4caf41be0998d0326160171; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2327809907164347089, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Normal.png`

- .png; (384, 384); 125447 byte; GUID f74fddfd2ed96c8f3ebfd44afbbd9e4e; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 3911421584132353241, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Disabled.png`

- .png; (384, 384); 65852 byte; GUID 5838a2c6c98c8e6a382c4a2c1bfbd93a; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6219061840314394545, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Highlighted.png`

- .png; (384, 384); 155916 byte; GUID a47dd4f116dcf92fcb3baccdb44c40ca; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -48187753390426217, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Normal.png`

- .png; (384, 384); 148892 byte; GUID 5cff23efcc143a223149b9dae8a84d65; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -1942739846722466993, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Disabled.png`

- .png; (384, 384); 65835 byte; GUID 59800cac3a2afaa9200f74e5f740b116; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -7091824333319045117, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Highlighted.png`

- .png; (384, 384); 152659 byte; GUID 390199340d5426b8448e5c18eb646721; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4904041343630304509, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Normal.png`

- .png; (384, 384); 154618 byte; GUID 585fbb1a166b0859cc7fbd826f817525; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 453967534372815966, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Disabled.png`

- .png; (384, 384); 65295 byte; GUID e9b777e28dd627c3f8a44d27a9b71fea; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -75815704544082116, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Highlighted.png`

- .png; (384, 384); 132974 byte; GUID ecb9c5aac730a634eb2d877e6e588969; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5549650284087551523, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Normal.png`

- .png; (384, 384); 152424 byte; GUID 6d5e5df2a6513ae4b607d82eae88e924; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6147305365366725362, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/UI/Generated/BackgroundSoft.png`

- .png; (415, 430); 80111 byte; GUID 28e6a62f95993a74a9e2dbd8a4ca5cf1; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -5210908662597136490, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/UIArtVariants.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:9230: m_Sprite: {fileID: 21300000, guid: 28e6a62f95993a74a9e2dbd8a4ca5cf1, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:3270: m_Sprite: {fileID: 21300000, guid: 28e6a62f95993a74a9e2dbd8a4ca5cf1, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block0Disabled.png`

- .png; (384, 384); 56249 byte; GUID 3c89bdacb5dd2ed48803135518a58a31; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8495966459991418233, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block0Highlighted.png`

- .png; (384, 384); 53493 byte; GUID 47e9c6337b6e96a4db3b1eb420543f99; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8765929113565392776, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block0Normal.png`

- .png; (384, 384); 62530 byte; GUID 71578ba30ff448328bff94be09991cde; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -4738021195338925408, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block1Disabled.png`

- .png; (384, 384); 56122 byte; GUID dd19ed06eda902443b9fe71b39b26615; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 9171629915536649711, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block1Highlighted.png`

- .png; (384, 384); 44707 byte; GUID a47b5d35082c61b42bc8fe4a0cdf7436; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7125189894888741201, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block1Normal.png`

- .png; (384, 384); 64323 byte; GUID 73b341fafa314676af9b747bb1269df5; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 8873924949879006168, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block2Disabled.png`

- .png; (384, 384); 59321 byte; GUID 0b75abfaef8411f429f2a90c47f66dc0; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7455638864653402289, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block2Highlighted.png`

- .png; (384, 384); 64514 byte; GUID adce75885423d094abe5c37d3b4b39cd; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -625785846316208143, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block2Normal.png`

- .png; (384, 384); 71644 byte; GUID 5d58df27a3dd4610b1fe7be4153a4fbb; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 2944593789210980750, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block3Disabled.png`

- .png; (384, 384); 59602 byte; GUID 45fb2f145d1858c42b9448893e136aea; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 744143619153080078, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block3Highlighted.png`

- .png; (384, 384); 70883 byte; GUID 1ec208d32205a224591137f36b11ef27; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4957633553924677500, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block3Normal.png`

- .png; (384, 384); 74690 byte; GUID 3e53aba25d9742feb0ebf91abc49c823; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6386308040033041660, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block4Disabled.png`

- .png; (384, 384); 53093 byte; GUID c9da168c19eb9ae4ba330fe89bfc1d94; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6409559031009954441, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block4Highlighted.png`

- .png; (384, 384); 39282 byte; GUID f11b45828b348984ab5e0b3928ad4874; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 8018499674757750637, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block4Normal.png`

- .png; (384, 384); 62090 byte; GUID c000cd65df794506a7f4fdecd18a0128; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4692950131324915142, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block5Disabled.png`

- .png; (384, 384); 58855 byte; GUID 0bde669c2c53a5d4cae9ad0a7122a829; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5114307727998365036, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block5Highlighted.png`

- .png; (384, 384); 57392 byte; GUID e67eff8a2dd6b474e943ee32745977c1; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8143733837897426619, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block5Normal.png`

- .png; (384, 384); 69803 byte; GUID 3ae8c3fbf2844dd1b0b3d845bbed8667; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7204493747986274894, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block6Disabled.png`

- .png; (384, 384); 48620 byte; GUID 49ed065d50fd9494c91802b8db7086af; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2035131612192785458, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block6Highlighted.png`

- .png; (384, 384); 51931 byte; GUID 6316af1de5ae50a4c9f2f12a3db721b3; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8928964709616841920, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block6Normal.png`

- .png; (384, 384); 60195 byte; GUID a1d802040d824b9abbd861037dbeb2a3; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8151066912651019718, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block7Disabled.png`

- .png; (384, 384); 50587 byte; GUID 53ae5b435a154b645bd99269b824b529; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 3929777981628782178, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block7Highlighted.png`

- .png; (384, 384); 58157 byte; GUID f53ce5c84fe1a464fb43e20e027c8cad; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 900884918216358690, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/Block7Normal.png`

- .png; (384, 384); 58715 byte; GUID bc81fc3e151741acbbf442d92a32c7b6; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 1479292640288875141, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/BlockBlast_Art_SpriteSheet.png`

- .png; (2048, 2048); 1184320 byte; GUID b0e052c013af4c4f9ff8ef2fd5c2fd7b; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6547413204877522291, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/BlockWhite.png`

- .png; (384, 384); 142091 byte; GUID 474bc36323be0924a91a4056d90b2718; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 45, y: 45, z: 45, w: 45}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -7041519908417514300, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs, Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:4122: m_Sprite: {fileID: 21300000, guid: 474bc36323be0924a91a4056d90b2718, type: 3}`
  - `Assets/Scenes/GamePlay.unity:11927: m_Sprite: {fileID: 21300000, guid: 474bc36323be0924a91a4056d90b2718, type: 3}`
  - `Assets/Scenes/GamePlay.unity:13698: m_Sprite: {fileID: 21300000, guid: 474bc36323be0924a91a4056d90b2718, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:2804: m_Sprite: {fileID: 21300000, guid: 474bc36323be0924a91a4056d90b2718, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:3235: m_Sprite: {fileID: 21300000, guid: 474bc36323be0924a91a4056d90b2718, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:4306: m_Sprite: {fileID: 21300000, guid: 474bc36323be0924a91a4056d90b2718, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:1533: m_Sprite: {fileID: 21300000, guid: 474bc36323be0924a91a4056d90b2718, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:3823: m_Sprite: {fileID: 21300000, guid: 474bc36323be0924a91a4056d90b2718, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:4768: m_Sprite: {fileID: 21300000, guid: 474bc36323be0924a91a4056d90b2718, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/CellBeveled.png`

- .png; (384, 384); 14261 byte; GUID 6abb93a4c43537e48af8b32860b35ce0; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 36, y: 36, z: 36, w: 36}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -666179557924137675, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas:37: - {fileID: 21300000, guid: 6abb93a4c43537e48af8b32860b35ce0, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/CrownIcon.png`

- .png; (256, 256); 75770 byte; GUID 313362a9b850b1943990b79a77dbcced; nhóm A.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/SettingsIcon.png`

- .png; (256, 256); 114619 byte; GUID c2dace9dc24067c47a1d980e1411c857; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5904627132653970092, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:10371: m_Sprite: {fileID: 5904627132653970092, guid: c2dace9dc24067c47a1d980e1411c857, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:3538: m_Sprite: {fileID: 5904627132653970092, guid: c2dace9dc24067c47a1d980e1411c857, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/StoneFrame.png`

- .png; (1024, 1024); 1196889 byte; GUID 33f3fbc37a979cf4db67791eb6f07857; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "400", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 112, y: 112, z: 112, w: 112}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4756602434617575680, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/GameplayUIInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:8506: m_Sprite: {fileID: 21300000, guid: 33f3fbc37a979cf4db67791eb6f07857, type: 3}`
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:1225: m_Sprite: {fileID: 21300000, guid: 33f3fbc37a979cf4db67791eb6f07857, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:3049: m_Sprite: {fileID: 21300000, guid: 33f3fbc37a979cf4db67791eb6f07857, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/TrophyIcon.png`

- .png; (256, 256); 105330 byte; GUID 7aca23cc87e5f7f48b6971a71a175b61; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8116845798535481512, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_27_Disabled.png`

- .png; (384, 384); 56249 byte; GUID 65785541b537432db9ee2964ba0f276a; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -364640900876226405, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_27_Highlighted.png`

- .png; (384, 384); 53493 byte; GUID 94b72216367e41e5b7ff621344d1bef9; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 9175017883495956034, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_27_Normal.png`

- .png; (384, 384); 62530 byte; GUID 4aec9a6d97334d3baba232b0714d253f; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2337570061654428930, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_28_Disabled.png`

- .png; (384, 384); 56122 byte; GUID fc9daee1ca53454a86ea6fd4926af76d; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2093349269767040530, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_28_Highlighted.png`

- .png; (384, 384); 44707 byte; GUID c8a1e1e80cef4236b4f3be6000729047; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 8994034169197929843, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_28_Normal.png`

- .png; (384, 384); 64323 byte; GUID 1b8ef5ea99c6447899a1b5e71b3c65db; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 3495419142862636717, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_29_Disabled.png`

- .png; (384, 384); 59321 byte; GUID fd99688a56544d8bbe7735a1797ccfd6; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -7546845627833400698, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_29_Highlighted.png`

- .png; (384, 384); 64514 byte; GUID b37453dbb0074c7dbae7636ee685bd5e; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2327809907164347089, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_29_Normal.png`

- .png; (384, 384); 71644 byte; GUID 58112988d28c4dae92a646601066dc23; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 3911421584132353241, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_30_Disabled.png`

- .png; (384, 384); 59602 byte; GUID d5b257f8a262488b9f6f88d52fa306f3; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6219061840314394545, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_30_Highlighted.png`

- .png; (384, 384); 70883 byte; GUID 98cc8b42d26a44f5b7d57e9d34ca7424; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -48187753390426217, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_30_Normal.png`

- .png; (384, 384); 74690 byte; GUID e726652dd4334022abd2a76cbf3c3b8d; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -1942739846722466993, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_31_Disabled.png`

- .png; (384, 384); 53093 byte; GUID 500d116eae3c49a7b80bcb96d3f7a425; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -7091824333319045117, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_31_Highlighted.png`

- .png; (384, 384); 39282 byte; GUID 91c9c72f35df4e5f87a6db81a5c4f0b7; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4904041343630304509, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_31_Normal.png`

- .png; (384, 384); 62090 byte; GUID 512a8ff167ca481889e584207263d26d; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 453967534372815966, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_32_Disabled.png`

- .png; (384, 384); 58855 byte; GUID 674d5671c1b64631a6c1558c00a8cc65; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -75815704544082116, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_32_Highlighted.png`

- .png; (384, 384); 57392 byte; GUID 9ca5d45adf434cc4b7df318e50ebaca6; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5549650284087551523, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_32_Normal.png`

- .png; (384, 384); 69803 byte; GUID 30ced4e8f6484f998ce3d793162a361d; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6147305365366725362, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/Generated`

### `Assets/_Project/_Assets/UI/ReferenceArt/BackgroundCliffs.png`

- .png; (120, 281); 88304 byte; GUID 656bf277a642fe94ba068cde4c33755f; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -4724004776432051660, 0
- Build graph: False; Resources graph: False; Scene dependency: Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:7574: m_Sprite: {fileID: 21300000, guid: 656bf277a642fe94ba068cde4c33755f, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockBlue.png`

- .png; (75, 77); 8683 byte; GUID 93ff932a58820c94596eb2954b24fa0e; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -5346708522279743761, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:64: - {fileID: 21300000, guid: 93ff932a58820c94596eb2954b24fa0e, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockBlueDisabled.png`

- .png; (75, 77); 7568 byte; GUID 8193d5cfaa54a354fb2329f09d378782; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 8265315578000441644, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:83: - {fileID: 21300000, guid: 8193d5cfaa54a354fb2329f09d378782, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockBlueHighlighted.png`

- .png; (75, 77); 8430 byte; GUID d341576f84a8794468572f591ecb30e5; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6109785170644282696, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:74: - {fileID: 21300000, guid: d341576f84a8794468572f591ecb30e5, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockCyan.png`

- .png; (75, 78); 8681 byte; GUID ed522f53135460143b3d4e02d5aea67c; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 1549685995635130972, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:68: - {fileID: 21300000, guid: ed522f53135460143b3d4e02d5aea67c, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockCyanDisabled.png`

- .png; (75, 78); 7577 byte; GUID ac512774c84ba0b47a0dda542eae37e8; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 8318136653198414461, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:87: - {fileID: 21300000, guid: ac512774c84ba0b47a0dda542eae37e8, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockCyanHighlighted.png`

- .png; (75, 78); 8438 byte; GUID 41ddb92a808c80d439993b8deb235b1c; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6683368868602793144, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:78: - {fileID: 21300000, guid: 41ddb92a808c80d439993b8deb235b1c, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockGreen.png`

- .png; (75, 77); 9070 byte; GUID b70b20a8d7b4cf9479040fc4e4ea699e; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2399699543274225224, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:67: - {fileID: 21300000, guid: b70b20a8d7b4cf9479040fc4e4ea699e, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockGreenDisabled.png`

- .png; (75, 77); 7825 byte; GUID 16cda7a9c59f6974c8db304d86c43d34; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 1876204457103340093, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:86: - {fileID: 21300000, guid: 16cda7a9c59f6974c8db304d86c43d34, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockGreenHighlighted.png`

- .png; (75, 77); 8850 byte; GUID 0e94224998c2893419ef6608410b54af; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8508786887268924986, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:77: - {fileID: 21300000, guid: 0e94224998c2893419ef6608410b54af, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockOrange.png`

- .png; (75, 77); 8538 byte; GUID e754c8cdda8adca4db826a184cc348aa; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6477963506834638451, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:70: - {fileID: 21300000, guid: e754c8cdda8adca4db826a184cc348aa, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockOrangeDisabled.png`

- .png; (75, 77); 7393 byte; GUID 38c4188596234264e83dce9a949f6c87; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4002108821181045633, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:89: - {fileID: 21300000, guid: 38c4188596234264e83dce9a949f6c87, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockOrangeHighlighted.png`

- .png; (75, 77); 8248 byte; GUID cb52831e768c60843b089157e688980d; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -5035481469518656861, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:80: - {fileID: 21300000, guid: cb52831e768c60843b089157e688980d, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockPink.png`

- .png; (75, 78); 8999 byte; GUID b2e2bd85f7672fb4483ed0ebc5aaac2c; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 860947830124741983, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:71: - {fileID: 21300000, guid: b2e2bd85f7672fb4483ed0ebc5aaac2c, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockPinkDisabled.png`

- .png; (75, 78); 7833 byte; GUID b37160f58519e0648877646047912545; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 385984870068260490, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:90: - {fileID: 21300000, guid: b37160f58519e0648877646047912545, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockPinkHighlighted.png`

- .png; (75, 78); 8779 byte; GUID 1893c814d7498e64b8d950717f4f007a; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -5251081363804676240, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:81: - {fileID: 21300000, guid: 1893c814d7498e64b8d950717f4f007a, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockPurple.png`

- .png; (75, 78); 8730 byte; GUID fdc5ce40b9efb9143adcb57746a8b95f; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 266263736554785849, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:69: - {fileID: 21300000, guid: fdc5ce40b9efb9143adcb57746a8b95f, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockPurpleDisabled.png`

- .png; (75, 78); 7746 byte; GUID edc794376b3b9a349841b0fd0ec2154d; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -5043313851994387454, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:88: - {fileID: 21300000, guid: edc794376b3b9a349841b0fd0ec2154d, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockPurpleHighlighted.png`

- .png; (75, 78); 8602 byte; GUID ce6832b72183b4f44862b21c088c9e93; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -4698760658709947201, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:79: - {fileID: 21300000, guid: ce6832b72183b4f44862b21c088c9e93, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockRed.png`

- .png; (75, 77); 8801 byte; GUID 245dc0b856f39014da74646af7a73bb2; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -1565828462937117185, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:66: - {fileID: 21300000, guid: 245dc0b856f39014da74646af7a73bb2, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockRedDisabled.png`

- .png; (75, 77); 7535 byte; GUID db47d6fd170967a47a8d0d2fc9bfefc0; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7095435138682085947, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:85: - {fileID: 21300000, guid: db47d6fd170967a47a8d0d2fc9bfefc0, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockRedHighlighted.png`

- .png; (75, 77); 8511 byte; GUID 174b871729b74f84dac4cd2189d681d7; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4998208881861534926, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:76: - {fileID: 21300000, guid: 174b871729b74f84dac4cd2189d681d7, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockWhiteDisabled.png`

- .png; (75, 78); 7626 byte; GUID 2e5dc47eb74cc444eaca888f2bf33444; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 8825075987212333141, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockWhiteHighlighted.png`

- .png; (75, 78); 8577 byte; GUID 886490a02b654954aab508c3d90f4517; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -5599904588754789503, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockYellow.png`

- .png; (75, 77); 8545 byte; GUID 68541bec3ddf9ed4b9e211e4a98b8d96; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -3386545547340684358, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:65: - {fileID: 21300000, guid: 68541bec3ddf9ed4b9e211e4a98b8d96, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockYellowDisabled.png`

- .png; (75, 77); 7322 byte; GUID 6a1ded1acdaf53b4f90b0b1166b5fd9f; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8641755686023199390, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:84: - {fileID: 21300000, guid: 6a1ded1acdaf53b4f90b0b1166b5fd9f, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BlockYellowHighlighted.png`

- .png; (75, 77); 8330 byte; GUID 974f22f93f843884c9a005474782e00b; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 3877480426548496970, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:75: - {fileID: 21300000, guid: 974f22f93f843884c9a005474782e00b, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BoardCrest.png`

- .png; (65, 48); 3565 byte; GUID b2f8cba33ce208943aa8361f526e4bc3; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4300788212218763903, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/ScriptableObjects/UITheme.asset:31: boardCrest: {fileID: 21300000, guid: b2f8cba33ce208943aa8361f526e4bc3, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/GameplayTheme.asset:31: boardCrest: {fileID: 21300000, guid: b2f8cba33ce208943aa8361f526e4bc3, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/BoardStone.png`

- .png; (292, 260); 44709 byte; GUID 9d374b8e01c52fa40a642b15f737031d; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 36, y: 46, z: 36, w: 36}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -795104177102107855, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:3160: m_Sprite: {fileID: 21300000, guid: 9d374b8e01c52fa40a642b15f737031d, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/CellBlue.png`

- .png; (77, 78); 8225 byte; GUID bc71c11f211f050458fc4ed0ba047edd; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 18, y: 18, z: 18, w: 18}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6735827912647421791, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/Crown.png`

- .png; (46, 46); 3525 byte; GUID 7a28466812cdee14298c638fb237d71e; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -1378521658571115459, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:743: m_Sprite: {fileID: 21300000, guid: 7a28466812cdee14298c638fb237d71e, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/HUDWood.png`

- .png; (432, 97); 66791 byte; GUID 95f45c3d4dd7a5b48a3c7da177f45c97; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -9185602115501719717, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:5224: m_Sprite: {fileID: 21300000, guid: 95f45c3d4dd7a5b48a3c7da177f45c97, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/SettingsButton.png`

- .png; (47, 57); 6660 byte; GUID dbc8a518c15dbcd4a9c82d9b08a649bc; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -1655109142887154325, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:463: m_Sprite: {fileID: 21300000, guid: dbc8a518c15dbcd4a9c82d9b08a649bc, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/UI/ReferenceArt/TrayWood.png`

- .png; (286, 186); 61014 byte; GUID 50094e1896d54cc41ae76cb45e821747; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -8615139583052809645, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/Editor/ReferenceGameplayArt.cs, Assets/_Project/Editor/ReferenceGameplayInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:4035: m_Sprite: {fileID: 21300000, guid: 50094e1896d54cc41ae76cb45e821747, type: 3}`
  - `Assets/_Project/_Assets/UI/Gameplay.spriteatlas: atlas folder Assets/_Project/_Assets/UI/ReferenceArt`

### `Assets/_Project/_Assets/sprite/blockblast_assets/background_sky_fantasy.png`

- .png; (2048, 1152); 3172919 byte; GUID 818fd33b9dc396c48872cbd9682c8bb5; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5942431498370404569, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/blockblast_assets/block_blue_1x1.png`

- .png; (1024, 1024); 1151645 byte; GUID 555633b4aa5b7914aab2aa002386ad38; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/blockblast_assets/block_green_1x1.png`

- .png; (1024, 1024); 1179843 byte; GUID 64d55ce642d7ae741b2871d3a0834c19; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/blockblast_assets/block_purple_1x1.png`

- .png; (1024, 1024); 1137757 byte; GUID 662d5bcef5597624e91709553ddc41aa; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/blockblast_assets/block_red_1x1.png`

- .png; (1024, 1024); 1115227 byte; GUID 335cc088e3e72204f903c79dce5bae9a; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/blockblast_assets/block_yellow_1x1.png`

- .png; (1024, 1024); 1134357 byte; GUID a18c38190d3542f4384b7b162907f529; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/blockblast_assets/board_base.png`

- .png; (1024, 1024); 1868014 byte; GUID cfbff5cfec10f1642942872372588b46; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4615799949758634320, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/blockblast_assets/board_frame.png`

- .png; (1024, 1024); 1627374 byte; GUID 60f1723b10386124eb7495d48ea3316b; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 263172089585269558, 0
- Build graph: False; Resources graph: False; Scene dependency: Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/_Project/Prefabs/UI/GameplayScreen.prefab:4802: m_Sprite: {fileID: 263172089585269558, guid: 60f1723b10386124eb7495d48ea3316b, type: 3}`
  - `Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity:2396: m_Sprite: {fileID: 263172089585269558, guid: 60f1723b10386124eb7495d48ea3316b, type: 3}`

### `Assets/_Project/_Assets/sprite/blockblast_assets/fantasy_lamp_left.png`

- .png; (1024, 1024); 1318057 byte; GUID 877d0de6cf82f8e46aa58acbe0b91ac2; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -4727951843711687826, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/blockblast_assets/fantasy_lamp_right.png`

- .png; (1024, 1024); 1270239 byte; GUID e76ae9842033e2e4e80d4b7ca3196ac2; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7275768555070315403, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/blockblast_assets/piece_slot_empty.png`

- .png; (1024, 1024); 1574797 byte; GUID 7350cd4480aa9bb4eb8b72d01123fdb9; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -1018300931629530764, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/blockblast_assets/piece_tray_background.png`

- .png; (2048, 1024); 3221229 byte; GUID fd36dc6506bcf2f4ab4bdf2b9a85332d; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7611654282280343406, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/spriteBlocks/board-removebg-preview.png`

- .png; (484, 516); 387105 byte; GUID 49ae5a78d384aa346961b1f6e1d14617; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -5759638410525292447, -1884719247, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/spriteBlocks/Bảng ý tưởng giao diện game xếp khối 3D 1.png`

- .png; (1254, 1254); 2245748 byte; GUID 6d5e86828e9e57e4b935c62181ca0a4e; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/spriteBlocks/Bảng ý tưởng giao diện game xếp khối 3D.png`

- .png; (1254, 1254); 2245748 byte; GUID a8f822ff5b58b8b4c86d106527dc7c74; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/spriteBlocks/Bộ tài sản giao diện game puzzle 1.png`

- .png; (1536, 1024); 1424345 byte; GUID 17ba2c300c4f5864fa34ae43282040a6; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/spriteBlocks/gpt-image-2-724bd9f2a28d0819f15fdd865a7a05c7 1.png`

- .png; (1536, 1024); 2102442 byte; GUID 4c04cc0516f13904fb8d893ad32fe85e; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5815241543676517819, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/sprite/spriteBlocks/gpt-image-2-e3a3c8b32d75b6b5be1ba0bbce36c025 1.png`

- .png; (1536, 1024); 1583282 byte; GUID 507cdfabf57d8384195eee7cebeedebe; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/01_SettingsPanel/settings_panel_empty.png`

- .png; (1086, 1448); 1719387 byte; GUID 9767fe3195276da4288db0e75e0b38cc; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/01_SettingsPanel/settings_panel_settings.png`

- .png; (1086, 1448); 1736785 byte; GUID 2b60ea2f88fb31342af468ca195310df; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -557927419750706629, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button.png`

- .png; (1254, 1254); 1045793 byte; GUID c6cb37ab2aab91348abd5259f2a43031; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 2530564911472380330, 554860485, 291628634, -633711312, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:15299: m_Sprite: {fileID: 2530564911472380330, guid: c6cb37ab2aab91348abd5259f2a43031, type: 3}`

### `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_blue_home.png`

- .png; (996, 304); 322826 byte; GUID 85008ae9c7bfe29438ce41e6c1b4fe90; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_blue_home_with_icon.png`

- .png; (996, 304); 338368 byte; GUID 56d175546cba6224091d43e9f826391a; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5307449885526249295, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:9645: m_Sprite: {fileID: 5307449885526249295, guid: 56d175546cba6224091d43e9f826391a, type: 3}`

### `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_blue_navigation.png`

- .png; (996, 304); 316736 byte; GUID 2f8a94fff5a8ae1409cf7bce2ba9598e; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_blue_replay.png`

- .png; (996, 304); 328424 byte; GUID 963e02f56bb74a947abe0a2e1be0ae64; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_blue_replay_with_icon.png`

- .png; (996, 304); 343528 byte; GUID b909dcad4786d784bb72fe26de4d4321; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -1546553725977554943, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:14104: m_Sprite: {fileID: -1546553725977554943, guid: b909dcad4786d784bb72fe26de4d4321, type: 3}`

### `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_round_blue.png`

- .png; (328, 332); 112560 byte; GUID b234f08252171fb4d8ed73b7b19409f5; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_yellow_close.png`

- .png; (1087, 388); 417928 byte; GUID 9307de7e4abe7a041b454140e9051b01; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/button_yellow_close_labeled.png`

- .png; (1087, 388); 442751 byte; GUID 0a6527460d85cdb44804ef8ebbbd9768; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5253870020984622663, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/MainMenu.unity:2603: m_Sprite: {fileID: 5253870020984622663, guid: 0a6527460d85cdb44804ef8ebbbd9768, type: 3}`

### `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/decoration_crystal_gold.png`

- .png; (277, 307); 81640 byte; GUID 2e15c949d809f6a42a0733ddb717691e; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/frame.png`

- .png; (1086, 1448); 1997008 byte; GUID 6ca6edb624fc96747b198ba92bb1841a; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7501387830980884940, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs, Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:7265: m_Sprite: {fileID: 7501387830980884940, guid: 6ca6edb624fc96747b198ba92bb1841a, type: 3}`
  - `Assets/Scenes/MainMenu.unity:5227: m_Sprite: {fileID: 7501387830980884940, guid: 6ca6edb624fc96747b198ba92bb1841a, type: 3}`

### `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/icon.png`

- .png; (1254, 1254); 866215 byte; GUID a4730db717491864990946acc4761c7e; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/slider.png`

- .png; (1254, 1254); 1002626 byte; GUID 9e9377c9e2a490f4bb2bb71eeb6e1c2a; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6259983639586318526, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/02_ButtonBackgrounds/tile_icon_blue.png`

- .png; (307, 307); 105165 byte; GUID bcaf8cf4d71dae44c95b5a39c1d3f973; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6315862204487574133, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:8582: m_Sprite: {fileID: 6315862204487574133, guid: bcaf8cf4d71dae44c95b5a39c1d3f973, type: 3}`
  - `Assets/Scenes/GamePlay.unity:9146: m_Sprite: {fileID: 6315862204487574133, guid: bcaf8cf4d71dae44c95b5a39c1d3f973, type: 3}`
  - `Assets/Scenes/GamePlay.unity:19020: m_Sprite: {fileID: 6315862204487574133, guid: bcaf8cf4d71dae44c95b5a39c1d3f973, type: 3}`
  - `Assets/Scenes/MainMenu.unity:878: m_Sprite: {fileID: 6315862204487574133, guid: bcaf8cf4d71dae44c95b5a39c1d3f973, type: 3}`
  - `Assets/Scenes/MainMenu.unity:2372: m_Sprite: {fileID: 6315862204487574133, guid: bcaf8cf4d71dae44c95b5a39c1d3f973, type: 3}`
  - `Assets/Scenes/MainMenu.unity:4068: m_Sprite: {fileID: 6315862204487574133, guid: bcaf8cf4d71dae44c95b5a39c1d3f973, type: 3}`

### `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_crystal.png`

- .png; (282, 328); 83431 byte; GUID 048f3b99c7ed6b542b5c00f6e56bafce; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_home.png`

- .png; (304, 282); 90220 byte; GUID 37a83d002a634b849a56dc1651d0f87e; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_music.png`

- .png; (280, 286); 61610 byte; GUID 9ccd4d2a29afedd469881b8bd975ac45; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6308264801367439915, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:20953: m_Sprite: {fileID: 6308264801367439915, guid: 9ccd4d2a29afedd469881b8bd975ac45, type: 3}`
  - `Assets/Scenes/MainMenu.unity:6102: m_Sprite: {fileID: 6308264801367439915, guid: 9ccd4d2a29afedd469881b8bd975ac45, type: 3}`

### `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_restart.png`

- .png; (284, 290); 77014 byte; GUID 331a574c8fc88ab4a96b5774c0fa77b8; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_settings_gear.png`

- .png; (304, 296); 95916 byte; GUID f6069b797d200ab489ca4731323005a6; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_sound.png`

- .png; (328, 273); 83520 byte; GUID 7f5a3fa4f136bfd448479a38bf271865; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 2942687380624916351, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:5528: m_Sprite: {fileID: 2942687380624916351, guid: 7f5a3fa4f136bfd448479a38bf271865, type: 3}`
  - `Assets/Scenes/MainMenu.unity:3512: m_Sprite: {fileID: 2942687380624916351, guid: 7f5a3fa4f136bfd448479a38bf271865, type: 3}`

### `Assets/_Project/_Assets/SkyIslandSetting/03_Icons/icon_vibration.png`

- .png; (324, 290); 95301 byte; GUID 109229f26b340074392e2185a79e1797; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5942659516046825127, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:1499: m_Sprite: {fileID: 5942659516046825127, guid: 109229f26b340074392e2185a79e1797, type: 3}`
  - `Assets/Scenes/MainMenu.unity:4593: m_Sprite: {fileID: 5942659516046825127, guid: 109229f26b340074392e2185a79e1797, type: 3}`

### `Assets/_Project/_Assets/SkyIslandSetting/04_Sliders_Toggles/slider_fill_green.png`

- .png; (925, 126); 93633 byte; GUID e67259eee322a1a41905418d2e49f69b; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 6458808808910329689, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:8719: m_Sprite: {fileID: 6458808808910329689, guid: e67259eee322a1a41905418d2e49f69b, type: 3}`
  - `Assets/Scenes/GamePlay.unity:14317: m_Sprite: {fileID: 6458808808910329689, guid: e67259eee322a1a41905418d2e49f69b, type: 3}`
  - `Assets/Scenes/MainMenu.unity:6523: m_Sprite: {fileID: 6458808808910329689, guid: e67259eee322a1a41905418d2e49f69b, type: 3}`
  - `Assets/Scenes/MainMenu.unity:6598: m_Sprite: {fileID: 6458808808910329689, guid: e67259eee322a1a41905418d2e49f69b, type: 3}`

### `Assets/_Project/_Assets/SkyIslandSetting/04_Sliders_Toggles/slider_handle_crystal.png`

- .png; (235, 306); 94807 byte; GUID 41b9ba68ecbaeee46abe168b27437097; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7064704858591190236, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:4047: m_Sprite: {fileID: 7064704858591190236, guid: 41b9ba68ecbaeee46abe168b27437097, type: 3}`
  - `Assets/Scenes/GamePlay.unity:17051: m_Sprite: {fileID: 7064704858591190236, guid: 41b9ba68ecbaeee46abe168b27437097, type: 3}`
  - `Assets/Scenes/MainMenu.unity:4931: m_Sprite: {fileID: 7064704858591190236, guid: 41b9ba68ecbaeee46abe168b27437097, type: 3}`
  - `Assets/Scenes/MainMenu.unity:7675: m_Sprite: {fileID: 7064704858591190236, guid: 41b9ba68ecbaeee46abe168b27437097, type: 3}`

### `Assets/_Project/_Assets/SkyIslandSetting/04_Sliders_Toggles/slider_track_blue.png`

- .png; (929, 161); 112118 byte; GUID 046e3808edfabb64995a14b694634a74; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7287056034414077285, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:6015: m_Sprite: {fileID: 7287056034414077285, guid: 046e3808edfabb64995a14b694634a74, type: 3}`
  - `Assets/Scenes/GamePlay.unity:18434: m_Sprite: {fileID: 7287056034414077285, guid: 046e3808edfabb64995a14b694634a74, type: 3}`
  - `Assets/Scenes/MainMenu.unity:2501: m_Sprite: {fileID: 7287056034414077285, guid: 046e3808edfabb64995a14b694634a74, type: 3}`
  - `Assets/Scenes/MainMenu.unity:3137: m_Sprite: {fileID: 7287056034414077285, guid: 046e3808edfabb64995a14b694634a74, type: 3}`

### `Assets/_Project/_Assets/SkyIslandSetting/04_Sliders_Toggles/toggle_off.png`

- .png; (447, 230); 101329 byte; GUID 6ca6aa97f1bb2184e86067917c08b5ee; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 3744754425710352626, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:4336: m_Sprite: {fileID: 3744754425710352626, guid: 6ca6aa97f1bb2184e86067917c08b5ee, type: 3}`
  - `Assets/Scenes/MainMenu.unity:501: m_Sprite: {fileID: 3744754425710352626, guid: 6ca6aa97f1bb2184e86067917c08b5ee, type: 3}`

### `Assets/_Project/_Assets/SkyIslandSetting/04_Sliders_Toggles/toggle_on.png`

- .png; (446, 224); 106561 byte; GUID 45b4fa8711c172d47a3519d025bf5103; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5600973481312737296, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/GamePlay.unity:14737: m_Sprite: {fileID: 5600973481312737296, guid: 45b4fa8711c172d47a3519d025bf5103, type: 3}`
  - `Assets/Scenes/MainMenu.unity:4649: m_Sprite: {fileID: 5600973481312737296, guid: 45b4fa8711c172d47a3519d025bf5103, type: 3}`

### `Assets/_Project/_Assets/SkyIslandSetting/05_SpriteSheets/buttons_sheet_transparent.png`

- .png; (1500, 1500); 1073741 byte; GUID 40e5d7380351b544bb094d7a56100792; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/05_SpriteSheets/icons_sheet_transparent.png`

- .png; (1500, 1500); 617092 byte; GUID de43afc82d37bf24ab27193c78255c2e; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/05_SpriteSheets/panel_transparent.png`

- .png; (1086, 1448); 1719387 byte; GUID fc79b26648f9dcd4aaaa74e4a4f9a429; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandSetting/05_SpriteSheets/sliders_toggles_sheet_transparent.png`

- .png; (1450, 1550); 539273 byte; GUID 12b2954087719164a9f222ee18c2c886; nhóm C.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/_Assets/SkyIslandMainMenu/VFX/SoftLight.png`

- .png; (64, 64); 2314 byte; GUID 07fb1a8428ce19745b8cb0d18359ecb9; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 4502769313607820382, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs, Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/MainMenu.unity:576: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:961: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:1201: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:1402: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:1477: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:2229: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:2304: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:2805: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:2920: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:3220: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:3295: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:3370: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:3814: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:3925: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:4314: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:4526: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:5485: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:7331: m_Sprite: {fileID: 4502769313607820382, guid: 07fb1a8428ce19745b8cb0d18359ecb9, type: 3}`

### `Assets/_Project/Materials/Textures/BlueBlock1.png`

- .png; (1024, 1024); 1963 byte; GUID 74826093612d72d4a99ec21043053a8e; nhóm C.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6024109629124664345, 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/BlueBlock2.png`

- .png; (1024, 1024); 1963 byte; GUID 02a12f61e715a234fbb5588780b7e17d; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/BrickBlock1.png`

- .png; (1024, 1024); 37586 byte; GUID 80f33d3037d0cbb4bbee34bfd267bd20; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/BrickBlock2.png`

- .png; (1024, 1024); 36695 byte; GUID 4c3d065b5a1ed5141a41bcf86bce4d5e; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/Checkpoint.png`

- .png; (1024, 1024); 6372 byte; GUID 080cbdb628ff69e4b8e004aecb770a2e; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/Cylinder.png`

- .png; (1024, 1024); 23442 byte; GUID dcf3f88b89e68ab46ab80214509bd130; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/Fan.png`

- .png; (1024, 1024); 6952 byte; GUID 0bfc91482ebe4f64fa07e2c9bed3cff2; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/Flipboard.png`

- .png; (1024, 1024); 9282 byte; GUID e08067b0ba12fc74db9504e0248d2282; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/grass03.png`

- .png; (512, 512); 721478 byte; GUID f05f69ebb14018e4ab8a4fd95d488647; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/GreenCube1.png`

- .png; (1024, 1024); 1963 byte; GUID 1cb12cbbf0ad8184ab723f80aaa33672; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/GreenCube2.png`

- .png; (1024, 1024); 1963 byte; GUID 6575d98f033fcab4497180288e896dc3; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/Lever.png`

- .png; (1024, 1024); 7104 byte; GUID 36ddec0aef1c44941ad3a238d7b7b647; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/MovingBlock.png`

- .png; (1024, 1024); 1963 byte; GUID 9d08ad644ca9f524eb3cd72d469983bb; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/OctagonBrick1.png`

- .png; (1024, 1024); 44320 byte; GUID f79326062bd47504d8cc9089bf63c0a5; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/OctagonBrick2.png`

- .png; (1024, 1024); 43693 byte; GUID b0aabe71e13c110469be0a440c2d07b5; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/OctagonWork1.png`

- .png; (1024, 1024); 6116 byte; GUID dd1b84e4892f4f346942458445f36bf1; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/OctagonWork2.png`

- .png; (1024, 1024); 5817 byte; GUID 6f6189becda3c6e489f00418b0fc7f6c; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/OctagonWork3.png`

- .png; (1024, 1024); 5600 byte; GUID 3e834c2e4bb3e7e46bd2ab23f72db0f9; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/OctagonWork4.png`

- .png; (1024, 1024); 6219 byte; GUID 5279db4fb17f36048bafd130845c31dc; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/OctagonWork5.png`

- .png; (1024, 1024); 5932 byte; GUID 3e819a1bf0bcb294f89b6d8269c2c299; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/OctagonWork6.png`

- .png; (1024, 1024); 5932 byte; GUID 6816dc91712c38046a3f9ca84a1cac48; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/SpeedBlock.png`

- .png; (512, 512); 29685 byte; GUID 560683d7f8c18564ca6082e9ec3f3eca; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/Star.png`

- .png; (1024, 1024); 8703 byte; GUID f2c97d11af2179543855e4bf8239c959; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/Wood.png`

- .png; (1024, 1024); 1906766 byte; GUID 737d53cbc683e854faafe2c54552297d; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/WorkBlock1.png`

- .png; (1024, 1024); 9008 byte; GUID 3aab550a3cc11ee48977dafb9824ea3c; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/WorkBlock2.png`

- .png; (1024, 1024); 8238 byte; GUID f1fd0f366a76a204284e15b015c38694; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/WorkBlock3.png`

- .png; (1024, 1024); 7847 byte; GUID 213489893b31c6d4fb4912afddf3193d; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/WorkBlock4.png`

- .png; (1024, 1024); 9205 byte; GUID 2a342aeedf77108489bae3a2f51ce4ae; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/WorkBlock5.png`

- .png; (1024, 1024); 8684 byte; GUID 1d90044069388b840ac2d001e5fa8de9; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Materials/Textures/WorkBlock6.png`

- .png; (1024, 1024); 8831 byte; GUID c562ef05cc6c33a4597a89e4a4dfac64; nhóm C.
- Import: `{"textureType": "-1", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "?", "sRGBTexture": "?", "filterMode": "-1", "wrapU": "?", "wrapV": "?", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/_Project/Environment/Art/CloudLeafAtlas.png`

- .png; (1254, 1254); 733745 byte; GUID 031964b2aaf4d29458f1d38c69d411e9; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 1932321895275581534, -2516684815595071745, 0
- Build graph: True; Resources graph: False; Scene dependency: Assets/Scenes/GamePlay.unity, Assets/Scenes/MainMenu.unity
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs
- Inbound với fileID/field và vị trí:
  - `Assets/Scenes/MainMenu.unity:425: m_Sprite: {fileID: 1932321895275581534, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:1126: m_Sprite: {fileID: 1932321895275581534, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:2730: m_Sprite: {fileID: 1932321895275581534, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:2995: m_Sprite: {fileID: 1932321895275581534, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:3595: m_Sprite: {fileID: -2516684815595071745, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:3739: m_Sprite: {fileID: 1932321895275581534, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:4000: m_Sprite: {fileID: 1932321895275581534, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:4239: m_Sprite: {fileID: 1932321895275581534, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:5014: m_Sprite: {fileID: 1932321895275581534, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:5697: m_Sprite: {fileID: 1932321895275581534, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:6291: m_Sprite: {fileID: 1932321895275581534, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:6393: m_Sprite: {fileID: 1932321895275581534, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:7019: m_Sprite: {fileID: -2516684815595071745, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/Scenes/MainMenu.unity:7581: m_Sprite: {fileID: -2516684815595071745, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/ReferenceEnvironment.asset:15: cloudSprite: {fileID: 1932321895275581534, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/_Project/_Assets/SkyIslandGameplay/Configs/ReferenceEnvironment.asset:16: leafSprite: {fileID: -2516684815595071745, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/_Project/Environment/Configs/GamePlayEnvironment.asset:15: cloudSprite: {fileID: 1932321895275581534, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/_Project/Environment/Configs/GamePlayEnvironment.asset:16: leafSprite: {fileID: -2516684815595071745, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/_Project/Environment/Configs/MainMenuEnvironment.asset:15: cloudSprite: {fileID: 1932321895275581534, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`
  - `Assets/_Project/Environment/Configs/MainMenuEnvironment.asset:16: leafSprite: {fileID: -2516684815595071745, guid: 031964b2aaf4d29458f1d38c69d411e9, type: 3}`

### `Assets/_Project/Art/AppIcon/SkyBlockPuzzle-Dragon-Icon-v1.png`

- .png; (1254, 1254); 2089167 byte; GUID 26505928d7ad39e41adbc7e923a41cee; nhóm A.
- Import: `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `ProjectSettings/ProjectSettings.asset:308: m_Icon: {fileID: 2800000, guid: 26505928d7ad39e41adbc7e923a41cee, type: 3}`

### `Assets/TutorialInfo/Icons/URP.png`

- .png; (350, 200); 24069 byte; GUID 727a75301c3d24613a3ebcec4a24c2c8; nhóm A.
- Import: `{"textureType": "2", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "0", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 0
- Build graph: False; Resources graph: False; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/Readme.asset:15: icon: {fileID: 2800000, guid: 727a75301c3d24613a3ebcec4a24c2c8, type: 3}`

### `Assets/TextMesh Pro/Sprites/EmojiOne.png`

- .png; (512, 512); 112319 byte; GUID dffef66376be4fa480fb02b19edbe903; nhóm A.
- Import: `{"textureType": "0", "spriteMode": "2", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "-1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: none
- Build graph: False; Resources graph: True; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/TextMesh Pro/Resources/Sprite Assets/EmojiOne.asset:23: m_Texture: {fileID: 2800000, guid: dffef66376be4fa480fb02b19edbe903, type: 3}`
  - `Assets/TextMesh Pro/Resources/Sprite Assets/EmojiOne.asset:74: spriteSheet: {fileID: 2800000, guid: dffef66376be4fa480fb02b19edbe903, type: 3}`

### `Assets/GoogleMobileAds/Editor/Resources/Ump/ConsentForm.png`

- .png; (973, 1574); 239390 byte; GUID cb0d255b8cd434556a80e3ae50b24b9c; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7003779338247311399, 0
- Build graph: False; Resources graph: True; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/GoogleMobileAds/Editor/Resources/Ump/ConsentForm.prefab:472: m_Sprite: {fileID: 21300000, guid: cb0d255b8cd434556a80e3ae50b24b9c, type: 3}`

### `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/AdImages/1024x768.png`

- .png; (1024, 768); 206289 byte; GUID 4c69db43dbd474a2987be5807129b4cc; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -528128221236377446, 0
- Build graph: False; Resources graph: True; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/AppOpen/1024x768.prefab:286: m_Sprite: {fileID: 21300000, guid: 4c69db43dbd474a2987be5807129b4cc, type: 3}`
  - `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/Interstitials/1024x768.prefab:286: m_Sprite: {fileID: 21300000, guid: 4c69db43dbd474a2987be5807129b4cc, type: 3}`
  - `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/Rewarded/1024x768.prefab:377: m_Sprite: {fileID: 21300000, guid: 4c69db43dbd474a2987be5807129b4cc, type: 3}`

### `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/AdImages/300x250.png`

- .png; (300, 250); 46644 byte; GUID 898f886a39d714db5b90d2c36676c894; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6560247188072693145, 0
- Build graph: False; Resources graph: True; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/Banners/MEDIUM_RECTANGLE.prefab:69: m_Sprite: {fileID: 21300000, guid: 898f886a39d714db5b90d2c36676c894, type: 3}`

### `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/AdImages/320x100.png`

- .png; (320, 100); 27345 byte; GUID 1a2723ddf964e4565a408007ab387283; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 7095944869193769577, 0
- Build graph: False; Resources graph: True; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/Banners/LARGE_BANNER.prefab:69: m_Sprite: {fileID: 21300000, guid: 1a2723ddf964e4565a408007ab387283, type: 3}`

### `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/AdImages/320x480.png`

- .png; (320, 480); 64507 byte; GUID 77d9c04b43d404f44ab60d7159b279bb; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 8217279028561276190, 0
- Build graph: False; Resources graph: True; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/AdImages/320x50.png`

- .png; (320, 50); 14916 byte; GUID 5a6531d4c9c484d679a2d6f9e6fefed5; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -2832473847120061193, 0
- Build graph: False; Resources graph: True; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/Banners/BANNER.prefab:69: m_Sprite: {fileID: 21300000, guid: 5a6531d4c9c484d679a2d6f9e6fefed5, type: 3}`

### `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/AdImages/468x60.png`

- .png; (468, 60); 20218 byte; GUID 74358f5fb13a549aabac4a5a7ac76739; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -6169817065691750558, 0
- Build graph: False; Resources graph: True; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/Banners/FULL_BANNER.prefab:90: m_Sprite: {fileID: 21300000, guid: 74358f5fb13a549aabac4a5a7ac76739, type: 3}`

### `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/AdImages/480x320.png`

- .png; (480, 320); 67392 byte; GUID d30303c99ba3f4b9b9924da34e6d742d; nhóm B.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -9152142378645801974, 0
- Build graph: False; Resources graph: True; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:


### `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/AdImages/728x90.png`

- .png; (728, 90); 32942 byte; GUID cfead63ff9d9d48e2b4ba3c2dee7cea9; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: 5850312064992421762, 0
- Build graph: False; Resources graph: True; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/Banners/LEADERBOARD.prefab:147: m_Sprite: {fileID: 21300000, guid: cfead63ff9d9d48e2b4ba3c2dee7cea9, type: 3}`

### `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/AdImages/768x1024.png`

- .png; (768, 1024); 181896 byte; GUID 8a53f9c63a5e94b14a98698a7b07365b; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -3030144160498305896, 0
- Build graph: False; Resources graph: True; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/AppOpen/768x1024.prefab:249: m_Sprite: {fileID: 21300000, guid: 8a53f9c63a5e94b14a98698a7b07365b, type: 3}`
  - `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/Interstitials/768x1024.prefab:249: m_Sprite: {fileID: 21300000, guid: 8a53f9c63a5e94b14a98698a7b07365b, type: 3}`
  - `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/Rewarded/768x1024.prefab:202: m_Sprite: {fileID: 21300000, guid: 8a53f9c63a5e94b14a98698a7b07365b, type: 3}`

### `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/AdImages/AdInspectorHome.png`

- .png; (645, 1395); 33310 byte; GUID d271d6a1aed6848559ecad6252e333b1; nhóm A.
- Import: `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- Subasset internalIDs từ meta: -95186541036295939, 0
- Build graph: False; Resources graph: True; Scene dependency: none
- Outgoing dependency: none
- Tin cậy: reference A xác định qua text; B/C cần kiểm tra thủ công.
- Code string/path/token/prefix (chỉ heuristic, có thể output generator): none
- Inbound với fileID/field và vị trí:
  - `Assets/GoogleMobileAds/Editor/Resources/PlaceholderAds/AdInspector/768x1024.prefab:451: m_Sprite: {fileID: 21300000, guid: d271d6a1aed6848559ecad6252e333b1, type: 3}`

## Loader/Editor/Reflection

- `Assets/_Project/Editor/AdMobChecks.cs:27: var config = UnityEngine.Resources.Load<AdsConfiguration>("AdsConfiguration");`
- `Assets/_Project/Editor/AudioIntegrationChecks.cs:23: static T Get<T>(object o,string n)=>(T)o.GetType().GetField(n,F).GetValue(o);`
- `Assets/_Project/Editor/AudioIntegrationChecks.cs:73: var board=GridBoard.Instance;board.ResetBoard();shape=ScriptableObject.CreateInstance<ShapeData>();shape.rows=shape.columns=1;typeof(ShapeData).GetField("matrixData",F).SetValue(shape,new[]{true});var runtime=shape.CreateRuntime();`
- `Assets/_Project/Editor/AudioIntegrationChecks.cs:90: var prefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Project/Prefabs/Audio/AudioManager.prefab");`
- `Assets/_Project/Editor/AudioIntegrationInstaller.cs:30: foreach(string guid in AssetDatabase.FindAssets("t:AudioClip",new[]{"Assets/_Project/Audio"}))`
- `Assets/_Project/Editor/AudioIntegrationInstaller.cs:32: string path=AssetDatabase.GUIDToAssetPath(guid);var clip=AssetDatabase.LoadAssetAtPath<AudioClip>(path);`
- `Assets/_Project/Editor/AudioIntegrationInstaller.cs:39: if(!AssetDatabase.IsValidFolder("Assets/_Project/Prefabs/Audio"))AssetDatabase.CreateFolder("Assets/_Project/Prefabs","Audio");`
- `Assets/_Project/Editor/AudioIntegrationInstaller.cs:40: GameObject prefab=AssetDatabase.LoadAssetAtPath<GameObject>(Prefab);`
- `Assets/_Project/Editor/AudioIntegrationInstaller.cs:49: var clip=AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/_Project/Audio/"+files[i]+".mp3");`
- `Assets/_Project/Editor/AudioIntegrationInstaller.cs:83: AssetDatabase.SaveAssets();`
- `Assets/_Project/Editor/BlockBlastRegressionChecks.cs:20: target.GetType().GetField(name, Private).SetValue(target, value);`
- `Assets/_Project/Editor/BlockBlastRegressionChecks.cs:22: target.GetType().GetField(name, Private).GetValue(target);`
- `Assets/_Project/Editor/BlockBlastRegressionChecks.cs:44: var prefab = AssetDatabase.LoadAssetAtPath<GridCell>("Assets/_Project/Prefabs/Shapes/GridCell.prefab");`
- `Assets/_Project/Editor/BlockInteractionChecks.cs:25: static T Field<T>(object o, string name) => (T)o.GetType().GetField(name, Flags).GetValue(o);`
- `Assets/_Project/Editor/BubblePopRegressionChecks.cs:22: var source = AssetDatabase.LoadAssetAtPath<BlockSpriteSet>("Assets/_Project/ScriptableObjects/BlockSpriteSet.asset");`
- `Assets/_Project/Editor/BubblePopRegressionChecks.cs:27: var field = typeof(BlockSpriteSet).GetField("sprites", Private);`
- `Assets/_Project/Editor/BubblePopRegressionChecks.cs:44: var config = UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<BubblePopConfig>(`
- `Assets/_Project/Editor/BubblePopRegressionChecks.cs:58: var sprites = AssetDatabase.LoadAssetAtPath<BlockSpriteSet>("Assets/_Project/ScriptableObjects/BlockSpriteSet.asset");`
- `Assets/_Project/Editor/BubblePopRegressionChecks.cs:76: var block = (SpriteRenderer)typeof(BubblePopEffect).GetField("blockVisual", Private).GetValue(reused);`
- `Assets/_Project/Editor/BubblePopRegressionChecks.cs:98: var prefab = AssetDatabase.LoadAssetAtPath<GridCell>("Assets/_Project/Prefabs/Shapes/GridCell.prefab");`
- `Assets/_Project/Editor/BubblePopRegressionChecks.cs:103: typeof(ShapeData).GetField("matrixData", Private).SetValue(shape, new[] { true });`
- `Assets/_Project/Editor/BubblePopRegressionChecks.cs:104: var sprite = AssetDatabase.LoadAssetAtPath<BlockSpriteSet>("Assets/_Project/ScriptableObjects/BlockSpriteSet.asset").SelectByIndex(2);`
- `Assets/_Project/Editor/BubblePopRegressionChecks.cs:126: typeof(GridBoard).GetField("grid", Private).SetValue(board, cells);`
- `Assets/_Project/Editor/BubblePopRegressionChecks.cs:137: var nextSprite = AssetDatabase.LoadAssetAtPath<BlockSpriteSet>("Assets/_Project/ScriptableObjects/BlockSpriteSet.asset").SelectByIndex(3);`
- `Assets/_Project/Editor/DifficultyChecks.cs:48: var config=AssetDatabase.LoadAssetAtPath<DifficultyConfig>(DifficultyInstaller.ConfigPath);Check(config!=null,"Missing config");`
- `Assets/_Project/Editor/DifficultyInstaller.cs:15: var config=AssetDatabase.LoadAssetAtPath<DifficultyConfig>(ConfigPath);`
- `Assets/_Project/Editor/DifficultyInstaller.cs:16: if(config==null){config=ScriptableObject.CreateInstance<DifficultyConfig>();AssetDatabase.CreateAsset(config,ConfigPath);}`
- `Assets/_Project/Editor/DifficultyInstaller.cs:39: AssetDatabase.SaveAssets();return "DDA installed; Easy="+shapes.Count(s=>s.difficulty==ShapeDifficulty.Easy)+", Medium="+shapes.Count(s=>s.difficulty==ShapeDifficulty.Medium)+", Hard="+shapes.Count(s=>s.difficulty==ShapeDifficulty.Hard);`
- `Assets/_Project/Editor/DifficultyPlayChecks.cs:24: static float Offset(DraggableShape p)=>(float)typeof(DraggableShape).GetField("dragOffsetYScreen",Flags).GetValue(p);`
- `Assets/_Project/Editor/DragonMascotChecks.cs:38: shape=ScriptableObject.CreateInstance<ShapeData>();shape.rows=shape.columns=1;shape.rotationPolicy=RotationPolicy.NoRotation;typeof(ShapeData).GetField("matrixData",BindingFlags.Instance|BindingFlags.NonPublic).SetValue(shape,new[]{true});`
- `Assets/_Project/Editor/GameOverChecks.cs:20: private static object Get(object target,string name) => target.GetType().GetField(name,Private).GetValue(target);`
- `Assets/_Project/Editor/GameOverChecks.cs:21: private static void Set(object target,string name,object value) => target.GetType().GetField(name,Private).SetValue(target,value);`
- `Assets/_Project/Editor/GameOverInstaller.cs:67: var theme = AssetDatabase.LoadAssetAtPath<UITheme>("Assets/_Project/ScriptableObjects/UITheme.asset");`
- `Assets/_Project/Editor/GameplayUIInstaller.cs:6: static Sprite Asset(string name)=>AssetDatabase.LoadAssetAtPath<Sprite>(Root+"/"+name+".png");`
- `Assets/_Project/Editor/GameplayUIInstaller.cs:7: static Sprite SpriteAt(string path,string name=null)=>AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().First(s=>name==null||s.name==name);`
- `Assets/_Project/Editor/GameplayUIInstaller.cs:13: pixels[y*n+x]=c;}tex.SetPixels(pixels);tex.Apply();File.WriteAllBytes(Root+"/"+name+".png",tex.EncodeToPNG());UnityEngine.Object.DestroyImmediate(tex);AssetDatabase.ImportAsset(Root+"/"+name+".png");var imp=(TextureImporter)AssetImporter.GetAtPath(Root+"/"+name+".png");imp.textureType=TextureImporterType.Sprite;imp.spriteImportMode=SpriteImportMode.Single;imp.spritePixelsPerUnit=100;var settings=new TextureImporterSettings();imp.ReadTextureSettings(settings);settings.spriteMeshType=SpriteMeshType.FullRect;imp.SetTextureSettings(settings);imp.filterMode=FilterMode.Bilinear;imp.textureCompression=TextureImporterCompression.Uncompressed;imp.mipmapEnabled=false;imp.spriteBorder=new Vector4(Mathf.Max(radius,border+2),Mathf.Max(radius,border+2),Mathf.Max(radius,border+2),Mathf.Max(radius,border+2));imp.SaveAndReimport();return Asset(name);}`
- `Assets/_Project/Editor/GameplayUIInstaller.cs:15: public static void GenerateArt(){Directory.CreateDirectory(Root);theme=AssetDatabase.LoadAssetAtPath<UITheme>("Assets/_Project/ScriptableObjects/UITheme.asset");`
- `Assets/_Project/Editor/GameplayUIInstaller.cs:23: EditorUtility.SetDirty(theme);AssetDatabase.SaveAssets();`
- `Assets/_Project/Editor/GameplayUIInstaller.cs:25: SpriteAtlasExtensions.Add(atlas,new UnityEngine.Object[]{AssetDatabase.LoadAssetAtPath<DefaultAsset>(Root),AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_Project/_Assets/UI_game_nen_trong_suot.png")});`
- `Assets/_Project/Editor/GameplayUIInstaller.cs:26: if(AssetDatabase.LoadAssetAtPath<SpriteAtlas>("Assets/_Project/UI/Gameplay.spriteatlas")==null)AssetDatabase.CreateAsset(atlas,"Assets/_Project/UI/Gameplay.spriteatlas");else UnityEngine.Object.DestroyImmediate(atlas);`
- `Assets/_Project/Editor/GameplayUIInstaller.cs:34: public static string Install(){theme=AssetDatabase.LoadAssetAtPath<UITheme>("Assets/_Project/ScriptableObjects/UITheme.asset");var scene=UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();if(scene.isDirty)throw new InvalidOperationException("Save/review current scene before install.");`
- `Assets/_Project/Editor/GameplayUIInstaller.cs:57: var traySO=new SerializedObject(tray);var shapePath=AssetDatabase.GetAssetPath(traySO.FindProperty("shapePrefab").objectReferenceValue);var shapeRoot=PrefabUtility.LoadPrefabContents(shapePath);var pv=shapeRoot.AddComponent<PieceView>();Set(pv,"theme",theme);PrefabUtility.SaveAsPrefabAsset(shapeRoot,shapePath);PrefabUtility.UnloadPrefabContents(shapeRoot);`
- `Assets/_Project/Editor/GameplayUIInstaller.cs:69: var config=AssetDatabase.LoadAssetAtPath<BlockBlast.Effects.BubblePopConfig>("Assets/_Project/ScriptableObjects/BubblePopConfig.asset");config.bubbleSprite=theme.squareParticle;config.bubbleCount=8;config.duration=.28f;config.inflateFraction=.08f/.28f;config.staggerDelay=0;config.maximumStagger=0;EditorUtility.SetDirty(config);`
- `Assets/_Project/Editor/GameplayUIInstaller.cs:70: var cellPath=AssetDatabase.GetAssetPath(new SerializedObject(board).FindProperty("cellPrefab").objectReferenceValue);var cellRoot=PrefabUtility.LoadPrefabContents(cellPath);var cellView=cellRoot.AddComponent<CellView>();Set(cellView,"theme",theme);PrefabUtility.SaveAsPrefabAsset(cellRoot,cellPath);PrefabUtility.UnloadPrefabContents(cellRoot);`
- `Assets/_Project/Editor/GameplayUIInstaller.cs:71: AssetDatabase.SaveAssets();UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);return "Installed gameplay UI";`
- `Assets/_Project/Editor/IslandEnvironmentChecks.cs:18: static T Get<T>(object o,string field)=>(T)o.GetType().GetField(field,F).GetValue(o);`
- `Assets/_Project/Editor/IslandEnvironmentInstaller.cs:31: var sprites=AssetDatabase.LoadAllAssetsAtPath(importer.assetPath).OfType<Sprite>().ToArray();`
- `Assets/_Project/Editor/IslandEnvironmentInstaller.cs:34: var material=AssetDatabase.LoadAssetAtPath<Material>(materialPath);`
- `Assets/_Project/Editor/IslandEnvironmentInstaller.cs:35: if(material==null){material=new Material(Shader.Find("SkyBlock/IslandWaterUnlit"));AssetDatabase.CreateAsset(material,materialPath);}`
- `Assets/_Project/Editor/IslandEnvironmentInstaller.cs:36: if(!AssetDatabase.IsValidFolder(Root+"/Backups"))AssetDatabase.CreateFolder(Root,"Backups");`
- `Assets/_Project/Editor/IslandEnvironmentInstaller.cs:49: var config=AssetDatabase.LoadAssetAtPath<IslandEnvironmentSettings>(configPath);`
- `Assets/_Project/Editor/IslandEnvironmentInstaller.cs:56: AssetDatabase.CreateAsset(config,configPath);`
- `Assets/_Project/Editor/IslandEnvironmentInstaller.cs:59: var prefab=AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);`
- `Assets/_Project/Editor/IslandEnvironmentInstaller.cs:74: SceneManager.SetActiveScene(active);AssetDatabase.SaveAssets();`
- `Assets/_Project/Editor/LineClearChecks.cs:29: static T Get<T>(object o, string name) => (T)o.GetType().GetField(name, F).GetValue(o);`
- `Assets/_Project/Editor/LineClearChecks.cs:35: typeof(ShapeData).GetField("matrixData", F).SetValue(shape, Enumerable.Repeat(true, rows * cols).ToArray());`
- `Assets/_Project/Editor/LineClearChecks.cs:176: var prefab = AssetDatabase.LoadAssetAtPath<BubblePopConfig>("Assets/_Project/ScriptableObjects/BubblePopConfig.asset");`
- `Assets/_Project/Editor/OfflineAdsChecks.cs:34: static readonly FieldInfo Network = typeof(AdsManager).GetField("NetworkOverrideForTests", BindingFlags.Static | BindingFlags.NonPublic);`
- `Assets/_Project/Editor/OfflineAdsChecks.cs:48: static T Get<T>(object target, string name) => (T)target.GetType().GetField(name, Fields).GetValue(target);`
- `Assets/_Project/Editor/OfflineAdsChecks.cs:49: static void Set(object target, string name, object value) => target.GetType().GetField(name, Fields).SetValue(target, value);`
- `Assets/_Project/Editor/ReferenceGameplayArt.cs:36: UnityEngine.Object.DestroyImmediate(tex);AssetDatabase.ImportAsset(path);`
- `Assets/_Project/Editor/ReferenceGameplayArt.cs:49: source=new Texture2D(2,2);source.LoadImage(File.ReadAllBytes("Assets/_Project/sprite/Bộ giao diện game xếp khối .png"));`
- `Assets/_Project/Editor/ReferenceGameplayArt.cs:80: AssetDatabase.Refresh();`
- `Assets/_Project/Editor/ReferenceGameplayArt.cs:85: purple.LoadImage(File.ReadAllBytes(Folder+"/BlockPink.png"));`
- `Assets/_Project/Editor/ReferenceGameplayArt.cs:94: var tex=new Texture2D(2,2);tex.LoadImage(File.ReadAllBytes(Folder+"/Block"+name+".png"));`
- `Assets/_Project/Editor/ReferenceGameplayInstaller.cs:17: private static Sprite Art(string name) => AssetDatabase.LoadAssetAtPath<Sprite>(ReferenceGameplayArt.Folder+"/"+name+".png");`
- `Assets/_Project/Editor/ReferenceGameplayInstaller.cs:79: var theme=AssetDatabase.LoadAssetAtPath<UITheme>("Assets/_Project/ScriptableObjects/UITheme.asset");`
- `Assets/_Project/Editor/ReferenceGameplayInstaller.cs:102: var atlas=AssetDatabase.LoadAssetAtPath<SpriteAtlas>("Assets/_Project/UI/Gameplay.spriteatlas");`
- `Assets/_Project/Editor/ReferenceGameplayInstaller.cs:103: SpriteAtlasExtensions.Add(atlas,new UnityEngine.Object[]{AssetDatabase.LoadAssetAtPath<DefaultAsset>(ReferenceGameplayArt.Folder)});`
- `Assets/_Project/Editor/ReferenceGameplayInstaller.cs:104: EditorUtility.SetDirty(atlas);AssetDatabase.SaveAssets();EditorSceneManager.SaveScene(scene);`
- `Assets/_Project/Editor/ScoringChecks.cs:19: Action<string,int> set=(n,v)=>typeof(GameManager).GetField("<"+n+">k__BackingField",flags).SetValue(m,v);`
- `Assets/_Project/Editor/ShapeLibraryPlayChecks.cs:28: static T Field<T>(object o,string n)=>(T)o.GetType().GetField(n,Flags).GetValue(o);`
- `Assets/_Project/Editor/ShapeLibraryRebuild.cs:23: public static ShapeData[] Load() => Names.Select((name,i) => AssetDatabase.LoadAssetAtPath<ShapeData>(PathFor(i))).ToArray();`
- `Assets/_Project/Editor/ShapeLibraryRebuild.cs:59: var oldGuids=old.Select(AssetDatabase.AssetPathToGUID).ToArray();`
- `Assets/_Project/Editor/ShapeLibraryRebuild.cs:60: if(!AssetDatabase.IsValidFolder(Folder+"/Core"))AssetDatabase.CreateFolder(Folder,"Core");`
- `Assets/_Project/Editor/ShapeLibraryRebuild.cs:61: if(!AssetDatabase.IsValidFolder(Folder+"/Diagonal"))AssetDatabase.CreateFolder(Folder,"Diagonal");`
- `Assets/_Project/Editor/ShapeLibraryRebuild.cs:64: if(AssetDatabase.LoadAssetAtPath<ShapeData>(PathFor(i))!=null)throw new InvalidOperationException("New catalog already exists; refusing overwrite: "+PathFor(i));`
- `Assets/_Project/Editor/ShapeLibraryRebuild.cs:66: AssetDatabase.CreateAsset(shape,PathFor(i));`
- `Assets/_Project/Editor/ShapeLibraryRebuild.cs:68: AssetDatabase.SaveAssets();var shapes=Load();Validate();`
- `Assets/_Project/Editor/ShapeLibraryRebuild.cs:83: foreach(string p in AssetDatabase.GetAllAssetPaths().Where(p=>p.StartsWith("Assets/")&&(p.EndsWith(".unity")||p.EndsWith(".prefab")||p.EndsWith(".asset"))))`
- `Assets/_Project/Editor/ShapeLibraryRebuild.cs:89: foreach(string path in old)if(!AssetDatabase.DeleteAsset(path))throw new InvalidOperationException("Could not remove old shape "+path);`
- `Assets/_Project/Editor/ShapeLibraryRebuild.cs:90: AssetDatabase.SaveAssets();`
- `Assets/_Project/Editor/UIArtVariants.cs:5: static Sprite Save(string name,Texture2D texture){var path=Root+name+".png";File.WriteAllBytes(path,texture.EncodeToPNG());Object.DestroyImmediate(texture);AssetDatabase.ImportAsset(path);var imp=(TextureImporter)AssetImporter.GetAtPath(path);imp.textureType=TextureImporterType.Sprite;imp.spriteImportMode=SpriteImportMode.Single;imp.filterMode=FilterMode.Bilinear;imp.textureCompression=TextureImporterCompression.Uncompressed;imp.mipmapEnabled=false;imp.spritePixelsPerUnit=100;var settings=new TextureImporterSettings();imp.ReadTextureSettings(settings);settings.spriteMeshType=SpriteMeshType.FullRect;imp.SetTextureSettings(settings);imp.SaveAndReimport();return AssetDatabase.LoadAssetAtPath<Sprite>(path);}`
- `Assets/_Project/Editor/UIArtVariants.cs:6: public static string Generate(){var theme=AssetDatabase.LoadAssetAtPath<UITheme>("Assets/_Project/ScriptableObjects/UITheme.asset");var source=Read(theme.blockSprites[0].texture,theme.blockSprites[0].texture.width,theme.blockSprites[0].texture.height);`
- `Assets/_Project/Editor/UIArtVariants.cs:8: var background=GameObject.Find("Background").GetComponent<SpriteRenderer>().sprite;var full=Read(background.texture,background.texture.width,background.texture.height);var region=new Texture2D((int)background.rect.width,(int)background.rect.height,TextureFormat.RGBA32,false);region.SetPixels(full.GetPixels((int)background.rect.x,(int)background.rect.y,region.width,region.height));region.Apply();Object.DestroyImmediate(full);var blur=Read(region,415,430);Object.DestroyImmediate(region);for(int pass=0;pass<3;pass++){var old=blur.GetPixels();var output=new Color[old.Length];for(int y=0;y<blur.height;y++)for(int x=0;x<blur.width;x++){Color sum=Color.clear;int count=0;for(int dy=-3;dy<=3;dy++)for(int dx=-3;dx<=3;dx++){sum+=old[Mathf.Clamp(y+dy,0,blur.height-1)*blur.width+Mathf.Clamp(x+dx,0,blur.width-1)];count++;}output[y*blur.width+x]=sum/count;}blur.SetPixels(output);blur.Apply();}Save("BackgroundSoft",blur);EditorUtility.SetDirty(theme);AssetDatabase.SaveAssets();return "16 state variants and baked blur";}`
- `Assets/_Project/Editor/UIRegressionChecks.cs:11: var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GridCell>(`
- `Assets/_Project/Editor/UIRegressionChecks.cs:13: var theme = UnityEngine.Object.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<BlockBlast.UI.UITheme>(`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs:13: static Sprite[] Frames(string name) => AssetDatabase.LoadAllAssetsAtPath(Root+name+".png").OfType<Sprite>().OrderByDescending(s=>s.rect.y).ThenBy(s=>s.rect.x).ToArray();`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs:17: var clip=AssetDatabase.LoadAssetAtPath<AnimationClip>(path);`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs:27: if(create)AssetDatabase.CreateAsset(clip,path);else EditorUtility.SetDirty(clip);return clip;`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs:38: var controller=AssetDatabase.LoadAssetAtPath<AnimatorController>(path)??AnimatorController.CreateAnimatorControllerAtPath(path);`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/DragonSheetInstaller.cs:52: EditorSceneManager.MarkSceneDirty(p.gameObject.scene);EditorSceneManager.SaveScene(p.gameObject.scene);AssetDatabase.SaveAssets();`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs:27: static Sprite Sprite(string name) => AssetDatabase.LoadAllAssetsAtPath(Root + name + ".png").OfType<Sprite>().First();`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs:37: var material = AssetDatabase.LoadAssetAtPath<Material>(path);`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs:38: if (material == null) { material = new Material(Shader.Find(shader)); AssetDatabase.CreateAsset(material, path); }`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs:49: t.Apply(); File.WriteAllBytes(path, t.EncodeToPNG()); UnityEngine.Object.DestroyImmediate(t); AssetDatabase.ImportAsset(path);`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs:52: return AssetDatabase.LoadAssetAtPath<Sprite>(path);`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs:64: AssetDatabase.Refresh();`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs:85: var cloudSprites = AssetDatabase.LoadAllAssetsAtPath("Assets/_Project/Environment/Art/CloudLeafAtlas.png").OfType<Sprite>().ToArray();`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs:120: EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs:127: var controller=AssetDatabase.LoadAssetAtPath<AnimatorController>(path);if(controller!=null)return controller;`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs:133: AssetDatabase.CreateAsset(idle,Root+"Animations/ExplorerIdle.anim");controller.AddMotion(idle);`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs:137: AssetDatabase.CreateAsset(depart,Root+"Animations/ExplorerDepart.anim");controller.AddMotion(depart);return controller;`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs:141: var prefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Project/Prefabs/UI/GameplayScreen.prefab");`
- `Assets/_Project/_Assets/SkyIslandMainMenu/Editor/MenuInstaller.cs:160: EditorSceneManager.MarkSceneDirty(p.gameObject.scene);EditorSceneManager.SaveScene(p.gameObject.scene);AssetDatabase.SaveAssets();`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs:11: static Sprite S(string n)=>AssetDatabase.LoadAllAssetsAtPath(R+n+".png").OfType<Sprite>().First();`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs:19: System.IO.Directory.CreateDirectory(R+"Configs");System.IO.Directory.CreateDirectory(R+"Prefabs");AssetDatabase.Refresh();`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs:20: var view=Object.FindFirstObjectByType<BoardView>();var old=view.Theme;var theme=Object.Instantiate(old);AssetDatabase.CreateAsset(theme,R+"Configs/GameplayTheme.asset");theme.cellSprite=S("BoardCell");theme.frameSprite=S("BoardFrame");theme.scaleFrameArtwork=true;theme.frameArtworkInsets=new Vector4(.14f,.14f,.14f,.17f);theme.overlay=Color.clear;theme.boardMaxWidth=1200;`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayArtInstaller.cs:41: EditorUtility.SetDirty(theme);EditorSceneManager.MarkSceneDirty(board.gameObject.scene);EditorSceneManager.SaveScene(board.gameObject.scene);AssetDatabase.SaveAssets();return "Static gameplay layout installed";`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayEditModeChecks.cs:44: color=r!=null?r.color:Color.white,sprite=r!=null&&r.sprite!=null?AssetDatabase.GetAssetPath(r.sprite):"" }; }).OrderBy(e=>e.path).ToArray()`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceChecks.cs:85: var button = (Button)typeof(SettingsPanel).GetField("open", Private).GetValue(settings);`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs:62: var skyMaterial = AssetDatabase.LoadAssetAtPath<Material>(skyPath);`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs:66: AssetDatabase.CreateAsset(skyMaterial, skyPath);`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs:73: var environmentSettings = AssetDatabase.LoadAssetAtPath<BlockBlast.Environment.IslandEnvironmentSettings>(environmentPath);`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs:77: AssetDatabase.CreateAsset(environmentSettings, environmentPath);`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs:85: var profile = AssetDatabase.LoadAssetAtPath<UnityEngine.Rendering.VolumeProfile>(profilePath);`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs:88: AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(volume.sharedProfile), profilePath);`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs:89: profile = AssetDatabase.LoadAssetAtPath<UnityEngine.Rendering.VolumeProfile>(profilePath);`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs:121: AssetDatabase.SaveAssets();`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs:133: var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(Root + "BoardFrame.png");`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs:136: if (!AssetDatabase.IsValidFolder(Root + "FrameSlices")) AssetDatabase.CreateFolder(Root.TrimEnd('/'), "FrameSlices");`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs:142: var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/GameplayReferenceLayout.cs:146: sprite.name = name; AssetDatabase.CreateAsset(sprite, path);`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/TrayFeedbackChecks.cs:39: var baseline=(Vector3)typeof(CellView).GetField("restScale",BindingFlags.NonPublic|BindingFlags.Instance).GetValue(cv);`
- `Assets/_Project/_Assets/SkyIslandGameplay/Editor/TrayFeedbackChecks.cs:42: var glow=(SpriteRenderer)typeof(CellView).GetField("placementGlow",BindingFlags.NonPublic|BindingFlags.Instance).GetValue(cv);`
- `Assets/_Project/Scripts/AdManager/AdsManager.cs:72: if (configuration == null) configuration = Resources.Load<AdsConfiguration>("AdsConfiguration");`
- `Assets/_Project/Scripts/Shape/TrayManager.cs:43: if (difficultyConfig == null) difficultyConfig = Resources.Load<DifficultyConfig>("DifficultyConfig");`
- `Assets/TutorialInfo/Scripts/Editor/ReadmeEditor.cs:45: var path = AssetDatabase.GetAssetPath(readmeAsset);`
- `Assets/TutorialInfo/Scripts/Editor/ReadmeEditor.cs:50: AssetDatabase.Refresh();`
- `Assets/TutorialInfo/Scripts/Editor/ReadmeEditor.cs:79: var ids = AssetDatabase.FindAssets("Readme t:Readme");`
- `Assets/TutorialInfo/Scripts/Editor/ReadmeEditor.cs:82: var readmeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));`
- `Assets/GoogleMobileAds/Editor/AndroidBuildPreProcessor.cs:93: if (!AssetDatabase.IsValidFolder(Path.Combine("Assets", "Plugins")))`
- `Assets/GoogleMobileAds/Editor/AndroidBuildPreProcessor.cs:95: AssetDatabase.CreateFolder("Assets", "Plugins");`
- `Assets/GoogleMobileAds/Editor/AndroidBuildPreProcessor.cs:96: AssetDatabase.Refresh();`
- `Assets/GoogleMobileAds/Editor/AndroidBuildPreProcessor.cs:100: if (!AssetDatabase.IsValidFolder(Path.Combine("Assets", "Plugins", "Android")))`
- `Assets/GoogleMobileAds/Editor/AndroidBuildPreProcessor.cs:102: AssetDatabase.CreateFolder(Path.Combine("Assets", "Plugins"), "Android");`
- `Assets/GoogleMobileAds/Editor/AndroidBuildPreProcessor.cs:103: AssetDatabase.Refresh();`
- `Assets/GoogleMobileAds/Editor/AndroidBuildPreProcessor.cs:179: AssetDatabase.Refresh();`
- `Assets/GoogleMobileAds/Editor/AndroidBuildPreProcessor.cs:205: AssetDatabase.Refresh();`
- `Assets/GoogleMobileAds/Editor/AndroidBuildPreProcessor.cs:253: AssetDatabase.Refresh();`
- `Assets/GoogleMobileAds/Editor/BuildPreProcessor.cs:36: if (!AssetDatabase.IsValidFolder(Path.Combine("Assets", "GoogleMobileAds")))`
- `Assets/GoogleMobileAds/Editor/BuildPreProcessor.cs:38: AssetDatabase.CreateFolder("Assets", "GoogleMobileAds");`
- `Assets/GoogleMobileAds/Editor/BuildPreProcessor.cs:50: AssetDatabase.CopyAsset(linkXmlPackagePath, _linkXmlAssetsPath);`
- `Assets/GoogleMobileAds/Editor/BuildPreProcessor.cs:52: AssetDatabase.Refresh();`
- `Assets/GoogleMobileAds/Editor/EditorPathUtils.cs:29: return AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));`
- `Assets/GoogleMobileAds/Editor/GoogleMobileAdsSettings.cs:25: var instance = Resources.Load<GoogleMobileAdsSettings>(MobileAdsSettingsFile);`
- `Assets/GoogleMobileAds/Editor/GoogleMobileAdsSettings.cs:34: AssetDatabase.CreateAsset(instance, assetPath);`
- `Assets/GoogleMobileAds/Editor/GoogleMobileAdsSettings.cs:35: AssetDatabase.SaveAssets();`
- `Assets/GoogleMobileAds/Editor/ManifestProcessor.cs:84: if (AssetDatabase.IsValidFolder("Packages/com.google.ads.mobile")) {`

## Không đọc được



Lưu ý lọc code: không coi prefix chung Assets/_Project/ trong ShapeLibraryRebuild (chỉ quản lý ShapeData) là loader ảnh; chỉ tính prefix cụ thể trong script có thao tác load ảnh. Token tên/prefix vẫn là khả năng dùng, không phải bằng chứng runtime.
