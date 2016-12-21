using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Citizen_Variable : MonoBehaviour {
    public CITIZEN_STATE State = CITIZEN_STATE.NONE;
    protected Queue<CITIZEN_STATE> Events = new Queue<CITIZEN_STATE>();

    public Class_Citizen Info  = new Class_Citizen();

    protected float Walk_Speed = 1.0f;

    protected float Talk_Time = 3.0f;
    public Citizen_Action Talk_Target = null;
    protected float Max_Loneliness = 100f; 
    public float Loneliness = 0f;

    public ParticleSystem[] Effects;

    public GameObject Select_House;
    public GameObject Event_OBJ;

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
