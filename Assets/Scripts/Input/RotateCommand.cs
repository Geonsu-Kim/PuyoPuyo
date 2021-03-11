using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCommand : Command
{

    public RotateCommand(PuyoController _controller) : base(_controller) { }
    public override void Execute(int key)
    {
        controller.Rotate(key);
    }

}
