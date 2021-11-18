using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ArmoredCarMinigame1 : MonoBehaviour
{
    public BulletEnemy_ArmoredCarMinigame1 bulletEnemyPrefab;
    public Coroutine shootCoroutine;

    //private void Start()
    //{
    //    StartCoroutine(EnemyShooting());
    //}

    IEnumerator EnemyShooting()
    {
        while (true)
        {
            BulletEnemy_ArmoredCarMinigame1 bullet = Instantiate(bulletEnemyPrefab, transform.GetChild(0).transform.position, Quaternion.identity);
            if (transform.GetChild(0).localEulerAngles.y == 180)
            {
                bullet.tag = "Finish";
                bullet.transform.localScale = new Vector2(-0.25f, 0.22f);
                bullet.transform.DOMoveX(bullet.transform.position.x - 20, 5).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Destroy(bullet.gameObject);
                });
            }
            if (transform.GetChild(0).localEulerAngles.y == 0)
            {
                bullet.tag = "Trash";
                bullet.transform.localScale = new Vector2(0.25f, 0.22f);
                bullet.transform.DOMoveX(bullet.transform.position.x + 20, 5).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Destroy(bullet.gameObject);
                });
            }

            yield return new WaitForSeconds(Random.Range(3,6));
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            shootCoroutine = StartCoroutine(EnemyShooting());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            StopCoroutine(shootCoroutine);
        }
    }
}
