using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheels : MonoBehaviour
{
    [SerializeField] private Transform m_gun;
    private float m_lastGunPosX;
    private float m_wheelDiameter;

    private SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_lastGunPosX = m_gun.transform.position.x;
        m_wheelDiameter = m_spriteRenderer.bounds.size.x * transform.localScale.x;
    }

    private void Update()
    {
        float diffBetweenPosX = m_gun.position.x - m_lastGunPosX;
        m_lastGunPosX = m_gun.position.x;
        float angle = 360 * diffBetweenPosX /(Mathf.PI * m_wheelDiameter);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y,
            transform.localEulerAngles.z - angle);
    }
}
