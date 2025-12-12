using UnityEngine;
using Unity.Mathematics;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    [Header("Run SFX")]
    [SerializeField] private float runSfxInterval = 0.35f; // nhịp bước chân
    [SerializeField] private float runSfxVolume = 0.6f;    // nhỏ hơn jump/attack
    private float runSfxTimer = 0f;
    private GameManager gameManager;
    private Rigidbody2D rb;
    private Animator animator;
    private float moveMent;
    [SerializeField] private float moveSpeed = 10f;
    private bool facingRight = true;
    [SerializeField] private float jumpHight = 10f;
    private bool isGround = true;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float distancePoint = 1f;
    private bool jumpPressed = false;
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float attackCooldown = 0.5f;
    private float nextAttackTime = 0f;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] LayerMask LayerPlayer;
    [SerializeField] float damageP = 1f;
    private float currentHp;
    [SerializeField] private Image hpBar;
    private Vector3 startPos;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }
    void Start()
    {
        currentHp = maxHealth;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateHpBar();
        Movement();
        jump();
        Animation();
        HandleRunSFX(); 
    }
    private void FixedUpdate()
    {
        if (jumpPressed == true)
        {
            rb.AddForce(new Vector2(0f, jumpHight), ForceMode2D.Impulse);

            jumpPressed = false;
        }
    }
    private void Movement()
    {

        moveMent = Input.GetAxis("Horizontal");
        transform.position += new Vector3(moveMent, 0f, 0f) * moveSpeed * Time.deltaTime;
        if (moveMent < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (moveMent > 0f && facingRight == false)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }
    }
    private void jump()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        

        if (Input.GetButtonDown("Jump") && isGround)
        {
            jumpPressed = true;
            AudioManager.Instance.SFX_Jump();
        }
    }

    private void Animation()
    {
        bool isJumping = !isGround;
        animator.SetBool("Jump", isJumping);
        if (math.abs(moveMent) > 0.1f)
        {
            animator.SetFloat("Run", 1f);
        }
        else if (moveMent < 0.1)
        {
            animator.SetFloat("Run", 0f);
        }
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            animator.SetTrigger("Attack1");
            AudioManager.Instance.SFX_Attack();
            Attack();
        }
    }

    private void HandleRunSFX()
    {
        bool isRunning = Mathf.Abs(moveMent) > 0.1f && isGround;

        if (!isRunning)
        {
            runSfxTimer = 0f;
            return;
        }

        runSfxTimer -= Time.deltaTime;
        if (runSfxTimer <= 0f)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.run, runSfxVolume);
            runSfxTimer = runSfxInterval;
        }
    }

    private void Attack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, LayerPlayer);
        if (collInfo)
        {
            if (collInfo.gameObject.GetComponent<PatrolEnemy1>() != null)
            {
                collInfo.gameObject.GetComponent<PatrolEnemy1>().takeDamageE(damageP);
            }
            if (collInfo.gameObject.GetComponent<PatrolBoss>() != null)
            {
                collInfo.gameObject.GetComponent<PatrolBoss>().takeDamageE(damageP);
            }
        }
    }

    public void TakeDamageP(float damage)
    {
        animator.SetTrigger("Hurt");
        AudioManager.Instance.SFX_Hit();

        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        if (currentHp <= 0) Die();
    }
    private void Die()
    {
        //Destroy(this.gameObject);
        gameManager.GameOver();
    }
    private void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHealth;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            AudioManager.Instance.SFX_TakeCoin();
            Destroy(collision.gameObject);
            gameManager.AddScore(1);
        }
        if (collision.CompareTag("Key"))
        {
            AudioManager.Instance.SFX_Win();
            Destroy(collision.gameObject);
            gameManager.GameWin();
        }
        if (collision.CompareTag("checkroi"))
        {

            animator.SetTrigger("Hurt");
            currentHp -= 10f;
            currentHp = Mathf.Max(currentHp, 0);
            if (currentHp <= 0) Die();
            transform.position = startPos;
        }
        if (collision.CompareTag("File"))
        {
            currentHp += 10f;
            if (currentHp > maxHealth) currentHp = maxHealth;
            Destroy(collision.gameObject);
            
        }

    }
}
