using UnityEngine;
using System.Collections;

public class Utilites {

    public Vector3 HexToVector3(Hex hex)
    {
        return new Vector3(hex.q, hex.r, hex.s);
    }
    public string HexNameStr(Hex hex)
    {
        return hex.q + "_" + hex.r + "_" + hex.s;
    }
}
