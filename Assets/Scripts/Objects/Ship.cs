using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Ship  {
    Action CB_MovesLeftChanged;
    public Ship(int movement, Hex hexposition, float CHweight, float CHsize)
    {
        Movement = movement;
        CurrentHexPosition = hexposition;
        Cargohold = new CargoHold(CHsize, CHweight);

    }
    public void RegisterMovesLeftCB(Action A)
    {
        CB_MovesLeftChanged = A;
    }
    public void ShipMoved(Hex NewHexLocation)
    {
        int NumberOfMoves = Hex.Distance(CurrentHexPosition, NewHexLocation);
        CurrentHexPosition = NewHexLocation;
        MovesLeft = MovesLeft - NumberOfMoves;
    }
    public int Movement { get; private set; }
    private int _movesleft = 0;
    public int MovesLeft
    {
        get
        {
            return _movesleft;
        }
        set
        {
            _movesleft = value;
            CB_MovesLeftChanged();
        }
    }
    public Hex CurrentHexPosition { get; set; }
    public Hex TargetHex { get; private set; }
    public void SetTargetHex(Hex hex)
    {
        TargetHex = hex;
        
        PathToTarget = Hex.AstarPath(this.CurrentHexPosition, TargetHex);
        
    }
    public List<Hex> PathToTarget { get; protected set; }

    public CargoHold Cargohold { get; set; }
}
public class CargoHold
{
    public CargoHold(float sizelimit, float weightlimit)
    {
        SizeLimit = sizelimit;
        WeightLimit = weightlimit;
        Hold = new List<Cargo>();
    }
    public float SizeLimit { get; set; }
    public float WeightLimit { get; set; }

    public float SizeUsedUp { get; private set; }
    public float CurrentWeight { get; private set; }
    public List<Cargo> Hold
    {
        get; set;
    }

    public bool Add(Cargo C)
    {

        if(C.Size + SizeUsedUp > SizeLimit)
        {
            return false;
        }
        if (C.Weight + CurrentWeight > WeightLimit)
        {
            return false;
        }

        Hold.Add(C);
        return true;
    }
}