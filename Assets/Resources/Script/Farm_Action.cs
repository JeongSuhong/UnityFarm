using UnityEngine;
using System.Collections;

public class Farm_Action : MonoBehaviour
{
    public CropInfo Planted_Crop;
    public FARM_STATE State = FARM_STATE.NONE;

    public void Ready_Crops()
    {
        if (State != FARM_STATE.NONE) { return; }

        Select_Crops_Action.Get_Inctance().View_SelectCrops_UI();
    }

    public void Plant_Crop()
    {
        if(State != FARM_STATE.NONE) { return; }

        int Crop_ID = Select_Crops_Action.Get_Inctance().Select_Crop_ID;

        if(Crop_ID == 0) { return; }

        State = FARM_STATE.GROWING;
        Planted_Crop = CropsManager.Get_Inctance().Get_CropInfo(Crop_ID);

        transform.GetChild(0).FindChild(Planted_Crop.Name).gameObject.SetActive(true);
    }

    public enum FARM_STATE
    {
        NONE,           // 아무것도 하지 않음
        GROWING,    // 작물 자라는중
        MATURE,       // 작물 자람
        ROT,              // 작물 썩음
    };
}
