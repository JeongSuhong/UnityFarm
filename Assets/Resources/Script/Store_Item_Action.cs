using UnityEngine;
using System.Collections;

public class Store_Item_Action : MonoBehaviour {

    public Camera ItemOBJ_Camera;
    public Transform ItemOBJ_Position;
    GameObject ItemOBJ;

	void Start()
    {
        ItemOBJ = transform.FindChild("ItemModel").gameObject;
    }
    
    // Update is called once per frame
	void Update () {

       Vector3 pos = ItemOBJ_Camera.ScreenToViewportPoint(ItemOBJ_Position.position);

        pos.z = 0f;

        ItemOBJ.transform.position = pos;

        ItemOBJ.transform.GetChild(0).Rotate(Vector3.up * Time.deltaTime * 10f);
	}
}
