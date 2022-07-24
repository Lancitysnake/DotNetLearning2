using System.Text.Json;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Text;
using System.Collections.Generic;





class Program
{
    public class AllObjects
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int FactoryId { get; set; }
        public int UnitId { get; set; }
        public int Volume { get; set; }
        public int MaxVolume { get; set; }
    }
    public class Unit
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int FactoryId { get; set; }
    }


    public class Factory
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
    }


    public class Tank
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public int UnitId { get; set; }
        public int Volume { get; set; }
        public int MaxVolume { get; set; }
    }
    static void Main(string[] args)
    {
        var allObjects = GetAllObjects();

        var tanks = GetTanks();

        var units = GetUnits();

        var factories = GetFactories();
        Console.WriteLine($"Количество резервуаров: {tanks.Length}, установок: {units.Length}");

        var foundUnit = FindUnit(units, tanks, "Резервуар 2");

        var factory = FindFactory(factories, foundUnit);

        Console.WriteLine($"Резервуар 2 принадлежит установке {foundUnit.Name} и заводу {factory.Name}");

        var totalVolume = GetTotalVolume(tanks);
        Console.WriteLine($"Общий объем резервуаров: {totalVolume}");

        List<string> ObjectsNames = new List<string>
             {
               "НПЗ№1",
               "НПЗ№2",
               "АВТ-10",
               "АВТ-6",
               "ГФУ-2",
               "Резервуар 256",
               "Резервуар 57",
               "Резервуар 35",
               "Дополнительный резервуар 24",
               "Резервуар 2",
               "Резервуар 1",
               "Надземный - вертикальный",
               "Надземный - горизонтальный",
               "Подземный - двустенный",
               "Подводный",
               "Газофракционирующая установка",
               "Атмосферно-вакуумная трубчатка",
               "Первый нефтеперерабатывающий завод",
               "Второй нефтеперерабатывающий завод"
             };

        /* var searching = Console.ReadLine();
         // Поиск информации по описанию объекта //
         FindInfo(factories, units, tanks, searching); 
         Console.WriteLine();*/
        Console.WriteLine($"{tanks[0].Name}, {tanks[0].Description}, {tanks[0].MaxVolume}");
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            IgnoreReadOnlyProperties = true
        };
        string jsonAllObjects = JsonSerializer.Serialize(allObjects, options);
        while (true)
        {
            Console.WriteLine("\nВведите название или описание искомого объекта, либо Stop, что бы прекратить поиск :");
            string search = Console.ReadLine();
            if (ObjectsNames.Contains(search))
            {
                string pathAllObjects = "C:/Users/Alex/source/repos/Lancitysnake/DotNetLearning2/allObjects.json";
                File.WriteAllText(pathAllObjects, jsonAllObjects);
                JsonSearch(pathAllObjects, search);
            }
            else if (search == "stop" || search == "Stop")
                break;
            else Console.WriteLine("\nДанный объект не найден, убедитесь в правильности написания названия объекта!");
        }

    }



    public static void JsonSearch(string path, string search)
    {
        var fb = new WebClient() { Encoding = Encoding.UTF8 }.DownloadString(path);
        var list = JsonSerializer.Deserialize<List<AllObjects>>(fb);
        foreach (var obj in list)
            if (obj.Name == search || obj.Description == search)
        // AllObjects result = list.Find(x => x.Name.ToString() == search);
                    PrintInfo(obj, path);        
    }

    public static void PrintInfo(AllObjects Object, string path)
    {
        Console.WriteLine("\nПо вашему запросу найден(a) " + Object.Name);
        if (Object.FactoryId == 0 && Object.UnitId == 0)
            Console.WriteLine($" это {Object.Description}");
        else if (Object.FactoryId != 0)
        {
            Console.WriteLine($"это {Object.Description}");
            var fb = new WebClient() { Encoding = Encoding.UTF8 }.DownloadString(path);
            var list = JsonSerializer.Deserialize<List<AllObjects>>(fb);

            AllObjects result = list.Find(x => x.Id == Object.FactoryId && x.FactoryId == 0 && x.UnitId == 0);
            Console.Write($" утановленна на объекте {result.Name} - {result.Description}");
        }
        else
        {
            Console.Write($" типа установки {Object.Description}. Заполненость хранилища {Object.Volume} кб.м. " +
                $"\n  максимальная ёмкость - {Object.MaxVolume} ");
            var fb = new WebClient() { Encoding = Encoding.UTF8 }.DownloadString(path);
            var list = JsonSerializer.Deserialize<List<AllObjects>>(fb);

            AllObjects result = list.Find(x => x.Id == Object.UnitId && x.FactoryId != 0);
            Console.Write($" утановленна на объекте {result.Name} - {result.Description}");
        }
    }

    /* public static void FindInfo(Factory[] factories, Unit[] units, Tank[] tanks, string description)  // Поиск информации по описанию объекта //
     {
         for (int i = 0; i < factories.Length; i++)
             if (factories[i].Description == description)
             {
                 Console.WriteLine($"По вашему запросу найден {factories[i].Name}, это {factories[i].Description}," +
                     $"\n ID № {factories[i].Id}.");
                 for (int j = 0; j < units.Length; j++)
                 {
                     int Volume = 0;
                     int maxVolume = 0;
                     if (units[j].FactoryId == factories[i].Id)
                     {

                         Console.WriteLine($"Оснащен установкой {units[j].Name} - {units[j].Description} cообщающейся с хранилищем: ");
                         for (int k = 0; k < tanks.Length; k++)
                             if (tanks[k].UnitId == units[j].Id)
                             {
                                 maxVolume += tanks[k].MaxVolume;
                                 Volume += tanks[k].Volume;
                                 Console.Write($"{tanks[k].Name}, типа установки {tanks[k].Description}, наполненностью в {tanks[k].Volume} кубических метров" +
                                     $"\n и общим объемом в {tanks[k].MaxVolume} кубических метров \n");
                             }
                     }
                     if (Volume != 0)
                         Console.WriteLine($"Общая наполненность хранилищ - {Volume} кб.м., максимальный объем - {maxVolume} ");
                 }
             }
         for (int i = 0; i < units.Length; i++)
         {
             int Volume = 0;
             int maxVolume = 0;
             if (units[i].Description == description)
             {
                 Console.WriteLine($"По вашему запросу найдена установка {units[i].Name} - {units[i].Description} cообщающаяся с хранилищем: ");
                 for (int k = 0; k < tanks.Length; k++)
                     if (tanks[k].UnitId == units[i].Id)
                     {
                         maxVolume += tanks[k].MaxVolume;
                         Volume += tanks[k].Volume;
                         Console.Write($"{tanks[k].Name}, типа установки {tanks[k].Description}, наполненностью в {tanks[k].Volume} кубических метров" +
                             $"\n и общим объемом в {tanks[k].MaxVolume} кубических метров \n");
                     }

             }
             if (Volume != 0)
                 Console.WriteLine($"Общая наполненность хранилищ - {Volume} кб.м., максимальный объем - {maxVolume} ");
         }

         for (int i = 0; i < tanks.Length; i++)
         {

             if (tanks[i].Description == description)
             {
                 Console.WriteLine($"По вашему запросу найден {tanks[i].Name} типа установки {tanks[i].Description}" +
                     $"\n наполненостью в {tanks[i].Volume}кб.м, и общим объемом в {tanks[i].MaxVolume} кб.м.");
             }
         }

     } */
    // реализуйте этот метод, чтобы он возвращал массив резервуаров, согласно приложенным таблицам
    // можно использовать создание объектов прямо в C# коде через new, или читать из файла (на своё усмотрение)
    public static AllObjects[] GetAllObjects()
    {
        AllObjects[] allObjects = new AllObjects[]
        {
                new AllObjects  {Description = "Надземный - вертикальный", Name = "Резервуар 1", MaxVolume = 2000, Volume = 1500, Id = 1, UnitId = 1},
                new AllObjects  {Description = "Надземный - горизонтальный", Name = "Резервуар 2", MaxVolume = 3000, Volume = 2500, Id = 2, UnitId = 1 },
                new AllObjects  {Description = "Надземный - горизонтальный", Name = "Дополнительный резервуар 24", MaxVolume = 3000, Volume = 3000, Id = 3, UnitId = 2 },
                new AllObjects  {Description = "Надземный - вертикальный", Name = "Резервуар 35", MaxVolume = 3000, Volume = 3000, Id = 4, UnitId = 2 },
                new AllObjects  {Description = "Подземный - двустенный", Name = "Резервуар 47", MaxVolume = 5000, Volume = 4000, Id = 5, UnitId = 2 },
                new AllObjects  {Description = "Подводный", Name = "Резервуар 256", MaxVolume = 500, Volume = 500, Id = 6, UnitId = 3 },
                new AllObjects {Description = "Газофракционирующая установка", Name = "ГФУ-2", Id = 1, FactoryId = 1},
                new AllObjects {Description = "Атмосферно-вакуумная трубчатка", Name = "АВТ-6", Id = 2, FactoryId = 1 },
                new AllObjects {Description = "Атмосферно-вакуумная трубчатка", Name = "АВТ-10", Id = 3, FactoryId = 2 },
                new AllObjects {Description = "Первый нефтеперерабатывающий завод", Name = "НПЗ№1", Id = 1},
                new AllObjects {Description = "Второй нефтеперерабатывающий завод", Name = "НПЗ№2", Id = 2}

        };
        return allObjects;
    }
    public static Tank[] GetTanks()
    {


        Tank[] tanks = new Tank[]
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
    // реализуйте этот метод, чтобы он возвращал массив установок, согласно приложенным таблицам
    public static Unit[] GetUnits()
    {
        Unit[] units = new Unit[]
        {
            new Unit {Description = "Газофракционирующая установка", Name = "ГФУ-2", Id = 1, FactoryId = 1},
            new Unit {Description = "Атмосферно-вакуумная трубчатка", Name = "АВТ-6", Id = 2, FactoryId = 1 },
            new Unit {Description = "Атмосферно-вакуумная трубчатка", Name = "АВТ-10", Id = 3, FactoryId = 2 },
        };
        return units;
    }

    // реализуйте этот метод, чтобы он возвращал массив заводов, согласно приложенным таблицам
    public static Factory[] GetFactories()
    {
        Factory[] factories = new Factory[]
        {
           new Factory {Description = "Первый нефтеперерабатывающий завод", Name = "НПЗ№1", Id = 1},
           new Factory {Description = "Второй нефтеперерабатывающий завод", Name = "НПЗ№2", Id = 2}
        };
        return factories;

    }

    // реализуйте этот метод, чтобы он возвращал установку (Unit), которой
    // принадлежит резервуар (Tank), найденный в массиве резервуаров по имени
    // учтите, что по заданному имени может быть не найден резервуар
    public static Unit FindUnit(Unit[] units, Tank[] tanks, string unitName)
    {
        var unitId = 0;
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].Name == unitName)
            {
                unitId = tanks[i].UnitId;
                break;
            }
        }
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i].Id == unitId)
            {
                unitId = i;
                break;
            }
        }
        return units[unitId];
    }

    // реализуйте этот метод, чтобы он возвращал объект завода, соответствующий установке
    public static Factory FindFactory(Factory[] factories, Unit unit)
    {
        var factoryId = 0;
        for (int i = 0; i < factories.Length; i++)
        {
            if (factories[i].Id == unit.FactoryId)
                factoryId = i;
        }
        return factories[factoryId];
    }

    // реализуйте этот метод, чтобы он возвращал суммарный объем резервуаров в массиве
    public static int GetTotalVolume(Tank[] units)
    {
        var totalVolume = 0;
        for (int i = 0; i < units.Length; i++)
            totalVolume += units[i].MaxVolume;
        return totalVolume;
    }
}
