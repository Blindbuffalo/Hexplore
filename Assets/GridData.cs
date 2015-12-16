using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridData  {

    public List<Hex> HexData { get; private set; }
    public void GenerateGridData(int radius)
    {
        HexData = new List<Hex>();
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                Hex f = new Hex(x, y, -x - y);
                HexData.Add(f);
            }
        }
    }

}
