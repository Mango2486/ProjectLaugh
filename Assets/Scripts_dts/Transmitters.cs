using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Transmitters : MonoBehaviour
{
    [Header("发射口")]
    [SerializeField] private Transform muzzle;

    [Header("碟片")] 
    [SerializeField] private GameObject discs;
    
    [Header("发射间隔")]
    [SerializeField] private float generateDuration=1f;
    
    [SerializeField]private Queue<GameObject> discsQueue = new Queue<GameObject>();
    [SerializeField]private int size;
    
    private  float timer = 0f;
    private void Start()
    {
        InitializeDiscsPool();

    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > generateDuration)
        {
            if (PreparedObject().TryGetComponent<Discs>(out Discs preparedDiscs))
            {
                int face;
                if (muzzle.localPosition.x < 0f)
                {
                    face = -1;
                }
                else
                {
                    face = 1;
                }
                preparedDiscs.StartCoroutine(preparedDiscs.Move(face,muzzle.position));
            }
            timer = 0;
        }
    }



    private void InitializeDiscsPool()
    {
        for (int i = 0; i < size; i++)
        {
           discsQueue.Enqueue(Copy());
        }
        
    }

    private GameObject Copy()
    {
        var copy = GameObject.Instantiate(discs,Vector3.zero,Quaternion.identity,muzzle);
        copy.transform.localPosition = muzzle.localPosition;
        copy.SetActive(false);
        return copy;
    }
    
    private GameObject AvailableObject()
    {
        GameObject availableObject = null;
        if (discsQueue.Count > 0 && !discsQueue.Peek().activeSelf)
        {
            availableObject = discsQueue.Dequeue();
        }
        else
        {
            availableObject = Copy();
        }
        discsQueue.Enqueue(availableObject);
        return availableObject;
    }

    private GameObject PreparedObject()
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        return preparedObject;
    }



}
