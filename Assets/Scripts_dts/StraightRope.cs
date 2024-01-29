using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightRope : MonoBehaviour
{
    [Header("整体移动时间")] 
    [SerializeField]private  float moveDuration = 2f;
    
    [Header("点位设置")]
    [SerializeField]private Transform startPoint;
    [SerializeField]private Transform endPoint;
    
    private Vector3 startPosition;
    private Vector3 endPosition;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        ChangeLocalPositionToWorldPosition();
        DrawLine();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {  
        //检测是否为玩家，之后则是相关存储绳索的脚本
        if (other.TryGetComponent<RopeMove>(out RopeMove ropeMove) && other.GetComponentInParent<PlayersManager>().GetStatus() == PlayersManager.PlayersStatus.Combined )
        {
            //1.把这根绳子的起止点传过去，包括生成的贝塞尔曲线的中间点。
            ropeMove.SetPositions(startPosition,Vector3.zero, endPosition,Vector3.zero);
            //2.设置状态为挂在绳子上
            ropeMove.OnRope();
            //3.人物沿绳索轨迹移动
            ropeMove.StartCoroutine(ropeMove.StraightMove(moveDuration));
        }
      
    }
    
    
    private void ChangeLocalPositionToWorldPosition()
    {
        startPosition = startPoint.TransformPoint(Vector3.zero);
        endPosition = endPoint.TransformPoint(Vector3.zero);
    }

    private void DrawLine()
    {
        lineRenderer.SetPosition(0,startPosition);
        lineRenderer.SetPosition(1,endPosition);
    }
}
