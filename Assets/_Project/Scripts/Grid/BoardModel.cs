using System.Collections.Generic;using UnityEngine;
namespace BlockBlast.Grid {
public sealed class BoardModel {
 readonly bool[,] occupied; readonly bool[] clearRows,clearCols; public int Size {get;}
 public BoardModel(int size){Size=size;occupied=new bool[size,size];clearRows=new bool[size];clearCols=new bool[size];}
 public bool Occupied(int r,int c)=>occupied[r,c];
 public void Set(int r,int c,bool value)=>occupied[r,c]=value;
 // Simulation helpers operate on independent copies, with no gameplay events or score.
 public BoardModel Clone(){var copy=new BoardModel(Size);copy.CopyFrom(this);return copy;}
 public void CopyFrom(BoardModel source){if(source.Size!=Size)throw new System.ArgumentException("Board size mismatch");System.Array.Copy(source.occupied,occupied,occupied.Length);}
 public int PlaceAndClear(bool[,] shape,int row,int col)
 {
  if(!CanPlace(shape,row,col))return -1;
  for(int r=0;r<shape.GetLength(0);r++)for(int c=0;c<shape.GetLength(1);c++)if(shape[r,c])occupied[row+r,col+c]=true;
  // Mark both axes before clearing intersections, matching GridBoard.ProcessCompletedLines.
  int count=0;System.Array.Clear(clearRows,0,Size);System.Array.Clear(clearCols,0,Size);
  for(int r=0;r<Size;r++){bool full=true;for(int c=0;c<Size;c++)full&=occupied[r,c];if(full){clearRows[r]=true;count++;}}
  for(int c=0;c<Size;c++){bool full=true;for(int r=0;r<Size;r++)full&=occupied[r,c];if(full){clearCols[c]=true;count++;}}
  for(int r=0;r<Size;r++)for(int c=0;c<Size;c++)if(clearRows[r]||clearCols[c])occupied[r,c]=false;
  return count;
 }
 public bool CanPlace(bool[,] shape,int row,int col){bool any=false;for(int r=0;r<shape.GetLength(0);r++)for(int c=0;c<shape.GetLength(1);c++)if(shape[r,c]){any=true;int y=row+r,x=col+c;if(y<0||x<0||y>=Size||x>=Size||occupied[y,x])return false;}return any;}
 public int CompletedBy(bool[,] shape,int row,int col){if(!CanPlace(shape,row,col))return -1;int count=0;
 for(int r=0;r<Size;r++){bool full=true,touched=false;for(int c=0;c<Size;c++){bool add=r>=row&&r<row+shape.GetLength(0)&&c>=col&&c<col+shape.GetLength(1)&&shape[r-row,c-col];full&=occupied[r,c]||add;touched|=add;}if(full&&touched)count++;}
 for(int c=0;c<Size;c++){bool full=true,touched=false;for(int r=0;r<Size;r++){bool add=r>=row&&r<row+shape.GetLength(0)&&c>=col&&c<col+shape.GetLength(1)&&shape[r-row,c-col];full&=occupied[r,c]||add;touched|=add;}if(full&&touched)count++;}return count;}
}}
