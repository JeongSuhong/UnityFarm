using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using System.Text;

/*
 *   상점과 아이템을 관리하는 스크립트.    
 *   Create_Store_Item_UI() : 상점에 나타나는 아이템버튼을 생성.
 *   Create_Install_OBJ() : 아이템 오브젝트를 생성.
 *   Get_ItemInfo() : 매개변수로 받은 아이템ID에 해당하는 아이템의 정보를 반환. 없으면 null 반환.
 *                         
 *    
 */
public class StoreManager : MonoBehaviour
{
    private List<Item> ItemInfo = new List<Item>();

    public GameObject Store_Item_UI_Prefab;
    public UIGrid Grid_Items;


    private static StoreManager instance = null;

    public static StoreManager Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(StoreManager)) as StoreManager;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("StoreManager ");
            instance = obj.AddComponent(typeof(StoreManager)) as StoreManager;

            Debug.Log("Fail to get StoreManager Instance");
        }
        return instance;
    }

    void Awake()
    {
        instance = this;

    }

    void Create_Store_Item_UI(Item item_info)
    {
        GameObject ItemUI = Instantiate(Store_Item_UI_Prefab, Grid_Items.transform) as GameObject;
        ItemUI.name = item_info.Buliding_ID.ToString();
        ItemUI.transform.localScale = Vector3.one;
        ItemUI.name = item_info.Name.ToString();

        ItemUI.GetComponent<Store_Item_Action>().Set_Item_Info(item_info);

        Grid_Items.repositionNow = true;
    }

    public void Create_Install_OBJ(int obj_index, int buliding_id, Vector3 pos, Vector3 rot)
    {
        Item Item_Info = Get_ItemInfo(buliding_id);

        if (Item_Info == null) { return; }

        GameObject EventOBJ_Prefab = Resources.Load("Prefabs/EventOBJ/" + Item_Info.Model_Name) as GameObject;

        if (EventOBJ_Prefab == null)
        {
            Debug.Log("Prefab Error!!!");
            return;
        }

        GameObject obj = Instantiate(EventOBJ_Prefab, pos, Quaternion.identity) as GameObject;
        obj.name = EventOBJ_Prefab.name;
        Destroy(obj.GetComponent<Rigidbody>());
        obj.transform.Rotate(rot);
        BulidingOBJ_Action obj_action = obj.GetComponent<BulidingOBJ_Action>();
        obj_action.Info = Item_Info;
        obj_action.Obj_Index = obj_index;
        obj_action.Is_SaveItem = true;
        obj_action.Set_Buy();
        obj_action.Set_Install();
        obj_action.Install_Action();

    }

    public Item Get_ItemInfo(int find_id)
    {
        for (int i = 0; i < ItemInfo.Count; i++)
        {
            if (ItemInfo[i].Buliding_ID == find_id)
            {
                return ItemInfo[i];
            }
        }

        return null;
    }
    public void View_StoreUI()
    {
        GetComponent<UIPanel>().alpha = 1;
        GameManager.Get_Inctance().Set_ViewUI();
    }
    public void NotView_StroeUI()
    {
        GetComponent<UIPanel>().alpha = 0;
        GameManager.Get_Inctance().Set_NotViewUI();
    }

    public void Update_Item_Limit_UI(int buliding_id)
    {
        string name = Get_ItemInfo(buliding_id).Name;
        GameObject obj = Grid_Items.transform.FindChild(name).gameObject;

        if(obj == null) { return; }

        obj.GetComponent<Store_Item_Action>().Check_Limit(buliding_id);
    }


    // 이하는 네트워크 관련 함수

    public IEnumerator Get_DB_ItemInfo()
    {
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Get_ItemInfo");

       yield return StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_DB_ItemInfo));
    }
    public void Reply_DB_ItemInfo(string json)
    {
        // JsonReader.Deserialize() : 원하는 자료형의 json을 만들 수 있다
        Dictionary<string, object> dataDic = (Dictionary<string, object>)JsonReader.Deserialize(json, typeof(Dictionary<string, object>));

        foreach (KeyValuePair<string, object> info in dataDic)
        {
            Item data = JsonReader.Deserialize<Item>(JsonWriter.Serialize(info.Value));

            ItemInfo.Add(data);
            Create_Store_Item_UI(data);
        }
    }
}
public enum ITEM_TYPE
{
    BUILDING = 0,
    LANDSCAPING,
     CONSUMABLES,
}
public class Item
{
    public int Buliding_ID;
    public ITEM_TYPE Type;
    public string Name;
    public string Sprite_Name;
    public string Model_Name;
    public int Price;
    public int Selling_Price;
    public int Buff_Happy;

}

