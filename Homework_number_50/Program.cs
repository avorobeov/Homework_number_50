using System;
using System.Collections.Generic;

namespace Homework_number_50
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Detail> details = new List<Detail>();
            Queue<Car> cars = new Queue<Car>();

            int startingAmountMoney = 30;

            details.Add(new Detail(10, "тормоза"));
            details.Add(new Detail(15, "двигатель"));
            details.Add(new Detail(5, "колисо"));

            cars.Enqueue(new Car("тормоза"));
            cars.Enqueue(new Car("тормоза"));
            cars.Enqueue(new Car("двигатель"));
            cars.Enqueue(new Car("колисо"));

            CarService carService = new CarService(startingAmountMoney, details, cars);
            carService.Work();
        }
    }

    class Detail
    {
        public Detail(int price, string title)
        {
            Price = price;
            Title = title;
        }

        public int Price { get; private set; }
        public string Title { get; private set; }
    }

    class Car
    {
        public Car(string damagedPart)
        {
            DamagedPart = damagedPart;
        }

        public string DamagedPart { get; private set; }
    }

    class CarService
    {
        private List<Detail> _details;
        private Queue<Car> _cars;

        private int _money = 100;

        public CarService(int money, List<Detail> details, Queue<Car> cars)
        {
            _money = money;
            _details = details;
            _cars = cars;
        }

        public void Work()
        {
            const string CommandServeCustomer = "1";
            const string CommandExit = "2";

            bool isExit = false;
            string userInput;

            while (isExit == false)
            {
                ShowInfo();

                Console.WriteLine($"Для того что бы обслужить следящего клиент нажмите: {CommandServeCustomer}\n" +
                                  $"Для того что бы закрыть приложение нажмите {CommandExit}\n");
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandServeCustomer:
                        ServeCustomer();
                        break;

                    case CommandExit:
                        isExit = true;
                        break;

                    default:
                        Console.WriteLine("Такой команды нет в наличии!");
                        break;
                }

                Console.WriteLine("\n\nДля продолжения ведите любую клавишу...");
                Console.ReadKey();
                Console.Clear();

                if (TryChecksResource() == false)
                {
                    isExit = true;
                }
            }
        }

        private void ServeCustomer()
        {
            const int AmountFine = 10;

            Car car = _cars.Dequeue();

            Console.WriteLine($"\nУ автомобиля неисправен: {car.DamagedPart}\n");

            Console.Write("Укажите деталь которую нужно заменить у клиента:");
            string titleDetail = Console.ReadLine();

            if (TryFindPart(out Detail detail, titleDetail) == true)
            {
                int priceRepair = GetPriceRepair(detail);

                WriteCostRepair(priceRepair, detail);

                if (TryReplacePart(detail, car) == true)
                {
                    Console.WriteLine($"Всё хорошо мы заработали {priceRepair}");

                    _money += priceRepair;
                }
                else
                {
                    Console.WriteLine($"Оператор ошибся с деталью и вы вынуждены возместить ущерб клиенту в розмере {priceRepair}\n");

                    _money -= priceRepair;
                }

            }
            else
            {
                Console.Write($"Сожалению у нас нет этой детали мы готовы выплатить вам компенсацию в размере {AmountFine}");

                _money -= AmountFine;
            }
        }

        private bool TryFindPart(out Detail detail, string titleDetail)
        {
            detail = null;

            for (int i = 0; i < _details.Count; i++)
            {
                if (titleDetail.ToLower() == _details[i].Title.ToLower())
                {
                    detail = _details[i];
                    _details.Remove(detail);

                    return true;
                }
            }

            return false;
        }

        private int GetPriceRepair(Detail detail)
        {
            int percentageCostDetail = 25;
            int maxDivisor = 100;

            return (detail.Price * percentageCostDetail / maxDivisor) + detail.Price;
        }

        private bool TryReplacePart(Detail detail, Car car)
        {
            return detail.Title.ToLower() == car.DamagedPart.ToLower();
        }

        private void WriteCostRepair(int priceRepair, Detail detail)
        {
            Console.WriteLine($"\n\n\nУ автомобиля поврежден: {detail.Title}\n" +
                              $"Цена дитали: {detail.Price}\n" +
                              $"Конечная сумма к оплате: {priceRepair}\n\n\n");
        }

        private void ShowInfo()
        {
            Console.WriteLine($"У вас количество клиентов в очереди {_cars.Count}\n" +
                              $"Количество деталей {_details.Count}\n" +
                              $"Количество денег {_money}\n\r\n");
        }

        private bool TryChecksResource()
        {
            return _cars.Count > 0 && _details.Count > 0 && _money > 0;
        }
    }
}
