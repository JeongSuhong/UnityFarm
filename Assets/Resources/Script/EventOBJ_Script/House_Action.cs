using UnityEngine;
using System.Collections;

public class House_Action : EventOBJ_Action {

    public GameObject Sleep_Effect;

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

    public void Set_Sleep_Effect(bool setting)
    {
        Sleep_Effect.SetActive(setting);
    }
}
