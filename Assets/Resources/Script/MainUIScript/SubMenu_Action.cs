using UnityEngine;
using System.Collections;

/*
 *  메인 화면 오른쪽에 있는 SubMenu의 스크립트. 
    C_Check_View_Menu() : SubMenu의 버튼을 누를때 나타나는 애니메이션코드. 
*/
public class SubMenu_Action : MonoBehaviour {

    bool Is_Viewing = false;

    public void Check_View_Menu()
    {
        StartCoroutine(C_Check_View_Menu());
    }

    IEnumerator C_Check_View_Menu()
    {
        float x = transform.localPosition.x;

        if (Is_Viewing)
        {
            Is_Viewing = false;

            while (x < 0)
            {
                transform.position += Vector3.right * Time.deltaTime * 3f;
                x = transform.localPosition.x;

                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            Is_Viewing = true;

            while (x > -400)
            {
                transform.position += Vector3.left * Time.deltaTime * 3f;
                x = transform.localPosition.x;

                yield return new WaitForSeconds(0.01f);
            }
        }

        yield break;
    }
}
