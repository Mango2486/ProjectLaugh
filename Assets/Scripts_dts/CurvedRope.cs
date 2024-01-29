using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedRope : MonoBehaviour
{
   [Header("整体移动时间")] 
   [SerializeField]private  float moveDuration = 2f;
   
   [Header("点位设置")]
   [SerializeField]private Transform startPoint;
   [SerializeField]private Transform controlPoint;
   [SerializeField]private Transform endPoint;

   private LineRenderer lineRenderer;
   

   private Vector3 startPosition;
   private Vector3 controlPosition;
   private Vector3 endPosition;
   
   private Vector3 midPosition;

   private void Awake()
   {
      lineRenderer = GetComponent<LineRenderer>();
   }

   private void Start()
   {
      ChangeLocalPositionToWorldPosition();
      StartCoroutine(GenerateCurve(moveDuration));
   }
   
   private void OnTriggerEnter2D(Collider2D other)
   {  
      //检测是否为玩家，之后则是相关存储绳索的脚本
      if (other.TryGetComponent<RopeMove>(out RopeMove ropeMove) && other.GetComponentInParent<PlayersManager>().GetStatus() == PlayersManager.PlayersStatus.Combined )
      {
         //1.把这根绳子的起止点传过去，包括生成的贝塞尔曲线的中间点。
         ropeMove.SetPositions(startPosition,controlPosition,endPosition,midPosition);
         //2.设置状态为挂在绳子上
         ropeMove.OnRope();
         //3.人物沿绳索轨迹移动
         ropeMove.StartCoroutine(ropeMove.CurveMove(moveDuration));
      }
      
   }

   
   //确定生成的贝塞尔曲线的中间位置的值，用于计算挂在绳子上的时候的任务旋转角度。
   private IEnumerator GenerateCurve(float duration)
   {
      float timer = 0f;
      while (timer < duration)
      {
         Vector3 position = BezierCurve.Bezeir(startPosition, controlPosition, endPosition, timer / duration);
         lineRenderer.positionCount++;
         lineRenderer.SetPosition(lineRenderer.positionCount-1,position);
         timer += Time.fixedDeltaTime;
         if (position.x < controlPosition.x)
         {
            midPosition = position;
         }
         yield return null;
      }
     
   }
   //本地转世界
   private void ChangeLocalPositionToWorldPosition()
   {
      startPosition = startPoint.TransformPoint(Vector3.zero);
      controlPosition = controlPoint.TransformPoint(Vector3.zero);
      endPosition = endPoint.TransformPoint(Vector3.zero);
   }
   

  
}
