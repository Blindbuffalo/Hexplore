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

    }
	
	// Update is called once per frame
	void Update () {
        if(run == false)
        {
            Planet p = GalaxyController.Instance.GetSolarSystem(0).Planets["Earth"];
            Ship s = GalaxyController.Instance.GetSolarSystem(0).Ships["Intrepid"];

            List<Hex> Hexs = RendevousWithOrbitingObject(p, s);

            foreach (Hex h in Hexs)
            {
              //  Debug.Log(Utilites.Instance.HexNameStr(h));
                DrawGraphics.Instance.DrawHex(h, Utilites.Instance.HexNameStr(h), Color.red);
            }
            foreach (Hex h in p.Orbit)
            {
                DrawGraphics.Instance.DrawHex(h, Utilites.Instance.HexNameStr(h), Color.blue);
            }
            
            run = true;
        }
	}
    public List<Hex> RendevousWithOrbitingObject(OrbitalObject OO, Ship ship)
    {
        Hex TargetsCurrentHex = OO.Orbit[OO.CurrentPosition];
        Hex TargetsPredictedHex;

        Hex ShipsCurrentHex = ship.CurrentHexPosition;



        List<Hex> Path = Hex.AstarPath(TargetsCurrentHex, ShipsCurrentHex);

        int distance = Path.Count;
        int predictedDist;
        

        int turns = (int)Mathf.Round(distance / ship.MovesLeft);

        if(turns <= 1)
        {
            //the number of moves the ship has left is enough to make it to the target this turn
            //just return the current hex of the target > this will be used as the ships target of movement
            return Path;
        }



        //target will have moved before we make it to its current hex.  try to predict our rendevous
        
        bool t = true;
        for (int turn = 0; turn < 100; turn++)
        {

            TargetsPredictedHex = OO.Orbit[OO.PredictPlanetPos(turn)];

            Path = Hex.AstarPath(TargetsPredictedHex, ShipsCurrentHex);

            distance = Path.Count - 1;

            turns = (int)Mathf.Ceil(distance / ship.Movement);
            
            if(turns == turn)
            {
                Debug.Log("Turns: " + turns + " Turn: " + turn + " Pathcount: " + Path.Count + " dist: " + distance);
                return Path;
            }


        }


        //something is not working correctly
        Debug.LogError("RendevousWithOrbitingObject: did not find a rendevous.");
        return null;
    }
}
