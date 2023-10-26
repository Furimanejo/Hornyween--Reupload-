using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpriteAnimation
{
    public List<Sprite> WalkAnimation();
    public bool IsWalking();
    public bool Flip();
}
