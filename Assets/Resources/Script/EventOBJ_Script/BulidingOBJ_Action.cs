using UnityEngine;
using System.Collections;

/*
    설치될 수 있는 모든 오브젝트의 부모 스크립트.
    Start_Action() : 오브젝트를 클릭 시 실행된다
    Install_Action() : 오브젝트가 설치 됬을 시 실행된다.
*/


public class BulidingOBJ_Action : MonoBehaviour {

    public Item Info = new Item();

    public int Obj_Index = -99;

    public bool Check_Buy_Item = false;
    public bool Is_Install = false;
    public Vector3 Origin_Position = Vector3.zero;
    public Vector3 Origin_Rotation = Vector3.zero;
    
    public virtual void Start_Action()
    {

    }
    public virtual void Install_Action()
    {
        Origin_Position = transform.localPosition;
        Origin_Rotation = transform.localRotation.eulerAngles;
    }

    void OnEnable()
    {
        Check_Is_Install = true;
    }

    // 아래는 설치 예시를 위한 샘플 오브젝트가 사용한다.
    // 샘플 오브젝트가 다른 오브젝트와 충돌했는지 판단한다.

    public bool Check_Is_Install = true;
    public bool Is_SaveItem = false;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("EventOBJ"))
        {
            Check_Is_Install = false;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.CompareTag("EventOBJ"))
        {
            Check_Is_Install = true;
        }
    }

    public void Set_Buy()
    {
        Check_Buy_Item = true;
    }
    public void Set_Install()
    {
        Is_Install = true;
    }
}
