using UnityEngine;
using UnityEngine.Events;

public class BoltsManager : MonoBehaviour
{
    public UnityEvent BoltsDone;
    [HideInInspector]public bool bolt1;
    [HideInInspector]public bool bolt2;
    [HideInInspector]public bool bolt3;
    [SerializeField] int totalBolts;
    int boltsDone;
    bool isDone;
    private void OnEnable()
    {
        if (TickManager.Instance != null)
        {
            TickManager.Instance.OnFastTick += FastTick;
        }
    }
    private void OnDisable()
    {
        if (TickManager.Instance != null)
        {
            TickManager.Instance.OnFastTick -= FastTick;
        }
        CancelInvoke();
    }
    private void FastTick()
    {
        if (isDone)
        {
            BoltsDone.Invoke();
            Debug.Log("Everything ready " + boltsDone);
            isDone = false;
        }
        if (boltsDone == totalBolts)
        {
            isDone = true;
        }
    }
    //private void Update()
    //{
        
    //    if (isDone)
    //    {
    //        BoltsDone.Invoke();
    //        Debug.Log("Everything ready "+boltsDone);
    //        isDone = false;
    //    }
    //    if (boltsDone==totalBolts)
    //    {
    //        isDone = true;
    //    }
    //}
    public void AddBolt()
    {
        boltsDone++;
    }
    public void CheckBool(int i)
    {
        switch(i)
        {
            case 1:
                bolt1 = true;
                break;
            case 2:
                bolt2 = true;
                break;
            case 3:
                bolt3 = true;
                break;
            default:
                break;
        }
        
    }
}
