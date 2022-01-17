using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Armored_ArmoredMinigame1 : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public VariableJoystick variableJoystick;
    public Vector2 direction;
    public Camera mainCamera;
    public Vector2 prePos, currentPos;
    public bool isPlayingCoroutine = false;
    public static float screenRatio;
    public bool isHoldMouse;
    public BulletPlayer_ArmoredCarMinigame1 bulletPlayerPrefab;
    public int bulletPower;
    public bool isResetBullet;
    public float resetBulletProgress;
    public Text txtBullet;
    public int healthPlayer;
    public Color startColor;
    public Color endColor;
    public Coroutine fadeCoroutine;
    public Vector2 lastPos;


    private void Start()
    {
        screenRatio = Screen.width * 1f / Screen.height;
        bulletPower = 3;
        speed = 10;
        healthPlayer = 3;
        for (int i = 0; i < healthPlayer; i++)
        {
            mainCamera.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
        }

        txtBullet.text = bulletPower.ToString();
        isResetBullet = false;
        currentPos = transform.position;
        prePos = currentPos;
        //StartCoroutine(UpdatePos());
        StartCoroutine(ResetPower());

    }

    IEnumerator FadeMeshRender()
    {
        float timeFade = 2;
        startColor = GetComponent<Renderer>().materials[0].color;
        endColor = new Color(startColor.a, startColor.g, startColor.b, 0);
        GetComponent<PolygonCollider2D>().enabled = false;
        while (timeFade > 0 && !GameController_ArmoredCarMinigame1.instance.isLose)
        {
            for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++)
            {
                float lerp = Mathf.PingPong(Time.time, 0.5f) / 0.5f;
                timeFade -= Time.deltaTime;
                GetComponent<Renderer>().materials[i].color = Color.Lerp(startColor, endColor, lerp);
                yield return new WaitForSeconds(0);
            }
        }
        for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++)
        {
            GetComponent<Renderer>().materials[i].color = startColor;
        }
        isHoldMouse = true;
        yield return new WaitForSeconds(1);
        GetComponent<PolygonCollider2D>().enabled = true;
        StopCoroutine(fadeCoroutine);

    }
    //public Coroutine fadeCoroutine;
    //private MaterialPropertyBlock materialBlock;
    //[SerializeField] protected MeshRenderer meshRenderer;
    //[SerializeField] private bool onTest = false;

    // start
    //{
    //materialBlock = new MaterialPropertyBlock();
    //    if(onTest)
    //    {
    //        //fadeCoroutine = StartCoroutine(FadeMeshRender());

    //    }
    //}
    //IEnumerator FadeMeshRender()
    //{
    //    float timeFade = 2;       
    //    while (timeFade > 0)
    //    {
    //        float lerp = Mathf.PingPong(Time.time, 0.5f) / 0.5f;
    //        SetColorForMaterial(lerp);
    //        yield return null;
    //    }


    //    StopCoroutine(fadeCoroutine);

    //}

    //private void SetColorForMaterial(float alpha)
    //{
    //    materialBlock.SetColor("_Color", new Color(1, 1, 1, alpha));
    //    meshRenderer.SetPropertyBlock(materialBlock);
    //}


    //IEnumerator UpdatePos()
    //{
    //    while (!isPlayingCoroutine)
    //    {
    //        if (isHoldMouse)
    //        {
    //            currentPos = transform.position;
    //            yield return new WaitForSeconds(0.01f);
    //            prePos = currentPos;
    //            yield return new WaitForSeconds(0.01f);
    //            currentPos = transform.position;
    //        }
    //        else
    //            yield return new WaitForSeconds(0.1f);
    //    }
    //}
    private void LateUpdate()
    {
        if (isHoldMouse)
        {
            if (transform.position.x > lastPos.x)
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            if (transform.position.x < lastPos.x)
            {
                transform.localEulerAngles = new Vector3(0, 180, 0);
            }
        }
        lastPos = transform.position;
    }


    IEnumerator ResetPower()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (bulletPower == 0)
            {
                resetBulletProgress = 0;
                isResetBullet = true;
                GameController_ArmoredCarMinigame1.instance.btnShoot.GetComponent<Image>().fillAmount = 0;
                GameController_ArmoredCarMinigame1.instance.btnShoot.enabled = false;

            }
            if (bulletPower < 3)
            {
                bulletPower++;
                txtBullet.text = bulletPower.ToString();
            }
        }
    }

    public void Shooting()
    {
        if (GameController_ArmoredCarMinigame1.instance.isTutorial2)
        {
            GameController_ArmoredCarMinigame1.instance.isTutorial2 = false;
        }
        if (GameController_ArmoredCarMinigame1.instance.tutorial2.activeSelf)
        {
            GameController_ArmoredCarMinigame1.instance.tutorial2.SetActive(false);
            GameController_ArmoredCarMinigame1.instance.tutorial2.transform.DOKill();
        }
        if (bulletPower > 0 && !GameController_ArmoredCarMinigame1.instance.isLose)
        {
            bulletPower--;
            txtBullet.text = bulletPower.ToString();
            BulletPlayer_ArmoredCarMinigame1 bullet = Instantiate(bulletPlayerPrefab, transform.GetChild(0).transform.position, Quaternion.identity);
            if (transform.localEulerAngles.y == 0)
            {
                bullet.transform.localScale = new Vector2(0.3f, 0.25f);
                bullet.transform.DOMoveX(bullet.transform.position.x + 30, 5).OnComplete(() =>
                {
                    Destroy(bullet.gameObject);
                });
            }
            if (transform.localEulerAngles.y > 0)
            {
                bullet.transform.localScale = new Vector2(-0.3f, 0.25f);
                bullet.transform.DOMoveX(bullet.transform.position.x - 30, 5).OnComplete(() =>
                {
                    Destroy(bullet.gameObject);
                });
            }
        }
    }

    private void Update()
    {
        if (!GameController_ArmoredCarMinigame1.instance.isLose)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isHoldMouse = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isHoldMouse = false;
            }

            //if (isHoldMouse)
            //{
            //    if (currentPos.x > prePos.x)
            //    {
            //        //transform.localScale = new Vector2(0.25f, 0.25f);
            //        transform.localEulerAngles = new Vector3(0, 0, 0);
            //    }
            //    if (currentPos.x < prePos.x)
            //    {
            //        //transform.localScale = new Vector2(-0.25f, 0.25f);
            //        transform.localEulerAngles = new Vector3(0, 180, 0);
            //    }
            //}
            if (isResetBullet)
            {
                resetBulletProgress += Time.deltaTime;
                GameController_ArmoredCarMinigame1.instance.btnShoot.GetComponent<Image>().fillAmount = (resetBulletProgress) / 2f;
                if (resetBulletProgress > 2)
                {
                    resetBulletProgress = 2;
                    isResetBullet = false;
                    GameController_ArmoredCarMinigame1.instance.btnShoot.enabled = true;
                }
            }
        }


    }

    void ShowTutorial2()
    {
        GameController_ArmoredCarMinigame1.instance.tutorial2.transform.localPosition = new Vector3(6.84f, -0.91f, 4f);
        GameController_ArmoredCarMinigame1.instance.tutorial2.SetActive(true);
        GameController_ArmoredCarMinigame1.instance.tutorial2.transform.DOLocalMove(new Vector3(6.74f, -2.19f, 4f), 1).SetLoops(-1);

    }

    public void FixedUpdate()
    {
        if (variableJoystick.Horizontal != 0 || variableJoystick.Vertical != 0)
        {
            if (GameController_ArmoredCarMinigame1.instance.tutorial1.activeSelf)
            {
                GameController_ArmoredCarMinigame1.instance.tutorial1.SetActive(false);
                GameController_ArmoredCarMinigame1.instance.tutorial1.transform.DOKill();
            }
            if (GameController_ArmoredCarMinigame1.instance.isTutorial2)
            {
                GameController_ArmoredCarMinigame1.instance.isTutorial2 = false;
                Invoke(nameof(ShowTutorial2), 1);
            }
        }


        direction = new Vector2(variableJoystick.Horizontal, variableJoystick.Vertical * 0.7f);
        rb.velocity = direction * speed;
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -mainCamera.orthographicSize * screenRatio * 2 + 2, mainCamera.orthographicSize * screenRatio * 4 - 2), Mathf.Clamp(transform.position.y, -mainCamera.orthographicSize + 1, mainCamera.orthographicSize - 1));

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameController_ArmoredCarMinigame1.instance.isWin)
        {
            if (collision.gameObject.CompareTag("Thief") || collision.gameObject.CompareTag("Trash"))
            {
                Debug.Log("va cham enemy hoac dan ben trai");
                if (!GameController_ArmoredCarMinigame1.instance.isWin)
                {
                    fadeCoroutine = StartCoroutine(FadeMeshRender());
                    isHoldMouse = false;
                    if (healthPlayer == 1)
                    {
                        healthPlayer--;
                        mainCamera.transform.GetChild(0).GetChild(healthPlayer).gameObject.SetActive(false);
                        GetComponent<PolygonCollider2D>().enabled = false;
                        speed = 0;
                        transform.DOMoveX(transform.position.x + 2, 1);
                        Debug.Log("Thua");
                        StopAllCoroutines();
                    }
                    if (healthPlayer > 1)
                    {
                        healthPlayer--;
                        GameObject tmpHeart = mainCamera.transform.GetChild(0).GetChild(healthPlayer).gameObject;
                        tmpHeart.GetComponent<SpriteRenderer>().DOFade(0, 0.3f).OnComplete(() =>
                        {
                            tmpHeart.GetComponent<SpriteRenderer>().DOFade(1, 0.3f).OnComplete(() =>
                            {
                                tmpHeart.GetComponent<SpriteRenderer>().DOFade(0, 0.3f).OnComplete(() =>
                                {
                                    tmpHeart.GetComponent<SpriteRenderer>().DOFade(1, 0.3f).OnComplete(() =>
                                    {
                                        tmpHeart.SetActive(false);

                                    });
                                });
                            });
                        });
                        speed = 0;
                        transform.DOMoveX(transform.position.x + 2, 1).OnComplete(() =>
                        {
                            speed = 10;
                            currentPos = transform.position;
                            prePos = currentPos;
                        });
                    }
                }
            }
            if (collision.gameObject.CompareTag("BodyCar") || collision.gameObject.CompareTag("Finish"))
            {
                Debug.Log("va cham enemy hoac dan ben phai");
                if (!GameController_ArmoredCarMinigame1.instance.isWin)
                {
                    fadeCoroutine = StartCoroutine(FadeMeshRender());
                    isHoldMouse = false;
                    if (healthPlayer == 1)
                    {
                        healthPlayer--;
                        mainCamera.transform.GetChild(0).GetChild(healthPlayer).gameObject.SetActive(false);
                        GetComponent<PolygonCollider2D>().enabled = false;
                        speed = 0;
                        transform.DOMoveX(transform.position.x - 2, 1);
                        Debug.Log("Thua");
                        StopAllCoroutines();
                    }
                    if (healthPlayer > 1)
                    {
                        healthPlayer--;
                        GameObject tmpHeart = mainCamera.transform.GetChild(0).GetChild(healthPlayer).gameObject;
                        tmpHeart.GetComponent<SpriteRenderer>().DOFade(0, 0.3f).OnComplete(() =>
                        {
                            tmpHeart.GetComponent<SpriteRenderer>().DOFade(1, 0.3f).OnComplete(() =>
                            {
                                tmpHeart.GetComponent<SpriteRenderer>().DOFade(0, 0.3f).OnComplete(() =>
                                {
                                    tmpHeart.GetComponent<SpriteRenderer>().DOFade(1, 0.3f).OnComplete(() =>
                                    {
                                        tmpHeart.SetActive(false);

                                    });
                                });
                            });
                        });
                        transform.DOMoveX(transform.position.x - 2, 1).OnComplete(() =>
                        {
                            speed = 10;
                            currentPos = transform.position;
                            prePos = currentPos;
                        });
                    }
                }
            }
        }
    }
}
