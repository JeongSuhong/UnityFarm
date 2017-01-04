using UnityEngine;
using System.Collections;

// 메인화면의 UI 버튼에 상속되는 스크립트. 
// 버튼을 누르고 화면을 드래그 할때 화면이 이동되는걸 방지. 

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
