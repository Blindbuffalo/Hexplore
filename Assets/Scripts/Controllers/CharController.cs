using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
public class CharController : MonoBehaviour {

    public DrawHexGraphics HexG;
    public Utilites Utility;
    public GameObject Prefab;
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

        if (MoveShip)
        {
            shipMoving = true;
            MoveShip = false;
        }
        if (shipMoving && MainShip.MovesLeft > 0)
        {
           // move the ship
            Vector3 t = Layout.HexToPixel(L, MainShip.PathToTarget[MoveShipPos], 0f);
            Vector3 c = Layout.HexToPixel(L, MainShip.CurrentHexPosition, 0f);
            Vector3 n = new Vector3(t.x - c.x, t.y - c.y, 0f);
            this.transform.Translate(n.normalized * 2f * Time.deltaTime);
            float offset = .05f;
            
            if (this.transform.position.x > t.x - offset && this.transform.position.x < t.x + offset && this.transform.position.y > t.y - offset && this.transform.position.y < t.y + offset)
            {
                MainShip.ShipMoved(MainShip.PathToTarget[MoveShipPos]);
                
                //MainShip.CurrentHexPosition = MainShip.PathToTarget[MoveShipPos];
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

                //List<Hex> t = Hex.AstarPath(MainShip.CurrentHexPosition, MouseOverHex);
                // foreach(Hex k in t)
                // {
                //     HexG.ChangeHexesColor(k, Color.red);
                // }
                //Debug.Log("down");
                //triggers only on first down
                MainShip.SetTargetHex(MouseOverHex);

                MouseOverHexStart = MouseOverHex;

                TextMesh Txt = MovementInd.GetComponentInChildren<TextMesh>();
                Txt.text = TurnsToTarget().ToString();
                MovementInd.SetActive(true);

                MovementInd.transform.position = Layout.HexToPixel(L, MouseOverHex, -9.89f);

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

                    MovementInd.transform.position = Layout.HexToPixel(L, MouseOverHex, -9.89f);
                    TextMesh Txt = MovementInd.GetComponentInChildren<TextMesh>();
                    Txt.text = TurnsToTarget().ToString();

                    CreateMovementLine();
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                //Debug.Log("up");
                //MovementInd.SetActive(false);
                MoveShip = true;
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
            LR.SetPosition(i, Layout.HexToPixel(L, h, -9.75f));
            i++;
        }
    }
    //private void moveShipToHex(Hex h)
    //{
    //    Point P2 = Layout.HexToPixel(L, h);
    //    this.transform.position = new Vector3((float)P2.x, (float)P2.y, 10f);

    //    this.transform.Translate(new Vector3((float)P2.x, (float)P2.y, 10f).normalized * Time.deltaTime);
    //}
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

        //HexG.CreateMovementHexGraphics(Reachable, Prefab, this.transform.gameObject);
    }
}
