using System.Collections.Generic;
using UnityEngine;
namespace BlockBlast.Difficulty
{
    public sealed class DifficultyState
    {
        struct Turn { public int lines,combo; public float seconds,occupancy; }
        readonly Queue<Turn> recent=new Queue<Turn>();
        public int BestScoreAtRunStart {get;private set;}
        public int Placements {get;private set;}
        public int Lines {get;private set;}
        public int MultiClears {get;private set;}
        public int NoClearStreak {get;private set;}
        public int NoMoveEvents {get;private set;}
        public int DangerHands {get;private set;}
        public int ReliefRemaining {get;set;}
        public int TinyHands {get;set;}
        public int HardHands {get;set;}
        public float Index {get;set;}
        public float ActiveSeconds {get;private set;}
        public float LastHandSeconds {get;private set;}
        public float LastContextPressure {get;set;}
        float lastPlacement,handStarted;
        public int RecentCount=>recent.Count;
        public void Reset(int best)
        {
            BestScoreAtRunStart=Mathf.Max(0,best);Placements=Lines=MultiClears=NoClearStreak=NoMoveEvents=DangerHands=0;
            ReliefRemaining=TinyHands=HardHands=0;Index=ActiveSeconds=lastPlacement=handStarted=LastHandSeconds=LastContextPressure=0;recent.Clear();
        }
        public void Tick(float seconds){ActiveSeconds+=Mathf.Clamp(seconds,0,1);}
        public void StartHand(){handStarted=ActiveSeconds;lastPlacement=ActiveSeconds;}
        public void CompleteHand(){LastHandSeconds=ActiveSeconds-handStarted;}
        public void Continue(){recent.Clear();NoClearStreak=0;ReliefRemaining=TinyHands=HardHands=0;LastContextPressure=0;Index=Mathf.Min(Index,.25f);lastPlacement=handStarted=ActiveSeconds;}
        public void NoMoves(){NoMoveEvents++;}
        public void Danger(){DangerHands++;}
        public void Record(int lines,int combo,float occupancy,DifficultyConfig config)
        {
            Placements++;Lines+=lines;if(lines>1)MultiClears++;
            NoClearStreak=lines==0?NoClearStreak+1:0;
            recent.Enqueue(new Turn{lines=lines,combo=combo,seconds=ActiveSeconds-lastPlacement,occupancy=occupancy});
            lastPlacement=ActiveSeconds;while(recent.Count>Mathf.Max(1,config.historyLength))recent.Dequeue();
        }
        public float ClearRate
        {
            get{if(recent.Count==0)return 0;int cleared=0;foreach(var t in recent)if(t.lines>0)cleared++;return (float)cleared/recent.Count;}
        }
        public float Performance(DifficultyConfig config)
        {
            if(recent.Count==0)return 0;
            float sum=0;
            foreach(var t in recent)
            {
                float value=(t.lines>0?.45f:0)+(t.lines>1?.15f:0)+.2f*Mathf.Clamp01(t.combo/Mathf.Max(1,config.comboReference))+.2f*(1-t.occupancy);
                if(config.useSpeed&&t.lines>0&&t.seconds>0&&t.seconds<config.fastSeconds)value+=config.speedInfluence;
                sum+=value;
            }
            return Mathf.Clamp01(sum/recent.Count)*Mathf.Clamp01((float)recent.Count/Mathf.Max(1,config.historyLength));
        }
    }
}
