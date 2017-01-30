using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

/*
 *   유저와 관련된걸 관리하는 스크립트.
 *    Obtain_Crop() : 작물을 획득했을때 호출. CropWarehouse_UI_Action의 UI호출.
 *    Get_Crop_Count() : 해당 작물을 몇개나 소지하고 있는지 반환. 해당 작물을 소지하고 있지 않을경우 -99반환
 *    Check_Obtain_Crop() : 해당 작물을 소유하고 있는지 아닌지 반환.
 *    Sell_Crop() : 작물을 판매. 해당 작물의 count를 감소하고 골드를 얻음.
 *    Increase_Gold() : 골드를 획득했을때 실행.
 *    C_Increase_Gold() : 골드를 획득했을때 골드 Label 에 뜨는 코드애니메이션.
 *    Increase_Jam() : Jam을 획득했을때 실행.
 *    Buy_OBJ() : 아이템을 구매했을때 실행. 구매한 아이템의 정보를 바꿈.
 */

public class UserManager : MonoBehaviour {

    private Dictionary<int, int> CropInven = new Dictionary<int, int>();               // Crop_ID | Count
    private Dictionary<int, int> ItemInven = new Dictionary<int, int>();                // Buliding_ID | Count
    private List<BulidingOBJ_Action> ItemOBJs = new List<BulidingOBJ_Action>();

    public int Level;
    public float Exp;
    private int Gold = 1000;
    public int Jam = 100;
    public int Max_House;
    public int House_Count;
    public int Max_Farm;
    public int Farm_Count;

    private static UserManager instance = null;

    public static UserManager Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(UserManager)) as UserManager;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("UserManager ");
            instance = obj.AddComponent(typeof(UserManager)) as UserManager;

            Debug.Log("Fail to get UserManager Instance");
        }
        return instance;
    }

    void Awake()
    {
        instance = this;
    }
    public void Obtain_Crop(int CropID, int count, int crop_exp)
    {
        Set_DB_Crop_Data(CropID, count);

        Increase_EXP(crop_exp);

    }
    public int Get_Crop_Count(int CropID)
    {
        if(Check_Obtain_Crop(CropID))
        {
            return CropInven[CropID];
        }
        else
        {
            return -99;
        }
    }
    public bool Check_Obtain_Crop(int CropID)
    {
        if(CropInven.ContainsKey(CropID))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Set_Gold(int value)
    {
        if (value < Gold)
        {
            UIManager.Get_Inctance().Set_MinusGold_UI(Gold - value);
            UIManager.Get_Inctance().Label_Gold.text = value.ToString();
        }
        else
        {
            StartCoroutine(C_Set_Gold(value));
        }

        Gold = value;
    }
    IEnumerator C_Set_Gold(int value)
    {
        float text_gold = 0;
        while (true)
        {
            if(text_gold <= value)
            {
                UIManager.Get_Inctance().Label_Gold.text = value.ToString();
                yield break;
            }

            text_gold = int.Parse(UIManager.Get_Inctance().Label_Gold.text);
            text_gold += Time.deltaTime * 300f;

            if (text_gold >= Gold)
            {
                UIManager.Get_Inctance().Label_Gold.text = Gold.ToString();
                break;
            }
           

            UIManager.Get_Inctance().Label_Gold.text = ((int)text_gold).ToString();

            yield return null;

        }
    }
    public int Get_Gold()
    {
        return Gold;
    }

    public void Increase_Jam(int value)
    {
        Jam += value;
        UIManager.Get_Inctance().Label_Jam.text = Jam.ToString();
    }

    public void Buy_OBJ(GameObject Select_OBJ)
    {
        GameObject obj = Instantiate(Select_OBJ, Select_OBJ.transform.position, Select_OBJ.transform.rotation) as GameObject;
        obj.name = Select_OBJ.name;
        Destroy(obj.GetComponent<Rigidbody>());
        BulidingOBJ_Action obj_action = obj.GetComponent<BulidingOBJ_Action>();

        obj_action.Info = Select_OBJ.GetComponent<BulidingOBJ_Action>().Info;
        obj_action.Is_SaveItem = true;
        obj_action.Set_Buy();
        obj_action.Set_Install();

        Set_Limit_InstallOBJ(obj_action.Info.Buliding_ID);

        obj_action.gameObject.SetActive(true);
        Select_OBJ.gameObject.SetActive(false);
        obj_action.Install_Action();

        Add_InstallOBJ(obj_action);
    }

    public void Increase_House_Count(int  value)
    {
        House_Count += value;
        UIManager.Get_Inctance().Set_HouseCount_UI(House_Count, Max_House);
        StoreManager.Get_Inctance().Update_Item_Limit_UI((int)LIMIT_CHECK_OBJ.HOUSE);
    }
    public bool Check_Install_House()
    {
        if(House_Count >= Max_House)
        {
            return false;
        }

        return true;
    }

    public void Increase_Farm_Count(int value)
    {
        Farm_Count += value;
        StoreManager.Get_Inctance().Update_Item_Limit_UI((int)LIMIT_CHECK_OBJ.FARM);
    }
    public bool Check_Install_Farm()
    {
        if (Farm_Count >= Max_Farm)
        {
            return false;
        }

        return true;
    }

    public void Increase_EXP(int value)
    {
        Update_DB_UserExp(value);
    }

    public bool Check_Inven_InstallOBJ(int Buldling_ID)
    {
        return ItemInven.ContainsKey(Buldling_ID);
    }

    public void Set_Limit_InstallOBJ(int Buliding_index)
    {
        switch ((LIMIT_CHECK_OBJ)Buliding_index)
        {
            case LIMIT_CHECK_OBJ.FARM:
                {
                    Increase_Farm_Count(1);
                    break;
                }
            case LIMIT_CHECK_OBJ.HOUSE:
                {
                    Increase_House_Count(1);
                    break;
                }
            default: { break; }
        }

    }

    public void Add_InstallOBJ(BulidingOBJ_Action obj_action)
    {
        if(ItemOBJs.Contains(obj_action)) { return; }

        ItemOBJs.Add(obj_action);
    }
    public void Delete_InstallOBJ(int obj_index, int buliding_obj)
    {
        for(int i = 0; i < ItemOBJs.Count; i++)
        {
           if( ItemOBJs[i].Obj_Index == obj_index && ItemOBJs[i].Info.Buliding_ID == buliding_obj )
            {
                if(ItemOBJs[i].Info.Buliding_ID == (int)LIMIT_CHECK_OBJ.HOUSE)
                {
                    Increase_House_Count(-1);
                }
                else if(ItemOBJs[i].Info.Buliding_ID == (int)LIMIT_CHECK_OBJ.FARM)
                {
                    Increase_Farm_Count(-1);
                }

                Destroy(ItemOBJs[i].gameObject);
                ItemOBJs.RemoveAt(i);

                return;
            }
        }
    }
    /////// Network //////

    public IEnumerator Get_DB_UserData()
    {
        int index = GameManager.Get_Inctance().Get_UserIndex();
       
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Get_UserData");

        sendData.Add("user_index", index);

        yield return StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Get_DB_UserData));
    }
    void Reply_Get_DB_UserData(string json)
    {
        // JSON Data 변환
        RecvUserData data = JsonReader.Deserialize<RecvUserData>(json);

        Level = data.Level;
        Exp = data.Exp;
        Gold = data.Gold;
        Jam = data.Jam;
        Max_House = data.Max_House;
        Max_Farm = data.Max_Farm;

        UIManager.Get_Inctance().Set_UserInfo(data);
        

    }

    public IEnumerator Get_DB_Install_Buliding()
    {
        int index = GameManager.Get_Inctance().Get_UserIndex();

        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Get_User_InstallOBJData");

        sendData.Add("user_index", index);

        yield return StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Get_DB_Install_Buliding));
    }
    void Reply_Get_DB_Install_Buliding(string json)
    {
        Dictionary<string, object> dataDic = (Dictionary<string, object>)JsonReader.Deserialize(json, typeof(Dictionary<string, object>));

        foreach (KeyValuePair<string, object> info in dataDic)
        {
            RecvInstallObjData data = JsonReader.Deserialize<RecvInstallObjData>(JsonWriter.Serialize(info.Value));

            if (!ItemInven.ContainsKey(data.Buliding_ID))
            {
                ItemInven.Add(data.Buliding_ID, 1);
            }
            else
            {
                ItemInven[data.Buliding_ID] += 1;
            }

            if (data.Check_Install)
            {
                Vector3 Pos = new Vector3(data.Pos_x, data.Pos_y, data.Pos_z);
                Vector3 Rot = new Vector3(0f, data.Rot_y, 0f);

                StoreManager.Get_Inctance().Create_Install_OBJ(data.Obj_Index, data.Buliding_ID, Pos, Rot);
                Set_Limit_InstallOBJ(data.Buliding_ID);
                StoreManager.Get_Inctance().Update_Item_Limit_UI(data.Buliding_ID);
            }
            else
            {
                Inventory_UI_Action.Get_Inctance().Create_ItemButton(StoreManager.Get_Inctance().Get_ItemInfo(data.Buliding_ID), data.Obj_Index, null);
            }
        }
    }

    private GameObject Get_Buliding_OBJ; // 아래 코드에서 쓰는 매개변수 ( Reply로 매개변수 보내는 방법을 모르겟다 )
    public void Set_DB_Install_Buliding(BulidingOBJ_Action buliding_action, GameObject obj)
    {
        int index = GameManager.Get_Inctance().Get_UserIndex();
        Get_Buliding_OBJ = obj;

        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Set_User_InstallOBJData");

        sendData.Add("user_index", index);
        sendData.Add("buliding_id", buliding_action.Info.Buliding_ID);
        sendData.Add("pos_x", obj.transform.position.x);
        sendData.Add("pos_y", obj.transform.position.y);
        sendData.Add("pos_z", obj.transform.position.z);
        sendData.Add("rot_y", obj.transform.rotation.eulerAngles.y);
        sendData.Add("check_install", buliding_action.Is_Install);
        sendData.Add("check_buy", buliding_action.Check_Buy_Item);
        sendData.Add("obj_index", buliding_action.Obj_Index);

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Set_DB_Install_Buliding));
    }
    void Reply_Set_DB_Install_Buliding(string json)
    {
        Dictionary<string, int> data = (Dictionary < string, int> )JsonReader.Deserialize<Dictionary<string, int>>(json);
        BulidingOBJ_Action action = Get_Buliding_OBJ.GetComponent<BulidingOBJ_Action>();

        // 아이템을 구매해서 설치한 경우
        if (data["user_gold"] != Gold)
        {
            action.Obj_Index = data["obj_index"];
            Buy_OBJ(Get_Buliding_OBJ);
        }
        // 보관한 아이템을 설치하는 경우
        else if(action.Check_Buy_Item && action.Is_Install)
        {
            Get_Buliding_OBJ.SetActive(true);
            action.Is_Install = true;
            Get_Buliding_OBJ.GetComponent<BulidingOBJ_Action>().Install_Action();
            return;
        }
        

        Set_Gold(data["user_gold"]);
    }

    public void Update_DB_UserExp(int value)
    {
        int index = GameManager.Get_Inctance().Get_UserIndex();

        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Update_User_Exp");

        sendData.Add("user_index", index);
        sendData.Add("value", value);

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Update_DB_UserExp));
    }
    void Reply_Update_DB_UserExp(string json)
    {
        int[] data = (int[])JsonReader.Deserialize(json, typeof(int[]));

        Level = data[0];
        Exp = data[1];

        UIManager.Get_Inctance().Set_ExpGrid(Exp / (Level * 100f));
        UIManager.Get_Inctance().Set_Level(Level);
    }

    void Set_DB_Crop_Data(int crop_id, int count)
    {
        int index = GameManager.Get_Inctance().Get_UserIndex();
  
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Set_User_Crop_Data");

        sendData.Add("user_index", index);
        sendData.Add("crop_id", crop_id);
        sendData.Add("count", count);

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Set_DB_Crop_Data));
    }
    void Reply_Set_DB_Crop_Data(string json)
    {
        RecvUserCropData data = (RecvUserCropData)JsonReader.Deserialize(json, typeof(RecvUserCropData));

        if (!CropInven.ContainsKey(data.Crop_ID))
        {
            CropInven.Add(data.Crop_ID, data.Crop_Count);
        }
        else
        {
            CropInven[data.Crop_ID] = data.Crop_Count;
        }

        CropWarehouse_UI_Action.Get_Inctance().Set_Crop_UI(data.Crop_ID);
    }

    public IEnumerator Get_DB_Crop_Data()
    {
        int index = GameManager.Get_Inctance().Get_UserIndex();

        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Get_User_Crop_Data");

        sendData.Add("user_index", index);

        yield return StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Get_DB_Crop_Data));
    } 
    void Reply_Get_DB_Crop_Data(string json)
    {
        Dictionary<string, object> dataDic = (Dictionary<string, object>)JsonReader.Deserialize(json, typeof(Dictionary<string, object>));

        foreach (KeyValuePair<string, object> info in dataDic)
        {
            RecvUserCropData data = JsonReader.Deserialize<RecvUserCropData>(JsonWriter.Serialize(info.Value));

            CropInven.Add(data.Crop_ID, data.Crop_Count);

            CropWarehouse_UI_Action.Get_Inctance().Set_Crop_UI(data.Crop_ID);
        }
    }

    public void Set_DB_Sell_Crop(int crop_id, int crop_count)
    {
        int index = GameManager.Get_Inctance().Get_UserIndex();

        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Set_Sell_Crop");

        sendData.Add("user_index", index);
        sendData.Add("crop_id", crop_id);
        sendData.Add("crop_count", crop_count);

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Set_DB_Sell_Crop));
    }
    void Reply_Set_DB_Sell_Crop(string json)
    {
        int[] data = (int[])JsonReader.Deserialize(json, typeof(int[]));

        int gold = data[0];
        int crop_id = data[1];
        int crop_count = data[2];

        CropInven[crop_id] -= crop_count;

        if (CropInven[crop_id] <= 0)
        {
            CropInven.Remove(crop_id);
        }

        CropWarehouse_UI_Action.Get_Inctance().Set_Crop_UI(crop_id);
        Set_Gold(gold);


    }

    public void Update_DB_User_Gold(int value)
    {
        int index = GameManager.Get_Inctance().Get_UserIndex();

        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Update_User_Gold");

        sendData.Add("user_index", index);
        sendData.Add("gold_value", value);

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Update_DB_User_Gold));
    }
    void Reply_Update_DB_User_Gold(string json)
    {
        int gold = (int)JsonReader.Deserialize(json, typeof(int));

        Set_Gold(gold);
    }

    public void Update_DB_User_Storage_OBJ(int Item_ID, int Buliding_ID)
    {
        int index = GameManager.Get_Inctance().Get_UserIndex();

        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Update_Storage_UserItem");

        sendData.Add("user_index", index);
        sendData.Add("item_id", Item_ID);
        sendData.Add("buliding_id", Buliding_ID);

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Update_DB_User_Storage_DB));
    }
    void Reply_Update_DB_User_Storage_DB(string json)
    {
        int[] datas = (int[])JsonReader.Deserialize(json, typeof(int[]));
        
        for(int i = 0; i < ItemOBJs.Count; i++)
        {
            if (ItemOBJs[i].Obj_Index == datas[0] && ItemOBJs[i].Info.Buliding_ID == datas[1] )
            {
                ItemOBJs[i].Is_Install = false;
                ItemOBJs[i].gameObject.SetActive(false);
                Inventory_UI_Action.Get_Inctance().Create_ItemButton(ItemOBJs[i].Info, ItemOBJs[i].Obj_Index, ItemOBJs[i].gameObject);

                return;
            }
        }
    }
    //// TEST
    public void Update_1000_User_Gold()
    {
        Update_DB_User_Gold(1000);
    }

    private class RecvInstallObjData
    {
        public int Obj_Index;
        public int Buliding_ID;
        public float Pos_x;
        public float Pos_y;
        public float Pos_z;
        public float Rot_y;
        public bool Check_Install;
    }
    private class RecvUserCropData
    {
        public int Crop_ID;
        public int Crop_Count;
    }
}
public class RecvUserData
{
    public int Level;
    public float Exp;
    public int Gold;
    public int Jam;
    public int Max_House;
    public int Max_Farm;
}