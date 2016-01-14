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
            Planet p = GalaxyController.Instance.GetSolarSystem(0).Planets["Saturn"];
            Ship s = GalaxyController.Instance.GetSolarSystem(0).Ships["Intrepid"];

            List<Hex> Hexs = RendevousWithOrbitingObject(p, s);
            if (Hexs == null)
            {
                run = true;
                return;
            }
            DrawSolarSystemGraphics.Instance.DrawHex(Hexs[0],"tttt", Color.red, offset: -1);
            //foreach (Hex h in p.Orbit)
            //{
            //    DrawGraphics.Instance.DrawHex(h, Utilites.Instance.HexNameStr(h), Color.blue, offset: 1);
            //}
            //foreach (Hex h in Hexs)
            //{
            //    //  Debug.Log(Utilites.Instance.HexNameStr(h));
            //    DrawGraphics.Instance.DrawHex(h, Utilites.Instance.HexNameStr(h), Color.red);
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
    public List<Hex> RendevousWithOrbitingObject(OrbitalObject OO, Ship ship)
    {
        Hex TargetsCurrentHex = OO.Orbit[OO.CurrentPosition];
        Hex TargetsPredictedHex;

        Hex ShipsCurrentHex = ship.CurrentHexPosition;



        List<Hex> Path = Hex.AstarPath(TargetsCurrentHex, ShipsCurrentHex);

        int distance = Path.Count;
        int predictedDist;
        int InitialDist = distance;
        Debug.Log("Initial Dist: " + InitialDist);
        int Intturns = (int)Mathf.Ceil(distance / ship.MovesLeft);

        if(Intturns <= 1)
        {
            //the number of moves the ship has left is enough to make it to the target this turn
            //just return the current hex of the target > this will be used as the ships target of movement
            return Path;
        }



        //target will have moved before we make it to its current hex.  try to predict our rendevous
        int LowestTurns = Intturns;
        int test = 0;
        bool t = true;
        for (int turn = 0; turn <= (Intturns + 50); turn++)
        {

            TargetsPredictedHex = OO.Orbit[OO.PredictPlanetPos(turn)];

            Path = Hex.AstarPath(TargetsPredictedHex, ShipsCurrentHex);

            distance = Path.Count - 1;

           int turns = (int)Mathf.Ceil(distance / ship.Movement);
            
            if (turns == turn)
            {
                
                return Path;
            }
            test = Mathf.Abs( turns - turn);


            if (test <= 1 )
                LowestTurns = turns;

           // Debug.Log("Test: " + test  + "LowestTurn: " + LowestTurns + "Turns: " + turns + " IntTurns: " + Intturns + " Turn: " + turn + " Pathcount: " + Path.Count + " dist: " + distance);
        }
        TargetsPredictedHex = OO.Orbit[OO.PredictPlanetPos(LowestTurns)];
        Path = Hex.AstarPath(TargetsPredictedHex, ShipsCurrentHex);
        return Path;
        //something is not working correctly
        //Debug.LogError("RendevousWithOrbitingObject: did not find a rendevous.");
        //return null;
    }
}
