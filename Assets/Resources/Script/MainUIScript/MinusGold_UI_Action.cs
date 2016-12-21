using UnityEngine;
using System.Collections;

public class MinusGold_UI_Action : MonoBehaviour {

    public UILabel Label_Gold;

    public void Set_UI(int gold)
    {
        Label_Gold.text = gold.ToString();
        StartCoroutine(C_Set_Timer());
    }
    IEnumerator C_Set_Timer()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    
    }

}
