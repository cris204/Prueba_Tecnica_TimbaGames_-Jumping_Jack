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
    private GameObject canvasNextLevel;
    [SerializeField]
    private GameObject nextLevelContainer;
    [SerializeField]
    private Image lifes;
    [SerializeField]
    private float delay = 0.1f;
    private string currentText;
    [SerializeField]
    private GameObject extraLife;
    [SerializeField]
    private bool continueWithGame;
    [SerializeField]
    private GameObject canvasEndGame;
    [SerializeField]
    private Text finalTxt;
    [SerializeField]
    private GameObject newHighScore;

    [SerializeField]
    private GameObject canvasIntro;

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
        GameManager.Instance.FinishLevel = true;
        AssignScore();
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
        GameManager.Instance.HighScoreUpdate(GameManager.Instance.Score);
        finalTxt.text = string.Format("FINAL SCORE    {0}\nwith   {1} HAZARDS", GameManager.Instance.Score, level);
        canvasEndGame.SetActive(true);
        StartCoroutine(PressEnterToContinue());
    }

    public void AssignTxtNextLevel(int level)
    {
        continueWithGame = false;
        canvasNextLevel.SetActive(true);
        if (level <= 20)
        {
            if (level < 2)
            {
                nextLevel.text = string.Format("NEXT LEVEL -   {0}   HAZARD", level);
            }
            else
            {
                nextLevel.text = string.Format("NEXT LEVEL -   {0}   HAZARDS", level);
            }
        }
        else
        {
            nextLevelContainer.SetActive(false);
        }

        storyTxt.text = Story.Instance.AssignStoryText(level - 1);

        StartCoroutine(ContinueNextLevel(level));
    }

    public void LessLifesImage()
    {
        lifes.fillAmount -= 0.111f;
    }

    public void ExtraLife()
    {
        extraLife.SetActive(true);
        lifes.fillAmount += 0.111f;
    }

    public void StartWithGame()
    {
        GameManager.Instance.FinishLevel = false;
        canvasIntro.SetActive(false);
        GameManager.Instance.StartNewLevel(0);
    }

    #region Corroutine

    IEnumerator ContinueNextLevel(int level)
    {
        StartCoroutine(ShowText(level));
        while (!continueWithGame)
        {

            yield return null;

        }
        yield return new WaitForSeconds(2);

        if (level <= 20)
        {
            GameManager.Instance.FinishLevel = false;
            GameManager.Instance.StartNewLevel(level);

            extraLife.SetActive(false);
        }
        else
        {

            EndGame(level);
        }
        canvasNextLevel.SetActive(false);
    }

    IEnumerator ShowText(int level)
    {
        for (int i = 0; i < Story.Instance.StoryTxt[level-1].Length + 1; i++)
        {
            currentText = Story.Instance.StoryTxt[level-1].Substring(0, i);
            storyTxt.text = currentText;
            yield return new WaitForSeconds(delay);
        }
        continueWithGame = true;
    }

    IEnumerator PressEnterToContinue()
    {
        while (!Input.GetButtonDown("Enter"))
        {
            yield return null;
        }
        nextLevelContainer.SetActive(true);
        StopAllCoroutines();
        canvasEndGame.SetActive(false);
        lifes.fillAmount = 0.666f;
        newHighScore.SetActive(false);
        GameManager.Instance.ReStart();


    }

    #endregion

}
