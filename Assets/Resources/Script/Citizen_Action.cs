using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

/*
 *  시민의 행동이나 정보를 관리하는 스크립트.
 *   Set_House() : 시민의 House를 매개변수로 받은 house로 바꿈.
 *   Set_Model() : 시민의 모델을 활성화시키고 변수에 저장.
 *   Set_Active(), C_Set_Active() : 시민의 AI를 활성화.
 *   C_Walk() : 시민의 상태가 WALK일때 실행, 자기 집 근처를 랜덤하게 돌아다님.
 *   C_Talk() : 시민의 외로움이 MAX이고 다른 시민과 충돌했을때 실행. 대화 애니메이션을 일정 시간동안 플레이.
 *   C_Resting() : 시민의 피로도가 MAX일때 실행, 할당된 집으로 돌아가 피로도가 0이 될때까지 모델 비활성화됨.
 *   C_Farmming() : 시민의 상태가 NONE이고 밭과 충돌할때 실행, 밭에서 랜덤하게 아이템 획득.
 *   Check_Event() : EventOBJ와 충돌했을때 해당 OBJ에 관한 Event가 가능한지 체크 후 상태 변환.
 */
public class Citizen_Action : Citizen_Variable {

    public void Set_House(GameObject house)
    {
        Info.Home_Index = house.GetComponent<BulidingOBJ_Action>().Obj_Index;
        Select_House = house;
    }
    public int Set_Model()
    {
        GameObject Citizen_Model = transform.FindChild("Model").gameObject;

        if (Info.Model_Index != -1)
        {
            Citizen_Model.transform.GetChild(Info.Model_Index).gameObject.SetActive(true);
        }
        else
        {
            int Model_Index = Random.Range(0, Citizen_Model.transform.childCount);
            Citizen_Model.transform.GetChild(Model_Index).gameObject.SetActive(true);
            Info.Model_Index = Model_Index;
        }

        Transform effects = transform.FindChild("Effect");
        Effects = new ParticleSystem[effects.childCount];
        for (int i = 0; i < effects.childCount; i++)
        {
            Effects[i] = effects.GetChild(i).GetComponent<ParticleSystem>();
        }
        Ani = Citizen_Model.transform.GetChild(Info.Model_Index).gameObject.GetComponent<Animation>();

        return Info.Model_Index;
    }

    public void Set_Active()
    {
        State = CITIZEN_STATE.NONE;
        StartCoroutine(C_Set_Active());
    }
    private IEnumerator C_Set_Active()
    {
        while(true)
        {
            if (Events.Count != 0)
            {
                State = Events.Dequeue();
            }
            else
            {
                if (Info.Tiredness == Info.Max_Tiredness)
                {
                    State = CITIZEN_STATE.RESTING;
                }
                else if (Loneliness == Max_Loneliness)
                {
                    if (Talk_Target != null)
                    {
                        State = CITIZEN_STATE.TALK;
                    }
                }

                if (State == CITIZEN_STATE.NONE)
                {
                    Set_Ani(CITIZEN_ANI.IDEL);
                    State = CITIZEN_STATE.WALK;
                }
            }

            switch(State)
            {
                case CITIZEN_STATE.WALK:
                    {
                        Set_Ani(CITIZEN_ANI.WALK);
                        yield return StartCoroutine("C_Walk");
                        break;
                    }

                case CITIZEN_STATE.TALK:
                    {
                        Set_Ani(CITIZEN_ANI.TALK);
                        yield return StartCoroutine("C_Talk");
                        break;
                    }

                case CITIZEN_STATE.RESTING:
                    {
                        yield return StartCoroutine("C_Resting");
                        break;
                    }

                case CITIZEN_STATE.FAMMING:
                    {
                        yield return StartCoroutine("C_Farmming");
                        break;
                    }
            }
            yield return null;
        }
    }

    private IEnumerator C_Walk()
    {
        float x = Random.Range(-8f, 8f);
        float z = Random.Range(-8f, 8f);
        Vector3 pos = Select_House.transform.position + new Vector3(x, 0f, z);
        pos.y = transform.localPosition.y;
        float distance = Mathf.Abs(Vector3.Distance(gameObject.transform.localPosition, pos));

        while (distance > 0.1f && State == CITIZEN_STATE.WALK)
        {
            distance = Mathf.Abs(Vector3.Distance(gameObject.transform.localPosition, pos));
            Vector3 v = pos - transform.localPosition;

            transform.localRotation = Quaternion.LookRotation(v);
            transform.Translate(Vector3.forward * Time.deltaTime * Walk_Speed);

            Increase_Loneliness(Time.deltaTime * 5f);
            Increase_Tiredness(Time.deltaTime * 5f);
            yield return null;
        }

        yield break;
    }
    private IEnumerator C_Talk()
    {
        Talk_Target.Talk_Target = this;
        Talk_Target.State = CITIZEN_STATE.TALK;

        Vector3 v = Talk_Target.transform.localPosition - transform.localPosition;
        transform.localRotation = Quaternion.LookRotation(v);

        while (State == CITIZEN_STATE.TALK)
        {
            if (Talk_Target == null || Loneliness <= 0f)
            {
                State = CITIZEN_STATE.NONE;
            }
            if (Talk_Target.State != CITIZEN_STATE.TALK)
            {
                State = CITIZEN_STATE.NONE;
            }

            Increase_Loneliness(-Time.deltaTime * 10f);
            yield return null;
        }
    }
    private IEnumerator C_Resting()
    {
        Set_Ani(CITIZEN_ANI.WALK);
        float distance = Mathf.Abs(Vector3.Distance(gameObject.transform.localPosition, Select_House.transform.position));

        while (distance > 1f)
        {
            distance = Mathf.Abs(Vector3.Distance(gameObject.transform.localPosition, Select_House.transform.position));
            Vector3 v = Select_House.transform.position - transform.localPosition;

            transform.localRotation = Quaternion.LookRotation(v);
            transform.Translate(Vector3.forward * Time.deltaTime * Walk_Speed);

            yield return null;
        }

        transform.GetChild(0).gameObject.SetActive(false);
        House_Sleep_Gauge_Action Gauge =  Select_House.GetComponent<House_Action>().Set_Sleep_UI(true);

        while(Info.Tiredness > 0)
        {
            Increase_Tiredness(-Time.deltaTime * 5f);
            Gauge.Set_Gauge(Info.Max_Tiredness, Info.Tiredness, Select_House.transform.position);
            yield return null;
        }

        Select_House.GetComponent<House_Action>().Set_Sleep_UI(false);
        transform.GetChild(0).gameObject.SetActive(true);
        State = CITIZEN_STATE.NONE;

        yield break;

    }
    private IEnumerator C_Farmming()
    {
        while(Event_OBJ.GetComponent<Farm_Action>() == null)
        {
            yield return null;
        }

        if(Event_OBJ.GetComponent<Farm_Action>().State != Farm_Action.FARM_STATE.NONE)
        {
            State = CITIZEN_STATE.NONE;
            yield break;
        }

        Set_Ani(CITIZEN_ANI.WALK);

        float distance = Mathf.Abs(Vector3.Distance(gameObject.transform.localPosition, Event_OBJ.transform.position));

        while (distance > 0.1f)
        {
            distance = Mathf.Abs(Vector3.Distance(gameObject.transform.localPosition, Event_OBJ.transform.position));
            Vector3 v = Event_OBJ.transform.position - transform.localPosition;

            transform.localRotation = Quaternion.LookRotation(v);
            transform.Translate(Vector3.forward * Time.deltaTime * Walk_Speed);
            yield return null;
        }


        Set_Ani(CITIZEN_ANI.FARMMING);

        yield return new WaitForSeconds(3f);

        Set_Effect(CITIZEN_EFFECT.GET_ITEM);

        CropInfo get_crop = CropsManager.Get_Inctance().Get_CropInfo(2);

        UserManager.Get_Inctance().Obtain_Crop(get_crop.ID, 1, 0);
        // test Leaf

        UIManager.Get_Inctance().Set_Get_Item_Icon(gameObject, "leaf", 1, ITEM_UI_TYPE.CROP);

        Increase_Tiredness(-10);
        State = CITIZEN_STATE.NONE;
        yield break;
    }

    void Check_Event(string obj_name)
    {
        switch(obj_name)
        {
            case "Farm":
                {
                    if (State != CITIZEN_STATE.WALK) { return; }
                    State = CITIZEN_STATE.FAMMING;
                    break;
                }
        }
    }


    void Increase_Loneliness(float value)
    {
        Loneliness += value;

        if(Loneliness <= 0) { Loneliness = 0; }
        if(Loneliness >= Max_Loneliness) { Loneliness = Max_Loneliness; }
    }
    void Increase_Tiredness(float value)
    {
        Info.Tiredness += value;

        if (Info.Tiredness <= 0) { Info.Tiredness = 0; }
        if (Info.Tiredness >= Info.Max_Tiredness) { Info.Tiredness = Info.Max_Tiredness; }
    }

    void Set_Ani(CITIZEN_ANI ani)
    {
        Ani.Play(Ani_Names[(int)ani], PlayMode.StopAll);
    }
    void Set_Effect(CITIZEN_EFFECT effect)
    {
        Effects[(int)effect].Play();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Citizen"))
        {
            // Check Talk
            if (State != CITIZEN_STATE.TALK && Loneliness > 50f)
            {
                if (CitizenManager.Get_Inctance().Check_Talk(col.gameObject.GetComponent<Citizen_Action>()))
                {
                    Talk_Target = col.gameObject.GetComponent<Citizen_Action>();
                    State = CITIZEN_STATE.TALK;
                }
            }
        }
        else if (col.gameObject.CompareTag("EventOBJ"))
        {
            if(!col.GetComponent<BulidingOBJ_Action>().Check_Is_Install || !col.GetComponent<BulidingOBJ_Action>().Check_Buy_Item) { return; }


            Event_OBJ = col.gameObject;
            Check_Event(col.gameObject.name);
        }
    }
}