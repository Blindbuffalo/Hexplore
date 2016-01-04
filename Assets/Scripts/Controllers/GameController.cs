using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
  
    public int CurrentTurn = 0;
    public int days = 0;
    public int SolarSystemDayMultiplier = 10;
    public Text Daystext;
    public Text XPtext;

    public List<Hex> Hexes = null;
    public int t = 0;

    public GameObject Prefab;
    public GameObject PlanetPreFab;
    public GameObject Rings;

    public int GridRadius = 150;

    public float g = 0f;

    public int XP { get; protected set; }
    private int XPtoNextLevel = 100; //need to make this better! money instead of xp?

    public GameObject SunGO;
    private static GameController instance;



    private GameController() { }

    public static GameController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (GameController)FindObjectOfType(typeof(GameController));
                if (instance == null)
                    instance = (new GameObject("GameController")).AddComponent<GameController>();
            }
            return instance;
        }
    }

    // Use this for initialization
    void Start()
    {
        //HexG = this.transform.GetComponent<DrawHexGraphics>();
        Debug.Log("start");

        GridData.Instance.GenerateGridData(GridRadius);

        Hexes = GridData.Instance.HexData;
        DrawHexGraphics.Instance.GenerateGraphics(Hexes, Prefab);

        Hex Sun = new Hex(0, 0, 0);
        //List<Planet> Planets = new List<Planet>();
        Dictionary<string, Planet> Planets = new Dictionary<string, Planet>();
        Planets.Add("Mercury", new Planet("Mercury", 2, Sun, 1, Color.cyan, 9, .5f, DrawHexGraphics.Instance.PlanetMoved));
        Planets.Add("Venus", new Planet("Venus", 4, Sun, 1, Color.green, 1, .5f, DrawHexGraphics.Instance.PlanetMoved));
        Planets.Add("Earth", new Planet("Earth", 6, Sun, 1, Color.blue, 1, .6f, DrawHexGraphics.Instance.PlanetMoved));
        Planets.Add("Mars", new Planet("Mars", 9, Sun, 1, Color.red, 1, .55f, DrawHexGraphics.Instance.PlanetMoved));
        Planets.Add("Jupitor", new Planet("Jupitor", 31, Sun, 2, Color.magenta, 1, 1.8f, DrawHexGraphics.Instance.PlanetMoved));
        Planets.Add("Saturn", new Planet("Saturn", 57, Sun, 3, Utilites.Instance.RGBcolor(176, 159, 114, 255), 55, 1.5f, DrawHexGraphics.Instance.PlanetMoved, rings: new Rings(1f, Utilites.Instance.RGBcolor(159, 183, 195, 115))));
        Planets.Add("Uranus", new Planet("Uranus", 85, Sun, 3, Color.magenta, 250, 1.1f, DrawHexGraphics.Instance.PlanetMoved));
        Planets.Add("Neptune", new Planet("Neptune", 110, Sun, 3, Color.magenta, 119, 1.1f, DrawHexGraphics.Instance.PlanetMoved));
        Planets.Add("Pluto", new Planet("Pluto", 145, Sun, 2, Color.magenta, 350, .2f, DrawHexGraphics.Instance.PlanetMoved));

        BlockedHexes.Instance.HexData = Hex.Neighbors(Sun);
        BlockedHexes.Instance.HexData.Add(Sun);




        new SolarSystem("Sol", Sun, 2, Planets);

        SunGO = Utilites.Instance.PlacePrefab(Prefab, SolarSystem.Instance.SunRadius, new Vector3(SolarSystem.Instance.Sun.q, SolarSystem.Instance.Sun.r, SolarSystem.Instance.Sun.s), Color.yellow);

        foreach (KeyValuePair<string, Planet> P in SolarSystem.Instance.Planets)
        {

            DrawHexGraphics.Instance.DrawOrbit(P.Value, SolarSystem.Instance.OrbitColor);
            DrawHexGraphics.Instance.DrawPlanetHex(P.Value, Prefab, SunGO);
            DrawHexGraphics.Instance.DrawPlanetObject(P.Value, PlanetPreFab, SunGO, Rings);
        }

        MissionController.Instance.InitMissions();
        MissionController.Instance.StartCurrentMainMission();

        CharController.Instance.UpdateUI = UpdateUI;
        CharController.Instance.MovePlanetHex = MovePlanetHex;

        UpdateUI();

        MissionController.Instance.DrawMissionIndicators();
    }
    private void UpdateUI()
    {

        Daystext.text = CurrentTurn.ToString() + " " + days;
        XPtext.text = "XP: " + XP.ToString();
    }
    void Update()
    {
        if (Input.GetKeyDown("space"))
            NextTurnButton_Click();


        //MissionController.CheckMissionStatus(Sol);

    }
    public void NextTurnButton_Click()
    {
        CurrentTurn++;
        days = SolarSystemDayMultiplier * CurrentTurn;


        UpdateUI();

        CharController.Instance.MainShip.MovesLeft = CharController.Instance.MainShip.Movement;

        foreach (KeyValuePair<string, Planet> P in SolarSystem.Instance.Planets)
        {
            P.Value.MovePlanet();
            DrawHexGraphics.Instance.MovePlanetHex(P.Value, SunGO, P.Value.CurrentPosition);
            DrawHexGraphics.Instance.MovePlanetObject(P.Value, SunGO);
            
            
        }

        MissionController.Instance.MainMissionProgress();

        MissionController.Instance.DrawMissionIndicators();
    }
    public void MovePlanetHex(int NumMoves)
    {
        foreach (KeyValuePair<string, Planet> P in SolarSystem.Instance.Planets)
        {
           
            DrawHexGraphics.Instance.MovePlanetHex(P.Value, SunGO, P.Value.PredictPlanetPos(NumMoves));
            
        }
    }
    public void TurnInCurrentMissionGoal()
    {
        MissionController.Instance.TurnInGoals();
    }
    public void IncreaseXP(int xp)
    {
        if (XP + xp >= XPtoNextLevel)
        {
            int Remainder = XPtoNextLevel - XP;
            XP = Remainder;
        }
        else
        {
            XP += xp;
        }

        UpdateUI();
    }
}
