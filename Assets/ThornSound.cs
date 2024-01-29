using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThornSound : MonoBehaviour
{
  [SerializeField] private SpriteRenderer spriteRenderer;

  public static bool isPlayd;
  private void OnEnable()
  {
    
  }

  private void Update()
  {
    if (spriteRenderer.enabled && !isPlayd)
    {
      int index = Random.Range(0, 3);
      GameManager.instance.PlaySoundSFX(index);
      isPlayd = true;
    }
  }
}
