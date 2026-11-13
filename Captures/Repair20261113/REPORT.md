# Gameplay repair 2026-11-13

## Changes
- GamePlay.unity: restored missing HUD object with existing GameHUD, SettingsPanel and HintController; reconnected GameOverController and RewardedGameplay; assigned existing Home/Replay/status references to preserve authored Settings layout.
- GameplayCell.prefab: background renderer size restored from 0.77 x 0.78 to 0.96 x 0.96, matching GridBoard.Reflow normalization. Sprite, fill, position and board logic unchanged.
- LineClearChecks.cs: added runtime assertions for Score text, Best text and PlayerPrefs high score after each clear scenario.

## Verification performed
- Unity 6000.3.24f1 successfully entered Play Mode after script compilation.
- Device Simulator: 64 blue cells fill the board; narrow regular gaps.
- Clicked Settings and Close in Play Mode: custom Settings artwork opens above board and closes; existing Home/Replay buttons preserved.
- Run Line Clear Play Checks: PASS on 2026-11-13. Seven scenarios cover row, column, multiple rows/columns, cross, three lines and full-board clear. New Score/Best/PlayerPrefs assertions passed. Existing preview, scoring/combo, VFX pool, reset and Game Over checks passed. Test restored original high score.
- Restored scene IDs unique and local controller references resolve.

## Limits
- No device build or production ads test performed.
- Editor reports an unrelated Unity AI NoSubscription service error.
- Other user edits in MainMenu, Settings art, fonts and editor settings were not reverted.
- Scene repair diff and pre-repair copies are in this directory. No commit/push performed.
