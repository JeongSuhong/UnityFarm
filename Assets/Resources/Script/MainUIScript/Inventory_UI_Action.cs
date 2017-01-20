using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *  유저가 가지고 있는 아이템을 보여주는 인벤토리 스크립트. 
 */
public class Inventory_UI_Action : MonoBehaviour {

    List<GameObject> Buttons = new List<GameObject>();

    public GameObject ItemButton_Prefab;
    public UIGrid Grid_Items;

    private static Inventory_UI_Action instance = null;

    public static Inventory_UI_Action Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(Inventory_UI_Action)) as Inventory_UI_Action;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("Inventory_UI_Action ");
            instance = obj.AddComponent(typeof(Inventory_UI_Action)) as Inventory_UI_Action;

            Debug.Log("Fail to get Inventory_UI_Action Instance");
        }
        return instance;
    }

    public void Create_ItemButton(Item Info, int obj_index, GameObject obj)
    {
        GameObject button = Instantiate(ItemButton_Prefab, Grid_Items.transform) as GameObject;
        button.transform.localScale = Vector3.one;
        button.name = Info.Name;
        Buttons.Add(button);
        button.GetComponent<Inventory_ItemButton_UI_Action>().Set_Info(Info, obj_index, obj);

        Grid_Items.repositionNow = true;
    }

    public void View_InventoryUI()
    {
        GetComponent<UIPanel>().alpha = 1;
        GameManager.Get_Inctance().Set_ViewUI();
    }

    public void Install_OBJ(GameObject button)
    {
        Buttons.Remove(button);
        Destroy(button);

        Grid_Items.repositionNow = true;
    }

    public void NotView_InventoryUI()
    {
        GetComponent<UIPanel>().alpha = 0;
        GameManager.Get_Inctance().Set_BasicSetting();
    }
}
