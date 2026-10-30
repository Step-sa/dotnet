using System;

namespace ArtGalleryApp
{
    public interface IValuable
    {
        decimal Price { get; set; }
    }

    public abstract class ArtPiece : IValuable
    {
        public string Title;
        public int Year;
        public Artist Author;
        public Genre Genre;

        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value < 0)
                    _price = 0;
                else
                    _price = value;
            }
        }

        public ArtPiece(string title, int year, Artist author, Genre genre, decimal price)
        {
            Title = title;
            Year = year;
            Author = author;
            Genre = genre;
            Price = price;
        }

        public abstract void ShowInfo();
    }

    public class Painting : ArtPiece
    {
        public string Technique;

        public Painting(string title, int year, Artist author, Genre genre, string technique, decimal price)
            : base(title, year, author, genre, price)
        {
            Technique = technique;
        }

        public override void ShowInfo()
        {
            Console.WriteLine("Картина «" + Title + "» (" + Year + " г.)");
            Console.WriteLine("  Автор: " + Author.Name + " (" + Author.Country + ", род. " + Author.BirthYear + ")");
            Console.WriteLine("  Жанр: " + Genre.Name + " (" + Genre.Description + ")");
            Console.WriteLine("  Техника: " + Technique + ", Оценка: " + Price + " руб.");
        }
    }

    public class Sculpture : ArtPiece
    {
        public string Material;

        public Sculpture(string title, int year, Artist author, Genre genre, string material, decimal price)
            : base(title, year, author, genre, price)
        {
            Material = material;
        }

        public override void ShowInfo()
        {
            Console.WriteLine("Скульптура «" + Title + "» (" + Year + " г.)");
            Console.WriteLine("  Автор: " + Author.Name + " (" + Author.Country + ", род. " + Author.BirthYear + ")");
            Console.WriteLine("  Жанр: " + Genre.Name + " (" + Genre.Description + ")");
            Console.WriteLine("  Материал: " + Material + ", Оценка: " + Price + " руб.");
        }
    }

    public class Artist
    {
        public string Name;
        public string Country;
        public int BirthYear;

        public Artist(string name, string country, int birthYear)
        {
            Name = name;
            Country = country;
            BirthYear = birthYear;
        }
    }

    public class Genre
    {
        public string Name;
        public string Description;

        public Genre(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

    public class Hall
    {
        public string Name;
        public int Floor;
        public ArtPiece[] Pieces;
        public int Count;

        public Hall(string name, int floor, int capacity)
        {
            Name = name;
            Floor = floor;
            Pieces = new ArtPiece[capacity];
            Count = 0;
        }

        public bool AddPiece(ArtPiece piece)
        {
            if (Count < Pieces.Length)
            {
                Pieces[Count] = piece;
                Count++;
                return true;
            }
            return false;
        }

        public void ShowAllPieces()
        {
            Console.WriteLine("--- " + Name + " (Этаж " + Floor + ", Мест: " + Count + "/" + Pieces.Length + ") ---");
            if (Count == 0)
            {
                Console.WriteLine("  В зале нет экспонатов.");
                return;
            }

            for (int i = 0; i < Count; i++)
            {
                Pieces[i].ShowInfo();
                Console.WriteLine();
            }
        }
    }

    public class Exhibition
    {
        public string Title;
        public Hall[] Halls;
        public int HallCount;

        public Exhibition(string title, int maxHalls)
        {
            Title = title;
            Halls = new Hall[maxHalls];
            HallCount = 0;
        }

        public void AddHall(Hall hall)
        {
            if (HallCount < Halls.Length)
            {
                Halls[HallCount] = hall;
                HallCount++;
            }
        }

        public void ShowExhibition()
        {
            
            Console.WriteLine("Выставка: " + Title);
            for (int i = 0; i < HallCount; i++)
            {
                Halls[i].ShowAllPieces();
            }
        }
    }

    public class Guide
    {
        public string Name;
        public int ExperienceYears;

        public Guide(string name, int experienceYears)
        {
            Name = name;
            ExperienceYears = experienceYears;
        }
    }

    public class Tour
    {
        public string Topic;
        public Guide AssignedGuide;
        public int DurationMinutes;
        public int MaxGroupSize;

        public Tour(string topic, Guide guide, int duration, int maxGroup)
        {
            Topic = topic;
            AssignedGuide = guide;
            DurationMinutes = duration;
            MaxGroupSize = maxGroup;
        }

        public void ShowTourInfo()
        {
            Console.WriteLine("Экскурсия: «" + Topic + "»");
            Console.WriteLine("Гид: " + AssignedGuide.Name + " (Стаж: " + AssignedGuide.ExperienceYears + " лет)");
            Console.WriteLine("Длительность: " + DurationMinutes + " мин., Макс. группа: " + MaxGroupSize + " чел.");
        }
    }

    public class Ticket
    {
        public Tour Tour;
        public int RegularCount;
        public int DiscountCount;
        public decimal StandardPrice = 500m;
        public decimal DiscountPrice = 300m;

        public Ticket(Tour tour, int regularCount, int discountCount)
        {
            Tour = tour;
            RegularCount = regularCount;
            DiscountCount = discountCount;
        }

        public decimal CalculateCost()
        {
            return (RegularCount * StandardPrice) + (DiscountCount * DiscountPrice);
        }
    }

    public class Program
    {
        static Artist SelectOrCreateArtist(Artist[] artists, ref int artistCount)
        {
            Console.WriteLine("\nВыберите автора:");
            for (int i = 0; i < artistCount; i++)
            {
                Console.WriteLine((i + 1) + ". " + artists[i].Name + " (" + artists[i].Country + ", " + artists[i].BirthYear + " г.р.)");
            }
            Console.WriteLine((artistCount + 1) + ". + Добавить нового автора");
            Console.Write("Выбор: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            if (choice >= 1 && choice <= artistCount)
            {
                return artists[choice - 1];
            }

            Console.Write("Введите ФИО автора: ");
            string name = Console.ReadLine();
            Console.Write("Страна: ");
            string country = Console.ReadLine();
            Console.Write("Год рождения: ");
            int birthYear = Convert.ToInt32(Console.ReadLine());

            Artist newArtist = new Artist(name, country, birthYear);
            artists[artistCount] = newArtist;
            artistCount++;
            return newArtist;
        }

        static Genre SelectOrCreateGenre(Genre[] genres, ref int genreCount)
        {
            Console.WriteLine("\nВыберите жанр:");
            for (int i = 0; i < genreCount; i++)
            {
                Console.WriteLine((i + 1) + ". " + genres[i].Name + " (" + genres[i].Description + ")");
            }
            Console.WriteLine((genreCount + 1) + ". + Добавить новый жанр");
            Console.Write("Выбор: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            if (choice >= 1 && choice <= genreCount)
            {
                return genres[choice - 1];
            }

            Console.Write("Название жанра: ");
            string name = Console.ReadLine();
            Console.Write("Краткое описание: ");
            string desc = Console.ReadLine();

            Genre newGenre = new Genre(name, desc);
            genres[genreCount] = newGenre;
            genreCount++;
            return newGenre;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Artist[] artists = new Artist[20];
            int artistCount = 0;
            artists[artistCount++] = new Artist("Иван Шишкин", "Россия", 1832);
            artists[artistCount++] = new Artist("Огюст Роден", "Франция", 1840);

            Genre[] genres = new Genre[10];
            int genreCount = 0;
            genres[genreCount++] = new Genre("Реализм", "Правдивое отображение реальности");
            genres[genreCount++] = new Genre("Пейзаж", "Изображение природы и ландшафтов");

            Hall hall1 = new Hall("Зал живописи", 1, 2);
            hall1.AddPiece(new Painting("Утро в сосновом лесу", 1889, artists[0], genres[1], "Холст, масло", 150000000m));
            hall1.AddPiece(new Painting("Рожь", 1878, artists[0], genres[1], "Холст, масло", 100000000m));

            Hall hall2 = new Hall("Зал скульптуры", 2, 2);
            hall2.AddPiece(new Sculpture("Мыслитель", 1904, artists[1], genres[0], "Бронза", 80000000m));

            Exhibition exhibition = new Exhibition("Шедевры галереи", 5);
            exhibition.AddHall(hall1);
            exhibition.AddHall(hall2);

            Guide guide = new Guide("Алексей Смирнов", 7);
            Tour tour = new Tour("Главные шедевры коллекции", guide, 60, 15);

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n1. Показать залы и экспонаты");
                Console.WriteLine("2. Добавить новый экспонат в зал");
                Console.WriteLine("3. Найти экспонаты по автору");
                Console.WriteLine("4. Расчет и покупка экскурсионных билетов");
                Console.WriteLine("5. Финансовый отчет (общая стоимость экспонатов)");
                Console.WriteLine("6. Выход");
                Console.Write("Выберите пункт: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        exhibition.ShowExhibition();
                        break;

                    case "2":
                        Console.WriteLine("Выберите зал для добавления:");
                        for (int i = 0; i < exhibition.HallCount; i++)
                        {
                            Console.WriteLine((i + 1) + ". " + exhibition.Halls[i].Name + " (Занято " + exhibition.Halls[i].Count + "/" + exhibition.Halls[i].Pieces.Length + ")");
                        }
                        Console.Write("Номер зала: ");
                        int hallIndex = Convert.ToInt32(Console.ReadLine()) - 1;

                        if (hallIndex >= 0 && hallIndex < exhibition.HallCount)
                        {
                            Hall selectedHall = exhibition.Halls[hallIndex];

                            Console.WriteLine("\nКакой тип экспоната добавить?");
                            Console.WriteLine("1. Картина");
                            Console.WriteLine("2. Скульптура");
                            Console.Write("Выбор: ");
                            string typeChoice = Console.ReadLine();

                            Console.Write("Название произведения: ");
                            string title = Console.ReadLine();

                            Artist selectedAuthor = SelectOrCreateArtist(artists, ref artistCount);
                            Genre selectedGenre = SelectOrCreateGenre(genres, ref genreCount);

                            Console.Write("Год создания: ");
                            int year = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Оценочная стоимость (руб): ");
                            decimal price = Convert.ToDecimal(Console.ReadLine());

                            ArtPiece newPiece = null;

                            if (typeChoice == "1")
                            {
                                Console.Write("Техника (например: Холст, масло): ");
                                string tech = Console.ReadLine();
                                newPiece = new Painting(title, year, selectedAuthor, selectedGenre, tech, price);
                            }
                            else if (typeChoice == "2")
                            {
                                Console.Write("Материал (например: Мрамор, Бронза): ");
                                string mat = Console.ReadLine();
                                newPiece = new Sculpture(title, year, selectedAuthor, selectedGenre, mat, price);
                            }

                            if (newPiece != null)
                            {
                                if (selectedHall.AddPiece(newPiece))
                                {
                                    Console.WriteLine("\nЭкспонат успешно добавлен в зал!");
                                }
                                else
                                {
                                    Console.WriteLine("\nОтказ: Зал переполнен, лимит мест исчерпан.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неверный номер зала.");
                        }
                        break;

                    case "3":
                        Console.Write("Введите имя или фамилию автора: ");
                        string query = Console.ReadLine().ToLower();
                        bool found = false;

                        for (int i = 0; i < exhibition.HallCount; i++)
                        {
                            Hall h = exhibition.Halls[i];
                            for (int j = 0; j < h.Count; j++)
                            {
                                if (h.Pieces[j].Author.Name.ToLower().Contains(query))
                                {
                                    Console.WriteLine("\nНайдено в «" + h.Name + "»:");
                                    h.Pieces[j].ShowInfo();
                                    found = true;
                                }
                            }
                        }

                        if (!found)
                        {
                            Console.WriteLine("Экспонатов указанного автора не найдено.");
                        }
                        break;

                    case "4":
                        tour.ShowTourInfo();
                        Console.Write("\nВведите общее количество посетителей в группе: ");
                        int totalCount = Convert.ToInt32(Console.ReadLine());

                        if (totalCount <= 0)
                        {
                            Console.WriteLine("Ошибка: количество посетителей должно быть больше 0.");
                            break;
                        }

                        if (totalCount > tour.MaxGroupSize)
                        {
                            Console.WriteLine("Ошибка: группа не может превышать " + tour.MaxGroupSize + " человек.");
                            break;
                        }

                        Console.Write("Сколько из них имеют льготы (студенты/пенсионеры)?: ");
                        int discountCount = Convert.ToInt32(Console.ReadLine());

                        if (discountCount < 0 || discountCount > totalCount)
                        {
                            Console.WriteLine("Ошибка: число льготников не может быть отрицательным или больше общего числа группы.");
                            break;
                        }

                        int regularCount = totalCount - discountCount;
                        Ticket ticket = new Ticket(tour, regularCount, discountCount);

                        Console.WriteLine("\n--- Расчет билетов ---");
                        Console.WriteLine("Обычные билеты: " + regularCount + " шт. x " + ticket.StandardPrice + " руб. = " + (regularCount * ticket.StandardPrice) + " руб.");
                        Console.WriteLine("Льготные билеты: " + discountCount + " шт. x " + ticket.DiscountPrice + " руб. = " + (discountCount * ticket.DiscountPrice) + " руб.");
                        Console.WriteLine("Итого к олате: " + ticket.CalculateCost() + " руб.");
                        break;

                    case "5":
                        decimal totalSum = 0;
                        int totalPiecesCount = 0;

                        for (int i = 0; i < exhibition.HallCount; i++)
                        {
                            Hall h = exhibition.Halls[i];
                            for (int j = 0; j < h.Count; j++)
                            {
                                totalSum += h.Pieces[j].Price;
                                totalPiecesCount++;
                            }
                        }

                        Console.WriteLine("--- Финансовый отчет галереи ---");
                        Console.WriteLine("Всего экспонатов на выставке: " + totalPiecesCount + " шт.");
                        Console.WriteLine("Общая оценочная стоимость фондов: " + totalSum + " руб.");
                        break;

                    case "6":
                        running = false;
                        Console.WriteLine("Программа завершена.");
                        break;

                    default:
                        Console.WriteLine("Неверный пункт меню.");
                        break;
                }
            }
        }
    }
}