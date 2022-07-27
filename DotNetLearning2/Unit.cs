partial class Program
{
    public class Unit
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int FactoryId { get; set; }
        public static List<Unit> GetUnits()
        {
            List<Unit> units = new List<Unit>
        {
            new Unit {Description = "Газофракционирующая установка", Name = "ГФУ-2", Id = 1, FactoryId = 1},
            new Unit {Description = "Атмосферно-вакуумная трубчатка", Name = "АВТ-6", Id = 2, FactoryId = 1 },
            new Unit {Description = "Атмосферно-вакуумная трубчатка", Name = "АВТ-10", Id = 3, FactoryId = 2 }
        };
            return units;
        }
        public static void CreateNewUnit(string description, string name, int id, int factoryId, List<EntitesStore> allObjects)
        => allObjects[0].Units.Add(new Unit { Description = description, Name = name, Id = id, FactoryId = factoryId });

        public static void UpdateUnit(Unit unit, int numb, List<string> Names)
        {
            Console.WriteLine("\nВведите новое название :");
            string newName = Console.ReadLine();
            if (newName != null && newName != "")
                unit.Name = newName;
            Console.WriteLine("\nВведите новое описание :");
            string newDescription = Console.ReadLine();
            if (newDescription != null && newDescription != "")
                unit.Description = newDescription;
            Console.WriteLine($"\nВведите новый идентификационный номер, если номер не изменяется, введите текущий ({unit.Id})");
            unit.Id = int.Parse(Console.ReadLine());
            Console.WriteLine($"\nВведите новый идентификационный номер завода, к которому относится данная установка," +
                $"\n если номер не изменяется, введите текущий ({unit.FactoryId})");
            unit.FactoryId = int.Parse(Console.ReadLine());
            Console.WriteLine("\nДанные успешно обновлены!");
        }

        public static Unit FindUnit(List<Unit> units, List<Tank> tanks, string unitName)
        {
            var unitId = 0;
            var unitFind = 0;

            for (int i = 0; i < tanks.Count; i++)
            {
                if (tanks[i].Name == unitName)
                {
                    unitFind = tanks[i].UnitId;
                    break;
                }
            }
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Id == unitFind)
                {
                    unitId = i;
                    break;
                }

            }

            return units[unitId];
        }
        public static void UpdateUnit(Unit unit)
        {
            Console.WriteLine("\n Введите новое название :");
            string newInfo = Console.ReadLine();
            if (!string.IsNullOrEmpty(newInfo))
                unit.Name = newInfo;
            Console.WriteLine("\n Введите новое описание :");
            newInfo = Console.ReadLine();
            if (!string.IsNullOrEmpty(newInfo))
                unit.Description = newInfo;
            Console.WriteLine("\n Введите новый идентификационный номер :");
            newInfo = Console.ReadLine();
            int number;
            bool success = int.TryParse(newInfo, out number);
            if (success)
                unit.Id = number;
            else
            {   
                Console.WriteLine($"\n Неверный формат ввода, установлено ID по умолчанию.({unit.Id})");
            }
            Console.WriteLine("\n Введите новый идентификационный номер завода на котором находится установка :");
            newInfo = Console.ReadLine();
            success = int.TryParse(newInfo, out number);
            if (success)
                unit.FactoryId = number;
            else
            {
                Console.WriteLine($"\n Неверный формат ввода, установлено ID по умолчанию.({unit.FactoryId})");
            }

        }
    }
}
