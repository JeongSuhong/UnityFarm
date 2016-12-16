using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Citizen_Action : Citizen_Variable {

    void Awake()
    {
        GameObject Citizen_Model = transform.FindChild("Model").gameObject;
        int R = Random.Range(0, Citizen_Model.transform.childCount);
        Citizen_Model.transform.GetChild(R).gameObject.SetActive(true);

        Transform effects = transform.FindChild("Effect");
        Effects = new ParticleSystem[effects.childCount];
        for(int i = 0; i < effects.childCount; i++)
        {
            Effects[i] = effects.GetChild(i).GetComponent<ParticleSystem>();
        }
        Ani = Citizen_Model.transform.GetChild(R).gameObject.GetComponent<Animation>();


        //test

        Loneliness = 100f;
    }

    public void Set_House(GameObject house)
    {
        Select_House = house;
    }

    public void Set_Active()
    {
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
                if (Tiredness == Max_Tiredness)
                {
                    State = CITIZEN_STATE.RESTING;
                }

                if (State == CITIZEN_STATE.NONE)
                {
                    Set_Ani(CITIZEN_ANI.IDEL);
                    State = CITIZEN_STATE.WALK;
                }
            }

            switch(State)
            {
                case CITIZEN_STATE.NONE:
                    {
                        break;
                    }

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

        while(Tiredness > 0)
        {
            Increase_Tiredness(-Time.deltaTime * 5f);
            Gauge.Set_Gauge(Max_Tiredness, Tiredness, Select_House.transform.position);
            yield return null;
        }

        Select_House.GetComponent<House_Action>().Set_Sleep_UI(false);
        transform.GetChild(0).gameObject.SetActive(true);
        State = CITIZEN_STATE.NONE;

        yield break;

    }
    private IEnumerator C_Farmming()
    {
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

        UserManager.Get_Inctance().Obtain_Crop(0, 1);
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
        Tiredness += value;

        if (Tiredness <= 0) { Tiredness = 0; }
        if (Tiredness >= Max_Tiredness) { Tiredness = Max_Tiredness; }
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
            Event_OBJ = col.gameObject;
            Check_Event(col.gameObject.name);
        }
    }
}