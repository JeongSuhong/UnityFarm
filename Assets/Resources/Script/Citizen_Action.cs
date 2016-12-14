using UnityEngine;
using System.Collections;

public class Citizen_Action : MonoBehaviour {

    public GameObject Select_House;

    CITIZEN_STATE State = CITIZEN_STATE.NONE;
    CITIZEN_ANI Ani_State = CITIZEN_ANI.IDEL;
    float Speed = 1.0f;

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
            if (State == CITIZEN_STATE.NONE)
            {
                State = CITIZEN_STATE.WALK;
            }

            switch(State)
            {
                case CITIZEN_STATE.NONE:
                    {
                        break;
                    }

                case CITIZEN_STATE.WALK:
                    {
                        yield return StartCoroutine("C_Walk");
                        break;
                    }

                case CITIZEN_STATE.TALK:
                    {
                        break;
                    }

                case CITIZEN_STATE.RESTING:
                    {
                        break;
                    }

                case CITIZEN_STATE.FAMMING:
                    {
                        break;
                    }
            }
            yield return null;
        }
    }
    private IEnumerator C_Walk()
    {
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        Vector3 pos = Select_House.transform.position + new Vector3(x, 0f, z);
        pos.y = transform.localPosition.y;
        float distance = Mathf.Abs(Vector3.Distance(gameObject.transform.localPosition, pos));

        while (distance > 0.1f)
        {
            distance = Mathf.Abs(Vector3.Distance(gameObject.transform.localPosition, pos));
            Vector3 v = pos - transform.localPosition;

            transform.localRotation = Quaternion.LookRotation(v);
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);

            yield return null;
        }

        yield break;
    }

    public enum CITIZEN_STATE
    {
        NONE,
        WALK,
        TALK,
        FAMMING,
        RESTING,
    }
    public enum CITIZEN_ANI
    {
        IDEL = 0,
        WALK,
        RUN,
        DAMAGE,
        DIE,
        ATTACK_1,
        ATTACK_2,
        ATTACK_3,
        TUMBLING,
        JUMP,
        SIT,
        SIT_IDLE,
        STAND,
    }
}
