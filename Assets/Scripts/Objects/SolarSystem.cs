using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class SolarSystem  {

    public Color OrbitColor = new Color(.05f, .05f, .05f, 1f);
    public SolarSystem(string name, Hex sun, int sunradius, Dictionary<string, Planet> planets, Dictionary<string, Ship> ships)
    {
        Name = name;
        Sun = sun;
        SunRadius = sunradius;

        Planets = planets;
        Ships = ships;
    }

    public string Name { get; set; }

    public Hex Sun { get; set; }
    public int SunRadius { get; set; }

    public Dictionary<string, Planet> Planets { get; set; }
    
    public Dictionary<string, Ship> Ships { get; set; }
}
