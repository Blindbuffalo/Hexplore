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
    Layout L = new Layout(Layout.pointy, new Point(.52, .52), new Point(0, 0));
    public Hex Center = new Hex(0, 0, 0);
    public Ship MainShip;
    public bool shipMoving = false;
    public Action<int> Test;
    public Hex MouseOverHex;
    public Hex MouseOverHexStart;
    // Use this for initialization
    void Start () {
        MovementInd.SetActive(false);
        MainShip = new Ship(1, new Hex(5, 0, -5));
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


        Point P = new Point(MousePositionX, MousePositionY);
        FractionalHex FH = Layout.PixelToHex(L, P);
        MouseOverHex = FractionalHex.HexRound(FH);


        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("down");

            MouseOverHexStart = MouseOverHex;
            
            TextMesh Txt = MovementInd.GetComponentInChildren<TextMesh>();
            Txt.text = TurnsToTarget().ToString();
            MovementInd.SetActive(true);
            
            
            Point p = Layout.HexToPixel(L, MouseOverHex);
            MovementInd.transform.position = new Vector3((float)p.x, (float)p.y, 9.89f);
            
        }

        if (Input.GetMouseButton(1)) //right mouse button
        {
            if(Utility.HexToVector3(MouseOverHexStart) != Utility.HexToVector3(MouseOverHex))
            {
                Debug.Log("Mouse moved to another hex");
                MouseOverHexStart = MouseOverHex;
                Point p = Layout.HexToPixel(L, MouseOverHex);
                MovementInd.transform.position = new Vector3((float)p.x, (float)p.y, 9.89f);
                TextMesh Txt = MovementInd.GetComponentInChildren<TextMesh>();
                Txt.text = TurnsToTarget().ToString();
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("up");
            MovementInd.SetActive(false);
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
    //private void moveShipToHex(Hex h)
    //{
    //    Point P2 = Layout.HexToPixel(L, h);
    //    this.transform.position = new Vector3((float)P2.x, (float)P2.y, 10f);

    //    this.transform.Translate(new Vector3((float)P2.x, (float)P2.y, 10f).normalized * Time.deltaTime);
    //}
    private void placeShipOnHex(Hex h)
    {



        Point P2 = Layout.HexToPixel(L, h);

        //Vector3 dir = Utility.HexToVector3(h) -this.transform.position;

        //float speed = 1;

        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //Quaternion q = Quaternion.AngleAxis(angle, Vector3.left);
        //transform.rotation = q;

        this.transform.position = new Vector3((float)P2.x, (float)P2.y, 10f);
        
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
