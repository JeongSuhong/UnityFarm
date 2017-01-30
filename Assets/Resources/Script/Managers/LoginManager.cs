using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using System;

/*
 *  로그인 화면을 담당하는 스크립트. 
 * 
 */
public class LoginManager : MonoBehaviour {

    public GameObject MainUI;

    public UIPanel LoginUI;
    public UIPanel JoinUI;

    public UIInput Login_ID;
    public UIInput Login_PW;
    public UIInput Join_ID;
    public UIInput Join_PW;

    void Awake()
    {
        LoginUI.alpha = 0;
        JoinUI.alpha = 0;
    }

    public void View_LoginUI()
    {
        MainUI.SetActive(false);
        LoginUI.alpha = 1;
        JoinUI.alpha = 0;
    }
    public void View_JoinUI()
    {
        LoginUI.alpha = 0;
        JoinUI.alpha = 1;
    }

    public void Set_Login()
    {
        if (Login_ID.value.Equals("아이디를 입력해주세요") || Login_PW.value.Equals("비밀번호를 입력해주세요"))
        {
            Debug.Log("계정  및 암호가 입력되지 않았습니다. 확인하고 다시 시도 하시기 바랍니다.");
            return;
        }

        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "SetLogin");

        sendData.Add("user_id", Login_ID.value);
        sendData.Add("user_pw", Login_PW.value);
        sendData.Add("login_time", DateTime.Now);

        Debug.Log("로그인 시간 " + DateTime.Now.ToString());

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, ReplyLogin));
    }
    void ReplyLogin(string json)
    {
        // JSON Data 변환
        int index = JsonReader.Deserialize<int>(json);

        GameManager.Get_Inctance().Set_UserIndex(index);
        // 회원 가입에 성공 했으므로 바로 로그인을 시도한다.
        GameManager.Get_Inctance().Start_Login();
    }

    public void Set_Join()
    {
        if (Join_ID.value.Equals("아이디를 입력해주세요") || Join_PW.value.Equals("비밀번호를 입력해주세요"))
        {
            Debug.Log("계정  및 암호가 입력되지 않았습니다. 확인하고 다시 시도 하시기 바랍니다.");
            return;
        }

        if (Join_ID.value.Length < 2 || Join_PW.value.Length < 1)
        {
            Debug.Log("계정과 암호는 4글자 이상으로 만들어야 합니다. 확인하고 다시 시도 하시기 바랍니다.");
            return;
        }

        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "SetJoin");

        sendData.Add("user_id", Join_ID.value);
        sendData.Add("user_pw", Join_PW.value);
        sendData.Add("join_time", DateTime.Now);

        Debug.Log("가입 시간 " + DateTime.Now.ToString());

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, ReplyLogin));
    }
}
