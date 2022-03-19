using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int GameLevel = 1;
    public GameObject Player;
    public List<GameObject> EnemyBomb = new List<GameObject>();
    public List<GameObject> EnemyPistol = new List<GameObject>();
    public List<GameObject> EnemyRifle = new List<GameObject>();
    public GameObject EnemyBombPre;
    public GameObject EnemyPistolPre;
    public GameObject EnemyRiflePre;

    void Start()
    {
    
    }

    void Update()
    {
        
    }

    public void Init()
    {
        GameLevel = 1;
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
    }
}
