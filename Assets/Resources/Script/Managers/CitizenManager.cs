using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

/*
 *  시민들을 관리하는 스크립트.
 *   Create_Citizen() : 시민을 생성. 한번 만들었었던 시민이어서 citizen_info가 존재하는지에 따라 실행되는 함수가 다르다.
 *   Check_Set_House() : 매개변수 House에 해당하는 시민이 있는지 반환. 없으면 null을 반환.
 *   Check_Talk() : 시민이 호출. 충돌한 상대방이 대화 가능한지 체크를 반환하는 함수.
 */
public class CitizenManager : MonoBehaviour
{
    public GameObject Citizen_Prefab;                       
    private List<Citizen_Info> Citizen_Infos = new List<Citizen_Info>();                            // 초기 시민들의 정보
    public List<Citizen_Info> Citizens = new List<Citizen_Info>();                                     // 유저가 보유하고있는 시민들의 정보

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
        int Citizen_Id = Random.Range(1, Citizen_Infos.Count);
        Citizen_Info info = Citizen_Infos[Citizen_Id];

        GameObject citizen = Instantiate(Citizen_Prefab, this.gameObject.transform) as GameObject;
        Citizen_Action citizen_action = citizen.GetComponent<Citizen_Action>();
        citizen.transform.localPosition = house.gameObject.transform.position + new Vector3(0f, 0f, -1.2f);

        citizen_action.Info =(Citizen_Info) info.Clone();
        citizen_action.Set_House(house);
        int Model_Index = citizen_action.Set_Model();
        citizen_action.Info.Model_Index = Model_Index;
        citizen_action.Info.ID = Citizen_Id;
        citizen_action.Info.HP = citizen_action.Info.Max_HP;   
        
        Citizens.Add(citizen_action.Info);

        Set_DB_User_CitizenData(citizen_action.Info);
        citizen_action.Set_Active();
    }
    public void Create_Citizen(GameObject house, Citizen_Info citizen_info)
    {
        GameObject citizen = Instantiate(Citizen_Prefab, this.gameObject.transform) as GameObject;
        Citizen_Action citizen_action = citizen.GetComponent<Citizen_Action>();
        citizen_action.Info = citizen_info;
        Citizen_Info another_info = Citizen_Infos[citizen_info.ID];
        citizen_action.Info.Name = another_info.Name;


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



    // 이하는 네트워크 관련 함수.

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

    public void Set_DB_User_CitizenData(Citizen_Info Info)
    {
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "Set_User_CitizensData");

        sendData.Add("user_index", GameManager.Get_Inctance().Get_UserIndex());
        sendData.Add("citizen_type", (int)Info.Type);

        sendData.Add("citizen_id", Info.ID);
        sendData.Add("model_type", Info.Model_Index);
        sendData.Add("level", Info.Level);
        sendData.Add("max_hp", Info.Max_HP);
        sendData.Add("hp", Info.HP);
        sendData.Add("max_tiredness", Info.Max_Tiredness);
        sendData.Add("tiredness", Info.Tiredness);
        sendData.Add("charm", Info.Charm);
        sendData.Add("exp", Info.Exp);
        sendData.Add("home_index", Info.Home_Index);

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, Reply_Data_Null));
    }


    void Reply_Data_Null(string json)
    {

    }
}
public class Citizen_Info
{
    public float Max_HP;
    public float Max_Tiredness;
    public int Charm;
    public string Name;
    public CITIZEN_TYPE Type;

    public int ID;
    public string Type_Explanation;
    public int Level = 1;
    public float Exp = 0;
    public float HP;
    public float Tiredness;
    public int Model_Index = -1;
    public int Home_Index = -1;

    public object Clone()
    {
        Citizen_Info info = new Citizen_Info();
        info.Level = this.Level;
        info.Exp = this.Exp;
        info.HP = this.HP;
        info.Tiredness = this.Tiredness;
        info.Max_HP = this.Max_HP;
        info.Max_Tiredness = this.Max_Tiredness;
        info.Charm = this.Charm;
        info.Type = this.Type;
        info.Name = this.Name;
        info.Model_Index = this.Model_Index;


        return info;
    }


}
public enum CITIZEN_STATE
{
    NONE,
    WALK,
    TALK,
    FAMMING,
    RESTING,
}
public enum CITIZEN_TYPE
{
    CHEF,
    MERCHANT,
    HUNTER,
}
public enum CITIZEN_ANI
{
    IDEL = 0,
    WALK,
    RUN,
    TUMBLING,
    SIT,
    SIT_IDLE,
    STAND,
    TALK,
    FARMMING,
}
public enum CITIZEN_EFFECT
{
    GET_ITEM = 0,
}
