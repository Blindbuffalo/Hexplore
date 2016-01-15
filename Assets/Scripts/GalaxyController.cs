using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Xml;

public class GalaxyController : MonoBehaviour {
    public Empire empire;


    private Dictionary<int, SolarSystem> Galaxy;



    private int CurrentSolarsystem = 0;
    private float Interval = .5f;
    private float CurrentTime = 0;

    public XmlIO xmlio = new XmlIO();

    private Action SolarSystemChanged;


    private GalaxyController() { }
    public static GalaxyController Instance;
    void Awake()
    {
        Debug.Log("Galaxy Controller awake()");
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

        // Use this for initialization
   void Start () {
        Debug.Log("Galaxy Controller start()");

        Galaxy = xmlio.ReadXmlFile();

        if (Galaxy == null)
        {
            //Hex Sun = new Hex(0, 0, 0);
            ////create the galaxy eventually this will load from a save file if one exists

            //Galaxy = new Dictionary<int, SolarSystem>();

            //Dictionary<string, Planet> Planets = new Dictionary<string, Planet>();

            //Planets.Add("Mercury", new Planet(name: "Mercury", orbitRadius: 2, parent: Sun, numberofmoves: 1, color: Color.cyan, position: 4, size: 1f, gravity: 0.05f ));
            //Planets.Add("Venus", new Planet("Venus", 4, Sun, 1, Color.green, 1, 1f, .01f ));
            //Planets.Add("Earth", new Planet("Earth", 6, Sun, 1, Color.blue, 6, 1.5f, 1f ));
            //Planets.Add("Mars", new Planet("Mars", 9, Sun, 1, Color.red, 1, 1.4f, .01f));
            //Planets.Add("Jupiter", new Planet("Jupiter", 31, Sun, 2, Color.magenta, 1, 3f, .01f));
            //Planets.Add("Saturn", new Planet("Saturn", 57, Sun, 3, Utilites.Instance.RGBcolor(176, 159, 114, 255), 1, 3f, .01f));
            ////Planets.Add(5, new Planet("Saturn", 57, Sun, 3, Utilites.Instance.RGBcolor(176, 159, 114, 255), 55, 1.5f, DrawHexGraphics.Instance.PlanetMoved, rings: new Rings(1f, Utilites.Instance.RGBcolor(159, 183, 195, 115))));
            //Planets.Add("Uranus", new Planet("Uranus", 85, Sun, 3, Color.magenta, 1, 1.6f, .01f));
            //Planets.Add("Neptune", new Planet("Neptune", 110, Sun, 3, Color.magenta, 1, 1.6f, .01f));
            //Planets.Add("Pluto", new Planet("Pluto", 145, Sun, 2, Color.magenta, 1, .5f, .01f));

            //Dictionary<string, Ship> Ships = new Dictionary<string, Ship>();
            //Ship ship = new Ship("Intrepid", 4, new Hex(25, 10, -35), 500f, 500f);
            //Ships.Add(ship.Name, ship);
            //SolarSystem Sol = new SolarSystem("sol", Sun, 7, Planets, Ships);

            //Galaxy.Add(0, Sol);

            //Dictionary<string, Planet> Planets2 = new Dictionary<string, Planet>();

            //Planets2.Add("1", new Planet("1", 4, Sun, 1, Color.red, 4, 3f, .01f));
            //Planets2.Add("2", new Planet("2", 8, Sun, 1, Color.green, 1, 2f, .01f));

            //Ships = new Dictionary<string, Ship>();
            //SolarSystem Sol2 = new SolarSystem("sol2", Sun, 3, Planets2, Ships);

            //Galaxy.Add(1, Sol2);

            

            //BlockedHexes.Instance.HexData.Add(new Hex(0, 0, 0));
        }
        if(empire == null)
        {
            empire = new Empire("The Buffalo Empire", "Buffalo", 1, 1);
        }
        NextTurnController.Instance.RegisterGalaxysNextTurnData(GenerateGalaxysNextTurnData);

       

    }
	
   
	// Update is called once per frame
	void Update () {



        //Cycle all data for the next turn
        //FIX: time intervals are temporary until i work our an actual next turn system
        //CurrentTime += Time.deltaTime;
        //if (CurrentTime >= Interval)
        //{
        //    //move all the planets into their "next turn" position in the data
        //    //this does not directly move the gameobjects (visuals)
        //    foreach(KeyValuePair<int, SolarSystem> Sol in Galaxy)
        //    {
        //        foreach (KeyValuePair<string, Planet> p in Sol.Value.Planets)
        //        {
        //            p.Value.MovePlanet();

        //        }
        //    }
            
        //    CurrentTime = 0f;
        //}


        //check to see what scene we are in.
        //eventually this will be triggered by things like deploying on a planet
        //or maybe going onto the ships bridge.  this section calls the drawing 
        //system to render the correct visuals
        Scene s = SceneManager.GetActiveScene();
        if (s.buildIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Ship ship = new Ship("Intrepid", 4, new Hex(0, 0, 0), 500f, 500f);
                AddShipToSolarSystem(CurrentSolarsystem, ship);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (CurrentSolarsystem == 0)
                {
                    CurrentSolarsystem = 1;
                }
                else
                {
                    CurrentSolarsystem = 0;
                }
                SolarSystemChanged();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                SceneManager.LoadScene(1);
            }
            
        }


        if (Input.GetKeyDown(KeyCode.F3))
        {
            xmlio.WriteXmlFile(Galaxy);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
           Galaxy = xmlio.ReadXmlFile();
        }

    }
    void OnDestroy()
    {
        Debug.Log("Galaxy controller destroy()");
        NextTurnController.Instance.UnregisterGalaxysNextTurnData(GenerateGalaxysNextTurnData);
    }
    public bool GenerateGalaxysNextTurnData()
    {
        Debug.Log("Generate Galaxy Next turn data");

        foreach (KeyValuePair<int, SolarSystem> Sol in Galaxy)
        {
            foreach (KeyValuePair<string, Planet> p in Sol.Value.Planets)
            {
                p.Value.MovePlanet();

            }
        }

        return true;
    }

    public void AddShipToSolarSystem(int SystemID, Ship ship)
    {
        Galaxy[SystemID].Ships.Add(ship.Name, ship);
    }


    public SolarSystem GetSolarSystem(int SystemID)
    {
        return Galaxy[SystemID];
    }
    public SolarSystem GetCurrentSolarSystem()
    {
        return GetSolarSystem(CurrentSolarsystem);
    }



    public void RegisterSolarSystemChanged(Action func)
    {
        SolarSystemChanged += func;
    }
    public void UnregisterSolarSystemChanged(Action func)
    {
        SolarSystemChanged += func;
    }
}
