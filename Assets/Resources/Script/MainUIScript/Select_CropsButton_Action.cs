using UnityEngine;
using System.Collections;

public class Select_CropsButton_Action : Base_Button_Action
{
    int Crop_ID;

    public UISprite Icon;
    public UILabel Name;
    public UILabel Price;
    public UILabel GrowTime;
   
    public void Set_Crop_info(CropInfo info)
    {
        Crop_ID = info.ID;

        Icon.spriteName = info.Sprite_Name;
        Name.text = info.Name;
        Price.text = info.Price.ToString();

        GrowTime.text = GameManager.Get_Inctance().Set_Text_Time(info.Grow_Time);
    }

    public void Select_Crop()
    {
        Select_Crops_Manager.Get_Inctance().Select_Crop(Crop_ID);
    }

}
