using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }


    [Header("General")]
    [SerializeField]
    private int lifes;
    [SerializeField]
    private int score;
    [SerializeField]
    private int highScore;
    [SerializeField]
    private int level;
    [SerializeField]
    private Transform[] lanesPosition;
    private Vector2 posToSpawn;
    [SerializeField]
    private bool slowTime;
    [SerializeField]
    private bool finishLevel;
    [SerializeField]
    private GameObject player;

    [Header("BackGround")]
    [SerializeField]
    private SpriteRenderer backGround;
    [SerializeField]
    private Color32 bGColor;

    [Header("Spawn Holes")]
    private int floor;
    private GameObject holeToActive;
    private HoleBehaviour holeBehaviour;
    [SerializeField]
    private int holesUnits;
    private Vector2 posToSpawnHole;
    private int floorFirstHole;
    [SerializeField]
    private float initialSpeed;

    [Header("Spawn Enemies")]
    private GameObject enemyToActive;
    private EnemyBehaviour enemyBehaviour;
    private int enemiesUnits;
    [SerializeField]
    private List<int> enemiesUsed=new List<int>();
    private int randomEnemy;
    private int enemySelected;
    private int floorEnemy;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        Floor = Random.Range(0, lanesPosition.Length);
        FloorEnemy = Random.Range(0, lanesPosition.Length);

        // DontDestroyOnLoad(this.gameObject);
    }



    // Use this for initialization
    void Start () {
        SetRandom();
        StartNewLevel(Level);

    }

    public void StartNewLevel(int level)
    {
        holesUnits = 0;//organize
        GetStartHoles();
        GetStartHoles();
        if (level > 0)
        {
            GetNewEnemy();
        }
    }

    #region Enemies

    public void GetNewEnemy()//organize at random 
    {
        posToSpawn.x = Random.Range(-7.66f, 7.66f);
        FloorEnemy = Random.Range(0, lanesPosition.Length);
        posToSpawn.y = lanesPosition[FloorEnemy].transform.localPosition.y + 0.4f;
        enemyToActive = EnemyPool.Instance.GetEnemy(randomNum());
       // enemyToActive = EnemyPool.Instance.GetEnemy(5);
        enemyToActive.transform.localPosition = posToSpawn;
        enemyBehaviour = enemyToActive.GetComponent<EnemyBehaviour>();
        enemyBehaviour.Floor = FloorEnemy;
    }

    public void GetEnemy(int oldFloor,int id)
    {
        enemyToActive = EnemyPool.Instance.GetEnemy(id);
        enemyBehaviour = enemyToActive.GetComponent<EnemyBehaviour>();
        enemyBehaviour.Floor = oldFloor;
        enemyBehaviour.Floor++;
        if (enemyBehaviour.Floor >= lanesPosition.Length)
        {
            enemyBehaviour.Floor = 0;

        }
        posToSpawn.x = 7.353f;
        posToSpawn.y = lanesPosition[enemyBehaviour.Floor].transform.localPosition.y + 0.4f;
        enemyToActive.transform.localPosition = posToSpawn;

    }

    #endregion

    #region Holes

    public void GetStartHoles()
    {
        holeToActive = HolePool.Instance.GetHoles(initialSpeed, false);
        initialSpeed *= -1;
        posToSpawnHole.x = Random.Range(-7.66f, 7.66f);
        posToSpawnHole.y = lanesPosition[Floor].transform.localPosition.y;
        holeToActive.transform.localPosition = posToSpawnHole;
        holeBehaviour = holeToActive.GetComponent<HoleBehaviour>();
        holeBehaviour.Floor = Floor;
        holesUnits++;

    }

    public void GetNewHole()
    {
        if (holesUnits < 8)
        {

            Floor = Random.Range(0, lanesPosition.Length);
            holeToActive = HolePool.Instance.GetHoles(0, true);

            posToSpawnHole.x = Random.Range(-7.66f, 7.66f);
            posToSpawnHole.y = lanesPosition[Floor].transform.localPosition.y;
            holeToActive.transform.localPosition = posToSpawnHole;
            holeBehaviour = holeToActive.GetComponent<HoleBehaviour>();
            holeBehaviour.Floor = Floor;
            holesUnits++;
        }
    }

    public void GetHole(float speed, int oldFloor)
    {

        holeToActive = HolePool.Instance.GetHoles(speed, false);
        holeBehaviour = holeToActive.GetComponent<HoleBehaviour>();
        holeBehaviour.Floor = oldFloor;
        if (speed > 0)
        {
            holeBehaviour.Floor--;
            if (holeBehaviour.Floor < 0)
            {
                holeBehaviour.Floor = lanesPosition.Length - 1;

            }
            posToSpawnHole.x = -7.66f;
            posToSpawnHole.y = lanesPosition[holeBehaviour.Floor].transform.localPosition.y;
            holeBehaviour.HorizontalSpeed = speed;
            holeToActive.transform.localPosition = posToSpawnHole;
        }
        else
        {
            holeBehaviour.Floor++;
            if (holeBehaviour.Floor >= lanesPosition.Length)
            {
                holeBehaviour.Floor = 0;

            }
            posToSpawnHole.x = 7.66f;
            posToSpawnHole.y = lanesPosition[holeBehaviour.Floor].transform.localPosition.y;
            holeBehaviour.HorizontalSpeed = speed;
            holeToActive.transform.localPosition = posToSpawnHole;
        }

    }


    #endregion

    public void ChangeTimeScale(float timeSpeed)
    {
        Time.timeScale = timeSpeed;
    }

    public void ChangeBGColorNormalStun(Color32 color)
    {
        StartCoroutine(ChangeColorStuned(color, bGColor));
    }

    public void ChangeBGColorStunByEnemy(Color32 color)
    {
        StartCoroutine(ChangeColorStunedByEnemy(color, bGColor));
    }

    public void ScoreUpdate()
    {
        Score += (Level+1)*5;
        CanvasManager.Instance.AssignScore();
    }

    public void HighScoreUpdate(int HighScoreUp)
    {
        if (HighScore < HighScoreUp)
        {
            HighScore = HighScoreUp;

            CanvasManager.Instance.AssignHighScore();
        }
    }
   
    public void LessLifes()
    {
        Lifes--;
        CanvasManager.Instance.LessLifesImage();
        Debug.Log(Lifes);

        if (Lifes <= 0)
        {
            HighScoreUpdate(Score);
            CanvasManager.Instance.EndGame(Level);
            Score = 0;
            FinishLevel = true;
            // ReStart();

        }
    }

    public void LevelUp()
    {
        Level += 1;
        if(Level==6|| Level == 11|| Level == 16)
        {
            Lifes++;
            CanvasManager.Instance.ExtraLife();
        }
        FinishLevel = true;
        player.layer = 10;
        player.transform.localPosition = Vector2.up * -3.33f;
        CanvasManager.Instance.AssignTxtNextLevel(Level);
    }

    public void ReStart()
    {
        FinishLevel = false;
        Level = 0;
        player.layer = 10;
        player.transform.localPosition = Vector2.up * -3.33f;
        PlayerController.Instance.Stuned = false;
        PlayerController.Instance.RestartAnimations();
        PlayerController.Instance.StartGame=true;
        Lifes = 6;
        StartNewLevel(Level);
    }


    #region Random enemies
    public void SetRandom()
    {
        for (int i = 0; i < 10; i++)
        {
            enemiesUsed.Add(i);
        }
    }

    int randomNum()
    {

        if (enemiesUsed.Count == 0)
        {
            SetRandom();
        }
        randomEnemy = Random.Range(0, enemiesUsed.Count);
        enemySelected = enemiesUsed[randomEnemy];
        Debug.Log(enemiesUsed[randomEnemy]);
        enemiesUsed.RemoveAt(randomEnemy);

        return enemySelected;
    }

#endregion


    #region corroutine

    IEnumerator ChangeColorStuned(Color32 color,Color32 colorBG)
    {
        
        backGround.color = color;
        slowTime = true;
        yield return new WaitForSeconds(0.1f);
        backGround.color = colorBG;
        slowTime = false;
        yield return new WaitForSeconds(0.1f);
        backGround.color = color;
        slowTime = true;
        yield return new WaitForSeconds(0.1f);
        slowTime = false;
        backGround.color = colorBG;
    }

    IEnumerator ChangeColorStunedByEnemy(Color32 color, Color32 colorBG)
    {

        backGround.color = color;
        slowTime = true;
        yield return new WaitForSeconds(0.2f);
        backGround.color = colorBG;
        slowTime = false;

    }


    #endregion


    #region Gets and Sets
    public int Floor
    {
        get
        {
            return floor;
        }

        set
        {
            floor = value;
        }
    }

    public bool SlowTime
    {
        get
        {
            return slowTime;
        }

        set
        {
            slowTime = value;
        }
    }

    public int FloorEnemy
    {
        get
        {
            return floorEnemy;
        }

        set
        {
            floorEnemy = value;
        }
    }

    public int HighScore
    {
        get
        {
            return highScore;
        }

        set
        {
            highScore = value;
        }
    }

    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public bool FinishLevel
    {
        get
        {
            return finishLevel;
        }

        set
        {
            finishLevel = value;
        }
    }

    public int Lifes
    {
        get
        {
            return lifes;
        }

        set
        {
            lifes = value;
        }
    }

    #endregion
}
