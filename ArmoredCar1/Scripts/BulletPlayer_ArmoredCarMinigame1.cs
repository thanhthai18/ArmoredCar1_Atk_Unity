using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletPlayer_ArmoredCarMinigame1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trash") || collision.gameObject.CompareTag("Finish"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("BodyCar") || collision.gameObject.CompareTag("Thief"))
        {
            Destroy(gameObject);
            collision.GetComponent<Enemy_ArmoredCarMinigame1>().StopAllCoroutines();
            collision.GetComponent<BoxCollider2D>().enabled = false;
            collision.transform.DOShakePosition(1.5f, 0.3f, 5, 45, false, true).OnComplete(() =>
            {
                Destroy(collision.gameObject);
                GameController_ArmoredCarMinigame1.instance.txtEnemy.text = --GameController_ArmoredCarMinigame1.instance.enemyCount + "/10";
                if(GameController_ArmoredCarMinigame1.instance.enemyCount == 0)
                {
                    Debug.Log("Win");
                }
            });
        }  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            Destroy(gameObject);
        }
    }
}
