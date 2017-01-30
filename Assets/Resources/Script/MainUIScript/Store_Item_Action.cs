using UnityEngine;
using System.Collections;

/*
 *  상점에 뜨는 Item버튼의 스크립트 
 *  Install_Item() : Item버튼을 클릭하면 실행. 클릭한 Item의 샘플 오브젝트를 만들고 창을 닫음.
 */

public class Store_Item_Action : MonoBehaviour {

    public Item Item_Info;
    GameObject EventOBJ = null;

    public UILabel Item_Name;
    public UISprite Item_Icon;
    public UILabel Label_Gold;
    public GameObject Effects;
    public GameObject Limit_Count_UI;
    public GameObject Limit_UI;

    readonly int[] LIMIT_BASIC = new int[3]{ 255, 153, 0 };
    readonly int[] LIMIT_LIMIT = new int[3] { 255, 0, 0 };

    public void Set_Item_Info(Item item_info)
    {
        Item_Info = item_info;

        Item_Name.text = Item_Info.Name;
        Item_Icon.spriteName = Item_Info.Sprite_Name;
        Label_Gold.text = Item_Info.Price.ToString();

        Limit_UI.SetActive(false);
    
        Check_Limit();
    }

    public void Install_Item()
    {
        if (Check_Install() == false) { return; }
        if(UserManager.Get_Inctance().Get_Gold() < Item_Info.Price) { return; }

        if (EventOBJ == null)
        {
            GameObject EventOBJ_Prefab = Resources.Load("Prefabs/InstallOBJ_Prefabs/" + Item_Info.Model_Name) as GameObject;

            if(EventOBJ_Prefab == null)
            {
                Debug.Log("Prefab Error!!!");
                return;
            }

            EventOBJ = Instantiate(EventOBJ_Prefab, Vector3.one, Quaternion.identity) as GameObject;
            EventOBJ.name = Item_Info.Model_Name;
            EventOBJ.GetComponent<BulidingOBJ_Action>().Info = Item_Info;
            EventOBJ.AddComponent<Rigidbody>().useGravity = false;
        }

        EventOBJ.SetActive(true);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Physics.Raycast(쏜 레이 정보, 충돌 정보, 거리)
        //  => 충돌이 되면 true를 리턴하면서 충돌 정보를 확인 할 수 있다.
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 pos = hit.collider.gameObject.transform.position;
            pos.y = 0.06f;
            EventOBJ.transform.position = pos;
        }

        StoreManager.Get_Inctance().NotView_StroeUI();
        Item_Install_UI_Action.Get_Inctance().View_Item_InstallUI(EventOBJ);
        GameManager.Get_Inctance().Install_Item(EventOBJ);

        Check_Limit();
    }
    bool Check_Install()
    {
        switch((LIMIT_CHECK_OBJ)Item_Info.Buliding_ID)
        {
            case LIMIT_CHECK_OBJ.HOUSE:
                {
                    return UserManager.Get_Inctance().Check_Install_House();
                }

            case LIMIT_CHECK_OBJ.FARM:
                {
                    return UserManager.Get_Inctance().Check_Install_Farm();
                }

            default:
                return true;
        }
    }
    public void Check_Limit()
    {
        if(Item_Info.Type == ITEM_TYPE.BUILDING)
        {
            if(UserManager.Get_Inctance().Check_Inven_InstallOBJ(Item_Info.Buliding_ID))
            {
                Limit_UI.SetActive(true);
                return;
            }
        }

        switch ((LIMIT_CHECK_OBJ)Item_Info.Buliding_ID)
        {
            case LIMIT_CHECK_OBJ.HOUSE:
                {
                    int house_count = UserManager.Get_Inctance().House_Count;
                    int max_count = UserManager.Get_Inctance().Max_House;
                    Limit_Count_UI.SetActive(true);
                    string text = string.Format("{0} / {1}", house_count, max_count);
                    Limit_Count_UI.GetComponent<UILabel>().text = text;

                    if(house_count >= max_count)
                    {
                        Limit_Count_UI.GetComponent<UILabel>().color = new Color(LIMIT_LIMIT[0], LIMIT_LIMIT[1], LIMIT_LIMIT[2]);
                        Limit_UI.SetActive(true);
                    }
                    else
                    {
                        Limit_Count_UI.GetComponent<UILabel>().color = new Color(LIMIT_BASIC[0], LIMIT_BASIC[1], LIMIT_BASIC[2]);
                    }

                    break;
                }
            case LIMIT_CHECK_OBJ.FARM:
                {
                    int farm_count = UserManager.Get_Inctance().Farm_Count;
                    int max_farm = UserManager.Get_Inctance().Max_Farm;

                    Limit_Count_UI.SetActive(true);
                    string text = string.Format("{0} / {1}", farm_count, max_farm);
                    Limit_Count_UI.GetComponent<UILabel>().text = text;

                    if (farm_count >= max_farm)
                    {
                        Limit_Count_UI.GetComponent<UILabel>().color = new Color(LIMIT_LIMIT[0], LIMIT_LIMIT[1], LIMIT_LIMIT[2]);
                        Limit_UI.SetActive(true);
                    }
                    else
                    {
                        Limit_Count_UI.GetComponent<UILabel>().color = new Color(LIMIT_BASIC[0], LIMIT_BASIC[1], LIMIT_BASIC[2]);
                    }

                    break;
                }

            default:
                break;
                
        }
    }

    public void Set_Limit()
    {
        Limit_UI.SetActive(true);
    }
    public void Set_NotLimit()
    {
        Limit_UI.SetActive(false);
    }

    enum EFFECTS
    {
        HAPPY = 0,
        MAX,
    }

}
public enum LIMIT_CHECK_OBJ
{
    FARM = 10,
    HOUSE = 11,
}
