using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    private Vector3 posDiff = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        this.posDiff = this.transform.position - player.transform.position;
        posDiff.z = 0;
    }

    void FixedUpdate()
    {
        Vector3 newPos =
            new Vector3(player.transform.position.x,
                        player.transform.position.y - 1,
                          this.transform.position.z);

        // 우주선이 날거나 착륙 중일 때(이동 중일 때)
        // 카메라가 우주선을 따라간다.
        switch (PlayerControl.Instance.currState)
        {
            case PlayerInfo.state.FLIED:
                this.transform.position =
                    Vector3.Lerp(this.transform.position, newPos + posDiff, Time.deltaTime * 12.5f);
                //this.transform.position = newPos + posDiff;
                break;

            case PlayerInfo.state.LANDED:
                this.transform.position =
                    Vector3.Lerp(this.transform.position, newPos + posDiff, Time.deltaTime * 12.5f);
                //Debug.Log("카메라 LANDED 진입");
                break;
            default:
                //Debug.Log("카메라 default 진입");
                break;
        }
    }
}
