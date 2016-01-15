using UnityEngine;
using System.Collections;
public enum ShipPartType { Engine, HullPlate, TransistorCoil }
public enum BioSampleType { Plant, Animal, Bacteria }
public enum NonBioSampleType { Dirt, Sand, Iron, Copper, Regolith, Aluminum, Water }
public class Cargo  {
    public Cargo()
    {
        
    }
    public string Name { get; set; }
    public float Size { get; set; }
    public float Weight { get; set; }
    

    public float Value { get; set; }
}
public class ShipPart : Cargo
{
    public ShipPart(string name, float size, float weight, int damage, float value, ShipPartType type)
    {
        Name = name;
        Size = size;
        Weight = weight;
        Value = value;


        Type = type;
        Damage = damage;
        
    }
    
    public ShipPartType Type { get; private set; }
    public int Damage { get; private set; }
}
public class MedicalSupplies : Cargo
{
    public MedicalSupplies(string name, float size, float weight, float value)
    {
        Name = name;
        Size = size;
        Weight = weight;
        Value = value;

    }
}
public class BiologicalSamples : Cargo
{
    public BiologicalSamples(string name, float size, float weight, float value, BioSampleType type)
    {
        Name = name;
        Size = size;
        Weight = weight;
        Value = value;
        Type = type;
    }
    public BioSampleType Type { get; private set; }
}
public class NonBiologicalSamples : Cargo
{
    public NonBiologicalSamples(string name, float size, float weight, float value, NonBioSampleType type)
    {
        Name = name;
        Size = size;
        Weight = weight;
        Value = value;
        Type = type;
    }
    public NonBioSampleType Type { get; private set; }
}
public class Contraband : Cargo
{
    public Contraband(string name, float size, float weight, float value, float illegality)
    {
        Name = name;
        Size = size;
        Weight = weight;
        Value = value;
        Illegality = illegality;
    }
    public float Illegality { get; private set; }
}
public class CargoResource : Cargo
{
    public CargoResource(string name, float size, float weight, float value, string shortname)
    {
        Name = name;
        Size = size;
        Weight = weight;
        Value = value;

        ShortName = shortname;
    }
    public string ShortName { get; protected set; }
}