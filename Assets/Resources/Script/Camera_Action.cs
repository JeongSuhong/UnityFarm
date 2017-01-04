using UnityEngine;
using System.Collections;

/*
 *  카메라이동을 담당하는 스크립트.
 *  드래그했을때 화면이 이동하는 부분을 처리함. 
 */

public class Camera_Action : MonoBehaviour
{
    public bool Not_CameraMoving = false;
    public float DragSpeed = 1;
    private Vector3 dragPosition;

    private static Camera_Action instance = null;

    public static Camera_Action Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(Camera_Action)) as Camera_Action;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("Camera_Action ");
            instance = obj.AddComponent(typeof(Camera_Action)) as Camera_Action;

            Debug.Log("Fail to get Camera_Action Instance");
        }
        return instance;
    }

    //  void Update() 는 일반적인 연산 및 Translate로 이동할시 사용
    //  void FixedUpdate()는Rigidbody를 이용한 연산시 사용함 ex) AddForce 등
    //  FixedUpdate는 일정한 시간마다 호출되기 때문에 정확한 물리연산을 필요로 할때 사용하면된다.
    void Update()
    {
        // 클릭한 곳의 Position값을 받는다. 한번 클릭한 순간에는 이동하지 않는다.
        if (Input.GetMouseButtonDown(0))
        {
            dragPosition = Input.mousePosition;
            return;
        }

        // 클릭중이 아니면 함수를 종료한다.
        if (!Input.GetMouseButton(0) || Not_CameraMoving) return;

        //  ScreenToViewportPoint(Position) = Position을 화면 공간에서 뷰포트(카메라) 공간으로 변경시킵니다.
        // Vector3 A - Vector3 B을 하면 A에서 B로가는 방향값만 나온다. (왼쪽? 오른쪽? )
        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragPosition);
        Vector3 move = new Vector3(pos.x * DragSpeed, 0, pos.y * DragSpeed);


        transform.Translate(move, Space.World);
    }

    public void Set_NotCameraMoving()
    {
        Not_CameraMoving = true;
    }
    public void Set_CameraMoving()
    {
        Not_CameraMoving = false;
    }
}
