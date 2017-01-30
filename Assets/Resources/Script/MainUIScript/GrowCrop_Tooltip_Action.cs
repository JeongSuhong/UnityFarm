using UnityEngine;
using System.Collections;

/*
 * 작물이 자라는 상태의 밭을 누르면 생성되는 작물 정보창
 *  Get_GrowTime() : 작물 정보창 내의 GrowTime ( 밭에서 돌아가는 Timer와는 별개 )
 */

public class GrowCrop_Tooltip_Action : MonoBehaviour {

    public UISprite CropIcon;
    public UILabel CropName;
    public UILabel GrowTime;
    public UILabel Crop_Exp;

    public void View_Tooltip(CropInfo info,  Farm_Action farm)
    {
        GameManager.Get_Inctance().Set_ViewUI();

        CropIcon.spriteName = info.Sprite_Name;
        CropName.text = info.Name;
        GrowTime.text = farm.GrowTime.ToString();
        Crop_Exp.text = info.Get_Exp.ToString();

        GetComponent<UIPanel>().alpha = 1;

        StartCoroutine(Get_GrowTime(farm));
    }

    IEnumerator Get_GrowTime(Farm_Action farm)
    {
        float grow_time = farm.GrowTime;
        while(grow_time > 0)
        {
            grow_time = farm.GrowTime;
            GrowTime.text = GameManager.Get_Inctance().Set_Text_Time((int)grow_time);

            yield return null;
        }

        yield break;
    }

    public void NotView_Tooltip()
    {
        GetComponent<UIPanel>().alpha = 0;
        StopAllCoroutines();
        GameManager.Get_Inctance().Set_BasicSetting();
    }
}
