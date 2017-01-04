using UnityEngine;
using System.Collections;

// House를 Click시 뜨는 시민 정보 UI에 대한 스크립트.

public class Citizen_Infomation_UI_Action : MonoBehaviour {

    public UILabel Label_Name;
    public UISlider Slider_HP;
    public UILabel Label_HP;
    public UILabel Label_Tiredness;
    public UISlider Slider_Tiredness;
    public UILabel Label_Charm;
    public UISprite Type_Icon;
    public UILabel Label_Type_Explanation;
    public UILabel Label_Level;
    public UISlider Slider_Exp;
    public UILabel Label_Exp;

    private static Citizen_Infomation_UI_Action instance = null;

    public static Citizen_Infomation_UI_Action Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(Citizen_Infomation_UI_Action)) as Citizen_Infomation_UI_Action;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("Citizen_Infomation_UI_Action ");
            instance = obj.AddComponent(typeof(Citizen_Infomation_UI_Action)) as Citizen_Infomation_UI_Action;

            Debug.Log("Fail to get Citizen_Infomation_UI_Action Instance");
        }
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    void Set_Info(Citizen_Info citizen)
    {
        Label_Name.text = citizen.Name;
        Slider_HP.value = citizen.HP / citizen.Max_HP;
        Label_HP.text = string.Format("{0} / {1}", (int)citizen.HP, (int)citizen.Max_HP);
        Slider_Tiredness.value = citizen.Tiredness / citizen.Max_Tiredness;
        Label_Tiredness.text = string.Format("{0} / {1}", (int)citizen.Tiredness, (int)citizen.Max_Tiredness);
        Label_Charm.text = citizen.Charm.ToString();
        Type_Icon.spriteName = citizen.Type.ToString();
        Label_Type_Explanation.text = citizen.Type_Explanation;
        Label_Level.text = "Lv "+citizen.Level.ToString();
        Slider_Exp.value = citizen.Exp;
        Label_Exp.text = citizen.Exp.ToString() + "%";
    }
    public void View_Window(Citizen_Info citizen)
    {
        Time.timeScale = 0f;
        GetComponent<UIPanel>().alpha = 1;
        GameManager.Get_Inctance().Set_ViewUI();
        Set_Info(citizen);
    }
    public void NotView_Window()
    {
        Time.timeScale = 1f;
        GetComponent<UIPanel>().alpha = 0;
        GameManager.Get_Inctance().Set_NotViewUI();
    }

}
