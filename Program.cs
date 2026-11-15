using System;
using System.Globalization;
using System.Text;

namespace Autosalon
{
    internal class Program
    {
        const double BasePrice = 20000.0;
        const double YearRetention = 0.88;
        const double MileagePerYear = 15.0;
        const double MaxMileageDiscount = 0.20;
        const int MaxTableSize = 20;
        const int LabelWidth = 11;
        const int CellWidth = 5;
        const int TotalWidth = 7;

        static readonly string[] Brands = { "Toyota", "BMW", "Audi", "Skoda", "Ford" };
        static readonly string[] Months = { "Січ", "Лют", "Бер", "Кві", "Тра", "Чер",
                                            "Лип", "Сер", "Вер", "Жов", "Лис", "Гру" };

        static void Main(string[] args)
        {
            ConfigureConsole();
            bool running = true;
            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ShowAbout(); Pause(); break;
                    case "2": ShowPriceTable(); Pause(); break;
                    case "3": ShowSalesTable(); Pause(); break;
                    case "0": running = false; break;
                    default: ShowUnknownChoice(); Pause(); break;
                }
            }
            Console.WriteLine("До побачення.");
        }

        static void ConfigureConsole()
        {
            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.CurrentCulture = new CultureInfo("uk-UA");
        }

        static void ShowMenu()
        {
            Console.WriteLine("=== Автосалон ===");
            Console.WriteLine("1 — довідка про програму");
            Console.WriteLine("2 — ціна автомобіля залежно від віку");
            Console.WriteLine("3 — продажі за марками та місяцями");
            Console.WriteLine("0 — вихід");
            Console.Write("Ваш вибір: ");
        }

        static void ShowUnknownChoice()
        {
            Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
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
            Console.WriteLine("Підсумок: середня ціна за марками");
            Console.WriteLine("та кількість проданих авто за місяць.");
            Console.WriteLine("Місткість сховища: 100 записів.");
        }

        static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                // кому й крапку приймаємо однаково
                string text = (Console.ReadLine() ?? "").Trim().Replace(',', '.');
                if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {
                    Console.WriteLine("Помилка: введіть число.");
                    continue;
                }
                if (!(value >= min && value <= max))
                {
                    Console.WriteLine($"Помилка: число має бути від {min} до {max}.");
                    continue;
                }
                return value;
            }
        }

        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string text = Console.ReadLine();
                if (!int.TryParse(text, out int value))
                {
                    Console.WriteLine("Помилка: введіть ціле число.");
                    continue;
                }
                if (value < min || value > max)
                {
                    Console.WriteLine($"Помилка: число має бути від {min} до {max}.");
                    continue;
                }
                return value;
            }
        }

        static double CalculatePriceByAge(double basePrice, double years)
        {
            return basePrice * Math.Pow(YearRetention, years);
        }

        static double CalculateMileageDiscount(double years)
        {
            double mileage = MileagePerYear * years;
            return Math.Min(MaxMileageDiscount, 0.002 * mileage);
        }

        static double CalculatePriceWithMileage(double priceByAge, double years)
        {
            return priceByAge * (1 - CalculateMileageDiscount(years));
        }

        static void ShowPriceTable()
        {
            double start = ReadDouble("Початок, років (від 0 до 30): ", 0, 30);
            double end = ReadDouble("Кінець, років (від початку до 30): ", start, 30);
            double step = ReadDouble("Крок, років (від 0,1 до 5): ", 0.1, 5);
            Console.WriteLine();
            PrintPriceTable(start, end, step);
        }

        static void PrintPriceTable(double start, double end, double step)
        {
            Console.WriteLine("Ціна автомобіля залежно від віку (дол.)");
            Console.WriteLine("{0,8}{1,14}{2,14}", "Років", "За віком", "З пробігом");

            // запас 1e-9 захищає останній рядок від похибки дробових чисел
            for (double years = start; years <= end + 1e-9; years += step)
            {
                double byAge = CalculatePriceByAge(BasePrice, years);
                double withMileage = CalculatePriceWithMileage(byAge, years);
                Console.WriteLine("{0,8:F2}{1,14:F2}{2,14:F2}", years, byAge, withMileage);
            }
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

        static int RowTotal(int row, int cols)
        {
            int total = 0;
            for (int c = 0; c < cols; c++)
            {
                total += SoldCars(row, c);
            }
            return total;
        }

        static int ColumnTotal(int col, int rows)
        {
            int total = 0;
            for (int r = 0; r < rows; r++)
            {
                total += SoldCars(r, col);
            }
            return total;
        }

        static int GrandTotal(int rows, int cols)
        {
            int total = 0;
            for (int r = 0; r < rows; r++)
            {
                total += RowTotal(r, cols);
            }
            return total;
        }

        static void ShowSalesTable()
        {
            int rows = ReadInt($"Кількість рядків (марок), від 1 до {MaxTableSize}: ", 1, MaxTableSize);
            int cols = ReadInt($"Кількість стовпців (місяців), від 1 до {MaxTableSize}: ", 1, MaxTableSize);
            PrintSalesTable(rows, cols);
        }

        static void PrintSalesTable(int rows, int cols)
        {
            Console.WriteLine("\nПродано автомобілів за марками та місяцями");
            PrintSalesHeader(cols);
            PrintSeparator(cols);
            for (int r = 0; r < rows; r++)
            {
                PrintSalesRow(r, cols);
            }
            PrintSeparator(cols);
            PrintSalesTotals(rows, cols);
        }

        static void PrintSeparator(int cols)
        {
            Console.WriteLine(new string('-', LabelWidth + cols * CellWidth + TotalWidth));
        }

        static void PrintSalesHeader(int cols)
        {
            Console.Write("Марка".PadRight(LabelWidth));
            for (int c = 0; c < cols; c++)
            {
                Console.Write(ColumnName(c).PadLeft(CellWidth));
            }
            Console.WriteLine("Разом".PadLeft(TotalWidth));
        }

        static void PrintSalesRow(int row, int cols)
        {
            Console.Write(RowName(row).PadRight(LabelWidth));
            for (int c = 0; c < cols; c++)
            {
                Console.Write(SoldCars(row, c).ToString().PadLeft(CellWidth));
            }
            Console.WriteLine(RowTotal(row, cols).ToString().PadLeft(TotalWidth));
        }

        static void PrintSalesTotals(int rows, int cols)
        {
            Console.Write("Разом".PadRight(LabelWidth));
            for (int c = 0; c < cols; c++)
            {
                Console.Write(ColumnTotal(c, rows).ToString().PadLeft(CellWidth));
            }
            Console.WriteLine(GrandTotal(rows, cols).ToString().PadLeft(TotalWidth));
        }
    }
}
