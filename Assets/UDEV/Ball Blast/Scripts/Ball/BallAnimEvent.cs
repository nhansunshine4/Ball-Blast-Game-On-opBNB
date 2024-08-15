using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAnimEvent : MonoBehaviour
{
    private BallController m_ballCtr;

    private void Awake()
    {
        m_ballCtr = GetComponentInParent<BallController>();
    }

    public void BackToIdle()
    {
        m_ballCtr?.BackToIdle();
    }
}
