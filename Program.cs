using System;

// № 4
using System.IO; // Формирование пути к журналу.

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
    // № 2
    public class Hall
    {
        public string Name;
        public int Floor;
        public readonly GalleryCollection<ArtPiece> Pieces;
        public int Count => Pieces.Count;

        public Hall(string name, int floor, int capacity)
        {
            Name = name;
            Floor = floor;

            Pieces = new GalleryCollection<ArtPiece>(capacity);
        }

        public bool AddPiece(ArtPiece piece)
        {
            return Pieces.TryAdd(piece);
        }

        public void ShowAllPieces()
        {

            Console.WriteLine("--- " + Name + " (Этаж " + Floor + ", Мест: " + Count + "/" + Pieces.Capacity + ") ---");
            if (Count == 0)
            {
                Console.WriteLine("  В зале нет экспонатов.");
                return;
            }

            foreach (ArtPiece piece in Pieces)
            {
                piece.ShowInfo();
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

    // № 2
    internal interface IArtPieceViewer<in T> where T : ArtPiece
    {
        void Show(T piece);
    }

    internal class ArtPieceTitleViewer : IArtPieceViewer<ArtPiece>
    {
        public void Show(ArtPiece piece)
        {
            Console.WriteLine("  " + piece.Title);
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

        // № 2
        static void ShowGenericCollectionDemo(Painting painting)
        {
            var paintings = new GalleryCollection<Painting>(1);
            paintings.TryAdd(painting);

            IEnumerable<Painting> paintingSequence = paintings;
            IEnumerable<ArtPiece> artPieces = paintingSequence;
            Console.WriteLine("Ковариантность: перебираем картины как экспонаты.");
            foreach (ArtPiece piece in artPieces)
                Console.WriteLine("  " + piece.Title);
            
            IArtPieceViewer<ArtPiece> artPieceViewer = new ArtPieceTitleViewer();
            IArtPieceViewer<Painting> paintingViewer = artPieceViewer;
            Console.WriteLine("Контравариантность: обработчик экспонатов выводит картину.");
            paintingViewer.Show(painting);
        }

        // № 3
        static void ShowSortingDemo(Hall hall, GalleryLogger logger)
        {
            GalleryCollection<ArtPiece> pieces = hall.Pieces;
            Func<ArtPiece, decimal> selectPrice = piece => piece.Price;
            Comparison<ArtPiece> compareByPrice = (first, second) =>
                selectPrice(first).CompareTo(selectPrice(second));
            Comparison<ArtPiece> compareByNewestFirst = (first, second) =>
                second.Year.CompareTo(first.Year);
 

            Action<ArtPiece> showPiece = piece =>
                Console.WriteLine("  " + piece.Title + " (" + piece.Year + " г., " + piece.Price + " руб.)");

            Console.WriteLine("Зал: " + hall.Name);
            Console.WriteLine("До сортировки:");
            pieces.ForEach(showPiece);

            if (pieces.Count >= 2)
            {
                int result = pieces.CompareAt(0, 1, compareByPrice);
                string relation = result < 0 ? "дешевле второго"
                    : result > 0 ? "дороже второго" : "стоит столько же, сколько второй";
                Console.WriteLine("Первый экспонат " + relation + ".");
            }

            pieces.Sort(compareByPrice);
            Console.WriteLine("По цене, от меньшей к большей:");
            pieces.ForEach(showPiece);

            pieces.Sort(compareByNewestFirst);
            Console.WriteLine("По году, от новых к старым:");
            pieces.ForEach(showPiece);

            // № 4
            logger.Log("Сортировка завершена. Экспонатов обработано", pieces.Count);
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // № 4
            // Оба подписчика получают каждую запись, а меню вызывает только Log<T>.
            var logger = new GalleryLogger();
            var consoleLog = new ConsoleLogWriter();
            var fileLog = new FileLogWriter(Path.Combine(Environment.CurrentDirectory, "gallery.log"));
            logger.EntryCreated += consoleLog.Write;
            logger.EntryCreated += fileLog.Write;
            logger.Log("Запуск приложения", "Художественная галерея");

            Artist[] artists = new Artist[20];
            int artistCount = 0;
            artists[artistCount++] = new Artist("Иван Шишкин", "Россия", 1832);
            artists[artistCount++] = new Artist("Огюст Роден", "Франция", 1840);

            Genre[] genres = new Genre[10];
            int genreCount = 0;
            genres[genreCount++] = new Genre("Реализм", "Правдивое отображение реальности");
            genres[genreCount++] = new Genre("Пейзаж", "Изображение природы и ландшафтов");

            Hall hall1 = new Hall("Зал живописи", 1, 2);
            Painting firstPainting = new Painting("Утро в сосновом лесу", 1889, artists[0], genres[1], "Холст, масло", 150000000m);
            hall1.AddPiece(firstPainting);
            hall1.AddPiece(new Painting("Рожь", 1878, artists[0], genres[1], "Холст, масло", 100000000m));

            Hall hall2 = new Hall("Зал скульптуры", 2, 2);
            hall2.AddPiece(new Sculpture("Мыслитель", 1904, artists[1], genres[0], "Бронза", 80000000m));

            Exhibition exhibition = new Exhibition("Шедевры галереи", 5);
            exhibition.AddHall(hall1);
            exhibition.AddHall(hall2);
            // № 4
            // Записываем название выставки после заполнения её залов.
            logger.Log("Выставка подготовлена", exhibition.Title);

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
                Console.WriteLine("7. Показать работу обобщённой коллекции");
                Console.WriteLine("8. Сравнить и отсортировать экспонаты зала живописи");
                Console.Write("Выберите пункт: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                // № 4
                // Фиксируем выбранный этап до выполнения команды меню.
                logger.Log("Выбран пункт меню", choice);
                switch (choice)
                {
                    case "1":
                        exhibition.ShowExhibition();
                        break;

                    case "2":
                        Console.WriteLine("Выберите зал для добавления:");
                        for (int i = 0; i < exhibition.HallCount; i++)
                        {
                            Console.WriteLine((i + 1) + ". " + exhibition.Halls[i].Name + " (Занято " + exhibition.Halls[i].Count + "/" + exhibition.Halls[i].Pieces.Capacity + ")");
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
                                    // № 4
                                    // Запись связывает добавленный экспонат с его залом.
                                    logger.Log("Экспонат добавлен в зал «" + selectedHall.Name + "»", newPiece.Title);
                                }
                                else
                                {
                                    Console.WriteLine("\nОтказ: Зал переполнен, лимит мест исчерпан.");
                                    // № 4
                                    // Записываем отказ, чтобы отличить попытку от успешного добавления.
                                    logger.Log("Добавление отклонено. Зал заполнен", selectedHall.Name);
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
                            foreach (ArtPiece piece in h.Pieces)
                            {
                                if (piece.Author.Name.ToLower().Contains(query))
                                {
                                    Console.WriteLine("\nНайдено в «" + h.Name + "»:");
                                    piece.ShowInfo();
                                    found = true;
                                }
                            }
                        }

                        if (!found)
                        {
                            Console.WriteLine("Экспонатов указанного автора не найдено.");
                        }
                        // № 4
                        // В журнале сохраняем результат завершённого поиска.
                        logger.Log("Поиск по автору завершён", found ? "есть совпадения" : "совпадений нет");
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
                        // № 4
                        // Рассчитываем сумму один раз для вывода и записи в журнал.
                        decimal ticketCost = ticket.CalculateCost();
                        Console.WriteLine("Итого к олате: " + ticketCost + " руб.");
                        logger.Log("Стоимость рассчитанных билетов, руб.", ticketCost);
                        break;

                    case "5":
                        decimal totalSum = 0;
                        int totalPiecesCount = 0;

                        for (int i = 0; i < exhibition.HallCount; i++)
                        {
                            Hall h = exhibition.Halls[i];
                            foreach (ArtPiece piece in h.Pieces)
                            {
                                totalSum += piece.Price;
                                totalPiecesCount++;
                            }
                        }

                        Console.WriteLine("--- Финансовый отчет галереи ---");
                        Console.WriteLine("Всего экспонатов на выставке: " + totalPiecesCount + " шт.");
                        Console.WriteLine("Общая оценочная стоимость фондов: " + totalSum + " руб.");
                        // № 4
                        // Сохраняем итоговую сумму после формирования отчёта.
                        logger.Log("Финансовый отчёт сформирован. Стоимость фондов, руб.", totalSum);
                        break;

                    case "6":
                        running = false;
                        Console.WriteLine("Программа завершена.");
                        // № 4
                        // Последняя запись отмечает штатный выход через меню.
                        logger.Log("Завершение приложения", "Выход из меню");
                        break;
                    
                    case "7":
                        ShowGenericCollectionDemo(firstPainting);
                        break;


                    case "8":
                        // № 4
                        // Передаём тот же логгер для записи результата сортировки.
                        ShowSortingDemo(hall1, logger);
                        break;

                    default:
                        Console.WriteLine("Неверный пункт меню.");
                        break;
                }
            }
        }
    }
}
