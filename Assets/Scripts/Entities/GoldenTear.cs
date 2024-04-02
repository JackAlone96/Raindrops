using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenTear : Tear
{
    override public void Pop()
    {
        TearManager.Instance.GoldenTearPopped();
        base.Pop();
    }
}
