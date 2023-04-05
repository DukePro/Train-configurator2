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
        private Terminal _terminal = new Terminal();

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
                        _terminal.CreateDirection();
                        break;

                    case MenuSellTickets:
                        _terminal.SellTickets();
                        break;

                    case MenuCompileTrain:
                        _terminal.CompileTrain();
                        break;

                    case MenuShowTrainParam:
                        _terminal.ShowTrain();
                        break;

                    case MenuSendTrain:
                        _terminal.SendTrain();
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

            if (_terminal.IsTrainFormed == true)
            {
                readyToGo = "ГОТОВ!";
                trainStatus = "Сформирован";
            }
            else
            {
                readyToGo = "НЕ ГОТОВ";
                trainStatus = "НЕ сформирован";
            }

            CleanConsoleString();
            Console.SetCursorPosition(infoPositionX, infoPositionY);
            Console.WriteLine($"Направление: {_terminal.Direction} | Билетов продано: {_terminal.Tickets} | Статус поезда: {trainStatus} | К отправке: {readyToGo}");
        }

        private void CleanConsoleString()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 0);
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

        private int _wagonNumber;

        private List<Wagon> _train = new List<Wagon>();

        private Random _rand = new Random();
        private Wagon _wagon = new Wagon();

        private Wagon[] _wagons = new Wagon[]
            {
                new Wagon("Плацкарт", 54),
                new Wagon("Купе", 36),
                new Wagon("СВ", 18),
                new Wagon("Люкс", 6),
            };

        public void CreateDirection()
        {
            string departurePoint;
            string arriavalPoint;

            if (Tickets == 0)
            {
                Console.WriteLine("Введите пункт отправления");
                departurePoint = Console.ReadLine();

                Console.WriteLine("Введите пункт прибытия");
                arriavalPoint = Console.ReadLine();

                Direction = $"{departurePoint} - {arriavalPoint}";

                Console.WriteLine($"Направление \"{departurePoint}\" - \"{arriavalPoint}\" создано");
            }
            else
            {
                Console.WriteLine("Нельзя изменить направление, на которое уже проданы билеты");
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
            int tempPax = Tickets;
            int reservedSeatCount = 0;
            int coupeCount = 0;
            int svCount = 0;
            int luxCount = 0;

            _wagonNumber = 1;

            if (tempPax > 0)
            {
                reservedSeatCount = tempPax / _wagons[0].Seats;

                while (tempPax >= _wagons[0].Seats)
                {
                    tempPax -= _wagons[0].Seats;
                    AddWagon(_wagons[0].Type, _wagons[0].Seats);
                }

                coupeCount = tempPax / _wagons[1].Seats;

                while (tempPax >= _wagons[1].Seats)
                {
                    tempPax -= _wagons[1].Seats;
                    AddWagon(_wagons[1].Type, _wagons[1].Seats);
                }

                svCount = tempPax / _wagons[2].Seats;

                while (tempPax >= _wagons[2].Seats)
                {
                    tempPax -= _wagons[2].Seats;
                    AddWagon(_wagons[2].Type, _wagons[2].Seats);
                }

                luxCount = tempPax / _wagons[3].Seats;

                while (tempPax <= _wagons[2].Seats - 1 && tempPax > 0)
                {
                    if (tempPax > 0 && tempPax < _wagons[3].Seats)
                    {
                        AddWagon(_wagons[3].Type, tempPax);
                        tempPax = 0;
                    }
                    else
                    {
                        tempPax -= _wagons[3].Seats;
                        AddWagon(_wagons[3].Type, _wagons[3].Seats);
                    }
                }

                IsTrainFormed = true;
                Console.WriteLine("Состав сформирован и готов к отправке");
            }
            else
            {
                Console.WriteLine("Сначала нужно продать билеты");
            }
        }

        private void AddWagon(string type, int pax)
        {
            if (type == "Плацкарт")
            {
                _train.Add(new Wagon("Плацкарт", 54, pax, _wagonNumber++));
            }
            if (type == "Купе")
            {
                _train.Add(new Wagon("Купе", 36, pax, _wagonNumber++));
            }
            if (type == "СВ")
            {
                _train.Add(new Wagon("СВ", 18, pax, _wagonNumber++));
            }
            if (type == "Люкс")
            {
                _train.Add(new Wagon("Люкс", 6, pax, _wagonNumber++));
            }
        }

        public void ShowTrain()
        {
            if (IsTrainFormed)
            {
                _wagon.ShowParameters(_train);
            }
            else
            {
                Console.WriteLine("Состав ещё не сформирован");
            }
        }

        public void SendTrain()
        {
            Direction = "Не задано";
            Tickets = 0;
            _train.Clear();
            IsTrainFormed = false;

            Console.WriteLine("Состав отправлен");
        }
    }

    class Wagon
    {
        public string Type { get; protected set; }
        public int Seats { get; protected set; }
        public int Pax { get; protected set; }
        public int Number { get; protected set; }

        public Wagon(string type, int seats, int pax, int number)
        {
            Type = type;
            Seats = seats;
            Pax = pax;
            Number = number;
        }

        public Wagon(string type, int seats) : this(type, seats, 0, 0)
        {

        }

        public Wagon()
        {
        }

        public void ShowParameters(List <Wagon> wagons)
        {
            int checkTotalPax = 0;

            Console.WriteLine("Состав сформирован следующим образом:");

            foreach (Wagon wagon in wagons)
            {
                Console.WriteLine($"Номер вагона - {wagon.Number} , Тип вагона - {wagon.Type} , Пассажиров в вагоне - {wagon.Pax}");
                checkTotalPax += wagon.Pax;
            }

            Console.WriteLine($"Всего пассажиров в поезде - {checkTotalPax}");
        }
    }
}