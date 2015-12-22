﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Ship  {
    Action CB_MovesLeftChanged;
    public Ship(int movement, Hex hexposition)
    {
        Movement = movement;
        CurrentHexPosition = hexposition;

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
}
