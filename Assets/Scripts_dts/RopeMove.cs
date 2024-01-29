using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class RopeMove : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 controlPosition;
    private Vector3 endPosition;
    private Vector3 midPosition;

   

  
    //添加到整体的collider上
    private Transform playerTransfrom;
    public Rigidbody2D rigidBody2D1;
    public Rigidbody2D rigidBody2D2;

    private bool isOnRope;

    private void Start()
    {
        playerTransfrom = gameObject.transform;
        isOnRope = false;

    }
    
    //计算两点之间倾角需要旋转的角度
    private void CalculateAngleToRotate(Vector3 start, Vector3 end, Transform player)
    {
        float x = end.x - start.x;
        float y = end.y - start.y;
        float longSide = Mathf.Sqrt(x * x + y * y);
        float angle = y / longSide * Mathf.Rad2Deg;

        playerTransfrom.eulerAngles = new Vector3(0, 0, angle);
    }
    
    
    //曲线移动协程
    public IEnumerator CurveMove(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            Vector3 onGoingPoint=
                BezierCurve.Bezeir(startPosition , controlPosition, endPosition, timer/duration);
            timer += Time.deltaTime;
            rigidBody2D1.position = new Vector2(onGoingPoint.x, onGoingPoint.y);
            rigidBody2D2.position = new Vector2(onGoingPoint.x, onGoingPoint.y-1.4f);
            if (onGoingPoint.x < midPosition.x)
            {
                CalculateAngleToRotate(new Vector3(onGoingPoint.x,onGoingPoint.y,0),midPosition,playerTransfrom);
            }
            else
            {
                CalculateAngleToRotate(midPosition,new Vector3(onGoingPoint.x,onGoingPoint.y,0),playerTransfrom);
            }
            yield return null;
        }
        OffRope();
       //TODO：将人物旋转角度调整，并且重置刚体速度
    }
    
    //直线移动协程
    public IEnumerator StraightMove(float duration)
    {
        float timer = 0f;
        CalculateAngleToRotate(startPosition,endPosition,playerTransfrom);
        while (timer < duration)
        {
           Vector3 onGoingPoint = Vector3.Lerp(startPosition, endPosition, timer / duration);
            timer += Time.deltaTime;
            rigidBody2D1.position = new Vector2(onGoingPoint.x, onGoingPoint.y);
            rigidBody2D2.position = new Vector2(onGoingPoint.x, onGoingPoint.y-1.4f);
            yield return null;
        }
    }

    public void SetPositions(Vector3 startPosition, Vector3 controlPosition, Vector3 endPosition, Vector3 midPosition)
    {
        this.startPosition = startPosition;
        this.controlPosition = controlPosition;
        this.endPosition = endPosition;
        this.midPosition = midPosition;
    }

    public void OnRope()
    {
        isOnRope = true;
    }

    public void OffRope()
    {
        isOnRope = false;
        rigidBody2D1.velocity = Vector2.zero;
        rigidBody2D2.velocity = Vector2.zero;
        
    }

    public bool IsOnRope()
    {
        return isOnRope;
    }
}
