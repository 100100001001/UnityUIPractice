using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0) // 자식이 없다면
        {
            // 지금 드래그하고 있는 아이템인 draggingItem이 이 스크립트를 가지고 있는 오브젝트 자식으로 들어감
            Drag.draggingItem.transform.SetParent(this.transform);

            // draggingItem에 있는 ItemInfo의 itemData을 가져와 item에 할당해줌
            Item item = Drag.draggingItem.GetComponent<ItemInfo>().itemData;
            GameManager.instance.AddItem(item);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
    }
}
