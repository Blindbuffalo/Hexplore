using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridController : MonoBehaviour
{
    public List<Hex> Hexes = null;
    public int t = 0;
    public GameObject Prefab;

    public int GridRadius = 30;
    
    public GridData Grid = new GridData();
    public Utilites Utility = new Utilites();

    public DrawHexGraphics HexG;

    public SolarSystem Sol;
    public float g = 0f;

    public Color OrbitColor = new Color(.05f, .05f, .05f, 1f);
	// Use this for initialization
	void Start () {
        HexG = this.transform.GetComponent<DrawHexGraphics>();
        Debug.Log("start");

        Grid.GenerateGridData(GridRadius);
        Hexes = Grid.HexData;
        HexG.GenerateGraphics(Hexes, Prefab);

        Hex Sun = new Hex(0, 0, 0);
        List<Planet> Planets = new List<Planet>();

        Planets.Add(new Planet("Mercury", 2, Sun, 1, Color.cyan, 1));
        Planets.Add(new Planet("Venus", 4, Sun, 1, Color.green, 1));
        Planets.Add(new Planet("Earth", 6, Sun, 1, Color.blue, 0));
        Planets.Add(new Planet("Mars", 9, Sun, 1, Color.red, 1));
        Planets.Add(new Planet("Jupitor", 31, Sun, 2, Color.magenta, 1));
        Planets.Add(new Planet("Saturn", 57, Sun, 3, Color.magenta, 1));
        Planets.Add(new Planet("Uranus", 85, Sun, 4, Color.magenta, 1));
        Planets.Add(new Planet("Neptune", 110, Sun, 5, Color.magenta, 1));
        Planets.Add(new Planet("Pluto", 145, Sun, 6, Color.magenta, 1));

        Sol = new SolarSystem("Sol", Sun, 2, Planets);


        foreach (Planet P in Sol.Planets)
        {
            DrawOrbit(P);
        }

        PlacePrefab(Prefab, Sol.SunRadius, new Vector3(Sol.Sun.q, Sol.Sun.r, Sol.Sun.s), Color.yellow);
	}
    void Update()
    {
        g += Time.deltaTime * 500;
        if (g > 100)
        {
            g = 0;
            foreach (Planet P in Sol.Planets)
            {
                DrawPlanet(P);
            }

        }
    }
    public void PlacePrefab(GameObject GO, float scale, Vector3 Position, Color Tint)
    {
        GO = (GameObject)Instantiate(GO, new Vector3(GO.transform.position.x, GO.transform.position.y, 5), Quaternion.identity);
        GO.transform.localScale = new Vector3(scale, scale, scale);
        GO.GetComponent<SpriteRenderer>().color = Tint;
    }
    public List<Hex> DrawOrbit(Planet Planet)
    {
        foreach (Hex h in Planet.Orbit)
        {

            HexG.ChangeHexesColor(Utility.HexNameStr(h), OrbitColor);
        }

        return Hexes;
    }
    
    public void DrawPlanet(Planet Planet)
    {
        Planet.CurrentPosition = Planet.CurrentPosition + Planet.NumberOfMoves;

        if (Planet.CurrentPosition >= Planet.Orbit.Count)
            {
                Planet.CurrentPosition = Planet.CurrentPosition - Planet.Orbit.Count;

            }
        HexG.ChangeHexesColor(Utility.HexNameStr(Planet.Orbit[Planet.LastPosition]), OrbitColor);
        HexG.ChangeHexesColor(Utility.HexNameStr(Planet.Orbit[Planet.CurrentPosition]), Planet.Col);

        Planet.LastPosition = Planet.CurrentPosition;
    }

}
