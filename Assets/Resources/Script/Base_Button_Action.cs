using UnityEngine;
using System.Collections;

public class Base_Button_Action : MonoBehaviour {

   void OnDragStart()
    {
        Camera_Action.Get_Inctance().Set_NotCameraMoving();
    }

    void OnDragEnd()
    {
        Camera_Action.Get_Inctance().Set_CameraMoving();
    }
}
