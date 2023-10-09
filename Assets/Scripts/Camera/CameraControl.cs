using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject player = null;
    private Vector3 posDiff = Vector3.zero;

    private PlayerRoot playerRoot;

    // Start is called before the first frame update
    void Start()
    {
        playerRoot = gameObject.AddComponent<PlayerRoot>() as PlayerRoot;
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

        // ���ּ��� ���ų� ���� ���� ��(�̵� ���� ��)
        // ī�޶� ���ּ��� ���󰣴�.
        switch(playerRoot.playerControl.currState)
        {
            case PlayerControl.Player.state.FLIED:
                this.transform.position =
                    Vector3.Lerp(this.transform.position, newPos + posDiff, Time.deltaTime * 15f);
                //Debug.Log("ī�޶� FLIED ����");
                break;

            case PlayerControl.Player.state.LANDED:
                this.transform.position = 
                    Vector3.Lerp(this.transform.position, newPos + posDiff, Time.deltaTime * 15f);
                //Debug.Log("ī�޶� LANDED ����");
                break;
            case PlayerControl.Player.state.STOP:
                this.transform.position =
                    Vector3.Lerp(this.transform.position, newPos + posDiff, Time.deltaTime * 15f);
                break;
            default:
                //Debug.Log("ī�޶� default ����");
                break;
        }
    }
}
