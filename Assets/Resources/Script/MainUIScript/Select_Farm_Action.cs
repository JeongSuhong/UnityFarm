using UnityEngine;
using System.Collections;

/*
 *  밭에 심을 작물을 선택 후 어느밭에 심을지 클릭을 담당하는 스크립트. 
 *  Select_Farm() : 선택한 작물이 무엇인지 나타내는 UI를 관리
 *  Finish_Select_Farm() : 화면UI상의 완료 버튼을 누르면 실행되는 함수. 밭선택UI을 초기화한다.
 */
public class Select_Farm_Action : MonoBehaviour {

    public UISprite Select_Crop;

    public void Select_Farm(string sprite_name)
    {
        Camera_Action.Get_Inctance().Set_NotCameraMoving();

        gameObject.SetActive(true);
        Select_Crop.gameObject.SetActive(true);
        Select_Crop.spriteName = sprite_name;
        GetComponent<UIPanel>().alpha = 1;

        GameManager.Get_Inctance().Plant_Drag_Farm();

    }

    public void Finish_Select_Farm()
    {
        Select_Crop.gameObject.SetActive(false);
        GetComponent<UIPanel>().alpha = 0;

        Select_Crops_Manager.Get_Inctance().Set_SelectCrop_Nothing();
        GameManager.Get_Inctance().Set_BasicSetting();

        NotView_SelectCrops_UI();
    }

    public void View_SelectCrops_UI()
    {
        GetComponent<UIPanel>().alpha = 1;
    }
    public void NotView_SelectCrops_UI()
    {
        GetComponent<UIPanel>().alpha = 0;
    }
}
