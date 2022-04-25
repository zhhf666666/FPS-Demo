using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeginningController : MonoBehaviour
{
    public float RotateSpeed = 180;
    public GameObject Buttons;
    public GameObject Login;
    public Text UserName;
    public Text GameTimes;
    public Text MaxLevelRecord;
    public Text AlertText;
    public float AlertTime = 2;
    public UserInfo User;
    private ClientSocket CS = new ClientSocket();
    public InputField UserNameInput;
    public InputField PasswordInput;
    private const string IP = "127.0.0.1";
    //private const string IP = "185.3.87.160";
    private const int PORT = 6666;

    void Start()
    {
        if(UserInfo.DoesExist() == false)
        {
            FirstEnterGame();
        }
        else
        {
            User = UserInfo.GetInstance();
            NotFirstEnterGame();
            if(User.UserName != "游客")
                StartCoroutine("SendUserInfo");
        }
    }

    void Update()
    {
        this.transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime);
        CheckMessage();
    }

    public void FirstEnterGame()
    {
        Login.SetActive(true);
        Buttons.SetActive(false);
    }

    public void NotFirstEnterGame()
    {
        Login.SetActive(false);
        Buttons.SetActive(true);
        SetUserInformation();
    }

    public void SetUserInformation()
    {
        UserName.text = "用户名: " + User.UserName;
        GameTimes.text = "游玩次数: " + User.GameTimes;
        MaxLevelRecord.text = "最高关卡记录: " + User.MaxLevelRecord;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ReturnToLogin()
    {
        User.DeleteInstance();
        FirstEnterGame();
    }

    IEnumerator DisplayAlert(string s)
    {
        AlertText.text = s;
        yield return new WaitForSeconds(AlertTime);
        AlertText.text = "";
    }

    public void OfflineLogin()
    {
        User = UserInfo.GetInstance();
        User.UserName = "游客";
        User.GameTimes = "0";
        User.MaxLevelRecord = "0";
        NotFirstEnterGame();
    }

    private void Send(JSONObject temp)
    {
        string temp2 = JSONConvert.SerializeObject(temp);
        byte[] data = System.Text.Encoding.UTF8.GetBytes(temp2);
        CS.SendData(data);
    }

    public void OnlineLogin()
    {
        StartCoroutine("SendOnlineLogin");
    }

    IEnumerator SendOnlineLogin()
    {
        if(UserNameInput.text.Length == 0 || PasswordInput.text.Length == 0)
        {
            StopCoroutine("DisplayAlert");
            StartCoroutine("DisplayAlert", "用户名或密码不能为空!");
            yield break;
        }
        if(CS.connected == false)
        {
            CS.Connect(IP, PORT);
            if(CS.connected == false)
            {
                StopCoroutine("DisplayAlert");
                StartCoroutine("DisplayAlert", "连接服务端失败");
                yield break;
            }
        }
        JSONObject obj = new JSONObject();
        obj["Title"] = "Login";
        obj["UserName"] = UserNameInput.text;
        obj["Password"] = PasswordInput.text;
        Send(obj);
    }

    IEnumerator SendUserInfo()
    {
        if(CS.connected == false)
        {
            CS.Connect(IP, PORT);
            if(CS.connected == false)
            {
                StopCoroutine("DisplayAlert");
                StartCoroutine("DisplayAlert", "连接服务端失败");
                yield break;
            }
        }
        JSONObject obj = new JSONObject();
        obj["Title"] = "UpdateUserInfo";
        obj["UserName"] = User.UserName;
        obj["GameTimes"] = User.GameTimes;
        obj["MaxLevelRecord"] = User.MaxLevelRecord;
        Send(obj);
    }

    public void CheckMessage()
    {
        if(CS.connected)
        {
            CS.BeginReceive();
        }
        string msg = CS.GetMsgFromQueue();
        if(!string.IsNullOrEmpty(msg))
        {
            JSONObject obj = JSONConvert.DeserializeObject(msg);
            if(obj["Title"].ToString() == "Error")
            {
                StopCoroutine("DisplayAlert");
                StartCoroutine("DisplayAlert", obj["Message"].ToString());
                PasswordInput.text = "";
                CS.CloseSocket();
            }
            else if(obj["Title"].ToString() == "UserInformation" && User == null)
            {
                User = UserInfo.GetInstance();
                User.UserName = obj["UserName"].ToString();
                User.GameTimes = obj["GameTimes"].ToString();
                User.MaxLevelRecord = obj["MaxLevelRecord"].ToString();
                CS.CloseSocket();
                NotFirstEnterGame();
            }
        }
    }
}