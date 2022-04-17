using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject PauseCanvas;
    public GameManager GM;
    public AudioSource PauseAudio;
    public UserInfo User;

    void Start()
    {
        
    }

    
    void Update()
    {
        CheckMenu();
    }

    public void CheckMenu()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseAudio.Play();
            if(GM.IsPause)
            {
                ReturnToGame();
            }
            else
            {
                PauseCanvas.SetActive(true);
                GM.IsPause = true;
                Time.timeScale = 0;
                foreach(AudioSource temp in GM.AudioList)
                    temp.Pause();
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void ReturnToMenu()
    {
        UpdateUserInfo();
        SceneManager.LoadScene("BeginningScene");
        Time.timeScale = 1;
    }

    public void ReturnToGame()
    {
        PauseCanvas.SetActive(false);
        GM.IsPause = false;
        Time.timeScale = 1;
        foreach(AudioSource temp in GM.AudioList)
            temp.UnPause();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }

    public void UpdateUserInfo()
    {
        User = UserInfo.GetInstance();
        int num = int.Parse(User.GameTimes);
        User.GameTimes = (num + 1).ToString();
        num = int.Parse(User.MaxLevelRecord);
        if(num < GM.GameLevel - 1)
            User.MaxLevelRecord = (GM.GameLevel - 1).ToString();
    }
}
