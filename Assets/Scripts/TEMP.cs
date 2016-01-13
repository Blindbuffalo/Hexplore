using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TEMP : MonoBehaviour {
    public GameObject Prefab;
    public List<Hex> Hexes = null;
    // Use this for initialization
    void Start () {
        GridData.Instance.GenerateGridData(10);

        Hexes = GridData.Instance.HexData;
        DrawGraphics.Instance.GenerateGraphics(Hexes, Prefab);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
