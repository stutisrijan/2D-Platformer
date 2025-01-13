using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movespeed = 5f;
    public Rigidbody2D rb;
    public float jumhight = 10f;
    public bool isGround = true;
    private float movement22;
    private bool facingright = true;
    public Animator animator;
    public int maxhealth = 3;

    public Transform attackpoint;
    public float attackradius = 1f;
    public LayerMask lattacklayer;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (maxhealth <= 0)
        {
            die();
        }
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
            Attackanimation();
        }


    }
    void Attackanimation()
    {
        animator.SetTrigger("Attack");
    }


    private void FixedUpdate()
    {
        transform.position += new Vector3(movement22, 0f, 0f) * Time.fixedDeltaTime * movespeed;

     

    }
    void Jump()
    {
        rb.AddForce(new Vector2(0f, jumhight), ForceMode2D.Impulse);



    }
    public void attack()
    {
       Collider2D hit= Physics2D.OverlapCircle(attackpoint.position, attackradius, lattacklayer);
    
    
        if (hit)
        {
            hit.GetComponent<Enemy1>().takedamage(1);

        }




    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);

        }
    }
    public void takedamage(int damage)
    {
        if (maxhealth <= 0)
        {
            return;
        }
        maxhealth -= damage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackpoint.position, attackradius);
    }

    void die()
    {
        GameObject tempeff=  Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(tempeff, 0.501f);
        Destroy(this.gameObject);
    }

}

