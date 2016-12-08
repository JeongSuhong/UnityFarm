using UnityEngine;
using System.Collections;

public class Sell_Crop_UI_Action : MonoBehaviour {

    int CropID;
    public UISprite Crop_Icon;
    public UILabel Crop_Name;
    public UILabel Label_Count;
    
    int count = 0;

    private static Sell_Crop_UI_Action instance = null;

    public static Sell_Crop_UI_Action Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(Sell_Crop_UI_Action)) as Sell_Crop_UI_Action;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("Sell_Crop_UI_Action ");
            instance = obj.AddComponent(typeof(Sell_Crop_UI_Action)) as Sell_Crop_UI_Action;

            Debug.Log("Fail to get Sell_Crop_UI_Action Instance");
        }
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    public void Set_Sell_Crop_UI(int ID, string Icon_Name, string Name)
    {
        CropID = ID;
        Crop_Icon.spriteName = Icon_Name;
        Crop_Name.text = Name;

        Label_Count.text = count.ToString();

        GetComponent<UIPanel>().alpha = 1;
    }
}
