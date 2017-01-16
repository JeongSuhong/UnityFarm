using UnityEngine;
using System.Collections;

/*
 *  밭에 심는 작물선택창의 스크립트. 
 *  Select_Crop() : 클릭한 버튼의 작물ID를 받아와 저장후  Select_Farm_Action 스크립트의 Select_Farm()를 실행시킴
 *  Set_SelectCrop_Nothing() : Select_Crop_ID를 -1로 초기화시킨다.
 */
public class Select_Crops_Manager : MonoBehaviour {

    public int Select_Crop_ID = 0;
    public GameObject Select_Farm_UI;

    private static Select_Crops_Manager instance = null;

    public static Select_Crops_Manager Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(Select_Crops_Manager)) as Select_Crops_Manager;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("Select_Crops_Manager ");
            instance = obj.AddComponent(typeof(Select_Crops_Manager)) as Select_Crops_Manager;

            Debug.Log("Fail to get Select_Crops_Action Instance");
        }
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    public void Select_Crop(int crop_id)
    {
        int gold = CropsManager.Get_Inctance().Get_CropInfo(crop_id).Price;

        if(gold > UserManager.Get_Inctance().Get_Gold()){ return; }

        Select_Crop_ID = crop_id;
        string Crop_SpriteName = CropsManager.Get_Inctance().Get_CropInfo(crop_id).Sprite_Name;

        Select_Farm_UI.GetComponent<Select_Farm_Action>().Select_Farm(Crop_SpriteName);

        NotView_SelectCrops_UI();
    }

    public void Set_SelectCrop_Nothing()
    {
        Select_Crop_ID = -1;
    }

    public void View_SelectCrops_UI()
    {
        Select_Crop_ID = -1;
        GetComponent<UIPanel>().alpha = 1;
    }
    public void NotView_SelectCrops_UI()
    {
        GetComponent<UIPanel>().alpha = 0;
    }

}
