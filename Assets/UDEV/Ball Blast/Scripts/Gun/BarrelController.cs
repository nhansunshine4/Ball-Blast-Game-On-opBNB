using System;
using System.Collections;
using System.Collections.Generic;
using UDEV.SPM;
using UnityEngine;
using UnityEngine.Events;

public class BarrelController : MonoBehaviour
{
    public BarrelData statData;
    [SerializeField] private Transform m_shootingPoint;
    [PoolerKeys(target = PoolerTarget.NONE)]
    [SerializeField] private string m_bulletPoolKey;
    private float m_curFR;
    private bool m_isShootted;

    public UnityEvent OnShoot;

    private void Start()
    {
        LoadStats();
    }

    private void LoadStats()
    {
        if (statData == null) return;
        m_curFR = statData.firerate;
    }

    private void Update()
    {
        ReduceFirerate();
    }

    public void Shoot()
    {
        if(m_isShootted || m_shootingPoint == null) return;
        m_isShootted = true;
        var bulletClone = PoolersManager.Ins.Spawn(PoolerTarget.NONE, m_bulletPoolKey, m_shootingPoint.position, Quaternion.identity);
        if (bulletClone == null) return;
        bulletClone.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        var projectComp = bulletClone.GetComponent<Projectile>();
        if(projectComp != null)
        {
            projectComp.Damage = statData.damage;
        }

        OnShoot?.Invoke();
    }

    private void ReduceFirerate()
    {
        if(!m_isShootted) return;
        m_curFR -= Time.deltaTime;

        if (m_curFR > 0) return;
        LoadStats();
        m_isShootted = false;
    }
}
