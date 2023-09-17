using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTestBox : HittableFromBelow
{
    protected override void Use(int playerNumber)
    {
        Debug.Log("AnimationTextBox.Use()");
    }
}
