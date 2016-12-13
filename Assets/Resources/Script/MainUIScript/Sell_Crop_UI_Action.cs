using UnityEngine;
using System.Collections;

public class Sell_Crop_UI_Action : MonoBehaviour {

    int CropID;
    int Crop_Obtain_Count;
    public UISprite Crop_Icon;
    public UILabel Crop_Name;
    public UILabel Label_Count;
    public UISlider Crop_Count_Slider;
    
   public int Count = 0;
   public float Slider_Count_Value = 0f;

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
        Crop_Obtain_Count = UserManager.Get_Inctance().Get_Crop_Count(CropID);

        Label_Count.text = Count.ToString();

        Slider_Count_Value = Mathf.Round((1f / Crop_Obtain_Count) * 10000f);
        Slider_Count_Value /= 10000f;

        GetComponent<UIPanel>().alpha = 1;
    }

    public void Set_Sell_Crop_Count_Slider()
    {
        if(Crop_Obtain_Count == 0) { return; }

        float Slider_Value = Crop_Count_Slider.value;

        Count = Mathf.RoundToInt(Slider_Value / Slider_Count_Value);

        if (Count == Crop_Obtain_Count)
        {
            Crop_Count_Slider.value = 1;
        }
        else
        {
            Crop_Count_Slider.value = Count * Slider_Count_Value;
        }

        Label_Count.text = Count.ToString();
    }
    public void Set_OneIncrease_Crop_Count()
    {
        if (Count >= Crop_Obtain_Count) { return; }

        Count++;
        Label_Count.text = Count.ToString();

        if (Count == Crop_Obtain_Count)
        {
            Crop_Count_Slider.value = 1;
        }
        else
        {
            Crop_Count_Slider.value = Count * Slider_Count_Value;
        }
    }
    public void Set_OneDecrease_Crop_Count()
    {
        if (Count <= 0) { return; }

        Count--;
        Label_Count.text = Count.ToString();

        Crop_Count_Slider.value = Count * Slider_Count_Value;
    }
    public void Sell_Crop()
    {
        UserManager.Get_Inctance().Sell_Crop(Count, CropID);
        NotView_Window();
    }
    public void NotView_Window()
    {
        Crop_Count_Slider.value = 0f;
        Label_Count.text = "0";
        CropID = -1;
        Crop_Obtain_Count = 0;
        Crop_Icon.spriteName = "";
        Crop_Name.text = "";

        GetComponent<UIPanel>().alpha = 0f;
    }
}
