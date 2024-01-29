using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Combination : MonoBehaviour
{
    [SerializeField] private Player1 player;
    [SerializeField] private PlayersManager playerManager;

    private void Start() {
        player = GetComponentInParent<Player1>();
        playerManager = player.GetComponentInParent<PlayersManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == "Player2") {
            playerManager.Combinatinig(transform.position);
            //Debug.Log("Comb");
        }
    }
}
