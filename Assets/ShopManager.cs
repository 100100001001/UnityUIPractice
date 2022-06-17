using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

// JSON화 할 수 있는 클래스
public class JsonList
{
    public List<ShopItem> target;
    // 생성자
    public JsonList(List<ShopItem> _target) => target = _target;
}


// 사용자 정의 자료형
[System.Serializable]
public class ShopItem
{
    public string type, name, explain, price, index;
    public bool isUsing;
    // 생성자 -> 초기값
    public ShopItem(string _type, string _name, string _explain, string _price, bool _isUsing, string _index)
    {
        type = _type;
        name = _name;
        explain = _explain;
        price = _price;
        isUsing = _isUsing;
        index = _index;
    }
}

public class ShopManager : MonoBehaviour
{
    public TextAsset itemDB;

    // 데이터를 받아둔 리스트
    [Header("---List---")]
    public List<ShopItem> allItemList = new List<ShopItem>();  // 상점의 모든 정보 리스트 (전체 데이터 베이스)
    public List<ShopItem> myItemList = new List<ShopItem>();   // 사용자의 아이템 리스트
    public List<ShopItem> currItemList = new List<ShopItem>(); // 캐릭터 탭을 선택하면 캐릭터만, 풍선 탭을 선택하면 풍선만 나오도록 리스트

    // UI 관련
    [Header("---UI---")]
    // 상점 슬롯
    public string curType = "Character"; // 기본값을 캐릭터로 해야하기 때문에 설정
    public GameObject[] slots;           // 슬롯들을 담아둘 배열
    public Sprite[] itemImages;          // 아이템 이미지들을 받아둘 배열

    // 버튼 이미지 변경
    public Image[] tabBtnImages;                     // 버튼 이미지 변경을 위한 이미지 배열
    public Sprite btnSelectSprite, btnIdleSprite;    // 버튼을 선택했을 때 이미지, 기본 버튼 이미지

    // Save, Load
    string filePath; // 파일의 위치

    void Start()
    {
        // Save
        //Debug.Log(Application.persistentDataPath);
        filePath = Application.persistentDataPath + "/myItemText.txt"; // myItemText.txt 이름으로 저장


        // enter(\n)를 기준으로 잘라준다 / 배열 생성 
        string[] lines = itemDB.text.Split('\n');
        //Debug.Log(lines[0]); // 결과 값 : Character	Pig	돼지	1	FALSE	0
        foreach (var line in lines)
        {
            // 탭(\t) 기준으로 잘라준다
            string[] rows = line.Split('\t');
            // item을 새로 만들어주고
            ShopItem item = new ShopItem(rows[0], rows[1], rows[2], rows[3], rows[4] == "TRUE", rows[5]);
            // allItemList에 item을 추가해준다
            allItemList.Add(item);
        }

        TabClick(curType);
        Load();
    }

    // 본인의 컴퓨터에 텍스트 타입으로 저장하는 메서드
    void Save()
    {
        // myItemList를 JsonList 클래스로 포장!
        JsonList tmp = new JsonList(myItemList);
        // myItemList를 JSON화
        string jData = JsonUtility.ToJson(tmp);
        // 파일 저장
        File.WriteAllText(filePath, jData);
    }

    // 저장되어 있는 텍스트 타입을 불러오는 메서드
    void Load()
    {
        // filePath에 저장된 파일이 없으면 실행하지 않음
        if (!File.Exists(filePath)) return;

        // 저장된 파일을 불러 옴
        string jData = File.ReadAllText(filePath);
        myItemList = JsonUtility.FromJson<JsonList>(jData).target; // FromJson은 제네릭 타입 -> 자료형을 맞춰줘야 함
    }

    public void SlotClick(int slotNum)
    {
        //Debug.Log(currItemList[slotNum].name);

        // 보유하지 않은 Item/보유하고 있는 Item
        ShopItem curItem = myItemList.Find(x => x.name == currItemList[slotNum].name);

        // 보유하지 않은 Item
        if (curItem == null)
        {
            myItemList.Add(currItemList[slotNum]);
            Debug.Log("구매 가능한 아이템입니다.");
        }
        // 보유하고 있는 Item
        else
        {
            Debug.Log("소유하고 있는 아이템입니다.");
        }

        Save();
    }

    public void TabClick(string tabName)
    {
        //Debug.Log(tabName);
        curType = tabName; // curType을 tabName으로 바꿔준다

        // allItemList에서 하나씩 꺼내서 type이 curType과 같은 걸 찾아준다
        // FindAll 을 찾아가며 알아서 반복한다! 
        currItemList = allItemList.FindAll(x => x.type == curType);

        /*
        // 정석은 이것!
        // 근데 이거는 Add를 하니 계속 추가만 되기 때문에 foreach문 시작 전 currItemList를 다 clear시킴
        currItemList.Clear();
        foreach (var item in allItemList)
        {
            if (item.type == curType)
            {
                currItemList.Add(item);
            }
        }
        */

        for (int i = 0; i < slots.Length; i++) // 슬롯의 갯수까지 for문을 돌도록 한다
        {
            bool isExist = i < currItemList.Count;
            slots[i].SetActive(isExist);

            if (isExist)
            {
                // slots[i]의 자식에 있는 NameText를 가져오자
                // 자식 중에는 text가 하나있기 때문에 GetComponentInChildren만 적어줘도 됨
                // 만약 자식 중 텍스트가 여러개 존재한다면 transform.GetChild(몇 번째 자식?).GetComponent

                // 첫번째 자식에서 Text 컴포넌트를 가져오고, 그 안의 text를 currItemList[i]번째의 이름으로 바꿔 줌
                slots[i].transform.GetChild(0).GetComponent<Text>().text = currItemList[i].name;

                // 두번째 자식에서 Image 컴포넌트를 가져오고, 그 안의 sprite를 currItemList[i].index의 이미지로 바꿔 줌
                int index = int.Parse(currItemList[i].index);
                slots[i].transform.GetChild(1).GetComponent<Image>().sprite = itemImages[index];

            }
        }

        // 탭 버튼 이미지 변경
        int tabNum = 0;

        switch (tabName)
        {
            case "Character":
                tabNum = 0;
                break;
            case "Balloon":
                tabNum = 1;
                break;
            default:
                break;
        }

        for (int i = 0; i < tabBtnImages.Length; i++)
        {
            // 선택했을 땐 btnSelectSprite로 변경
            if (i == tabNum) tabBtnImages[i].sprite = btnSelectSprite;
            else tabBtnImages[i].sprite = btnIdleSprite;
        }
    }

}

