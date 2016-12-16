using UnityEngine;
using System.Collections;

public class Get_Item_MiniUI_Action : MonoBehaviour {

    public bool Is_Playing = false;

    public UISprite Icon;
    public UILabel Count;

    public void Set_Awake(string sprite_name, int count, UIAtlas type)
    {
        Is_Playing = true;
        gameObject.SetActive(true);
        Icon.atlas = type;
        Icon.spriteName = sprite_name;
        Count.text = count.ToString();

        StartCoroutine(C_Update());
    }

    IEnumerator C_Update()
    {
        float y = 0;
        while(y < 1f)
        {
            y += Time.deltaTime * 5f;

            transform.Translate(new Vector3(0f, Time.deltaTime, 0f));

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.1f);

        Is_Playing = false;
        gameObject.SetActive(false);

        yield break;
    }
}
