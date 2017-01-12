using UnityEngine;
using System.Collections;

public class Notic_Action : MonoBehaviour {

    private static Notic_Action instance = null;

    public static Notic_Action Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(Notic_Action)) as Notic_Action;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("Notic ");
            instance = obj.AddComponent(typeof(Notic_Action)) as Notic_Action;

            Debug.Log("Fail to get Notic_Action Instance");
        }
        return instance;
    }

    void Awake()
    {
        GetComponent<UIPanel>().alpha = 0;
        transform.localScale = Vector3.zero;
    }

	public void Set_Notic(string text)
    {
        GetComponent<UIPanel>().alpha = 1;
        GetComponent<TweenScale>().ResetToBeginning();
        GetComponent<TweenScale>().enabled = true;
        StopCoroutine("C_StartAni");
        StartCoroutine("C_StartAni");

        GetComponentInChildren<UILabel>().text = text;
    }

    IEnumerator C_StartAni()
    {
        yield return new WaitForSeconds(3.0f);

        GetComponent<UIPanel>().alpha = 0;
        transform.localScale = Vector3.zero;
    }
}
