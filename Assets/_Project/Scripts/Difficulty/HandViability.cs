using System.Diagnostics;
using BlockBlast.Data;
using BlockBlast.Grid;
namespace BlockBlast.Difficulty
{
    public struct PlacementStep { public int piece,row,column; }
    public sealed class ViabilityResult
    {
        public bool complete,exhausted;
        public int depth,nodes;
        public PlacementStep[] path;
    }
    // Bounded DFS checks all piece orders and legal positions; a timeout is UNKNOWN, not impossible.
    public sealed class HandViability
    {
        BoardModel[] boards;
        readonly PlacementStep[] path=new PlacementStep[3];
        ShapeRuntimeData[] hand;
        ViabilityResult result;
        int limit;
        Stopwatch clock;
        double deadline;
        public ViabilityResult Check(BoardModel board,ShapeRuntimeData[] selected,int nodeLimit,Stopwatch timer,double deadlineMilliseconds)
        {
            if(boards==null||boards[0].Size!=board.Size){boards=new BoardModel[4];for(int i=0;i<4;i++)boards[i]=new BoardModel(board.Size);}
            boards[0].CopyFrom(board);hand=selected;limit=nodeLimit;clock=timer;deadline=deadlineMilliseconds;
            result=new ViabilityResult{path=new PlacementStep[3]};
            result.complete=Search(0,0);return result;
        }
        bool Search(int depth,int used)
        {
            if(depth>result.depth){result.depth=depth;System.Array.Copy(path,result.path,depth);}
            if(depth==3)return true;
            for(int i=0;i<3;i++)
            {
                if((used&(1<<i))!=0)continue;
                var matrix=hand[i].GetMatrix();var board=boards[depth];
                // Clear-producing moves first. Both passes remain bounded by node/time budgets.
                for(int pass=0;pass<2;pass++)for(int r=0;r<=board.Size-hand[i].rows;r++)for(int c=0;c<=board.Size-hand[i].columns;c++)
                {
                    if(++result.nodes>limit||clock.Elapsed.TotalMilliseconds>=deadline){result.exhausted=true;return false;}
                    if(!board.CanPlace(matrix,r,c))continue;
                    int lines=board.CompletedBy(matrix,r,c);
                    if((pass==0)!=(lines>0))continue;
                    boards[depth+1].CopyFrom(board);boards[depth+1].PlaceAndClear(matrix,r,c);
                    path[depth]=new PlacementStep{piece=i,row=r,column=c};
                    if(Search(depth+1,used|(1<<i)))return true;
                    if(result.exhausted)return false;
                }
            }
            return false;
        }
    }
}
