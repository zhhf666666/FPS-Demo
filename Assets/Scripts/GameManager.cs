using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int GameLevel = 1;
    public Text GameLevelText;
    public GameObject Player;
    public List<GameObject> EnemyBomb = new List<GameObject>();
    public List<GameObject> EnemyPistol = new List<GameObject>();
    public List<GameObject> EnemyRifle = new List<GameObject>();
    public GameObject EnemyBombPre;
    public GameObject EnemyPistolPre;
    public GameObject EnemyRiflePre;
    public int LivingEnemy = 5;
    public Text LivingEnemyText;
    public Text AlertText;
    public int AlertTime = 3;
    public int LevelInterval = 10;
    public bool IsPause = false;
    public GameObject PickUpHealth;
    public GameObject PickUpBullet;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Init();
        SetText();
    }

    void Update()
    {
        
    }

    public void Init()
    {
        GameLevel = 1;
        LivingEnemy = 5;
        for(int i=0;i<2;i++)
        {
            GameObject temp = Instantiate(EnemyBombPre, this.transform);
            EnemyBomb.Add(temp);
            temp.GetComponent<EnemyController>().Birth();
        }
        for(int i=0;i<2;i++)
        {
            GameObject temp = Instantiate(EnemyPistolPre, this.transform);
            EnemyPistol.Add(temp);
            temp.GetComponent<EnemyController>().Birth();
        }
        for(int i=0;i<1;i++)
        {
            GameObject temp = Instantiate(EnemyRiflePre, this.transform);
            EnemyRifle.Add(temp);
            temp.GetComponent<EnemyController>().Birth();
        }
        Player.GetComponent<PlayerController>().Birth();
    }

    public void SetText()
    {
        LivingEnemyText.text = "剩余敌人: " + LivingEnemy.ToString();
        GameLevelText.text = "第" + GameLevel.ToString() + "关";
    }

    public void DecreaseEnemy()
    {
        LivingEnemy--;
        if(LivingEnemy == 0)
        {
            UpdateLevel();
        }
        else
        {
            SetText();
        }
    }

    public void UpdateLevel()
    {
        GameLevel++;
        SetText();
        StartCoroutine("UpdateLevelAlert");
        IncreaseEnemy();
    }

    public void ActivateEnemy()
    {
        for(int i=0;i<EnemyBomb.Count;i++)
        {
            EnemyBomb[i].GetComponent<EnemyController>().Birth();
        }
        for(int i=0;i<EnemyPistol.Count;i++)
        {
            EnemyPistol[i].GetComponent<EnemyController>().Birth();
        }
        for(int i=0;i<EnemyRifle.Count;i++)
        {
            EnemyRifle[i].GetComponent<EnemyController>().Birth();
        }
    }

    IEnumerator DisplayAlert(string s)
    {
        AlertText.text = s;
        yield return new WaitForSeconds(AlertTime);
        AlertText.text = "";
    }

    IEnumerator UpdateLevelAlert()
    {
        string content = "补给品已到达，10秒后敌人将再度来袭！";
        StopCoroutine("DisplayAlert");
        StartCoroutine("DisplayAlert", content);
        InitPickUp();
        yield return new WaitForSeconds(LevelInterval);
        content = "敌人来袭！";
        StopCoroutine("DisplayAlert");
        StartCoroutine("DisplayAlert", content);
        ActivateEnemy();
        LivingEnemy = GameLevel + 4;
        SetText();
    }

    public void InitPickUp()
    {
        Vector3 pos = new Vector3(Random.Range(-29f, 29f), 0, Random.Range(-29f, 29f));
        Instantiate(PickUpHealth, pos, this.transform.rotation);
        pos = new Vector3(Random.Range(-29f, 29f), 0, Random.Range(-29f, 29f));
        Instantiate(PickUpBullet, pos, this.transform.rotation);
    }

    public void IncreaseEnemy()
    {
        int ans = GameLevel % 3;
        if(ans == 0)
        {
            GameObject temp = Instantiate(EnemyBombPre, new Vector3(0, 0, 0), this.transform.rotation, this.transform);
            EnemyBomb.Add(temp);
        }
        else if(ans == 1)
        {
            GameObject temp = Instantiate(EnemyPistolPre, new Vector3(0, 0, 0), this.transform.rotation, this.transform);
            EnemyPistol.Add(temp);
        }
        else if(ans == 2)
        {
            GameObject temp = Instantiate(EnemyRiflePre, new Vector3(0, 0, 0), this.transform.rotation, this.transform);
            EnemyRifle.Add(temp);
        }
    }

    public void GetHealth()
    {
        int HP = Random.Range(8 * (GameLevel + 4), 12 * (GameLevel + 4));
        Player.GetComponent<HealthController>().GetHP(HP);
        string message = "获得" + HP.ToString() + "点生命值";
        StartCoroutine("DisplayAlert", message);
    }

    public void GetBullet()
    {
        int PistolBullet = Random.Range(2 * (GameLevel + 4), 4 * (GameLevel + 4));
        int RifleBullet = Random.Range(5 * (GameLevel + 4), 7 * (GameLevel + 4));
        var WeaponBulletController = Player.GetComponentsInChildren<BulletAmountController>();
        for(int i=0;i<WeaponBulletController.Length;i++)
        {
            if(WeaponBulletController[i].transform.name == "Pistol")
            {
                WeaponBulletController[i].AddBullet(PistolBullet);
            }
            else if(WeaponBulletController[i].transform.name == "Rifle")
            {
                WeaponBulletController[i].AddBullet(RifleBullet);
            }
        }
        string message = "获得" + PistolBullet.ToString() + "发手枪子弹和" + RifleBullet.ToString() + "发步枪子弹";
        StartCoroutine("DisplayAlert", message);
    }
}
