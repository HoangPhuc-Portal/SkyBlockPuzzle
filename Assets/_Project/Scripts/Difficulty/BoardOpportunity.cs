using System.Collections.Generic;
using BlockBlast.Data;
using BlockBlast.Grid;
using UnityEngine;
namespace BlockBlast.Difficulty
{
    public sealed class ShapeCandidate
    {
        public ShapeRuntimeData shape;
        public int legal,possible,sourceVariants;
        public float ContextDifficulty=>1-(float)legal/Mathf.Max(1,possible);
    }
    public sealed class BoardOpportunity
    {
        public float occupancy,legalRatio,largestEmptyRatio,largeShapeAvailability,danger;
        public int totalLegal,largestEmptyRegion,dangerLevel;
        public static float Occupancy(BoardModel board)
        {int count=0;for(int r=0;r<board.Size;r++)for(int c=0;c<board.Size;c++)if(board.Occupied(r,c))count++;return (float)count/(board.Size*board.Size);}
        public static int CountLegal(BoardModel board,ShapeRuntimeData shape)
        {int count=0;for(int r=0;r<=board.Size-shape.rows;r++)for(int c=0;c<=board.Size-shape.columns;c++)if(board.CanPlace(shape.GetMatrix(),r,c))count++;return count;}
        public static BoardOpportunity Evaluate(BoardModel board,List<ShapeCandidate> candidates,DifficultyConfig config)
        {
            var result=new BoardOpportunity{occupancy=Occupancy(board)};int possible=0,large=0,largeFit=0;
            foreach(var candidate in candidates)
            {
                var s=candidate.shape;candidate.possible=Mathf.Max(0,board.Size-s.rows+1)*Mathf.Max(0,board.Size-s.columns+1);
                candidate.legal=CountLegal(board,s);result.totalLegal+=candidate.legal;possible+=candidate.possible;
                if(s.Cells.Count>=5||s.Source.difficulty==ShapeDifficulty.Hard){large++;if(candidate.legal>0)largeFit++;}
            }
            result.legalRatio=(float)result.totalLegal/Mathf.Max(1,possible);result.largeShapeAvailability=large==0?1:(float)largeFit/large;
            var seen=new bool[board.Size,board.Size];var queue=new Queue<Vector2Int>();
            var directions=new[]{Vector2Int.up,Vector2Int.down,Vector2Int.left,Vector2Int.right};int empty=0;
            for(int r=0;r<board.Size;r++)for(int c=0;c<board.Size;c++)
            {
                if(board.Occupied(r,c)){continue;}empty++;if(seen[r,c])continue;
                int area=0;queue.Enqueue(new Vector2Int(r,c));seen[r,c]=true;
                while(queue.Count>0)
                {
                    var p=queue.Dequeue();area++;
                    foreach(var d in directions){var n=p+d;if(n.x<0||n.y<0||n.x>=board.Size||n.y>=board.Size||seen[n.x,n.y]||board.Occupied(n.x,n.y))continue;seen[n.x,n.y]=true;queue.Enqueue(n);}
                }
                result.largestEmptyRegion=Mathf.Max(result.largestEmptyRegion,area);
            }
            result.largestEmptyRatio=(float)result.largestEmptyRegion/Mathf.Max(1,empty);
            Vector4 w=config.dangerWeights;float sum=Mathf.Max(.001f,w.x+w.y+w.z+w.w);
            float scarcity=1-Mathf.Clamp01((float)result.totalLegal/Mathf.Max(1,candidates.Count)/Mathf.Max(1,config.legalPlacementReference));
            result.danger=Mathf.Clamp01((w.x*Mathf.InverseLerp(config.occupancyRange.x,config.occupancyRange.y,result.occupancy)+w.y*scarcity+w.z*(1-result.largestEmptyRatio)+w.w*(1-result.largeShapeAvailability))/sum);
            result.dangerLevel=result.danger>=config.dangerThresholds.z?3:result.danger>=config.dangerThresholds.y?2:result.danger>=config.dangerThresholds.x?1:0;
            return result;
        }
    }
    public sealed class DifficultyDecision
    {
        public float progress,index,performance;
        public Vector3 weights;
        public string tier;
        public ViabilityPolicy policy;
        public bool relief;
        public static DifficultyDecision Evaluate(int score,DifficultyState state,BoardOpportunity board,DifficultyConfig config)
        {
            var d=new DifficultyDecision();
            float p;
            if(state.BestScoreAtRunStart>0)
            {
                d.progress=(float)score/state.BestScoreAtRunStart;
                var t=config.scoreRatios;
                if(d.progress<t.x)p=.25f*Mathf.InverseLerp(0,t.x,d.progress);
                else if(d.progress<t.y)p=Mathf.Lerp(.25f,.5f,Mathf.InverseLerp(t.x,t.y,d.progress));
                else if(d.progress<t.z)p=Mathf.Lerp(.5f,.75f,Mathf.InverseLerp(t.y,t.z,d.progress));
                else p=Mathf.Lerp(.75f,1,Mathf.InverseLerp(t.z,t.w,d.progress));
            }
            else
            {
                d.progress=-1;var w=DifficultyConfig.Normalize(config.firstRunWeights);
                p=w.x*Mathf.Clamp01(score/Mathf.Max(1,config.firstRunScore))+w.y*Mathf.Clamp01(state.Placements/Mathf.Max(1,config.firstRunPlacements))+w.z*Mathf.Clamp01(state.Lines/Mathf.Max(1,config.firstRunLines));
            }
            d.performance=state.Performance(config);float opportunity=1-board.danger;
            bool challenge=d.performance>=config.performanceForChallenge&&opportunity>=config.opportunityForChallenge&&board.dangerLevel<2;
            if(!challenge)p=Mathf.Min(p,.55f);
            var weights=DifficultyConfig.Normalize(config.indexWeights);
            float maturity=Mathf.Clamp01((float)state.Placements/Mathf.Max(1,config.historyLength));
            float target=weights.x*p+weights.y*d.performance+weights.z*opportunity*maturity-config.pressureAdjustment*board.danger;
            bool struggle=state.NoClearStreak>=config.noClearReliefTurns||(state.RecentCount>=3&&state.ClearRate<config.struggleClearRate)||state.LastContextPressure>.85f;
            if(struggle||board.dangerLevel>=2)state.ReliefRemaining=Mathf.Max(state.ReliefRemaining,config.reliefHands);
            d.relief=state.ReliefRemaining>0;if(d.relief){target-=config.pressureAdjustment*.5f;state.ReliefRemaining--;}
            state.Index=Mathf.MoveTowards(state.Index,Mathf.Clamp01(target),target>state.Index?config.increaseStep:config.decreaseStep);d.index=state.Index;
            var bands=config.bands;
            if(bands==null||bands.Length==0){d.weights=Vector3.right;d.tier="Safe default";return d;}
            int lower=0;while(lower+1<bands.Length&&bands[lower+1].index<=d.index)lower++;
            var a=bands[lower];var b=bands[Mathf.Min(lower+1,bands.Length-1)];
            d.weights=DifficultyConfig.Normalize(Vector3.Lerp(a.weights,b.weights,Mathf.InverseLerp(a.index,b.index,d.index)));
            d.tier=a.name;d.policy=a.viability;
            float pressure=Mathf.Max(board.danger,d.relief?.6f:0);
            float removed=d.weights.z*Mathf.Clamp01(config.hardRelief*pressure);d.weights.z-=removed;d.weights.x+=removed*.65f;d.weights.y+=removed*.35f;
            if(state.HardHands>0){float transfer=d.weights.z*.5f;d.weights.z-=transfer;d.weights.y+=transfer;}
            if(state.TinyHands>=config.tinyHandLimit&&board.dangerLevel<2){float transfer=d.weights.x*.2f;d.weights.x-=transfer;d.weights.y+=transfer;}
            d.weights=DifficultyConfig.Normalize(d.weights);
            if(d.weights.x<config.easyFloor){float remaining=1-config.easyFloor;float other=d.weights.y+d.weights.z;d.weights=new Vector3(config.easyFloor,other>0?remaining*d.weights.y/other:remaining,other>0?remaining*d.weights.z/other:0);}
            return d;
        }
    }
}
