> **Cập nhật Unity MCP:** đã xác minh AssetDatabase; xem [09_UNITY_VERIFICATION_REPORT.md](09_UNITY_VERIFICATION_REPORT.md) và JSON 06–08. Phần dưới là baseline static trước khi kết nối; các giới hạn “chưa có MCP” chỉ áp dụng lần quét trước. Chưa chạy Play Mode.

# Audit Sky Block Puzzle — 14/11/2026

CHỈ ĐỌC: working tree hiện tại, không sửa/xóa Assets/meta/import/scene/prefab/code. Không có Unity MCP; chưa gọi AssetDatabase API hoặc chạy runtime. Không thêm công cụ Editor. Graph GUID/fileID text/meta + atlas folder + Resources + chuỗi C# không bao phủ hết binary/package/loader ghép chuỗi/reflection. CẦN KIỂM TRA THỦ CÔNG các trường hợp chưa rõ. A=có serialized reference, không khẳng định đang hiển thị; B=code/resource có khả năng dùng; C=chưa thấy reference, KHÔNG an toàn xóa mặc định. D trùng lặp là nhãn độc lập.

## Duplicate byte SHA-256

### Nhóm 1

- `Assets/_Project/_Assets/Black.png` — C; SHA 63f38affd0d0af77e49913860ac179420ebbb52a00ea3bf0285de5cc3701db21; importerHash 5608fc09bc27723f2620d060c783354c8d56264d2f2fff5b00608349ece5c15d; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/Black_Normal.png` — C; SHA 63f38affd0d0af77e49913860ac179420ebbb52a00ea3bf0285de5cc3701db21; importerHash 75aa64cc246ecdb34c72e4f3f77d45f5897bca9b4de1f8f235ff032591744a36; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 2

- `Assets/_Project/_Assets/Blue.png` — A; SHA 4178050008e64ec17dc18100ae074ba6077147e4dab70433e177a992e07acfd0; importerHash f9189707f7d51082f653cafea7a6894d98f1476b422ccdf62c51f8aea0d758a3; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/Blue_Normal.png` — C; SHA 4178050008e64ec17dc18100ae074ba6077147e4dab70433e177a992e07acfd0; importerHash d69c4d5f5ec197ab6b51e5fd8fb1689cc8826b48a803f749d996f2e02e889430; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 3

- `Assets/_Project/_Assets/cell.png` — A; SHA 38dd5aa2d9e3728a42981a9cb844156ac2445b8c25811874c1ea3bb4c52aa3e1; importerHash a5b63703210de339ef7cbd0eb5f52b0d1d8da77f0b77a458bb7f096c40972d7e; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 36, y: 36, z: 36, w: 36}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/cell_empty.png` — C; SHA 38dd5aa2d9e3728a42981a9cb844156ac2445b8c25811874c1ea3bb4c52aa3e1; importerHash 622d08864c48455dd841f5c2c262dee71ddb2810386c144eddf9511c0df77d4f; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 36, y: 36, z: 36, w: 36}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 4

- `Assets/_Project/_Assets/Cyan.png` — B; SHA ed84b43c222c6b4effd80742e14760a4fbf7648f525bb0baf61810c3e0ecf85c; importerHash 57b19bb749efaafc715790dd366769f5168ceea1d10c958f2cd22b34c764f1c7; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/Cyan_Normal.png` — C; SHA ed84b43c222c6b4effd80742e14760a4fbf7648f525bb0baf61810c3e0ecf85c; importerHash 937816deec0157512f371a023a6ef218733d6793dfe26493fab7a6c2e8b2ecda; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 5

- `Assets/_Project/_Assets/Green.png` — B; SHA 4e89fbf3834f2e061c5c916a2c7045a21487ff69dbc78a3929d4096fd153a43e; importerHash d559ee38ec5be801d6a8b319ca797053df1d640ba9f562ac6f7a687fab26c4da; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/Green_Normal.png` — C; SHA 4e89fbf3834f2e061c5c916a2c7045a21487ff69dbc78a3929d4096fd153a43e; importerHash e1eff6df7ea2bbc5abfe6a914e432817df77e75c5407f08eddea1a7aa6714c08; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 6

- `Assets/_Project/_Assets/Orange.png` — B; SHA d680741c8ed4f71930af1cd3da9ab5918539e10faa19ed0f7314b63a78a6c29b; importerHash 7771b63d39046e672d1597bd84e2d2cc24c71ebed4c4d09f4b7b45fe4d95b25a; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/Orange_Normal.png` — C; SHA d680741c8ed4f71930af1cd3da9ab5918539e10faa19ed0f7314b63a78a6c29b; importerHash 02d829f39ad3024ea661e9446e932bb6535228d8e987fb26ab1486921557e7d9; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 7

- `Assets/_Project/_Assets/Pink.png` — B; SHA 4ab6bb2ed0510198b69a99a1ac0b1baa39268f745264a453bce24d95f0ed9d3f; importerHash 14a92e6668f4545a146db31c179d564769b8ffa6fb4eedd78a746b212f54e198; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/Pink_Normal.png` — C; SHA 4ab6bb2ed0510198b69a99a1ac0b1baa39268f745264a453bce24d95f0ed9d3f; importerHash d891f3ee1be5b602a4f97aa5913021f76a00f1d2c3853de8ed718ceee3316ea2; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 8

- `Assets/_Project/_Assets/Purple.png` — B; SHA 791f4145611d0ffc222dd55e81568bbb6590ebb64854e41c4b6a0e706d956950; importerHash 2b61022f2d816e7a1fb70b322efaad092b8fadccdb1ab039ae25da782f090df9; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/Purple_Normal.png` — C; SHA 791f4145611d0ffc222dd55e81568bbb6590ebb64854e41c4b6a0e706d956950; importerHash c3914abafd9749074477b271c3c0ddb30afaff569940b47e09e8432b405e246d; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 9

- `Assets/_Project/_Assets/Red.png` — B; SHA b0d061693f7f0bf74416d858934a740c680c85112c03c4315c1ff6f7b7b57b1b; importerHash 72dc0493c31fd44e4732cc1659774dcc9120d9a1b0394dccfbaf4f193c3b6435; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/Red_Normal.png` — C; SHA b0d061693f7f0bf74416d858934a740c680c85112c03c4315c1ff6f7b7b57b1b; importerHash ac3bf84877dfaf9eec779bf8c33ff9f1fb0c31d39801ab39ef46a0b63395ec62; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 10

- `Assets/_Project/_Assets/White.png` — B; SHA 3913ff9cde2d7c30b97d61a8a417f3379c31bba1e8ab2702ae93d63a3aeb967d; importerHash b94905547a6c188324f133989c0a3b14a6ede9a7884c00061c73aa44da2dfb3a; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/White_Normal.png` — C; SHA 3913ff9cde2d7c30b97d61a8a417f3379c31bba1e8ab2702ae93d63a3aeb967d; importerHash c97ca62d221391759fe6fa47ec4a001c0d17b04b6b7e9909676d504f36f7d6f9; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 11

- `Assets/_Project/_Assets/Yellow.png` — B; SHA cfe0d6fe9a1766a35ba7289fb55101343842a56ce8748b70b43ea39c1bc4ab91; importerHash 62d50dc7a358e714857a5b4d1c32b5378e7f3f6a20698e94707d6d5a465e6bdc; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/Yellow_Normal.png` — C; SHA cfe0d6fe9a1766a35ba7289fb55101343842a56ce8748b70b43ea39c1bc4ab91; importerHash 270973a502d00ae7cc8c9ac82090ecc8042d4a3847d3a2ea39a9b731cc97a9b3; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 12

- `Assets/_Project/_Assets/SkyIslandGameplay/Panel.png` — A; SHA c4d949b753637d84f63431b77ae92dfed62f8313822ec06230238a8369990187; importerHash df3a30abc816ff9e2b787ed627196dab6b25c1c0e8b2f8b6883708fce573be8c; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "200", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 64, y: 64, z: 64, w: 64}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/Panel.png` — B; SHA c4d949b753637d84f63431b77ae92dfed62f8313822ec06230238a8369990187; importerHash 7933cb8f865bade95a88f540b72d8dbdf3e89099bf2d0af4b372413efa6db772; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "200", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 64, y: 64, z: 64, w: 64}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 13

- `Assets/_Project/_Assets/SkyIslandGameplay/SquareParticle.png` — A; SHA 36f928a89ac2aad088c3d7a54957710d9289346b9753810676557e78185c2bc9; importerHash 8ee8d6f63bcb3f27a25b85a0773fe5c0cd83bd82a46f4ce73af27c645dc581a0; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 4, y: 4, z: 4, w: 4}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/SquareParticle.png` — B; SHA 36f928a89ac2aad088c3d7a54957710d9289346b9753810676557e78185c2bc9; importerHash caaf437daf55c1694f39b50f7afccac68ff10cc7b096ead1a8fd6fa2941bff2c; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 4, y: 4, z: 4, w: 4}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 14

- `Assets/_Project/_Assets/SkyIslandGameplay/TraySlot.png` — A; SHA d56ca0b9a55cafdb3e78322e142848a07d16eb0ed00ef53cc47170bbe0c65a68; importerHash 2ae90105026e684fff3dfcd6e619e31478e49b8e6c1a7effd1afc0a1344209db; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "200", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 56, y: 56, z: 56, w: 56}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/TraySlot.png` — B; SHA d56ca0b9a55cafdb3e78322e142848a07d16eb0ed00ef53cc47170bbe0c65a68; importerHash 50e92468f08d26faf34e2bfc3758944316bc7920eeac525fc2a4cdd56006b394; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "200", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 56, y: 56, z: 56, w: 56}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 15

- `Assets/_Project/_Assets/UI/AntigravityGen/BackgroundSoft.png` — B; SHA eb3556315b891580d35297c14855f58b2be1f6529dfac86db0c07ff17e0b7e6a; importerHash db6295ab813e53c54df69f9450a80b6955110600509555e6bb5dc1919d2769c5; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/BackgroundSoft.png` — A; SHA eb3556315b891580d35297c14855f58b2be1f6529dfac86db0c07ff17e0b7e6a; importerHash 21e074c01cc34259f64c3352843555e8895f79a07c1448a0b2c968ab8cfc2d7c; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 16

- `Assets/_Project/_Assets/UI/AntigravityGen/Block0Disabled.png` — C; SHA f5fd576b524e8a50c95c66cb35ddfd11febb81ed3b1dc4c9a925d8336ed16e0b; importerHash eaea8093082398e3519c7f345cf4eb8460557bfd41448e7d0481b5de40a0cb5f; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Disabled.png` — C; SHA f5fd576b524e8a50c95c66cb35ddfd11febb81ed3b1dc4c9a925d8336ed16e0b; importerHash e71ab1e32a33ea4c2c84ba3ae28448832a8373273e40a99d2fe4a03b69d619d2; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 17

- `Assets/_Project/_Assets/UI/AntigravityGen/Block0Highlighted.png` — C; SHA b1acbc13da2720dae9ab9c5c601aefbab9dae09a253141eb772ff29ed91cdac8; importerHash ff44ebcb252060b35208ef957fc7b40af35106e1248e2f1deac17af23e7afb63; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Highlighted.png` — C; SHA b1acbc13da2720dae9ab9c5c601aefbab9dae09a253141eb772ff29ed91cdac8; importerHash af1bef9d8fb33cb88f14a3ed13dac50f599d63415e000fbaef7b436fc6cf534f; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 18

- `Assets/_Project/_Assets/UI/AntigravityGen/Block0Normal.png` — C; SHA e7a8ba19ab9ec658e75a6540f6d33832d0a46e41cd6865750294129a6f489e9a; importerHash da40edc3af40a417cdea0640637b508c03ef41006d360763b68a326d7c234a30; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Normal.png` — C; SHA e7a8ba19ab9ec658e75a6540f6d33832d0a46e41cd6865750294129a6f489e9a; importerHash f560f21d9c01c0b7f9828678742f16b494f1c212c27a711db8d24b3f0e12c7c1; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 19

- `Assets/_Project/_Assets/UI/AntigravityGen/Block1Disabled.png` — C; SHA 8ac5bc571f393898ca8cec27c1e98581ae1e1068b40d3484d4993a8d2c12a1e2; importerHash 2c3aef94a861dae7d59867ba52dc8b44456ce5a17204321b939024cfb72bf7c7; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_28_Disabled.png` — C; SHA 8ac5bc571f393898ca8cec27c1e98581ae1e1068b40d3484d4993a8d2c12a1e2; importerHash 7b7c9f1eda7f37f9dfb0f0d4496d3f986ddfc7c4128c1f72d1c95f3d00b9b0e8; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 20

- `Assets/_Project/_Assets/UI/AntigravityGen/Block1Highlighted.png` — C; SHA f9dd60300e28afef56b96e44e14ba6ea7ce08802fee3a2d8bb0432684b9d3000; importerHash 5b1ff5033e37e032783e7a08aedc8f7e6024b812549993822d71a429bcc776b4; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_28_Highlighted.png` — C; SHA f9dd60300e28afef56b96e44e14ba6ea7ce08802fee3a2d8bb0432684b9d3000; importerHash 611e6d896301a4d356d8b75ee94dd5f5f70e133bb02b304b263233523413d49c; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 21

- `Assets/_Project/_Assets/UI/AntigravityGen/Block1Normal.png` — C; SHA 5cb047f1e654bbdb3f77c13c35378cf81c6d5b493d5738082207ba3fd0173508; importerHash c5309376d4eac6be220020c6b7098639816f6ed328c64768b6934242db795909; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_28_Normal.png` — C; SHA 5cb047f1e654bbdb3f77c13c35378cf81c6d5b493d5738082207ba3fd0173508; importerHash 528d19d9f0a86aa2cf6ae21bbaf472e178120655f2d4981965424e43103a2b11; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 22

- `Assets/_Project/_Assets/UI/AntigravityGen/Block2Disabled.png` — C; SHA 78fbd4a1519f70c14686b56a0de009e8926472877dd6dd8aea778e1829514050; importerHash 6954d264cbe14cbc86c2ab9d708b0415b75fa5e17d267db8c51c5c8c10a2714d; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Disabled.png` — C; SHA 78fbd4a1519f70c14686b56a0de009e8926472877dd6dd8aea778e1829514050; importerHash eade7df3fea3f61d8e9dc806bc4f2bb10375d0ae7c68203408c6a952d5b29fad; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 23

- `Assets/_Project/_Assets/UI/AntigravityGen/Block2Highlighted.png` — C; SHA dcaab5ce7035f45bd8b275b206edf990a252c537338bea9cade15371effdc260; importerHash 4d5bbcbc6d06730e592794035d351866916c1cd9fd8e2f005c4163a20292baa0; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Highlighted.png` — C; SHA dcaab5ce7035f45bd8b275b206edf990a252c537338bea9cade15371effdc260; importerHash 66acc3e551a7f66240cf76d873c51ca91451257cd4723716a53a3f01f4a4a9aa; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 24

- `Assets/_Project/_Assets/UI/AntigravityGen/Block2Normal.png` — C; SHA 997fc45be625744686581f9e7044b55ecf1672ca2790a4c178ae91abbf3b0ad4; importerHash 5ae75b0b92a6ba9fc8a060c22fd9ce39eebeeceba06d3c14847b9dcc580dbf87; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Normal.png` — C; SHA 997fc45be625744686581f9e7044b55ecf1672ca2790a4c178ae91abbf3b0ad4; importerHash 877013dff7d5dec6a0ef68bc994f0172497712e4bc4e2cf2232a84d09ce1fc60; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 25

- `Assets/_Project/_Assets/UI/AntigravityGen/Block3Disabled.png` — C; SHA b18f77cb9cc21416c150624b443784fbf1bcbbb35c798a203568a08c8c437f2d; importerHash 59364c3c290090482f0c17457bef1ebd627cfd004089f270a0f94ac88edfe549; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Disabled.png` — C; SHA b18f77cb9cc21416c150624b443784fbf1bcbbb35c798a203568a08c8c437f2d; importerHash 4438e06e743f919785f8bba5fc77513dc399212d789a3c0006a206e0be644d91; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 26

- `Assets/_Project/_Assets/UI/AntigravityGen/Block3Highlighted.png` — C; SHA 038c414af2ff7b5f23210536e267396bcb7cb841f44a93a37f645a621372092f; importerHash 9ced5a386f8a6ef85119df1244cfaad2e3a4d43f654dd989e12fa4cb2931ba33; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Highlighted.png` — C; SHA 038c414af2ff7b5f23210536e267396bcb7cb841f44a93a37f645a621372092f; importerHash 13b44f2cfca2ebabdf51ba8198003a234ca2daed509624c98cc9c2896ac050c2; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 27

- `Assets/_Project/_Assets/UI/AntigravityGen/Block3Normal.png` — C; SHA ccc3b6e2fe174f01090416d35e6aadced994a8374d9dd8d2a60570a3c0689906; importerHash 57db0a089f2f4139340ee4e2acc46e9a135fa2d79efdb975fd8bcc9f4683ef63; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Normal.png` — C; SHA ccc3b6e2fe174f01090416d35e6aadced994a8374d9dd8d2a60570a3c0689906; importerHash 1b2cef3c377057a20191465fffdbc925911465b78cc3d007825584726912c7e3; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 28

- `Assets/_Project/_Assets/UI/AntigravityGen/Block4Disabled.png` — C; SHA b81f3118f040e6272c694dd9ad52cb79f380499eb8944f4fe3c7baa4277b4e19; importerHash 26873a3280fc60ea8c55a23e4285e8df3e4859587f18898e94b8658791345571; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Disabled.png` — C; SHA b81f3118f040e6272c694dd9ad52cb79f380499eb8944f4fe3c7baa4277b4e19; importerHash cfb01aacd5a66b73031581f41e2b5099e40a5e6b0d5ccc7b7f5dbb9f4b0f07bc; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 29

- `Assets/_Project/_Assets/UI/AntigravityGen/Block4Highlighted.png` — C; SHA 7bbfd98b007848f1b5caa0d7722daf5a9025e89a5118f7f40e2d0fd68db96ce7; importerHash 86570a601428f0dc45317377419d0ccf75e418a1b69fd0dd087e8379918fd172; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Highlighted.png` — C; SHA 7bbfd98b007848f1b5caa0d7722daf5a9025e89a5118f7f40e2d0fd68db96ce7; importerHash 3b01c8af34966021290a2ec730b9d4ac0aa4dbdf32b78bd8a9adffa4893a27be; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 30

- `Assets/_Project/_Assets/UI/AntigravityGen/Block4Normal.png` — C; SHA 1be8b4cbedc1b27fafaacfe3a95d3b3cbb5a54ab7500ba400002e217070bb811; importerHash 0903f2f3827fa60a02373f5f2d523d4aee875c4dab19b246e074f523112b43ee; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Normal.png` — C; SHA 1be8b4cbedc1b27fafaacfe3a95d3b3cbb5a54ab7500ba400002e217070bb811; importerHash 8deb50eb9481bab0f9b408130f13e8cf9930e90c69e662f0ad3c596d31ed2562; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 31

- `Assets/_Project/_Assets/UI/AntigravityGen/Block5Disabled.png` — C; SHA 181f1765ec000dec6f89e5e3b628145b643bcbee083f524544ee82f6a9cde707; importerHash fd5ef8e594b053ef4f6c1811a22d4b67256c267ebe76c39a8a060189f2f01ee2; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Disabled.png` — C; SHA 181f1765ec000dec6f89e5e3b628145b643bcbee083f524544ee82f6a9cde707; importerHash 9e3a1718124eaae128736418605890a9057358b3186be136de23d5cb8a8c815c; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 32

- `Assets/_Project/_Assets/UI/AntigravityGen/Block5Highlighted.png` — C; SHA 232051d08735167038d2d24b395c7ac33bb0451b458f22985946c27ba5a7cca8; importerHash 19f51a4bc8dbf884054c8012c4409935e7abf608b59e022d35f844758f6c4dda; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Highlighted.png` — C; SHA 232051d08735167038d2d24b395c7ac33bb0451b458f22985946c27ba5a7cca8; importerHash 1840ee88e469a32dde88310d92298f42807ba57c31dc470b3795f6c61d18b8cf; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 33

- `Assets/_Project/_Assets/UI/AntigravityGen/Block5Normal.png` — C; SHA 4b3446e50e13b4a7823e23d0b7caaf1ef608ee2fbb824e44efd2cc803f1dac5a; importerHash dbd86bf47d59d0fd58d65f12a1c6ab40e00b2183624dc44a7e7b409cb0af0e95; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Normal.png` — C; SHA 4b3446e50e13b4a7823e23d0b7caaf1ef608ee2fbb824e44efd2cc803f1dac5a; importerHash 41a7a2be363a9cb4fb917aebd536143cde449a93388b3d75d9ce860b661b6757; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 34

- `Assets/_Project/_Assets/UI/AntigravityGen/Block6Disabled.png` — C; SHA 5db44dd04a85e17e4a960a68ea03c3efc50a4b33047841865fc3851fb77b1131; importerHash c83e3e00d2ec252c7d0b2cf4ffbe6a00c6df4c351543583e253711a708636945; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/Block7Disabled.png` — C; SHA 5db44dd04a85e17e4a960a68ea03c3efc50a4b33047841865fc3851fb77b1131; importerHash d5d9f17b8040d272c94c67423e4a70308225ffbd2d9f70ba4bc2de89e60bee60; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 35

- `Assets/_Project/_Assets/UI/AntigravityGen/Block6Highlighted.png` — C; SHA 72f2b2e870a85a2a2e130f174a87d1f75e84df2b319ea3103b25e76e1ebfc5b7; importerHash 85576f1a477b08b4025613ec7c7159bdd6da74da5d0a6b8394c687e6430308b9; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/AntigravityGen/Block7Highlighted.png` — C; SHA 72f2b2e870a85a2a2e130f174a87d1f75e84df2b319ea3103b25e76e1ebfc5b7; importerHash 0f73e98eaad1fb9a2fa0f3b470420104c26001ec00e1b416635f93703742b156; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 36

- `Assets/_Project/_Assets/UI/AntigravityGen/CellBeveled.png` — B; SHA 5ba05afc369e4b18bb6110ee9ae47790294931dac58b9a552dcd02424d1150fa; importerHash f95250fb4b6f0f9f6398f3b348ba9d8dd79f23e5fd4c84ebd51a12fe686bad9d; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 36, y: 36, z: 36, w: 36}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/CellBeveled.png` — A; SHA 5ba05afc369e4b18bb6110ee9ae47790294931dac58b9a552dcd02424d1150fa; importerHash bfde6603e040ef766afcc2776c906712a41c3034bd56884f39b3a148415d9add; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "150", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 36, y: 36, z: 36, w: 36}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 37

- `Assets/_Project/_Assets/UI/AntigravityGen/StoneFrame.png` — B; SHA e23251c1acc980a18b9c18750a2ab1285f179be9dcfb7102cff7b6eedda38eb0; importerHash b9805492c376b755fb16c5a13638a865d55617aa3d73ef544576457db11430ea; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "400", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 112, y: 112, z: 112, w: 112}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/StoneFrame.png` — A; SHA e23251c1acc980a18b9c18750a2ab1285f179be9dcfb7102cff7b6eedda38eb0; importerHash a82cdbf7b742f2e64e5e3b39e1418e2189f55e7cd1c73427cd05ffc61d7d7d58; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "400", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 112, y: 112, z: 112, w: 112}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 38

- `Assets/_Project/_Assets/UI/Generated/Block0Disabled.png` — A; SHA 80dac19f30d3bffd64c2e05a47aac1ee73d71b1c4c173c8e0912c19940c94d0f; importerHash 6f8d27803f3bd609015ffc77f03e0d41ddfb10babf5222a709a5bb4b34709d21; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_27_Disabled.png` — A; SHA 80dac19f30d3bffd64c2e05a47aac1ee73d71b1c4c173c8e0912c19940c94d0f; importerHash bb758a331ff8a5029289bf56e7875d0e94a4bd5528c4fad49618811e99e65cb9; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 39

- `Assets/_Project/_Assets/UI/Generated/Block0Highlighted.png` — A; SHA 3fd6a47daaa13ebcc914b949bfc56259693c139d85f16eef764ba4ae6cb5a80c; importerHash b828c398fb89f83f4a7238d7bba6555197bb385710837bc03c0b13dcf7100993; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_27_Highlighted.png` — A; SHA 3fd6a47daaa13ebcc914b949bfc56259693c139d85f16eef764ba4ae6cb5a80c; importerHash 14b7f2b22a78b8ee62d30f361905274f20b5a221af590081e51b05d7d5a63614; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 40

- `Assets/_Project/_Assets/UI/Generated/Block0Normal.png` — A; SHA f4f3c7ee5ffd6b44f0734278202d7c6ad299ad43e36e0f5e8925bc5e976269cd; importerHash c5cf2cac72255e1e56c5b693cd1a31bee84a578296e8e25e24057946714ce1f6; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_27_Normal.png` — A; SHA f4f3c7ee5ffd6b44f0734278202d7c6ad299ad43e36e0f5e8925bc5e976269cd; importerHash 03b0fa2d48063d987dd561a81e95e7e627a5e5b545e3913743a507c0c45756c0; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 41

- `Assets/_Project/_Assets/UI/Generated/Block1Disabled.png` — A; SHA 657094ba0fd14bbab4fe862eb1b6dfc447bccac313f2b288e3d5e09001c4311e; importerHash f5c24a8cf4b19816a116a5004e5c17b42b75d4661e5d85d1ce47f75716a8d941; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_28_Disabled.png` — A; SHA 657094ba0fd14bbab4fe862eb1b6dfc447bccac313f2b288e3d5e09001c4311e; importerHash 05ef30e4ab5899bba03d4ed2c9b9b9ec63c6163a3495e96ac7bbb088182a26e6; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 42

- `Assets/_Project/_Assets/UI/Generated/Block1Highlighted.png` — A; SHA 78a0fcd48bc3452115e773ebe7400f8a36c83c81a9d00a3763a37ea37448db0d; importerHash 117f9aabac981840e69285026f88753ed4ad97c86a0f277447815996a2da8607; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_28_Highlighted.png` — A; SHA 78a0fcd48bc3452115e773ebe7400f8a36c83c81a9d00a3763a37ea37448db0d; importerHash 3b1524e01b20ee0fa7789a3a259310f635cd09abdf4e08513bc270dfc78f8bc1; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 43

- `Assets/_Project/_Assets/UI/Generated/Block1Normal.png` — A; SHA b594668609d67d53e003abd8451f14dea34ee3fa44721c25fe2e5bf8e247c424; importerHash 00e056dae3728c1709308fdf7e371dcf5f71f736c277527ad992d8c5ab3b998c; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_28_Normal.png` — A; SHA b594668609d67d53e003abd8451f14dea34ee3fa44721c25fe2e5bf8e247c424; importerHash a86e933a674d856def9ec3bd23a9c0acfbd9d0e2f4380f4bd1bd7788ea670919; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 44

- `Assets/_Project/_Assets/UI/Generated/Block2Disabled.png` — A; SHA 39ea3e8223e7897905b77ca1fb319d295f24e6af04513649429d0fb978629383; importerHash 5fa7b2eaf2d6c531f04f24e86224e90cc6b81e4e349e2abb8d2d34ffccaafae5; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_29_Disabled.png` — A; SHA 39ea3e8223e7897905b77ca1fb319d295f24e6af04513649429d0fb978629383; importerHash 97604b0ddaa0dbfb4e3d5be3db4e72a31d76f0f094cf95aca2b4164ed52e01fb; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 45

- `Assets/_Project/_Assets/UI/Generated/Block2Highlighted.png` — A; SHA 5aadd89c57bd4c805e8d196a26a9070ec25ed04dedd3a5280489c2adbe851ab2; importerHash d0d711a51fefd7330f239c09eeba5ea90be1a6b64ff1fb983186c7686706091a; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_29_Highlighted.png` — A; SHA 5aadd89c57bd4c805e8d196a26a9070ec25ed04dedd3a5280489c2adbe851ab2; importerHash f6ef9848f4b5df85d00d9eb3d85e1836be4d3d860d04c4a4180c731df140ab7e; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 46

- `Assets/_Project/_Assets/UI/Generated/Block2Normal.png` — A; SHA 421c7b2e5ad1e72d7b8c9ca3312594082c83e69313c8856c24852c0370e63c65; importerHash 490da708eaf59cab46c7562f4bbfe9258f3d1a22fff821fa2ccc723d77b759d4; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_29_Normal.png` — A; SHA 421c7b2e5ad1e72d7b8c9ca3312594082c83e69313c8856c24852c0370e63c65; importerHash f467f03c90370c322c8761efb01280f73b90912154fd66b99eb17aea3f7fa453; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 47

- `Assets/_Project/_Assets/UI/Generated/Block3Disabled.png` — A; SHA 8e7b488d9dcfc23981d728a945c8c8af9288a70a527c70efc89839660cd66e97; importerHash 3644f56d71b73c3c8f7f632fc0d83c353c385ee1564d52e03bdc32dd78f26c3c; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_30_Disabled.png` — A; SHA 8e7b488d9dcfc23981d728a945c8c8af9288a70a527c70efc89839660cd66e97; importerHash 7596898fbf5d0103c909167e44212d9c7790a1d2f38e446f2388555669dd9a01; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 48

- `Assets/_Project/_Assets/UI/Generated/Block3Highlighted.png` — A; SHA fb03eaa040d4d548f7eb324cf47e7c8c69a596abae7a3dad0245f1ff53803ef0; importerHash f952b70724ae173bb84c0dbacb1e2e3d4e84635708d58b4dd9e1654d2572b4fa; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_30_Highlighted.png` — A; SHA fb03eaa040d4d548f7eb324cf47e7c8c69a596abae7a3dad0245f1ff53803ef0; importerHash 4667c1477fb3c2a295036c27d2977f76b585a1785e57c8c6e7e7d92039af59c7; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 49

- `Assets/_Project/_Assets/UI/Generated/Block3Normal.png` — A; SHA 88a6d2f0d561f6c8a72da62cb4eea57327d3f778e164cfed69847f587059fec4; importerHash a050970382cdf8546dcbb44beea97c810558160a3b812054eee539696ce9088b; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_30_Normal.png` — A; SHA 88a6d2f0d561f6c8a72da62cb4eea57327d3f778e164cfed69847f587059fec4; importerHash ace981d304b07672050abcd08f19aa3f22c0f79c27314a3d49362d150bbb1165; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 50

- `Assets/_Project/_Assets/UI/Generated/Block4Disabled.png` — A; SHA 5a6bc4394dbac815910fe4d18ffe0e3fef87ca629e8d2233b63d9acd46e2d65d; importerHash 78b5a930e20080010e8b15b6c3109c50fefbf8f07f05e1a1315c0774adcf2448; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_31_Disabled.png` — A; SHA 5a6bc4394dbac815910fe4d18ffe0e3fef87ca629e8d2233b63d9acd46e2d65d; importerHash fdca5e77eb86e04490ab882ae2f21ccbac0faaf50d4e5cdbb61025367933476d; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 51

- `Assets/_Project/_Assets/UI/Generated/Block4Highlighted.png` — A; SHA c927b7d9baa9cbfa7e1a3af5cbd620b57c912ac6ce968c1d0a0bc55a726f5dd2; importerHash 1ffe3ad286cd0e4f2af3f788b5fe4c195b9e633b2e0b8feb560fb265e7697457; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_31_Highlighted.png` — A; SHA c927b7d9baa9cbfa7e1a3af5cbd620b57c912ac6ce968c1d0a0bc55a726f5dd2; importerHash 8314c0c3299ca274376a497736bb669f66b4e7b5b969cf48510e9124c1749751; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 52

- `Assets/_Project/_Assets/UI/Generated/Block4Normal.png` — A; SHA c6bafad199140f526e3a35a4fd4411aa97b886b8fdaeb9df3cb861895ca479cd; importerHash 4a5f70347dbf00414fac00df442ba04100318fcec38b7b08bad9e631bec05e5f; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_31_Normal.png` — A; SHA c6bafad199140f526e3a35a4fd4411aa97b886b8fdaeb9df3cb861895ca479cd; importerHash 70ad822954f4f7e9050c213f2672e8a64f9915d8027c2d2d32fb1e1351841bc3; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 53

- `Assets/_Project/_Assets/UI/Generated/Block5Disabled.png` — A; SHA 2e1df970be90b45399b112ec53cf16bcdddf60efcd45fb5efb2b11c862134cdb; importerHash bc07e6456c118f2a898921fc39f3c74a9e4dba61806b73eeeea87f8dbe4ab872; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_32_Disabled.png` — A; SHA 2e1df970be90b45399b112ec53cf16bcdddf60efcd45fb5efb2b11c862134cdb; importerHash 60732b363d33be30f1b1c93db6d3bb7d141cf459fe6ae6aa7573468165abde7a; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 54

- `Assets/_Project/_Assets/UI/Generated/Block5Highlighted.png` — A; SHA dd7494558485e55434dcb392f8ff929ed0e872ad17b6cdeec8005204eae32435; importerHash 896e1ff3403e571ad97bd0a812802d4d24f3b9b9f844cfafcac7821a78e4699d; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_32_Highlighted.png` — A; SHA dd7494558485e55434dcb392f8ff929ed0e872ad17b6cdeec8005204eae32435; importerHash b38637aae62edd54a24956d8cb7b1553849ecf85ad5d37beb1e1b67ddcc35073; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 55

- `Assets/_Project/_Assets/UI/Generated/Block5Normal.png` — A; SHA 2f42a9558e37f560a9b29cccb903cf8e609d0aaa11ba19c3620a349a6e12095f; importerHash 5310e0e5ab2430b9bee2fce8487766f1312d81ff25dfaa9f2bd740636b0e4b42; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "0", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`
- `Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_32_Normal.png` — A; SHA 2f42a9558e37f560a9b29cccb903cf8e609d0aaa11ba19c3620a349a6e12095f; importerHash 1425ea2e869d20d204a9dd9c30e553bc9805fd6060e46733ba67490c38291a08; import `{"textureType": "8", "spriteMode": "1", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "0", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "1", "wrapV": "1", "alphaIsTransparency": "1"}`

Importer đầy đủ bỏ GUID: KHÁC (kể cả slicing/ID/name); không thay thế trực tiếp. Cần remap GUID/fileID và test.

### Nhóm 56

- `Assets/_Project/_Assets/sprite/spriteBlocks/Bảng ý tưởng giao diện game xếp khối 3D 1.png` — C; SHA be926e2f9d689ee2a0d89433fc42227874e31d7d00bd332cd3b2d8b36ccb5fc7; importerHash c2eae230b814d3ec2541b19c1cb9f00bad9c06f80baf07775a21ea382d85689f; import `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- `Assets/_Project/_Assets/sprite/spriteBlocks/Bảng ý tưởng giao diện game xếp khối 3D.png` — C; SHA be926e2f9d689ee2a0d89433fc42227874e31d7d00bd332cd3b2d8b36ccb5fc7; importerHash c2eae230b814d3ec2541b19c1cb9f00bad9c06f80baf07775a21ea382d85689f; import `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`

Importer đầy đủ bỏ GUID: giống. Cần remap GUID/fileID và test.

### Nhóm 57

- `Assets/_Project/_Assets/SkyIslandSetting/01_SettingsPanel/settings_panel_empty.png` — C; SHA bc1caa7293c78e90caeee8d5be937cde9bdbf494dca46e8cd471137ef258c85b; importerHash c2eae230b814d3ec2541b19c1cb9f00bad9c06f80baf07775a21ea382d85689f; import `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`
- `Assets/_Project/_Assets/SkyIslandSetting/05_SpriteSheets/panel_transparent.png` — C; SHA bc1caa7293c78e90caeee8d5be937cde9bdbf494dca46e8cd471137ef258c85b; importerHash c2eae230b814d3ec2541b19c1cb9f00bad9c06f80baf07775a21ea382d85689f; import `{"textureType": "0", "spriteMode": "0", "spritePixelsToUnits": "100", "spritePivot": "{x: 0.5, y: 0.5}", "spriteBorder": "{x: 0, y: 0, z: 0, w: 0}", "maxTextureSize": "2048", "isReadable": "0", "enableMipMap": "1", "textureCompression": "1", "sRGBTexture": "1", "filterMode": "1", "wrapU": "0", "wrapV": "0", "alphaIsTransparency": "0"}`

Importer đầy đủ bỏ GUID: giống. Cần remap GUID/fileID và test.

## Pixel giống, byte khác


## Gần giống dHash ≤4, aspect lệch ≤3%

Chưa review thị giác; ảnh phẳng/frame khác màu có thể false positive; không kết luận trùng.
- 0: Assets/_Project/Materials/Textures/BlueBlock1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/BlueBlock2.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/BlueBlock1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/GreenCube1.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/BlueBlock1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/GreenCube2.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/BlueBlock1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/MovingBlock.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/BlueBlock2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/GreenCube1.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/BlueBlock2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/GreenCube2.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/BlueBlock2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/MovingBlock.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/GreenCube1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/GreenCube2.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/GreenCube1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/MovingBlock.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/GreenCube2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/MovingBlock.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/OctagonWork2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork3.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/OctagonWork2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork6.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/OctagonWork3.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork6.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/OctagonWork4.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork5.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/WorkBlock2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/WorkBlock3.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/WorkBlock2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/WorkBlock4.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/WorkBlock2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/WorkBlock5.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/WorkBlock2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/WorkBlock6.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/WorkBlock3.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/WorkBlock4.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/WorkBlock3.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/WorkBlock5.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/WorkBlock3.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/WorkBlock6.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/WorkBlock4.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/WorkBlock5.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/WorkBlock4.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/WorkBlock6.png (1024, 1024)
- 0: Assets/_Project/Materials/Textures/WorkBlock5.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/WorkBlock6.png (1024, 1024)
- 0: Assets/_Project/_Assets/Black.png (384, 384) ↔ Assets/_Project/_Assets/Black_Disabled.png (384, 384)
- 0: Assets/_Project/_Assets/Black_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Black_Normal.png (384, 384)
- 0: Assets/_Project/_Assets/Cyan.png (384, 384) ↔ Assets/_Project/_Assets/Green.png (384, 384)
- 0: Assets/_Project/_Assets/Cyan.png (384, 384) ↔ Assets/_Project/_Assets/Green_Normal.png (384, 384)
- 0: Assets/_Project/_Assets/Cyan_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Green_Disabled.png (384, 384)
- 0: Assets/_Project/_Assets/Cyan_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Green.png (384, 384)
- 0: Assets/_Project/_Assets/Cyan_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Green_Normal.png (384, 384)
- 0: Assets/_Project/_Assets/Orange_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Pink_Highlighted.png (384, 384)
- 0: Assets/_Project/_Assets/UI/AntigravityGen/Block3Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block4Normal.png (384, 384)
- 0: Assets/_Project/_Assets/UI/AntigravityGen/Block3Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Normal.png (384, 384)
- 0: Assets/_Project/_Assets/UI/AntigravityGen/Block4Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Normal.png (384, 384)
- 0: Assets/_Project/_Assets/UI/AntigravityGen/BlockWhite.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/BlockWhite.png (384, 384)
- 0: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/Black.png (384, 384) ↔ Assets/_Project/_Assets/Black_Highlighted.png (384, 384)
- 1: Assets/_Project/_Assets/Black_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Black_Highlighted.png (384, 384)
- 1: Assets/_Project/_Assets/Black_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Black_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/Cyan.png (384, 384) ↔ Assets/_Project/_Assets/Orange.png (384, 384)
- 1: Assets/_Project/_Assets/Cyan.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/Cyan_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Disabled.png (384, 384)
- 1: Assets/_Project/_Assets/Cyan_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Yellow.png (384, 384)
- 1: Assets/_Project/_Assets/Cyan_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/Cyan_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Orange.png (384, 384)
- 1: Assets/_Project/_Assets/Cyan_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/Green.png (384, 384) ↔ Assets/_Project/_Assets/Orange.png (384, 384)
- 1: Assets/_Project/_Assets/Green.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/Green_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Disabled.png (384, 384)
- 1: Assets/_Project/_Assets/Green_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Highlighted.png (384, 384)
- 1: Assets/_Project/_Assets/Green_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Orange.png (384, 384)
- 1: Assets/_Project/_Assets/Green_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/Orange.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Highlighted.png (384, 384)
- 1: Assets/_Project/_Assets/Orange_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Red.png (384, 384)
- 1: Assets/_Project/_Assets/Orange_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Red_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/Orange_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Highlighted.png (384, 384)
- 1: Assets/_Project/_Assets/SkyIslandSetting/01_SettingsPanel/settings_panel_empty.png (1086, 1448) ↔ Assets/_Project/_Assets/SkyIslandSetting/01_SettingsPanel/settings_panel_settings.png (1086, 1448)
- 1: Assets/_Project/_Assets/SkyIslandSetting/01_SettingsPanel/settings_panel_settings.png (1086, 1448) ↔ Assets/_Project/_Assets/SkyIslandSetting/05_SpriteSheets/panel_transparent.png (1086, 1448)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block0Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block6Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block0Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block7Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block1Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block3Highlighted.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block1Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Highlighted.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block2Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block3Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block2Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block4Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block2Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block5Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block2Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block2Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block2Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block3Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_28_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block3Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block6Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block3Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block7Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block3Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block4Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block6Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block4Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block7Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block4Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block5Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block6Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block5Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block6Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block6Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block6Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block6Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block7Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block7Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/Block7Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/CrownIcon.png (256, 256) ↔ Assets/_Project/_Assets/UI/Generated/CrownIcon.png (256, 256)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_28_Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Highlighted.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/Generated/Block1Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/Block1Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/Generated/Block1Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_28_Normal.png (384, 384)
- 1: Assets/_Project/_Assets/UI/Generated/Block1Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_28_Highlighted.png (384, 384)
- 1: Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_28_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_28_Normal.png (384, 384)
- 2: Assets/_Project/Materials/Textures/OctagonWork1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork2.png (1024, 1024)
- 2: Assets/_Project/Materials/Textures/OctagonWork1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork3.png (1024, 1024)
- 2: Assets/_Project/Materials/Textures/OctagonWork1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork6.png (1024, 1024)
- 2: Assets/_Project/Materials/Textures/WorkBlock1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/WorkBlock2.png (1024, 1024)
- 2: Assets/_Project/Materials/Textures/WorkBlock1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/WorkBlock3.png (1024, 1024)
- 2: Assets/_Project/Materials/Textures/WorkBlock1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/WorkBlock4.png (1024, 1024)
- 2: Assets/_Project/Materials/Textures/WorkBlock1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/WorkBlock5.png (1024, 1024)
- 2: Assets/_Project/Materials/Textures/WorkBlock1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/WorkBlock6.png (1024, 1024)
- 2: Assets/_Project/_Assets/Black_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Red_Disabled.png (384, 384)
- 2: Assets/_Project/_Assets/Blue.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Disabled.png (384, 384)
- 2: Assets/_Project/_Assets/Blue.png (384, 384) ↔ Assets/_Project/_Assets/Purple.png (384, 384)
- 2: Assets/_Project/_Assets/Blue.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/Blue_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Disabled.png (384, 384)
- 2: Assets/_Project/_Assets/Blue_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Purple.png (384, 384)
- 2: Assets/_Project/_Assets/Blue_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/Cyan.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/Cyan_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Red.png (384, 384)
- 2: Assets/_Project/_Assets/Cyan_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Red_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/Cyan_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Green_Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/Cyan_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/Green.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/Green_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Red.png (384, 384)
- 2: Assets/_Project/_Assets/Green_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Red_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/Green_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Orange.png (384, 384)
- 2: Assets/_Project/_Assets/Green_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/Green_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/Orange_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/Orange_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Red_Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/Pink_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/Pink_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Red_Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block0Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block5Disabled.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block0Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Disabled.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block0Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block5Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block0Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block0Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block3Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block0Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block4Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block0Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block5Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block0Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block0Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block0Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block2Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block6Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block2Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block7Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block3Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block4Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block3Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block3Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block5Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block3Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block3Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block4Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block4Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block5Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block4Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block4Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block5Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Disabled.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block5Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block5Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block5Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block5Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block6Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block7Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block6Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/Block7Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Disabled.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Highlighted.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Normal.png (384, 384)
- 2: Assets/_Project/_Assets/UI/ReferenceArt/BlockPinkDisabled.png (75, 78) ↔ Assets/_Project/_Assets/UI/ReferenceArt/BlockPurpleDisabled.png (75, 78)
- 3: Assets/_Project/Materials/Textures/BrickBlock1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/BrickBlock2.png (1024, 1024)
- 3: Assets/_Project/_Assets/Black.png (384, 384) ↔ Assets/_Project/_Assets/Red_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/Black_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Red_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/Black_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Red_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/Blue.png (384, 384) ↔ Assets/_Project/_Assets/Cyan_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/Blue.png (384, 384) ↔ Assets/_Project/_Assets/Green_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/Blue.png (384, 384) ↔ Assets/_Project/_Assets/Red.png (384, 384)
- 3: Assets/_Project/_Assets/Blue.png (384, 384) ↔ Assets/_Project/_Assets/Red_Normal.png (384, 384)
- 3: Assets/_Project/_Assets/Blue_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/Blue_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Red_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/Blue_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Blue_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Pink_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Blue_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Blue_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Cyan_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/Blue_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Green_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/Blue_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Red.png (384, 384)
- 3: Assets/_Project/_Assets/Blue_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Red_Normal.png (384, 384)
- 3: Assets/_Project/_Assets/Cyan.png (384, 384) ↔ Assets/_Project/_Assets/Green_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Cyan_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Cyan_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/White_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/Cyan_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Cyan_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Green_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Green.png (384, 384) ↔ Assets/_Project/_Assets/Green_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Green_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Green_Normal.png (384, 384)
- 3: Assets/_Project/_Assets/Green_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Green_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Pink_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Green_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Yellow.png (384, 384)
- 3: Assets/_Project/_Assets/Green_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/Green_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Green_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Normal.png (384, 384)
- 3: Assets/_Project/_Assets/Orange.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Orange.png (384, 384) ↔ Assets/_Project/_Assets/Pink_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Orange.png (384, 384) ↔ Assets/_Project/_Assets/Red_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Orange.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/Orange_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Normal.png (384, 384)
- 3: Assets/_Project/_Assets/Orange_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Pink_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Orange_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Red_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/Orange_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/Pink_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/Purple.png (384, 384) ↔ Assets/_Project/_Assets/Red.png (384, 384)
- 3: Assets/_Project/_Assets/Purple.png (384, 384) ↔ Assets/_Project/_Assets/Red_Normal.png (384, 384)
- 3: Assets/_Project/_Assets/Purple_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/White.png (384, 384)
- 3: Assets/_Project/_Assets/Purple_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/White_Normal.png (384, 384)
- 3: Assets/_Project/_Assets/Purple_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Red.png (384, 384)
- 3: Assets/_Project/_Assets/Purple_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Red_Normal.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block0Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block3Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block0Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block0Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block2Normal.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block0Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Normal.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block1Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block4Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block1Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block2Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block3Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block2Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block2Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Normal.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block3Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block4Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block3Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block5Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block3Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block3Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block3Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block3Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block4Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block4Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_28_Normal.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block5Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block5Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block7Normal.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block6Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/BlockWhite.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block6Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/BlockWhite.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block7Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/BlockWhite.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block7Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/BlockWhite.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/Block7Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Normal.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Normal.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_28_Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Highlighted.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_30_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Disabled.png (384, 384)
- 3: Assets/_Project/_Assets/UI/Generated/Block2Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/Block7Normal.png (384, 384)
- 3: Assets/_Project/_Assets/UI/Generated/Block7Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_29_Normal.png (384, 384)
- 3: Assets/_Project/_Assets/UI/ReferenceArt/BlockBlue.png (75, 77) ↔ Assets/_Project/_Assets/UI/ReferenceArt/BlockBlueHighlighted.png (75, 77)
- 3: Assets/_Project/_Assets/UI/ReferenceArt/BlockCyanDisabled.png (75, 78) ↔ Assets/_Project/_Assets/UI/ReferenceArt/BlockWhiteDisabled.png (75, 78)
- 4: Assets/_Project/Materials/Textures/BlueBlock1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork4.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/BlueBlock1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork5.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/BlueBlock2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork4.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/BlueBlock2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork5.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/GreenCube1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork4.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/GreenCube1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork5.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/GreenCube2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork4.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/GreenCube2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork5.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/MovingBlock.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork4.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/MovingBlock.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork5.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/OctagonBrick1.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonBrick2.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/OctagonWork2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork4.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/OctagonWork2.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork5.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/OctagonWork3.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork4.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/OctagonWork3.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork5.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/OctagonWork4.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork6.png (1024, 1024)
- 4: Assets/_Project/Materials/Textures/OctagonWork5.png (1024, 1024) ↔ Assets/_Project/Materials/Textures/OctagonWork6.png (1024, 1024)
- 4: Assets/_Project/_Assets/Blue.png (384, 384) ↔ Assets/_Project/_Assets/Pink.png (384, 384)
- 4: Assets/_Project/_Assets/Blue.png (384, 384) ↔ Assets/_Project/_Assets/Pink_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/Blue_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Pink_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/Blue_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Green_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Blue_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Orange.png (384, 384)
- 4: Assets/_Project/_Assets/Blue_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/Blue_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Pink.png (384, 384)
- 4: Assets/_Project/_Assets/Blue_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Pink_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/Cyan.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Cyan.png (384, 384) ↔ Assets/_Project/_Assets/Pink_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Cyan.png (384, 384) ↔ Assets/_Project/_Assets/Red_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Cyan.png (384, 384) ↔ Assets/_Project/_Assets/Yellow.png (384, 384)
- 4: Assets/_Project/_Assets/Cyan.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/Cyan.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/Cyan_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Orange.png (384, 384)
- 4: Assets/_Project/_Assets/Cyan_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/Cyan_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Cyan_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Pink_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Cyan_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Red_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Cyan_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Yellow.png (384, 384)
- 4: Assets/_Project/_Assets/Cyan_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/Cyan_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/Green.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Green.png (384, 384) ↔ Assets/_Project/_Assets/Pink_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Green.png (384, 384) ↔ Assets/_Project/_Assets/Red_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Green.png (384, 384) ↔ Assets/_Project/_Assets/Yellow.png (384, 384)
- 4: Assets/_Project/_Assets/Green.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/Green.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/Green_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/White.png (384, 384)
- 4: Assets/_Project/_Assets/Green_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/White_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/Green_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Orange_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Green_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Pink_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Green_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Red_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Green_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Yellow.png (384, 384)
- 4: Assets/_Project/_Assets/Green_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/Green_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/Orange.png (384, 384) ↔ Assets/_Project/_Assets/White.png (384, 384)
- 4: Assets/_Project/_Assets/Orange.png (384, 384) ↔ Assets/_Project/_Assets/White_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/Orange_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Pink_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/Orange_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Purple.png (384, 384)
- 4: Assets/_Project/_Assets/Orange_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/Orange_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Orange_Normal.png (384, 384) ↔ Assets/_Project/_Assets/White.png (384, 384)
- 4: Assets/_Project/_Assets/Orange_Normal.png (384, 384) ↔ Assets/_Project/_Assets/White_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/Pink.png (384, 384) ↔ Assets/_Project/_Assets/Purple.png (384, 384)
- 4: Assets/_Project/_Assets/Pink.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/Pink_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Pink_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Purple.png (384, 384)
- 4: Assets/_Project/_Assets/Pink_Normal.png (384, 384) ↔ Assets/_Project/_Assets/Purple_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/Purple_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Red_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Purple_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Yellow.png (384, 384)
- 4: Assets/_Project/_Assets/Purple_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/Purple_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Purple_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/Red_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/Block0Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block4Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/Block0Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/Block1Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/BlockWhite.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/Block1Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/BlockWhite.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/Block2Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block4Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/Block2Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/Block4Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block5Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/Block4Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/Block4Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/Block4Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/Block4Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block6Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/Block4Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Block7Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/Block5Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/Block6Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/Block7Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/BlockWhite.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_28_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_27_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_28_Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/BlockWhite.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_29_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_31_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/UI_game_nen_trong_suot_32_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/Generated/Block1Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/Block3Normal.png (384, 384)
- 4: Assets/_Project/_Assets/UI/Generated/Block1Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_30_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/UI/Generated/Block2Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/Block7Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/Generated/Block3Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/Block4Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/Generated/Block3Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_31_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/Generated/Block3Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_28_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/Generated/Block4Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/Block7Normal.png (384, 384)
- 4: Assets/_Project/_Assets/UI/Generated/Block4Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_30_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/Generated/Block7Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_29_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/Generated/Block7Normal.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_31_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_28_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_30_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_30_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/UI/Generated/UI_game_nen_trong_suot_31_Disabled.png (384, 384)
- 4: Assets/_Project/_Assets/UI/ReferenceArt/BlockBlueHighlighted.png (75, 77) ↔ Assets/_Project/_Assets/UI/ReferenceArt/BlockYellowDisabled.png (75, 77)
- 4: Assets/_Project/_Assets/White_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Yellow.png (384, 384)
- 4: Assets/_Project/_Assets/White_Disabled.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Normal.png (384, 384)
- 4: Assets/_Project/_Assets/White_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/SkyIslandGameplay/Panel.png (512, 512)
- 4: Assets/_Project/_Assets/White_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/UI/AntigravityGen/Panel.png (512, 512)
- 4: Assets/_Project/_Assets/Yellow.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Highlighted.png (384, 384)
- 4: Assets/_Project/_Assets/Yellow_Highlighted.png (384, 384) ↔ Assets/_Project/_Assets/Yellow_Normal.png (384, 384)

Trần giảm byte nguồn nếu hợp nhất tất cả byte duplicates: 8.55 MiB; chưa duyệt, không cộng với C vì có thể giao nhau.
