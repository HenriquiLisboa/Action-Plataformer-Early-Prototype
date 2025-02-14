using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player reference;
    public int Life;
    public int Jumper;
    public float Speed;
    public float Jumpforce;
    public bool isJumping;
    public bool DMGreference { get { return DMG; } }
    public GameObject player;

    private bool DMG; 
    private Rigidbody2D rig;
    private BoxCollider2D coll;
    private Animator anim;

    [SerializeField] private LayerMask JumpableGround; 

    // Start é chamado uma vez antes do primeiro frame Update
    void Awake()
    {
        reference = this;
    }
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        GameController.instance.UpdateLife(Life);
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        Jump();
        Attack();
    }

    void FixedUpdate()
    {
        Walk();
    }

    //Moves the character
    void Walk()
    {
        float movement = Input.GetAxis("Horizontal");

        if(DMG == false)
        {
            rig.velocity = new Vector2(movement * Speed, rig.velocity.y);
        }

        if(movement > 0)
        {
            anim.SetInteger("Transition", 2);
            transform.eulerAngles = new Vector3(0,0,0);
        }

        if(movement < 0)
        {
            anim.SetInteger("Transition", 2);
            transform.eulerAngles = new Vector3(0,180,0);
        }

        if(movement == 0)
        {
            anim.SetInteger("Transition", 1);
        }

        // Check for collision with walls
        if (isHittingWall() && !isGrounded())
        {
            rig.velocity = new Vector2(0f, rig.velocity.y);
        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && isGrounded() && !isHittingCelling() && Jumper > 0)
        {
            anim.SetInteger("Transition", 3);
            rig.velocity = new Vector2(rig.velocity.x, Jumpforce);
            anim.SetInteger("Jumping", 1);
            Jumper--;
        }
        if(Input.GetButtonDown("Jump") && isJumping && Jumper > 0)
        {
            anim.SetInteger("Transition", 3);
            rig.velocity = new Vector2(rig.velocity.x, Jumpforce);
            anim.SetInteger("Jumping", 1);
            Jumper--;
        }
        if(!isGrounded() && Jumper > 0)
        {
            isJumping = true;
        }
        if(isGrounded())
        {
            anim.SetInteger("Jumping", 0);
            Jumper = 1;
            isJumping = false;
        }
        else
        {
            anim.SetInteger("Jumping", 1);
        }
    }

    public void Attack()
    {
        StartCoroutine("Atacc");
    }

    IEnumerator Atacc()
    {
        if(Input.GetButtonDown("Fire1") && DMG == false)
        {
            anim.SetInteger("Attacking", 1);

            if(isJumping)
            {
                anim.SetInteger("Jumping", 1);
                anim.SetInteger("Attacking", 1);
            }

            yield return new WaitForSeconds(0.1f);
            anim.SetInteger("Attacking", 0);
        }
    }

    public void Damage(int dmg)
    {
        Life -= dmg;
        GameController.instance.UpdateLife(Life);
        anim.SetTrigger("Damage");

        if(transform.rotation.y == 0)
        {
            transform.position += new Vector3(-2f, 0, 0);
        }
        
        if(transform.rotation.y == 180)
        {
            transform.position += new Vector3(2f, 0, 0);
        }

        if(Life <= 0)
        {
            player.SetActive(false);
            GameController.instance.GameOver();
        }

        DMG = true;
        StartCoroutine("ResetDamage");
    }

    IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(1.5f);
        DMG = false;
    }

    public void Heal(int healing)
    {
        if(Life < 10)
        {
            Life += healing;
            GameController.instance.UpdateLife(Life);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Respawn")
        {
            if(Life > 1)
            {
                Life -= 1;
                GameController.instance.UpdateLife(Life);
                transform.position = new Vector2(-6.3f,-0.82f);
            }
            else
            {
                Life -= 1;
                GameController.instance.UpdateLife(Life);
                player.SetActive(false);
                GameController.instance.GameOver();
            }
        }        
    }

    private bool isGrounded()
    {
        float rayLength = 1.295f;
        float boxSizeX = 1.1f;
        float boxSizeY = 0.0225f; 

        RaycastHit2D hitBelow = Physics2D.BoxCast(coll.bounds.center, new Vector2(boxSizeX, boxSizeY), 0f, Vector2.down, rayLength, JumpableGround);
        DebugDrawBoxCast(coll.bounds.center, new Vector2(boxSizeX, boxSizeY), Vector2.down, rayLength, hitBelow.collider != null ? Color.red : Color.green);

        return hitBelow.collider != null;
    }

    private bool isHittingWall()
    {
        float rayLength = 0.49f;
        float boxSizeX = 0.18f;
        float boxSizeY = 2.56f;

        float horizontalMovement = Input.GetAxis("Horizontal");

        if (horizontalMovement > 0)
        {
            // Check for collision to the right only if moving right
            RaycastHit2D hitRight = Physics2D.BoxCast(coll.bounds.center, new Vector2(boxSizeX, boxSizeY), 0f, Vector2.right, rayLength, JumpableGround);
            DebugDrawBoxCast(coll.bounds.center, new Vector2(boxSizeX, boxSizeY), Vector2.right, rayLength, hitRight.collider != null ? Color.red : Color.green);

            return hitRight.collider != null;
        }
        else if (horizontalMovement < 0)
        {
            // Check for collision to the left only if moving left
            RaycastHit2D hitLeft = Physics2D.BoxCast(coll.bounds.center, new Vector2(boxSizeX, boxSizeY), 0f, Vector2.left, rayLength, JumpableGround);
            DebugDrawBoxCast(coll.bounds.center, new Vector2(boxSizeX, boxSizeY), Vector2.left, rayLength, hitLeft.collider != null ? Color.red : Color.green);

            return hitLeft.collider != null;
        }

        return false;
    }

    private bool isHittingCelling()
    {
        float rayLength = 0.49f;
        float boxSizeX = 1.05f;
        float boxSizeY = 2.5f;

        // Check for collision above
        RaycastHit2D hitAbove = Physics2D.BoxCast(coll.bounds.center, new Vector2(boxSizeX, boxSizeY), 0f, Vector2.up, rayLength, JumpableGround);
        DebugDrawBoxCast(coll.bounds.center, new Vector2(boxSizeX, boxSizeY), Vector2.up, rayLength, hitAbove.collider != null ? Color.red : Color.green);

        return hitAbove.collider != null;
    }

    void DebugDrawBoxCast(Vector3 origin, Vector2 size, Vector3 direction, float length, Color color)
    {
        Vector3 endPoint = origin + direction * length;

        Debug.DrawLine(endPoint + new Vector3(-size.x / 2, -size.y / 2), endPoint + new Vector3(size.x / 2, -size.y / 2), color);
        Debug.DrawLine(endPoint + new Vector3(size.x / 2, -size.y / 2), endPoint + new Vector3(size.x / 2, size.y / 2), color);
        Debug.DrawLine(endPoint + new Vector3(size.x / 2, size.y / 2), endPoint + new Vector3(-size.x / 2, size.y / 2), color);
        Debug.DrawLine(endPoint + new Vector3(-size.x / 2, size.y / 2), endPoint + new Vector3(-size.x / 2, -size.y / 2), color);
    }
}