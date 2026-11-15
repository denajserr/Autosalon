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
        
        const int MaxTableSize = 20;

        static readonly string[] Brands = { "Toyota", "BMW", "Audi", "Skoda", "Ford" };
        static readonly string[] Months = { "Січ", "Лют", "Бер", "Кві", "Тра", "Чер",
            "Лип", "Сер", "Вер", "Жов", "Лис", "Гру" };
        
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
                        ShowSalesTable();
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
        static bool TryReadSize(string prompt, out int size)
        {
            Console.Write(prompt);
            string text = Console.ReadLine();
            if (!int.TryParse(text, out size) || size <= 0 || size > MaxTableSize)
            {
                Console.WriteLine($"Помилка: потрібне ціле число від 1 до {MaxTableSize}.");
                return false;
            }
            return true;
        }

        static string RowName(int index)
        {
            return index < Brands.Length ? Brands[index] : "Марка " + (index + 1);
        }

        static string ColumnName(int index)
        {
            return index < Months.Length ? Months[index] : "М" + (index + 1);
        }

// умовні дані: справжні продажі з'являться разом зі сховищем
        static int SoldCars(int row, int col)
        {
            return (row * 5 + col * 3 + row * col) % 9 + 1;
        }

        static void ShowSalesTable()
        {
            if (!TryReadSize("Кількість рядків (марок): ", out int rows)) return;
            if (!TryReadSize("Кількість стовпців (місяців): ", out int cols)) return;

            int[] colTotals = new int[cols];
            int grandTotal = 0;
            string line = new string('-', 11 + cols * 5 + 7);

            Console.WriteLine("\nПродано автомобілів за марками та місяцями");
            Console.Write("{0,-11}", "Марка");
            for (int c = 0; c < cols; c++)
            {
                Console.Write("{0,5}", ColumnName(c));
            }
            Console.WriteLine("{0,7}", "Разом");
            Console.WriteLine(line);

            for (int r = 0; r < rows; r++)
            {
                int rowTotal = 0;
                Console.Write("{0,-11}", RowName(r));
                for (int c = 0; c < cols; c++)
                {
                    int value = SoldCars(r, c);
                    rowTotal += value;
                    colTotals[c] += value;
                    Console.Write("{0,5}", value);
                }
                grandTotal += rowTotal;
                Console.WriteLine("{0,7}", rowTotal);
            }

            Console.WriteLine(line);
            Console.Write("{0,-11}", "Разом");
            for (int c = 0; c < cols; c++)
            {
                Console.Write("{0,5}", colTotals[c]);
            }
            Console.WriteLine("{0,7}", grandTotal);
        }
    }
}