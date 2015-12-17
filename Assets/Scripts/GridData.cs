using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridData  {

    public List<Hex> HexData { get; private set; }
    public void GenerateGridData(int radius)
    {
        HexData = new List<Hex>();

        int Xcheck = 0;
        int Xcheck2 = 0;

        HexData = new List<Hex>();
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (x < Xcheck)
                {

                }
                else
                {
                    if (y > 0 && x == radius - Xcheck2)
                    {
                        Xcheck2++;
                        break;
                    }
                    else
                    {
                        Hex f = new Hex(x, y, -x - y);
                        HexData.Add(f);
                    }
                }
            }
            if (Xcheck != -radius)
            {
                Xcheck--;
            }
        }
    }
}
