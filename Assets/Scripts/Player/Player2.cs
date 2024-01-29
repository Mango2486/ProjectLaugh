using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Player2 : MonoBehaviour
{
    public enum PlayerStatus {
        common = 0,
        flying = 1,
        inGround = 2,
    }

    [Header("Move")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private LayerMask layerMask;

    [Header("Rotate")]
    [SerializeField] private float balaneForce = 70f;
    [SerializeField] private float rotateSpeed = 200f;

    [Header("Separate")]
    [SerializeField] private float separateForce = 10f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;

    [Header("Combinate")]
    [SerializeField] private float combSpeed = 5f;

    [Header("Components")]
    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayersManager playersManager;
    [SerializeField] private Player1 player1;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private bool isCombing = false;
    [SerializeField] private bool isDividing = false;

    private CapsuleCollider2D cc;
    private Rigidbody2D rb;

    private float angleRange = 1;
    private Vector2 colliderSize;
    private Vector2 slopNormalPerp;
    private Vector2 headPoint;

    private void Start() {
        gameInput.Player2_Interact += GameInput_OnInteractAction;
        playersManager.OnPlayersStatusChanged += OnPlayerStatusChanged;
        playersManager.OnCombinating += OnCombinating;

        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        colliderSize = cc.size;
    }

    private void OnCombinating(object sender, PlayersManager.OnCombinatingEventArgs e) {
        isCombing = true;
        headPoint = e.targetPos;
    }

    private void OnPlayerStatusChanged(object sender, PlayersManager.OnPlayersStatusChangedEventArgs e) {
        if (e.status == PlayersManager.PlayersStatus.Separate) { 
            SeparateAction();
        }
        else if (e.status == PlayersManager.PlayersStatus.Combined) {
            playerStatus = PlayerStatus.common;
            rb.freezeRotation = false;
            isCombing = false;
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        switch (playersManager.GetStatus()) {
            case PlayersManager.PlayersStatus.Combined:
                playersManager.SetStatus(PlayersManager.PlayersStatus.Waiting);
                playersManager.HeadCheck();
                break;
            case PlayersManager.PlayersStatus.Waiting:
                playersManager.HeadCheck();
                playersManager.SetStatus(PlayersManager.PlayersStatus.Separate);
                break;
            case PlayersManager.PlayersStatus.Separate:
                Jump();
                break;
        }
    }

    private void Update() 
    {
        if (isCombing) {
            var curPos = rb.position;
            rb.position = Vector2.MoveTowards(curPos, headPoint, combSpeed * Time.deltaTime);
            if (Math.Abs(rb.position.x - headPoint.x) < 0.1 && Math.Abs(rb.position.y - headPoint.y) < 0.1) {
                //rb.inertia = 0;
                rb.velocity = Vector2.zero;
                playersManager.PosCheck();
                playersManager.SetStatus(PlayersManager.PlayersStatus.Combined);
            }
        } else {
            HandleInputVector();
            SlopeCheck();
        }

        if (playersManager.GetStatus() == PlayersManager.PlayersStatus.Separate) {
            if (Physics2D.Raycast(groundCheck.position, Vector2.down, Mathf.Epsilon, layerMask)) {
                playerStatus = PlayerStatus.inGround;
            } else {
                playerStatus = PlayerStatus.flying;
            }
        }

        if (playerStatus == PlayerStatus.common) {
            transform.localScale = playersManager.GetLegScale();
        }
    }

    private void SeparateAction() {
        rb.AddForce(Vector2.up * separateForce, ForceMode2D.Impulse);
        GameManager.instance.PlaySoundSFX(3);
        rb.rotation = 0;
        isDividing = true;
        rb.freezeRotation = true;
    }

    private void HandleInputVector() {
        var player2_InputVector = gameInput.Get_Player2_InputVector();
        var player2_moveDir = new Vector2(-player2_InputVector.x * slopNormalPerp.x, -player2_InputVector.x * slopNormalPerp.y);

        if (playerStatus != PlayerStatus.common && player2_InputVector.x != 0) {
            transform.localScale = new Vector3(player2_InputVector.x, transform.localScale.y, transform.localScale.z);
        }

        if (playerStatus == PlayerStatus.common) {
            player2_moveDir = new Vector2(player2_InputVector.x, player2_InputVector.y);
            rb.AddRelativeForce(player2_moveDir * balaneForce, ForceMode2D.Force);
        }
        else if (playerStatus == PlayerStatus.flying) {
            player2_moveDir = new Vector2(player2_InputVector.x, player2_InputVector.y);
            rb.velocity = new Vector2(player2_moveDir.x * moveSpeed, rb.velocity.y);
        } 
        else {
            var targetVelocity = player2_moveDir * moveSpeed;
            var curVelocity = rb.velocity;
            var acceleration = (targetVelocity - curVelocity).normalized * 15f;
            rb.velocity = Vector2.MoveTowards(curVelocity, targetVelocity, acceleration.magnitude * Time.deltaTime);
        }
    }

    private void SlopeCheck() {
        var checkPos = transform.position - new Vector3(0f, colliderSize.y / 2);

        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckVertical(Vector2 checkPos) {
        var raycastHit = Physics2D.Raycast(checkPos, new Vector2(1, -1), colliderSize.y / 2 * (float)Math.Sqrt(2), layerMask);
        if (raycastHit) {
            //平滑旋转至rb垂直斜面
            var targetAngle = Vector2.Angle(Vector2.up, raycastHit.normal);
            if (raycastHit.normal.x <= 0) {
                if (rb.rotation < targetAngle - angleRange) {
                    rb.rotation += rotateSpeed * Time.deltaTime;
                } else if (rb.rotation > targetAngle + angleRange) {
                    rb.rotation -= rotateSpeed * Time.deltaTime;
                }
            } else {
                if (rb.rotation > -targetAngle + angleRange) {
                    rb.rotation -= rotateSpeed * Time.deltaTime;
                } else if (rb.rotation < -targetAngle - angleRange) {
                    rb.rotation += rotateSpeed * Time.deltaTime;
                }
            }

            //取斜面平行线
            slopNormalPerp = Vector2.Perpendicular(raycastHit.normal).normalized;

            Debug.DrawRay(raycastHit.point, slopNormalPerp, Color.blue);
            Debug.DrawRay(raycastHit.point, raycastHit.normal, Color.red);
        }
    }

    private void Jump() {
        if (playerStatus == PlayerStatus.inGround) {
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            //rb.position = Vector2.zero;
        }
    }

    public PlayerStatus GetPlayerStatus() {
        return playerStatus;
    }
}