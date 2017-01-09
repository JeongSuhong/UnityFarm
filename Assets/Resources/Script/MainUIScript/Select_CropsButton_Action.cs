using UnityEngine;
using System.Collections;

/*
 *  밭에 심을 작물창에 뜨는 작물버튼의 스크립트. 
 */
public class Select_CropsButton_Action : Base_Button_Action
{
    int Crop_ID;

    public UISprite Icon;
    public UILabel Name;
    public UILabel Price;
    public UILabel Exp;
    public UILabel GrowTime;

    public GameObject NotLevel_Crop;
   
    public void Set_Crop_info(CropInfo info)
    {
        NotLevel_Crop.SetActive(false);

        Crop_ID = info.ID;

        Icon.spriteName = info.Sprite_Name;
        Name.text = info.Name;
        Price.text = info.Price.ToString();
        Exp.text = info.Get_Exp.ToString();
        GrowTime.text = GameManager.Get_Inctance().Set_Text_Time(info.Grow_Time);

        if(info.Level > UserManager.Get_Inctance().Level)
        {
            NotLevel_Crop.SetActive(true);
            NotLevel_Crop.GetComponentInChildren<UILabel>().text = "Lv. " + info.Level.ToString();
        }
    }

    public void Set_Level()
    {
        float level = CropsManager.Get_Inctance().Get_CropInfo(Crop_ID).Level;

        if (level <= UserManager.Get_Inctance().Level)
        {
            NotLevel_Crop.SetActive(false);
        }
    }

    public void Select_Crop()
    {
        if(NotLevel_Crop.activeSelf) { return;  }

        Select_Crops_Manager.Get_Inctance().Select_Crop(Crop_ID);
    }

}
