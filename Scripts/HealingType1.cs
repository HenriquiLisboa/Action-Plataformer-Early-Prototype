using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingType1 : MonoBehaviour
{
    public int HealingPower = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Heal(HealingPower);
            Destroy(gameObject);
        }
    }
}
