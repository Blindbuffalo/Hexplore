using UnityEngine;
using System.Collections;

public class Utilites : MonoBehaviour {

    public Vector3 HexToVector3(Hex hex)
    {
        return new Vector3(hex.q, hex.r, hex.s);
    }
    public string HexNameStr(Hex hex)
    {
        return hex.q + "_" + hex.r + "_" + hex.s;
    }
    public GameObject PlacePrefab(GameObject GO, float scale, Vector3 Position, Color Tint)
    {
        GO = (GameObject)Instantiate(GO, new Vector3(GO.transform.position.x, GO.transform.position.y, 5), Quaternion.identity);
        GO.transform.localScale = new Vector3(scale, scale, scale);
        GO.GetComponent<SpriteRenderer>().color = Tint;
        return GO;
    }
    public Color RGBcolor(int r, int g, int b, int a)
    {


        return new Color((float)r / 255, (float)g / 255, (float)b / 255, (float)a / 255);
    }
}
