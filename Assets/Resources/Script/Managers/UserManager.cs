using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

public class UserManager : MonoBehaviour {

    private Dictionary<int, int> CropInven = new Dictionary<int, int>();
    private Dictionary<int, int> ItemInven = new Dictionary<int, int>();

    private int Level;
    private float Exp;
    private int Gold = 1000;
    private int Jam = 100;
    public int Max_House;

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

        GameManager.Get_Inctance().Start_Update();

        Get_DB_UserData();
        Get_DB_Install_Buliding();
    }

    public void Obtain_Crop(int CropID, int count)
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


    }
    public int Get_Crop_Count(int CropID)
    {
        if(CropInven.ContainsKey(CropID))
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
        UIManager.Get_Inctance().Label_Level.text = Level.ToString();
        Exp = data.Exp;
        UIManager.Get_Inctance().Slider_Exp.value = Exp;
        Gold = data.Gold;
        UIManager.Get_Inctance().Label_Gold.text = Gold.ToString();
        Jam = data.Jam;
        UIManager.Get_Inctance().Label_Jam.text = Jam.ToString();
        Max_House = data.Max_House;
        //test
        UIManager.Get_Inctance().Label_House.text = " 0 / " + Max_House.ToString();
    }

    public void Set_DB_Install_Buliding(BulidingOBJ_Action buliding_action, GameObject obj)
    {
        int index = GameManager.Get_Inctance().Get_UserIndex();
        Get_Buliding_Action = buliding_action;

        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Set_InstallOBJ");

        sendData.Add("user_index", index);
        sendData.Add("buliding_id", buliding_action.Info.Buliding_ID);
        sendData.Add("pos_x", obj.transform.position.x);
        sendData.Add("pos_y", obj.transform.position.y);
        sendData.Add("pos_z", obj.transform.position.z);
        sendData.Add("rot_x", obj.transform.rotation.eulerAngles.x);
        sendData.Add("rot_y", obj.transform.rotation.eulerAngles.y);
        sendData.Add("rot_z", obj.transform.rotation.eulerAngles.z);
        sendData.Add("check_install", buliding_action.Info.Check_Install);


        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Set_DB_Install_Buliding));
    }
    void Reply_Set_DB_Install_Buliding(string json)
    {
        if (JsonReader.Deserialize<string>(json) == null) { return; }

        int obj_index = JsonReader.Deserialize<int>(json);

        if(Get_Buliding_Action != null)
        {
            Get_Buliding_Action.Info.Obj_Index = obj_index;
        }
    }

    void Get_DB_Install_Buliding()
    {
        int index = GameManager.Get_Inctance().Get_UserIndex();

        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Get_InstallOBJ");

        sendData.Add("user_index", index);

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Get_DB_Install_Buliding));
    }
    void Reply_Get_DB_Install_Buliding(string json)
    {
        if (JsonReader.Deserialize<string>(json) == null) { return; }

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

    private class RecvUserData
    {
        public int Level;
        public float Exp;
        public int Gold;
        public int Jam;
        public int Max_House;
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