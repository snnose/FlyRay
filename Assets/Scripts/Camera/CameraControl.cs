using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject player = null;
    private Vector3 posDiff = Vector3.zero;

    private GameRoot playerRoot;

    // Start is called before the first frame update
    void Start()
    {
        playerRoot = gameObject.AddComponent<GameRoot>() as GameRoot;
        this.player = GameObject.FindGameObjectWithTag("Player");

        this.posDiff = this.transform.position - player.transform.position;
        posDiff.z = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        Vector3 newPos =
            new Vector3(player.transform.position.x,
                        player.transform.position.y,
                          this.transform.position.z);

        // 우주선이 날거나 착륙 중일 때(이동 중일 때)
        // 카메라가 우주선을 따라간다.
        switch(playerRoot.playerControl.currState)
        {
            case PlayerControl.PlayerInfo.state.FLIED:
                this.transform.position =
                    Vector3.Lerp(this.transform.position, newPos + posDiff, Time.deltaTime * 15f);
                //Debug.Log("카메라 FLIED 진입");
                break;

            case PlayerControl.PlayerInfo.state.LANDED:
                this.transform.position = 
                    Vector3.Lerp(this.transform.position, newPos + posDiff, Time.deltaTime * 15f);
                //Debug.Log("카메라 LANDED 진입");
                break;
            case PlayerControl.PlayerInfo.state.STOP:
                this.transform.position =
                    Vector3.Lerp(this.transform.position, newPos + posDiff, Time.deltaTime * 15f);
                break;
            default:
                //Debug.Log("카메라 default 진입");
                break;
        }
    }
}
