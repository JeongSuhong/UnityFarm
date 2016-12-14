using UnityEngine;
using System.Collections;

public class House_Action : EventOBJ_Action {

	public override void Install_Action()
    {
        GameObject Citizen = CitizenManager.Get_Inctance().Check_Set_House(gameObject);

        if (Citizen != null)
        {

        }
        else
        {
            CitizenManager.Get_Inctance().Create_Citizen(gameObject);
        }
    }
}
