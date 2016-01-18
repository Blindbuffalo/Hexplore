using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

public class XmlIO
{
    public void WriteXmlFile(Dictionary<int, SolarSystem> Galaxy)
    {
        XDocument doc;
        XElement XmlSolarsystem = null;
        XElement XmlGalaxy = new XElement("Galaxy");
        XElement XmlSolarSystems = new XElement("SolarSystems");
        foreach (KeyValuePair<int, SolarSystem> sol in Galaxy)
        {
            XmlSolarsystem = new XElement("solarsystem",
                    new XAttribute("id", sol.Key),
                    new XAttribute("name", sol.Value.Name),
                    new XAttribute("sunradius", sol.Value.SunRadius),
                    new XElement("sunhex",
                        new XAttribute("q", sol.Value.Sun.q),
                        new XAttribute("r", sol.Value.Sun.r),
                        new XAttribute("s", sol.Value.Sun.s)
                    ),
                    new XElement("planets", 
                        sol.Value.Planets.Select(p => new XElement("planet",
                            new XAttribute("name", p.Key),
                            new XAttribute("orbitradius", p.Value.OrbitRadius),
                            new XAttribute("size", p.Value.Size),
                            new XAttribute("numberofmoves", p.Value.NumberOfMoves),
                            new XAttribute("currentposition", p.Value.CurrentPosition),
                            new XAttribute("orbitdir", (int)p.Value.OrbitDirection),
                            new XAttribute("gravity", p.Value.Gravity),
                            new XElement("color",
                                new XAttribute("r", p.Value.Col.r),
                                new XAttribute("g", p.Value.Col.g),
                                new XAttribute("b", p.Value.Col.b),
                                new XAttribute("a", p.Value.Col.a)
                                ),
                            new XElement("parenthex",
                                    new XAttribute("q", p.Value.Parent.q),
                                    new XAttribute("r", p.Value.Parent.r),
                                    new XAttribute("s", p.Value.Parent.s)
                                ),
                            new XElement("atmoshpere",
                                    new XAttribute("breathable", (bool)p.Value.Atmosphere.Breathable)
                                )
                            )
                        )
                    )
                );

            XmlSolarSystems.Add(XmlSolarsystem);
            
        }
        XmlGalaxy.Add(XmlSolarSystems);
        doc = new XDocument(
                XmlGalaxy
            );

        doc.Save(Application.dataPath + "/XMLdata/GalaxyData.xml");
    }
    public Dictionary<int, SolarSystem> ReadXmlFile()
    {
        XDocument doc = XDocument.Load(Application.dataPath + "/XMLdata/GalaxyData.xml");
        Dictionary<int, SolarSystem> Sols = (from s in doc.Descendants("solarsystem")
                                        select new SolarSystem
                                        {
                                            id = (int?)s.Attribute("id") ?? 0,
                                            Name = (string)s.Attribute("name") ?? "unknownsol",
                                            SunRadius = (int?)s.Attribute("sunradius") ?? 6,
                                            Sun = new Hex(
                                                (int?)s.Element("sunhex").Attribute("q") ?? 0,
                                                (int?)s.Element("sunhex").Attribute("r") ?? 0,
                                                (int?)s.Element("sunhex").Attribute("s") ?? 0
                                                ),
                                            Planets = (from t in s.Descendants("planet")
                                                       select new Planet(
                                                           name: (string)t.Attribute("name") ?? "unknownplanet",
                                                           orbitRadius: (int?)t.Attribute("orbitradius") ?? 6,
                                                           parent: new Hex(
                                                                        (int?)t.Element("parenthex").Attribute("q") ?? 0,
                                                                        (int?)t.Element("parenthex").Attribute("r") ?? 0,
                                                                        (int?)t.Element("parenthex").Attribute("s") ?? 0
                                                                        ),
                                                           numberofmoves: (int?)t.Attribute("numberofmoves") ?? 1,
                                                           color: new Color(
                                                                        1f,
                                                                        1f,
                                                                        1f,
                                                                        1f
                                                                        ), 
                                                           position: (int?)t.Attribute("currentposition") ?? 0, 
                                                           size: (float?)t.Attribute("size") ?? 1.0f, 
                                                           gravity: (float?)t.Attribute("gravity") ?? 1.0f, 
                                                           OD: (OrbitDir)((int?)t.Attribute("orbitdir") ?? 0),
                                                           rings: null,
                                                           atmosphere: new Atmosphere(
                                                                        (bool?)t.Element("atmoshpere").Attribute("breathable") ?? false
                                                                        )
                                                               )
                                                       ).ToDictionary(x=>x.Name, x=>x)

                                        }).ToDictionary(x => x.id, x => x);

        //foreach (KeyValuePair<int, SolarSystem> el in Sols)
        //{
        //    //Debug.Log(el.Key + ": " + el.Value.Name);
        //    foreach (KeyValuePair<string, Planet> p in el.Value.Planets)
        //    {
        //        //Debug.Log("      Planet: " + p.Value.Name);
        //    }
        //}

        return Sols;
    }
}




