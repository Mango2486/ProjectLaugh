using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2AnimationController : MonoBehaviour
{
    [SerializeField] private Player2 player2;
    [SerializeField] private Player1 player1;
    [SerializeField] private PlayersManager playerManager;
    [SerializeField] private Rigidbody2D rigidbody;

    [SerializeField] private Animator animator;

    private const string IdleHead = "IdleHead";
    private const string CombHead = "CombHead";
    private const string DivideHead = "DivideHead";
    private const string HeadIdle = "HeadIdle";
    private const string HeadWalk = "HeadWalk";
    private const string HeadJump = "HeadJump";

    private Rigidbody2D legRb;

    private bool isPaused = false;

    private void Start() {

        player2 = GetComponent<Player2>();
        player1 = transform.parent.GetChild(1).GetComponent<Player1>();
        playerManager = GetComponentInParent<PlayersManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        legRb = player1.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        switch (playerManager.GetStatus()) {
            case PlayersManager.PlayersStatus.Combined:
                PlayInCombStatus();
                break;
            case PlayersManager.PlayersStatus.Waiting:
                PlayInCombStatus();
                break;
            case PlayersManager.PlayersStatus.Separate:
                PlayInSeparateStatus();
                break;
            case PlayersManager.PlayersStatus.Combinating:
                if (isPaused) {
                    ResumeAnimation();
                    GameManager.instance.PlaySoundSFX(8);
                }
                animator.Play(DivideHead);
                break;
        }
    }

    private void PlayInSeparateStatus() {
        switch(player2.GetPlayerStatus()) {
            case Player2.PlayerStatus.inGround:
                if (isPaused || animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f) {
                    ResumeAnimation();
                    break;
                }

                if (Math.Abs(rigidbody.velocity.x) > 0.1) {
                    animator.Play(HeadWalk);
                } else {
                    animator.Play(HeadIdle);
                }
                break;
            case Player2.PlayerStatus.flying:
                animator.Play(HeadJump);
                break;
        }
    }

    private void PlayInCombStatus() {
        if (legRb.velocity.x > 0.05) {
            animator.Play(CombHead);
        } else {
            animator.Play(IdleHead);
        }
    }

    // 当动画事件触发时调用，暂停动画
    public void PauseAnimation() {
        animator.speed = 0;
        isPaused = true;
    }

    // 恢复暂停的动画播放
    private void ResumeAnimation() {
        animator.speed = 1;
        isPaused = false;
    }

    public void FinishDivide() {
        playerManager.AnimCheck();
        playerManager.SetStatus(PlayersManager.PlayersStatus.Combined);
    }

    public void PlayCombSFX() {
        GameManager.instance.PlaySoundSFX(7);
    }
}
