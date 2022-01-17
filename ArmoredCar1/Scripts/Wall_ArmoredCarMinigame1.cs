using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_ArmoredCarMinigame1 : MonoBehaviour
{
    public int health = -1;

    private void Start()
    {
        health = 3;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("People"))
        {
            
            Destroy(collision.gameObject);
            if(health == 1)
            {
                health = 0;
                Destroy(gameObject);
            }
            if(health == 2)
            {
                health--;
                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
            if (health == 3)
            {
                health--;
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
        }
        if (collision.gameObject.CompareTag("Trash") || collision.gameObject.CompareTag("Finish"))
        {
            Destroy(collision.gameObject);
        }
    }
}
