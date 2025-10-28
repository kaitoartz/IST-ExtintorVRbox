using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class WrenchTowerSupport : MonoBehaviour
{
    public float playerYPOS;

    [Header("References")]
    public Transform attachSocketTransform;
    public Transform player;

    [Header("Limits and Offset")]
    public int maxLimit;
    int minLimit = 0;
    [Range (-3, 3)] public float yOffset;

    [Header("Line")]
    public LineRenderer rope;
    public Transform[] linePos;
    public Vector3[] vectorPos;

    Tween objMovement;

    private void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }
    //Evaluar si requiere de update o configurar very fast tick
    void Update()
    {
        RenderRopeByPoints(rope, linePos);
        playerYPOS = player.position.y;
        objMovement = attachSocketTransform.DOLocalMoveY(playerYPOS + yOffset, 1f, false);
        if (Mathf.Round(player.position.y) <= minLimit || Mathf.Round(player.position.y) >= maxLimit) return;
        else
        {
            objMovement.Play();
        }
        
    }
    public void PullCloseWrenchBucket(float dis)
    {
        attachSocketTransform.DOLocalMoveX(dis, 3f, false);
    }
    public void RenderRopeByPoints(LineRenderer line, Transform[] posList)
    {
        rope.positionCount = posList.Length;
        vectorPos = new Vector3[rope.positionCount];
        for(int i = 0; i < posList.Length; i++)
        {
            vectorPos[i] = posList[i].position;
        }
        line.SetPositions(vectorPos);
    }
}
