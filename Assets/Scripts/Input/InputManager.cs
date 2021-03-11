using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager :MonoBehaviour
{

    public BoardController boardController;
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Z)) { boardController.Rotate(1); }
            else if (Input.GetKeyDown(KeyCode.X)) { boardController.Rotate(-1); }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) { boardController.Move(-1); }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) { boardController.Move(1); }
        }
    }
}
