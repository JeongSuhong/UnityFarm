using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CitizenManager : MonoBehaviour {

    public GameObject Citizen_Prefab;
    public List<GameObject> Citizens = new List<GameObject>();

    private static CitizenManager instance = null;

    public static CitizenManager Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(CitizenManager)) as CitizenManager;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("CitizenManager ");
            instance = obj.AddComponent(typeof(CitizenManager)) as CitizenManager;

            Debug.Log("Fail to get CitizenManager Instance");
        }
        return instance;
    }

    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            Citizens.Add(gameObject.transform.GetChild(i).gameObject);
        }
    }

    public void Create_Citizen(GameObject house)
    {
        GameObject Citizen = Instantiate(Citizen_Prefab, this.gameObject.transform) as GameObject;
        Citizen.GetComponent<Citizen_Action>().Set_House(house);

        GameObject Citizen_Model = Citizen.transform.FindChild("Model").gameObject;

        int R = Random.Range(0, Citizen_Model.transform.childCount);

        Citizen_Model.transform.GetChild(R).gameObject.SetActive(true);

        Citizen.transform.localPosition = house.gameObject.transform.position + new Vector3(0f, 0f, -1.2f);
        Citizen.GetComponent<Citizen_Action>().Set_Active();

    }
    public GameObject Check_Set_House(GameObject house)
    {
        for(int i = 0; i < Citizens.Count; i++)
        {
            if(Citizens[i].GetComponent<Citizen_Action>().Select_House == house)
            {
                return Citizens[i];
            }
        }

        return null;
    }

}
