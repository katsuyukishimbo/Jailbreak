using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;
[RequireComponent(typeof(MoveSceneManager))]
// [RequireComponent(typeof(SaveManager))]
// [RequireComponent(typeof(SoundManager))]
public class GameManager : SingletonMonoBehaviour<GameManager>
{
  public bool isDebugMode = false;
  [SerializeField]
  GameObject clearCanvasPrefab;
  [SerializeField]
  GameObject gameOverCanvasPrefab;
  [System.NonSerialized]
  public bool countDown = false;
  int score = 0;
  int numOfCoins = 0;
  MoveSceneManager moveSceneManager;
  // SaveManager saveManager;
  // SoundManager soundManager;
  ThirdPersonUserControl character;
  Text coinText;
  Canvas canvas;
  Button retryButton;
  Button nextButton;
  Button titleButton;
  public int Score
  {
    set
    {
      score = value;
      SetCoinText();
      if(score >= numOfCoins)
      {
        StageClear();
      }
    }
    get
    {
      return score;
    }
  }
  public int NumOfCoins
  {
    set
    {
      numOfCoins = value;
      SetCoinText();
    }
    get
    {
      return numOfCoins;
    }
  }
  public void Awake()
  {
    if (this != Instance)
    {
      Destroy(gameObject);
      return;
    }
    DontDestroyOnLoad(gameObject);
    moveSceneManager = GetComponent<MoveSceneManager>();
    // saveManager = GetComponent<SaveManager>();
    // soundManager = GetComponent<SoundManager>();
  }
  void Start()
  {
    if (Debug.isDebugBuild)
    {
      if(moveSceneManager.StageName != "Title")
      {
        LoadComponents();
      }
    }
  }
  public void LoadComponents()
  {
    character = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonUserControl>();
    score = 0;
  }
  public void StageClear()
  {
    countDown = false;
    character.allowInput = false;
    canvas = Instantiate(clearCanvasPrefab, transform.position, Quaternion.identity).GetComponent<Canvas>();
    nextButton = GameObject.Find("NextStageButton").GetComponent<Button>();
    titleButton = GameObject.Find("TitleButton").GetComponent<Button>();
    if (moveSceneManager.CurrentStageNum >= moveSceneManager.NumOfStage - 1)
    {
      nextButton.interactable = false;
    }
    else
    {
      nextButton.onClick.AddListener(() => moveSceneManager.MoveToStage(moveSceneManager.CurrentStageNum + 1));
    }
    titleButton.onClick.AddListener(() => moveSceneManager.MoveToStage(0));
  }
  public void GameOver()
  {
    countDown = false;
    character.allowInput = false;
    canvas = Instantiate(gameOverCanvasPrefab, transform.position, Quaternion.identity).GetComponent<Canvas>();
    retryButton = GameObject.Find("RetryButton").GetComponent<Button>();
    titleButton = GameObject.Find("TitleButton").GetComponent<Button>();
    retryButton.onClick.AddListener(() => moveSceneManager.MoveToStage(moveSceneManager.CurrentStageNum));
    titleButton.onClick.AddListener(() => moveSceneManager.MoveToStage(0));
  }
  void SetCoinText()
  {
    coinText = GameObject.Find("Coin_Num").GetComponent<Text>();
    coinText.text = Score.ToString() + "/" + numOfCoins.ToString();
  }
}