using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class GameController : MonoBehaviour {
    public int CurrentTurn = 0;
    public int days = 0;
    public int SolarSystemDayMultiplier = 10;
    public Text text;

    public List<Hex> Hexes = null;
    public int t = 0;

    public GameObject Prefab;
    public GameObject PlanetPreFab;
    public GameObject Rings;

    public int GridRadius = 20;

    public GridData Grid = new GridData();
    public Utilites Utility;

    public DrawHexGraphics HexG;

    public SolarSystem Sol;
    public float g = 0f;

    public CharController charController;

    private GameObject SunGO;

    public MissionController MissionController;

    // Use this for initialization
    void Start()
    {
        //HexG = this.transform.GetComponent<DrawHexGraphics>();
        Debug.Log("start");

        Grid.GenerateGridData(GridRadius);

        Hexes = Grid.HexData;
        HexG.GenerateGraphics(Hexes, Prefab);

        Hex Sun = new Hex(0, 0, 0);
        List<Planet> Planets = new List<Planet>();
        
        Planets.Add(new Planet("Mercury", 2, Sun, 1, Color.cyan, 1, .5f, PlanetMoved));
        Planets.Add(new Planet("Venus", 4, Sun, 1, Color.green, 1, .5f, PlanetMoved));
        Planets.Add(new Planet("Earth", 6, Sun, 1, Color.blue, 1, .6f, PlanetMoved));
        Planets.Add(new Planet("Mars", 9, Sun, 1, Color.red, 1, .55f, PlanetMoved));
        Planets.Add(new Planet("Jupitor", 31, Sun, 2, Color.magenta, 1, 1.8f, PlanetMoved));
        Planets.Add(new Planet("Saturn", 57, Sun, 3, Utility.RGBcolor(176, 159, 114, 255), 55, 1.5f, PlanetMoved, rings: new Rings(1f, Utility.RGBcolor(159, 183, 195, 115))));
        Planets.Add(new Planet("Uranus", 85, Sun, 3, Color.magenta, 250, 1.1f, PlanetMoved));
        Planets.Add(new Planet("Neptune", 110, Sun, 3, Color.magenta, 119, 1.1f, PlanetMoved));
        Planets.Add(new Planet("Pluto", 145, Sun, 2, Color.magenta, 350, .2f, PlanetMoved));

        Sol = new SolarSystem("Sol", Sun, 2, Planets);

        SunGO = Utility.PlacePrefab(Prefab, Sol.SunRadius, new Vector3(Sol.Sun.q, Sol.Sun.r, Sol.Sun.s), Color.yellow);

        foreach (Planet P in Sol.Planets)
        {

            HexG.DrawOrbit(P, Sol.OrbitColor);
            HexG.DrawPlanetHex(P, Sol.OrbitColor);
            HexG.DrawPlanetObject(P, PlanetPreFab, SunGO, Rings);
        }

        MissionController.InitMissions();


    }
    void Update()
    {
        if (Input.GetKeyDown("space"))
            NextTurnButton_Click();
        if (Input.GetKeyDown("backspace"))
            MissionController.MSMissionStep++;

        //MissionController.CheckMissionStatus(Sol);

    }
    public void NextTurnButton_Click()
    {
        CurrentTurn++;
        
        days = SolarSystemDayMultiplier * CurrentTurn;
        text.text = CurrentTurn.ToString() + " " + days;

        charController.MainShip.MovesLeft = charController.MainShip.Movement;

        foreach (Planet P in Sol.Planets)
        {
            HexG.DrawPlanetHex(P, Sol.OrbitColor);
            HexG.MovePlanetObject(P, SunGO);
        }


        MissionController.SpawnMissions(Sol, SunGO);


    }
    public void PlanetMoved(Planet P)
    {

    }
}
