using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int MoneyValue;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameController.instance.UpdateMoney(MoneyValue);
            Destroy(gameObject);
        }
    }
}
