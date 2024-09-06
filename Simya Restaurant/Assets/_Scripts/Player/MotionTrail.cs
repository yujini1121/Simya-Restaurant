using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionTrail : MonoBehaviour
{
    [System.Serializable]
    public class MeshInfo
    {
        public GameObject   motionObj;
        public Mesh         mesh;
    }

    [SerializeField] private SkinnedMeshRenderer SMR;
    private Transform trailContainerTrans;
    private List<MeshInfo>      meshInfos = new List<MeshInfo>();
    private List<Vector3>       posMemory = new List<Vector3>();
    private List<Quaternion>    rotMemory = new List<Quaternion>();
    private int curMemBeginIndex = 0;
    private WaitForSeconds drawCycleT;

    [Header("Trail Info")]
    [SerializeField][Range(0, 30)] private int trailCount;
    [SerializeField] private float drawCycle = 0.2f;
    [SerializeField] private Material motionTrailMat;
    [SerializeField][ColorUsage(true, true)] private Color frontColor;
    [SerializeField][ColorUsage(true, true)] private Color backColor;


    void Start()
    {
        trailContainerTrans = new GameObject("TrailContainer").transform;

        for (int i = 0; i < trailCount; i++)
        {
            MeshInfo mf = new MeshInfo();

            #region SkinnedMeshRenderer에 분할된 Material 병합
            Mesh bakedMesh = new Mesh();
            SMR.BakeMesh(bakedMesh);

            Mesh combinedMesh = new Mesh();
            CombineInstance[] combine = new CombineInstance[SMR.sharedMesh.subMeshCount];

            for (int j = 0; j < SMR.sharedMesh.subMeshCount; j++)
            {
                combine[j].mesh = bakedMesh;
                combine[j].subMeshIndex = j;
            }
            combinedMesh.CombineMeshes(combine, true, false);
            #endregion

            mf.motionObj = new GameObject("MotionObject");
            mf.motionObj.transform.parent = trailContainerTrans;
            mf.motionObj.transform.localPosition = Vector3.zero;

            MeshRenderer meshRenderer = mf.motionObj.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material(motionTrailMat);

            mf.motionObj.AddComponent<MeshFilter>().mesh = combinedMesh;

            mf.mesh = new Mesh();

            meshInfos.Add(mf);

            #region MotionTrail 색상 설정
            float alphaVal = (1f - (float)i / trailCount) * 0.5f;
            Color color = Color.Lerp(frontColor, backColor, (float)i / trailCount);
            color.a = alphaVal;
            meshRenderer.material.color = color;
            #endregion
        }

        drawCycleT = new WaitForSeconds(drawCycle);
        StartCoroutine(BakeMeshCoroutine());
    }

    private IEnumerator BakeMeshCoroutine()
    {
        while (true)
        {
            for (int i = meshInfos.Count - 2; i >= 0; i--)
            {
                meshInfos[i + 1].mesh.vertices = meshInfos[i].mesh.vertices;
                meshInfos[i + 1].mesh.triangles = meshInfos[i].mesh.triangles;
            }

            SMR.BakeMesh(meshInfos[0].mesh);

            /*
            posMemory.Insert(0, transform.position);
            rotMemory.Insert(0, transform.rotation);

            if (posMemory.Count > trailCount)
                posMemory.RemoveAt(posMemory.Count - 1);
            if (rotMemory.Count > trailCount)
                rotMemory.RemoveAt(rotMemory.Count - 1);
            */

            if (posMemory.Count < trailCount && rotMemory.Count < trailCount)
            {
                posMemory.Insert(0, transform.position);
                rotMemory.Insert(0, transform.rotation);
            }
            else
            {
                posMemory[(posMemory.Count + curMemBeginIndex) % posMemory.Count] = transform.position;
                rotMemory[(rotMemory.Count + curMemBeginIndex) % posMemory.Count] = transform.rotation;
            }

            curMemBeginIndex = (curMemBeginIndex + 1) % posMemory.Count;

            for (int i = 0; i < meshInfos.Count; i++)
            {
                /*
                meshInfos[i].motionObj.transform.position = posMemory[Mathf.Min(i, posMemory.Count - 1)];
                meshInfos[i].motionObj.transform.rotation = rotMemory[Mathf.Min(i, rotMemory.Count - 1)];
                */

                meshInfos[i].motionObj.transform.position = posMemory[(i + curMemBeginIndex) % posMemory.Count];
                meshInfos[i].motionObj.transform.rotation = rotMemory[(i + curMemBeginIndex) % rotMemory.Count];
            }

            yield return drawCycleT;
        }
    }
}
