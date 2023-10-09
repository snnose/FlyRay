using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoot : MonoBehaviour
{
    public GameObject player;
    public GameObject ground;
    public Rigidbody2D playerRb2D;
    public BoxCollider2D groundCollider2D;

    public PlayerControl playerControl;

    Vector2 minPos = Vector2.zero;
    List<Vector2> posList;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb2D = player.GetComponent<Rigidbody2D>();
        ground = GameObject.FindGameObjectWithTag("Ground");
        groundCollider2D = ground.GetComponent<BoxCollider2D>();

        playerControl = player.GetComponent<PlayerControl>();

        posList = new List<Vector2>();
    }
   
    // Update is called once per frame
    void Update()
    {
        // ���콺 ���� ��ư �Է� ��
        if (Input.GetMouseButtonDown(0) && playerControl.isIdle())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

            if (hit.collider != null && hit.collider.tag == "Player")
            {
                playerControl.beginGrab();
            }

            // ����� ���
            //Debug.Log("���� ���� : " + playerControl.currState);
        }

        // ���콺 ���� ��ư �巡�� ��
        if (playerControl.isGrab())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            playerRb2D.velocity = Vector2.zero;
            playerRb2D.angularVelocity = 0f;
            player.transform.position = mousePos;

            posList.Add(mousePos);
        }

        // ���콺 ���� ��ư �� ��
        if (Input.GetMouseButtonUp(0) && playerControl.isGrab())
        {
            // �÷��̾ ���� �����Ѵ�.
            playerControl.beginFly();

            Vector2 minPos = findMinPos();                
            Vector2 lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);                
            //int throwTime = 0;

            // ������ ���� ���� ���
            Vector2 throwVector = lastPos - minPos;

            // �÷��̾ ���� ���Ѵ�
            playerRb2D.AddForce(throwVector * 3, ForceMode2D.Impulse);

            this.gameObject.AddComponent<SpawnManager>();

            // ����� ���
            //Debug.Log("minPos = (" + minPos.x + ", " + minPos.y +
            //    ") / lastPos = (" + lastPos.x + ", " + lastPos.y +
            //    ")");

            //Debug.Log("���� ���� : " + playerControl.currState);
        }

        // ���� ���� �߿� ���� ��������
        if (playerControl.isFly() && playerControl.isOnGround)
        {
            playerControl.beginLand();

            //Debug.Log("���� ���� : " + playerControl.currState);
        }

        // ���� �������� ����ٸ�
        if (playerControl.isLand() && playerRb2D.velocity == Vector2.zero)
        {
            playerControl.beginStop();

            //Debug.Log("���� ���� : " + playerControl.currState);
        }
    }

    Vector2 findMinPos()
    {
        Vector2 minPos = Vector2.zero;
        int lastNum = posList.Count;

        for (int i = lastNum - 1; i > 0; i--)
        {
            //throwTime++;

            if (posList[i - 1].x > posList[i].x || posList[i - 1].y > posList[i].y)
            {
                minPos = posList[i];
                break;
            }
        }

        posList.Clear();
        return minPos;
    }
}
