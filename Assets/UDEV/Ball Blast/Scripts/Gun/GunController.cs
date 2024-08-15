using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunController : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed;
    private float m_curSpeed;
    private float m_limitOffset;
    private float m_minX;
    private float m_maxX;
    private Vector2 m_startingPos;

    private Rigidbody2D m_rb;
    [SerializeField] BarrelController[] m_barrelCtrs;

    public UnityEvent OnBallCollision;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_startingPos = transform.position;
        GetLimitPos();
    }

    private void Update()
    {
        MoveAndShoot();
        LimitMove();
    }

    private void GetLimitPos()
    {
        GetLimitOffsetX();
        m_minX = ViewPortUtil.MinX + m_limitOffset;
        m_maxX = ViewPortUtil.MaxX - m_limitOffset;
    }

    private void GetLimitOffsetX()
    {
        var collider = GetComponent<Collider2D>();
        if (collider == null) return;
        m_limitOffset = collider.bounds.size.x / 2;
    }

    private void MoveAndShoot()
    {
        if (!Input.GetMouseButton(0)) return;
        Move();
        BarrelsTrigger();
    }

    private void Move()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float movingTargetPosX = Mathf.Lerp(transform.position.x, mousePos.x, m_moveSpeed * Time.deltaTime);
        Vector2 movingTargetPos = new Vector2(movingTargetPosX, transform.position.y);
        transform.position = movingTargetPos;
    }

    private void LimitMove()
    {
        float limitPosX = Mathf.Clamp(transform.position.x, m_minX, m_maxX);
        Vector3 limitPos = new Vector3(limitPosX, transform.position.y, transform.position.z);
        transform.position = limitPos;
    }

    private void BarrelsTrigger()
    {
        if (m_barrelCtrs == null || m_barrelCtrs.Length <= 0) return;
        for (int i = 0; i < m_barrelCtrs.Length; i++)
        {
            var barrelCtr = m_barrelCtrs[i];
            if (barrelCtr == null) continue;
            barrelCtr.Shoot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Ball.ToString()))
        {
            OnBallCollision?.Invoke();
        }
    }
}
