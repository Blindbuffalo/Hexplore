using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

public class SolarSystem  {
    public SolarSystem()
    {

    }
    public SolarSystem(string name, Hex sun, int sunradius, Dictionary<string, Planet> planets, Dictionary<string, Ship> ships)
    {
        Name = name;
        Sun = sun;
        SunRadius = sunradius;

        Planets = planets;
        Ships = ships;
    }

    public Color OrbitColor = new Color(.05f, .05f, .05f, 1f);

    //xml attributes
    public int id { get; set; }
    public string Name { get; set; }
    public int SunRadius { get; set; }

    //xml elements
    public Hex Sun { get; set; }
    public Dictionary<string, Planet> Planets { get; set; }

    public Dictionary<string, Ship> Ships { get; set; }
}
