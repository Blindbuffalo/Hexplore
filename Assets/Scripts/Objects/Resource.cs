using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ResourceList { Iron, Lithium, Steel };
public class Resource  {
    public Resource(string name, string shortname)
    {
        Name = name;
        ShortName = shortname;
    }

    public string Name { get; protected set; }
    public string ShortName { get; protected set; }

    public float Ammount { get; protected set; }

}

public class MineableResources
{
    public Dictionary<ResourceList, Resource> Resources = new Dictionary<ResourceList, Resource>() {
        { ResourceList.Iron, new Resource("Iron", "Ir") },
        { ResourceList.Lithium, new Resource("Lithium", "Li") }
        };
}
public class ManufacturableResources
{
    public Dictionary<ResourceList, Resource> Resources = new Dictionary<ResourceList, Resource>() {
        { ResourceList.Steel, new Resource("Steel", "St") }
        };
}
public class ResourceStockpile
{
    public MineableResources minedResources = new MineableResources();
    public ManufacturableResources manufacturedResources = new ManufacturableResources();
}