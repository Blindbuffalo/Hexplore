using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
public class CharController : MonoBehaviour {

    public DrawHexGraphics HexG;
    public Utilites Utility;

    public GameObject MovementInd;
    Layout L = new Layout(Layout.pointy, new Vector3(.52f, .52f), new Vector3(0f, 0f));
    public Hex Center = new Hex(0, 0, 0);
    public Ship MainShip;
    public bool shipMoving = false;
    public bool MoveShip = false;
    
    public int MoveShipPos = 1;

    public Action<int> Test;
    public Hex MouseOverHex;
    public Hex MouseOverHexStart;
    public float speed = 0.0025F;
    public float rotationSpeed = 0.004F;
    public float MinNextTileDist = .025f;

    // Use this for initialization
    void Start () {
        MovementInd.SetActive(false);
        MainShip = new Ship(3, new Hex(5, 0, -5));
        MainShip.RegisterMovesLeftCB(RedrawMovementHexes);

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
        }
        if (shipMoving && MainShip.MovesLeft > 0)
        {
           // move the ship
            Vector3 t = Layout.HexToPixel(L, MainShip.PathToTarget[MoveShipPos], -10f);
            Vector3 c = Layout.HexToPixel(L, MainShip.CurrentHexPosition, 0f);
            Vector3 n = new Vector3(t.x - c.x, t.y - c.y, 0f);
            Vector3 m = new Vector3(this.transform.position.x, this.transform.position.y, -10);
            //Debug.Log(Utility.HexNameStr(MainShip.PathToTarget[MoveShipPos]));
            this.transform.Translate(n.normalized * 1f * Time.deltaTime);

            if ((t - m).sqrMagnitude < MinNextTileDist * MinNextTileDist)
            {

                
                MainShip.ShipMoved(MainShip.PathToTarget[MoveShipPos]);

               // MainShip.CurrentHexPosition = MainShip.PathToTarget[MoveShipPos];
                MoveShipPos++;
                if (MoveShipPos > MainShip.PathToTarget.Count - 1)
                {
                    MoveShipPos = 1;
                    shipMoving = false;
                    MovementInd.SetActive(false);
                }
            }


            
            
        }
        else
        {
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
                if (Utility.HexToVector3(MouseOverHexStart) != Utility.HexToVector3(MouseOverHex))
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
        }
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

        this.transform.position = Layout.HexToPixel(L, h, -10f);
        
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
}
