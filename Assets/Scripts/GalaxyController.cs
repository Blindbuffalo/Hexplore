using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEditor;

public class GalaxyController : MonoBehaviour {
    private Dictionary<int, SolarSystem> Galaxy;

    private int CurrentSolarsystem = 0;
    private float Interval = .5f;
    private float CurrentTime = 0;

    private Action<SolarSystem> OnNextTurnCB;


    private GalaxyController() { }
    private static GalaxyController instance;


    public static GalaxyController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (GalaxyController)FindObjectOfType(typeof(GalaxyController));
                if (instance == null)
                    instance = (new GameObject("GalaxyController")).AddComponent<GalaxyController>();
            }
            return instance;
        }
    }


    // Use this for initialization
    void Start () {
        if(Galaxy == null)
        {
            Hex Sun = new Hex(0, 0, 0);
            //create the galaxy eventually this will load from a save file if one exists

            Galaxy = new Dictionary<int, SolarSystem>();

            Dictionary<string, Planet> Planets = new Dictionary<string, Planet>();

            Planets.Add("Mercury", new Planet("Mercury", 2, Sun, 1, Color.cyan, 4, 1f ));
            Planets.Add("Venus", new Planet("Venus", 4, Sun, 1, Color.green, 1, 1f ));
            Planets.Add("Earth", new Planet("Earth", 6, Sun, 1, Color.blue, 1, 1.5f ));
            Planets.Add("Mars", new Planet("Mars", 9, Sun, 1, Color.red, 1, 1.4f ));
            Planets.Add("Jupiter", new Planet("Jupiter", 31, Sun, 2, Color.magenta, 1, 3f ));
            Planets.Add("Saturn", new Planet("Saturn", 57, Sun, 3, Utilites.Instance.RGBcolor(176, 159, 114, 255), 1, 3f ));
            //Planets.Add(5, new Planet("Saturn", 57, Sun, 3, Utilites.Instance.RGBcolor(176, 159, 114, 255), 55, 1.5f, DrawHexGraphics.Instance.PlanetMoved, rings: new Rings(1f, Utilites.Instance.RGBcolor(159, 183, 195, 115))));
            Planets.Add("Uranus", new Planet("Uranus", 85, Sun, 3, Color.magenta, 1, 1.6f ));
            Planets.Add("Neptune", new Planet("Neptune", 110, Sun, 3, Color.magenta, 1, 1.6f ));
            Planets.Add("Pluto", new Planet("Pluto", 145, Sun, 2, Color.magenta, 1, .5f ));


            SolarSystem Sol = new SolarSystem("sol", Sun, 5, Planets);

            Galaxy.Add(0, Sol);
            
        }
        DrawGraphics.Instance.DrawSolarSystem(Galaxy[CurrentSolarsystem]);


    }
	
	// Update is called once per frame
	void Update () {
        CurrentTime += Time.deltaTime;
        if (CurrentTime >= Interval)
        {
            foreach (KeyValuePair<string, Planet> p in Galaxy[0].Planets)
            {
                p.Value.MovePlanet();

            }
            CurrentTime = 0f;
        }



        OnNextTurnCB(Galaxy[0]);
    }

    public void RegisterOnNextTurn(Action<SolarSystem> func)
    {
        OnNextTurnCB += func;
    }
    public void UnregisterOnNextTurn(Action<SolarSystem> func)
    {
        OnNextTurnCB -= func;
    }
}
