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
    public static UserInfo User;

    void Start()
    {
        if(User == null)
        {
            FirstEnterGame();
        }
        else
        {
            NotFirstEnterGame();
        }
    }

    void Update()
    {
        this.transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime);
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
        GameTimes.text = "游玩次数: " + User.GameTimes.ToString();
        MaxLevelRecord.text = "最高关卡记录: " + User.MaxLevelRecord.ToString();
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
        User = null;
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
        User = new UserInfo("游客");
        NotFirstEnterGame();
    }

    public void OnlineLogin()
    {
        string content = "111";
        StopCoroutine("DisplayAlert");
        StartCoroutine("DisplayAlert", content);
    }
}