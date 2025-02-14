using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraySword : MonoBehaviour
{
    public int damage;
    public bool isAttacking;
    public Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        Attack();
    }

    void Attack()
    {
        StartCoroutine("ATK");
    }

    IEnumerator ATK()
    {
        if(Input.GetButtonDown("Fire1") && !player.DMGreference)
        {
            yield return new WaitForSeconds(0.15f);
            isAttacking = true;

            yield return new WaitForSeconds(0.0001f);
            isAttacking = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && isAttacking == true)
        {
            collision.gameObject.GetComponent<Enemy1>().Damage(damage);
        }
    }
}
