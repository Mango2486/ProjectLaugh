using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCtrl : MonoBehaviour
{
    public Vector2 Offset;
    public float maxDst = 1.0f;
    public float camSize=5.0f;
    Vector2 followPoint;
    Camera cam;
    public camLimit curCamLimt;
    float rat = 0;
    Vector3 curTar, wanderTar;
    Player1 p1=null;
    Player2 p2=null;
    class limit
    {
        public Vector3 center;
        public Vector2 size;
        public limit(Vector3 c, Vector2 s)
        {
            center = c;
            size = s;
        }
    }
    limit camLimitRect;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {   
        p1 = FindObjectOfType<Player1>();
        p2 = FindObjectOfType<Player2>();
        if (p1 && p2)
        {
            followPoint = new Vector2((p1.transform.position.x + p2.transform.position.x) / 2.0f, (p1.transform.position.y + p2.transform.position.y) / 2.0f);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, camSize, Time.deltaTime);
            rat = (float)Screen.width / (float)Screen.height;
            if (curCamLimt != null) camLimitRect = new limit(curCamLimt.transform.position, curCamLimt.transform.lossyScale);
            Vector3 vet3 = new Vector3(followPoint.x + Offset.x, followPoint.y + Offset.y, cam.transform.position.z);
            float dst = Vector2.Distance(followPoint, cam.transform.position);
            if (curCamLimt == null)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, vet3, Mathf.Clamp(Time.deltaTime * Mathf.Clamp((dst / maxDst), 0.1f, 1), 0, 1));
                wanderTar = vet3;
            }
            if (curCamLimt != null)
            {
                Vector3 tmpVet3 = cam.transform.position;
                if ((!curCamLimt.bar.left || (vet3.x - cam.orthographicSize * rat > camLimitRect.center.x - camLimitRect.size.x / 2.0f)) &&
                    (!curCamLimt.bar.right || (vet3.x + cam.orthographicSize * rat < camLimitRect.center.x + camLimitRect.size.x / 2.0f)))
                {
                    tmpVet3.x = vet3.x;
                }
                else
                {
                    if (curCamLimt.bar.left && vet3.x - cam.orthographicSize * rat <= camLimitRect.center.x - camLimitRect.size.x / 2.0f)
                    {
                        tmpVet3.x = camLimitRect.center.x - camLimitRect.size.x / 2.0f + cam.orthographicSize * rat;
                    }
                    if (curCamLimt.bar.right && vet3.x + cam.orthographicSize * rat >= camLimitRect.center.x + camLimitRect.size.x / 2.0f)
                    {
                        tmpVet3.x = camLimitRect.center.x + camLimitRect.size.x / 2.0f - cam.orthographicSize * rat;
                    }
                }

                if ((!curCamLimt.bar.down || (vet3.y - cam.orthographicSize > camLimitRect.center.y - camLimitRect.size.y / 2.0f)) &&
                    (!curCamLimt.bar.top || (vet3.y + cam.orthographicSize < camLimitRect.center.y + camLimitRect.size.y / 2.0f)))
                {
                    tmpVet3.y = vet3.y;
                }
                else
                {
                    if (curCamLimt.bar.down && vet3.y - cam.orthographicSize <= camLimitRect.center.y - camLimitRect.size.y / 2.0f)
                    {
                        tmpVet3.y = camLimitRect.center.y - camLimitRect.size.y / 2.0f + cam.orthographicSize;
                    }
                    if (curCamLimt.bar.top && vet3.y + cam.orthographicSize >= camLimitRect.center.y + camLimitRect.size.y / 2.0f)
                    {
                        tmpVet3.y = camLimitRect.center.y + camLimitRect.size.y / 2.0f - cam.orthographicSize;
                    }
                }

                cam.transform.position = Vector3.Lerp(cam.transform.position, tmpVet3, Mathf.Clamp(Time.deltaTime * Mathf.Clamp((dst / maxDst), 0.1f, 1), 0, 1));
                wanderTar = tmpVet3;
            }
        }
    }
    private void OnDrawGizmos()
    {
        //cam = Camera.main;
        //player= FindObjectOfType<PlayerCtrl>();
        //rat = (float)Screen.width / (float)Screen.height;
        //Gizmos.color = Color.white;
        if (cam != null)
        {
            Gizmos.DrawIcon(Vector3.Scale(cam.transform.position, new Vector3(1, 1, 0)), "camCenter.png");
            Gizmos.DrawIcon(Vector3.Scale(cam.transform.position - (Vector3)Offset, new Vector3(1, 1, 0)), "curTar.png");
            Gizmos.DrawIcon(Vector3.Scale(wanderTar - (Vector3)Offset, new Vector3(1, 1, 0)), "wanderTar.png");
        }
        //Vector3 vet3 = new Vector3(player.transform.position.x + Offset.x, player.transform.position.y + Offset.y, cam.transform.position.z) + player.transform.right;
        //Gizmos.color = Color.blue;
        //Vector2[] rect = new Vector2[4];
        //for(int i = 0; i < 4; i++)
        //{
        //    rect[0] = new Vector2(-1, 1);
        //    rect[1] = new Vector2(-1, -1);
        //    rect[2] = new Vector2(1, -1);
        //    rect[3] = new Vector2(1, 1);
        //}
        //for (int i = 0; i < 3; i++)
        //{
        //    Gizmos.DrawLine(vet3 + rect[i].x * cam.orthographicSize * rat * Vector3.right + rect[i].y * cam.orthographicSize * Vector3.up, vet3 + rect[i+1].x * cam.orthographicSize * rat * Vector3.right + rect[i+1].y * cam.orthographicSize* Vector3.up);
        //}
        //Gizmos.DrawLine(vet3 + rect[3].x * cam.orthographicSize * rat * Vector3.right + rect[3].y * cam.orthographicSize * Vector3.up, vet3 + rect[0].x * cam.orthographicSize * rat * Vector3.right + rect[0].y * cam.orthographicSize * Vector3.up);
    }
}