using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ViewPortUtil
{
    private static float m_minX;
    private static float m_maxX;
    private static float m_minY;
    private static float m_maxY;

    public static float MinX { get => m_minX;}
    public static float MaxX { get => m_maxX;}
    public static float MinY { get => m_minY;}
    public static float MaxY { get => m_maxY;}

    public static void GetWorldPos()
    {
        m_minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        m_maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        m_minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        m_maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
    }
}
