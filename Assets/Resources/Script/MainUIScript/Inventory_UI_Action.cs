using UnityEngine;
using System.Collections;

/*
 *  유저가 가지고 있는 아이템을 보여주는 인벤토리 스크립트. 
 */
public class Inventory_UI_Action : MonoBehaviour {


    public void View_InventoryUI()
    {
        GetComponent<UIPanel>().alpha = 1;
    }
    public void NotView_InventoryUI()
    {
        GetComponent<UIPanel>().alpha = 0;
    }
}
