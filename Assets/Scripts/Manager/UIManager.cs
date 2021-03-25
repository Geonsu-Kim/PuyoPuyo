using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : SingletonBase<UIManager>
{
    private bool pause;
    [SerializeField]private GameObject Panel_Resume;
    [SerializeField] private ScoreViewer scoreViewer;
    [SerializeField] private SFXButton btn_Resume;
    [SerializeField] private EventSystem eventSystem;
    public bool Pause
    {
        get { return pause; }
        set
        {
            pause = value;
            if (pause)
            {
                SoundManager.Instance.PlayUISFX(UISound.Pause);
                PauseGame();
            }
            else
            {
                SoundManager.Instance.PlayUISFX(UISound.Resume);
                ResumeGame();
            }
        }
    }
    public void PauseGame()
    {
        eventSystem.SetSelectedGameObject(btn_Resume.gameObject);
        btn_Resume.OnSelect(null);
        Time.timeScale = 0;
        Panel_Resume.SetActive(true);
    }
    public void ResumeGame()
    {
        pause = false;
        Time.timeScale = 1;
        Panel_Resume.SetActive(false);
    }
    public void StartOverGame()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }
    public void SetScore(int score,bool softDrop)
    {
        scoreViewer.SetScore(score, softDrop);
    }
    public void SetScoreCalcuration(int popCount,int bonus)
    {
        scoreViewer.SetScoreCalcuration(popCount, bonus);
    }
}
