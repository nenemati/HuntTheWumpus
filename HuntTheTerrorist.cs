using System;
using System.Collections.Generic;

namespace HuntTheTerrorist
{
	class HuntTheTerrorist
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Time to kill some terrorists!");

			PlayHuntTheTerrorist();

		}

		public static void PlayHuntTheTerrorist()
		{
			//Creating the network of rooms.
			Dictionary<int, int[]> roomMap = CreateRoomMap();

			//Creating a player, set of terrorists, and a hostage.
			Player player1 = DropPlayerinRoom(roomMap);
			Terrorist[] terrorists = DropTerroristsInRooms();		
			Hostage hostage = DropHostageInRoom(terrorists);			
			
			//Player is prompted for a next action.
			ChooseAction(player1, terrorists, hostage, roomMap);

		}

		public static Dictionary<int, int[]> CreateRoomMap()
		{
			Dictionary<int, int[]> roomMap = new Dictionary<int, int[]>();

			int[] Room1 = {2, 5, 3};
			roomMap.Add(1, Room1);

			int[] Room2 = {1, 10, 3};
			roomMap.Add(2, Room2);

			int[] Room3 = {2, 4, 12};
			roomMap.Add(3, Room3);

			int[] Room4 = {3, 5, 14};
			roomMap.Add(4, Room4);

			int[] Room5 = {1, 10, 3};
			roomMap.Add(5, Room5);

			int[] Room6 = {5, 7, 15};
			roomMap.Add(6, Room6);

			int[] Room7 = {6, 8, 17};
			roomMap.Add(7, Room7);

			int[] Room8 = {1, 7, 9};
			roomMap.Add(8, Room8);

			int[] Room9 = {8, 10, 18};
			roomMap.Add(9, Room9);

			int[] Room10 = {2, 9, 11};
			roomMap.Add(10, Room10);

			int[] Room12 = {3, 11, 13};
			roomMap.Add(12, Room12);

			int[] Room13 = {12, 14, 20};
			roomMap.Add(13, Room13);

			int[] Room14 = {4, 13, 15};
			roomMap.Add(14, Room14);

			int[] Room15 = {6, 14, 16};
			roomMap.Add(15, Room15);

			int[] Room16 = {15, 17, 20};
			roomMap.Add(16, Room16);

			int[] Room17 = {7, 16, 18};
			roomMap.Add(17, Room17);

			int[] Room18 = {9, 17, 19};
			roomMap.Add(18, Room18);

			int[] Room19 = {11, 18, 20};
			roomMap.Add(19, Room19);

			int[] Room20 = {13, 16, 19};
			roomMap.Add(20, Room20);

			return roomMap;
		}

		//This puts the player in one of the outer rooms. 
		public static Player DropPlayerinRoom(Dictionary<int, int[]> roomMap)
		{
			Player player = new Player();

			//Player starts from any room from 1 - 5 (outermost rooms)
			Random p = new Random();
			player.playerRoomNumber = p.Next(1,6);

			Console.WriteLine("You have been dropped into Room {0}.", player.playerRoomNumber);
			AnnounceAdjacentRooms(player.playerRoomNumber, roomMap);

			return player;
		}

		//This creates a set of terrorists and defines which rooms they are in.
		public static Terrorist[] DropTerroristsInRooms()
		{
			Terrorist terrorist1 = new Terrorist();
			Terrorist terrorist2 = new Terrorist();
			Terrorist terrorist3 = new Terrorist();
			Terrorist terrorist4 = new Terrorist();
			Terrorist terrorist5 = new Terrorist();

			Random terroristRoomNumGen = new Random();
			terrorist1.terroristRoomNumber = terroristRoomNumGen.Next(6,9);
			terrorist2.terroristRoomNumber = terroristRoomNumGen.Next(9,12);
			terrorist3.terroristRoomNumber = terroristRoomNumGen.Next(12,15);
			terrorist4.terroristRoomNumber = terroristRoomNumGen.Next(15,18);
			terrorist5.terroristRoomNumber = terroristRoomNumGen.Next(18,21);

			Terrorist[] terrorists = {terrorist1, terrorist2, terrorist3, terrorist4, terrorist5};
			return terrorists;
		}

		public static Hostage DropHostageInRoom(Terrorist[] terrorists)
		{
			Hostage hostage = new Hostage();

			Random hostageRoomOptionsGen = new Random();
			hostage.hostageRoomNumber = terrorists[hostageRoomOptionsGen.Next(1,5)].terroristRoomNumber;

			return hostage;
		}		

		public static void AnnounceAdjacentRooms(int roomNumber, Dictionary<int, int[]> roomMap)
		{
			int[] adjacentRooms = roomMap[roomNumber];

			//Printing where the player can move to next and how many arrows they have.
			Console.WriteLine("Tunnels lead to rooms {0}, {1}, and {2}.\n", adjacentRooms[0], adjacentRooms[1], adjacentRooms[2]);
		}

		public static void ChooseAction(Player player1, Terrorist[] terrorists, Hostage hostage, Dictionary<int, int[]> roomMap)
		{
			//While the player has not been killed by the terrorist, 
			//the player is prompted for another action.
			while (player1.IsPlayerAlive() == true)
			{
				//Ask the player what they want to do. 
				Console.WriteLine("Shoot (S), throw a grenade (G), throw a flashbang (F), move (M), or quit (Q): ");
				string input = Console.ReadLine();
				string playerChoice = input.Trim().ToUpper();

				//If player wants to shoot an arrow, they lose an arrow.
				if (playerChoice == "S")
				{
					player1.ShootBullets();
					Console.WriteLine("\nYou have {0} bullets.", player1.GetPlayerBullets());
				}
				else if (playerChoice == "G")
				{
					player1.ThrowGrenades();
					Console.WriteLine("You have {0} grenades.", player1.GetPlayerGrenades());
				}
				else if (playerChoice == "F")
				{
					player1.ThrowFlashbangs();
					Console.WriteLine("You have {0} flashbangs.", player1.GetPlayerFlashbangs());
				}
				//If player wants to move, they are prompted for which room and are moved there.
				else if (playerChoice == "M")
				{
					bool roomExists = false;
					int[] adjacentRooms = roomMap[player1.playerRoomNumber];
					while (!roomExists)
					{
						Console.WriteLine("Which room would you like to move to?");
						int roomNumber = int.Parse(Console.ReadLine());

						for (int i=0; i<3; i++)
						{
							if (roomNumber == adjacentRooms[i])
							{
								roomExists = true;
								player1.alive = EnterRoom(player1, roomNumber, terrorists, hostage, roomMap);
								if (player1.alive == true)
								{
									CheckAdjacentRooms(player1, terrorists, hostage, roomMap);
								}
							}
						}
					}
				}
				else if (playerChoice == "Q")
				{
					player1.alive = false;
				}
				else
				{
					Console.WriteLine("Invalid entry");
				}
			}

		}

		public static bool EnterRoom(Player player, int roomNumber, Terrorist[] terrorists, Hostage hostage, Dictionary<int, int[]> roomMap)
		{
			player.playerRoomNumber = roomNumber;
			Console.WriteLine("--------------------------------------------------------------\nYou have now entered Room {0}.", player.GetPlayerRoomNumber());

			player.alive = CheckForTerrorists(player, terrorists);

			if (player.alive == false)
			{
				return player.alive;
			}
			else
			{
				player.alive = CheckForHostage(player, hostage);

				if (player.alive == false)
				{
					return player.alive;
				}
				else
				{
					AnnounceAdjacentRooms(player.playerRoomNumber, roomMap);
					Console.WriteLine("You have {0} bullets, {1} flashbangs, and {2} grenades.\n", player.GetPlayerBullets(), player.GetPlayerFlashbangs(), player.GetPlayerGrenades());
				}
			}

			return player.alive;
		}

		public static void CheckAdjacentRooms(Player player, Terrorist[] terrorists, Hostage hostage, Dictionary<int, int[]> roomMap)
		{
			int[] adjacentRooms = roomMap[player.playerRoomNumber];

			for (int i=0; i<adjacentRooms.Length; i++)
			{
				for (int j=0; j<terrorists.Length; j++)
				{
					if (adjacentRooms[i] == terrorists[j].terroristRoomNumber)
					{
						Console.WriteLine("Terrorist in an adjacent room!");
					}
				}

				if (adjacentRooms[i] == hostage.hostageRoomNumber)
				{
					Console.WriteLine("Hostage in an adjacent room!");
				}
			}
		}

		public static bool CheckForTerrorists(Player player, Terrorist[] terrorists)
		{
			for (int i=0; i<terrorists.Length; i++)
			{
				if (terrorists[i].terroristRoomNumber == player.playerRoomNumber)
				{
					Console.WriteLine("Killed by a terrorist!\nBetter luck next time!");
					player.alive = false;
				}
			}

			return player.alive;
		}

		public static bool CheckForHostage(Player player, Hostage hostage)
		{
			if (player.playerRoomNumber == hostage.hostageRoomNumber)
			{
				Console.WriteLine("You found the hostage!\nCongratulations, you win!!");
				player.alive = false;
			}

			return player.alive;
		}

	}
}





// Fix sensory functions to utilize dictionaries
//associate terrorist and hostage room numbers with combat actions
//is player alive test
//are npc's alive
//are npc's blinded test