partial class Program
{
    public class Tank
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public int UnitId { get; set; }
        public int Volume { get; set; }
        public int MaxVolume { get; set; }

        public static int GetTotalVolume(List<Tank> units)
        {
            var totalVolume = 0;
            for (int i = 0; i < units.Count; i++)
                totalVolume += units[i].MaxVolume;
            return totalVolume;
        }

        public static void CreateNewTank(string description, string name, int id, List<EntitesStore> allObjects, int volume, int maxVolume, int unitId)
        => allObjects[0].Tanks.Add(new Tank { Description = description, Name = name, Id = id, Volume = volume, MaxVolume = maxVolume, UnitId = unitId });

        public static List<Tank> GetTanks()
        {
            List<Tank> tanks = new List<Tank>
        {
                new Tank  {Description = "Надземный - вертикальный", Name = "Резервуар 1", MaxVolume = 2000, Volume = 1500, Id = 1, UnitId = 1},
                new Tank  {Description = "Надземный - горизонтальный", Name = "Резервуар 2", MaxVolume = 3000, Volume = 2500, Id = 2, UnitId = 1 },
                new Tank  {Description = "Надземный - горизонтальный", Name = "Дополнительный резервуар 24", MaxVolume = 3000, Volume = 3000, Id = 3, UnitId = 2 },
                new Tank  {Description = "Надземный - вертикальный", Name = "Резервуар 35", MaxVolume = 3000, Volume = 3000, Id = 4, UnitId = 2 },
                new Tank  {Description = "Подземный - двустенный", Name = "Резервуар 47", MaxVolume = 5000, Volume = 4000, Id = 5, UnitId = 2 },
                new Tank  {Description = "Подводный", Name = "Резервуар 256", MaxVolume = 500, Volume = 500, Id = 6, UnitId = 3 }
        };
            return tanks;
        }

        public static void UpdateTank(Tank tank)
        {
            Console.WriteLine("\n Введите новое название :");
            string newInfo = Console.ReadLine();
            if (!string.IsNullOrEmpty(newInfo))
                tank.Name = newInfo;
            Console.WriteLine("\n Введите новое описание :");
            newInfo = Console.ReadLine();
            if (!string.IsNullOrEmpty(newInfo))
                tank.Description = newInfo;
            Console.WriteLine("\n Введите новый идентификационный номер :");
            newInfo = Console.ReadLine();
            int number;
            bool success = int.TryParse(newInfo, out number);
            if (success)
                tank.Id = number;
            else
            {                
                Console.WriteLine($"\n Неверный формат ввода, установлено ID по умолчанию.({tank.Id})");
            }
            Console.WriteLine("\n Введите новый идентификационный номер установки с которой сообщается данный резервуар :");
            newInfo = Console.ReadLine();
            success = int.TryParse(newInfo, out number);
            if (success)
                tank.UnitId = number;
            else
            {
                tank.UnitId = 0;
                Console.WriteLine("\n Неверный формат ввода, установлено ID по умолчанию.(0)");
            }
            Console.WriteLine("\n Укажите текущую наполненость резевуара (кб.м.) :");
            newInfo = Console.ReadLine();
            success = int.TryParse(newInfo, out number);
            if (success)
                tank.Volume = number;
            else
            {                
                Console.WriteLine();
            }
            Console.WriteLine("\n Укажите максимальную ёмкость резевуара (кб.м.) :");
            newInfo = Console.ReadLine();
            success = int.TryParse(newInfo, out number);
            if (success)
                tank.MaxVolume = number;
            else
            {
                Console.WriteLine($"\n Неверный формат ввода, установлено значение по умолчанию.({tank.MaxVolume})");
            }
        }

        public static void UpdateTank(Tank tank, int numb, List<string> Names)
        {
            Console.WriteLine("\nВведите новое название :");
            string newName = Console.ReadLine();
            if (newName != null && newName != "")
                tank.Name = newName;
            Console.WriteLine("\nВведите новое описание :");
            string newDescription = Console.ReadLine();
            if (newDescription != null && newDescription != "")
               tank.Description = newDescription;
            Console.WriteLine($"\nВведите новый идентификационный номер, если номер не изменяется, введите текущий ({tank.Id})");
            tank.Id = int.Parse(Console.ReadLine());
            Console.WriteLine($"\nВведите новый идентификационный номер установки, к которой относится данный резервуар," +
                $"\n если номер не изменяется, введите текущий ({tank.UnitId})");
            tank.UnitId = int.Parse(Console.ReadLine());
            Console.WriteLine($"\nВведите новый максимальный объем резервуара, если объем не изменяется, введите текущий ({tank.MaxVolume})");
            tank.MaxVolume = int.Parse(Console.ReadLine());
            Console.WriteLine($"\nВведите заполненность резервуара, если объем не изменился, введите текущий ({tank.Volume})");
            tank.Volume = int.Parse(Console.ReadLine());
            Console.WriteLine("\nДанные успешно обновлены!");
        }
    }
}
