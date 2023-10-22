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


    private float fuelAmount = 0f;
    private int throwTime = 0;
    private List<Vector2> posList = new List<Vector2>();

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
    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 ���� ��ư �Է� ��
        if (playerControl.IsIdle())
        {
            playerRb2D.freezeRotation = true;
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

                if (hit.collider != null && hit.collider.tag == "Player")
                {
                    playerControl.BeginGrab();
                }
            }
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
                playerRb2D.freezeRotation = false;

                throwTime = 0;
                Vector2 minPos = findMinPos(posList);
                Vector2 lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
               
                // ������ ���� ���� ���
                Vector2 throwVector = lastPos - minPos;
                // ���� �ð��� ª�� ���� ������ ���� ���ϵ���
                // �÷��̾ ���� ���Ѵ�
                playerRb2D.AddForce(throwVector * ((throwTime + 50) / throwTime), ForceMode2D.Impulse);
            }
        }

        // ���� ���� ��
        if (playerControl.IsFly())
        {
            // ������ ������ ���ϵ��� �÷��̾ ȸ����Ų��.
            float rotateZ = CalRotateZ(new Vector2(playerRb2D.velocity.x, playerRb2D.velocity.y));
            player.transform.rotation = Quaternion.Euler(0f, 180f, -rotateZ);

            if (Input.GetKey(KeyCode.Space) && fuelAmount > 0)
            {
                Vector2 F = Vector2.one;
                F.y *= 2f;

                // ���� ���Ѵ�
                playerRb2D.AddForce(F, ForceMode2D.Force);

                fuelAmount -= Time.deltaTime * 100f;
                ChangeFuelGageAmount(fuelAmount / 100);
            }

            // ���� ��������
            if (playerControl.isOnGround)
                playerControl.BeginLand();
        }

        // ���� �������ٸ�
        if (playerControl.IsLand())
        {
            // ȸ�� �ӵ��� ���� �� ������ �ȹٷ� ���� �� �����Ѵ�.
            playerRb2D.angularVelocity *= 0.1f;

            if (player.transform.rotation.z == 0f)
                playerRb2D.freezeRotation = true;

            // ���ּ��� ����ٸ�
            if (playerRb2D.velocity == Vector2.zero)
            {
                playerControl.BeginStop();
            }
        }
    }

    Vector2 findMinPos(List<Vector2> posList)
    {
        Vector2 mPos = Vector2.zero;
        int lastNum = posList.Count;

        for (int i = lastNum - 1; i > 0; i--)
        {
            throwTime++;

            if (posList[i - 1].x > posList[i].x && posList[i - 1].y > posList[i].y)
            {
                mPos = posList[i];
                break;
            }
        }

        posList.Clear();
        return mPos;
    }

    private float CalRotateZ(Vector2 v)
    {
        float f = 0f;
        Vector2 normV = v.normalized;

        f = Mathf.Atan2(normV.y ,normV.x) * Mathf.Rad2Deg;

        return f;
    }

    private Vector2 CalDir()
    {
        Vector2 dir = Vector2.one;
        float dirX = dir.x * Mathf.Cos(-player.transform.rotation.z);
        float dirY = dir.y * Mathf.Sin(-player.transform.rotation.z);

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
