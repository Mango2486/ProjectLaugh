using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager instance { get; set; }

   [SerializeField] private AudioSource audioSource;

   [SerializeField] private AudioClip[] soundSFX;

   [SerializeField] private GameObject deathUI;

   [SerializeField] private GameObject playerPrefab;

   [SerializeField] private Transform spawnPoint;

   private Vector3 resetPosition;
   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
      }
   }

   private void Start()
   {
      resetPosition = spawnPoint.position;
   }

   public void PlaySoundSFX(int index)
   {
      audioSource.PlayOneShot(soundSFX[index]);
      
   }

   public void Loop()
   {
      audioSource.clip = soundSFX[soundSFX.Length-1];
      audioSource.loop = true;
   }

   public void ResetGame()
   {
      deathUI.SetActive(true);
      Instantiate(playerPrefab, resetPosition, quaternion.identity);
   }

   public void ChangeResetPosition(Vector3 position)
   {
      resetPosition = position;
   }

   public void StopPlaySound()
   {
      audioSource.Stop();
   }
}
