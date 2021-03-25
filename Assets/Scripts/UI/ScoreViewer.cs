using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreViewer : MonoBehaviour
{
    private const string format = "D8";
    [SerializeField] private Image[] ScoreRet;
    [SerializeField] private Image[] ScorePopCount;
    [SerializeField] private Image[] ScoreBonus;
    [SerializeField] private UISet uiSet;
    [SerializeField] private GameObject ScoreRetPanel;
    [SerializeField] private GameObject ScoreCalcPanel;
    public void SetScore(int score,bool softDrop)
    {
        if (!softDrop)
        {

            ScoreCalcPanel.SetActive(false);
            ScoreRetPanel.SetActive(true);
        }
        if (score>Util.maxScore)
        {
            for (int i = 0; i < ScoreRet.Length; i++)
            {
                ScoreRet[i].sprite = uiSet.Number[9];
            }
        }
        string numString = score.ToString(format);
        for (int i = 0; i < ScoreRet.Length; i++)
        {
            int num=int.Parse(numString[i].ToString());
            ScoreRet[i].sprite = uiSet.Number[num];
        }
    }
    public void SetScoreCalcuration(int popCount,int bonus)
    {
        ScoreRetPanel.SetActive(false);
        string numPopCount = popCount.ToString();
        string numBonus = bonus.ToString();
        for (int i = ScorePopCount.Length-1,k= ScorePopCount.Length-numPopCount.Length; i >= 0; i--)
        {
            if (i - k < 0)
            {
                ScorePopCount[i].sprite = null;
                ScorePopCount[i].color = Color.clear;
                continue;
            }
            int num= int.Parse(numPopCount[i-k].ToString());
            ScorePopCount[i].sprite= uiSet.Number[num];
            ScorePopCount[i].color = Color.white;
        }
        for (int i = ScoreBonus.Length - 1, k = ScoreBonus.Length - numBonus.Length; i >= 0; i--)
        {
            if (i - k < 0)
            {
                ScoreBonus[i].sprite = null;
                ScoreBonus[i].color = Color.clear;
                continue;
            }
            int num = int.Parse(numBonus[i - k].ToString());
            ScoreBonus[i].sprite = uiSet.Number[num];
            ScoreBonus[i].color = Color.white;
        }
        ScoreCalcPanel.SetActive(true);
    }
}
