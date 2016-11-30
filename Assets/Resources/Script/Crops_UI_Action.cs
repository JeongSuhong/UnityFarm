using UnityEngine;
using System.Collections;

public class Crops_UI_Action : MonoBehaviour {

    int Crop_ID;

    public UISprite Icon;
    public UILabel Name;
    public UILabel Price;
    public UILabel GrowTime;
   
    void Awake()
    {
        Crop_ID = 3;
    }

    public void Select_Crop()
    {
        Select_Crops_Action.Get_Inctance().Select_Crop(Crop_ID);
    }

}
