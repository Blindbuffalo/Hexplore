using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIshipController : MonoBehaviour {
    private Ship ShipData;

    bool runAstar = false;

    private AIshipController() { }
    public static AIshipController Instance;
    void Awake()
    {
        //Debug.Log("AIshipController awake()");
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

    void Start () {
        NextTurnController.Instance.RegisterAIshipsNextTurnData(GenerateAIshipsNextTurnData);
    }
    void OnDestroy()
    {
        //Debug.Log("Galaxy controller destroy()");
        NextTurnController.Instance.UnregisterAIshipsNextTurnData(GenerateAIshipsNextTurnData);
    }

    void Update () {
        if(Input.GetKeyDown(KeyCode.F2))
        {
            Planet p = GalaxyController.Instance.GetSolarSystem(0).Planets["Saturn"];
            Ship s = GalaxyController.Instance.GetSolarSystem(0).Ships["Intrepid"];

            List<Hex> Hexs = RendezvousWithOrbitingObject(p, s);
            if (Hexs == null)
            {
                runAstar = true;
                return;
            }
            DrawSolarSystemGraphics.Instance.DrawHex(Hexs[0], "tttt", Color.yellow, offset: -2);
            DrawSolarSystemGraphics.Instance.DrawHex(s.CurrentHexPosition, "tttt", Color.yellow, offset: -2);
            foreach (Hex h in p.Orbit)
            {
                DrawSolarSystemGraphics.Instance.DrawHex(h, Utilites.Instance.HexNameStr(h), Color.blue, offset: 1);
            }
            foreach (Hex h in Hexs)
            {
                //  Debug.Log(Utilites.Instance.HexNameStr(h));
                DrawSolarSystemGraphics.Instance.DrawHex(h, Utilites.Instance.HexNameStr(h), Color.green);
            }
            runAstar = true;
        }

        

    }
    public bool GenerateAIshipsNextTurnData()
    {
        //check to see if an AIship needs to be spawned

        if (GalaxyController.Instance.empire.CanSpawnCargoShip())
        {
            //spawn an AI cargo ship
            GalaxyController.Instance.empire.CurrentComercialCargoShips++;
            Ship s = new Ship("Cargo 001", 3, GalaxyController.Instance.GetSolarSystem(0).Planets["Mars"].GetCurrentHexPosition(), 500f, 500f);
            s.PositionOnPath = 0;
            s.PathToTarget = RendezvousWithOrbitingObject(GalaxyController.Instance.GetSolarSystem(0).Planets["Earth"], s);
            s.MovesLeft = 0;
            s.justSpawned = true;
            GalaxyController.Instance.GetCurrentSolarSystem().Ships.Add(s.Name, s);
            
        }

        SolarSystem Sol = GalaxyController.Instance.GetCurrentSolarSystem();
        Dictionary<string, Ship> Ships = Sol.Ships;
        //Debug.Log("Ship next turn: begin");
        if (Sol.Ships == null) return false;
        foreach (KeyValuePair<string, Ship> ShipKV in Ships)
        {
            Ship s = ShipKV.Value;
            s.MovesLeft = s.Movement;

            if (s.justSpawned)
            {
                s.justSpawned = false;
            }
            else
            {
                if (s.PathToTarget != null)
                {

                    
                    Debug.Log("Ship next turn: ship has a path " + s.PositionOnPath);
                    if (s.PositionOnPath >= s.PathToTarget.Count - 1)
                    {
                        s.CurrentHexPosition = s.PathToTarget[s.PathToTarget.Count - 1];
                        s.MovesLeft = s.MovesLeft - (s.PositionOnPath - s.PathToTarget.Count);
                        s.PathToTarget = null;
                        //s.SetTargetHex (s.CurrentHexPosition);
                        
                        Debug.Log("Ship next turn: reached the end");
                    }
                    else
                    {
                        s.MovesLeft = 0;
                        s.CurrentHexPosition = s.PathToTarget[s.PositionOnPath];
                        Debug.Log("Ship next turn: move " + Utilites.Instance.HexNameStr(s.CurrentHexPosition));
                        
                    }
                    s.PositionOnPath += s.Movement;
                }
            }
        }

        //Debug.Log("Ship next turn: end");
        return true;
    }
    public int NumberOfTurnsToRendezvous(OrbitalObject OO, Ship ship)
    {
        Hex TargetsCurrentHex = OO.Orbit[OO.CurrentPosition];

        Hex ShipsCurrentHex = ship.CurrentHexPosition;

        int distance = Hex.Distance(ShipsCurrentHex, TargetsCurrentHex);

        int InitialDist = distance;

        int Intturns = (int)Mathf.Ceil((float)distance / (float)ship.Movement);
        int test = 0;
        if (Intturns == 1)
        {
            //the number of moves the ship has left is enough to make it to the target this turn
            //just return the current hex of the target > this will be used as the ships target of movement
            return 0;
        }
        int LowestPlanetTurns = -1;
        int LowestShipTurns = 10000;
        for (int i = 1; i < InitialDist; i++)
        {
            distance = Hex.Distance(ShipsCurrentHex, OO.Orbit[OO.PredictPlanetPos(i)]);

            Intturns = (int)Mathf.Ceil((float)distance / (float)ship.Movement);

            if(Intturns == i)
            {
                return Intturns;
            }
            test = Mathf.Abs(Intturns - i);

            if(LowestShipTurns < test)
            {
                return LowestPlanetTurns;
            }

            if (test <= 1)
            {
                LowestPlanetTurns = i;
                LowestShipTurns = Intturns;
            }
        }


        return LowestPlanetTurns;
    }
    public List<Hex> RendezvousWithOrbitingObject(OrbitalObject OO, Ship ship)
    {
        Hex TargetsPredictedHex;

        Hex ShipsCurrentHex = ship.CurrentHexPosition;

        int NumTurnsToRendevous = NumberOfTurnsToRendezvous(OO, ship);
        if(NumTurnsToRendevous == -1)
        {
            Debug.LogError("RendezvousWithOrbitingObject: Didnt find a path.");
            return null;
        }
        TargetsPredictedHex = OO.Orbit[OO.PredictPlanetPos(NumTurnsToRendevous)];
        List<Hex> Path = Hex.AstarPath(TargetsPredictedHex, ShipsCurrentHex);
        return Path;
    }
}
