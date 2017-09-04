using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.IO;
using AdvMUD.Entities;
using AdvMUD.Questing;

namespace AdvMUD.DataProviders
{
    public class JsonProvider : IDataProvider
    {
        public string type {
            get
            {
                return "json";
            }
        }
        public string roomDirectory { get { return "loadingFiles/JsonRoomFile.json"; } }
        public string questDirectory { get { return "loadingFiles/Quests.json"; } }
        public string saveRoomDirectory { get { return "../../loadingFiles/RoomSave.json"; } }
        public Room GetRootRoom (string path, out Dictionary<int,Room> roomDict)
        {
            Room[] rooms = JsonDeserialize<Room[]>(path);
            roomDict = new Dictionary<int, Room>();
            foreach (Room room in rooms)
            {
                roomDict.Add(room.id, room);
                foreach (NPC npc in room.npcs)
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

        public Quest[] GetQuests(string path)
        {
            return JsonDeserialize<Quest[]>(path);
        }

        public Player GetPlayer(string path)
        {
            return JsonDeserialize<Player>(path);
        }

        public void SaveRooms(string path, Room[] rooms)
        {
            JsonSerialize(rooms, path);
        }


        public static void JsonSerialize(object o, string path)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            StringBuilder str = new StringBuilder();
            serializer.Serialize(o, str);
            File.WriteAllText(path, str.ToString());
        }

        public static T JsonDeserialize<T>(string path)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return (T)(serializer.Deserialize(File.ReadAllText(path), typeof(T)));
        }
    }
}
