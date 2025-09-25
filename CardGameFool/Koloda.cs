using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CardGameFool;

public partial class Koloda{
    public List<Card> cards;
    
    

    private static string[]
        Mast = { "Hearts", "Diamonds", "Clovers", "Spades" }; //"♥-Hearts", "♦-Diamonds", "♣-Clovers", "♠-Spades"

    //массив кортежей, чтобы легче сравнивать карты по значению Value
    private static (string rank, int value)[] ranks =
        { ("6", 6), ("7", 7), ("8", 8), ("9", 9), ("10", 10), ("В", 11), ("Д", 12), ("К", 13), ("Т", 14) };

    public Koloda() {
        cards = new List<Card>();
        
        //массив карт
        foreach (var mast in Mast) {
            foreach (var (rank, value) in ranks) {
                cards.Add(new Card(mast, rank, value));
            }
        }
        Shuffle();
        

     
    }

    

    public void Shuffle() {
        Random rnd = new Random();
        //тасование карт
        int lastIndex = cards.Count - 1;
        while (lastIndex > 0) {
            Card tmpCard = cards[lastIndex];
            int rndIndex = rnd.Next(0, lastIndex);
            cards[lastIndex] = cards[rndIndex];
            cards[rndIndex] = tmpCard;
            lastIndex--;
        }
    }
    public Card GetCardFromKoloda() {
        if (cards.Count == 0) return null;
        var card = cards[0];
        cards.RemoveAt(0);
        return card;
    }
    

    public int Count => cards.Count;

    public Card GetTrumpCard() {
        //козырь
        Random rnd = new Random();
        return cards[rnd.Next(0, cards.Count)];
    }
}