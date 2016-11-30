using UnityEngine;
using System.Collections;

public class StoreManager : MonoBehaviour {


    public void View_StoreUI()
    {
        gameObject.SetActive(true);
    }
    public void NotView_StroeUI()
    {
        gameObject.SetActive(false);
    }
}
