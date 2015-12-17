using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawHexGraphics : MonoBehaviour {
    public Dictionary<string, GameObject> HexGraphics = null;
    public Utilites Utility;
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
            Point p = Layout.HexToPixel(L, h);

            if (HexGraphics.ContainsKey(Utility.HexNameStr(h)))
            {

            }
            else
            {

                GameObject g = (GameObject)Instantiate(Prefab, new Vector3((float)p.x, (float)p.y, 0), Quaternion.identity);
                g.name = Utility.HexNameStr(h);
                g.transform.SetParent(this.transform);
                HexGraphics.Add(Utility.HexNameStr(h), g);
            }

        }
    }
    public void ChangeHexesColor(Hex h, Color color)
    {
        string HexName = Utility.HexNameStr(h);
        if (HexGraphics.ContainsKey(HexName))
        {
            GameObject HexGO = HexGraphics[HexName];
            HexGO.GetComponent<SpriteRenderer>().color = color;
            HexGraphics[HexName] = HexGO;
        }
    }
    public void DrawOrbit(Planet Planet, Color OrbitColor)
    {
        foreach (Hex h in Planet.Orbit)
        {
            ChangeHexesColor(h, OrbitColor);
        }

        //return Hexes;
    }

    public void DrawPlanet(Planet Planet, Color OrbitColor)
    {
        Planet.CurrentPosition = Planet.CurrentPosition + Planet.NumberOfMoves;

        if (Planet.CurrentPosition >= Planet.Orbit.Count)
        {
            Planet.CurrentPosition = Planet.CurrentPosition - Planet.Orbit.Count;

        }
        ChangeHexesColor(Planet.Orbit[Planet.LastPosition], OrbitColor);
        ChangeHexesColor(Planet.Orbit[Planet.CurrentPosition], Planet.Col);

        Planet.LastPosition = Planet.CurrentPosition;
    }
}
