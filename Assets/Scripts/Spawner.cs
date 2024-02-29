using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;
    private int cloneCount = 0;

    int level = 0;
    float timer;
    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
        InvokeRepeating("ConstantSpawn", 0f, 1f);
    }

    void ConstantSpawn()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        Spawn(0);
        //Spawn(0);
        //Spawn(0);
        //Spawn(0);
        //Spawn(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        timer += Time.deltaTime;
        level = Mathf.FloorToInt(GameManager.instance.gameTime / 60) + 1;

        if (timer > (10 / level))
        {
            timer = 0;
            for (int i = 0; i <= level; i++)
            {
                int test = Random.Range(0, spawnData.Length);
                //Spawn(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            BossSpawn(8);
        }
    }

    void Spawn(int id)
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.name = "Enemy" + cloneCount;
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[id]);
        cloneCount++;
    }

    void BossSpawn(int level)
    {
        GameObject boss = GameManager.instance.pool.Get(8);
        boss.name = "Boss" + cloneCount;
        boss.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        boss.GetComponent<Boss>().Init();
        cloneCount++;
    }
}
[System.Serializable]
public class SpawnData
{
    public enum EnemyType {Melee, Ranged}
    public int spriteType;
    public int health;
    public float speed;
    public EnemyType enemyType;
    public int atk;
    public int exp;
}
