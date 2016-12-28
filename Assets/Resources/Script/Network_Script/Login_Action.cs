using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using JsonFx.Json;
using System;

public class Login_Action : MonoBehaviour {
   
    public UIInput Login_ID;
    public UIInput Login_PW;
    public UIInput Join_ID;
    public UIInput Join_PW;

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
        StartCoroutine(Login());
    }
    IEnumerator Login()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("Main");

        while (!async.isDone)
        {
            yield return null;
        }
        yield return null;
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
        sendData.Add("join_time", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"));

        Debug.Log(DateTime.Now.ToString());

        StartCoroutine(NetworkManager.Instance.ProcessNetwork(sendData, ReplyLogin));
    }
}
