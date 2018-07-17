using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour {

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
    private List<GameObject> enemiesList;

    private GameObject created;
    private GameObject enemyToActivate;
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
        enemiesList = new List<GameObject>();
        for (int i = 0; i < size; i++)
        {
            CreateEnemies();
        }

    }

    private void CreateEnemies()
    {
        created = Instantiate(enemies[0]);
        created.gameObject.SetActive(false);
        enemiesList.Add(created);


    }

    public GameObject GetEnemy()
    {
        if (enemiesList.Count == 0)
        {
            CreateEnemies();
        }
        return QuantityEnemies();
    }

    private GameObject QuantityEnemies()
    {
        enemyToActivate = enemiesList[enemiesList.Count - 1];
        enemiesList.RemoveAt(enemiesList.Count - 1);
        enemyBehaviour = enemyToActivate.GetComponent<EnemyBehaviour>();

        enemyBehaviour.HorizontalSpeed = speedDirection;

        enemyToActivate.gameObject.SetActive(true);

        return enemyToActivate;
    }

    public void DisableEnemy(GameObject enemy)
    {
        enemy.gameObject.SetActive(false);
        enemiesList.Add(enemy);
    }
}
