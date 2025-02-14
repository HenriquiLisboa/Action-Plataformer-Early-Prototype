using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public int Life;
    public int DMG;
    public float Speed;
    public float Walker;
    public float Timer;
    public bool WalkTurn = true;

    public GameObject enemy;
    private Animator anim;
    private Rigidbody2D rig;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
    
    }

    void FixedUpdate()
    {
        Timer += Time.deltaTime;

        if(Timer >= Walker)
        {
            WalkTurn = !WalkTurn;
            Timer = 0f;
        }
        
        if(WalkTurn)
        {
            transform.eulerAngles = new Vector2(0, 0);
            rig.velocity = Vector2.left * Speed;
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            rig.velocity = Vector2.right * Speed;
        }
    }

    public void Damage(int dmg)
    {
        Life -= dmg;
        anim.SetTrigger("Hit");

        if(Life <= 0)
        {
            enemy.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Damage(DMG);
        }
    }
}
