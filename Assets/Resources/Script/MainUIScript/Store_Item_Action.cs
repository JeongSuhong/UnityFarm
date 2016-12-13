﻿using UnityEngine;
using System.Collections;

public class Store_Item_Action : MonoBehaviour {

    Item Item_Info;
    GameObject EventOBJ = null;

    public UILabel Item_Name;
    public UISprite Item_Icon;
    public UILabel Label_Gold;
    public UILabel Label_Happy;


    public void Set_Item_Info(Item item_info)
    {
        Item_Info = item_info;

        Item_Name.text = Item_Info.Name;
        Item_Icon.spriteName = Item_Info.Sprite_Name;
        Label_Gold.text = Item_Info.Price.ToString();
        Label_Happy.text = Item_Info.Buff_Happy.ToString();

        if (Item_Info.Buff_Happy == 0)
        {
            Label_Happy.gameObject.SetActive(false);
        }
    }

    public void Install_Item()
    {
        if (EventOBJ == null)
        {
            GameObject EventOBJ_Prefab = Resources.Load("Prefabs/EventOBJ/" + Item_Info.Model_Name) as GameObject;
            EventOBJ = Instantiate(EventOBJ_Prefab, Vector3.one, Quaternion.identity) as GameObject;
            EventOBJ.name = Item_Info.Model_Name;
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
        Item_UI_Action.Get_Inctance().View_Item_InstallUI(EventOBJ, Item_Info.ID);
        GameManager.Get_Inctance().Install_Item(EventOBJ);
    }
}