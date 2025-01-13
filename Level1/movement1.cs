using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class movement1 : MonoBehaviour
{

    public int maxhealth = 3 ;
    public Text health;
    public Animator animator;
    public Rigidbody2D rb;
    private float movement22;
    public float movespeed = 5f;
    private bool facingright = true;
    public float jumhight = 5f;
    public bool isGround = true;
    public Transform attackpoint;
    public float attackradius = 1f;
    public LayerMask attacklayer;
    public int currentcoin = 0;
    public Text coinText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (maxhealth <= 0)
        {
            
            Die();
        }
        coinText.text = currentcoin.ToString();


        health.text = maxhealth.ToString();
        movement22 = Input.GetAxis("Horizontal");

        if (movement22 < 0f && facingright)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingright = false;
        }
        else if (movement22 > 0f && facingright == false)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingright = true;
        }
        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            Jump();
            isGround = false;
            animator.SetBool("Jump", true);
        }
        if (Mathf.Abs(movement22) > 0.1f)
        {
            animator.SetFloat("Run", 1f);

        }
        else if (movement22 < 0.1f)
        {
            animator.SetFloat("Run", 0f);


        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }

    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement22, 0f, 0f) * Time.fixedDeltaTime * movespeed;

    }
    void Jump()
    {
        rb.AddForce(new Vector2(0f, jumhight), ForceMode2D.Impulse);



    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);
        }
    }
    public void Attack()
    {
       Collider2D collInfo= Physics2D.OverlapCircle(attackpoint.position, attackradius, attacklayer);
        if (collInfo)
        {
            if (collInfo.gameObject.GetComponent<PatrolEnemy>()!= null)
            {
                collInfo.gameObject.GetComponent<PatrolEnemy>().TakeDamage(1);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (attackpoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackpoint.position, attackradius);
        
    }
    public void TakeDamage( int damage)
    {
        if (maxhealth <= 0)
        {
            return;
        }
        maxhealth -= damage;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            currentcoin += 1;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");
            Destroy(other.gameObject,1f);

        }
        if (other.gameObject.tag == "VictoryPoint")
        {
            FindAnyObjectByType<Scenemanagement>().Loadlevel();

        }
        
    }
    void Die()
    {
        Debug.Log("player died");
        FindObjectOfType<GameManager>().isGameActive = false;
        Destroy(this.gameObject);

    }



}
