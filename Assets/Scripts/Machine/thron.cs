using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class thron : MonoBehaviour
{
    // Start is called before the first frame update
    public AnimationCurve breakCurve;
    public AnimationCurve aniCurve;
    float timer = 0.0f;
    BoxCollider2D col2d;
    SpriteRenderer render;
    public Sprite[] sprites = new Sprite[4];
    Vector3 startPos;
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        col2d = GetComponent<BoxCollider2D>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (breakCurve.Evaluate(timer) > 0.5f)
        {
            render.enabled = true;
            col2d.enabled = true;
        }
        else
        {
            render.enabled = false;
            col2d.enabled = false;
            ThornSound.isPlayd = false;
            render.sprite = sprites[Random.Range(0, 4)];
        }
        transform.position =startPos+ Vector3.up * aniCurve.Evaluate(timer);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayersManager>().SetStatus(PlayersManager.PlayersStatus.Die);
        }
    }
}
