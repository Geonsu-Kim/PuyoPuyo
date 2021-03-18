using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private float lastTime;
    private float timeOffset = 0.1f;
    private bool firstInput = true;
    private bool holding = false;
    public BoardController boardController;
    private void Update()
    {
        if (boardController.CanControl())
        {
            if (Input.GetKeyDown(KeyCode.Z)) { boardController.Rotate(1); }
            else if (Input.GetKeyDown(KeyCode.X)) { boardController.Rotate(-1); }
            else
            {

                if (Input.GetKey(KeyCode.LeftArrow)&& InputIntervalCheck())
                {

                    boardController.Move(-1);
                }
                else if (Input.GetKey(KeyCode.RightArrow)&& InputIntervalCheck())
                {
                    boardController.Move(1);
                }
                else if (Input.GetKey(KeyCode.DownArrow)&& InputIntervalCheck())
                {
                    boardController.Move(0);
                }
            }


        }
    }

    bool InputIntervalCheck()
    {

        if (Time.time - lastTime > timeOffset)
        {
            lastTime = Time.time;
            return true;
        }
        return false;
    }
}
