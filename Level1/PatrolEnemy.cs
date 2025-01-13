using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    public int maxHealth = 5;
    public float movespeed = 2f;
    public Transform checkpoint;
    public float distance = 1f;
    public LayerMask layermask;
    public bool facingleft = true;
    public bool inrange = false;
    public Transform player;
    public float attackrange =10f ;
    public float retrieved = 2.5f;
    public float chasespeed = 4f;
    public Animator animator;

    public Transform attackpoin;
    public float attackradius =1f ;
    public LayerMask attacklayer;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<GameManager>().isGameActive == false)
        {
            return;
        }
        if (maxHealth <= 0)
        {
            die(); 
        }
        if (Vector2.Distance(transform.position, player.position) <=attackrange)
        {
            inrange = true;

        }
        else
        {
            inrange = false;
        }

        if (inrange)
        {
            if ( player.position.x>transform.position.x  && facingleft == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingleft = false;
            }
            else if( player.position.x < transform.position.x && facingleft==false)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingleft = true;

            }



            if (Vector2.Distance(transform.position, player.position)> retrieved)
            {
                animator.SetBool("Attack1", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chasespeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack1", true);


            }
            
        }
      

       
    }

   public void Attack()
   {
       Collider2D collInfo =   Physics2D.OverlapCircle(attackpoin.position, attackradius, attacklayer);
            if (collInfo) { 
               if (collInfo.gameObject.GetComponent<movement1>()!= null)
            {
                collInfo.gameObject.GetComponent<movement1>().TakeDamage(1);
            }
        }   
    }

    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0)
        {
            return;
        }
        maxHealth -= damage;
    }



    private void OnDrawGizmosSelected()
    {
        if (checkpoint == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkpoint.position, Vector2.down * distance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackrange);

    
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackpoin.position, attackradius);



    }
    void die()
    {
        Debug.Log(this.transform.name + "died");
        Destroy(this.gameObject);
    }
}
