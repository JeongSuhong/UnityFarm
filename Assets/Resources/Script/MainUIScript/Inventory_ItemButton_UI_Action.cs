using UnityEngine;
using System.Collections;

public class Inventory_ItemButton_UI_Action : MonoBehaviour {

    public Item Item_Info;
    GameObject ItemOBJ;

    public UISprite Item_Icon;
    public UILabel Label_Name;
    public UISprite[] States_Icon = new UISprite[3];
    public UILabel[] States_Label = new UILabel[3];

    public void Set_Info(Item info, int obj_index, GameObject obj)
    {
        Item_Icon.spriteName = info.Sprite_Name;
        Label_Name.text = info.Name;

        if(info.Buff_Happy != 0)
        {
            States_Icon[0].gameObject.SetActive(true);
            States_Icon[0].spriteName = "heart";

            States_Label[0].gameObject.SetActive(true);
            States_Label[0].text = "행복 + "+info.Buff_Happy.ToString();
        }

        Item_Info = info;

        if (obj == null)
        {
            GameObject EventOBJ_Prefab = Resources.Load("Prefabs/InstallOBJ_Prefabs/" + info.Model_Name) as GameObject;

            if (EventOBJ_Prefab == null)
            {
                Debug.Log("Prefab Error!!!");
                return;
            }

            ItemOBJ = Instantiate(EventOBJ_Prefab, Vector3.one, Quaternion.identity) as GameObject;
            ItemOBJ.name = Item_Info.Model_Name;
            BulidingOBJ_Action item_action = ItemOBJ.GetComponent<BulidingOBJ_Action>();
            item_action.Is_Install = false;
            item_action.Check_Buy_Item = true;

            item_action.Info = Item_Info;
            item_action.Obj_Index = obj_index;

            UserManager.Get_Inctance().Add_InstallOBJ(item_action);
        }
        else
        {
            ItemOBJ = obj;
        }

        ItemOBJ.SetActive(false);

    }

    public void Install_OBJ()
    {
        ItemOBJ.SetActive(true);
        Item_Install_UI_Action.Get_Inctance().View_Item_InstallUI(ItemOBJ);
        Inventory_UI_Action.Get_Inctance().NotView_InventoryUI();
        GameManager.Get_Inctance().Install_Item(ItemOBJ);
        Inventory_UI_Action.Get_Inctance().Install_OBJ(gameObject);

    }

}
