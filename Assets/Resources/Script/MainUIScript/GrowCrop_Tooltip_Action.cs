using UnityEngine;
using System.Collections;

public class GrowCrop_Tooltip_Action : MonoBehaviour {

    public UISprite CropIcon;
    public UILabel CropName;
    public UILabel GrowTime;

    public void View_Tooltip(CropInfo info, Farm_Action farm)
    {
        Camera_Action.Get_Inctance().Set_NotCameraMoving();

        CropIcon.spriteName = info.Sprite_Name;
        CropName.text = info.Name;
        GrowTime.text = farm.GrowTime.ToString();

        GetComponent<UIPanel>().alpha = 1;

        StartCoroutine(Get_GrowTime(farm));
    }

    IEnumerator Get_GrowTime(Farm_Action farm)
    {
        float growtime = farm.GrowTime;

        while(growtime > 0)
        {
            growtime = farm.GrowTime;

            GrowTime.text = GameManager.Get_Inctance().Set_Text_Time((int)growtime); ;

            yield return null;
        }

        yield break;
    }

    public void NotView_Tooltip()
    {
        GetComponent<UIPanel>().alpha = 0;
        GameManager.Get_Inctance().Set_BasicSetting();
    }
}
