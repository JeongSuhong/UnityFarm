using UnityEngine;
using System.Collections;

/*
 *  상점에 뜨는 Item버튼의 스크립트 
 *  Install_Item() : Item버튼을 클릭하면 실행. 클릭한 Item의 샘플 오브젝트를 만들고 창을 닫음.
 */

public class Store_Item_Action : MonoBehaviour {

    Item Item_Info;
    GameObject EventOBJ = null;

    public UILabel Item_Name;
    public UISprite Item_Icon;
    public UILabel Label_Gold;
    public GameObject Effects;
    public GameObject Limit;

    readonly int[] LIMIT_BASIC = new int[3]{ 255, 153, 0 };
    readonly int[] LIMIT_LIMIT = new int[3] { 255, 0, 0 };

    public void Set_Item_Info(Item item_info)
    {
        Item_Info = item_info;

        Item_Name.text = Item_Info.Name;
        Item_Icon.spriteName = Item_Info.Sprite_Name;
        Label_Gold.text = Item_Info.Price.ToString();

        Check_Limit(Item_Info.Buliding_ID);
    }

    public void Install_Item()
    {
        if(Check_Install(Item_Info.Buliding_ID) == false) { return; }

        if (EventOBJ == null)
        {
            GameObject EventOBJ_Prefab = Resources.Load("Prefabs/EventOBJ/" + Item_Info.Model_Name) as GameObject;

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
    }
    bool Check_Install(int Buliding_ID)
    {
        switch((LIMIT_CHECK_OBJ)Buliding_ID)
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
    public void Check_Limit(int Buliding_ID)
    {
        switch ((LIMIT_CHECK_OBJ)Buliding_ID)
        {
            case LIMIT_CHECK_OBJ.HOUSE:
                {
                    int house_count = UserManager.Get_Inctance().House_Count;
                    int max_count = UserManager.Get_Inctance().Max_House;
                    Limit.SetActive(true);
                    string text = string.Format("{0} / {1}", house_count, max_count);
                    Limit.GetComponent<UILabel>().text = text;

                    if(house_count >= max_count)
                    {
                        Limit.GetComponent<UILabel>().color = new Color(LIMIT_LIMIT[0], LIMIT_LIMIT[1], LIMIT_LIMIT[2]);
                    }
                    else
                    {
                        Limit.GetComponent<UILabel>().color = new Color(LIMIT_BASIC[0], LIMIT_BASIC[1], LIMIT_BASIC[2]);
                    }

                    break;
                }
            case LIMIT_CHECK_OBJ.FARM:
                {
                    int farm_count = UserManager.Get_Inctance().Farm_Count;
                    int max_farm = UserManager.Get_Inctance().Max_Farm;

                    Limit.SetActive(true);
                    string text = string.Format("{0} / {1}", farm_count, max_farm);
                    Limit.GetComponent<UILabel>().text = text;

                    if (farm_count >= max_farm)
                    {
                        Limit.GetComponent<UILabel>().color = new Color(LIMIT_LIMIT[0], LIMIT_LIMIT[1], LIMIT_LIMIT[2]);
                    }
                    else
                    {
                        Limit.GetComponent<UILabel>().color = new Color(LIMIT_BASIC[0], LIMIT_BASIC[1], LIMIT_BASIC[2]);
                    }

                    break;
                }

            default:
                break;
                
        }
    }

    enum EFFECTS
    {
        HAPPY = 0,
        MAX,
    }

}
public enum LIMIT_CHECK_OBJ
{
    FARM = 0,
    HOUSE = 1,
}
