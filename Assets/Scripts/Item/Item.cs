using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Auto,   // 자동
    Manual  // 수동
}

[Serializable]
public class Item
{
    public string Rcode;
    public ItemType Type;
}
