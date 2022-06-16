using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자 정의 자료형
[System.Serializable] // 직렬화
public class Item
{
    public enum ItemType { SP, MP, HP }

    public ItemType type;
    public string name;
    public int value;
}

public class ItemInfo : MonoBehaviour
{
    public Item itemData;
}
