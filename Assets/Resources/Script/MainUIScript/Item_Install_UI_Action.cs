using UnityEngine;
using System.Collections;

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
    int ID;
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

    public void View_Item_InstallUI(GameObject obj, int id)
    {
        GetComponent<UIPanel>().alpha = 1f;
        ID = id;
        Select_OBJ = obj;
        StartCoroutine(C_View_Item_InstallUI());
    }
    IEnumerator C_View_Item_InstallUI()
    {
        if(Select_OBJ == null) { yield break; }
        while (true)
        {
            Check_OBJState();
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

    void Check_OBJState()
    {
        EventOBJ_Action state = Select_OBJ.GetComponent<EventOBJ_Action>();

            Set_Base_Button_UI();

        if(state.Check_Is_Install == false)
        {
            Set_NotInstall_UI();
        }

        if (state.Is_SaveItem == false)
        {
            Set_NotSaveItem_UI();
        }
    }

    void Set_NotInstall_UI()
    {
        NotInstall_Icon.SetActive(true);
        NotButtons[(int)BUTTON_TYPE.BUY].SetActive(true);
    }
    void Set_NotSaveItem_UI()
    {
        NotButtons[(int)BUTTON_TYPE.SELL].SetActive(true);
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
        GetComponent<UIPanel>().alpha = 0f;
        Set_Base_Button_UI();
        StopCoroutine("C_View_Item_InstallUI");

        Select_OBJ.transform.rotation = Quaternion.identity;

        Select_OBJ.SetActive(false);

        GameManager.Get_Inctance().Set_BasicSetting();
    }

    public void Buy_OBJ()
    {
        GameObject obj = Instantiate(Select_OBJ, Select_OBJ.transform.position, Select_OBJ.transform.rotation) as GameObject;
        obj.name = Select_OBJ.name;
        Destroy( obj.GetComponent<Rigidbody>() );
        EventOBJ_Action obj_action = obj.GetComponent<EventOBJ_Action>();

        obj_action.Is_SaveItem = true;
        obj_action.Is_Install = true;

        obj.GetComponent<EventOBJ_Action>().Install_Action();
 
        UserManager.Get_Inctance().Increase_Gold(-100);

        NotView_Item_InstallUI();

    }
    public void Rotation_OBJ()
    {
        Select_OBJ.transform.Rotate(new Vector3(0, 90, 0f));
    }
}
