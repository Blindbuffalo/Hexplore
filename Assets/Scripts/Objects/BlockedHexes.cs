using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockedHexes  {


    private static BlockedHexes instance;

    private BlockedHexes() { }

    public static BlockedHexes Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BlockedHexes();
            }
            return instance;
        }
    }
    public List<Hex> HexData { get; set; }
}

