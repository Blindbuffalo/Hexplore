using UnityEngine;
using System.Collections;

public class Atmosphere  {
    public Atmosphere(bool breathable = false)
    {
        Breathable = breathable;
    }
    public bool Breathable {get; protected set;}
}

