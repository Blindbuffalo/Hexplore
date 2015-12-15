using System.Collections.Generic;
using Gamelogic.Grids;
using UnityEngine;

public class HexLinesGrid : GridBehaviour<PointyHexPoint>
{
	private PointyHexPoint startNode;
    private PointyHexPoint endNode;
    
    private PointyHexPoint Scaler;


	public override void InitGrid()
	{
        Debug.Log("InitGird");
        int Radius = 4;


        startNode = PointyHexPoint.Zero;

        ClearGrid();


	}
    public void ClearGrid()
    {
        foreach (var point in Grid)
        {
            Grid[point].Color = Color.black;
        }
    }
    public List<PointyHexPoint> DrawOrbit(PointyHexPoint Center, int Radius)
    {
        Scaler = new PointyHexPoint(PointyHexPoint.SouthWest.X * Radius, PointyHexPoint.SouthWest.Y * Radius);

        PointyHexPoint CurrentHex = Center + Scaler;

        
        List<PointyHexPoint> Hexes = new List<PointyHexPoint>();
        foreach (PointyHexPoint Dir in PointyHexPoint.MainDirections)
        {
            //Debug.Log(Dir.ToString());
            for (int j = 0; j < Radius; j++)
            {
                Hexes.Add(CurrentHex);
                CurrentHex = CurrentHex + Dir;
                //Debug.Log(CurrentHex.X + " " + CurrentHex.Y);

            }
        }

        Grid[Center].Color = Color.yellow;
        foreach (PointyHexPoint Hex in Hexes)
        {
            Grid[Hex].Color = Color.red;
        }

        return Hexes;
    }
    void update()
    {

    }
    public void OnLeftClick(PointyHexPoint clickedPoint)
    {
        DrawOrbit(startNode, 4);
    }

    public void OnRightClick(PointyHexPoint clickedPoint)
    {
        ClearGrid();
    }

}
