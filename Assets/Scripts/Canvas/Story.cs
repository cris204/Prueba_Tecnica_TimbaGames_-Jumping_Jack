using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Story : MonoBehaviour {

    private static Story instance;
    public static Story Instance
    {
        get
        {
            return instance;
        }
    }



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

    [SerializeField]
    private string[] storyTxt = new string[21];

    // Use this for initialization
    void Start () {
        StoryTxt[0] = "Jumping Jack is quick and bold \nWith skill his story will unfold";
        StoryTxt[1] = "THE BALLAD OF JUMPING JACK \nA daring explorer named Jack...";
        StoryTxt[2] = "Once found a peculiar track...";
        StoryTxt[3] = "There were dangers galore...";
        StoryTxt[4] = "Even holes in the floor...";
        StoryTxt[5] = "So he kept falling flat on \nhis back...";
        StoryTxt[6] = "Quite soon he got used to \nthe place...";
        StoryTxt[7] = "He could jump to escape from \nthe chase...";
        StoryTxt[8] = "But without careful thought...";
        StoryTxt[9] = "His leaps came to nought...";
        StoryTxt[10] = "And he left with a much \nwider face...";
        StoryTxt[11] = "Things seemed just as bad as \ncould be...";
        StoryTxt[12] = "Hostile faces were all Jack \ncould see...";
        StoryTxt[13] = "He tried to stay calm...";
        StoryTxt[14] = "And to come to no harm...";
        StoryTxt[15] = "But more often got squashed \nlike a flea...";
        StoryTxt[16] = "By now Jack was in a \ngreat flap...";
        StoryTxt[17] = "He felt like a rat in a trap...";
        StoryTxt[18] = "If only he'd guessed...";
        StoryTxt[19] = "That soon he could rest...";
        StoryTxt[20] = "After jumping the very \nlast gap.      - WELL DONE";


    }
    public string AssignStoryText(int level)
    {
        return StoryTxt[level];
    }



    #region Get and Set
    public string[] StoryTxt
    {
        get
        {
            return storyTxt;
        }

        set
        {
            storyTxt = value;
        }
    }
    #endregion

}
