using System;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введіть кількість працівників:");
        int n;

        while (!int.TryParse(Console.ReadLine(), out n) || n <= 0)
        {
            Console.WriteLine("Будь ласка, введіть коректне число більше нуля.");
        }

        Worker[] workers = CreateWorkersArray(n);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Оберіть завдання:");
            Console.WriteLine("1. Вивести інформацію про одного працівника");
            Console.WriteLine("2. Вивести інформацію про всіх працівників");
            Console.WriteLine("3. Показати стаж роботи працівника");
            Console.WriteLine("4. Перевірити, чи працівник проживає близько до головного офісу");
            Console.WriteLine("5. Вихід");

            int choice;

            while (!int.TryParse(Console.ReadLine(), out choice) || (choice < 1 || choice > 5))
            {
                Console.WriteLine("Будь ласка, введіть коректне число від 1 до 5.");
            }

            switch (choice)
            {
                case 1:
                    Console.Write("Введіть прізвище працівника: ");
                    string lastName = Console.ReadLine();

                    Worker foundWorker = FindWorkerByLastName(workers, lastName);

                    if (foundWorker != null)
                    {
                        DisplayWorkerInfo(foundWorker);
                    }
                    else
                    {
                        Console.WriteLine($"Працівник із прізвищем {lastName} не знайдений.");
                    }
                    break;

                case 2:
                    DisplayAllWorkersInfo(workers);
                    break;

                case 3:
                    Console.Write("Введіть прізвище працівника:");
                    string lastNameForExperience = Console.ReadLine();

                    Worker workerForExperience = FindWorkerByLastName(workers, lastNameForExperience);

                    if (workerForExperience != null)
                    {
                        DisplayWorkExperience(workerForExperience);
                    }
                    else
                    {
                        Console.WriteLine($"Працівник із прізвищем {lastNameForExperience} не знайдений.");
                    }
                    break;

                case 4:
                    Console.Write("Введіть прізвище працівника:");
                    string lastNameForLiving = Console.ReadLine();

                    Worker workerForLiving = FindWorkerByLastName(workers, lastNameForLiving);

                    if (workerForLiving != null)
                    {
                        DisplayLivingInfo(workerForLiving);
                    }
                    else
                    {
                        Console.WriteLine($"Працівник із прізвищем {lastNameForLiving} не знайдений.");
                    }
                    break;

                case 5:
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }

    static Worker[] CreateWorkersArray(int n)
    {
        Worker[] workers = new Worker[n];

        for (int i = 0; i < n; i++)
        {
            Console.Clear();
            Console.WriteLine($"Введіть інформацію для працівника #{i + 1}:");
            workers[i] = Worker.CreateWorkerFromConsole();
        }

        return workers;
    }

    static void DisplayWorkerInfo(Worker worker)
    {
        Console.Clear();
        Console.WriteLine(worker.ToString());
        Console.WriteLine("\nНатисніть Enter, щоб продовжити...");
        Console.ReadLine();
    }

    static void DisplayAllWorkersInfo(Worker[] workers)
    {
        Console.Clear();
        foreach (var worker in workers)
        {
            Console.WriteLine(worker.ToString());
            Console.WriteLine("------------------------------");
        }

        Console.WriteLine("\nНатисніть Enter, щоб продовжити...");
        Console.ReadLine();
    }

    static void DisplayWorkExperience(Worker worker)
    {
        Console.Clear();
        int monthsOfWork = worker.GetWorkExperience();
        Console.WriteLine($"Стаж роботи працівника: {monthsOfWork} місяців");
        Console.WriteLine("\nНатисніть Enter, щоб продовжити...");
        Console.ReadLine();
    }

    static void DisplayLivingInfo(Worker worker)
    {
        Console.Clear();
        bool livesNearMainOffice = worker.LivesNotFarFromTheMainOffice();
        Console.WriteLine($"Проживає близько до головного офісу: {livesNearMainOffice}");
        Console.WriteLine("\nНатисніть Enter, щоб продовжити...");
        Console.ReadLine();
    }

    static Worker FindWorkerByLastName(Worker[] workers, string lastName)
    {
        return workers.FirstOrDefault(worker => worker.FullName.Split(' ').Last().Equals(lastName, StringComparison.OrdinalIgnoreCase));
    }
}

class Worker
{
    public string FullName { get; set; }
    public string HomeCity { get; set; }
    public DateTime StartDate { get; set; }
    public Company WorkPlace { get; set; }

    public int GetWorkExperience()
    {
        DateTime currentDate = DateTime.Now;
        int monthsOfWork = (currentDate.Year - StartDate.Year) * 12 + currentDate.Month - StartDate.Month;
        return monthsOfWork;
    }

    public bool LivesNotFarFromTheMainOffice()
    {
        return HomeCity.Equals(WorkPlace.MainOfficeCity, StringComparison.OrdinalIgnoreCase);
    }

    public static Worker CreateWorkerFromConsole()
    {
        Console.Write("Прізвище та ініціали: ");
        string fullName = Console.ReadLine();

        Console.Write("Місто проживання: ");
        string homeCity = Console.ReadLine();

        DateTime startDate;

        Console.Write("Дата початку роботи (рік-місяць-день): ");

        while (!DateTime.TryParse(Console.ReadLine(), out startDate))
        {
            Console.WriteLine("Будь ласка, введіть коректну дату у форматі (рік-місяць-день):");
        }

        Console.Write("Інформація про компанію:\nНазва компанії: ");
        string companyName = Console.ReadLine();

        Console.Write("Місто головного офісу компанії: ");
        string mainOfficeCity = Console.ReadLine();

        Console.Write("Посада працівника: ");
        string position = Console.ReadLine();

        double salary;

        Console.Write("Зарплата: ");

        while (!double.TryParse(Console.ReadLine(), out salary) || salary <= 0)
        {
            Console.WriteLine("Будь ласка, введіть коректну зарплатню більше нуля.");
        }

        bool isFullTime;

        Console.Write("Працює на повний робочий день (true/false): ");

        while (!bool.TryParse(Console.ReadLine(), out isFullTime))
        {
            Console.WriteLine("Будь ласка, введіть коректне значення (true або false):");
        }

        Company company = new Company(companyName, mainOfficeCity, position, salary, isFullTime);

        return new Worker { FullName = fullName, HomeCity = homeCity, StartDate = startDate, WorkPlace = company };
    }

    public override string ToString()
    {
        return $"Працівник: {FullName}\nМісто проживання: {HomeCity}\nДата початку роботи: {StartDate}\n{WorkPlace}";
    }
}

class Company
{
    public string Name { get; set; }
    public string MainOfficeCity { get; set; }
    public string Position { get; set; }
    public double Salary { get; set; }
    public bool IsFullTimeEmployee { get; set; }

    public Company(string name, string mainOfficeCity, string position, double salary, bool isFullTimeEmployee)
    {
        Name = name;
        MainOfficeCity = mainOfficeCity;
        Position = position;
        Salary = salary;
        IsFullTimeEmployee = isFullTimeEmployee;
    }

    public override string ToString()
    {
        return $"Компанія: {Name}\nГоловний офіс: {MainOfficeCity}\nПосада: {Position}\nЗарплата: {Salary}\nПовний робочий день: {IsFullTimeEmployee}";
    }
}
