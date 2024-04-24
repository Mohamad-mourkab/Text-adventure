using System;
using System.ComponentModel;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;
	// private object itemname;

	//onderstaande fiels (slots) niet gebruiken...
	public Room officeSlot;
	public Room hallwaySlot;
	public Room outsideSlot;
	public Room gateSlot;

	// Constructor
	public Game()
	{
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms
		Room outside = new Room("outside the main entrance of the university");
		Room gate = new Room("outside the university gate");
		Room theatre = new Room("in a lecture theatre.");
		Room hallway = new Room("in a long hallway filled with paintings.");
		Room lab = new Room("in a computing lab.");
		Room office = new Room("in the computing admin office");
		Room swimmingpool = new Room("in the male swimming section");
		Room gym = new Room("in the muscle training area");
		Room armory = new Room("in a room full of weapons for display");
		Room cafe = new Room("in a room where you can order coffie");
		Room classroom = new Room("in a room full of tables.");
		Room dressingroom = new Room("in a room where you change clothes");




		// Initialise room exits
		outside.AddExit("east", theatre);
		outside.AddExit("south", gate);
		outside.AddExit("west", hallway);
		outside.AddExit("up", gym);

		hallway.AddExit("north", office);
		hallway.AddExit("east", outside);
		hallway.AddExit("west", theatre);
		hallway.AddExit("south", armory);

		theatre.AddExit("east", hallway);
		theatre.AddExit("west", outside);
		theatre.AddExit("south", cafe);

		cafe.AddExit("north", theatre);

		office.AddExit("south", hallway);
		office.AddExit("east", classroom);


		armory.AddExit("north", hallway);
		armory.AddExit("west", lab);

		lab.AddExit("east", armory);

		classroom.AddExit("west", office);


		gym.AddExit("east", swimmingpool);
		gym.AddExit("down", outside);

		swimmingpool.AddExit("west", gym);
		swimmingpool.AddExit("east", dressingroom);

		dressingroom.AddExit("west", swimmingpool);



		officeSlot = office;
		gateSlot = gate;
		hallwaySlot = hallway;
		outsideSlot = outside;


		// Create your Items here
		Item potion = new Item(8, "a very weird looking potion.");
		Item juice = new Item(5, "a beautiful oranje drink.");
		Item bluekey = new Item(7, "an old blue rusty key.");
		Item oldshoes = new Item(4, "a very very very rusty shoes.");
		Item medkit = new Item(9, "very useful for reparing wounds.");
		Item banana = new Item(5, "healthy and delious food that somehow still fresh.");
		Item redkey = new Item(9, "a redkey shinny key");


		// And add them to the Rooms
		// ...
		theatre.Chest.Put("potion", potion);
		dressingroom.Chest.Put("medkit", medkit);
		classroom.Chest.Put("redkey", redkey);
		lab.Chest.Put("bluekey", bluekey);
		classroom.Chest.Put("banana", banana);
		armory.Chest.Put("oldshoes", oldshoes);
		cafe.Chest.Put("juice", juice);


		// Start game outside
		player.CurrentRoom = outside;
		officeSlot.IsLocked();
		gateSlot.IsLocked();
	}

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		while (!finished)
		{
			Command command = parser.GetCommand();
			finished = ProcessCommand(command);
			if (player.CurrentRoom == gateSlot && !player.IsAlive())
			{
				Console.WriteLine("You have died while trying to go to the nearest hospital. Bruhh you need to look out for your health...");

				finished = true;
			}
			if (player.CurrentRoom != gateSlot && !player.IsAlive())
			{
				Console.WriteLine("You have died. surely it's not that hard to navigate around a university right?...");
				finished = true;
			}
			if (player.CurrentRoom == gateSlot && player.IsAlive())
			{
				Console.WriteLine("You have left the university. you have won the game hahahhaha!");
				finished = true;
			}

		}
		Console.WriteLine("Thank you for playing this very very amazing text adventure game.");
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
	}

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to Zuul!");
		Console.WriteLine("Zuul is a new, incredibly amazing adventure game where you and yes, only you are trapped inside this very very amazing university. You wil have to through the hole university till you manage to find a way to Escape.");
		Console.WriteLine("If you need any help during your gameplay. Just type[help].");
		Console.WriteLine();
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}

	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;

		if (command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit; // false
		}

		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				break;
			case "look":
				LookRoom();
				break;
			case "quit":
				wantToQuit = true;
				break;
			case "take":
				Take(command);
				break;
			case "drop":
				Drop(command);
				break;
			case "use":
				Use(command);
				break;
		}

		return wantToQuit;
	}

	// ######################################
	// implementations of user commands:
	// ######################################

	// Print out some help information.
	// Here we print the mission and a list of the command words.

	private void LookRoom()
	{
		var Chest = player.CurrentRoom.Chest.GetInfo();
		var Backpack = player.backpack.GetInfo();

		Console.WriteLine(player.CurrentRoom.GetLongDescription());
		Console.Write("Chest: ");
		foreach (var item in Chest)
		{
			Console.Write(item.Key);

			if (item.Key != Chest.Keys.Last())
				Console.Write(", ");

		}
		Console.WriteLine();

		Console.Write("Backpack: ");
		foreach (var item in Backpack)
		{
			Console.Write(item.Key);

			if (item.Key != Backpack.Keys.Last())
				Console.Write(", ");

		}
		Console.WriteLine();

	}

	public void Use(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Type use and the item name + direction to excute");
			return;
		}


		if (command.SecondWord == "bluekey")
		{
			if (command.ThirdWord == "north")
			{

				if (player.CurrentRoom == hallwaySlot)
				{
					Console.WriteLine("You used the bluekey. the north room is unlocked");
					officeSlot.locked = false;
				}
				else
				{
					Console.WriteLine("you cant use the bluekey here");
					return;
				}
			}
			else
			{
				Console.WriteLine("you cant use the bluekey here");
				return;
			}
		}




		if (command.SecondWord == "redkey")
		{
			if (command.ThirdWord == "south")
			{

				if (player.CurrentRoom == outsideSlot)
				{
					Console.WriteLine("You used the redkey. the south room is unlocked");
					gateSlot.locked = false;
				}
				else
				{
					Console.WriteLine("you cant use the redkey here");
					return;
				}
			}
			else
			{
				Console.WriteLine("you cant use the redkey here");
				return;
			}
		}





		Item item = player.backpack.Get(command.SecondWord);

		if (item == null)
		{
			Console.WriteLine($"You have no {command.SecondWord} or you cant use {command.SecondWord} here");
			return;
		}

		// ============================

		else if (command.SecondWord == "medkit")
		{
			player.Heal(40);
			Console.WriteLine("You feel much better");
			Console.WriteLine("");

			Console.WriteLine($"your health is: {player.Health}");

		}
		else if (command.SecondWord == "banana")
		{
			player.Heal(15);
			Console.WriteLine("You feel a bit better");
			Console.WriteLine("");

			Console.WriteLine($"your health is: {player.Health}");

		}
		else if (command.SecondWord == "potion")
		{
			Console.WriteLine("You drinked the potion, u feel very sick");
			player.Damage(10);
			Console.WriteLine("");

			Console.WriteLine($"your health is: {player.Health}");

		}
		else if (command.SecondWord == "oldshoes")
		{
			Console.WriteLine("You tried to wear the oldshoes. But you suddenly got hurt while trying to put them on.");
			player.Damage(5);
			Console.WriteLine("");

			Console.WriteLine($"your health is: {player.Health}");
		}


	}

	private void Take(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("type take and the item name to excute");
			return;
		}

		bool success = player.TakeFromChest(command.SecondWord);

		string itemname = command.SecondWord;
		if (!success)
		{
			Console.WriteLine($"you cant take {itemname}");
			return;
		}

		Console.WriteLine($"you have picked {itemname}");
	}

	private void Drop(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("type Drop and the item name to excute");
			return;
		}

		bool success = player.DropToChest(command.SecondWord);
		string itemname = command.SecondWord;
		if (!success)
		{
			Console.WriteLine($"you cant drop {itemname}");
			return;
		}

		Console.WriteLine($"you have dropped {itemname}");
	}

	private void PrintHelp()
	{
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You wander around at the university.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}

	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	private void GoRoom(Command command)
	{
		if (!command.HasSecondWord())
		{
			// if there is no second word, we don't know where to go...
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to " + direction + "!");
			return;
		}
		else if (nextRoom.locked == true)
		{
			Console.WriteLine("This room is locked.");
			return;
		}

		player.CurrentRoom = nextRoom;
		player.Damage(5);

		Console.WriteLine(player.CurrentRoom.GetLongDescription());
		Console.WriteLine($"your health is: {player.Health}");
	}
}
