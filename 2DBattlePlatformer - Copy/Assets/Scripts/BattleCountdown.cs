using UnityEngine;

public class BattleCountdown : MonoBehaviour
{
    void Start()
    {
        GetComponentInChildren<Animator>().Play("CountdownAnim");
    }
}
