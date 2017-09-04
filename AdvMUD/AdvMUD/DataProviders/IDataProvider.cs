using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdvMUD
{
    public interface IDataProvider
    {
        string type { get; }
        string roomDirectory { get; }
        string questDirectory { get; }
        string saveRoomDirectory { get; }
        Room GetRootRoom(string path, out Dictionary<int,Room> allRooms);
        Player GetPlayer(string path);
        Questing.Quest[] GetQuests(string path);
        void SaveRooms(string path, Room[] rooms);
    }
}
