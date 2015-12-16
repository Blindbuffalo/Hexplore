using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawHexGraphics : MonoBehaviour {
    public Dictionary<string, GameObject> HexGraphics = null;
    public Utilites Utility = new Utilites();
    void Start()
    {
        Debug.Log("hexgraphics Start");
        
    }
    public void GenerateGraphics(List<Hex> Hexes, GameObject Prefab)
    {
        HexGraphics = new Dictionary<string, GameObject>();
        Layout L = new Layout(Layout.pointy, new Point(.52, .52), new Point(0, 0));

        foreach (Hex h in Hexes)
        {
           
            //if (h.q == 0f && h.r == 0f && h.s == 0f)
            //{
            //    Hex TestHex = Hex.Neighbor(h, 0);
            //    Debug.Log(TestHex.q + " " + TestHex.r + " " + TestHex.s);
            //}
            Point p = Layout.HexToPixel(L, h);

            if (HexGraphics.ContainsKey(Utility.HexNameStr(h)))
            {

            }
            else
            {

                GameObject g = (GameObject)Instantiate(Prefab, new Vector3((float)p.x, (float)p.y, 0), Quaternion.identity);
                g.name = Utility.HexNameStr(h);
                HexGraphics.Add(Utility.HexNameStr(h), g);
            }

        }
    }
    public void ChangeHexesColor(string HexName, Color color)
    {
        if (HexGraphics.ContainsKey(HexName))
        {
            GameObject HexGO = HexGraphics[HexName];
            HexGO.GetComponent<SpriteRenderer>().color = color;
            HexGraphics[HexName] = HexGO;
        }
    }
}
