using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

class Player
{
    // fields
    private int health;

    public int Health
    {
        get { return health; }
    }
    public Inventory backpack;

    // properties
    public Room CurrentRoom { get; set; }

    // constructor
    public Player()
    {
        CurrentRoom = null;
        health = 50;
        backpack = new Inventory(25);
    }

    // methods
    public void Damage(int amount)
    {
        health -= amount;

        if (health < 0){
            health = 0;
        }
    }

    public bool TakeFromChest(string itemname)
    {
        Item pickup = CurrentRoom.Chest.Get(itemname);

        if (pickup == null)
        {
            Console.WriteLine($"There is no item {itemname}");
            return false;
        }

        //onderzoeken waarom dit zo kan!!!
        if (backpack.Put(itemname, pickup))
        {
            
            Console.WriteLine("you picked the item");
            return true;
        }
        else
        {
            Console.WriteLine($"{itemname} does not fit in your backpack.");
            return false;
        }
    }

    public bool DropToChest(string itemname)
    {
        Item pickup = backpack.Get(itemname);

        if (pickup == null)
        {
            Console.WriteLine($"There is no item {itemname} in your backpack");
            return false;
        }

        CurrentRoom.Chest.Put(itemname, pickup);

        return true;
    }

    public void Heal(int amount)
    {
        health += amount;
    }

    public bool IsAlive()
    {
        return health > 0;
    }
}