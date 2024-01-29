using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float t = 1.0f;
    public float dst = 2.0f;
    Vector3 localPosition;
    public bool onAni = false;
    void Start()
    {
        localPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!onAni)
        {
            onAni = true;
            StartCoroutine(ani(t, dst));
        }
    }
    IEnumerator ani(float t,float d)
    {
        float timr = 0.0f;
        Vector3 local =localPosition+Vector3.right* Random.Range(-d, d)+Vector3.up* Random.Range(-d, d);
        while (timr < t)
        {
            timr += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, local, timr/t);
            yield return null;
        }
        onAni = false;
        yield break;
    }
}
