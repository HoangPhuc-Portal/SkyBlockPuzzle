using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using BlockBlast.UI;
using BlockBlast.Grid;
using BlockBlast.Managers;
using BlockBlast.Shape;

// Editor-only provider deliberately delivers duplicate/late callbacks to test the recipient guard.
public sealed class OfflineAdsCheckProvider : RewardedProvider
{
    public bool Ready = true;
    public int Shows, Prepares;
    public Action<RewardResult> Pending;
    public override bool HasNetwork => AdsManager.NetworkAvailable;
    public override bool IsReady => Ready && Pending == null;
    public override void Prepare() { Prepares++; }
    public override void Show(Action<RewardResult> callback) { Shows++; Pending = callback; }
    public override void Cancel() { Pending = null; }
    public void Complete(RewardResult result) { var callback = Pending; Pending = null; callback?.Invoke(result); callback?.Invoke(result); }
}

public static class OfflineAdsChecks
{
    static readonly BindingFlags Fields = BindingFlags.Instance | BindingFlags.NonPublic;
    static readonly FieldInfo Network = typeof(AdsManager).GetField("NetworkOverrideForTests", BindingFlags.Static | BindingFlags.NonPublic);
    static IEnumerator routine;
    static double deadline;
    static float next;
    static bool background;
    static object previousNetwork;
    static readonly Dictionary<string, string> saved = new Dictionary<string, string>();
    static readonly List<string> passed = new List<string>();
    static RewardedGameplay rewards;
    static RewardedProvider original;
    static OfflineAdsCheckProvider provider;
    static GameOverController flow;
    static SettingsPanel settings;
    static HintController hints;
    static GameManager gm;
    static T Get<T>(object target, string name) => (T)target.GetType().GetField(name, Fields).GetValue(target);
    static void Set(object target, string name, object value) => target.GetType().GetField(name, Fields).SetValue(target, value);
    static void Check(bool value, string reason) { if (!value) throw new Exception(reason); }
    static void Pass(string message) { passed.Add("PASS: " + message); }
    static void Click(object target, string name) => Get<Button>(target, name).onClick.Invoke();
    static GridCell[] Cells => GridBoard.Instance.GetComponentsInChildren<GridCell>();
    static void Score() => typeof(GameManager).GetMethod("ScorePlacement", Fields).Invoke(gm, new object[] { 3, 1, false });
    static void Seed()
    {
        Score(); Cells[0].SetOccupied(true); GridBoard.Instance.SyncModel();
        var p = TrayManager.Instance.Pieces.First();
        GridBoard.Instance.UpdateDragPreview(p.ShapeData, GridBoard.Instance.CellPosition(2, 2), p.BlockSprite);
    }
    static void AssertNewRound(int high, int coins, int[] old)
    {
        Check(!gm.IsInputLocked && !gm.IsGameOver && !settings.IsOpen && !flow.IsPopupVisible && !rewards.IsBusy, "Panel/input stuck after replay");
        Check(gm.CurrentScore == 0 && gm.CurrentCombo == 0 && gm.ConsecutiveNoClearTurns == 0, "Score/combo reset incorrect");
        Check(gm.HighScore == high && gm.Coins == coins, "Replay changed record/coins");
        Check(Cells.All(c => !c.IsOccupied), "Replay left occupancy");
        for (int r=0;r<GridBoard.GRID_SIZE;r++) for(int c=0;c<GridBoard.GRID_SIZE;c++) Check(!GridBoard.Instance.Model.Occupied(r,c), "Model not empty");
        Check(TrayManager.Instance.Pieces.Count(p => p != null) == 3 && TrayManager.Instance.Pieces.All(p => !old.Contains(p.GetInstanceID())), "Tray not fresh 3 pieces");
        Check(Get<List<GridCell>>(GridBoard.Instance,"currentPreviewCells").Count == 0 && Get<List<GridCell>>(GridBoard.Instance,"currentHighlightedLineCells").Count == 0, "Preview/highlight retained");
        Check(hints.Remaining == Get<int>(hints,"initialHintCount"), "Existing per-round hint policy changed");
    }
    [MenuItem("Block Blast/Tests/Offline Online Gameplay and Ads (Play Mode)")]
    public static void Run()
    {
        if (!EditorApplication.isPlaying || routine != null) { Debug.LogWarning("Start Play Mode, then run once."); return; }
        passed.Clear(); saved.Clear();
        foreach (string key in new[] {"BlockBlast_HighScore", "UI.Music", "UI.SFX", "UI.Haptic"})
            saved[key] = !PlayerPrefs.HasKey(key) ? null : (key=="UI.Music" || key=="UI.SFX" ? PlayerPrefs.GetFloat(key).ToString(System.Globalization.CultureInfo.InvariantCulture) : PlayerPrefs.GetInt(key).ToString());
        previousNetwork = Network.GetValue(null); Network.SetValue(null, false);
        background = Application.runInBackground; Application.runInBackground = true; EditorApplication.isPaused = false;
        routine = Exercise(); deadline = EditorApplication.timeSinceStartup + 120; next = 0;
        EditorApplication.update += Tick;
    }
    static void Tick()
    {
        try
        {
            Check(EditorApplication.isPlaying && EditorApplication.timeSinceStartup < deadline, "Play stopped/timeout");
            if (Time.unscaledTime < next) return;
            if (!routine.MoveNext()) { Finish(null); return; }
            next = Time.unscaledTime + (routine.Current is float ? (float)routine.Current : 0);
        }
        catch(Exception error) { Finish(error.ToString()); }
    }
    static IEnumerator Exercise()
    {
        SceneManager.LoadScene("GamePlay"); yield return 1f;
        gm=GameManager.Instance; flow=UnityEngine.Object.FindFirstObjectByType<GameOverController>();
        settings=UnityEngine.Object.FindFirstObjectByType<SettingsPanel>(); hints=UnityEngine.Object.FindFirstObjectByType<HintController>();
        rewards=UnityEngine.Object.FindFirstObjectByType<RewardedGameplay>(); original=Get<RewardedProvider>(rewards,"provider");
        Check(original is AdMobRewardedProvider, "Scene not wired to AdMob provider");
        provider=rewards.gameObject.AddComponent<OfflineAdsCheckProvider>(); Set(rewards,"provider",provider);
        int init=AdsManager.Instance.InitializationCalls;
        AdsManager.Instance.PrepareRewarded(); AdsManager.Instance.LoadRewarded(); AdsManager.Instance.LoadBanner();
        Check(!AdsManager.Instance.IsRewardedReady && AdsManager.Instance.InitializationCalls==init, "Offline SDK request");
        Pass("Offline manager does not initialize/load/show; production scene uses existing AdMob provider");
        foreach(bool online in new[]{false,true})
        {
            Network.SetValue(null,online); provider.Ready=online; yield return .1f;
            Seed(); int high=gm.HighScore, coins=gm.Coins; int[] old=TrayManager.Instance.Pieces.Select(p=>p.GetInstanceID()).ToArray();
            int shows=provider.Shows, prepares=provider.Prepares;
            gm.ConfirmGameOver(); yield return 1f;
            Click(flow,"replayButton"); Click(flow,"replayButton"); yield return .3f;
            AssertNewRound(high,coins,old);
            Check(provider.Shows==shows && provider.Prepares==prepares,"Game Over Replay requested ad");
            Pass((online?"Online":"Offline")+" Game Over Replay, double click, clean board/preview, fresh tray, preserved record/coins");
            Seed(); high=gm.HighScore; coins=gm.Coins; old=TrayManager.Instance.Pieces.Select(p=>p.GetInstanceID()).ToArray();
            Click(settings,"open"); Check(settings.IsOpen,"Settings did not open");
            Click(settings,"replay"); Click(settings,"replay"); yield return .1f;
            AssertNewRound(high,coins,old);
            Check(provider.Shows==shows && provider.Prepares==prepares,"Settings Replay requested ad");
            Pass((online?"Online":"Offline")+" Settings Replay and open without ads");
            int before=hints.Remaining; hints.ShowHint();
            Check(hints.Remaining==before-1 && provider.Shows==shows,"Available Hint requires ad or wrong count"); hints.Cancel();
            Pass((online?"Online":"Offline")+" available Hint consumes exactly one without ad");
        }
        typeof(HintController).GetProperty("Remaining").SetValue(hints,0);
        Network.SetValue(null,false); hints.ShowHint(); Click(rewards,"watchHint");
        int count=provider.Shows, score=gm.CurrentScore;
        Check(!rewards.IsBusy && hints.Remaining==0 && Get<TMP_Text>(rewards,"hintStatus").text.Contains("Internet"),"Offline hint incorrectly rewarded/blocked");
        rewards.CloseHint(); Check(!gm.IsInputLocked,"Offline hint popup stuck");
        Pass("Offline exhausted Hint asks for Internet, no grant, no score change, popup closes");
        Network.SetValue(null,true); provider.Ready=false; hints.ShowHint(); Click(rewards,"watchHint");
        Check(!rewards.IsBusy && provider.Shows==count && provider.Prepares>0 && hints.Remaining==0,"Not-ready handling");
        provider.Ready=true;
        foreach(var result in new[]{RewardResult.Cancelled,RewardResult.Failed,RewardResult.Earned})
        {
            Click(rewards,"watchHint"); Check(rewards.IsBusy,"Reward request did not start");
            Click(rewards,"watchHint"); provider.Complete(result);
            Check(hints.Remaining==(result==RewardResult.Earned?2:0) && gm.CurrentScore==score && !rewards.IsBusy,"Hint reward count/failure/duplicate incorrect");
        }
        Check(!gm.IsInputLocked,"Rewarded hint remained modal");
        Pass("Online not-ready prepares; cancelled/failed grant zero; earned duplicated callback grants exactly 2, no auto-use");
        Seed(); gm.ConfirmGameOver(); yield return 1f;
        score=gm.CurrentScore; int record=gm.HighScore; var ids=TrayManager.Instance.Pieces.Select(p=>p.GetInstanceID()).ToArray();
        Network.SetValue(null,false); count=provider.Shows; Click(rewards,"continueButton");
        Check(provider.Shows==count && flow.IsPopupVisible && gm.IsGameOver && Cells.Any(c=>c.IsOccupied) && Get<TMP_Text>(rewards,"reviveStatus").text.Contains("Internet"),"Offline Continue changed game");
        Pass("Offline Continue rejected; Game Over and board preserved");
        Network.SetValue(null,true); Click(rewards,"continueButton"); provider.Complete(RewardResult.Failed);
        Check(gm.IsGameOver && gm.CurrentScore==score,"Failed Continue changed game");
        Click(rewards,"continueButton"); provider.Complete(RewardResult.Earned);
        Check(!gm.IsInputLocked && !gm.IsGameOver && !flow.IsPopupVisible && gm.CurrentScore==score && gm.HighScore==record && gm.CurrentCombo==0 && Cells.All(c=>!c.IsOccupied),"Earned Continue reset incorrect");
        Check(TrayManager.Instance.Pieces.Count(p=>p!=null)==3 && TrayManager.Instance.Pieces.All(p=>!ids.Contains(p.GetInstanceID())),"Continue tray not fresh");
        Pass("Online failed Continue preserves round; earned Continue clears board, preserves score/record, resets combo and respawns 3");
        // Invalidated callback may arrive after a reset; it must not grant anything.
        typeof(HintController).GetProperty("Remaining").SetValue(hints,0); hints.ShowHint(); Click(rewards,"watchHint");
        var late=provider.Pending; rewards.ResetRequests(); late(RewardResult.Earned); Check(hints.Remaining==0,"Late callback granted reward");
        Network.SetValue(null,false); yield return .1f;
        Check(!gm.IsInputLocked && !AdsManager.Instance.IsRewardedReady,"Network loss locked gameplay");
        Click(settings,"open"); Get<Slider>(settings,"music").value=.23f; Get<Slider>(settings,"sfx").value=.37f; Get<Toggle>(settings,"haptic").isOn=false;
        Check(Mathf.Approximately(PlayerPrefs.GetFloat("UI.Music"),.23f) && Mathf.Approximately(PlayerPrefs.GetFloat("UI.SFX"),.37f) && !SettingsPanel.HapticEnabled,"Offline settings persistence");
        count=provider.Shows; Click(settings,"replay");
        Check(provider.Shows==count && Mathf.Approximately(PlayerPrefs.GetFloat("UI.Music"),.23f),"Replay changed settings");
        Pass("Late callback ignored; online-to-offline gameplay continues; offline audio/haptic settings saved and preserved on Replay");
        Network.SetValue(null,true); yield return 2f;
        Check(Get<bool>(AdsManager.Instance,"networkWasAvailable") && (AdsManager.Instance.IsInitialized || Get<bool>(AdsManager.Instance,"consentPending") || Get<bool>(AdsManager.Instance,"initializeStarted")),"Reconnect did not restart ads pipeline");
        Pass("Offline-to-online restarts existing SDK pipeline without recreating manager (Editor simulator)");
        Network.SetValue(null,false); Click(settings,"open"); Click(settings,"home"); yield return 1f;
        Check(SceneManager.GetActiveScene().name=="MainMenu","Offline Home failed");
        var loader=UnityEngine.Object.FindFirstObjectByType<PlaySceneLoader>(); Check(loader!=null,"Missing menu loader"); loader.Play(); yield return 2f;
        Check(SceneManager.GetActiveScene().name=="GamePlay" && !GameManager.Instance.IsInputLocked,"Offline Play loading failed");
        Pass("Offline Settings Home and MainMenu Play scene loading work");
        Pass(AdMobChecks.Run());
    }
    static void Finish(string error)
    {
        EditorApplication.update-=Tick; (routine as IDisposable)?.Dispose(); routine=null;
        if(rewards!=null) { rewards.ResetRequests(); Set(rewards,"provider",original); }
        if(provider!=null) UnityEngine.Object.DestroyImmediate(provider);
        Network.SetValue(null,previousNetwork); Application.runInBackground=background;
        foreach(var entry in saved)
        {
            if(entry.Value==null) PlayerPrefs.DeleteKey(entry.Key);
            else if(entry.Key=="UI.Music" || entry.Key=="UI.SFX") PlayerPrefs.SetFloat(entry.Key,float.Parse(entry.Value,System.Globalization.CultureInfo.InvariantCulture));
            else PlayerPrefs.SetInt(entry.Key,int.Parse(entry.Value));
        }
        PlayerPrefs.Save(); Directory.CreateDirectory("Captures");
        string report=DateTime.Now.ToString("s")+"\n"+string.Join("\n",passed)+(error==null?"\nALL PASS (simulated network/provider in Editor; native device verification still required).":"\nFAIL: "+error);
        File.WriteAllText("Captures/offline-online-ads-checks.txt",report);
        if(error==null) Debug.Log(report); else Debug.LogError(report);
        if(EditorApplication.isPlaying) SceneManager.LoadScene("MainMenu");
    }
}
