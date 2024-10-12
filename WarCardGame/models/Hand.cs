namespace WarCardGame.models;

public class Hand
{
    public Queue<Card> Cards { get; set; }
    
    public Hand()
    {
        Cards = new Queue<Card>(); 
    }
    //implement later
    public void WonRound()
    {
        
    }
}