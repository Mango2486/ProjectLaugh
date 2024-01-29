using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool active = false;
    public Pedal[] pedal_Arr;
    bool onAni = false;
    public float aniTime = 3.0f;
    public int dst = 2;
    Vector2 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        active = activeCheck();
        if (!onAni && active)
        {
            StartCoroutine(ani(1.0f));
        }
        if (!active) transform.position = Vector2.Lerp(transform.position, startPos, Time.deltaTime*10);
    }
    bool activeCheck()
    {
        bool value = true;
        for(int i = 0; i < pedal_Arr.Length; i++)
        {
            if (!pedal_Arr[i].onPedal)
            {
                value = false;
                break;
            }
        }
        return value;
    }
    IEnumerator ani(float time)
    {
        float t = 0.0f;
        Vector2 startPos=transform.position;
        Vector2 tarPos = startPos - (Vector2)transform.up * dst;
        while (t<time)
        {
            if (!active) break;
            t += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, tarPos, t / time);
            yield return null;
        }
        if(t>=time)Destroy(gameObject);
        yield break;
    }
}
