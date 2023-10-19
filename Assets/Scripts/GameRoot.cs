using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRoot : MonoBehaviour
{
    private GameObject player;
    private GameObject ground;
    private GameObject fuelUI;

    public PlayerControl playerControl;
    public FuelUIControl fuelUIControl;

    private Rigidbody2D playerRb2D;
    private BoxCollider2D groundCollider2D;

    private PlayerControl.PlayerInfo playerInfo;
    private Image fuelGage;
    private float fuelAmount;

    private Vector2 minPos = Vector2.zero;
    private List<Vector2> posList;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ground = GameObject.FindGameObjectWithTag("Ground");
        fuelUI = GameObject.FindGameObjectWithTag("FuelUI");

        groundCollider2D = ground.GetComponent<BoxCollider2D>();
        playerRb2D = player.GetComponent<Rigidbody2D>();

        playerControl = player.GetComponent<PlayerControl>();
        fuelUIControl = fuelUI.GetComponent<FuelUIControl>();

        playerInfo = playerControl.GetPlayerInfo();
        fuelGage = fuelUIControl.GetFuelGage();
        fuelAmount = playerInfo.GetFuelAmount();
        posList = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 왼쪽 버튼 입력 시
        if (Input.GetMouseButtonDown(0) && playerControl.IsIdle())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

            if (hit.collider != null && hit.collider.tag == "Player")
            {
                playerControl.BeginGrab();
            }

            // 디버깅 출력
            //Debug.Log("현재 상태 : " + playerControl.currState);
        }

        // 마우스 왼쪽 버튼 드래깅 시
        if (playerControl.IsGrab())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            playerRb2D.velocity = Vector2.zero;
            playerRb2D.angularVelocity = 0f;
            player.transform.position = mousePos;

            posList.Add(mousePos);

            // 마우스 왼쪽 버튼 땔 때
            if (Input.GetMouseButtonUp(0))
            {
                // 플레이어가 날기 시작한다.
                playerControl.BeginFly();

                Vector2 minPos = findMinPos();
                Vector2 lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //int throwTime = 0;

                // 던지는 방향 벡터 계산
                Vector2 throwVector = lastPos - minPos;

                // 플레이어에 힘을 가한다
                playerRb2D.AddForce(throwVector * 3, ForceMode2D.Impulse);

                // 디버깅 출력
                //Debug.Log("minPos = (" + minPos.x + ", " + minPos.y +
                //    ") / lastPos = (" + lastPos.x + ", " + lastPos.y +
                //    ")");

                //Debug.Log("현재 상태 : " + playerControl.currState);
            }
        }

        // 비행 상태 중
        if (playerControl.IsFly())
        {
            // 점점 아래로 기운다
            player.transform.rotation *=
                Quaternion.Euler(0f, 0f, -0.02f);

            if (Input.GetKey(KeyCode.W))
            {
                player.transform.rotation *=
                    Quaternion.Euler(0f, 0f, 0.1f);
            }

            if (Input.GetKey(KeyCode.S))
            {
                player.transform.rotation *=
                    Quaternion.Euler(0f, 0f, -0.1f);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                // 현재 바라보는 방향을 계산
                Vector2 dir = CalDir();
                // 힘을 가한다
                playerRb2D.AddForce(dir * 2f, ForceMode2D.Force);

                fuelAmount -= Time.deltaTime * 100f;
                Debug.Log(fuelAmount);
                ChangeFuelGageAmount(fuelAmount / 100);
            }

            // 땅에 떨어지면
            if (playerControl.isOnGround)
                playerControl.BeginLand();
        }

        // 땅에 떨어지고 멈췄다면
        if (playerControl.IsLand() && playerRb2D.velocity == Vector2.zero)
        {
            playerControl.BeginStop();

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

    private Vector2 CalDir()
    {
        Vector2 dir = Vector2.one;
        float dirX = dir.x * Mathf.Cos(player.transform.rotation.z);
        float dirY = dir.y * Mathf.Sin(player.transform.rotation.z);

        dir = new Vector2(dirX, dirY);

        return dir;
    }

    void ChangeFuelGageAmount(float amount)
    {
        fuelGage.fillAmount = amount;

        if (fuelGage.fillAmount <= 0f)
            fuelGage.fillAmount = 0f;
    }
}
