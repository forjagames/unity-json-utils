using System.Collections;
using System.Collections.Generic;
using ForjaGames.Json.Examples;
using UnityEngine;

namespace ForjaGames.Json.Example
{
  public class ParseExampleScript : MonoBehaviour
  {
    [SerializeField]
    private Transform ball;

    [HideInInspector]
    private string jsonBallPosition;

    void Start()
    {
      this.SaveBallPosition();
    }

    public void SaveBallPosition()
    {
      this.jsonBallPosition = JsonParser.Json(new BallPosition(this.ball.transform.position));

      Debug.Log("Ball position saved!");
    }

    public void LoadBallPosition()
    {
      var ballPosition = JsonParser.Parse<BallPosition>(this.jsonBallPosition).Position;

      ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
      ball.GetComponent<Rigidbody>().transform.position = ballPosition;

      Debug.Log("Ball position loaded!");
    }

    public void PrintBallPositionFromJSON()
    {
      Debug.Log("Saved ball position:");
      Debug.Log(this.jsonBallPosition);

      Debug.Log("Current ball position:");
      Debug.Log(JsonParser.Json(new BallPosition(this.ball.transform.position)));
    }
  }
}