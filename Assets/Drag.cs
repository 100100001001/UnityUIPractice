using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public static GameObject draggingItem = null; // 드래그하는 아이템
    // static! 전용 공간! 클래스 멤버를 만들고 다이렉트로 접근 가능

    Transform inventoryTr; // 인벤토리
    Transform itemTr;      // 아이템의 transform 변수 선언
    Transform itemListTr;
    CanvasGroup itemCG;    // 아이템의 CanvasGroup

    void Start()
    {
        itemTr = GetComponent<Transform>();
        itemCG = GetComponent<CanvasGroup>();

        var inventory = GameObject.Find("Inventory"); // var 임시 변수
        //inventoryTr = inventory.GetComponent<Transform>();
        inventoryTr = inventory.transform; // GetComponent로 가져와도 되고 이렇게도 써도 된다

        var itemList = GameObject.Find("ItemList");
        itemListTr = itemList.transform;

    }
    // UI 관련이기 때문에 Update()는 필요 없다

    // 외부에서 호출 되어야하기 때문에 public 선언
    // 드래그 중일 때 호출하는 메서드
    public void OnDrag(PointerEventData eventData)
    {
        // print("OnDrag");
        itemTr.position = Input.mousePosition; // 아이템의 위치가 마우스의 위치를 따라 감
    }

    // 드래그를 시작했을 때 호출하는 메서드
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 분가 시켜주자
        // parent(프로퍼티) 또는 SetParent(메서드)를 써도 됨
        //itemTr.parent = inventoryTr; // inventoryTr가 itemTr의 부모
        itemTr.SetParent(inventoryTr);

        draggingItem = gameObject; // 이 스크립트를 가지고 있는 나 자신을 draggingItem에 넣어줌
        //Debug.Log(draggingItem.name);
        itemCG.blocksRaycasts = false;
    }

    // 드래그가 끝났을 때 호출하는 메서드
    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem = null; // 드래그가 끝났기 때문에 draggingItem를 null로 만들어 줌
        itemCG.blocksRaycasts = true;

        // 아이템의 부모를 비교
        // 아이템이 허공에 있다면 부모가 Inventory임
        if (itemTr.parent == inventoryTr)
        {
            itemTr.SetParent(itemListTr);
            
            Item item = GetComponent<ItemInfo>().itemData;
            GameManager.instance.RemoveItem(item);
        }
    }
}
