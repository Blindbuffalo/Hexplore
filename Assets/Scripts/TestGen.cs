using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[ExecuteInEditMode]
public class TestGen : MonoBehaviour {
    public  GameObject HexPrefab;
    public int RadiusStart = 0;
    public int radiusEnd = 85;
    public bool t = false;
    private Layout L = new Layout(Layout.pointy, new Vector3(1f, 1f), new Vector3(0f, 0f));
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    if(t == false)
        {
            GameObject go = null;
            go = new GameObject("GameObject");

            go.transform.SetParent(this.transform);
            int count = 1;
            List<Hex> Orbits = new List<Hex>();
            for (int Radius = RadiusStart; Radius <= radiusEnd; Radius++)
            {
                Orbits.AddRange( CalcOrbit(Radius) );
            }

            foreach (Hex h in Orbits)
            {
                if (count >= 9300)
                {

                    count = 0;

                    go = new GameObject("GameObject");
                    go.transform.SetParent(this.transform);
                }

                GameObject g = (GameObject)Instantiate(HexPrefab, Layout.HexToPixel(L, h, 8), Quaternion.identity);

                g.name = Utilites.Instance.HexNameStr(h);
                //g.GetComponent<SpriteRenderer>().color = Planet.Col;
                //g.GetComponentInChildren<Renderer>().material.color = Color.red;
                g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
                g.transform.SetParent(go.transform);
                count++;
            }

            Orbits = null;
            count++;
            t = true;
            //Combine();
        }
	}
    public List<Hex> LineOfHexes()
    {
        List<Hex> path = Hex.AstarPath(new Hex(3, 0, -3), new Hex(500, 0, -500));
        foreach (Hex h in path)
        {
            GameObject g = (GameObject)Instantiate(HexPrefab, Layout.HexToPixel(L, h, 8), Quaternion.identity);

            g.name = Utilites.Instance.HexNameStr(h) + "_path";
            //g.GetComponent<SpriteRenderer>().color = Planet.Col;
            //g.GetComponentInChildren<Renderer>().material.color = Color.red;
            g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
            g.transform.SetParent(this.transform);
        }

        return path;
    }
    protected List<Hex> CalcOrbit(int OrbitRadius)
    {

        Hex Scale = Hex.Scale(Hex.directions[4], OrbitRadius);
        Hex CurrentHex = Hex.Add(new Hex(0,0,0), Scale);

        List<Hex> Hexes = new List<Hex>();
        foreach (Hex Dir in Hex.directions)
        {
            //Debug.Log(Dir.ToString());
            for (int j = 0; j < OrbitRadius; j++)
            {
                Hexes.Add(CurrentHex);
                CurrentHex = Hex.Add(CurrentHex, Dir);
                //Debug.Log(CurrentHex.X + " " + CurrentHex.Y);

            }
        }

        return Hexes;
    }
    public void tt()
    {
        GridData.Instance.GenerateGridData(50);
        List<Hex> hs = GridData.Instance.HexData;
        //foreach (Hex h in hs)
        int Parent = 0;
        int count = 0;
        GameObject go = new GameObject("GameObject");
        go.transform.SetParent(this.transform);
        for (int i = 0; i < hs.Count; i++)
        {
            if (count >= 10000)
            {
                Parent++;
                count = 0;

                go = new GameObject("GameObject");
                go.transform.SetParent(this.transform);
            }

            GameObject g = (GameObject)Instantiate(HexPrefab, Layout.HexToPixel(L, hs[i], 8), Quaternion.identity);

            g.name = "Int" + "_Hex";
            //g.GetComponent<SpriteRenderer>().color = Planet.Col;
            //g.GetComponentInChildren<Renderer>().material.color = Color.red;
            g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
            g.transform.SetParent(go.transform);
            count++;
        }
    }
    public void Combine()
    {
        MeshFilter[] meshFilters =  this.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length - 1];
        int i = 0;
        foreach (MeshFilter MF in meshFilters)
        {
            if (MF.gameObject.GetInstanceID() == this.gameObject.GetInstanceID())
            {

            }
            else
            { 
                combine[i].mesh = MF.sharedMesh;
                combine[i].transform = MF.transform.localToWorldMatrix;
                MF.gameObject.SetActive(false);
                i++;
            }
            
        }
        transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);
    }

}
