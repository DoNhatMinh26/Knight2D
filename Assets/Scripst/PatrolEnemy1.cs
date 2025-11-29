using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;


public class PatrolEnemy1 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private float distance = 1f;
    [SerializeField] LayerMask layerMask;
    private bool facingRight = true;
    [SerializeField] private Transform player;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float retrieveDistance = 2.7f;
    [SerializeField] private float chaseSpeed = 4f;
    private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] LayerMask LayerPlayer;
    [SerializeField] float damageE = 1f;
    [SerializeField] private float maxHealth = 5f;
    private float currentHp;
    [SerializeField] private Image hpBar;
    public GameObject coinPrefab;  
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        UpdateHpBar();
        //Trong phạm vi tấn công
        if (Vector2.Distance(transform.position,player.position) <= attackRange)
        {
            Chanse();
        }
        else
        {
            Patrol();
        }
    }
    private void Chanse()
    {
        if (transform.position.x > player.position.x && facingRight == true)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (transform.position.x < player.position.x && facingRight == false)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }
        if (Vector2.Distance(transform.position, player.position) > retrieveDistance)
        {
            RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);

            if (hit == false)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, 0f * Time.deltaTime); 
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            animator.SetBool("Attack1", false);
        }
        else
        {
            animator.SetBool("Attack1", true);
        }

    }
    private void Patrol()
    {
        transform.Translate(Vector2.right * moveSpeed* Time.deltaTime);
        RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);
        Collider2D hit1 = Physics2D.OverlapCircle(attackPoint.position, 0.2f, layerMask);
        if ((hit == false || hit1 == true) && facingRight)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            facingRight = false;
        }
        else if ((hit == false || hit1 == true) && facingRight == false)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingRight = true;
        }
    }
    private void Attack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, LayerPlayer);
        if (collInfo)
        {
            if (collInfo.gameObject.GetComponent<Player>() != null)
            {
                collInfo.gameObject.GetComponent<Player>().TakeDamageP(damageE);
            }
        }
    }
    public void takeDamageE(float damage)
    {

        animator.SetTrigger("Hurt");
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        if (currentHp <= 0) Die();
    }
    private void Die()
    {
        
        Vector3 spawnPos = new Vector3(transform.position.x,transform.position.y+2f,0);
        Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        Destroy(this.gameObject);

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
        if (checkPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

}
