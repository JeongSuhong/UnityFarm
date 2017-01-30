using UnityEngine;
using System.Collections;

/*
 *  아이템을 설치할때 샘플 오브젝트와 함께 뜨는 버튼들을 관리하는 UI스크립트
 *  C_View_Item_InstallUI()으로 샘플 오브젝트를 따라다닌다. 
 *  Check_OBJState() : 샘플 오브젝트가 해당 자리에 설치 가능한지를 판단하는 스크립트.
 *  Set_Base_Button_UI() : 모든 버튼을 사용가능한 상태로 만듬.
 *  Buy_OBJ() : 구매버튼을 누르면 실행. UserManager에 있는 Buy_OBJ()를 호출.
 */


public class Item_Install_UI_Action : MonoBehaviour {
    
    enum BUTTON_TYPE
    {
        SELL = 0,
        STORAGE,
        BUY,
        CANCEL,
        ROTATION,
    }

    public GameObject[] Buttons;
    public GameObject[] NotButtons;
    public GameObject NotInstall_Icon;

    GameObject Select_OBJ = null;
    Item Select_OBJ_Info;
    private static Item_Install_UI_Action instance = null;

    public static Item_Install_UI_Action Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(Item_Install_UI_Action)) as Item_Install_UI_Action;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("Item_Install_UI_Action ");
            instance = obj.AddComponent(typeof(Item_Install_UI_Action)) as Item_Install_UI_Action;

            Debug.Log("Fail to get Item_Install_UI_Action Instance");
        }
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    public void View_Item_InstallUI(GameObject obj)
    {
        GetComponent<UIPanel>().alpha = 1f;
        Select_OBJ = obj;
        Select_OBJ_Info = Select_OBJ.GetComponent<BulidingOBJ_Action>().Info;
        GameManager.Get_Inctance().Install_Item(obj);
        StartCoroutine(C_View_Item_InstallUI());
    }
    IEnumerator C_View_Item_InstallUI()
    {
        Check_OBJState();

        while (true)
        {
            if(Select_OBJ == null) { yield break; }

            Check_InstallOBJ();
            // 처음 위치해야할 OBJ의 월드상의 위치를 화면상의 위치로 변환한다.
            Vector3 p = Camera.main.WorldToViewportPoint(Select_OBJ.transform.position);
            // p의 화면상 위치는 Camera기준이기 때문에 UICamera기준으로 다시 바꾼다.
            GetComponent<UIPanel>().transform.position = UICamera.mainCamera.ViewportToWorldPoint(p);

            p = transform.localPosition;
            p.x = Mathf.RoundToInt(p.x);
            p.y = Mathf.RoundToInt(p.y);
            p.z = 0f;
            transform.localPosition = p;

            yield return null;
        }
    }

    void Check_InstallOBJ()
    {
        BulidingOBJ_Action state = Select_OBJ.GetComponent<BulidingOBJ_Action>();

        if(state.Info.Type == ITEM_TYPE.BUILDING)
        {
            Set_NotSellItem_UI();
        }

        if(state.Check_Is_Install == false)
        {
            Set_NotInstall_UI();
        }
        else
        {
            Set_Install_UI();
        }
    }
    void Check_OBJState()
    {
        BulidingOBJ_Action state = Select_OBJ.GetComponent<BulidingOBJ_Action>();

        Set_Base_Button_UI();

        if (state.Check_Buy_Item == false)
        {
            Set_NotSellItem_UI();
            Set_NotStorge_UI();
        }
        if(Select_OBJ_Info.Buliding_ID == (int)LIMIT_CHECK_OBJ.FARM || Select_OBJ_Info.Buliding_ID == (int)LIMIT_CHECK_OBJ.HOUSE)
        {
            Set_NotStorge_UI();
        }
    }

    void Set_NotInstall_UI()
    {
        NotInstall_Icon.SetActive(true);
        NotButtons[(int)BUTTON_TYPE.BUY].SetActive(true);
    }
    void Set_Install_UI()
    {
        NotInstall_Icon.SetActive(false);
        NotButtons[(int)BUTTON_TYPE.BUY].SetActive(false);
    }
    void Set_NotSellItem_UI()
    {
        NotButtons[(int)BUTTON_TYPE.SELL].SetActive(true);
    }
    void Set_NotStorge_UI()
    {
        NotButtons[(int)BUTTON_TYPE.STORAGE].SetActive(true);
    }

    void Set_Base_Button_UI()
    {
        NotInstall_Icon.SetActive(false);
        for (int i = 0; i < NotButtons.Length; i++)
        {
            NotButtons[i].SetActive(false);
        }
    }

    public void NotView_Item_InstallUI()
    {
        StopCoroutine("C_View_Item_InstallUI");

        Select_OBJ = null;
        Select_OBJ_Info = null;

        GetComponent<UIPanel>().alpha = 0f;
        Set_Base_Button_UI();
        GameManager.Get_Inctance().Set_BasicSetting();
    }


    public void Cancel_Install()
    {
        // 산 아이템이 아니면 취소 버튼을 누를때 없어지게 한다.
        if (!Select_OBJ.GetComponent<BulidingOBJ_Action>().Check_Buy_Item)
        {
            Select_OBJ.SetActive(false);
        }
        else
        {
            Select_OBJ.transform.position = Select_OBJ.GetComponent<BulidingOBJ_Action>().Origin_Position;
            Select_OBJ.transform.Rotate(Select_OBJ.GetComponent<BulidingOBJ_Action>().Origin_Rotation);
        }

        NotView_Item_InstallUI();
    }
    public void Install_OBJ()
    {
        BulidingOBJ_Action action = Select_OBJ.GetComponent<BulidingOBJ_Action>();
        action.Is_Install = true;

        UserManager.Get_Inctance().Set_DB_Install_Buliding(action, Select_OBJ);

        Select_OBJ.SetActive(false);

        NotView_Item_InstallUI();
    }
    public void Rotation_OBJ()
    {
        Select_OBJ.transform.Rotate(new Vector3(0, 90, 0f));
    }
    public void Storage_OBJ()
    {
        BulidingOBJ_Action action = Select_OBJ.GetComponent<BulidingOBJ_Action>();
        UserManager.Get_Inctance().Update_DB_User_Storage_OBJ(action.Obj_Index, action.Info.Buliding_ID);
        NotView_Item_InstallUI();
    }
    public void Sell_OBJ()
    {
        StoreManager.Get_Inctance().Set_DB_Sell_Item(Select_OBJ.GetComponent<BulidingOBJ_Action>().Obj_Index, Select_OBJ_Info.Buliding_ID);
        NotView_Item_InstallUI();
    }
}
