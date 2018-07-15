using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    private static CanvasManager instance; 
    public static CanvasManager Instance
    {
        get
        {
            return instance;
        }
    }

    [Header("Score")]
    [SerializeField]
    private Text highScoreTxt;
    [SerializeField]
    private Text scoreTxt;
    [SerializeField]
    private Text nextLevel;
    [SerializeField]
    private string highScoreString;
    [SerializeField]
    private string scoreString;
    [SerializeField]
    private GameObject nextLevelContainer;
    [SerializeField]
    private Image lifes;

    void Awake () {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
      //  DontDestroyOnLoad(this.gameObject);
    }



    // Update is called once per frame
    void Update () {
        highScoreTxt.text = string.Format("HI{0}", GameManager.Instance.HighScore);
        scoreTxt.text = string.Format("SC{0}", GameManager.Instance.Score);
    }

    public void AssignTxtNextLevel(int level)
    {
        nextLevelContainer.SetActive(true);
        if (level < 2)
        {
            nextLevel.text = string.Format("NEXT LEVEL -   {0}   HAZARD", level);
        }
        else
        {
            nextLevel.text = string.Format("NEXT LEVEL -   {0}   HAZARDS", level);
        }
        StartCoroutine(ContinueNextLevel(level));
    }
    public void LessLifesImage()
    {
        lifes.fillAmount -= 0.1667f;
    }


    #region Corroutine

    IEnumerator ContinueNextLevel(int level)
    {
        yield return new WaitForSeconds(5);
        GameManager.Instance.FinishLevel = false;
        GameManager.Instance.StartNewLevel(level);

        Debug.Log("start again");
        nextLevelContainer.SetActive(false);
    }

    #endregion

}
