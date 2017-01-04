using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 시민에 관한 변수 스크립트 
 * Events : 시민이 처리해야할 중요 Event를 담아놓는 큐. 큐에 내용이 있으면 일반 이벤트를 끝내고 큐 이벤트 실행.
 * 
 */
public class Citizen_Variable : MonoBehaviour {

    public CITIZEN_STATE State = CITIZEN_STATE.NONE;
    protected Queue<CITIZEN_STATE> Events = new Queue<CITIZEN_STATE>();

    public Citizen_Info Info  = new Citizen_Info();

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