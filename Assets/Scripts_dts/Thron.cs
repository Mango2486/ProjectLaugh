using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Thron : MonoBehaviour
{

    private Vector3 originalPosition;

    private float moveSpeed = 2f;

    private float timer = 0f;
    //TODO：人物碰上后发出声音并死亡
    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    private void Start()
    {
       //TODO:出声
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        Vector3 currentPosition = transform.localPosition;
        timer += Time.deltaTime;
        float newY = Mathf.Lerp(currentPosition.y, 0, moveSpeed * Time.deltaTime);
        transform.localPosition = new Vector3(currentPosition.x, newY, currentPosition.z);
    }
    
    private void OnDisable()
    {
        transform.localPosition = originalPosition;
    }
}
