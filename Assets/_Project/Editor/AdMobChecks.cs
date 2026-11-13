using System;
using System.Collections.Generic;
using BlockBlast.UI;
using UnityEditor;

public static class AdMobChecks
{
    [MenuItem("Block Blast/Tests/Rewarded callback safety")]
    public static void RunMenu() => UnityEngine.Debug.Log(Run());
    public static string Run()
    {
        var passed = new List<string>();
        Check("close without reward", t => t.Close(), RewardResult.Cancelled, passed);
        Check("duplicate reward and close", t => { t.Earn(); t.Earn(); t.Close(); t.Close(); }, RewardResult.Earned, passed);
        Check("show failure", t => t.Fail(), RewardResult.Failed, passed);
        Check("failure wins over earned", t => { t.Earn(); t.Fail(); t.Close(); }, RewardResult.Failed, passed);
        Check("late reward after close", t => { t.Close(); t.Earn(); t.Close(); }, RewardResult.Cancelled, passed);
        int count = 0;
        var cancelled = new RewardedRequest(_ => count++);
        cancelled.CancelRecipient(); cancelled.Earn(); cancelled.Close();
        if (count != 0) throw new Exception("Unloaded recipient received reward");
        passed.Add("unloaded recipient ignored");
        var pending = new RewardedRequest(_ => count++); pending.Earn();
        if (count != 0) throw new Exception("Gameplay resumed before fullscreen closed");
        pending.Close(); if (count != 1) throw new Exception("Confirmed earned reward lost");
        passed.Add("earned deferred until full-screen closed");
        var config = UnityEngine.Resources.Load<AdsConfiguration>("AdsConfiguration");
        if (config == null || !config.TestMode || !config.BannerId.Contains("3940256099942544") || !config.RewardedId.Contains("3940256099942544")) throw new Exception("Editor must use official test IDs");
        passed.Add("editor test IDs");
        return string.Join("\nPASS: ", passed);
    }
    private static void Check(string name, Action<RewardedRequest> sequence, RewardResult expected, List<string> passed)
    {
        int calls = 0; RewardResult actual = RewardResult.Unavailable;
        var request = new RewardedRequest(result => { calls++; actual = result; });
        sequence(request);
        if (calls != 1 || actual != expected) throw new Exception(name + ": " + calls + " / " + actual);
        passed.Add(name);
    }
}
