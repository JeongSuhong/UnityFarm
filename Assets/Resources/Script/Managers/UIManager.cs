using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

    public GameObject UIRoot;

    public GameObject GrowCrop_Tooltip;
    public UIPanel Item_UI;
    

    public Transform CropWarehouse_UI_Position;
    public GameObject Drop_Item_Icon_Prefab;
    List<GameObject> Drop_Item_Icons = new List<GameObject>();

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

    // 작물 얻었을때나 아이템 획득했을때 아이콘 만드는 함수.
    // 아이콘이 처음 위치해야할 OBJ 랑 SpriteName을 매개변수로 받는다.
    public void Set_Drop_Item_Icon(GameObject callOBJ, string sprite_name)
    {
        // 비활성화하고 있는 Icon을 담는 변수.
        GameObject Icon = null;

        // 하나도 생성된 아이콘이 없으면 만든다.
        if (Drop_Item_Icons.Count == 0)
        {
            Create_Drop_Item_Icon();
        }
        // 비활성화된 아이콘이 있는지 체크한다.
        for (int i = 0; i < Drop_Item_Icons.Count; i++)
        {
            if (Drop_Item_Icons[i].GetComponent<Drop_Item_Icon_Action>().Is_Playing == false)
            {
                Icon = Drop_Item_Icons[i];
                break;
            }

            // 만약 전부다 활성화되있는 상태라면 새로 하나 만들고 다시 체크한다.
            if (i == Drop_Item_Icons.Count - 1)
            {
                Create_Drop_Item_Icon();
                i = 0;
            }
        }

        // 처음 위치해야할 OBJ의 월드상의 위치를 화면상의 위치로 변환한다.
        Vector3 p = Camera.main.WorldToViewportPoint(callOBJ.transform.position);
        // p의 화면상 위치는 Camera기준이기 때문에 UICamera기준으로 다시 바꾼다.
        Icon.transform.position = UICamera.mainCamera.ViewportToWorldPoint(p);

        p = Icon.transform.localPosition;
        p.x = Mathf.RoundToInt(p.x);
        p.y = Mathf.RoundToInt(p.y);
        p.z = 0f;
        Icon.transform.localPosition = p;

        // ICon이 마지막으로 도착해야할 OBJ Position과 Sprite_name을 매개변수로 넘긴다.
        Icon.GetComponent<Drop_Item_Icon_Action>().Set_Awake(CropWarehouse_UI_Position.position, sprite_name);
    }
    // Drop_Item_Icon을 만드는 함수. 
    void Create_Drop_Item_Icon()
    {
        GameObject obj = Instantiate(Drop_Item_Icon_Prefab, UIRoot.transform) as GameObject;
        obj.transform.localScale = Vector3.one;
        Drop_Item_Icons.Add(obj);
    }

}
