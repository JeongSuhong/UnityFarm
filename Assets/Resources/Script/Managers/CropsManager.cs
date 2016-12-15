using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CropsManager : MonoBehaviour
{ 
    Dictionary<int, CropInfo> CropsInfo = new Dictionary<int, CropInfo>();

    public UIGrid Grid_Select_Crops;
    public GameObject Select_Crop_UI_Prefab;

    private static CropsManager instance = null;

    public static CropsManager Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(CropsManager)) as CropsManager;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("CropsManager ");
            instance = obj.AddComponent(typeof(CropsManager)) as CropsManager;

            Debug.Log("Fail to get CropsManager Instance");
        }
        return instance;
    }

    void Awake()
    {
        instance = this;

        CropInfo testInfo = new CropInfo();
        testInfo.ID = 3;
        testInfo.Is_Farmming = true;
        testInfo.Name = "사탕무";
        testInfo.SpriteName = "radish";
        testInfo.Grow_Time = 5;
        testInfo.Selling_Price = 30;
        testInfo.Price = 10;

        CropsInfo.Add(testInfo.ID, testInfo);

        testInfo.ID = 0;
        testInfo.Is_Farmming = false;
        testInfo.Name = "나뭇잎";
        testInfo.SpriteName = "leaf";
        testInfo.Grow_Time = 0;
        testInfo.Selling_Price = 1;
        testInfo.Price = 0;

        CropsInfo.Add(testInfo.ID, testInfo);

        foreach (KeyValuePair<int, CropInfo> crop in CropsInfo)
        {
            if (crop.Value.Is_Farmming)
            {
                Set_SelectCropUI(crop.Value);
            }
        }
    }

    public CropInfo Get_CropInfo(int ID)
    {
        if(CropsInfo.ContainsKey(ID))
        {
            return CropsInfo[ID];
        }
        else
        {
            return null;
        }
    }

    void Set_SelectCropUI(CropInfo info)
    {
        GameObject obj = Instantiate(Select_Crop_UI_Prefab, Grid_Select_Crops.transform) as GameObject;
        obj.transform.localScale = Vector3.one;
        obj.name = info.Name;
        obj.GetComponent<Select_CropsButton_Action>().Set_Crop_info(info);

        Grid_Select_Crops.repositionNow = true;
    }
}

public class CropInfo
{
    public int ID;
    public bool Is_Farmming;
    public string Name;
    public string SpriteName;
    public int Grow_Time;
    public int Selling_Price;
    public int Price;
}