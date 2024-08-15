using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallController : MonoBehaviour
{
    public BallData[] ballDatas;
    private Animator m_anim;
    private Rigidbody2D m_rb;
    private SpriteRenderer m_spriteRenderer;
    private Collider2D m_collider;
    private BallData m_curBallData;

    private Vector2 m_lastVelocity;
    private Vector2 m_getHitPos;
    private Color32 m_curColor;

    private int m_curHealth;

    public UnityEvent OnInit;
    public UnityEvent<Vector2> OnTakeDamage;
    public UnityEvent OnDead;
    public UnityEvent<Vector3> OnDeadWithPosition;
    public UnityEvent<Vector2> OnGroundHitted;

    public BallData CurBallData { get => m_curBallData; private set => m_curBallData = value; }
    public Color32 CurColor { get => m_curColor; private set => m_curColor = value; }
    public int CurHealth { get => m_curHealth; private set => m_curHealth = value; }

    private void Awake()
    {
        m_anim = GetComponentInChildren<Animator>();
        m_rb = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_collider = GetComponent<Collider2D>();
        m_curBallData = Helper.GetRandom(ballDatas);
    }

    private void Update()
    {
        m_lastVelocity = m_rb.velocity;
    }

    public void Init()
    {
        if (m_curBallData == null) return;
        m_curHealth = Random.Range(m_curBallData.minHealth, m_curBallData.maxHealth);
        m_spriteRenderer.sprite = m_curBallData.spriteRender;
        m_curColor = GetRandomColor();
        m_spriteRenderer.color = m_curColor;

        OnInit?.Invoke();
    }

    private Color32 GetRandomColor()
    {
        byte r = (byte) Random.Range(0, 255);
        byte b = (byte)Random.Range(0, 255);
        byte g = (byte)Random.Range(0, 255);
        return new Color32(r, b, g, 255);
    }

    public void ActiveCollision()
    {
        if(m_collider == null) return;
        m_collider.isTrigger = false;
    }

    public void MoveTrigger(Vector2 force, bool  flipForceX = false)
    {
        if(m_rb == null) return;
        if (flipForceX)
        {
            m_rb.AddForce(new Vector2(force.x * -1, force.y));
        }
        else
        {
            m_rb.AddForce(force);
        }
    }

    private void ReflectVelocity(Collision2D collision)
    {
        if(m_curBallData == null || collision == null) return;
        var speed = m_lastVelocity.magnitude;
        speed = Mathf.Clamp(speed, 0f, m_curBallData.maxSpeed);
        var reflectDir = Vector3.Reflect(m_lastVelocity.normalized, collision.contacts[0].normal);
        reflectDir += GetRandomOffset(reflectDir);

        if (collision.gameObject.CompareTag(GameTag.Wall_Bottom.ToString()))
        {
            OnGroundHitted?.Invoke(collision.contacts[0].point);

            if (speed > 4.5f) return;
            m_rb.velocity = Vector2.zero;
            m_rb.angularVelocity = 0f;
            m_rb.AddForce(reflectDir * 200f);
            return;
        }
        m_rb.velocity = reflectDir * speed;
    }

    private Vector3 GetRandomOffset(Vector3 reflectDir)
    {
        if(reflectDir.x == 0)
        {
            float randOffsetX = Random.Range(-0.6f, 1f);
            return new Vector3(randOffsetX, 0f, 0f);
        }

        if (reflectDir.y == 0) {
            float randomOffsetY = Random.Range(-0.6f, 1);
            return new Vector3(0f, randomOffsetY, 0f);
        }
        return Vector3.zero;
    }

    public void TakeDamage(int damage, Vector2 getHitPos)
    {
        m_curHealth -= damage;
        m_curHealth = Mathf.Max(m_curHealth, 0);
        m_getHitPos = getHitPos;

        OnTakeDamage?.Invoke(getHitPos);

        if(m_curHealth <= 0)
        {
            Die();
        }

        m_anim?.SetBool(AnimParam.is_hitted.ToString(), true);
    }

    private void Die()
    {
        OnDeadWithPosition?.Invoke(transform.position);
        OnDead?.Invoke();

        gameObject.SetActive(false);
    }

    private void ResetBall()
    {
        m_rb.velocity = Vector3.zero;
        m_lastVelocity = Vector3.zero;
        m_rb.angularVelocity = 0f;
        m_collider.isTrigger = true;
    }

    public void BackToIdle()
    {
        m_anim?.SetBool(AnimParam.is_hitted.ToString(), false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ReflectVelocity(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ReflectVelocity(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ActiveCollision();
    }

    private void OnDisable()
    {
        ResetBall();

        OnInit?.RemoveAllListeners();
        OnDeadWithPosition?.RemoveAllListeners();
        OnDead?.RemoveAllListeners();
        OnTakeDamage?.RemoveAllListeners();
    }
}
