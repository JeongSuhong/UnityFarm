using UnityEngine;
using System.Collections;

public class GameManager_Action : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            // 카메라에서 화면상의 마우스 좌표에 해당하는 공간으로 레이를 쏜다.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Physics.Raycast(쏜 레이 정보, 충돌 정보, 거리)
            //  => 충돌이 되면 true를 리턴하면서 충돌 정보를 확인 할 수 있다.
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // 충돌한 obj를 가져와 obj가 Player일 경우 Skill을 발동시킨다.
                GameObject obj = hit.collider.gameObject;

                if (obj.CompareTag("EventOBJ"))
                {
                    Debug.Log("Click Farm!");
                }
            }

        }
    }
}
