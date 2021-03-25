using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SFXButton : Button
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (SoundManager.Instance != null)
        {
            onClick.AddListener(ClickSound);
        }
    }
    void ClickSound()
    {
        SoundManager.Instance.PlayUISFX(UISound.BtnClick);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        SoundManager.Instance.PlayUISFX(UISound.FocusMove);
    }

}
