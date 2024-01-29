using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Discs : MonoBehaviour
{
   [Header("生命周期")] [SerializeField] private float duration = 1f;
   [Header("移动速度")] [SerializeField] private float moveSpeed = 5f;

   private Rigidbody2D rigidbody2D;
   private Vector3 originalPosition;

   private void OnEnable()
   {
      rigidbody2D = GetComponent<Rigidbody2D>();
   }

   //碟片移动
   public IEnumerator Move(int face, Vector3 muzzlePosition)
   {

      //TODO:添加碟片发射音乐

      // float timer = 0;
      // while (timer < duration)
      // {
      //    var position = transform.position;
      //    position = new Vector3(position.x + face * moveSpeed * Time.deltaTime,position.y,position.z);
      //    transform.position = position;
      //    timer += Time.deltaTime;
      //
      //    yield return null;
      // }
      rigidbody2D.velocity = new Vector2(face * moveSpeed, 0);
      originalPosition = muzzlePosition;
      transform.position = muzzlePosition;
      yield return new WaitForSeconds(duration);
      gameObject.SetActive(false);
      
   }


//与人物相撞
   private void OnTriggerEnter2D(Collider2D other)
   {
      //TODO:触发人物死亡
      if (other.CompareTag("Player"))
      {
         other.GetComponentInParent<PlayersManager>().SetStatus(PlayersManager.PlayersStatus.Die);
         //TODO:禁用飞碟，重置位置
         transform.position = originalPosition;
         gameObject.SetActive(false);
      }
      
   }
   
}
