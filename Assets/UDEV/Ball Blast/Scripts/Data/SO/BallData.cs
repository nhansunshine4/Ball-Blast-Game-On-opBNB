using UnityEngine;

[CreateAssetMenu(fileName = "New BallData", menuName = "UDEV/BB/Create BallData")]
public class BallData : ScriptableObject
{
    public Sprite spriteRender;
    public Vector2 bounceForce = new Vector2(245, 245);
    public float maxSpeed;
    public int minHealth;
    public int maxHealth;
}
