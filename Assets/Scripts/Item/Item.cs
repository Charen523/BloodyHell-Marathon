using System;

public enum ItemType
{
    Auto,  
    Manual 
}

[Serializable]
public class Item
{
    public string Rcode;
    public ItemType type;
}
