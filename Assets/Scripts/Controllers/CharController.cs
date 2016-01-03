using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
public class CharController : MonoBehaviour {

    public GameObject MovementInd;
    Layout L = new Layout(Layout.pointy, new Vector3(.52f, .52f), new Vector3(0f, 0f));
    public Hex Center = new Hex(0, 0, 0);
    public Ship MainShip;
    public bool shipMoving = false;
    public bool MoveShip = false;
    
    public int MoveShipPos = 1;

    public Action UpdateUI;
    public Hex MouseOverHex;
    public Hex MouseOverHexStart;
    public float speed = 0.0025F;
    public float rotationSpeed = 0.004F;
    public float MinNextTileDist = .025f;

    public bool checkedMissions = false;

    public int XP {get; protected set; }
    private int XPtoNextLevel = 100; //need to make this better!

    private static CharController instance;
    public static CharController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (CharController)FindObjectOfType(typeof(CharController));
                if (instance == null)
                    instance = (new GameObject("CharController")).AddComponent<CharController>();
            }
            return instance;
        }
    }

    // Use this for initialization
    void Start () {
        MovementInd.SetActive(false);
        MainShip = new Ship(5, new Hex(-5, 6, -1), 10f, 5f);
        MainShip.RegisterMovesLeftCB(RedrawMovementHexes);

       // MainShip.Cargohold.Add(new BiologicalSamples("Icky Goo", 0f, 0f, 0f, BioSampleType.Animal));
       // MainShip.Cargohold.Add(new ShipPart("Super Engine", 0f, 0f, 0, 0f, ShipPartType.Engine));

        placeShipOnHex(MainShip.CurrentHexPosition);

        MainShip.MovesLeft = MainShip.Movement;
        //List<Hex> Reachable = Hex.Reachable(MainShip.CurrentHexPosition, MainShip.MovesLeft);
        //HexG.CreateMovementHexGraphics(Reachable, Prefab, this.transform.gameObject);


    }
	
	// Update is called once per frame
	void Update () {
        float MousePositionY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        float MousePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;


        Vector3 P = new Vector3(MousePositionX, MousePositionY);
        FractionalHex FH = Layout.PixelToHex(L, P);
        MouseOverHex = FractionalHex.HexRound(FH);

        int qm = MainShip.CurrentHexPosition.q - MouseOverHex.q;
        int rm = MainShip.CurrentHexPosition.r - MouseOverHex.r;

        //Debug.Log(qm + " " + rm);

        if (MoveShip)
        {
            shipMoving = true;
            MoveShip = false;
            this.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        }
        if (shipMoving && MainShip.MovesLeft > 0)
        {
            checkedMissions = false;
            // move the ship
            Vector3 t = Layout.HexToPixel(L, MainShip.PathToTarget[MoveShipPos], -15f);
            Vector3 c = Layout.HexToPixel(L, MainShip.CurrentHexPosition, 0f);
            Vector3 n = new Vector3(t.x - c.x, t.y - c.y, 0f);
            Vector3 m = new Vector3(this.transform.position.x, this.transform.position.y, -15);
            //Debug.Log(Utility.HexNameStr(MainShip.PathToTarget[MoveShipPos]));


            //Child.transform.Rotate(Vector3.forward, .1f);


            if (RotateShip(this.transform.GetChild(0).gameObject, n))
            {
                this.transform.Translate(n.normalized * 2f * Time.deltaTime);

                if ((t - m).sqrMagnitude < MinNextTileDist * MinNextTileDist)
                {


                    MainShip.ShipMoved(MainShip.PathToTarget[MoveShipPos]);



                    // MainShip.CurrentHexPosition = MainShip.PathToTarget[MoveShipPos];
                    MoveShipPos++;
                    if (MoveShipPos > MainShip.PathToTarget.Count - 1)
                    {
                        //ship has reached its destination
                        MoveShipPos = 1;
                        shipMoving = false;
                        MovementInd.SetActive(false);
                    }
                }
            }
        }
        else
        {
            //only check on the missions progress at the end of a move cycle.
            if (checkedMissions == false)
            {
                MissionController.Instance.MainMissionProgress();

                checkedMissions = true;
            }
                //ship doesnt need to move wait for right click from user
            //right mouse button
            if (Input.GetMouseButtonDown(1))
            {
                MainShip.SetTargetHex(MouseOverHex);

                MouseOverHexStart = MouseOverHex;

                TextMesh Txt = MovementInd.GetComponentInChildren<TextMesh>();

                if (BlockedHexes.Instance.HexData.Contains(MouseOverHex))
                {
                    Txt.text = "X";
                }
                else
                {
                    Txt.text = TurnsToTarget().ToString();
                }
                
                MovementInd.SetActive(true);

                MovementInd.transform.position = Layout.HexToPixel(L, MouseOverHex, -9f);

                CreateMovementLine();
            }

            if (Input.GetMouseButton(1))
            {
                //triggers while the button is down
                if (Utilites.Instance.HexToVector3(MouseOverHexStart) != Utilites.Instance.HexToVector3(MouseOverHex))
                {
                    MainShip.SetTargetHex(MouseOverHex);
                    //Debug.Log("Mouse moved to another hex");
                    MouseOverHexStart = MouseOverHex;

                    MovementInd.transform.position = Layout.HexToPixel(L, MouseOverHex, -9f);
                    TextMesh Txt = MovementInd.GetComponentInChildren<TextMesh>();

                    if (BlockedHexes.Instance.HexData.Contains(MouseOverHex))
                    {
                        Txt.text = "X";
                    }
                    else
                    {
                        Txt.text = TurnsToTarget().ToString();
                    }

                    CreateMovementLine();
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                //Debug.Log("up");
                //MovementInd.SetActive(false);
                if (BlockedHexes.Instance.HexData.Contains(MouseOverHex))
                {
                    MoveShip = false;
                    MoveShipPos = 1;
                    shipMoving = false;
                    MovementInd.SetActive(false);
                }
                else
                {
                    MoveShip = true;
                }
                
            }

            if (OnHexWithPlanet())
            {
                
                this.transform.GetChild(0).localPosition = new Vector3(0, .4f, 0);
            }
            else
            {
                this.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
            }
        }

        

    }
    public bool OnHexWithPlanet()
    {
        // check to see if we are on a tile with a planet
        foreach (Planet p in SolarSystem.Instance.Planets)
        {
            if (Hex.Equals(MainShip.CurrentHexPosition, p.Orbit[p.CurrentPosition]))
            {
                //Debug.Log("in the same square as a planet! " + p.Name);


                

                return true;
            }
        }
        return false;
    }
    public bool RotateShip(GameObject Child, Vector3 direction)
    {
        

        //Vector3 direction = Layout.HexToPixel(L, new Hex(-9, 10, 0), 0f) - Child.transform.position;
        float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;




        //current angle in degrees
        float anglecurrent = Child.transform.eulerAngles.z;
        float checkAngle = 0;


        checkAngle = ((ang + 360) % 360);
        float CheckTurnDir = anglecurrent - checkAngle;
       //Debug.Log("A:" + ang + " -- AC" + anglecurrent + " -- CA" + checkAngle + " TD:" + CheckTurnDir);


        

        if (anglecurrent <= checkAngle + 0.5f && anglecurrent >= checkAngle - 0.5f)
        {
            return true;
        }
        else
        {
            if (CheckTurnDir > 0)
            {
                Child.transform.Rotate(new Vector3(0, 0, -2f));
            }
            else
            {
                Child.transform.Rotate(new Vector3(0, 0, 2f));
                //Debug.Log(ang + "AC" + anglecurrent + " -- CA" + checkAngle + " TD:" + CheckTurnDir);
            }
        }

        return false;
    }
    public int TurnsToTarget()
    {
        int d = Hex.Distance(MainShip.CurrentHexPosition, MouseOverHex);
        int TurnsToTarget = 0;

        if (d > MainShip.MovesLeft)
        {
            float r = d - MainShip.MovesLeft;
            float t = (r / MainShip.Movement) + 1;
            TurnsToTarget = (int)Mathf.Ceil(t);
        }
        else
        {
            TurnsToTarget = 1;
        }
        return TurnsToTarget;
    }
    public void CreateMovementLine()
    {
        LineRenderer LR = MovementInd.GetComponent<LineRenderer>();
        int i = 0;
        LR.SetVertexCount(MainShip.PathToTarget.Count);
        foreach (Hex h in MainShip.PathToTarget)
        {
            //Debug.Log(i.ToString());
            LR.SetPosition(i, Layout.HexToPixel(L, h, -8f));
            i++;
        }
    }
    private void placeShipOnHex(Hex h)
    {

        this.transform.position = Layout.HexToPixel(L, h, -15f);
        
    }
    void RedrawMovementHexes()
    {
        List<Hex> Reachable = Hex.Reachable(MainShip.CurrentHexPosition, MainShip.MovesLeft);

        foreach (Transform child in transform)
        {
            if (!child.name.StartsWith("ship"))
            {
                GameObject.Destroy(child.gameObject);
            }

        }
    }

    public void IncreaseXP(int xp)
    {
        if (XP + xp >= this.XPtoNextLevel)
        {
            int Remainder = this.XPtoNextLevel - XP;
            XP = Remainder;
        }
        else
        {
            XP += xp;
        }

        UpdateUI();
    }
}
