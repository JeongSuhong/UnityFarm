using UnityEngine;
using System.Collections;

public class EventOBJ_Action : MonoBehaviour {

    public bool Is_Install = true;
    public bool Is_SaveItem = false;

    public virtual void Start_Action()
    {

    }
    public virtual void Install_Action()
    {

    }

    void OnEnable()
    {
        Is_Install = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("EventOBJ"))
        {
            Is_Install = false;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.CompareTag("EventOBJ"))
        {
            Is_Install = true;
        }
    }
}
