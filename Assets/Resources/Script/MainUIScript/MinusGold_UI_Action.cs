using UnityEngine;
using System.Collections;

/*
 *  물건을 구매했을때 골드창 아래에 뜨는 골드감소 UI 스크립트. 
 */
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
