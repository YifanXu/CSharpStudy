using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Threading;

namespace MUD_GTK_MONO
{
	public class Act
	{

		private const double counterAttackFactor = 0.7;
		public readonly IRoom spawn;
		private readonly string playerSave = string.Format("data{0}playerSaveFile.txt", Path.DirectorySeparatorChar);
		private Dictionary<string, Action<Player, string>> commands;
		public SubSpace tunnel;
		private bool Arrived = false;
		private Thread portalling;

		private Dictionary<string, Direction> moveCommands = new Dictionary<string, Direction>(StringComparer.OrdinalIgnoreCase){
			{"n", Direction.North },
			{"north", Direction.North },
			{"s", Direction.South },
			{"south", Direction.South },
			{"w", Direction.West },
			{"west", Direction.West },
			{"e", Direction.East },
			{"east", Direction.East }
		};

		public Act(IRoom entrance)
		{
			this.spawn = entrance;
			this.tunnel = new SubSpace();
			this.commands = new Dictionary<string, Action<Player, string>>(StringComparer.OrdinalIgnoreCase)
			{
				{"move", Move},
				{"go", Move},
				{"travel", Travel },
				{"jump", Travel },
				{"portal", Travel },
				{"pay", Pay },
				{"offer", Pay },
				{"look", Look},
				{"examine", Examine },
				{"ex", Examine },
				{"help", Help },
				{"get", Get },
				{"obtain", Get },
				{"take", Get },
				{"pickup", Get },
				{"inv", CheckInv },
				{"inventory", CheckInv },
				{"quit", Save },
				{"exit", Save },
				{"save", Save },
				{"talkto", Talk },
				{"interact", Talk },
				{"attack", Attack },
				{"assault", Attack },
				{"RepeatAttack", RepeatAttack },
				{"Bang", RepeatAttack},
				{"Exits", ShowExits}
			};
		}

		public void execute(Player player, string command, string parameter)
		{
			Action<Player, string> action;
			if(!commands.TryGetValue(command, out action))
			{
				write(ConsoleColor.Red, "Invalid Input");
			}
			else
			{
				action(player, parameter);
			}
		}

		public void Move(Player player, string parameter)
		{
			Direction dir;
			if (!moveCommands.TryGetValue(parameter, out dir))
			{
				write(ConsoleColor.Red, "Invalid Paremeter");
			}
			else
			{
				IRoom targetRoom = player.current.Move(dir);
				if (targetRoom != null)
				{
					player.current = targetRoom;
					write(ConsoleColor.Green, player.current.Name);
					Console.WriteLine(player.current.description);
					write(ConsoleColor.Yellow, "Type in your action");
				}
				else
				{
					write(ConsoleColor.Red, "You cannot move in that direction");
				}
			}
		}

		public void Travel (Player player, string parameter)
		{
			if(player.current is SubSpace)
			{
				if (!Arrived) {
					write (ConsoleColor.Cyan, "You are not ready to leave yet.");
					return;
				}
				portalling.Join ();
				Arrived = false;
				write(ConsoleColor.Green, "You successfully went through the portal.");
				player.current = player.current.desto;
				write(ConsoleColor.Green, player.current.Name);
				Console.WriteLine(player.current.description);
				write(ConsoleColor.Yellow, "Type in your action");
				return;
			}
			if(!(player.current is Portal))
			{
				write(ConsoleColor.Red, "There is no portal to go through.");
				return;
			}
			if(player.current.desto == null)
			{
				write(ConsoleColor.Cyan, "The portal seemed to be broken. It doesn't lead to anywhere.");
				return;
			}
			if (player.current.Locked)
			{
				write(ConsoleColor.Red, "The portal is unavaliable for you to go through. Pay the guardian or defeat it to access the portal.");
				return;
			}
			if(player.current.reactivation > 0)
			{
				write(ConsoleColor.Red, String.Format("The portal is unstable for you to use. Try again in {0} seconds.",player.current.reactivation));
				return;
			}
			write(ConsoleColor.Green, "You walked through the portal.");
			tunnel.desto = player.current.desto;
			player.current = tunnel;
			write(ConsoleColor.Green, player.current.Name);
			Console.WriteLine(player.current.description);
			write(ConsoleColor.Yellow, "Type in your action");
			portalling = new Thread (Moving);
			portalling.Start ();
		}

		public void Moving ()
		{
			int Time = 15;
			while (Time > 0) {
				Time--;
				Thread.Sleep (1000);
				write (ConsoleColor.Cyan, String.Format ("Moving.... {0} seconds left", Time));
			}
			Arrived = true;
			write (ConsoleColor.Green, "You have arrived. Type in 'travel' again to reenter the world.");
		}

		public void Pay ( Player player, string parameter)
		{
			int offer;
			if (int.TryParse(parameter,out offer))
			{
				string message;
				player.current.Pay(offer, out message);
				write(ConsoleColor.Cyan, message);
			}else
			{
				write(ConsoleColor.Red, "Invalid Parameter");
			}
		}

		public void Examine(Player player, string parameter)
		{
			write(ConsoleColor.Cyan, player.current.examine(parameter));
		}

		public void Look(Player player, string parameter)
		{
			write(ConsoleColor.Green, player.current.Name);
			write(ConsoleColor.White, player.current.description);
			string[] names = player.current.getNPCNames();
			if (names == null)
			{
				write(ConsoleColor.Cyan, "You are the only person here.");
			}
			else
			{
				write(ConsoleColor.Cyan, "People that are with you include: ");
				foreach (string name in names)
				{
					write(ConsoleColor.Cyan, name);
				}
			}
		}

		public void Talk(Player player, string parameter)
		{
			string dialouge = player.current.getNPCDialogue(parameter);
			if (!string.IsNullOrEmpty(dialouge))
			{
				write(ConsoleColor.Cyan, String.Format("The person says '{1}'", parameter, dialouge));
			}
		}

		public void Help(Player player, string parameter)
		{
			write(ConsoleColor.Cyan, "Avaliable commands include:");
			foreach(string command in commands.Keys)
			{
				write(ConsoleColor.Cyan, command);
			}
		}

		public void Get(Player player, string parameter)
		{
			Item thing = player.current.obtain(parameter);
			if (thing != null)
			{
				player.Inventory.Add(thing);
			}
		}

		public void CheckInv(Player player, string paremeter)
		{
			write(ConsoleColor.Green, string.Format("You player.currently have {0} hp left", player.Health));
			write(ConsoleColor.Cyan, "You inventory contains:");
			foreach (Item invItem in player.Inventory)
			{
				write(ConsoleColor.Cyan, invItem.name);
			}

		}

		public void Save(Player player, string paremeter)
		{
			player.position = player.current.ID;
			var seralizer = new XmlSerializer(typeof(Player));
			using (Stream s = new FileStream(playerSave, FileMode.Create, FileAccess.Write))
			{
				seralizer.Serialize(s, player);
			}
			write(ConsoleColor.Green, "Saved.");
			return;
		}

		public void Attack(Player player, string parameter)
		{
			NPC target = player.current.getNPC(parameter);
			if (target != null)
			{
				target.standing--;
				Damage(player, target);
				if (target.Health <= 0)
				{
					if (target == player.current.guardian)
					{
						player.current.Locked = false;
					}
					player.current.NPCs.Remove(target);
				}
				else if (player.Health <= 0)
				{
					player.DropItems(player.current);
					write(ConsoleColor.Cyan, "You have awaken at the spawnpoint.");
					player.current = spawn;
				}
			}

		}

		public void RepeatAttack(Player player, string parameter)
		{
			NPC deletedTarget = player.current.getNPC(parameter);
			if (deletedTarget != null)
			{
				while (deletedTarget.Health > 0 && player.Health > 0)
				{
					deletedTarget.standing--;
					Damage(player, deletedTarget);
					foreach (NPC angryNPC in player.current.angryNPCs)
					{
						if (angryNPC.Health >= 0)
						{
							Damage(angryNPC, player);
						}
					}
					System.Threading.Thread.Sleep(100);
				}
				if (deletedTarget.Health <= 0)
				{
					if (deletedTarget == player.current.guardian)
					{
						player.current.Locked = false;
					}
					player.current.NPCs.Remove(deletedTarget);
				}
				else if (player.Health <= 0)
				{
					player.DropItems(player.current);
					write(ConsoleColor.Cyan, "You have awaken at the spawnpoint.");
					player.current = spawn;
				}
			}
		}

		public void ShowExits(Player player, string parameter)
		{
			write(ConsoleColor.Cyan, "Exits avaliable are:");
			foreach (KeyValuePair<Direction, IRoom> pair in player.current.ConnectingRooms)
			{
				if (pair.Value != null)
				{
					write(ConsoleColor.Cyan, String.Format(" - {0} : {1}", pair.Key.ToString(), pair.Value.Name));
				}
			}

			if(player.current is Portal)
			{
				player.current.ShowTravelInfo();
			}
		}

		public static void Damage(NPC npc, Player player)
		{
			int damage = (int)((double)npc.damage * (1 - player.resistence));
			player.Health -= damage;
			write(ConsoleColor.Red, string.Format("Seeing you as an enemy, {1} suddenly attacked you for {0} damage.", damage, npc.name));
			if (player.Health > 0)
			{
				damage = (int)((double)player.damage * (1 - npc.resistence) * counterAttackFactor);
				write(ConsoleColor.Green, string.Format("You attempted an counterattck on {1} and dealt {0} damage.", damage, npc.name));
				npc.Health -= damage;
			}
			else
			{
				write(ConsoleColor.Cyan, string.Format("You died to {0}", npc.name));
			}
		}

		public static void Damage(Player player, NPC npc)
		{
			int damage = (int)((double)player.damage * (1 - npc.resistence));
			npc.Health -= damage;
			write(ConsoleColor.Green, String.Format("You dealt {0} damage to {1}", damage, npc.name));
			if (npc.Health > 0)
			{
				damage = (int)((double)npc.damage * (1 - player.resistence) * counterAttackFactor);
				player.Health -= damage;
				write(ConsoleColor.Red, string.Format("{1} counterattacked and dealt {0} damage to you.", damage, npc.name));
			}
			else
			{
				write(ConsoleColor.Green, string.Format("You successfully defeated {0}", npc.name));
			}



		}
		public static void write(ConsoleColor color, string text){
		
		}
	}
}

