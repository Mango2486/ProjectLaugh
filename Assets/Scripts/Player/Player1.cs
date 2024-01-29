using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayersManager;

public class Player1 : MonoBehaviour
{
    public enum PlayerStatus {
        common = 0,
        flying = 1,
        inGround = 2,
    }

    [Header("Move")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float accelerateSpeed = 10f;
    [SerializeField] private LayerMask layerMask;

    [Header("Slop")]
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float slopCheckDistance;

    [Header("Rotate")]
    [SerializeField] private float relativeFoce = 3f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;

    [Header("Comb")]
    [SerializeField] private float combTime = 1f;

    [Header("Components")]
    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayersManager playersManager;
    [SerializeField] private Transform headPoint;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform controlPoint;
    [SerializeField] private float combCounter;
    [SerializeField] private PlayerStatus playerStatus;

    private CapsuleCollider2D cc;
    private Rigidbody2D rb;
    private RelativeJoint2D rj;
    private HingeJoint2D hj;

    private float angleRange = 1;
    private bool isCombinating = false;
    private Vector2 colliderSize;
    private Vector2 slopNormalPerp;


    private void Start()
    {
        gameInput.Player1_Interact += GameInput_OnInteractAction;
        playersManager.OnPlayersStatusChanged += OnPlayerStatusChanged;

        cc = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rj = GetComponent<RelativeJoint2D>();
        hj = GetComponent<HingeJoint2D>();

        headPoint.transform.position = rj.connectedBody.position;
        controlPoint.transform.position = transform.TransformPoint(hj.anchor);

        colliderSize = cc.size;

        rj.maxForce = relativeFoce;
    }

    private void OnPlayerStatusChanged(object sender, PlayersManager.OnPlayersStatusChangedEventArgs e) {
        if (e.status == PlayersManager.PlayersStatus.Separate) {
            SeparateInteraction();
            combCounter = combTime;
        }
        else if (e.status == PlayersManager.PlayersStatus.Die) {
            hj.enabled = false;
        }
        else if (e.status == PlayersManager.PlayersStatus.Combined) {
            headPoint.gameObject.SetActive(false);
            playerStatus = PlayerStatus.common;
            hj.enabled = true;
            rj.enabled = true;
            isCombinating = false;
        }
        else if (e.status == PlayersManager.PlayersStatus.Combinating) {
            isCombinating = true;
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        switch(playersManager.GetStatus()) {
            case PlayersManager.PlayersStatus.Combined:
                playersManager.SetStatus(PlayersManager.PlayersStatus.Waiting);
                playersManager.LegCheck();
                break;
            case PlayersManager.PlayersStatus.Waiting:
                playersManager.LegCheck();
                playersManager.SetStatus(PlayersManager.PlayersStatus.Separate);
                SeparateInteraction();
                break;
            case PlayersManager.PlayersStatus.Separate:
                Jump();
                break;
        }
    }

    private void Update()
    {
        if (isCombinating) {
            rb.velocity = Vector2.zero;
        } else {
            HandleMovement();
            SlopeCheck();
        }

        if (playersManager.GetStatus() == PlayersManager.PlayersStatus.Separate) {
            if (combCounter > 0) {
                combCounter -= Time.deltaTime;
            } else {
                headPoint.gameObject.SetActive(true);
            }

            if (Physics2D.Raycast(groundCheck.position, Vector2.down, Mathf.Epsilon, layerMask)) {
                playerStatus = PlayerStatus.inGround;
            } else {
                playerStatus = PlayerStatus.flying;
            }
        }
    }

    private void SeparateInteraction() {
        rj.enabled = false;
        hj.enabled = false;
    }

    private void HandleMovement() {
        var player1_InputVector = gameInput.Get_Player1_InputVetor();
        var player1_moveDir = new Vector2 (-player1_InputVector.x * slopNormalPerp.x, -player1_InputVector.x * slopNormalPerp.y);

        if (player1_InputVector.x != 0) {
            transform.localScale = new Vector3( player1_InputVector.x, transform.localScale.y, transform.localScale.z);
        }

        //rb.velocity = player1_moveDir * moveSpeed;
        if (playerStatus == PlayerStatus.flying) {
            player1_moveDir = new Vector2(player1_InputVector.x, player1_InputVector.y);
            rb.velocity = new Vector2(player1_moveDir.x * moveSpeed, rb.velocity.y);
        } else {
            var targetVelocity = player1_moveDir * moveSpeed;
            var curVelocity = rb.velocity;
            var acceleration = (targetVelocity - curVelocity).normalized * accelerateSpeed;
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
            }
            else{
                if (rb.rotation > -targetAngle + angleRange) {
                    rb.rotation -= rotateSpeed * Time.deltaTime;
                }
                else if (rb.rotation < -targetAngle - angleRange ) {
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
        }
    }

    public PlayerStatus GetPlayerStatus() {
        return playerStatus;
    }
}