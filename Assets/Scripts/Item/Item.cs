using System;

public enum ItemType
{
    Auto,  // 자동 사용
    Manual // 수동 사용
}

[Serializable]
public class Item
{
    public string Rcode;
    public ItemType type;
}
