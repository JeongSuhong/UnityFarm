using UnityEngine;
using System.Collections;

public class LevelUP_UI_Action : MonoBehaviour {

    public UILabel Label_Level;

    public void Set_Action(int level)
    {
        Label_Level.text = level.ToString();

        TweenScale[] tweens = GetComponentsInChildren<TweenScale>();

        for(int i = 0; i < tweens.Length; i++)
        {
            tweens[i].enabled = true;
        }

        Time.timeScale = 0;
        GameManager.Get_Inctance().Set_ViewUI();
    }
    public void NotView_Window()
    {
        gameObject.SetActive(false);

        Time.timeScale = 1;

        GameManager.Get_Inctance().Set_NotViewUI();
    }
}
