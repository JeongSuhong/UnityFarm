using UnityEngine;
using System.Collections;

public class Farm_Action : MonoBehaviour
{

    public CROP Planted_Crop = CROP.NONE;
    public FARM_STATE State = FARM_STATE.NONE;

    public void Ready_Crops()
    {
        if (State == FARM_STATE.NONE)
        {
            Select_Crops_Action.Get_Inctance().View_SelectCrops_UI();
        }
    }

    public void Plant_Crop(int crop_id )
    {
        State = FARM_STATE.GROWING;
        Planted_Crop = (CROP)crop_id;
    }

    public bool Check_FarmState_None()
    {
        if(State == FARM_STATE.NONE) { return true; }
        else { return false; }
    }

    public enum FARM_STATE
    {
        NONE,           // 아무것도 하지 않음
        GROWING,    // 작물 자라는중
        MATURE,       // 작물 자람
        ROT,              // 작물 썩음
    };
}
