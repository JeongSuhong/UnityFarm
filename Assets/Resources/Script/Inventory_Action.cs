using UnityEngine;
using System.Collections;

public class Inventory_Action : MonoBehaviour {


    public void View_InventoryUI()
    {
        GetComponent<UIPanel>().alpha = 1;
    }
    public void NotView_InventoryUI()
    {
        GetComponent<UIPanel>().alpha = 0;
    }
}
