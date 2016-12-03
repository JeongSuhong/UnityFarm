using UnityEngine;
using System.Collections;

public class Farm_Action : MonoBehaviour
{
    public CropInfo Planted_Crop;
    public FARM_STATE State = FARM_STATE.NONE;

    public float GrowTime = 0;

    public void Check_Action_Farm()
    {
        if (State == FARM_STATE.NONE)
        {
            Select_Crops_Action.Get_Inctance().View_SelectCrops_UI();
        }
        else if (State == FARM_STATE.GROWING)
        {
            UIManager.Get_Inctance().View_GrowCrop_Tooltip(Planted_Crop, this);
        }
        else if (State == FARM_STATE.MATURE)
        {
            Harvest_Crop();
        }
    }

    public void Plant_Crop()
    {
        if(State != FARM_STATE.NONE) { return; }

        int Crop_ID = Select_Crops_Action.Get_Inctance().Select_Crop_ID;

        if(Crop_ID == 0) { return; }

        State = FARM_STATE.GROWING;

        Planted_Crop = CropsManager.Get_Inctance().Get_CropInfo(Crop_ID);
        GrowTime = Planted_Crop.Grow_Time;
        StartCoroutine(C_Grow_Time());

        transform.GetChild(0).FindChild(Planted_Crop.Name).gameObject.SetActive(true);
    }
    public void Harvest_Crop()
    {
        State = FARM_STATE.ROT;

        transform.GetChild(0).FindChild(Planted_Crop.Name).gameObject.SetActive(false);
    }


    IEnumerator C_Grow_Time()
    {
        while(GrowTime > 0)
        {
            GrowTime -= Time.deltaTime;

            yield return null;
        }

        State = FARM_STATE.MATURE;

        yield break;
    }

    public enum FARM_STATE
    {
        NONE,           // 아무것도 하지 않음
        GROWING,    // 작물 자라는중
        MATURE,       // 작물 자람
        ROT,              // 작물 썩음
    };
}
