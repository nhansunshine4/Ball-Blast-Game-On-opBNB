using UnityEngine;

public class BarrelVisual : MonoBehaviour
{
    [SerializeField] private AudioClip m_shootSound;

    public void OnShoot()
    {
        float randomPitch = Random.Range(0.9f, 1.1f);
        AudioController.Ins.PlaySound(m_shootSound);
    }
}
