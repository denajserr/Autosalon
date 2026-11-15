using System;
using System.Text;
using System.Globalization;

namespace Autosalon
{
    internal class Program
    {
        const double BasePrice = 20000.0;
        const double YearRetention = 0.88;
        const double MileagePerYear = 15.0;
        const double MaxMileageDiscount = 0.20;
        
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.CurrentCulture = new CultureInfo("uk-UA");

            bool running = true;
            while (running)
            {
                Console.WriteLine("=== Автосалон ===");
                Console.WriteLine("1 — довідка про програму");
                Console.WriteLine("2 — ціна автомобіля залежно від віку");
                Console.WriteLine("3 — продажі за марками та місяцями");
                Console.WriteLine("0 — вихід");
                Console.Write("Ваш вибір: ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ShowAbout();
                        Pause();
                        break;
                    case "2":
                        ShowPriceTable();
                        Pause();
                        break;
                    case "3":
                        Console.WriteLine("Поки не зроблено.");
                        Pause();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("До побачення.");
                        break;
                    default:
                        Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
                        Pause();
                        break;
                }
            }
        }

        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true);
            Console.Clear();
        }
        static void ShowAbout()
        {
            string[] names = { "VIN", "Марка", "Код моделі (довідник)", "Рік випуску",
                "Пробіг", "Колір", "Ціна", "Чи проданий" };
            string[] types = { "string", "string", "int", "int", "int", "string", "double", "bool" };

            Console.WriteLine("Тема: Автосалон.");
            Console.WriteLine("Сутності: Користувач, Автомобіль, Модель (довідник).");
            Console.WriteLine($"Поля сутності «Автомобіль» ({names.Length}):");
            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine("{0,2}. {1,-24}{2}", i + 1, names[i], types[i]);
            }
            Console.WriteLine("Підсумок: середня ціна за марками та кількість проданих авто за місяць.");
            Console.WriteLine("Місткість сховища: 100 записів.");
        }

        static void ShowPriceTable()
        {
            double start = 0.5;
            double end = 6.0;
            double step = 0.5;

            Console.WriteLine("Ціна автомобіля залежно від віку (дол.)");
            Console.WriteLine("{0,8}{1,14}{2,14}", "Років", "За віком", "З пробігом");

            // запас 1e-9 захищає останній рядок від похибки дробових чисел
            for (double years = start; years <= end + 1e-9; years += step)
            {
                double byAge = BasePrice * Math.Pow(YearRetention, years);
                double mileage = MileagePerYear * years;
                double discount = Math.Min(MaxMileageDiscount, 0.002 * mileage);
                double withMileage = byAge * (1 - discount);
                Console.WriteLine("{0,8:F2}{1,14:F2}{2,14:F2}", years, byAge, withMileage);
            }
        }
    }
}