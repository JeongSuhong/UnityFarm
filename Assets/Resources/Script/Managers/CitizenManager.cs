using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CitizenManager : MonoBehaviour {

    public GameObject Citizen_Prefab;
    public List<Citizen_Action> Citizens = new List<Citizen_Action>();

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
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Citizens.Add(gameObject.transform.GetChild(i).gameObject.GetComponent<Citizen_Action>());
        }
    }

    public void Create_Citizen(GameObject house)
    {
        GameObject citizen = Instantiate(Citizen_Prefab, this.gameObject.transform) as GameObject;
        Citizen_Action citizen_action = citizen.GetComponent<Citizen_Action>();
        citizen_action.Set_House(house);
        citizen.transform.localPosition = house.gameObject.transform.position + new Vector3(0f, 0f, -1.2f);
        citizen_action.Set_Active();

        Citizens.Add(citizen_action);

    }
    public Citizen_Action Check_Set_House(GameObject house)
    {
        for (int i = 0; i < Citizens.Count; i++)
        {
            if (Citizens[i].Select_House == house)
            {
                return Citizens[i];
            }
        }

        return null;
    }

    public bool Check_Talk(Citizen_Action target)
    {
        if (target.State == CITIZEN_STATE.WALK && target.Loneliness > 50f)
        {
            return true;
        }

        return false;
    }
}

public class Class_Citizen
{
    public CITIZEN_TYPE Type = CITIZEN_TYPE.CHEF;
    public string Type_Explanation = "Type 설명문입니다. ";
    public string Name = "Test Name";
    public int Level = 1;
    public float Exp = 0;
    public float Max_HP = 100;
    public float HP = 100;
    public float Max_Tiredness = 100;
    public float Tiredness = 0;
    public int Charm= 1;


}
