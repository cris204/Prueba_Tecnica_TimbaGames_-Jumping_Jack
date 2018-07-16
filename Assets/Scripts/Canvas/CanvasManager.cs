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

    [SerializeField]
    private Text highScoreTxt;
    [SerializeField]
    private Text scoreTxt;
    [SerializeField]
    private Text nextLevel;
    [SerializeField]
    public Text storyTxt;
    [SerializeField]
    private GameObject nextLevelContainer;
    [SerializeField]
    private Image lifes;
    [SerializeField]
    private float delay = 0.1f;
    private string currentText;
    [SerializeField]
    private bool continueWithGame;

    
    [SerializeField]
    private GameObject endGame;
    [SerializeField]
    private Text finalTxt;
    [SerializeField]
    private GameObject newHighScore;

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

    private void Start()
    {
        scoreTxt.text = string.Format("SC{0}", GameManager.Instance.Score);
        highScoreTxt.text = string.Format("HI{0}", GameManager.Instance.HighScore);
    }


    public void AssignScore()
    {

        scoreTxt.text = string.Format("SC{0}", GameManager.Instance.Score);
    }

    public void AssignHighScore()
    {

        highScoreTxt.text = string.Format("HI{0}", GameManager.Instance.HighScore);
        newHighScore.SetActive(true);
    }

    public void EndGame(int level)
    {
        finalTxt.text = string.Format("FINAL SCORE    {0}\nwith   {1} HAZARDS", GameManager.Instance.Score, level);
        endGame.SetActive(true);
    }

    public void AssignTxtNextLevel(int level)
    {
        continueWithGame = false;
        nextLevelContainer.SetActive(true);
        if (level < 2)
        {
            nextLevel.text = string.Format("NEXT LEVEL -   {0}   HAZARD", level);
        }
        else
        {
            nextLevel.text = string.Format("NEXT LEVEL -   {0}   HAZARDS", level);
        }
        
        storyTxt.text = Story.Instance.AssignStoryText(level-1);
        StartCoroutine(ContinueNextLevel(level));
    }

    public void LessLifesImage()
    {
        lifes.fillAmount -= 0.1667f;
    }


    #region Corroutine

    IEnumerator ContinueNextLevel(int level)
    {
        StartCoroutine(ShowText());
        while (!continueWithGame)
        {

            yield return null;

        }
        yield return new WaitForSeconds(2);
        GameManager.Instance.FinishLevel = false;
        GameManager.Instance.StartNewLevel(level);
        nextLevelContainer.SetActive(false);
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i < Story.Instance.StoryTxt[0].Length + 1; i++)
        {
            currentText = Story.Instance.StoryTxt[0].Substring(0, i);
            storyTxt.text = currentText;
            yield return new WaitForSeconds(delay);
        }
        continueWithGame = true;
    }


    #endregion

}
