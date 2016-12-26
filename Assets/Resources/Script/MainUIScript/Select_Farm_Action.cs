using UnityEngine;
using System.Collections;

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
