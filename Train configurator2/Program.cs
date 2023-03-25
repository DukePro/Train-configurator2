namespace TrainConfigurator
{
    class Programm
    {
        static void Main()
        {
            Menu menu = new Menu();
            menu.ShowMenu();
        }
    }

    class Menu
    {
        Terminal terminal = new Terminal();

        public void ShowMenu()
        {
            const string MenuCreateDirection = "1";
            const string MenuSellTickets = "2";
            const string MenuCompileTrain = "3";
            const string MenuShowTrainParam = "4";
            const string MenuSendTrain = "5";
            const string MenuExit = "0";

            bool isExit = false;

            string userInput;

            while (isExit == false)
            {
                ShowStatus();

                Console.WriteLine("\nМеню:");
                Console.WriteLine(MenuCreateDirection + " - Создать направление");
                Console.WriteLine(MenuSellTickets + " - Продать билеты");
                Console.WriteLine(MenuCompileTrain + " - Сформировать состав");
                Console.WriteLine(MenuShowTrainParam + " - Показать параметры состава");
                Console.WriteLine(MenuSendTrain + " - Отправить поезд");
                Console.WriteLine(MenuExit + " - Выход");

                userInput = Console.ReadLine();
                CleanConsoleBelowLine();

                switch (userInput)
                {
                    case MenuCreateDirection:
                        terminal.CreateDirection();
                        break;

                    case MenuSellTickets:
                        terminal.SellTickets();
                        break;

                    case MenuCompileTrain:
                        terminal.CompileTrain();
                        break;

                    case MenuShowTrainParam:
                        //_config.ShowTrainParameters();
                        break;

                    case MenuSendTrain:
                        //SendTrain();
                        break;

                    case MenuExit:
                        isExit = true;
                        break;
                }
            }
        }
         
        private void ShowStatus()
        {
            int infoPositionY = 0;
            int infoPositionX = 0;
            string readyToGo;
            string trainStatus;

            if (terminal.IsTrainFormed == true)
            {
                readyToGo = "ГОТОВ";
                trainStatus = "Сформирован";
            }
            else
            {
                readyToGo = "НЕ ГОТОВ";
                trainStatus = "НЕ сформирован";
            }

            CleanConsoleString();
            Console.SetCursorPosition(infoPositionX, infoPositionY);
            Console.WriteLine($"Направление: {terminal.Direction} | Билетов продано: {terminal.Tickets} | Статус поезда: {trainStatus} | К отправке: {readyToGo}");
        }

        private void CleanConsoleString()
        {
            Console.CursorLeft = 0;

            int currentLineCursor = Console.CursorTop;

            Console.Write(new string(' ', Console.WindowWidth));
            Console.CursorTop = currentLineCursor;
            Console.SetCursorPosition(0, currentLineCursor);
        }

        private void CleanConsoleBelowLine()
        {
            int currentLineCursor = Console.CursorTop;

            for (int i = currentLineCursor; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, currentLineCursor);
        }
    }

    class Terminal
    {
        public string Direction { get; private set; } = "Не задано";
        public int Tickets { get; private set; } = 0;
        public bool IsTrainFormed { get; private set; } = false;

        private List<Wagon> _train = new List<Wagon>();

        private Random _rand = new Random();
        private Wagon _wagon = new Wagon();

        

        public void CreateDirection()
        {
            string from;
            string to;

            if (Tickets == 0)
            {
                Console.WriteLine("Введите пункт отправления");
                from = Console.ReadLine();

                Console.WriteLine("Введите пункт прибытия");
                to = Console.ReadLine();

                Direction = $"{from} - {to}";
            }
            else
            {
                Console.WriteLine("Нельзя изменить направление, на которое уже проданы билеты.");
            }
        }

        public void SellTickets()
        {
            if (Direction != "Не задано")
            {
                if (Tickets == 0)
                {
                    Tickets = _rand.Next(200, 648);

                    Console.WriteLine("Продано билетов - " + Tickets);
                }
                else
                {
                    Console.WriteLine("Билеты уже проданы");
                }
            }
            else
            {
                Console.WriteLine("Сначала нужно создать направление");
            }
        }

        public void CompileTrain()
        {
            Wagon[] wagons = new Wagon[]
            {
                new Wagon("ReservedSeat", 54),
                new Wagon("Coupe", 36),
                new Wagon("SV", 18),
                new Wagon("Lux", 6),
            };

            int tempPax = Tickets;
            int reservedSeatCount = 0;
            int coupeCount = 0;
            int svCount = 0;
            int luxCount = 0;

            if (Tickets > 0) 
            {
                reservedSeatCount = tempPax / wagons[0].Seats;
                tempPax -= tempPax % wagons[0].Seats;
                
                if (tempPax > 0)
                {
                    coupeCount = tempPax / wagons[1].Seats;
                    tempPax = tempPax % wagons[1].Seats;
                }
                else if (tempPax > 0)
                {
                    svCount = tempPax / wagons[2].Seats;
                    tempPax = tempPax % wagons[2].Seats;
                }
                else if (tempPax > 0)
                {
                    luxCount = tempPax / wagons[4].Seats;
                    tempPax = tempPax % wagons[4].Seats;

                    if (tempPax > 0)
                    {
                        luxCount += 1;
                    }
                }
            }

            AddWagons(reservedSeatCount, coupeCount, svCount, luxCount);
            IsTrainFormed = true;
        }

        private void AddWagons(int reservedSeatCount, int coupeCount, int svCount, int luxCount)
        {
            int wagonCount = 0;

            for (int i = 0 ; i > wagonCount; i++ )
            {
                //Создать добавление нужных вагонов в список _train на основе их количества.
            }
        }

    }

    class Wagon
    {
        public string Type { get; protected set; }
        public int Seats { get; protected set; }
        public int WagonNumber { get; protected set; }

        public Wagon(string type, int seats, int wagonNumber)
        {
            Type = type;
            Seats = seats;
            WagonNumber = wagonNumber;
        }

        public Wagon(string type, int seats) : this (type, seats, 0)
        {

        }

        public Wagon()
        {
        }
    }
}