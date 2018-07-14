using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }



    [SerializeField]
    private int lifes;
    [SerializeField]
    private int score;
    [SerializeField]
    private int holes;
    [SerializeField]
    private int enemys;
    [SerializeField]
    private Transform[] lanesPosition;

    [Header("Spawn Holes")]
    private Vector2 posToSpawnHole;
    private GameObject holeToActive;
    private HoleBehaviour holeBehaviour;
    private int holesUnits;
    private int floor;
    

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
        floor = Random.Range(0, lanesPosition.Length);
    }



    // Use this for initialization
    void Start () {
        holesUnits = 2;

    }

    public void GetNewHole()
    {
        if (holesUnits<8)
        {
            posToSpawnHole.x = Random.Range(-7.66f, 7.66f);
            floor = Random.Range(0, lanesPosition.Length);
            posToSpawnHole.y = lanesPosition[floor].transform.localPosition.y;

            //posToSpawnHole.y = lanesPosition[1].transform.localPosition.y - 0.14f;
            holeToActive = HolePool.Instance.GetHoles(0, true);
            holeToActive.transform.localPosition = posToSpawnHole;
            holeBehaviour = holeToActive.GetComponent<HoleBehaviour>();
            holeBehaviour.Floor = floor;
            holesUnits++;
        }
    }

    public void GetHole(float speed,int oldFloor)
    {

        holeToActive = HolePool.Instance.GetHoles(speed,false);
        holeBehaviour = holeToActive.GetComponent<HoleBehaviour>();
        holeBehaviour.Floor = oldFloor;
        if (speed > 0)
        {
            holeBehaviour.Floor--;
            if(holeBehaviour.Floor<0)
            {
                holeBehaviour.Floor = lanesPosition.Length-1;

            }
            posToSpawnHole.x = -7.66f;
            //posToSpawnHole.y = lanesPosition[holeBehaviour.Floor].transform.localPosition.y - 0.14f;
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
            //posToSpawnHole.y = lanesPosition[holeBehaviour.Floor].transform.localPosition.y - 0.14f;
            posToSpawnHole.y = lanesPosition[holeBehaviour.Floor].transform.localPosition.y;
            holeBehaviour.HorizontalSpeed = speed;
            holeToActive.transform.localPosition = posToSpawnHole;
        }

    }

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

    #endregion
}
