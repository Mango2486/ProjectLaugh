using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelClear : MonoBehaviour
{
    [SerializeField] private SceneIndex.sceneIndex nextSceneIndex;
    [SerializeField] private GameObject victory;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("过关时播放声音后等待时间")]
    [SerializeField] private float waitTime;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<PlayersManager>().GetStatus() == PlayersManager.PlayersStatus.Combined)
        {   
            victory.SetActive(true);
            spriteRenderer.color = new Color(0, 0, 0, 0);
            Destroy(other.GetComponentInParent<PlayersManager>().gameObject);
            NextLevel();
        }
    }

    private  void NextLevel()
    {
        StartCoroutine(nameof(LevelClearCorutine));
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator LevelClearCorutine()
    {
        //TODO：播放声音
        GameManager.instance.StopPlaySound();
        GameManager.instance.PlaySoundSFX(10);
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene((int)nextSceneIndex);
    }
}
