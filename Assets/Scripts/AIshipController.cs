using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIshipController : MonoBehaviour {
    private Ship ShipData;

    bool run = false;

    private AIshipController() { }
    public static AIshipController Instance;
    void Awake()
    {
        Debug.Log("AIshipController awake()");
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
        NextTurnController.Instance.RegisterAIshipsNextTurnData(GenerateAIshipsNextTurnData);
    }
    void OnDestroy()
    {
        Debug.Log("Galaxy controller destroy()");
        NextTurnController.Instance.UnregisterAIshipsNextTurnData(GenerateAIshipsNextTurnData);
    }
    // Update is called once per frame
    void Update () {
        if(Input.GetKeyDown(KeyCode.F2))
        {
            Planet p = GalaxyController.Instance.GetSolarSystem(0).Planets["Earth"];
            Ship s = GalaxyController.Instance.GetSolarSystem(0).Ships["Intrepid"];

            List<Hex> Hexs = RendevousWithOrbitingObject(p, s);
            if (Hexs == null)
            {
                run = true;
                return;
            }
            DrawSolarSystemGraphics.Instance.DrawHex(Hexs[0], "tttt", Color.red, offset: -1);
            DrawSolarSystemGraphics.Instance.DrawHex(s.CurrentHexPosition, "tttt", Color.red, offset: -1);
            //foreach (Hex h in p.Orbit)
            //{
            //    DrawSolarSystemGraphics.Instance.DrawHex(h, Utilites.Instance.HexNameStr(h), Color.blue, offset: 1);
            //}
            //foreach (Hex h in Hexs)
            //{
            //    //  Debug.Log(Utilites.Instance.HexNameStr(h));
            //    DrawSolarSystemGraphics.Instance.DrawHex(h, Utilites.Instance.HexNameStr(h), Color.red);
            //}
            run = true;
        }




	}
    public bool GenerateAIshipsNextTurnData()
    {
        SolarSystem Sol = GalaxyController.Instance.GetCurrentSolarSystem();
        Dictionary<string, Ship> Ships = Sol.Ships;

        foreach (KeyValuePair<string, Ship> ShipKV in Ships) { }
        {

        }

        return true;
    }
    public int NumberOfTurnsToRendevous(OrbitalObject OO, Ship ship)
    {
        Hex TargetsCurrentHex = OO.Orbit[OO.CurrentPosition];

        Hex ShipsCurrentHex = ship.CurrentHexPosition;

        int distance = Hex.Distance(ShipsCurrentHex, TargetsCurrentHex);

        int InitialDist = distance;

        int Intturns = (int)Mathf.Ceil(distance / ship.Movement);

        if (Intturns == 1)
        {
            //the number of moves the ship has left is enough to make it to the target this turn
            //just return the current hex of the target > this will be used as the ships target of movement
            return Intturns;
        }

        for (int i = 0; i < InitialDist; i++)
        {
            distance = Hex.Distance(ShipsCurrentHex, TargetsCurrentHex);

            Intturns = (int)Mathf.Ceil(distance / ship.Movement);

            if(Intturns == i)
            {
                return Intturns;
            }

        }

        return -1;
    }
    public List<Hex> RendevousWithOrbitingObject(OrbitalObject OO, Ship ship)
    {
        Hex TargetsPredictedHex;

        Hex ShipsCurrentHex = ship.CurrentHexPosition;

        int NumTurnsToRendevous = NumberOfTurnsToRendevous(OO, ship);

        TargetsPredictedHex = OO.Orbit[OO.PredictPlanetPos(NumTurnsToRendevous)];
        List<Hex> Path = Hex.AstarPath(TargetsPredictedHex, ShipsCurrentHex);
        return Path;
    }
}
