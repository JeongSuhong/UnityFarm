using UnityEngine;
using System.Collections;

public class Select_Crops_Action : MonoBehaviour {

    public int Select_Crop_ID = 0;

    private static Select_Crops_Action instance = null;

    public static Select_Crops_Action Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(Select_Crops_Action)) as Select_Crops_Action;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("Select_Crops_Action ");
            instance = obj.AddComponent(typeof(Select_Crops_Action)) as Select_Crops_Action;

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
        GameManager.Get_Inctance().Plant_Drag_Farm();
    }

    public void View_SelectCrops_UI()
    {
        GetComponent<UIPanel>().alpha = 1;
    }
    public void NotView_SelectCrops_UI()
    {
        Select_Crop_ID = 0;
        GetComponent<UIPanel>().alpha = 0;
    }

}
