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
        // 마우스 왼쪽 버튼 입력 시
        if (Input.GetMouseButtonDown(0) && playerControl.isIdle())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

            if (hit.collider != null && hit.collider.tag == "Player")
            {
                playerControl.beginGrab();
            }

            // 디버깅 출력
            //Debug.Log("현재 상태 : " + playerControl.currState);
        }

        // 마우스 왼쪽 버튼 드래깅 시
        if (playerControl.isGrab())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            playerRb2D.velocity = Vector2.zero;
            playerRb2D.angularVelocity = 0f;
            player.transform.position = mousePos;

            posList.Add(mousePos);
        }

        // 마우스 왼쪽 버튼 땔 때
        if (Input.GetMouseButtonUp(0) && playerControl.isGrab())
        {
            // 플레이어가 날기 시작한다.
            playerControl.beginFly();

            Vector2 minPos = findMinPos();                
            Vector2 lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);                
            //int throwTime = 0;

            // 던지는 방향 벡터 계산
            Vector2 throwVector = lastPos - minPos;

            // 플레이어에 힘을 가한다
            playerRb2D.AddForce(throwVector * 3, ForceMode2D.Impulse);

            this.gameObject.AddComponent<SpawnManager>();

            // 디버깅 출력
            //Debug.Log("minPos = (" + minPos.x + ", " + minPos.y +
            //    ") / lastPos = (" + lastPos.x + ", " + lastPos.y +
            //    ")");

            //Debug.Log("현재 상태 : " + playerControl.currState);
        }

        // 비행 상태 중에 땅에 떨어지면
        if (playerControl.isFly() && playerControl.isOnGround)
        {
            playerControl.beginLand();

            //Debug.Log("현재 상태 : " + playerControl.currState);
        }

        // 땅에 떨어지고 멈췄다면
        if (playerControl.isLand() && playerRb2D.velocity == Vector2.zero)
        {
            playerControl.beginStop();

            //Debug.Log("현재 상태 : " + playerControl.currState);
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
