using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Rings
{
    public Rings(float ringscale, Color ringcolor)
    {
        RingsScale = ringscale;
        RingColor = ringcolor;
    }
    public float RingsScale { get; private set; }
    public Color RingColor { get; private set; }
}
public class Planet {

    public Planet(string name, int orbitRadius, Hex parent, int numberofmoves, Color color, int position, float size, Rings rings = null)
    {
        Name = name;
        OrbitRadius = orbitRadius;
        Parent = parent;
        Orbit = CalcOrbit();
    
        Size = size;

        Rings = rings;

        NumberOfMoves = numberofmoves;
        Col = color;

        LastPosition = position;
        CurrentPosition = position;
    }

    public string Name { get; private set; }
    public int OrbitRadius { get; private set; }
    public Hex Parent { get; private set; }

    public List<Hex> Orbit { get; private set; }

    public float Size { get; private set; }

    public Rings Rings {get; set;}

    public Color Col { get; set; }
    public int NumberOfMoves { get; set; }

    public int CurrentPosition { get; set; }
    public int LastPosition { get; set; }
    
    private List<Hex> CalcOrbit()
    {

        Hex Scale = Hex.Scale(Hex.directions[4], OrbitRadius);
        Hex CurrentHex = Hex.Add(Parent, Scale);

        List<Hex> Hexes = new List<Hex>();
        foreach (Hex Dir in Hex.directions)
        {
            //Debug.Log(Dir.ToString());
            for (int j = 0; j < OrbitRadius; j++)
            {
                Hexes.Add(CurrentHex);
                CurrentHex = Hex.Add(CurrentHex, Dir);
                //Debug.Log(CurrentHex.X + " " + CurrentHex.Y);

            }
        }

        return Hexes;
    }
}
