partial class Program
{
    public class Factory
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public static List<Factory> GetFactories()
        {
            List<Factory> factories = new List<Factory>
        {
           new Factory {Description = "Первый нефтеперерабатывающий завод", Name = "НПЗ№1", Id = 1},
           new Factory {Description = "Второй нефтеперерабатывающий завод", Name = "НПЗ№2", Id = 2}
        };
            return factories;

        }

        public static void UpdateFactory(Factory factory, int numb, List<string> Names)
        {
            Console.WriteLine("\nВведите новое название :");
            string newName = Console.ReadLine();
            if (newName != null && newName != "")
            {
                int nameNumb = Names.IndexOf(factory.Name);
                Names[nameNumb] = newName;
                factory.Name = newName;
            }
            Console.WriteLine("\nВведите новое описание :");
            string newDescription = Console.ReadLine();
            if (newDescription != null && newDescription != "")
                factory.Description = newDescription;
            Console.WriteLine($"\nВведите новый идентификационный номер, если номер не изменяется введите старый ({factory.Id}");
            factory.Id = int.Parse(Console.ReadLine());
            Console.WriteLine("\nДанные успешно обновлены!");
        }
        public static void CreateNewFactory(string description, string name, int id, List<EntitesStore> allObjects)
        => allObjects[0].Factorys.Add(new Factory { Description = description, Name = name, Id = id });
        public static Factory FindFactory(List<Factory> factories, Unit unit)
        {
            var factoryId = 0;
            for (int i = 0; i < factories.Count; i++)
            {
                if (factories[i].Id == unit.FactoryId)
                    factoryId = i;
            }
            return factories[factoryId];
        }

        public static void UpdateFactory(Factory factory)
        {
            Console.WriteLine("\n Введите новое название :");
            string newInfo = Console.ReadLine();
            if (!string.IsNullOrEmpty(newInfo))
                factory.Name = newInfo;
            Console.WriteLine("\n Введите новое описание :");
            newInfo = Console.ReadLine();
            if (!string.IsNullOrEmpty(newInfo))
                factory.Description = newInfo;
            Console.WriteLine("\n Введите новый идентификационный номер :");
            newInfo = Console.ReadLine();
            int number;
            bool success = int.TryParse(newInfo, out number);
            if (success)
                factory.Id = number;
            else
            {
                Console.WriteLine($"\n Неверный формат ввода, установлено ID по умолчанию.({factory.Id})");
            }
        }
    }
}
