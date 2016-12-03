using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameObject GrowCrop_Tooltip;

    private static UIManager instance = null;

    public static UIManager Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(UIManager)) as UIManager;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("UIManager ");
            instance = obj.AddComponent(typeof(UIManager)) as UIManager;

            Debug.Log("Fail to get UIManager Instance");
        }
        return instance;
    }

    void Awake()
    {
        instance = this;
    }


    public void View_GrowCrop_Tooltip(CropInfo info, Farm_Action farm)
    {
        GrowCrop_Tooltip.GetComponent<GrowCrop_Tooltip_Action>().View_Tooltip(info, farm);
    }

}
