using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{

    protected PuyoController controller;
    public Command(PuyoController _controller)
    {
        controller = _controller;
    }
    public abstract void Execute(int keyNum);

}

