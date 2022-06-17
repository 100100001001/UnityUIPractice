using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // 싱글톤
    void Awake()
    {
        if (instance == null) instance = this;
    }

    // 인벤토리에서 아이템을 장착했을 때
    public void AddItem(Item item)
    {
        switch (item.type)
        {
            case Item.ItemType.SP:
                Debug.Log("공격력 증가 +" + item.value);
                break;
            case Item.ItemType.MP:
                Debug.Log("방어력 증가 +" + item.value);
                break;
            case Item.ItemType.HP:
                Debug.Log("체력 증가 +" + item.value);
                break;
            default:
                break;
        }
    }

    // 인벤토리에서 아이템을 뺐을 때
    public void RemoveItem(Item item)
    {
        switch (item.type)
        {
            case Item.ItemType.SP:
                Debug.Log("공격력 감소 -" + item.value);
                break;
            case Item.ItemType.MP:
                Debug.Log("방어력 감소 -" + item.value);
                break;
            case Item.ItemType.HP:
                Debug.Log("체력 감소 -" + item.value);
                break;
            default:
                break;
        }
    }
}
