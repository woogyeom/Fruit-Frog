using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    private int cloneCount = 0;
    private Danger dangerScript;

    int level = 0;
    float timer;
    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        InvokeRepeating("ConstantSpawn", 0f, 1.5f);
        dangerScript = GetComponent<Danger>();
    }

    void ConstantSpawn()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        Spawn(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        timer += Time.deltaTime;
        level = Mathf.FloorToInt(GameManager.instance.gameTime / 60);

        if (level >= 8 && !GameManager.instance.isBossSpawned)
        {
            dangerScript.warningText.enabled = true;
            dangerScript.Init();
            GameManager.instance.isBossSpawned = true;
            Invoke("SpawnBossWithDelay", 3f); 
        }
        
        if (timer > 4)
        {
            timer = 0;
            for (int i = 0; i <= level + 1; i++)
            {
                int mob = Random.Range(0, Mathf.Min(level + 1, spawnData.Length));
                Spawn(mob);
            }
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

    void SpawnBossWithDelay()
    {
        BossSpawn(8);
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
