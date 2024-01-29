using UnityEngine;

public class Throns : MonoBehaviour
{
    [SerializeField] private GameObject[] thorns;

    [SerializeField] private float showDuration;
    [SerializeField] private float hideDuration;

    private float timer;
    private int activeCount;

    private void Start()
    {
        timer = 0f;
        activeCount = 0;
        Initialize();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        ControlThorns();
       
    }

    private void ControlThorns()
    {
        if (timer > hideDuration && activeCount < thorns.Length)
        {
            thorns[activeCount].SetActive(true);
            activeCount++;
            timer = 0f;
        }
        if (timer > showDuration && activeCount == thorns.Length)
        {
            Initialize();
            timer = 0f;
            activeCount = 0;
        }
    }
    private void Initialize()
    {
        for (int i = 0; i < thorns.Length; i++)
        {
            thorns[i].SetActive(false);
        }
    }
}
