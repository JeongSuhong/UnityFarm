using UnityEngine;
using System.Collections;
using JsonFx.Json;

/*
 * 밭 오브젝트가 사용하는 스크립트.
 * Start_Action() : 클릭시 실행 -> Check_Action_Farm()실행
 * Check_Action_Farm() : 밭의 상태를 판단해서 함수를 실행시킴
 * Plant_Crop() : 심을 작물의 정보를 파악하고 심음
 * Harvest_Crop() : 다 자란 작물을 수확하고 UserManager에게 수확작물정보 전달, 찌꺼기 오브젝트 생성
 * Cleaning_Farm() : 찌꺼기 오브젝트 삭제, 밭 상태 초기화 
 * C_Grow_Time() : 작물이 자라는 중에 돌아가는 코루틴. 다 자라면 수확가능상태로 변경
 */


public class Farm_Action : BulidingOBJ_Action
{
    public CropInfo Planted_Crop;                           // 현재 밭에 심겨져 있는 작물의 정보
    public FARM_STATE State = FARM_STATE.NONE;

    public GameObject CropModelObj;
    public GameObject RotObj;
    public GameObject SeedObj;
    public GameObject Harvest_Effect;

    public float GrowTime = 0;                          // 작물이 자라는 시간의 타이머

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

        if(Crop_ID == -1) { return; }

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

        UIManager.Get_Inctance().Set_Drop_Item_Icon(gameObject, Planted_Crop.Sprite_Name);

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
