using UnityEngine;
using System.Collections;

public class CropWarehouse_Crop_UI_Action : MonoBehaviour {

    int CropID;

    public UISprite Crop_Icon;
    public UILabel Label_Count;

    public void Set_Crop_Info(int crop_id, int Count)
    {
        CropID = crop_id;
        Crop_Icon.spriteName = CropsManager.Get_Inctance().Get_CropInfo(crop_id).Sprite_Name;
        Label_Count.text = Count.ToString();
    }
    public void View_Sell_Crop_UI()
    {
        CropInfo CropInfo = CropsManager.Get_Inctance().Get_CropInfo(CropID);

        Sell_Crop_UI_Action.Get_Inctance().Set_Sell_Crop_UI(CropID, CropInfo.Sprite_Name, CropInfo.Name);
    }
}
