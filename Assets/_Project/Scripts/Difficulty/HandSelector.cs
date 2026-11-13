using System;
using System.Collections.Generic;
using System.Diagnostics;
using BlockBlast.Data;
using BlockBlast.Grid;
using UnityEngine;
namespace BlockBlast.Difficulty
{
    public sealed class HandSelection
    {
        public ShapeRuntimeData[] hand;
        public PlacementStep[] path;
        public BoardOpportunity board;
        public DifficultyDecision decision;
        public int rejected,searchNodes;
        public bool fallback,complete,policyRelaxed,noLegalMoves;
        public double milliseconds;
        public string Summary()
        {
            string names=hand==null?"none":string.Join(", ",Array.ConvertAll(hand,s=>s.Source.name+"@"+s.Rotation));
            return $"{decision.tier} DDI={decision.index:F2} progress={decision.progress:F2} performance={decision.performance:F2} occupancy={board.occupancy:F2} legal={board.totalLegal} danger={board.danger:F2} weights={decision.weights} rejected={rejected} fallback={fallback} complete={complete} relaxed={policyRelaxed} nodes={searchNodes} ms={milliseconds:F2} [{names}]";
        }
    }
    // Pure selector: no GameObjects, events, Score writes or Ads dependency.
    public sealed class HandSelector
    {
        readonly List<ShapeCandidate> candidates=new List<ShapeCandidate>();
        readonly System.Random random;
        readonly HandViability viability=new HandViability();
        public IReadOnlyList<ShapeCandidate> Candidates=>candidates;
        public HandSelector(IEnumerable<ShapeData> shapes,int seed)
        {
            random=new System.Random(seed);var sources=new HashSet<ShapeData>();
            foreach(var shape in shapes)
            {
                if(shape==null||!shape.IsValid||!sources.Add(shape))continue;
                var unique=new HashSet<string>();var variants=shape.Variants();int count=0;
                foreach(var v in variants)if(unique.Add(v.Key))count++;
                unique.Clear();foreach(var v in variants)if(unique.Add(v.Key))candidates.Add(new ShapeCandidate{shape=v,sourceVariants=count});
            }
        }
        ShapeCandidate Pick(Vector3 weights,DifficultyConfig config,bool onlyLegal=false)
        {
            float[] totals=new float[3];
            foreach(var c in candidates)if(!onlyLegal||c.legal>0)totals[(int)c.shape.Source.difficulty]+=1;
            for(int i=0;i<3;i++)if(totals[i]==0)weights[i]=0;
            weights=DifficultyConfig.Normalize(weights);
            double roll=random.NextDouble();int tier=roll<weights.x?0:roll<weights.x+weights.y?1:2;
            if(totals[tier]==0){for(int i=0;i<3;i++)if(totals[i]>0){tier=i;break;}}
            float total=0;
            foreach(var c in candidates)if((int)c.shape.Source.difficulty==tier&&(!onlyLegal||c.legal>0))total+=ContextWeight(c,config);
            if(total<=0)return null;
            float target=(float)random.NextDouble()*total;ShapeCandidate last=null;
            foreach(var c in candidates)
            {
                if((int)c.shape.Source.difficulty!=tier||(onlyLegal&&c.legal==0))continue;
                last=c;target-=ContextWeight(c,config);if(target<=0)return c;
            }
            return last;
        }
        static float ContextWeight(ShapeCandidate c,DifficultyConfig config)=>Mathf.Lerp(Mathf.Max(.01f,config.contextFloor),1,(float)c.legal/Mathf.Max(1,c.possible))/Mathf.Max(1,c.sourceVariants);
        public HandSelection Select(BoardModel realBoard,int score,DifficultyState state,DifficultyConfig config)
        {
            var watch=Stopwatch.StartNew();
            var board=realBoard.Clone(); // Never mutate the real board, even during fallback.
            var result=new HandSelection();result.board=BoardOpportunity.Evaluate(board,candidates,config);
            result.decision=DifficultyDecision.Evaluate(score,state,result.board,config);
            if(result.board.dangerLevel>=2)state.Danger();
            if(candidates.Count==0){result.noLegalMoves=true;result.milliseconds=watch.Elapsed.TotalMilliseconds;return result;}
            if(result.board.totalLegal==0)
            {
                // Library genuinely has no legal move. Existing Tray GameOver detection remains authoritative.
                result.noLegalMoves=true;result.hand=new[]{candidates[0].shape,candidates[0].shape,candidates[0].shape};
                result.milliseconds=watch.Elapsed.TotalMilliseconds;return result;
            }
            Vector3 weights=result.decision.weights;
            ShapeRuntimeData[] partialHand=null;PlacementStep[] partialPath=null;
            int attempts=Mathf.Max(1,config.candidateAttempts);
            for(int attempt=0;attempt<attempts&&watch.Elapsed.TotalMilliseconds<config.searchMilliseconds;attempt++)
            {
                var hand=new ShapeRuntimeData[3];int hard=0,initial=0;
                for(int i=0;i<3;i++)
                {
                    var w=weights;if(hard>=config.maxHardPerHand)w.z=0;
                    var pick=Pick(w,config);hand[i]=pick.shape;if(pick.legal>0)initial++;if(pick.shape.Source.difficulty==ShapeDifficulty.Hard)hard++;
                }
                if(initial==0){result.rejected++;continue;}
                int remaining=config.searchNodeBudget-result.searchNodes;if(remaining<=0)break;
                var proof=viability.Check(board,hand,remaining,watch,config.searchMilliseconds);result.searchNodes+=proof.nodes;
                int minimum=result.decision.policy==ViabilityPolicy.CompleteHand?3:result.decision.policy==ViabilityPolicy.PreferThreeMinimumTwo?2:1;
                if(proof.complete)
                {
                    result.hand=hand;result.complete=true;result.path=proof.path;break;
                }
                if(proof.exhausted&&proof.depth>=minimum){partialHand=hand;partialPath=proof.path;}
                result.rejected++;
                // Reject proven impossible candidates at EVERY tier. Retry with less Hard, never force a loss.
                float transfer=weights.z*.35f;weights.z-=transfer;weights.x+=transfer*.7f;weights.y+=transfer*.3f;
                if(proof.exhausted)break;
            }
            if(result.hand==null)
            {
                result.fallback=true;
                // Construct a witnessed path on a private board, with a bounded number of attempts.
                for(int attempt=0;attempt<2&&result.hand==null;attempt++)
                {
                    var simulation=board.Clone();var hand=new ShapeRuntimeData[3];var path=new PlacementStep[3];bool success=true;int hard=0;
                    for(int i=0;i<3;i++)
                    {
                        foreach(var c in candidates)c.legal=BoardOpportunity.CountLegal(simulation,c.shape);
                        var fallbackWeights=state.TinyHands>=config.tinyHandLimit&&result.board.dangerLevel<2
                            ? new Vector3(.35f,.6f,.05f) : new Vector3(.65f,.3f,.05f);
                        if(hard>=config.maxHardPerHand)fallbackWeights.z=0;
                        var chosen=Pick(fallbackWeights,config,true);
                        if(chosen==null){success=false;break;}
                        if(chosen.shape.Source.difficulty==ShapeDifficulty.Hard)hard++;
                        hand[i]=chosen.shape;int count=chosen.legal;int position=random.Next(count);bool placed=false;
                        for(int r=0;r<=simulation.Size-chosen.shape.rows&&!placed;r++)for(int c=0;c<=simulation.Size-chosen.shape.columns;c++)
                            if(simulation.CanPlace(chosen.shape.GetMatrix(),r,c)&&position--==0){simulation.PlaceAndClear(chosen.shape.GetMatrix(),r,c);path[i]=new PlacementStep{piece=i,row=r,column=c};placed=true;break;}
                    }
                    if(success){result.hand=hand;result.path=path;result.complete=true;}
                }
                if(result.hand==null)
                {
                    var single=candidates.Find(c=>c.shape.Cells.Count==1);
                    if(single!=null)
                    {
                        var simulation=board.Clone();var hand=new[]{single.shape,single.shape,single.shape};var path=new PlacementStep[3];int placed=0;
                        for(int i=0;i<3;i++)for(int r=0;r<simulation.Size;r++){bool done=false;for(int c=0;c<simulation.Size;c++)if(simulation.CanPlace(single.shape.GetMatrix(),r,c)){simulation.PlaceAndClear(single.shape.GetMatrix(),r,c);path[i]=new PlacementStep{piece=i,row=r,column=c};placed++;done=true;break;}if(done)break;}
                        if(placed==3){result.hand=hand;result.path=path;result.complete=true;}
                    }
                    if(result.hand==null)
                    {
                        if(partialHand!=null){result.hand=partialHand;result.path=partialPath;result.policyRelaxed=true;}
                        else
                        {
                        // Misconfigured/custom catalog without a safe triple: at least preserve a real initial move.
                        foreach(var c in candidates)c.legal=BoardOpportunity.CountLegal(board,c.shape);
                        var legal=Pick(Vector3.right,config,true);result.hand=new[]{legal.shape,legal.shape,legal.shape};result.policyRelaxed=true;
                        }
                    }
                }
                // Restore metrics to the actual initial board after constructive simulations.
                foreach(var c in candidates)c.legal=BoardOpportunity.CountLegal(board,c.shape);
            }
            bool tiny=true,allHard=true;float pressure=0;
            foreach(var s in result.hand){tiny&=s.Cells.Count<=2;allHard&=s.Source.difficulty==ShapeDifficulty.Hard;var c=candidates.Find(x=>x.shape==s);pressure+=c.ContextDifficulty;}
            state.TinyHands=tiny?state.TinyHands+1:0;state.HardHands=allHard?state.HardHands+1:0;state.LastContextPressure=pressure/3;
            result.milliseconds=watch.Elapsed.TotalMilliseconds;return result;
        }
    }
}
