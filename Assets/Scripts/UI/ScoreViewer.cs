using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreViewer : MonoBehaviour
{
    [SerializeField] private Image[] Score;
    [SerializeField] private UISet uiSet;
    public void SetScore(int score)
    {
        if(score>Util.maxScore)
        {
            for (int i = 0; i < Score.Length; i++)
            {
                Score[i].sprite = uiSet.Number[9];
            }
        }
        string numString = score.ToString("D8");
        Debug.Log(numString);
        for (int i = 0; i < Score.Length; i++)
        {
            int num=int.Parse(numString[i].ToString());
            Score[i].sprite = uiSet.Number[num];
        }
    }
}
