using UnityEngine;
using System.Collections;

// 아이템을 획득했을때 생성 및 연출되는 아이템 아이콘의 스크립트.
// 목적지가 있는 아이템 아이콘이 사용한다.

public class Drop_Item_Icon_Action : MonoBehaviour
{
    public Vector3 Target;                              // 아이템 아이콘이 도착하는 목적지
    public UISprite Icon;
    public bool Is_Playing = false;

    public void Set_Awake(Vector3 target, string sprite_name)
    {
      
        Icon.spriteName = sprite_name;
        gameObject.SetActive(true);
        Is_Playing = true;
        Target = target;

        StartCoroutine(C_Update());
    }

    IEnumerator C_Update()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            if(Vector3.Distance(transform.position, Target) < 0.2f)
            {
                Is_Playing = false;
                gameObject.SetActive(false);
                yield break;
            }

            Vector3 Distance = Vector3.Lerp(transform.position, Target, Time.deltaTime * 3f);

            transform.position = Distance;

            yield return null;
        }
     
    }



}
