using DG.Tweening;
using UnityEngine;

public class LineDeVida : MonoBehaviour
{
    [Header("Line")]
    public LineRenderer rope;
    public Transform[] linePos;
    public Vector3[] vectorPos;
    Transform lastPos;
    PlayerRope ropeOBJ;

    Tween objMovement;

    void Update()
    {
        RenderRopeByPoints(rope, linePos);
    }
    public void RenderRopeByPoints(LineRenderer line, Transform[] posList)
    {
        rope.positionCount = posList.Length;
        vectorPos = new Vector3[rope.positionCount];
        for (int i = 0; i < posList.Length; i++)
        {
            vectorPos[i] = posList[i].position;
        }
        line.SetPositions(vectorPos);
    }
    //public void HookPlayer()
    //{
    //    rope.enabled = true;
    //    lastPos = linePos[1];
    //    linePos[1]=player;
    //    //ropeOBJ.StartUsing();
    //}
    //public void UnhookPlayer()
    //{
    //    rope.enabled = false;
    //    linePos[1] = lastPos;
    //    ropeOBJ.QuitUsing();
    //}
}