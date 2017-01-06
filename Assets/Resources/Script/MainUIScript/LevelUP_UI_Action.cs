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
    }
    public void NotView_Window()
    {
        gameObject.SetActive(false);

        Time.timeScale = 1;
    }
}
