using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

public class UserManager : MonoBehaviour {

    private Dictionary<int, int> CropInven = new Dictionary<int, int>();
    private Dictionary<int, int> ItemInven = new Dictionary<int, int>();
    public Dictionary<Item, Vector3> Building_Inven = new Dictionary<Item, Vector3>();

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
        Get_UserData();
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

    public void Obtain_Building(Item building , Vector3 pos)
    {
        Building_Inven.Add(building, pos);
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
        // Mathf.Sign()을 쓰면 음수일때는 -, 양수일때는 +로 계산이 된다.
        while (true)
        {
            text_gold = int.Parse(UIManager.Get_Inctance().Label_Gold.text);
            text_gold += Time.deltaTime * 100f;

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


    /////// Network //////

    public void Get_UserData()
    {
        int index = GameManager.Get_Inctance().Get_UserIndex();
       
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Get_UserData");

        sendData.Add("user_index", index);

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Get_UserData));
    }
    void Reply_Get_UserData(string json)
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


}
public class RecvUserData
{
    public int Level;
    public float Exp;
    public int Gold;
    public int Jam;
    public int Max_House;
}
