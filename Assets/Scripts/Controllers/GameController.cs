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

    private GameObject SunGO;


    // Use this for initialization
    void Start()
    {
        //HexG = this.transform.GetComponent<DrawHexGraphics>();
        Debug.Log("start");

        GridData.Instance.GenerateGridData(GridRadius);

        Hexes = GridData.Instance.HexData;
        DrawHexGraphics.Instance.GenerateGraphics(Hexes, Prefab);

        Hex Sun = new Hex(0, 0, 0);
        List<Planet> Planets = new List<Planet>();
        
        Planets.Add(new Planet("Mercury", 2, Sun, 1, Color.cyan, 9, .5f, DrawHexGraphics.Instance.PlanetMoved));
        Planets.Add(new Planet("Venus", 4, Sun, 1, Color.green, 1, .5f, DrawHexGraphics.Instance.PlanetMoved));
        Planets.Add(new Planet("Earth", 6, Sun, 1, Color.blue, 1, .6f, DrawHexGraphics.Instance.PlanetMoved));
        Planets.Add(new Planet("Mars", 9, Sun, 1, Color.red, 1, .55f, DrawHexGraphics.Instance.PlanetMoved));
        Planets.Add(new Planet("Jupitor", 31, Sun, 2, Color.magenta, 1, 1.8f, DrawHexGraphics.Instance.PlanetMoved));
        Planets.Add(new Planet("Saturn", 57, Sun, 3, Utilites.Instance.RGBcolor(176, 159, 114, 255), 55, 1.5f, DrawHexGraphics.Instance.PlanetMoved, rings: new Rings(1f, Utilites.Instance.RGBcolor(159, 183, 195, 115))));
        Planets.Add(new Planet("Uranus", 85, Sun, 3, Color.magenta, 250, 1.1f, DrawHexGraphics.Instance.PlanetMoved));
        Planets.Add(new Planet("Neptune", 110, Sun, 3, Color.magenta, 119, 1.1f, DrawHexGraphics.Instance.PlanetMoved));
        Planets.Add(new Planet("Pluto", 145, Sun, 2, Color.magenta, 350, .2f, DrawHexGraphics.Instance.PlanetMoved));

        BlockedHexes.Instance.HexData = Hex.Neighbors(Sun);
        BlockedHexes.Instance.HexData.Add(Sun);




        new SolarSystem("Sol", Sun, 2, Planets);

        SunGO = Utilites.Instance.PlacePrefab(Prefab, SolarSystem.Instance.SunRadius, new Vector3(SolarSystem.Instance.Sun.q, SolarSystem.Instance.Sun.r, SolarSystem.Instance.Sun.s), Color.yellow);

        foreach (Planet P in SolarSystem.Instance.Planets)
        {

            DrawHexGraphics.Instance.DrawOrbit(P, SolarSystem.Instance.OrbitColor);
            DrawHexGraphics.Instance.DrawPlanetHex(P, Prefab, SunGO);
            DrawHexGraphics.Instance.DrawPlanetObject(P, PlanetPreFab, SunGO, Rings);
        }

        MissionController.Instance.InitMissions();
        MissionController.Instance.StartCurrentMainMission();

        CharController.Instance.UpdateUI = UpdateUI;
        CharController.Instance.MovePlanetHex = MovePlanetHex;

        UpdateUI();
    }
    private void UpdateUI()
    {

        Daystext.text = CurrentTurn.ToString() + " " + days;
        XPtext.text = "XP: " + CharController.Instance.XP.ToString();
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

        foreach (Planet P in SolarSystem.Instance.Planets)
        {
            P.MovePlanet();
            DrawHexGraphics.Instance.MovePlanetHex(P, SunGO, P.CurrentPosition);
            DrawHexGraphics.Instance.MovePlanetObject(P, SunGO);
            
            
        }

        MissionController.Instance.MainMissionProgress();
        


        //MissionController.Instance.MissionP = MissionController.MissionProgress.progressing;
        //MissionController.Instance.DeliveryMissionProgress(MissionController.Instance.MainStoryMissions[0]);
        //MissionController.Instance.SpawnMissions(SolarSystem.Instance, SunGO);


    }
    public void MovePlanetHex(int NumMoves)
    {
        foreach (Planet P in SolarSystem.Instance.Planets)
        {
           
            DrawHexGraphics.Instance.MovePlanetHex(P, SunGO, P.PredictPlanetPos(NumMoves));
            
        }
    }
    public void TurnInCurrentMissionGoal()
    {
        MissionController.Instance.TurnInGoals();
    }
}
