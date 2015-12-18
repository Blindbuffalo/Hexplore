using UnityEngine;
using System.Collections;

public class Ship  {
    public Ship(int movement, Hex hexposition)
    {
        Movement = movement;
        MovesLeft = movement;
        CurrentHexPosition = hexposition;
    }
    public void ShipMoved(Hex NewHexLocation)
    {
        int NumberOfMoves = Hex.Distance(CurrentHexPosition, NewHexLocation);
        CurrentHexPosition = NewHexLocation;
        MovesLeft = MovesLeft - NumberOfMoves;
    }
    public int Movement { get; private set; }
    public int MovesLeft { get; private set; }
    public Hex CurrentHexPosition { get; set; }

}
