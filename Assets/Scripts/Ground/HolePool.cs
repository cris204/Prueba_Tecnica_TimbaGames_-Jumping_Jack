using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolePool : MonoBehaviour {
    private static HolePool instance;

    public static HolePool Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private GameObject hole;

    [SerializeField]
    private int size;

    [SerializeField]
    private List<GameObject> holesList;

    private GameObject created;
    private GameObject holeToActivate;

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
        holesList = new List<GameObject>();
        for (int i = 0; i < size; i++)
        {
            CreateHoles();
        }

    }

    private void CreateHoles()
    {
        created = Instantiate(hole);
        created.gameObject.SetActive(false);
        holesList.Add(created);


    }

    public GameObject GetHoles()
    {
        if (holesList.Count == 0)
        {
            CreateHoles();
        }
        return QuantityHoles();
    }

    private GameObject QuantityHoles()
    {
        holeToActivate = holesList[holesList.Count - 1];
        holesList.RemoveAt(holesList.Count - 1);

        holeToActivate.gameObject.SetActive(true);

        return holeToActivate;
    }

    public void DisableHole(GameObject hole)
    {
        hole.gameObject.SetActive(false);
        holesList.Add(hole);
    }

}
