using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class GalaxyController : MonoBehaviour {
    private Dictionary<int, SolarSystem> Galaxy;

    private int CurrentSolarsystem = 0;
    public GameObject PlanetPrefab;
    public GameObject HexPrefab;

    // Use this for initialization
    void Start () {
        if(Galaxy == null)
        {
            Hex Sun = new Hex(0, 0, 0);
            //create the galaxy eventually this will load from a save file if one exists

            Galaxy = new Dictionary<int, SolarSystem>();

            Dictionary<int, Planet> Planets = new Dictionary<int, Planet>();

            Planets.Add(0, new Planet("Mercury", 2, Sun, 1, Color.cyan, 9, 1f, DrawHexGraphics.Instance.PlanetMoved));
            Planets.Add(1, new Planet("Venus", 4, Sun, 1, Color.green, 1, 1f, DrawHexGraphics.Instance.PlanetMoved));
            Planets.Add(2, new Planet("Earth", 6, Sun, 1, Color.blue, 1, 1.5f, DrawHexGraphics.Instance.PlanetMoved));
            Planets.Add(3, new Planet("Mars", 9, Sun, 1, Color.red, 1, 1.4f, DrawHexGraphics.Instance.PlanetMoved));
            Planets.Add(4, new Planet("Jupitor", 31, Sun, 2, Color.magenta, 1, 3f, DrawHexGraphics.Instance.PlanetMoved));
            Planets.Add(5, new Planet("Saturn", 57, Sun, 3, Utilites.Instance.RGBcolor(176, 159, 114, 255), 55, 3f, DrawHexGraphics.Instance.PlanetMoved));
            //Planets.Add(5, new Planet("Saturn", 57, Sun, 3, Utilites.Instance.RGBcolor(176, 159, 114, 255), 55, 1.5f, DrawHexGraphics.Instance.PlanetMoved, rings: new Rings(1f, Utilites.Instance.RGBcolor(159, 183, 195, 115))));
            Planets.Add(6, new Planet("Uranus", 85, Sun, 3, Color.magenta, 250, 1.6f, DrawHexGraphics.Instance.PlanetMoved));
            Planets.Add(7, new Planet("Neptune", 110, Sun, 3, Color.magenta, 119, 1.6f, DrawHexGraphics.Instance.PlanetMoved));
            Planets.Add(8, new Planet("Pluto", 145, Sun, 2, Color.magenta, 350, .5f, DrawHexGraphics.Instance.PlanetMoved));


            SolarSystem Sol = new SolarSystem("sol", Sun, 2, Planets);

            Galaxy.Add(0, Sol);
            
        }
        foreach (KeyValuePair<int, Planet> planet in Galaxy[CurrentSolarsystem].Planets)
        {
            DrawHexGraphics.Instance.DrawPlanetObject(planet.Value, PlanetPrefab, this.gameObject, null);
            DrawHexGraphics.Instance.DrawPlanetHex(planet.Value, HexPrefab, this.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
