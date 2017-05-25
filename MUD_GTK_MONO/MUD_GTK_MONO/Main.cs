using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MUD_GTK_MONO
{

	class MainClass
	{
		public static Player player;
		public static List<RunningNPC> Runningboys = new List<RunningNPC>();
		public string savePath = "data" + Path.DirectorySeparatorChar + "saveFile";
		public const double counterAttackFactor = 0.7;
		static void OldMain(string[] args)
		{
			Thread runNPC = new Thread(MoveNPC);
			var path = new Paths();

			//Load Files?
			write(ConsoleColor.Green, "new or load?");
			string input = Console.ReadLine();
			while (string.IsNullOrEmpty(input) || (input != "new" && input != "load")) {
				write(ConsoleColor.Red, "Invalid Input");
				input = Console.ReadLine();     
			}
			if(input == "new")
			{
				player = new Player();
			}else
			{
				XmlSerializer seralizer = new XmlSerializer(typeof(Player));
				using (Stream s = new FileStream(path.playerSave, FileMode.Open, FileAccess.Read))
				{
					player = (Player) seralizer.Deserialize(s);
				}
			}
			IRoom entrance = ReadFile(path.defaultMap,player.position);
			player.current = entrance;
			Act action = new Act(entrance);
			runNPC.Start();

			write(ConsoleColor.Green, player.current.Name);
			Console.WriteLine(player.current.description);

			write(ConsoleColor.Yellow, "Type in your action");



			while (true)
			{
				input = Console.ReadLine();
				string command = input.Split(' ')[0];
				string parameter = string.Empty;
				if (command.Length != input.Length)
				{
					parameter = input.Substring(command.Length + 1);
				}
				action.execute(player, command, parameter);

				foreach(NPC angryNPC in player.current.angryNPCs)
				{
					Act.Damage(angryNPC, player);
					if (angryNPC.Health <= 0)
					{
						player.current.NPCs.Remove(angryNPC);
						continue;
					}
					else if (player.Health <= 0)
					{
						player.DropItems(player.current);
						write(ConsoleColor.Cyan, "You have awaken at the spawnpoint.");
						player.current = entrance;
					}
				}
			}
		}

		public static void write (ConsoleColor color, string text)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(text);
			Console.ResetColor();
		}

		public static void MoveNPC()
		{
			Random r = new Random();
			while (true)
			{
				foreach(RunningNPC entity in Runningboys)
				{
					lock (entity)
					{
						if (r.Next(100 / entity.stamina) == 0)
						{
							for (int i = 0; i < 50; i++)
							{
								Direction movingDirection = (Direction)r.Next(4);
								if (entity.currentRoom.ConnectingRooms[movingDirection] != null)
								{
									IRoom destoRoom = entity.currentRoom.ConnectingRooms[movingDirection];
									if (player.current == entity.currentRoom)
									{
										write(ConsoleColor.Yellow, String.Format("{0} has entered the area.", entity.name));
									}

									destoRoom.NPCs.Add(entity);
									entity.currentRoom.NPCs.Remove(entity);
									entity.currentRoom = destoRoom;


									if (player.current == entity.currentRoom)
									{
										write(ConsoleColor.Yellow, String.Format("{0} has left the area.", entity.name));
									}
									break;
								}
							}
						}
					}
				}
				Thread.Sleep(2000);
			}
		}

		public static IRoom ReadFile(string path, int position)
		{
			player.current = new Room();
			if (!File.Exists(path))
			{
				return null;
			}
			List<IRoom> rooms = new List<IRoom>();
			int line = 0;
			string[] input = File.ReadAllLines(path);

			while(line < input.Length && input[line] != "END")
			{
				//Add Room
				string[] parameters = input[line].Split('|');
				if (parameters[0] == "P")
				{
					if (input[line + 1].StartsWith("G"))
					{
						line++;
						parameters = input[line].Split('|');
						NPC guardian = new NPC(parameters[2], parameters[3], int.Parse(parameters[4]), int.Parse(parameters[5]));
						line--;
						parameters = input[line].Split('|');
						if (int.Parse(parameters[4]) == -1)
						{
							guardian.standing = -3;
						}
						rooms.Add(new Portal(parameters[2], parameters[3], int.Parse(parameters[4]), guardian));
						line++;
					}
					else
					{
						rooms.Add(new Portal(parameters[2], parameters[3]));
					}
				}
				else
				{
					rooms.Add(new Room(parameters[2], parameters[3]));
				}
				line++;
				//Add Items
				while(input[line][0] == 'I')
				{
					parameters = input[line].Split('|');
					rooms[rooms.Count - 1].Objects.Add(new Item(parameters[2], parameters[3], parameters[5]));
					Item currentItem = rooms[rooms.Count - 1].Objects[Int32.Parse(parameters[1])];
					if (parameters[4] == "Y")
					{
						currentItem.obtainable = true;
						currentItem.roomDescription = parameters[6];
					}
					else
					{
						currentItem.obtainable = false;
					}
					line++;
				}
				while(input[line][0] == 'N' || input[line][0] == 'M')
				{
					parameters = input[line].Split('|');
					if (input[line].StartsWith("M"))
					{
						RunningNPC runningboi = new RunningNPC(parameters[2], parameters[3], parameters[4], int.Parse(parameters[5]), rooms[rooms.Count - 1]);
						rooms[rooms.Count - 1].NPCs.Add(runningboi);
						Runningboys.Add(runningboi);
					}
					else
					{
						if (parameters.Length == 7)
						{
							rooms[rooms.Count - 1].NPCs.Add(new NPC(parameters[2], parameters[3], parameters[4], int.Parse(parameters[5]), int.Parse(parameters[6])));
						}
						else
						{
							rooms[rooms.Count - 1].NPCs.Add(new NPC(parameters[2], parameters[3], parameters[4]));
						}
					}
					line++;
				}
			}
			line++;
			while (line < input.Length && !input[line].StartsWith("P"))
			{
				string[] parameters = input[line].Split('|');
				int originRoom = int.Parse(parameters[0]);
				int destoRoom = int.Parse(parameters[2]);
				switch (parameters[1])
				{
				case "E":
					rooms[originRoom].ConnectingRooms[Direction.East] = rooms[destoRoom];
					break;
				case "W":
					rooms[originRoom].ConnectingRooms[Direction.West] = rooms[destoRoom];
					break;
				case "N":
					rooms[originRoom].ConnectingRooms[Direction.North] = rooms[destoRoom];
					break;
				case "S":
					rooms[originRoom].ConnectingRooms[Direction.South] = rooms[destoRoom];
					break;
				}
				line++;
			}
			while (line < input.Length)
			{
				string[] parameters = input[line].Split('|');
				rooms[int.Parse(parameters[1])].desto = rooms[int.Parse(parameters[2])];
				line++;
			}
			player.current = rooms[0];
			for(int i = 0; i < rooms.Count; i++)
			{
				if(i == position)
				{
					player.current = rooms[i];
				}
				rooms[i].ID = i;
			}
			return rooms[0];
		}


	}
}
