using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_ArmoredCarMinigame1 : MonoBehaviour
{
    public static GameController_ArmoredCarMinigame1 instance;
    public Camera mainCamera;
    public Armored_ArmoredMinigame1 armoredObj;
    public Button btnShoot;
    public GameObject enemyPrefab;
    public List<Transform> listPosEnemy = new List<Transform>();
    public Text txtEnemy;
    public int enemyCount;
    public bool isWin, isLose, isTutorial2;
    public GameObject tutorial1, tutorial2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);

        isWin = false;
        isLose = false;
        isTutorial2 = true;
    }

    void Start()
    {
        SetSizeCamera();
        btnShoot.onClick.AddListener(armoredObj.Shooting);
        mainCamera.transform.position = new Vector3(armoredObj.transform.position.x, mainCamera.transform.position.y, -10);
        SetUpMap();
        txtEnemy.text = enemyCount + "/10";
        tutorial2.SetActive(false);
        tutorial1.transform.localPosition = new Vector3(-7.29f, -3.07f, 4f);
        tutorial1.SetActive(true);
        tutorial1.transform.DOLocalMove(new Vector3(-6.73f, -3.17f, 4f), 1).SetLoops(-1);
    }

    void SetSizeCamera()
    {
        float f1 = 16.0f / 9;
        float f2 = Screen.width * 1.0f / Screen.height;
        mainCamera.orthographicSize *= f1 / f2;
    }

    void SetUpMap()
    {
        enemyCount = 10;
        List<int> listCheckSame = new List<int>();
        int ran;
        for (int v = 0; v < listPosEnemy.Count; v++)
        {
            listCheckSame.Add(v);
        }
        for (int i = 0; i < enemyCount; i++)
        {
            ran = Random.Range(0, listCheckSame.Count);
            GameObject tmpEnemy = Instantiate(enemyPrefab, listPosEnemy[listCheckSame[ran]].position, Quaternion.identity);
            if (listCheckSame[ran] < 3)
            {
                tmpEnemy.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                tmpEnemy.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
                tmpEnemy.tag = "Thief";
            }
            listCheckSame.RemoveAt(ran);
        }

    }

    private void Update()
    {
        mainCamera.transform.position = new Vector3(armoredObj.transform.position.x, mainCamera.transform.position.y, -10);
        mainCamera.transform.position = new Vector3(Mathf.Clamp(mainCamera.transform.position.x, -mainCamera.orthographicSize * Armored_ArmoredMinigame1.screenRatio + 2, mainCamera.orthographicSize * Armored_ArmoredMinigame1.screenRatio * 3 - 2), mainCamera.transform.position.y, -10);
    }

}
