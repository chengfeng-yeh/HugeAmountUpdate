using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGroupUpdate : MonoBehaviour
{
    private float m_rotateSpeed = 30;
    private Vector3 m_rotateAxis;
    private float m_initDegree;
    private Transform m_cachedTrans;

    private void Awake()
    {
        m_rotateAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        m_initDegree = Random.Range(-180f, 180f);
        m_cachedTrans = transform;
        m_cachedTrans.Rotate(m_rotateAxis, m_initDegree);
    }

    public void MyRotateUpdate(float p_deltaTime)
    {
        m_cachedTrans.Rotate(m_rotateAxis, p_deltaTime * m_rotateSpeed);
    }
}
