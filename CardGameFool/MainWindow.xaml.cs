using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardGameFool;
    

    

public partial class MainWindow : Window, INotifyPropertyChanged {
    
    
    const int initialCardsCount = 6;
    static Koloda koloda = new Koloda();// Создаем колоду
    static bool gameOver = false;
    static Player player1 = new Player("Игрок");
    static Player comp = new Player("Компьютер");
    static Card trumpCard = koloda.GetTrumpCard();// Определяем козырь
    private static Card Choose;
    public static string MainInformation { get; set; }

    
    public static ObservableCollection<Button> Player1_GameButtonsCards { get;  } = new();
    //public List<Button> Cards => game_cards;
    public ObservableCollection<Button> ButtonTrump_card { get;  } = new();
    public MainWindow() {
        InitializeComponent();
        DataContext = this;
        //AddPlayerViewButtons();
        SetButtonTrumpCard();
        /*foreach (var card in koloda.cards) {
            player1.AllCardButtons.Add(player1.GetBtn($"{card.Rank}-{card.Mast}",
                $"pack://Application:,,,/Images/cards/{card.Rank}_{card.Mast}.png"));
        }*/
        
        //словарик имя_карты-готовая_кнопка
        /*for (var i = 0; i < koloda.cards.Count; i++) {
            player1.CardButtonsInHand.Add(koloda.GetCardFromKoloda());
            player1.CardButtonsInHand.Add($"{koloda.cards[i].Rank}-{koloda.cards[i].Mast}", player1.AllCardButtons[i]);
        }*/
        Game();
    }

    void Game() {
        for (int i = 0; i < initialCardsCount; i++) {//раздаем карты
            player1.AddCard(koloda.GetCardFromKoloda());
            Player1_GameButtonsCards.Add(player1.CardButtonsInHand[i]);
            comp.AddCard(koloda.GetCardFromKoloda());
        }
        int modeHit = 1;//определение приоритета ходов
        while (!gameOver) {
            /*Console.Clear();
            Console.WriteLine($"Козырь: {trumpSuit}");
            Console.WriteLine($"Остаток в колоде: {koloda.Count}");
            Console.WriteLine("\n------------------------------");
            Console.WriteLine($"{player1.Name} vs {comp.Name}");*/
            //Console.WriteLine($"{player1.Name} карты:");
            //player1.ShowHand();

            if (modeHit == 1) {
                modeHit = HitPlayer();
            }
            else {
                modeHit = HitComp();
            }
            /*
            for (int i = 0; i < 5; i++) {
                Console.Write(". ");
                System.Threading.Thread.Sleep(500);
            }
        */
        }
        MessageBox.Show("Игра завершена. Спасибо за игру!");
    }
    
    /*void AddPlayerViewButtons() {
        for (var i = 0; i < initialCardsCount; i++) {
            Player1_GameButtonsCards.Add(player1.CardButtonsInHand[i]);
        }

    }*/
    void SetButtonTrumpCard() {
        //var cardTrump = trumpCard;
        var buttonTrump=new Button();
        buttonTrump.Background =
            new ImageBrush(new BitmapImage(new Uri($"pack://Application:,,,/Images/cards/{trumpCard.Mast}.png")));
        ButtonTrump_card.Add(buttonTrump);
        //trumpCard=cardTrump;
    }
    private void Card_OnClick(object sender, RoutedEventArgs e) {
        var btn = sender as Button;
        Choose=new Card(btn.Content.ToString()., btn.Content.ToString());
        throw new NotImplementedException();
    }
    
    public static int HitPlayer(Card attackCard) {
            // --------------Ход игрока1--------------
            MainInformation = "Ваш ход";
            /*
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > player1.CardsInHand.Count) {
                MainInformation="Некорректный ввод. Попробуйте снова:";
            }
            */
            
            //Card attackCard = player1.CardsInHand[choice - 1];
            
            MainInformation=$"Вы выбрали: {attackCard.Rank} - {attackCard.Mast}";

            // ---------компьютер защищается--------------

            //первый массив с подходящими мастями и старшими званиями
            var defendingCards = comp.CardsInHand.Where(c => c.Mast == attackCard.Mast && c.Value > attackCard.Value).ToList();
            //второй массив с козырями
            List<Card> trampCards = null;
            if (defendingCards.Count != 0) {
                if (defendingCards.First().Mast != trumpCard.Mast) {
                    trampCards = comp.CardsInHand.Where(c => c.Mast == trumpCard.Mast).ToList();
                }
                defendingCards = defendingCards.OrderBy(card => card.Value).ToList();//сортировка подходящих карт по рангу;
                Card defendCard = defendingCards[0];
                Console.WriteLine($"{comp.Name} защищается картой: {defendCard}");
                comp.RemoveCard(defendCard);

                // Удаляем карту атаки из руки игрока
                player1.RemoveCard(attackCard);

                // Пополнение рук до 6 карт
                for (int i = 0; player1.CardsInHand.Count < initialCardsCount && koloda.Count > 0; i++) {
                    player1.AddCard(koloda.GetCardFromKoloda());
                    Player1_GameButtonsCards.Add(player1.CardButtonsInHand[i]);
                }
                /*
                while (player1.CardsInHand.Count < initialCardsCount && koloda.Count > 0) {
                    player1.AddCard(koloda.GetCardFromKoloda());
                    Player1_GameButtonsCards.Add(player1.CardButtonsInHand[i]);
                }
                */
                while (comp.CardsInHand.Count < initialCardsCount && koloda.Count > 0)
                    comp.AddCard(koloda.GetCardFromKoloda());

                // Проверка победы
                if (!player1.CardsInHand.Any()) {
                    MainInformation="Вы выиграли!";
                    gameOver = true;
                }
                if (!comp.CardsInHand.Any()) {
                    MainInformation="Компьютер выиграл!";
                    gameOver = true;
                }
                return 2;
            }
            //если нет обычных карт ходим козырями
            else if (trampCards != null) {
                trampCards = trampCards.OrderBy(card => card.Value).ToList();//сортировка козырей по рангу;
                Card trampCard = trampCards[0];
                Console.WriteLine($"{comp.Name} защищается картой: {trampCard}");
                comp.RemoveCard(trampCard);

                // Удаляем карту атаки из руки игрока
                player1.RemoveCard(attackCard);

                // Пополнение рук до 6 карт
                for (int i = 0; player1.CardsInHand.Count < initialCardsCount && koloda.Count > 0; i++) {
                    player1.AddCard(koloda.GetCardFromKoloda());
                    Player1_GameButtonsCards.Add(player1.CardButtonsInHand[i]);
                }
                /*
                while (player1.CardsInHand.Count < initialCardsCount && koloda.Count > 0) {
                    player1.AddCard(koloda.GetCardFromKoloda());
                    Player1_GameButtonsCards.Add(player1.CardButtonsInHand[i]);
                }
                */
                while (comp.CardsInHand.Count < initialCardsCount && koloda.Count > 0)
                    comp.AddCard(koloda.GetCardFromKoloda());

                // Проверка победы
                if (!player1.CardsInHand.Any()) {
                    Console.WriteLine("Вы выиграли!");
                    gameOver = true;
                }
                if (!comp.CardsInHand.Any()) {
                    Console.WriteLine("Компьютер выиграл!");
                    gameOver = true;
                }
                return 2;
            }
            //если нечем отбиваться принимаем карту
            else {
                Console.WriteLine($"{comp.Name} не может защититься и берет карту.");
                comp.AddCard(attackCard);
                player1.RemoveCard(attackCard);

                // Пополнение рук до 6 карт
                for (int i = 0; player1.CardsInHand.Count < initialCardsCount && koloda.Count > 0; i++) {
                    player1.AddCard(koloda.GetCardFromKoloda());
                    Player1_GameButtonsCards.Add(player1.CardButtonsInHand[i]);
                }
                /*
                while (player1.CardsInHand.Count < initialCardsCount && koloda.Count > 0) {
                    player1.AddCard(koloda.GetCardFromKoloda());
                    Player1_GameButtonsCards.Add(player1.CardButtonsInHand[i]);
                }
                */
                while (comp.CardsInHand.Count < initialCardsCount && koloda.Count > 0)
                    comp.AddCard(koloda.GetCardFromKoloda());
                
                // Проверка победы
                if (!player1.CardsInHand.Any()) {
                    Console.WriteLine("Вы выиграли!");
                    gameOver = true;
                }
                if (!comp.CardsInHand.Any()) {
                    Console.WriteLine("Компьютер выиграл!");
                    gameOver = true;
                }
                return 1;
            }
    }
    public static int HitComp() {
        // --------------Ход компьютера--------------
        Console.WriteLine("\nХод компьютера");
        comp.CardsInHand = comp.CardsInHand.OrderBy(card => card.Value).ToList();//сортировка
        Card attackCard = null;
        for (int i = 0; i < comp.CardsInHand.Count; i++) {
            if (comp.CardsInHand[i].Mast != trumpCard.Mast) {//идем обычной мастью
                attackCard = comp.CardsInHand[i];
                comp.RemoveCard(attackCard);
                break;
            }
        }
        if (attackCard == null) {
            for (int i = 0; i < comp.CardsInHand.Count; i++) {
                if (comp.CardsInHand[i].Mast == trumpCard.Mast) {//если все козыри идем первым минимальным
                    attackCard = comp.CardsInHand[i];
                    comp.RemoveCard(attackCard);
                    break;
                }
            }
        }
        Console.WriteLine($"Карта компьютера: {attackCard}");

        // ---------игрок1 защищается--------------
        Console.WriteLine("\nВаш ответный ход. Введите номер карты для защиты или 0 для принятия карты:");
        int choice;
        while (true) {
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > player1.CardsInHand.Count) {
                Console.WriteLine("Некорректный ввод. Попробуйте снова: ");
            }
            if (choice == 0) {
                player1.CardsInHand.Add(attackCard);
                Console.WriteLine($"Вы берете карту.");
                // Пополнение рук до 6 карт
                while (comp.CardsInHand.Count < initialCardsCount && koloda.Count > 0)
                    comp.AddCard(koloda.GetCardFromKoloda());
                    
                    
                // Проверка победы
                if (!player1.CardsInHand.Any()) {
                    Console.WriteLine("Вы выиграли!");
                    gameOver = true;
                }
                if (!comp.CardsInHand.Any()) {
                    Console.WriteLine("Компьютер выиграл!");
                    gameOver = true;
                }
                return 2;
            }
            else {
                Card defendCard = player1.CardsInHand[choice - 1];
                Console.WriteLine($"Вы выбрали: {defendCard}");
                //проверка если защитная старше по масти или выбран козырь против некозыря
                if ((defendCard.Mast == attackCard.Mast && defendCard.Value > attackCard.Value) || (defendCard.Mast == trumpCard.Mast && attackCard.Mast != trumpCard.Mast)) {
                    player1.RemoveCard(defendCard);
                    Console.WriteLine($"Вы защищаетесь картой: {defendCard}");
                    break;
                }
                else {
                    Console.WriteLine("Данная карта не подходит. Выберете другую: ");
                }
            }
        }
        // Пополнение рук до 6 карт
        for (int i = 0; player1.CardsInHand.Count < initialCardsCount && koloda.Count > 0; i++) {
            player1.AddCard(koloda.GetCardFromKoloda());
            Player1_GameButtonsCards.Add(player1.CardButtonsInHand[i]);
        }
        /*
        while (player1.CardsInHand.Count < initialCardsCount && koloda.Count > 0) {
            player1.AddCard(koloda.GetCardFromKoloda());
            Player1_GameButtonsCards.Add(player1.CardButtonsInHand[i]);
        }
        */
        while (comp.CardsInHand.Count < initialCardsCount && koloda.Count > 0)
            comp.AddCard(koloda.GetCardFromKoloda());
        // Проверка победы
        if (!player1.CardsInHand.Any()) {
            Console.WriteLine("Вы выиграли!");
            gameOver = true;
        }
        if (!comp.CardsInHand.Any()) {
            Console.WriteLine("Компьютер выиграл!");
            gameOver = true;
        }
        return 1;
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    
}