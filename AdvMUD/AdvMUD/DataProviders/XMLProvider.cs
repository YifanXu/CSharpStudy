using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using AdvMUD.Entities;
using AdvMUD.Questing;

namespace AdvMUD.DataProviders
{
    public class XMLProvider : IDataProvider
    {
        public string roomDirectory { get { return "loadingFiles/XmlRoomFile.xml"; } }
        public string questDirectory { get { return "loadingFiles/Quests.xml"; } }
        public string saveRoomDirectory { get { return "../../loadingFiles/RoomSave.xml"; } }
        public string type
        {
            get
            {
                return "xml";
            }
        }
        public Room GetRootRoom (string path, out Dictionary<int,Room>roomDict)
        {
            Room[] rooms;
            XmlSerializer seralizer = new XmlSerializer(typeof(Room[]));
            using (Stream s = new FileStream(path,FileMode.Open,FileAccess.Read))
            {
                rooms = (Room[]) seralizer.Deserialize(s);
            }
            roomDict = new Dictionary<int, Room>();
            foreach(Room room in rooms)
            {
                roomDict.Add(room.id, room);
                foreach(NPC npc in room.npcs)
                {
                    npc.Location = room;
                }
            }
            foreach (Room room in roomDict.Values)
            {
                if (room.connections != null)
                {
                    foreach (RoomConnection connection in room.connections)
                    {
                        connection.ValidateConnection(roomDict);
                    }
                }
            }
            return roomDict[0];
        }

        public void SaveRooms (string path, Room[] rooms)
        {
            XmlSerializer seralizer = new XmlSerializer(typeof(Room[]));
            using (Stream s = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                seralizer.Serialize(s, rooms);
            }
        }

        private static T ConvertNode<T>(XmlNode node) where T : class
        {
            MemoryStream stm = new MemoryStream();
            StreamWriter stw = new StreamWriter(stm);
            stw.Write(node.OuterXml);
            stw.Flush();

            stm.Position = 0;

            XmlSerializer s = new XmlSerializer(typeof(T));
            T result = (T)(s.Deserialize(stm));

            return result;
        }

        public Player GetPlayer (string path)
        {
            return new Player();
        }

        public Quest[] GetQuests(string path)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Quest[]));
            using (Stream s = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return ser.Deserialize(s) as Quest[];
            }
        }


        private XmlNode FindNodeByName(XmlNodeList list, string name)
        {
            foreach (XmlNode node in list)
            {
                if (String.Equals(name, node.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return node;
                }
            }
            return null;
        }

    }
}
