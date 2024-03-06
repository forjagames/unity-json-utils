using UnityEngine;

namespace ForjaGames.Json.Examples
{
  public class BallPosition
  {
    public float x;

    public float y;

    public float z;

    public Vector3 Position => new Vector3(this.x, this.y, this.z);

    public BallPosition() : this(Vector3.zero)
    {
    }

    public BallPosition(Vector3 position)
    {
      this.x = position.x;
      this.y = position.y;
      this.z = position.z;
    }

    public BallPosition(Transform transform) : this(transform.position)
    {
    }
  }
}
