using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;

public enum ROTATE_METHOD
{
    ROTATE_SELF,
    ROTATE_BY_SINGLE_UPDATE,
    ROTATE_BY_JOB,
}

public class CubeGenerator : MonoBehaviour
{
    [SerializeField]
    private CubeRotateItsSelf m_cubeRotateItsselfTemplate;
    [SerializeField]
    private CubeGroupUpdate m_cubeGroupUpdateTemplate;
    [SerializeField]
    private float m_initX;
    [SerializeField]
    private float m_initY;
    [SerializeField]
    private float m_gapX;
    [SerializeField]
    private float m_gapY;
    [SerializeField]
    private int m_rowNum;
    [SerializeField]
    private int m_columnNum;
    [SerializeField]
    private ROTATE_METHOD m_rotateMethod = ROTATE_METHOD.ROTATE_SELF;

    private CubeGroupUpdate[] m_groupCubes;
    private Transform[] m_forJobCubes;
    private TransformAccessArray m_transformsAccessArray;
    private CubeRotationJob m_cubeRotationJob;
    private JobHandle m_cubeRotationHandle;
    private bool m_allCubeGenerated = false;

    private Coroutine m_generateCubeCoroutine = null;
    private void Start()
    {
        //generate some cubes.
        switch (m_rotateMethod)
        {
            case ROTATE_METHOD.ROTATE_SELF:
                m_generateCubeCoroutine = StartCoroutine(GenerateCubes());
                break;
            case ROTATE_METHOD.ROTATE_BY_SINGLE_UPDATE:
                m_generateCubeCoroutine = StartCoroutine(GenerateGroupCubes());
                break;
            case ROTATE_METHOD.ROTATE_BY_JOB:
                m_generateCubeCoroutine = StartCoroutine(GenerateForJobCubes());
                break;
        }        
    }
    
    private void Update()
    {
        if (ROTATE_METHOD.ROTATE_BY_SINGLE_UPDATE == m_rotateMethod)
        {
            if (null != m_groupCubes && m_allCubeGenerated)
            {
                float tempDeltaTime = Time.deltaTime;
                for (int i = 0; i < m_rowNum; ++i)
                {
                    for (int j = 0; j < m_columnNum; j++)
                    {
                        m_groupCubes[i * m_columnNum + j].MyRotateUpdate(tempDeltaTime);
                    }
                }
            }
        }
        else if (ROTATE_METHOD.ROTATE_BY_JOB == m_rotateMethod)
        {
            if (null != m_forJobCubes && m_allCubeGenerated)
            {
                float tempDeltaTime = Time.deltaTime;

                m_cubeRotationJob = new CubeRotationJob();
                m_cubeRotationJob.m_DeltaTime = tempDeltaTime;
                m_cubeRotationHandle = m_cubeRotationJob.Schedule(m_transformsAccessArray);
            }
        }        
    }

    private void LateUpdate()
    {
        if (ROTATE_METHOD.ROTATE_BY_JOB == m_rotateMethod)
        {
            if (null != m_forJobCubes && m_allCubeGenerated)
            {
                m_cubeRotationHandle.Complete();
            }
        }
    }

    private IEnumerator GenerateCubes()
    {
        for(int i=0;i<m_rowNum;++i)
        {
            for(int j=0;j<m_columnNum;j++)
            {
                GameObject tempGo = GameObject.Instantiate(m_cubeRotateItsselfTemplate.gameObject, new Vector3(m_initX + m_gapX * j, m_initY + m_gapY * i, 0), Quaternion.identity, null);
            }
            yield return null;
        }
        yield break;            
    }

    private IEnumerator GenerateGroupCubes()
    {
        m_groupCubes = new CubeGroupUpdate[m_rowNum * m_columnNum];
        for (int i = 0; i < m_rowNum; ++i)
        {
            for (int j = 0; j < m_columnNum; j++)
            {
                GameObject tempGo = GameObject.Instantiate(m_cubeGroupUpdateTemplate.gameObject, new Vector3(m_initX + m_gapX * j, m_initY + m_gapY * i, 0), Quaternion.identity, null);
                m_groupCubes[i * m_columnNum + j] = tempGo.GetComponent<CubeGroupUpdate>();
            }
            yield return null;
        }
        m_allCubeGenerated = true;
        yield break;
    }

    private IEnumerator GenerateForJobCubes()
    {
        m_forJobCubes = new Transform[m_rowNum * m_columnNum];
        for (int i = 0; i < m_rowNum; ++i)
        {
            for (int j = 0; j < m_columnNum; j++)
            {
                GameObject tempGo = GameObject.Instantiate(m_cubeGroupUpdateTemplate.gameObject, new Vector3(m_initX + m_gapX * j, m_initY + m_gapY * i, 0), Quaternion.identity, null);
                m_forJobCubes[i * m_columnNum + j] = tempGo.transform;
            }
            yield return null;
        }
        m_transformsAccessArray = new TransformAccessArray(m_forJobCubes);
        m_allCubeGenerated = true;
        yield break;
    }

    private void OnDestroy()
    {
        m_transformsAccessArray.Dispose();
        if (null != m_generateCubeCoroutine)
        {
            StopCoroutine(m_generateCubeCoroutine);
            m_generateCubeCoroutine = null;
        }
    }
}
