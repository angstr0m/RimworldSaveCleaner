using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RimworldSaveCleaner
{
    using System.IO;
    using System.Xml;

    class Program
    {
        static void Main(string[] args)
        {
            // Load the save file.
            XmlDocument modifiedSave = new XmlDocument();
            modifiedSave.Load(args[0]);

            // Get the pawns that should be kept under all circumstances.
            var forcefullyKeptPawns = modifiedSave.DocumentElement.SelectSingleNode("game/world/worldPawns/pawnsForcefullyKeptAsWorldPawns");
            var forcefullyKeptPawnsIDs = new List<string>();
            foreach (XmlNode keptPawn in forcefullyKeptPawns.ChildNodes)
            {
                var pawnId = keptPawn.InnerText;
                var cleandedPawnId = pawnId.Replace("Thing_", "");

                forcefullyKeptPawnsIDs.Add(cleandedPawnId);
            }

            // Remove all alive pawns that are not forcefully kept
            var alivePawns = modifiedSave.DocumentElement.SelectSingleNode("game/world/worldPawns/pawnsAlive");
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

            // Remove all dead pawns that are not forcefully kept
            var deadPawns = modifiedSave.DocumentElement.SelectSingleNode("game/world/worldPawns/pawnsDead");
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

            // Save the modified file.
            Directory.CreateDirectory("./ModifiedSaves/");
            modifiedSave.Save("./ModifiedSaves/ModifiedSave.rws");
        }
    }
}
