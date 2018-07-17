using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{

    private static EnemyPool instance;

    public static EnemyPool Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private GameObject[] enemies;

    [SerializeField]
    private int size;
    [SerializeField]
    private int sizePerEnemy;
    [SerializeField]
    private int typeOfEnemies;

    [SerializeField]
    private List<GameObject> enemiesList;

    private GameObject created;
    public GameObject enemyToActivate;
    private EnemyBehaviour enemyBehaviour;
    [SerializeField]
    private float speedDirection;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            Prepare();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Prepare()
    {

        for (int i = 0; i < sizePerEnemy; i++)
        {

            for (typeOfEnemies = 0; typeOfEnemies < 10; typeOfEnemies++)
            {
                CreateEnemies(typeOfEnemies);
            }

        }
    }

    private void CreateEnemies(int type)
    {
        created = Instantiate(enemies[type]);
        created.name = created.name.Replace("(Clone)", "");
        //created.GetComponent<SpriteRenderer>().color = Color.red;
        created.gameObject.SetActive(false);
        enemiesList.Add(created);
    }

    public GameObject GetEnemy(int id)
    {
        //if (enemiesList.Count == 0)
        Debug.Log(enemiesList.Contains(enemies[id]));
        if (enemiesList.Contains(enemies[id]))
        {
            CreateEnemies(id);
        }
        return QuantityEnemies(id);
    }

    private GameObject QuantityEnemies(int id)
    {

        enemyToActivate = enemiesList.Find(i => i.name == enemies[id].name);
        enemiesList.Remove(enemyToActivate);
        enemyToActivate.gameObject.SetActive(true);

        return enemyToActivate;
    }

    public void DisableEnemy(GameObject enemy)
    {
        enemy.gameObject.SetActive(false);
        enemiesList.Add(enemy);
    }
}