using UnityEngine;
using System.Collections;

public class Empire  {
    public Empire(string longname, string shortname, int comercialcargoshipsallowed, int comercialcolonyshipsallowed)
    {
        LongName = longname;
        ShortName = shortname;
        ComercialCargoShipsAllowed = comercialcargoshipsallowed;
        ComercialColonyShipsAllowed = comercialcolonyshipsallowed;
    }

    private string LongName = "The Terran Federation";
    private string ShortName = "The Federation";

    private int ComercialCargoShipsAllowed = 1;
    private int ComercialColonyShipsAllowed = 1;

    
}
