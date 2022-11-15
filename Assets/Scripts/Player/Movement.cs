using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static Movement instance;

    [SerializeField] float speed;
    [SerializeField] float DashForce;
    [SerializeField] ParticleSystem dust;
    [SerializeField] GameObject GameOver,back;
    [SerializeField] UnityEngine.UI.Text coins, Score;
    Camera cam;


    private Rigidbody2D rb;
    private Vector2 moveDir;
    private SpriteRenderer playerVisual;
    private Animator anim;

    public bool canMove;

    void Awake()
    {
        if(instance == null) instance = this;

        moveDir = new Vector2();
        rb = GetComponent<Rigidbody2D>();
        playerVisual = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        cam = Camera.main;

    }

    private void Start()
    {
        EnemyBehaviour.ended = false;
    }

    void Update()
    {
        if (canMove)
        {
            moveDir.x = Input.GetAxisRaw("Horizontal");
            moveDir.y = Input.GetAxisRaw("Vertical");

            if(moveDir.magnitude != 0)
            {
                CreateDust();
            }

            anim.SetFloat("Speed", moveDir.SqrMagnitude());

            Vector3 mousePosition = Input.mousePosition;
            mousePosition = cam.ScreenToWorldPoint(mousePosition);

            if (mousePosition.x > transform.position.x)
                playerVisual.flipX = false;
            else if (mousePosition.x < transform.position.x)
                playerVisual.flipX = true;

        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir * speed * Time.fixedDeltaTime);
        
    }

    void CreateDust()
    {
        if(!dust.isPlaying)
        dust.Play();
    }

    private void OnDestroy()
    {
        GameOver.SetActive(true);
        coins.text = Coin.Collected.ToString();
        Score.text = EnemyBehaviour.score.ToString();
        Debug.Log(EnemyBehaviour.score);
        if (Coin.Collected > 0)
        {
            MetafabManager.MintCurrency(Coin.Collected, () =>
            {
                back.SetActive(true);
            });
        }
        if (EnemyBehaviour.score > 0)
        {
            MetafabManager.GetPlayerData((res) =>
            {
                var sc = res.Score;
                if (sc == null) sc = "0";
                Debug.Log(sc);
                if (EnemyBehaviour.score > int.Parse(sc))
                {
                    MetafabManager.SetPlayerData(new ProtectedPlayerData() { Score = EnemyBehaviour.score.ToString(), WeaponEquipedID = "" });
                }
            });
        }
        Coin.ResetCount();
        Debug.Log(Coin.Collected);

        EnemyBehaviour.ResetCount();
        EnemyBehaviour.ended = true;

        Debug.Log(EnemyBehaviour.score);

    }

}
