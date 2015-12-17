using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class SolarSystem  {
    public Color OrbitColor = new Color(.05f, .05f, .05f, 1f);
    public SolarSystem(string name, Hex sun, int sunradius, List<Planet> planets)
    {
        Name = name;
        Sun = sun;
        SunRadius = sunradius;

        Planets = planets;
    }

    public string Name { get; set; }
    public Hex Sun { get; set; }
    public List<Planet> Planets { get; set; }
    public int SunRadius { get; set; }
}
