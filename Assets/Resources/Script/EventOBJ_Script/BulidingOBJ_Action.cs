using UnityEngine;
using System.Collections;

public class BulidingOBJ_Action : MonoBehaviour {

    public Item Info = new Item();

    public bool Is_Install = false;
    public bool Check_Is_Install = true;
    public bool Is_SaveItem = false;

    public virtual void Start_Action()
    {

    }
    public virtual void Install_Action()
    {

    }

    void OnEnable()
    {
        Check_Is_Install = true;
    }

    void Update()
    {
       Debug.Log(  Info.Obj_Index );
    }

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

    public void Set_Info_ObjIndex(int index)
    {
        Info.Obj_Index = index;
    }

}
