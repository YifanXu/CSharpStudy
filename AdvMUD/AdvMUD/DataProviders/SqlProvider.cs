using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace AdvMUD.DataProviders
{
    class SqlProvider
    {
        private const string connectionString = "Server=.;Database=Testing;Trusted_Connection=True;";
        private const string roomSave = "MUDData.dbo.tblRoomData";

        public static void SaveRooms (Dictionary<int,Room> rooms)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                int affected = 0;
                foreach(Room room in rooms.Values)
                {
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = String.Format("INSERT INTO {0} ([Id],[Name],[Desc]) VALUES ('{1}','{2}','{3}')",roomSave,room.id,room.name,room.desc);
                        //command.CommandText = "INSERT INTO @databaseName ([Id],[Name],[Desc]) VALUES (@id,@name,@desc)";
                        command.CommandType = CommandType.Text;
                        /*command.Parameters.AddWithValue("@databaseName", roomSave);
                        command.Parameters.AddWithValue("@id", room.id);
                        command.Parameters.AddWithValue("@name", room.name);
                        command.Parameters.AddWithValue("@desc", room.desc);*/

                        affected += command.ExecuteNonQuery();
                        Console.WriteLine("{0} rows affected", affected);
                    }
                }
            }
        }
    }
}
