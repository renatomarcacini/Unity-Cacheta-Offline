public class HumanPlayStrategy : IPlayStrategy
{
    public void ChooseMovement(PlayerBase player)
    {
        
    }

    public void Play(PlayerBase player)
    {
        GameManager.Instance.NextTurn();
    }
}
