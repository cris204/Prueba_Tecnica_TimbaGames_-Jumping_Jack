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
    private Transform lanesPosition;

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
    }



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            HolePool.Instance.GetHoles();
        }
	}

    public void GetNewHole()
    {
        HolePool.Instance.GetHoles();
    }
}
