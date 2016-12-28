using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class House_Action : BulidingOBJ_Action
{
    public GameObject Sleep_Effect;

    GameObject Sleep_Gauge;
    List<GameObject> Drop_Item_Icons = new List<GameObject>();

    public override void Start_Action()
    {
        if (CitizenManager.Get_Inctance().Check_Set_House(Info.Obj_Index) == null)
        {
            Debug.Log("citizen Error!!! ");
            return;
        }

        Citizen_Info citizen = CitizenManager.Get_Inctance().Check_Set_House(Info.Obj_Index);

        Debug.Log(Info.Obj_Index);

        Citizen_Infomation_UI_Action.Get_Inctance().View_Window(citizen);
    }
    public override void Install_Action()
    {
        Citizen_Info Citizen = CitizenManager.Get_Inctance().Check_Set_House(Info.Obj_Index);

        if (Citizen != null)
        {
            CitizenManager.Get_Inctance().Create_Citizen(gameObject, Citizen);
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
