using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using BlockBlast.Managers;
public static class ScoringChecks
{
    [MenuItem("Block Blast/Run Scoring Checks")]
    public static void RunMenu() => Debug.Log(Run());
    public static string Run()
    {
        if(Application.isPlaying)throw new Exception("Run in Edit Mode");
        const string key="BlockBlast_HighScore";
        bool exists=PlayerPrefs.HasKey(key);int saved=PlayerPrefs.GetInt(key);
        var go=new GameObject("Isolated scoring check");go.SetActive(false);
        var m=go.AddComponent<GameManager>();
        var flags=BindingFlags.NonPublic|BindingFlags.Instance;
        var method=typeof(GameManager).GetMethod("ScorePlacement",flags);
        Action<string,int> set=(n,v)=>typeof(GameManager).GetField("<"+n+">k__BackingField",flags).SetValue(m,v);
        Action<int,int,bool> turn=(cells,lines,empty)=>method.Invoke(m,new object[]{cells,lines,empty});
        Action<bool,string> check=(ok,msg)=>{if(!ok)throw new Exception(msg);};
        try {
            PlayerPrefs.SetInt(key,0);
            int[] cells={4,4,5,4,4,4};int[] lines={0,1,2,2,1,1};int[] combos={0,0,0,2,0,0};int[] gains={4,14,45,124,314,14};
            for(int i=0;i<cells.Length;i++){set("CurrentCombo",combos[i]);turn(cells[i],lines[i],i==4);check(m.LastScoreGain==gains[i],"Formula case "+i);}
            set("CurrentCombo",3);for(int i=0;i<3;i++){turn(1,0,false);check(m.CurrentCombo==(i==2?0:3),"Combo grace");}
            for(int i=0;i<12;i++)turn(1,1,false);check(m.CurrentCombo==8,"Combo cap");
            int before=m.CurrentScore;turn(0,1,true);check(m.CurrentScore==before,"Invalid count");
            set("CurrentScore",599);set("HighScore",500);PlayerPrefs.SetInt(key,500);turn(1,0,false);check(m.HighScore==600&&PlayerPrefs.GetInt(key)==600,"Persistence");
            m.ResetRound();check(m.CurrentScore==0&&m.CurrentCombo==0&&m.LastScoreGain==0&&m.ConsecutiveNoClearTurns==0&&m.HighScore==600,"Replay");
            return "PASS: gains 4/14/45/124/314/14, 3-miss grace, x8 cap, invalid count, record 600, reset";
        } finally {UnityEngine.Object.DestroyImmediate(go);if(exists)PlayerPrefs.SetInt(key,saved);else PlayerPrefs.DeleteKey(key);PlayerPrefs.Save();}
    }
}
