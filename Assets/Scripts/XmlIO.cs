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
        XElement XmlGalaxy = new XElement("Galaxy",
                    new XElement("SolarSystems")
                );
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
                            new XAttribute("lastposition", p.Value.LastPosition),
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
                                )
                            )
                        )
                    )
                );

            XmlGalaxy.Add(XmlSolarsystem);
            
        }
        doc = new XDocument(
                XmlGalaxy
            );

        doc.Save(Application.dataPath + "/XMLdata/GalaxyData.xml");
    }
    public void ReadXmlFile()
    {
        XDocument doc = XDocument.Load("Test.xml");
        List<XElement> test = (from e in doc.Elements()
                                select e).ToList<XElement>();

        foreach (XElement el in test)
        {
            //
        }
    }
}

    


