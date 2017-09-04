using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvMUD.DataProviders;
using System.Xml.Serialization;
using System.IO;

namespace AdvMUD
{
    class Program
    {
        static void Main(string[] args)
        {
            XMLProvider xmlprovider = new XMLProvider();
            JsonProvider jsonprovider = new JsonProvider();
            Game game = new Game(jsonprovider, "../../loadingFiles/JsonRoomFile.json");
            SqlProvider.SaveRooms(game.allRooms);

            //Dictionary<int, Room> rooms;
            //provider.GetRootRoom(provider.roomDirectory, out rooms);
            //Room[] roomArray = rooms.Values.ToArray();
            //foreach(Room room in roomArray)
            //{
            //    if (room.connections != null)
            //    {
            //        foreach(RoomConnection connection in room.connections)
            //        {
            //            connection.targetRoom = null;
            //        }
            //    }
            //    if (room.npcs != null)
            //    {
            //        foreach (Entities.NPC npc in room.npcs)
            //        {
            //            npc.Location = null;
            //        }
            //    }
            //}
            //JsonProvider.JsonSerialize(roomArray, json.saveRoomDirectory);
            //Questing.Quest[] quests = Player.player.quests;
            //JsonProvider.JsonSerialize(quests, jsonprovider.questDirectory);

                                    game.Run();

            Console.ReadLine();
        }
    }
}
