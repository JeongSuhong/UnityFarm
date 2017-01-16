using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *  메인 UI에 관련된 스크립트. 
 *  Set_Drop_Item_Icon() : 작물 얻었을때나 아이템 획득했을때 아이콘을 만들고 관리.
 *  Create_Drop_Item_Icon() : 위 함수의 아이콘을 만듬.
 *  Set_MinusGold_UI() : 골드감소 아이콘을 만듬.
 */
public class UIManager : MonoBehaviour {

    public GameObject UIRoot;

    public UILabel Label_Level;
    public UISlider Slider_Exp;
    public UILabel Label_Gold;  
    public UILabel Label_Jam;
    public UILabel Label_House;

    public UIAtlas[] Item_UI_Atlas;
    
    public GameObject GrowCrop_Tooltip;
    public UIPanel Item_UI;

    public GameObject MinusGold_UI;

    public Transform CropWarehouse_UI_Position;
    public GameObject Drop_Item_Icon_Prefab;
    List<GameObject> Drop_Item_Icons = new List<GameObject>();

    public GameObject Get_Item_Mini_Icon_Prefab;
    List<GameObject> Get_Item_Mini_Icons = new List<GameObject>();

    public GameObject LevelUp_UI = null;

    public GameObject Clicking_UI = null;

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

    public void Set_UserInfo(RecvUserData info)
    {
        Label_Level.text = info.Level.ToString();
        Slider_Exp.value = info.Exp / ( 100 * info.Level );
        Label_Gold.text = info.Gold.ToString();
        Label_Jam.text = info.Jam.ToString();
        Set_HouseCount_UI(UserManager.Get_Inctance().House_Count, info.Max_House);
    }

    public void View_GrowCrop_Tooltip(CropInfo info, Farm_Action farm)
    {
        GrowCrop_Tooltip.GetComponent<GrowCrop_Tooltip_Action>().View_Tooltip(info, farm);
    }

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

    public void Set_Get_Item_Icon(GameObject callOBJ, string sprite_name, int count, ITEM_UI_TYPE type)
    {
        // 비활성화하고 있는 Icon을 담는 변수.
        GameObject Icon = null;

        // 하나도 생성된 아이콘이 없으면 만든다.
        if (Get_Item_Mini_Icons.Count == 0)
        {
            Create_Get_Item_Icon();
        }
        // 비활성화된 아이콘이 있는지 체크한다.
        for (int i = 0; i < Get_Item_Mini_Icons.Count; i++)
        {
            if (Get_Item_Mini_Icons[i].GetComponent<Get_Item_MiniUI_Action>().Is_Playing == false)
            {
                Icon = Get_Item_Mini_Icons[i];
                break;
            }

            // 만약 전부다 활성화되있는 상태라면 새로 하나 만들고 다시 체크한다.
            if (i == Get_Item_Mini_Icons.Count - 1)
            {
                Create_Get_Item_Icon();
                i = 0;
            }
        }

        // 처음 위치해야할 OBJ의 월드상의 위치를 화면상의 위치로 변환한다.
        Vector3 p = Camera.main.WorldToViewportPoint(callOBJ.transform.position);
        // p의 화면상 위치는 Camera기준이기 때문에 UICamera기준으로 다시 바꾼다.
        Icon.transform.position = UICamera.mainCamera.ViewportToWorldPoint(p);

        p = Icon.transform.localPosition;
        p.x = Mathf.RoundToInt(p.x) - 50f;
        p.y = Mathf.RoundToInt(p.y) + 120f;
        p.z = 0f;
        Icon.transform.localPosition = p;


        // ICon이 마지막으로 도착해야할 OBJ Position과 Sprite_name을 매개변수로 넘긴다.
        Icon.GetComponent<Get_Item_MiniUI_Action>().Set_Awake(sprite_name, count, Item_UI_Atlas[(int)type]);
    }
    void Create_Get_Item_Icon()
    {
        GameObject obj = Instantiate(Get_Item_Mini_Icon_Prefab, UIRoot.transform) as GameObject;
        obj.transform.localScale = Vector3.one;
        Get_Item_Mini_Icons.Add(obj);
    }

    public void Set_MinusGold_UI(int gold)
    {
        MinusGold_UI.transform.localPosition = MinusGold_UI.GetComponent<TweenPosition>().from;
        MinusGold_UI.SetActive(true);
        MinusGold_UI.GetComponent<MinusGold_UI_Action>().Set_UI(gold);
    }
    
    public void Set_HouseCount_UI(int count, int max)
    {
        Label_House.text = count.ToString() +" / " + max.ToString();
    }

    public void Set_ExpGrid(float value)
    {
        Slider_Exp.value = value;
    }
    public void Set_Level(int value)
    {
        if(value != int.Parse(Label_Level.text))
        {
            LevelUp_UI.SetActive(true);
            LevelUp_UI.GetComponent<LevelUP_UI_Action>().Set_Action(value);
            CropsManager.Get_Inctance().Check_LevelLimit_CropButton();
        }

        Label_Level.text = value.ToString();
    }

    public void View_Clicking(Vector3 pos)
    {
        Clicking_UI.transform.position = pos;
        Clicking_UI.GetComponent<UIPanel>().alpha = 1f;
    }
    public void Set_Clicking_UI(float value, GameObject obj)
    {
        Clicking_UI.GetComponentInChildren<UISlider>().value = value;
    }
    public void NotView_Clicking()
    {
        Clicking_UI.GetComponent<UIPanel>().alpha = 0;
    }
}
public enum ITEM_UI_TYPE
{
    CROP,
    ITEM,
    GOLD,
}