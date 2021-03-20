using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private float lastTime;
    private float lastTimeDownArrow;
    private float timeOffset;
    private float timeOffsetDownArrow=0.02f;
    private int firstInput;
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
                else if (Input.GetKey(KeyCode.DownArrow)&& InputIntervalCheckDownArrow())
                {
                    boardController.Move(0);
                }
            }
        }
        if (CheckKeyUp())
        {
            firstInput = 0;
            timeOffset = 0.06f;
        }
    }
    bool CheckKeyUp()
    {
        return Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow);
    }
    bool InputIntervalCheck()
    {
        bool ret;
        if (Time.time - lastTime > timeOffset)
        {
            lastTime = Time.time;
            ret = true;
        }
        else ret = false;
        if (firstInput<30)
        {
            timeOffset = 0.5f;
            firstInput++;
        }
        else timeOffset = 0.06f;
        return ret;
    }
    bool InputIntervalCheckDownArrow()
    {
        if (Time.time - lastTimeDownArrow > timeOffsetDownArrow)
        {
            lastTimeDownArrow = Time.time;
            return true;
        }
        return false;
       
    }
}
