using System;

public enum ItemType
{
    Auto,  // �ڵ�
    Manual // ����
}

[Serializable]
public class Item
{
    public string Rcode;
    public ItemType type;
}
