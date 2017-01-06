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

    private Dictionary<int, int> CropInven = new Dictionary<int, int>();
    private Dictionary<int, int> ItemInven = new Dictionary<int, int>();

    public int Level;
    private float Exp;
    private int Gold = 1000;
    private int Jam = 100;
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
        Get_DB_UserData();
        GameManager.Get_Inctance().Start_Update();

        instance = this;
    }
    public void Obtain_Crop(int CropID, int count, int crop_exp)
    {
        if (!CropInven.ContainsKey(CropID))
        {
            CropInven.Add(CropID, count);
        }
        else
        {
            CropInven[CropID] += count;
        }

        CropWarehouse_UI_Action.Get_Inctance().Set_Crop_UI(CropID);
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

    public void Sell_Crop(int count, int crop_id)
    {
        if(count <= 0 ) { return; }

        int sell_value = CropsManager.Get_Inctance().Get_CropInfo(crop_id).Selling_Price * count;

        CropInven[crop_id] -= count;

        if (CropInven[crop_id] <= 0)
        {
            CropInven.Remove(crop_id);
        }

        CropWarehouse_UI_Action.Get_Inctance().Set_Crop_UI(crop_id);

        Increase_Gold(sell_value);


    }

    public void Increase_Gold(int value)
    {
        Gold += value;

        if (value < 0)
        {
            UIManager.Get_Inctance().Set_MinusGold_UI(value);
            UIManager.Get_Inctance().Label_Gold.text = Gold.ToString();
        }
        else
        {
            StartCoroutine(C_Increase_Gold(value));
        }
    }
    IEnumerator C_Increase_Gold(int value)
    {
        float text_gold = 0;
        while (true)
        {
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
    public void Increase_Jam(int value)
    {
        Jam += value;
        UIManager.Get_Inctance().Label_Jam.text = Jam.ToString();
    }

    public void Buy_OBJ(GameObject Select_OBJ, Item Select_OBJ_Info)
    {
        GameObject obj = Instantiate(Select_OBJ, Select_OBJ.transform.position, Select_OBJ.transform.rotation) as GameObject;
        obj.name = Select_OBJ.name;
        Destroy(obj.GetComponent<Rigidbody>());
        BulidingOBJ_Action obj_action = obj.GetComponent<BulidingOBJ_Action>();

        obj_action.Is_SaveItem = true;
        obj_action.Is_Install = true;
        obj_action.Info = Select_OBJ_Info;
        Get_Buliding_Action = obj_action;

        Increase_Gold(-obj_action.Info.Price);
        Set_DB_Install_Buliding(obj_action, obj);
    }

    public void Increase_House_Count()
    {
        House_Count++;
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

    public void Increase_Farm_Count()
    {
        Farm_Count++;
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

    private BulidingOBJ_Action Get_Buliding_Action;


    /////// Network //////

    void Get_DB_UserData()
    {
        int index = GameManager.Get_Inctance().Get_UserIndex();
       
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Get_UserData");

        sendData.Add("user_index", index);

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Get_DB_UserData));
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

    public void Set_DB_Install_Buliding(BulidingOBJ_Action buliding_action, GameObject obj)
    {
        int index = GameManager.Get_Inctance().Get_UserIndex();
        Get_Buliding_Action = buliding_action;

        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Set_User_InstallOBJData");

        sendData.Add("user_index", index);
        sendData.Add("buliding_id", buliding_action.Info.Buliding_ID);
        sendData.Add("pos_x", obj.transform.position.x);
        sendData.Add("pos_y", obj.transform.position.y);
        sendData.Add("pos_z", obj.transform.position.z);
        sendData.Add("rot_x", obj.transform.rotation.eulerAngles.x);
        sendData.Add("rot_y", obj.transform.rotation.eulerAngles.y);
        sendData.Add("rot_z", obj.transform.rotation.eulerAngles.z);
        sendData.Add("check_install", buliding_action.Check_Is_Install);

        Get_Buliding_Action = buliding_action;


        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Set_DB_Install_Buliding));
    }
    void Reply_Set_DB_Install_Buliding(string json)
    {
        int obj_index = JsonReader.Deserialize<int>(json);

        Get_Buliding_Action.Obj_Index = obj_index;
        Get_Buliding_Action.Install_Action();
    }

    public void Get_DB_Install_Buliding()
    {
        int index = GameManager.Get_Inctance().Get_UserIndex();

        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Get_User_InstallOBJData");

        sendData.Add("user_index", index);

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Get_DB_Install_Buliding));
    }
    void Reply_Get_DB_Install_Buliding(string json)
    {
        Dictionary<string, object> dataDic = (Dictionary<string, object>)JsonReader.Deserialize(json, typeof(Dictionary<string, object>));

        foreach (KeyValuePair<string, object> info in dataDic)
        {
            RecvInstallObjData data = JsonReader.Deserialize<RecvInstallObjData>(JsonWriter.Serialize(info.Value));
            Vector3 Pos = new Vector3(data.Pos_x, data.Pos_y, data.Pos_z);
            Vector3 Rot = new Vector3(data.Rot_x, data.Rot_y, data.Rot_z);

            if (data.Check_Install)
            {
                StoreManager.Get_Inctance().Create_Install_OBJ(data.Obj_Index, data.Buliding_ID, Pos, Rot);
            }
        }
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

    private class RecvInstallObjData
    {
        public int Obj_Index;
        public int Buliding_ID;
        public float Pos_x;
        public float Pos_y;
        public float Pos_z;
        public float Rot_x;
        public float Rot_y;
        public float Rot_z;
        public bool Check_Install;
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