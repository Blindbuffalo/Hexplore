using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public enum OrbitDir { CCW, CW }

public class OrbitalObject
{
    public string Name { get; protected set; }
    public int OrbitRadius { get; protected set; }
    public Hex Parent { get; protected set; }

    public List<Hex> Orbit { get; protected set; }

    public float Size { get; protected set; }

    public Color Col { get; set; }
    public int NumberOfMoves { get; set; }

    private int _currentPosition;
    public int CurrentPosition
    {
        get
        {
            return _currentPosition;
        }
        set
        {
            _currentPosition = value;
            //PlanetMoved(this);
        }
    }
    public int LastPosition { get; set; }

    public OrbitDir OrbitDirection { get; set; }
    public int PredictPlanetPos(int Multi)
    {
        int CP = 0;
        if (Multi == 0) { return _currentPosition; }
        if (OrbitDirection == OrbitDir.CCW)
        {
            CP = (_currentPosition) - (NumberOfMoves * Multi);
            while (CP < 0)
            {
                CP = (Orbit.Count) + CP;
            }
        }
        else
        {
            //CW rotation
            CP = (_currentPosition) + (NumberOfMoves * Multi);
            while (CP >= Orbit.Count)
            {
                CP = CP - (Orbit.Count);
            }
        }

        return CP;
    }
    public void MovePlanet()
    {
        CurrentPosition = PredictPlanetPos(1);
    }
    protected List<Hex> CalcOrbit()
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
public class Planet : OrbitalObject {
    public Planet(string name, int orbitRadius, Hex parent, int numberofmoves, Color color, int position, float size, OrbitDir OD = OrbitDir.CCW, Rings rings = null)
    {
        Name = name;
        OrbitRadius = orbitRadius;
        Parent = parent;
        Orbit = CalcOrbit();

        Size = size;

        NumberOfMoves = numberofmoves;
        Col = color;

        LastPosition = position;
        CurrentPosition = position;

        OrbitDirection = OD;

        Rings = rings;
    }
    public Rings Rings {get; set;}
}
public class Astroid : OrbitalObject
{
    public Astroid(string name, int orbitRadius, Hex parent, int numberofmoves, Color color, int position, float size, OrbitDir OD = OrbitDir.CCW)
    {
        Name = name;
        OrbitRadius = orbitRadius;
        Parent = parent;
        Orbit = CalcOrbit();

        Size = size;

        NumberOfMoves = numberofmoves;
        Col = color;

        LastPosition = position;
        CurrentPosition = position;

        OrbitDirection = OD;
    }
}
public class DwarfPlanet : OrbitalObject
{
    public DwarfPlanet(string name, int orbitRadius, Hex parent, int numberofmoves, Color color, int position, float size, OrbitDir OD = OrbitDir.CCW)
    {
        Name = name;
        OrbitRadius = orbitRadius;
        Parent = parent;
        Orbit = CalcOrbit();

        Size = size;

        NumberOfMoves = numberofmoves;
        Col = color;

        LastPosition = position;
        CurrentPosition = position;

        OrbitDirection = OD;
    }
}