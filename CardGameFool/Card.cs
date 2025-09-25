namespace CardGameFool;

public partial class Card{
    public string Mast { get; set; } //масть
    public string Rank { get; set; } //звание
    public int Value { get; set; } // Для сравнения карт

    public Card(string mast, string rank, int value) {
        Mast = mast;
        Rank = rank;
        Value = value;
    }
    /*public override string ToString() {
        return $"{Rank} - {Mast}";
    }*/
}