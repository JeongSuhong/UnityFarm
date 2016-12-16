using UnityEngine;
using System.Collections;

public class Farm_Action : EventOBJ_Action
{
    public CropInfo Planted_Crop;
    public GameObject CropModelObj;
    public GameObject RotObj;
    public GameObject SeedObj;
    public GameObject Harvest_Effect;
    public FARM_STATE State = FARM_STATE.NONE;

    public float GrowTime = 0;

    public override void Start_Action()
    {
        Harvest_Effect.SetActive(false);
        Check_Action_Farm();
    }
    public void Check_Action_Farm()
    {
        if (State == FARM_STATE.NONE)
        {
            Select_Crops_Manager.Get_Inctance().View_SelectCrops_UI();
        }
        else if (State == FARM_STATE.GROWING)
        {
            UIManager.Get_Inctance().View_GrowCrop_Tooltip(Planted_Crop, this);
        }
        else if (State == FARM_STATE.MATURE)
        {
            Harvest_Crop();
        }
        else if (State == FARM_STATE.ROT)
        {
            Cleaning_Farm();
        }
    }

    public void Plant_Crop()
    {
        if(State != FARM_STATE.NONE) { return; }

        int Crop_ID = Select_Crops_Manager.Get_Inctance().Select_Crop_ID;

        if(Crop_ID == 0) { return; }

        State = FARM_STATE.GROWING;

        SeedObj.SetActive(true);

        Planted_Crop = CropsManager.Get_Inctance().Get_CropInfo(Crop_ID);
        GrowTime = Planted_Crop.Grow_Time;

        UserManager.Get_Inctance().Increase_Gold(-Planted_Crop.Price);

        StartCoroutine(C_Grow_Time());

    }
    void Harvest_Crop()
    {
        State = FARM_STATE.ROT;

        CropModelObj.SetActive(false);
        CropModelObj.transform.FindChild(Planted_Crop.Name).gameObject.SetActive(false);
        RotObj.SetActive(true);
        Harvest_Effect.SetActive(true);

        UserManager.Get_Inctance().Obtain_Crop(Planted_Crop.ID, 1);

        UIManager.Get_Inctance().Set_Drop_Item_Icon(gameObject, Planted_Crop.SpriteName);

        Planted_Crop = null;
    }
    void Cleaning_Farm()
    {
        State = FARM_STATE.NONE;
        RotObj.SetActive(false);
    }



    IEnumerator C_Grow_Time()
    {
        while(GrowTime > 0)
        {
            GrowTime -= Time.deltaTime;

            yield return null;
        }

        State = FARM_STATE.MATURE;
        SeedObj.SetActive(false);
        CropModelObj.SetActive(true);
        CropModelObj.transform.FindChild(Planted_Crop.Name).gameObject.SetActive(true);

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
