using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    void Start()
    {
        var renderer = GetComponentInChildren<SpriteRenderer>();
        var r = Random.Range(0, sprites.Count);
        renderer.sprite = sprites[r];
    }
}