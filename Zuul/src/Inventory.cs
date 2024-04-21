class Inventory
{
    //fields
    private int maxWeight;
    private Dictionary<string, Item> items;

    //constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.items = new Dictionary<string, Item>();
    }

    //methods
    public bool Put(string itemName, Item item)
    {
        if (item.Weight <= FreeWeight())
        {
            items.Add(itemName, item);
            return true;
        }
        return false;
    }

    public Item Get(string itemname)
    {
        if (items.ContainsKey(itemname))
        {
            Item item = items[itemname];
            items.Remove(itemname);
            return item;
        }

        return null;
    }
    public int TotalWeight()
    {
        int total = 0;

        foreach (var kvp in items)
        {
            total += kvp.Value.Weight;
        }

        return total;
    }

    public int FreeWeight()
    {
        return maxWeight - TotalWeight();
    }

    public Dictionary<string, Item> GetInfo()
    {
        return items;
    }

}
