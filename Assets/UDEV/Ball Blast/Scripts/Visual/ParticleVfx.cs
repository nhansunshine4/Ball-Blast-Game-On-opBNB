using UnityEngine;

public class ParticleVfx : MonoBehaviour
{
    private ParticleSystem m_particleSystem;

    private void Awake()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
    }

    public void SetColor(Color color)
    {
        if (m_particleSystem == null) return;

        var main = m_particleSystem.main;
        main.startColor = color;
    }
}
