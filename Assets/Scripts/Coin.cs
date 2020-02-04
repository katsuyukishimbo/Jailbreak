using UnityEngine;
public class Coin : MonoBehaviour
{
  [SerializeField]
  float rotateSpeed = 90.0f;
  GameManager gameManager;
  void Start()
  {
    gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
  }
  void Update()
    {
    transform.Rotate(Vector3.right * Time.deltaTime * rotateSpeed);
  }
  void OnCollisionEnter(Collision collision)
  {
    if(collision.gameObject.tag == "Player")
    {
      gameManager.Score += 1;
      Destroy(gameObject);
    }
  }
}