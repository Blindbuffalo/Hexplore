using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
//using UnityEditor;
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
        //Debug.Log("Galaxy Controller awake()");
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
        //Debug.Log("Galaxy Controller start()");



        Dictionary<string, Ship> Ships = new Dictionary<string, Ship>();
        if (Galaxy == null)
        {
            Galaxy = xmlio.ReadXmlFile();


            BlockedHexes.Instance.HexData = new List<Hex>();
            Hex Sun = new Hex(0, 0, 0);
            BlockedHexes.Instance.HexData = (Hex.Neighbors(Sun));
            BlockedHexes.Instance.HexData.Add(Sun);
            if(Galaxy != null)
            {
                Ship ship = new Ship("Intrepid", 3, new Hex(-1, 6, -5), 500f, 500f);
                Ships.Add(ship.Name, ship);
                Galaxy[0].Ships = Ships;
            }
        }

        if(empire == null)
        {
            empire = new Empire("The Buffalo Empire", "Buffalo", 1, 1);
        }



        NextTurnController.Instance.RegisterGalaxysNextTurnData(GenerateGalaxysNextTurnData);

       

    }
	
   
	// Update is called once per frame
	void Update () {

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



    }
    void OnDestroy()
    {
        //Debug.Log("Galaxy controller destroy()");
        NextTurnController.Instance.UnregisterGalaxysNextTurnData(GenerateGalaxysNextTurnData);
    }
    public bool GenerateGalaxysNextTurnData()
    {
        //Debug.Log("Generate Galaxy Next turn data");

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

    public List<SolarSystem> GetAllSolarsystems()
    {
        return Galaxy.Select(x => x.Value).ToList<SolarSystem>();
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
