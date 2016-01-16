using UnityEngine;
using System.Collections;

public class Empire  {
    public Empire(string longname, string shortname, int comercialcargoshipsallowed = 1, int comercialcolonyshipsallowed = 1 )
    {
        LongName = longname;
        ShortName = shortname;
        ComercialCargoShipsAllowed = comercialcargoshipsallowed;
        ComercialColonyShipsAllowed = comercialcolonyshipsallowed;

        CurrentComercialCargoShips = 0;
    }

    private string LongName = "The Terran Federation";
    private string ShortName = "The Federation";

    public int ComercialCargoShipsAllowed { get; protected set; }
    public int ComercialColonyShipsAllowed { get; protected set; }

    public int CurrentComercialCargoShips { get; set; }

    

    public bool CanSpawnCargoShip()
    {
        if(CurrentComercialCargoShips < ComercialCargoShipsAllowed)
        {
            return true;
        }
        return false;
    }
}
