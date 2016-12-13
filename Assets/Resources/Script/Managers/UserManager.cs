using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserManager : MonoBehaviour {

    private Dictionary<int, int> CropInven = new Dictionary<int, int>();
    private Dictionary<int, int> ItemInven = new Dictionary<int, int>();

    private int Gold = 1000;
    public UILabel Label_Gold;
    private int Jam = 100;
    public UILabel Label_Jam;

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
        Label_Gold.text = Gold.ToString();
        Label_Jam.text = Jam.ToString();
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

        Gold += sell_value;
        Label_Gold.text = Gold.ToString();
        
    }

    public void Increase_Gold(int value)
    {
        Gold += value;
        Label_Gold.text = Gold.ToString();
    }
    public void Increase_Jam(int value)
    {
        Jam += value;
        Label_Jam.text = Jam.ToString();
    }
}
    
