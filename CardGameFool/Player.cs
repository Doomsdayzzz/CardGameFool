using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CardGameFool;

public class Player{
    
        public string Name { get; set; }
        public List<Card> CardsInHand { get; set; }
        //public Dictionary<string, Button> CardButtonsInHand;
        //public List<Button> CardButtonsInHand;
        //public List<Button> AllCardButtons;
        
        public Player(string name) {
            Name = name;
            CardsInHand = new List<Card>();
            //CardButtonsInHand = new Dictionary<string, Button>();
            //CardButtonsInHand =new ();
            //AllCardButtons = new List<Button>();
            //массив кнопок
            
        }

        public void AddCard(Card card) {
            CardsInHand.Add(card);
            
        }

        public void RemoveCard(Card card) {
            CardsInHand.Remove(card);
        }
        
        /*
        public Button GetBtn(string name, string imageName) {
            Button button = new Button();
            button.Content = name;
            button.Background = new ImageBrush(new BitmapImage(new Uri(imageName)));
            return button;
        }
        */
        
        /*
        public Button GetButtonFromKoloda() {
            if (CardButtonsInHand.Count == 0) return null;
            var button = CardButtonsInHand.First();
            CardButtonsInHand.Remove(CardButtonsInHand.First());
            return button;
        }
        */

        /*public void ShowHand() {//вывод карт на консоль
            CardsInHand = CardsInHand.OrderBy(card => card.Value).ToList();//сортировка
            Console.WriteLine($"Карты на руках:");
            for (int i = 0; i < CardsInHand.Count; i++) {
                Console.WriteLine($"{i + 1}. {CardsInHand[i]}");
            }
        }*/
   
}