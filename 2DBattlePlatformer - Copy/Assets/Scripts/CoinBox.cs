using UnityEngine;

public class CoinBox : HittableFromBelow
{
    [SerializeField] int _coinsInBox = 3;

    int _remainingCoinsInBox;

    protected override bool CanUse => _remainingCoinsInBox > 0;

    private void Start()
    {
        _remainingCoinsInBox = _coinsInBox;
    }

    protected override void Use(int playerNumber)
    {
        // if (_remainingCoinsInBox > 0)
        _remainingCoinsInBox--;
        Coin.CoinsCollected++;

        ScoreSystem.Add(10);

        CombatScoreSystem.UpdatePoints(1, playerNumber);

        GetComponent<AudioSource>().Play();
    }
}
