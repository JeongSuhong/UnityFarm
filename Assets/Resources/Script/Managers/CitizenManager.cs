using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

public class CitizenManager : MonoBehaviour
{
    public GameObject Citizen_Prefab;
    private List<Citizen_Info> Citizen_Infos = new List<Citizen_Info>();
    public List<Citizen_Info> Citizens = new List<Citizen_Info>();

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
        Get_DB_CitizenInfo();
        Get_DB_User_CitizenData();
    }

    public void Create_Citizen(GameObject house)
    {
        int Citizen_Id = Random.Range(0, Citizen_Infos.Count);
        Citizen_Info info = Citizen_Infos[Citizen_Id];
        info.HP = info.Max_HP;

        GameObject citizen = Instantiate(Citizen_Prefab, this.gameObject.transform) as GameObject;
        Citizen_Action citizen_action = citizen.GetComponent<Citizen_Action>();
        info.Home_Index = house.GetComponent<House_Action>().Info.Obj_Index;
        citizen_action.Set_House(house);
        citizen.transform.localPosition = house.gameObject.transform.position + new Vector3(0f, 0f, -1.2f);
        int Model_Index = citizen_action.Set_Model();
        info.Model_Index = Model_Index;
        citizen_action.Info = info;
        
        Citizens.Add(citizen_action.Info);

        citizen_action.Set_DB_User_CitizenData();
        citizen_action.Set_Active();
    }
    public void Create_Citizen(GameObject house, Citizen_Info citizen_info)
    {
        GameObject citizen = Instantiate(Citizen_Prefab, this.gameObject.transform) as GameObject;
        Citizen_Action citizen_action = citizen.GetComponent<Citizen_Action>();
        citizen_action.Info = citizen_info;
        citizen.transform.localPosition = house.gameObject.transform.position + new Vector3(0f, 0f, -1.2f);
        citizen_action.Set_House(house);
        citizen_action.Set_Model();
        citizen_action.Set_Active();

        Citizens.Add(citizen_action.Info);
    }
    public Citizen_Info Check_Set_House(int house_index)
    {
        for (int i = 0; i < Citizens.Count; i++)
        {
            if (Citizens[i].Home_Index == house_index)
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

    private void Get_DB_CitizenInfo()
    {
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Get_CitizenInfo");

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Get_DB_CitizenInfo));
    }
    private void Reply_Get_DB_CitizenInfo(string json)
    {
        Dictionary<string, object> dataDic = (Dictionary<string, object>)JsonReader.Deserialize(json, typeof(Dictionary<string, object>));

        foreach (KeyValuePair<string, object> info in dataDic)
        {
            Citizen_Info data = JsonReader.Deserialize<Citizen_Info>(JsonWriter.Serialize(info.Value));
            Citizen_Infos.Add(data);
        }
    }

    private void Get_DB_User_CitizenData()
    {
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Get_User_CitizenData");

        sendData.Add("user_index", GameManager.Get_Inctance().Get_UserIndex());

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Get_DB_User_CitizenData));
    }
    private void Reply_Get_DB_User_CitizenData(string json)
    {
        Dictionary<string, object> dataDic = (Dictionary<string, object>)JsonReader.Deserialize(json, typeof(Dictionary<string, object>));

        foreach (KeyValuePair<string, object> info in dataDic)
        {
            Citizen_Info data = JsonReader.Deserialize<Citizen_Info>(JsonWriter.Serialize(info.Value));
            Citizens.Add(data);
        }
    }
}


public class Citizen_Info
{
    public float Max_HP;
    public float Max_Tiredness;
    public int Charm;
    public string Name;
    public CITIZEN_TYPE Type;

    public int id;
    public string Type_Explanation;
    public int Level = 1;
    public float Exp = 0;
    public float HP;
    public float Tiredness;
    public int Model_Index = -1;
    public int Home_Index = -1;


}
