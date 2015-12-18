using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CharController : MonoBehaviour {

    public DrawHexGraphics HexG;
    public Utilites Utility;

    Layout L = new Layout(Layout.pointy, new Point(.52, .52), new Point(0, 0));

    public Ship MainShip;


    // Use this for initialization
    void Start () {
        MainShip = new Ship(3, new Hex(5, 0, -5));
        moveShipToHex(MainShip.CurrentHexPosition);

        List<Hex> Reachable = Hex.Reachable(MainShip.CurrentHexPosition, MainShip.MovesLeft);
        foreach (Hex h in Reachable)
        {
            HexG.ChangeHexesColor(h, Color.cyan);
        }

    }
	
	// Update is called once per frame
	void Update () {
        float currentCamPositionY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        float currentCamPositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        if (Input.GetMouseButton(0)) //left mouse button
        {
            Point P = new Point(currentCamPositionX, currentCamPositionY);
            FractionalHex FH = Layout.PixelToHex(L, P);
            Hex ClickedHex = FractionalHex.HexRound(FH);

            int d = Hex.Distance(MainShip.CurrentHexPosition, ClickedHex);

            if(d > MainShip.MovesLeft)
            {
                Debug.LogError("you clicked outside of the allowed movement area.");
            }
            else
            {
                MainShip.ShipMoved(ClickedHex);

                moveShipToHex(ClickedHex);

                List<Hex> Reachable = Hex.Reachable(MainShip.CurrentHexPosition, MainShip.MovesLeft);
                foreach (Hex h in Reachable)
                {
                    HexG.ChangeHexesColor(h, Color.red);
                }
            }
            

            //
            //List<Hex> H = new List<Hex>();
            //for (int dx = -3; dx <= 3; dx++)
            //{
            //    for (int dy = Mathf.Max(-3, -dx - 3); dy <= Mathf.Min(3, -dx + 3); dy++)
            //    {
            //        int dz = -dx - dy;
            //        H.Add(Hex.Add(h, new Hex(dx, dy, dz)));
            //    }
            //}
            //foreach (Hex r in H)
            //{
            //    HexG.ChangeHexesColor(r, Color.red);
            //}

        }
    }
    private void moveShipToHex(Hex h)
    {
        Point P2 = Layout.HexToPixel(L, h);
        this.transform.position = new Vector3((float)P2.x, (float)P2.y, 0f);
    }

}
