﻿using UnityEngine;
using System.Collections;

public class StoreManager : MonoBehaviour {


    public void View_StoreUI()
    {
        GetComponent<UIPanel>().alpha = 1;
    }
    public void NotView_StroeUI()
    {
        GetComponent<UIPanel>().alpha = 0;
    }
}
