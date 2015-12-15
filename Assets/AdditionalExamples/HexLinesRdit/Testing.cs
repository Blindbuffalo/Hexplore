using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamelogic.Grids;
public class Testing : MonoBehaviour {
    public HexLinesGrid t;
    public List<PointyHexPoint> Hexes;
    public int Position = 0;
    public float g = 0f;
    public int PlanetMoveDistance = 3;
    public int OrbitRadius = 6;
    public int NumberOfPositions;
    public int LastPosition = 0;
	// Use this for initialization
	void Start () {
        Debug.Log("StartScript");
        NumberOfPositions = OrbitRadius * 6;
        
	}
	
	// Update is called once per frame
	void Update () {
        if (t == null)
        {
            LastPosition = Position;
             t = this.GetComponent<HexLinesGrid>();
             Hexes = t.DrawOrbit(PointyHexPoint.Zero, OrbitRadius);
             Debug.Log(NumberOfPositions);
        }
        g += Time.deltaTime * 500;
        //Debug.Log(g);
        if (g > 100)
        {
            g = 0;

            Position = Position + PlanetMoveDistance;
            //Debug.Log(Position + " " + LastPosition);
            if (Position >= NumberOfPositions)
            {
                Position = Position - NumberOfPositions;
                
            }
            t.Grid[Hexes[LastPosition]].Color = Color.red;
            t.Grid[Hexes[Position]].Color = Color.blue;
            LastPosition = Position;
        }
	}
}
