
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public float attackrange = 5f;
    public Transform attackpoint;
    public float attackraduis = .5f;
    public LayerMask attacklayer;
    public int maxhealth = 2;
    public GameObject explosionprefab;

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


        if (Vector2.Distance(player.position, transform.position) <= attackrange) {

            RAttack();
        }

       
        
    }
    void RAttack()
    {
       int randomattack= Random.Range(0, 2);
        if (randomattack == 0)
        {
            animator.SetTrigger("Attack1");

        }
        else
        {
            animator.SetTrigger("Attack2");

        }

    }

    public void attack()
    {
       Collider2D collInfo= Physics2D.OverlapCircle(attackpoint.position, attackraduis, attacklayer);
        if (collInfo)
        {
            collInfo.GetComponent<Player>().takedamage(1);
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackrange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackpoint.position, attackraduis);

        
    }

    private void die()
    {
        GameObject temexplosioneffect = Instantiate(explosionprefab, transform.position, Quaternion.identity);
        Destroy(temexplosioneffect, 1f); 
        Destroy(this.gameObject);

    }
}
