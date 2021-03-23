using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : SingletonBase<UIManager>
{
    private bool pause;
    [SerializeField]private GameObject Canvas;
    [SerializeField] private Button btn_Resume;
    [SerializeField] private EventSystem eventSystem;
    public bool Pause
    {
        get { return pause; }
        set
        {
            pause = value;
            if (pause)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }
    public void PauseGame()
    {
        eventSystem.SetSelectedGameObject(btn_Resume.gameObject);
        btn_Resume.OnSelect(null);
        Time.timeScale = 0;
        Canvas.SetActive(true);
    }
    public void ResumeGame()
    {
        pause = false;
        Time.timeScale = 1;
        Canvas.SetActive(false);
    }
    public void StartOverGame()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }

}
