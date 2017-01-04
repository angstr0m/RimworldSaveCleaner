﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RimworldSaveCleaner
{
    using System.Xml;

    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument modifiedSave = new XmlDocument();
            modifiedSave.Load("./Saves/Graton.rws");

            var forcefullyKeptPawns = modifiedSave.DocumentElement.SelectSingleNode("game/world/worldPawns/pawnsForcefullyKeptAsWorldPawns");
            var alivePawns = modifiedSave.DocumentElement.SelectSingleNode("game/world/worldPawns/pawnsAlive");
            var deadPawns = modifiedSave.DocumentElement.SelectSingleNode("game/world/worldPawns/pawnsDead");

            var forcefullyKeptPawnsIDs = new List<string>();
            foreach (XmlNode keptPawn in forcefullyKeptPawns.ChildNodes)
            {
                var pawnId = keptPawn.InnerText;
                var cleandedPawnId = pawnId.Replace("Thing_", "");

                forcefullyKeptPawnsIDs.Add(cleandedPawnId);
            }
            
            var alivePawnsToRemove = new List<XmlNode>();
            foreach (XmlNode alivePawn in alivePawns.ChildNodes)
            {
                var pawnId = alivePawn.SelectSingleNode("id").InnerText;

                if (!forcefullyKeptPawnsIDs.Contains(pawnId))
                {
                    alivePawnsToRemove.Add(alivePawn);
                }
            }
            foreach (var xmlNode in alivePawnsToRemove)
            {
                alivePawns.RemoveChild(xmlNode);
            }

            var deadPawnsToRemove = new List<XmlNode>();
            foreach (XmlNode deadPawn in deadPawns.ChildNodes)
            {
                var pawnId = deadPawn.SelectSingleNode("id").InnerText;

                if (!forcefullyKeptPawnsIDs.Contains(pawnId))
                {
                    deadPawnsToRemove.Add(deadPawn);
                }
            }
            foreach (var xmlNode in deadPawnsToRemove)
            {
                deadPawns.RemoveChild(xmlNode);
            }

            modifiedSave.Save("./Saves/ModifiedSave.rws");

            Console.WriteLine(forcefullyKeptPawnsIDs);
        }
    }
}
