using TMPro;
using UDEV.SPM;
using UnityEngine;

public class BallVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_hpCountingTxt;
    [PoolerKeys(target = PoolerTarget.NONE)]
    [SerializeField] private string m_gotHitVfxPoolKey;
    [PoolerKeys(target = PoolerTarget.NONE)]
    [SerializeField] private string m_deadVfxPoolKey;
    [PoolerKeys(target = PoolerTarget.NONE)]
    [SerializeField] private string m_groundHittedVfxPoolKey;

    [SerializeField] private AudioClip m_groundHittedSound;
    [SerializeField] private AudioClip m_deadSound;

    private BallController m_ballCtr;

    private void Awake()
    {
        m_ballCtr = GetComponent<BallController>();
    }

    public void OnInit()
    {
        UpdateHpCounting();
    }
    
    public void OnTakeDamage(Vector2 getHitPos)
    {
        if(m_ballCtr == null) return;
        var gotHitVfxClone = PoolersManager.Ins.Spawn(PoolerTarget.NONE, m_gotHitVfxPoolKey, getHitPos, Quaternion.identity);
        SetParticleColor(gotHitVfxClone);
        UpdateHpCounting();
    }

    private void UpdateHpCounting()
    {
        if (m_hpCountingTxt != null) m_hpCountingTxt.text = m_ballCtr.CurHealth.ToString();
    }

    public void OnDead(Vector3 pos) { 
        var deadVfxClone = PoolersManager.Ins.Spawn(PoolerTarget.NONE, m_deadVfxPoolKey, pos, Quaternion.identity);
        SetParticleColor(deadVfxClone);

        CineController.Ins.ShakeTrigger();
        float randomPitch = Random.Range(0.8f, 1.15f);
        AudioController.Ins.PlaySound(m_deadSound, randomPitch);
    }

    public void OnHitGround(Vector2 pos)
    {
        PoolersManager.Ins.Spawn(PoolerTarget.NONE, m_groundHittedVfxPoolKey, pos, Quaternion.identity);
        float randomPitch = Random.Range(0.7f, 1.15f);
        AudioController.Ins.PlaySound(m_groundHittedSound, randomPitch);
    }

    private void SetParticleColor(GameObject particle)
    {
        if(particle == null) return;
        var particleVfx = particle.GetComponent<ParticleVfx>();
        particleVfx?.SetColor(m_ballCtr.CurColor);
    }
}
