using UnityEngine;
using System.Collections;

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
        Camera_Action.Get_Inctance().Set_CameraMoving();
        GetComponent<UIPanel>().alpha = 1;
    }
    public void NotView_SelectCrops_UI()
    {
        GetComponent<UIPanel>().alpha = 0;
    }

}
