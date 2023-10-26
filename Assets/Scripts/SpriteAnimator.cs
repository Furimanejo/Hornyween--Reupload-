using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float frameRate = 1;
    ISpriteAnimation animationSource;

    private void Awake()
    {
        animationSource = gameObject.GetComponentInParent<ISpriteAnimation>();
    }
    private void Update()
    {
        spriteRenderer.flipX = animationSource.Flip();

        if(animationSource.IsWalking())
            UpdateWalkAnimationFrame();
    }
    void UpdateWalkAnimationFrame()
    {
        var sprites = animationSource.WalkAnimation();
        var index = (int)(Time.time * frameRate) % sprites.Count;
        spriteRenderer.sprite = sprites[index];
    }
}