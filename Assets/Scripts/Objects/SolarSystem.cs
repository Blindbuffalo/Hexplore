using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class SolarSystem  {

    private static SolarSystem instance;

    private SolarSystem() { }

    public static SolarSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SolarSystem();
            }
            return instance;
        }
    }
    public Color OrbitColor = new Color(.05f, .05f, .05f, 1f);
    public SolarSystem(string name, Hex sun, int sunradius, Dictionary<string, Planet> planets)
    {
        if (instance == null)
        {
            instance = new SolarSystem();
        }
        instance.Name = name;
        instance.Sun = sun;
        instance.SunRadius = sunradius;

        instance.Planets = planets;
    }

    public string Name { get; set; }
    public Hex Sun { get; set; }
    //public List<Planet> Planets { get; set; }
    public Dictionary<string, Planet> Planets { get; set; }
    public int SunRadius { get; set; }
}
