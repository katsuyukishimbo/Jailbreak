using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MoveSceneManager : SingletonMonoBehaviour<MoveSceneManager>
{
  [SerializeField, Tooltip("フェードイン、フェードアウトの間の待ち時間")]
  float fadeWaitTime = 0;
  [SerializeField, Tooltip("トランジションに使用するカンバスのプレハブ")]
  GameObject fadeCanvasPrefab;
  int currentStageNum = 0; //現在のステージ番号（0始まり）
  GameObject fadeCanvasObj;
  FadeCanvas fadeCanvas;
  GameManager gameManager;
  public int CurrentStageNum
  {
    set
    {
      currentStageNum = Mathf.Clamp(value, 0, SceneManager.sceneCountInBuildSettings);
    }
    get
    {
      return currentStageNum;
    }
  }
  //BuildSettingに登録されているシーンの数を取得
  //ただし、エディタ上で登録されてないシーンを読んでいる状態だとそのシーンも数に含まれるので注意
  public int NumOfStage
  {
    get
    {
      return SceneManager.sceneCountInBuildSettings;
    }
  }
  public string StageName
  {
    get
    {
      return SceneManager.GetActiveScene().name;
    }
  }
  void Awake()
  {
    if (this != Instance)
    {
      Destroy(gameObject);
      return;
    }
    DontDestroyOnLoad(gameObject);
    gameManager = GetComponent<GameManager>();
  }
  void Start()
  {
    //デリゲートの登録
    SceneManager.sceneLoaded += OnSceneLoaded;
  }
  //シーンのロード時に実行（最初は実行されない）
  void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    if(StageName != "Title")
    {
      gameManager.LoadComponents();
    }
  }
  //シーンの読み込みと待機を行うコルーチン
  IEnumerator WaitForLoadScene(int sceneNum)
  {
    //フェードオブジェクトを生成
    fadeCanvasObj = Instantiate(fadeCanvasPrefab);
    //コンポーネントを取得
    fadeCanvas = fadeCanvasObj.GetComponent<FadeCanvas>();
    //フェードインさせる
    fadeCanvas.fadeIn = true;
    yield return new WaitForSeconds(fadeWaitTime);
    //シーンを非同期で読込し、読み込まれるまで待機する
    yield return SceneManager.LoadSceneAsync(sceneNum);
    //フェードアウトさせる
    fadeCanvas.fadeOut = true;
    CurrentStageNum = sceneNum;
  }
  //任意のステージに移動する処理
  public void MoveToStage(int sceneNum)
  {
    //コルーチンを実行
    StartCoroutine(WaitForLoadScene(sceneNum));
  }
}