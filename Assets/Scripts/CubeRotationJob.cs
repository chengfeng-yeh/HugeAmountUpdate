using UnityEngine;
using UnityEngine.Jobs;

public struct CubeRotationJob : IJobParallelForTransform
{
    public float m_DeltaTime;

    public void Execute(int p_index, TransformAccess p_trans)
    {
        p_trans.rotation = p_trans.rotation * Quaternion.AngleAxis(m_DeltaTime * 30f, Vector3.up);
    }
}

