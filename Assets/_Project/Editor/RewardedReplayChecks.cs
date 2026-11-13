using UnityEditor;

// Legacy test entry point now verifies the current free-Replay contract.
public static class RewardedReplayChecks
{
    [MenuItem("Block Blast/Tests/Replay without Ads (Play Mode)")]
    public static void Run() => OfflineAdsChecks.Run();
}
