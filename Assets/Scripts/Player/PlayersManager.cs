using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public enum PlayersStatus {
        Combined = 0,
        Waiting  = 1,
        Separate = 2,
        Combinating = 3,
        Die = 4
    }

    [Header("玩家")]
    [SerializeField] private Transform Player1;
    [SerializeField] private Transform Player2;

    [Header("分离等待时间")]
    [SerializeField] private float waitingTime = 1f;

    [Header("分离时间")]
    [SerializeField] private float separatTime = 10f;

    [Header("死亡角度")]
    [SerializeField] private float dieAngle = 120f;

    [Header("监视器")]
    [SerializeField] private Vector3 vectorHead;
    [SerializeField] private Vector3 vectorFoot;
    [SerializeField] private SpriteRenderer headSprite;
    [SerializeField] private SpriteRenderer legSprite;
    [SerializeField] private float joinAngle;
    [SerializeField] private float interactionCounter;
    [SerializeField] private float separateCounter;
    [SerializeField] private PlayersStatus status;

    [Header("画线")] 
    [SerializeField] private Transform controlPoint;
    [SerializeField]private int numberOfPoints = 50;
    private LineRenderer lineRenderer;
    private Vector3 startPosition;
    private Vector3 controlPosition;
    private Vector3 endPosition;

    private bool headCheck = false;
    private bool legCheck = false;

    private bool posCheck = false;
    private bool animCheck = false;

    public event EventHandler<OnCombinatingEventArgs> OnCombinating;
    public class OnCombinatingEventArgs : EventArgs {
        public Vector2 targetPos;
    }

    public event EventHandler<OnPlayersStatusChangedEventArgs> OnPlayersStatusChanged;
    public class OnPlayersStatusChangedEventArgs : EventArgs {
        public PlayersStatus status;
    }

    private void Start()
    {   
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numberOfPoints;

        headSprite = Player2.GetComponent<SpriteRenderer>();
        legSprite = Player1.GetComponent<SpriteRenderer>();
    }

    private void Update() {
        switch (status) {
            case PlayersStatus.Combined:
                lineRenderer.enabled = true;
                ChangeLocalPositionToWorldPosition();
                joinAngle = Player1.GetComponent<HingeJoint2D>().jointAngle;
                DrawCurve();
                if (joinAngle > dieAngle || joinAngle < - dieAngle) {
                    SetStatus(PlayersStatus.Die);
                }
                headSprite.color = Color.white;
                legSprite.color = Color.white;
                break;
            case PlayersStatus.Waiting:
                lineRenderer.enabled = true;
                ChangeLocalPositionToWorldPosition();
                joinAngle = Player1.GetComponent<HingeJoint2D>().jointAngle;
                DrawCurve();
                if (joinAngle > dieAngle || joinAngle < -dieAngle) {
                    SetStatus(PlayersStatus.Die);
                }
                if (interactionCounter > 0) {
                    interactionCounter -= Time.deltaTime;
                }
                else {
                    PosCheck();
                    AnimCheck();
                    SetStatus(PlayersStatus.Combined);
                }
                break;
            case PlayersStatus.Separate:
                if (separateCounter > 0) {
                    var curHeadSprite = headSprite;
                    var curLegSprite = legSprite;
                    separateCounter -= Time.deltaTime;
                    headSprite.color = new Color(curHeadSprite.color.r, curHeadSprite.color.g, curHeadSprite.color.b, separateCounter / separatTime);
                    legSprite.color = new Color(curLegSprite.color.r, curLegSprite.color.g, curLegSprite.color.b, separateCounter / separatTime);
                } else {
                    SetStatus(PlayersStatus.Die);
                }
                lineRenderer.enabled = false;
                break;
            case PlayersStatus.Die:
                lineRenderer.enabled = false;
                GameManager.instance.PlaySoundSFX(6);
                Debug.Log("GameOver");
                GameManager.instance.ResetGame();
                Destroy(gameObject);
                break;
        }
    }

    public void SetStatus(PlayersStatus status) {
        
        switch (status) {
            case PlayersStatus.Waiting:
                StartInteract();
                break;
            case PlayersStatus.Separate:
                if (!headCheck) return;
                if (!legCheck) return;
                separateCounter = separatTime;
                ResetChecks();
                break;
            case PlayersStatus.Combined:
                if (!posCheck) return;
                if (!animCheck) return; 
                ResetChecks();
                ResetCombCheck();
                Debug.Log("Combined");
                break;
        }

        this.status = status;

        OnPlayersStatusChanged?.Invoke(this, new OnPlayersStatusChangedEventArgs {
            status = status
        });
    }

    public PlayersStatus GetStatus() {
        return status;
    }

    public void StartInteract() {
        interactionCounter = waitingTime;
    }

    public float GetSeparateCounter() {
        return separateCounter;
    }

    public float GetSeparateTime() {
        return separatTime;
    }

    public void HeadCheck() {
        headCheck = true;
        GameManager.instance.PlaySoundSFX(4);
    }

    public void LegCheck() {
        legCheck = true;
        GameManager.instance.PlaySoundSFX(4);
    }

    private void ResetChecks() {
        headCheck = false;
        legCheck = false;
    }

    public void PosCheck() {
        posCheck = true;
    }

    public void AnimCheck() {
        animCheck = true;
    }

    private void ResetCombCheck() {
        posCheck = false;
        animCheck = false;
    }

    public Vector3 GetLegScale() {
        return Player1.transform.localScale;
    }
    
    private void DrawCurve()
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i / (float)(numberOfPoints - 1);
            Vector3 position = BezierCurve.CalculateBezierPoint(t, Player1.position, controlPoint.position, Player2.position);
            lineRenderer.SetPosition(i, position);
        }
    }
    
    private void ChangeLocalPositionToWorldPosition()
    {
        startPosition = Player1.TransformPoint(Vector3.zero);
        controlPosition = controlPoint.TransformPoint(Vector3.zero);
        endPosition = Player2.TransformPoint(Vector3.zero);
    }

    public void Combinatinig(Vector2 targetPosition) {
        SetStatus(PlayersStatus.Combinating);
        OnCombinating?.Invoke(this, new OnCombinatingEventArgs {
            targetPos = targetPosition
        });
    }

}
