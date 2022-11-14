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
    [SerializeField] UnityEngine.UI.Text coins;
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

        MetafabManager.MintCurrency(Coin.Collected, () =>
        {
            Coin.ResetCount();
            back.SetActive(true);
        });

    }

}
