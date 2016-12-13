using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CitizenManager : MonoBehaviour {

    public List<GameObject> Citizens = new List<GameObject>();

    void Start()
    {
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            Citizens.Add(gameObject.transform.GetChild(i).gameObject);
        }
    }

}
