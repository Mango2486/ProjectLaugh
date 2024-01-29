using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Player1AnimationController : MonoBehaviour
{
    [SerializeField] private Player1 player1;
    [SerializeField] private PlayersManager playerManager;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;

    private const string legIdle = "IdleLeg";
    private const string legWalk = "LegWalk";
    private const string legJump = "LegJump";

    private void Start() {
        player1 = GetComponent<Player1>();
        playerManager = GetComponentInParent<PlayersManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        switch (player1.GetPlayerStatus()) {
            case Player1.PlayerStatus.flying:
                animator.Play(legJump);
                break;
            case Player1.PlayerStatus.inGround:
                if (Math.Abs(rigidbody.velocity.x) > 0.1) {
                    animator.Play(legWalk);
                } else {
                    animator.Play(legIdle);
                }
                break;
            default:
                if (Math.Abs(rigidbody.velocity.x) > 0.1) {
                    animator.Play(legWalk);
                } else {
                    animator.Play(legIdle);
                }
                break;
        }
    }

    public void PauseAnimation() {
        animator.speed = 0;
    }

    public void PlayAnimation() {
        animator.speed = 1;
    }

    public void PlayLandingSFX() {
        GameManager.instance.PlaySoundSFX(8);
    }
}
