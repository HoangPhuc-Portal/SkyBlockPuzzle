using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BlockBlast.Core;
using BlockBlast.Data;
using BlockBlast.Difficulty;
using BlockBlast.Grid;
using UnityEditor;
using UnityEngine;

public static class DifficultyChecks
{
    static void Check(bool value,string why){if(!value)throw new Exception(why);}
    static string Key(BoardModel b){var s=new System.Text.StringBuilder();for(int r=0;r<b.Size;r++)for(int c=0;c<b.Size;c++)s.Append(b.Occupied(r,c)?'1':'0');return s.ToString();}
    public static BoardModel Fixture(int index)
    {
        var b=new BoardModel(GridBoard.GRID_SIZE);
        for(int r=0;r<b.Size;r++)for(int c=0;c<b.Size;c++)
        {
            bool occupied=index==1?r<b.Size/3&&c<b.Size-1:index==2?(r+c)%2==0:index==3?r!=c:index==4?(r<2&&c>=2)||(c<2&&r>=2):false;
            b.Set(r,c,occupied);
        }
        return b;
    }
    static DifficultyState State(int best,bool skilled,DifficultyConfig config)
    {
        var s=new DifficultyState();s.Reset(best);
        if(best>0)for(int i=0;i<config.historyLength;i++){s.Tick(1);s.Record(skilled?2:0,skilled?8:0,skilled?.1f:.8f,config);}
        return s;
    }
    public static void VerifyProof(BoardModel board,HandSelection result)
    {
        Check(result.hand!=null&&result.hand.Length==3,"Missing hand");
        Check(result.hand.Any(s=>BoardOpportunity.CountLegal(board,s)>0),"All selected pieces initially impossible");
        if(!result.complete)return;
        var copy=board.Clone();var seen=new HashSet<int>();
        foreach(var p in result.path){Check(seen.Add(p.piece),"Duplicate proof piece");Check(copy.PlaceAndClear(result.hand[p.piece].Source==null?null:Matrix(result.hand[p.piece]),p.row,p.column)>=0,"Invalid witnessed path");}
        Check(seen.Count==3,"Incomplete path");
    }
    static bool[,] Matrix(ShapeRuntimeData shape){var m=new bool[shape.rows,shape.columns];foreach(var c in shape.Cells)m[c.x,c.y]=true;return m;}
    [MenuItem("Block Blast/Tests/DDA Simulation Checks")]
    public static void RunMenu()=>UnityEngine.Debug.Log(Run());
    public static string Run()
    {
        Check(!EditorApplication.isPlaying,"Run simulation in Edit Mode");
        var config=AssetDatabase.LoadAssetAtPath<DifficultyConfig>(DifficultyInstaller.ConfigPath);Check(config!=null,"Missing config");
        var shapes=ShapeLibraryRebuild.Load();Check(shapes.Length==17,"Wrong shape catalog");
        var report=new List<string>{"DDA SIMULATION — "+DateTime.Now.ToString("s")};int events=0;
        Action placed=()=>events++;Action<int> score=_=>events++;GameEvents.OnShapePlaced+=placed;GameEvents.OnScoreChanged+=score;GameEvents.OnLinesCleared+=score;
        try
        {
            var selector=new HandSelector(shapes,937);var state=State(1000,true,config);var empty=Fixture(0);
            float previous=-1;
            foreach(int current in new[]{0,450,750,930,1000,1100,1300})
            {
                state=State(1000,true,config);HandSelection result=null;
                for(int i=0;i<12;i++)result=selector.Select(empty,current,state,config);
                Check(result.decision.index>=previous,"Score progression reduced DDI on open board");previous=result.decision.index;
                Check(state.BestScoreAtRunStart==1000,"Snapshot drifted");
                report.Add($"Progress {current}/1000: DDI={result.decision.index:F3}, weights={result.decision.weights}");
            }
            var first=selector.Select(empty,0,State(0,false,config),config);Check(first.decision.progress==-1&&!float.IsNaN(first.decision.index)&&first.decision.weights.x>=.7f,"First-run progression invalid");
            var mature=State(0,false,config);for(int i=0;i<45;i++)mature.Record(1,5,.1f,config);
            var firstAdvanced=selector.Select(empty,600,mature,config);Check(firstAdvanced.decision.index>first.decision.index,"First-run not progressing");
            var zero=DifficultyConfig.Normalize(new Vector3(-3,0,-2));Check(zero==Vector3.right,"Negative weights not sanitized");
            // Independent 3x3 fixture: initially only a single fits; clearing opens space for a square.
            var small=new BoardModel(3);for(int r=0;r<3;r++)for(int c=0;c<3;c++)small.Set(r,c,r!=c);
            var single=shapes.First(s=>s.name=="Single").Variants()[0];var square=shapes.First(s=>s.name=="Square_2x2").Variants()[0];
            Check(BoardOpportunity.CountLegal(small,square)==0,"Conditional fixture invalid");
            var proof=new HandViability().Check(small,new[]{square,single,single},50000,Stopwatch.StartNew(),1000);
            Check(proof.complete&&proof.path[0].piece!=0,"Search ignored clear or alternate piece order");
            var cross=small.Clone();Check(cross.PlaceAndClear(Matrix(single),0,0)==2,"Simultaneous row/column clear mismatch");
            Check(small.Occupied(0,1),"Simulation touched source");
            var impossible=new BoardModel(3);for(int r=0;r<3;r++)for(int c=0;c<3;c++)impossible.Set(r,c,true);
            var noProof=new HandViability().Check(impossible,new[]{square,square,square},10000,Stopwatch.StartNew(),1000);
            Check(!noProof.complete&&!noProof.exhausted&&noProof.depth==0,"Proven-impossible classification wrong");
            var limited=new HandViability().Check(empty,new[]{square,square,square},1,Stopwatch.StartNew(),1000);
            Check(limited.exhausted&&!limited.complete,"Budget exhaustion incorrectly called viable");
            report.Add("PASS: first-run no division by zero; relative milestones and snapshot; normalized weights; simulation clears both axes, explores another order; impossible vs unknown distinguished.");
            var openResult=selector.Select(Fixture(1),950,State(1000,true,config),config);
            var openLegal=selector.Candidates.First(c=>c.shape.Source.name=="Square_3x3").legal;
            var fragmented=selector.Select(Fixture(2),950,State(1000,true,config),config);
            Check(openLegal>0&&selector.Candidates.First(c=>c.shape.Source.name=="Square_3x3").legal==0&&fragmented.board.danger>openResult.board.danger,"Context ignores fragmentation");
            report.Add($"PASS: Square_3x3 legal {openLegal} -> 0 with fragmentation; danger {openResult.board.danger:F3} -> {fragmented.board.danger:F3}.");
            string[] names={"New player/open","Developing/open","Near record/skilled/open","Beyond record/skilled/open","Near record/struggling","Fragmented","Near full","Clear opportunities"};
            var hardRates=new float[names.Length];
            for(int scenario=0;scenario<names.Length;scenario++)
            {
                int boardKind=scenario==5?2:scenario==6?3:scenario==7?4:0;
                int best=scenario==0?0:1000, current=scenario==0?0:scenario==1?600:scenario==3?1200:950;
                bool good=scenario!=0&&scenario!=4;
                var b=Fixture(boardKind);string snapshot=Key(b);var rng=new HandSelector(shapes,41000+scenario);var stats=State(best,good,config);
                for(int i=0;i<12;i++)rng.Select(b,current,stats,config);
                int[] counts=new int[3];int rejected=0,fallback=0,relaxed=0,relief=0,complete=0;double ms=0,max=0;Vector3 target=Vector3.zero;
                for(int i=0;i<100;i++)
                {
                    var selection=rng.Select(b,current,stats,config);VerifyProof(b,selection);Check(Key(b)==snapshot,"Real board changed");
                    foreach(var s in selection.hand){Check(shapes.Contains(s.Source),"Unknown source");counts[(int)s.Source.difficulty]++;}
                    Check(selection.hand.Count(s=>s.Source.difficulty==ShapeDifficulty.Hard)<=config.maxHardPerHand,"Hard-per-hand limit exceeded");
                    rejected+=selection.rejected;if(selection.fallback)fallback++;if(selection.policyRelaxed)relaxed++;if(selection.decision.relief)relief++;if(selection.complete)complete++;
                    ms+=selection.milliseconds;max=Math.Max(max,selection.milliseconds);target+=selection.decision.weights;
                }
                hardRates[scenario]=counts[2]/300f;
                report.Add($"{names[scenario]} | target mean {target/100} | actual E/M/H {counts[0]/3f:F1}%/{counts[1]/3f:F1}%/{counts[2]/3f:F1}% | rejected {rejected}, fallback {fallback}, relaxed {relaxed}, relief {relief}, complete {complete}/100 | mean {ms/100:F3} ms, max {max:F3} ms");
            }
            Check(hardRates[3]>hardRates[0],"Advanced not more challenging than starter");
            Check(hardRates[6]<hardRates[3],"Near-full board did not reduce Hard");
            Check(events==0,"Selection emitted real gameplay events");
            report.Add("PASS: 800 sampled hands, all have an initial move; every claimed complete proof replayed; board and gameplay events unchanged. Desktop Editor timings only, not mobile FPS certification.");
            var tight=UnityEngine.Object.Instantiate(config);
            try
            {
                tight.searchNodeBudget=100;tight.searchMilliseconds=1;
                foreach(ViabilityPolicy policy in Enum.GetValues(typeof(ViabilityPolicy)))
                {
                    foreach(var band in tight.bands)band.viability=policy;
                    var b=Fixture(2);string snapshot=Key(b);var rng=new HandSelector(shapes,100+(int)policy);
                    for(int i=0;i<10;i++){var selection=rng.Select(b,1200,State(1000,true,tight),tight);VerifyProof(b,selection);Check(Key(b)==snapshot,"Policy fallback changed board");}
                }
            }
            finally {UnityEngine.Object.DestroyImmediate(tight);}
            report.Add("PASS: all 3 ViabilityPolicy modes retain legal moves under a 100-node / 1ms budget (30 additional hands).");
            report.Add("Gameplay regression: "+BlockBlastRegressionChecks.Run());
            Directory.CreateDirectory("Captures");File.WriteAllText("Captures/dda-simulation-checks.txt",string.Join("\n",report));
            return string.Join("\n",report);
        }
        finally {GameEvents.OnShapePlaced-=placed;GameEvents.OnScoreChanged-=score;GameEvents.OnLinesCleared-=score;}
    }
}
