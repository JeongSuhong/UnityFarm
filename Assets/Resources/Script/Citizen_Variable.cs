using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Citizen_Variable : MonoBehaviour {

    public GameObject Select_House;
    public GameObject Event_OBJ;

    public CITIZEN_STATE State = CITIZEN_STATE.NONE;
    protected Queue<CITIZEN_STATE> Events = new Queue<CITIZEN_STATE>();

    protected float Walk_Speed = 1.0f;

    protected float Talk_Time = 3.0f;
    public Citizen_Action Talk_Target = null;
    protected const float Max_Loneliness = 100f; 
    public float Loneliness = 0f;
    protected const float Max_Tiredness = 100f;
    public float Tiredness = 0f;


    protected CITIZEN_ANI Ani_State = CITIZEN_ANI.IDEL;
    protected  string[] Ani_Names = new string[] { "idle", "walk", "run", "tumbling", "sit", "sit_idle", "stand", "talk", "farmming" };
    protected  Animation Ani;
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
    TUMBLING,
    SIT,
    SIT_IDLE,
    STAND,
    TALK,
    FARMMING,
}

