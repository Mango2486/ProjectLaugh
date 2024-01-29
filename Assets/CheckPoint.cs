using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<PlayersManager>().GetStatus() == PlayersManager.PlayersStatus.Combined)
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 3.0f, transform.position.z);
            GameManager.instance.ChangeResetPosition(position);
        }
    }
}
