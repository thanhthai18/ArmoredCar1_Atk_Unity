using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ArmoredCarMinigame1 : MonoBehaviour
{
    public BulletEnemy_ArmoredCarMinigame1 bulletEnemyPrefab;
    public Coroutine shootCoroutine;
    public SkeletonAnimation anim;
    [SpineAnimation] public string anim_Idle, anim_Khoc, anim_PhatNo;

    private void Start()
    {
        //anim.state.Complete += AnimComplete;
        //PlayAnim(anim, anim_Idle, true);
    }

    private void AnimComplete(Spine.TrackEntry trackEntry)
    {
        //if (trackEntry.Animation.Name == anim_DinhDon || trackEntry.Animation.Name == anim_BossDinhDon)
        //{
        //    PlayAnim(anim, anim_Idle, true);
        //}
    }

    public void PlayAnim(SkeletonAnimation anim, string nameAnim, bool loop)
    {
        anim.state.SetAnimation(0, nameAnim, loop);
    }


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

            yield return new WaitForSeconds(Random.Range(3, 6));

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
