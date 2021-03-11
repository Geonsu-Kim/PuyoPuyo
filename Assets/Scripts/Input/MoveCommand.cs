using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Command
{
    public MoveCommand(PuyoController _controller) : base(_controller) { }
    public override void Execute(int key)
    {
        controller.Move(key);
    }

}
