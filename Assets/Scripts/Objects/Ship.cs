﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
public class Ship  {
    Action CB_MovesLeftChanged;

    public string Name { get; set; }
    public Ship(string name, int movement, Hex hexposition, float CHweight, float CHsize)
    {
        Name = name;
        Movement = movement;
        MovesLeft = movement;
        CurrentHexPosition = hexposition;
        Cargohold = new CargoHold(CHsize, CHweight);
        PositionOnPath = 0;
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
            if(CB_MovesLeftChanged != null)
                CB_MovesLeftChanged();
        }
    }
    public Hex CurrentHexPosition { get; set; }
    //public Hex TargetHex { get; private set; }
    //public void SetTargetHex(Hex hex)
    //{
    //    TargetHex = hex;
        
    //    PathToTarget = Hex.AstarPath(this.CurrentHexPosition, TargetHex);
        
    //}
    public List<Hex> PathToTarget { get; set; }
    public int PositionOnPath { get; set; }
    public CargoHold Cargohold { get; set; }
    public bool justSpawned { get; set; }
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
        get; protected set;
    }
    public bool ItemInCargoHold(Cargo i)
    {
        foreach (Cargo c in Hold)
        {
            ShipPart sp = c as ShipPart;
            if (sp != null)
            {
                ShipPart CargoNeeded = i as ShipPart;
                if (CargoNeeded != null)
                {
                    if (sp.Type == CargoNeeded.Type)
                    {

                        return true;
                    }
                }
            }
        }
        return false;
    }
    public bool RemoveFirstItemOfSameType(Cargo i)
    {
        ShipPart CargoNeeded = i as ShipPart;
        Cargo C = (from c in Hold
                   where (c as ShipPart).Type == CargoNeeded.Type
                   select c).FirstOrDefault();

        if (C != null)
        {

            Hold.Remove(C);
            Debug.Log("removed");
            return true;
        }

        return false;
    }
    public void DebugListOfItemsInHold()
    {
        Debug.LogError("CargoHold:");
        Debug.Log("------------------------------------------------------");
        foreach (Cargo c in Hold)
        {
            Debug.Log(c.Name + ":  " + (c as ShipPart).Type.ToString());
        }
        Debug.Log("------------------------------------------------------");
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