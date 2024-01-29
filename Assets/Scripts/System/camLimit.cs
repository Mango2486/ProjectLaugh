using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camLimit : MonoBehaviour
{
    public float camSize = 5.0f;
    CamCtrl camCtrl;
    [System.Serializable]
    public class block
    {
        public bool top = false;
        public bool down = false;
        public bool left = false;
        public bool right = false;
    }
    public block bar;
    void Start()
    {
        camCtrl = FindObjectOfType<CamCtrl>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            camCtrl.curCamLimt = this;
            camCtrl.camSize= camSize;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            camCtrl.curCamLimt = null;
            camCtrl.camSize = 5;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.815f, 1, 0.615f, 0.5f);
        Gizmos.DrawCube(transform.position, transform.lossyScale);
        Gizmos.color = new Color(0.898f, 0.686f,0.886f, 1.0f);
        if (bar.left) Gizmos.DrawLine(transform.position - transform.lossyScale.x / 2.0f * Vector3.right + transform.lossyScale.y / 2.0f * Vector3.up, transform.position - transform.lossyScale.x / 2.0f * Vector3.right - transform.lossyScale.y / 2.0f * Vector3.up);
        if (bar.right) Gizmos.DrawLine(transform.position + transform.lossyScale.x / 2.0f * Vector3.right + transform.lossyScale.y / 2.0f * Vector3.up, transform.position + transform.lossyScale.x / 2.0f * Vector3.right - transform.lossyScale.y / 2.0f * Vector3.up);
        if (bar.top) Gizmos.DrawLine(transform.position - transform.lossyScale.x / 2.0f * Vector3.right + transform.lossyScale.y / 2.0f * Vector3.up, transform.position + transform.lossyScale.x / 2.0f * Vector3.right + transform.lossyScale.y / 2.0f * Vector3.up);
        if (bar.down) Gizmos.DrawLine(transform.position - transform.lossyScale.x / 2.0f * Vector3.right - transform.lossyScale.y / 2.0f * Vector3.up, transform.position + transform.lossyScale.x / 2.0f * Vector3.right - transform.lossyScale.y / 2.0f * Vector3.up);
    }
}
