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
        // ���콺 ���� ��ư �Է� ��
        if (Input.GetMouseButtonDown(0) && playerControl.IsIdle())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

            if (hit.collider != null && hit.collider.tag == "Player")
            {
                playerControl.BeginGrab();
            }

            // ����� ���
            //Debug.Log("���� ���� : " + playerControl.currState);
        }

        // ���콺 ���� ��ư �巡�� ��
        if (playerControl.IsGrab())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            playerRb2D.velocity = Vector2.zero;
            playerRb2D.angularVelocity = 0f;
            player.transform.position = mousePos;

            posList.Add(mousePos);

            // ���콺 ���� ��ư �� ��
            if (Input.GetMouseButtonUp(0))
            {
                // �÷��̾ ���� �����Ѵ�.
                playerControl.BeginFly();

                Vector2 minPos = findMinPos();
                Vector2 lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //int throwTime = 0;

                // ������ ���� ���� ���
                Vector2 throwVector = lastPos - minPos;

                // �÷��̾ ���� ���Ѵ�
                playerRb2D.AddForce(throwVector * 3, ForceMode2D.Impulse);

                // ����� ���
                //Debug.Log("minPos = (" + minPos.x + ", " + minPos.y +
                //    ") / lastPos = (" + lastPos.x + ", " + lastPos.y +
                //    ")");

                //Debug.Log("���� ���� : " + playerControl.currState);
            }
        }

        // ���� ���� ��
        if (playerControl.IsFly())
        {
            // ���� �Ʒ��� ����
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
                // ���� �ٶ󺸴� ������ ���
                Vector2 dir = CalDir();
                // ���� ���Ѵ�
                playerRb2D.AddForce(dir * 2f, ForceMode2D.Force);

                fuelAmount -= Time.deltaTime * 100f;
                Debug.Log(fuelAmount);
                ChangeFuelGageAmount(fuelAmount / 100);
            }

            // ���� ��������
            if (playerControl.isOnGround)
                playerControl.BeginLand();
        }

        // ���� �������� ����ٸ�
        if (playerControl.IsLand() && playerRb2D.velocity == Vector2.zero)
        {
            playerControl.BeginStop();

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
