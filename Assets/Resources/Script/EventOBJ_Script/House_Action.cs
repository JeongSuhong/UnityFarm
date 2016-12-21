using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class House_Action : EventOBJ_Action {

    public GameObject Sleep_Effect;

    GameObject Sleep_Gauge;
    List<GameObject> Drop_Item_Icons = new List<GameObject>();

    public override void Start_Action()
    {
        Class_Citizen citizen = CitizenManager.Get_Inctance().Check_Set_House(gameObject).Info;
        Citizen_Infomation_UI_Action.Get_Inctance().View_Window(citizen);
    }
    public override void Install_Action()
    {
        Citizen_Action Citizen = CitizenManager.Get_Inctance().Check_Set_House(gameObject);

        if (Citizen != null)
        {

        }
        else
        {
            CitizenManager.Get_Inctance().Create_Citizen(gameObject);
        }
    }

    public House_Sleep_Gauge_Action Set_Sleep_UI(bool setting)
    {
        if (setting)
        {
            Set_Sleep_Gauge();
            Sleep_Effect.SetActive(true);

            return Sleep_Gauge.GetComponent<House_Sleep_Gauge_Action>();
        }
        else
        {
            Sleep_Gauge.SetActive(false);
            Sleep_Effect.SetActive(false);

            return null;
        }
    }
    void Set_Sleep_Gauge()
    {
        if(Sleep_Gauge == null)
        {
            GameObject Prefab = Resources.Load("Prefabs/UI/Panel_House_Sleep_Gauge") as GameObject;
            GameObject Gauge = Instantiate(Prefab, UIManager.Get_Inctance().UIRoot.transform) as GameObject;
            Gauge.transform.localScale = Vector3.one;
            Gauge.name = "House_Sleep_Gauge";
            Sleep_Gauge = Gauge;
        }

        Sleep_Gauge.SetActive(true);
        
    }
}
