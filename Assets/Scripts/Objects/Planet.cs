using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planet  {

    public Planet(string name, int orbitRadius, Hex parent, int numberofmoves, Color color, int position)
    {
        Name = name;
        OrbitRadius = orbitRadius;
        Parent = parent;
        Orbit= CalcOrbit();

        NumberOfMoves = numberofmoves;
        Col = color;

        LastPosition = position;
        CurrentPosition = position;
    }

    public string Name { get; private set; }
    public int OrbitRadius { get; private set; }
    public Hex Parent { get; private set; }

    public List<Hex> Orbit { get; private set; }

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
