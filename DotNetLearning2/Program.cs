using System.Text.Json;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Text;
using System.Collections.Generic;
using OfficeOpenXml;





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
        string pathAllObjects = "C:/Users/Alex/source/repos/Lancitysnake/DotNetLearning2/allObjects.json";
        string pathNamesOfObjects = "C:/Users/Alex/source/repos/Lancitysnake/DotNetLearning2/NamesOfObjects.json";
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
        var fb = new WebClient() { Encoding = Encoding.UTF8 }.DownloadString(pathNamesOfObjects);
        var ObjectsNames = JsonSerializer.Deserialize<List<string>>(fb);
       

        Console.WriteLine($"{tanks[0].Name}, {tanks[0].Description}, {tanks[0].MaxVolume}");

        
        int indexOfObject;
        string nameForSearch;
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            IgnoreReadOnlyProperties = true
        };
        

        while (true)
        {

            Console.WriteLine("\nВведите номер команды:" +
            "\n 1 - Редактировать объект." +
            "\n 2 - Удалить объект." +
            "\n 3 - Найти объект." +
            "\n 4 - Добавить новый." +
            "\n 5 - Список всех объектов." +
            "\n 6 - Выход из программы.");

            string nextUserAnswer = Console.ReadLine();
            if (nextUserAnswer == "5")
                foreach (var obj in ObjectsNames)
                    Console.WriteLine("\n" + obj);

            if (nextUserAnswer == "3")
            {
                Console.WriteLine("Введите название объекта :");
                nameForSearch = Console.ReadLine();
                if (ObjectsNames.Contains(nameForSearch))
                    indexOfObject = JsonSearchAndPrint(pathAllObjects, nameForSearch);
                else Console.WriteLine("\nДанный объект не найден, убедитесь в правильности написания названия объекта!");
            }

            else if (nextUserAnswer == "2")
            {
                Console.WriteLine("Введите название объекта :");
                nameForSearch = Console.ReadLine();
                if (ObjectsNames.Contains(nameForSearch))
                {
                    indexOfObject = JsonSearch(pathAllObjects, nameForSearch);
                    ObjectsNames.Remove(nameForSearch);
                    DeleteObject(allObjects, indexOfObject);
                }
                else Console.WriteLine("\nДанный объект не найден, убедитесь в правильности написания названия объекта!");
            }
            else if (nextUserAnswer == "6")
                return;
            else if (nextUserAnswer == "4")
            {
                WhatTheObjectUNeed(allObjects);
                ObjectsNames.Add(allObjects[allObjects.Count - 1].Name);

            }
            else if (nextUserAnswer == "1")
            {
                Console.WriteLine("Введите название объекта :");
                nameForSearch = Console.ReadLine();
                if (ObjectsNames.Contains(nameForSearch))
                {
                    indexOfObject = JsonSearch(pathAllObjects, nameForSearch);
                    if (allObjects[indexOfObject].FactoryId == 0 && allObjects[indexOfObject].UnitId == 0)
                        UpdateFactory(allObjects, indexOfObject,ObjectsNames);
                    else if (allObjects[indexOfObject].FactoryId != 0)
                        UpdateUnit(allObjects, indexOfObject, ObjectsNames);
                    else
                        UpdateTank(allObjects, indexOfObject, ObjectsNames);
                }
                else
                    Console.WriteLine("\nДанный объект не найден, убедитесь в правильности написания названия объекта!");
            }


        }
        string jsonAllObjects = JsonSerializer.Serialize(allObjects, options);
        string jsonNamesOfObjects = JsonSerializer.Serialize(ObjectsNames, options);
        File.WriteAllText(pathAllObjects, jsonAllObjects);
        File.WriteAllText(pathNamesOfObjects, jsonNamesOfObjects);


    }
    public static void WhatTheObjectUNeed(List<AllObjects> allObjects)
    {
        int countOfObjects = allObjects.Count;
        string name, description;
        int id, factoryId, unitId, volume, maxVolume;
        Console.WriteLine("\n Выберите тип объекта :" +
            "\n1 - Завод / Factory" +
            "\n2 - Установка / Unit" +
            "\n3 - Резервуар / Tank");
        string answer = (Console.ReadLine());

        Console.WriteLine("\nВведите название : ");
        name = Console.ReadLine();
        Console.WriteLine("\nВведите описание : ");
        description = Console.ReadLine();
        Console.WriteLine("\nВведите идентификационный номер (цифры!) : ");
        id = int.Parse(Console.ReadLine());

        if (answer == "1")
        {
            CreateNewFactory(description, name, id, allObjects);
            PrintObjectCreated(countOfObjects, allObjects);
        }
        else if (answer == "2")
        {
            Console.WriteLine("\nВведите идентификационный номер завода, которому принадлежит данная утсановка : ");
            factoryId = int.Parse(Console.ReadLine());
            CreateNewUnit(description, name, id, factoryId, allObjects);
            PrintObjectCreated(countOfObjects, allObjects);
        }
        else
        {
            Console.WriteLine("\nВведите идентификационный номер установки, которой принадлежит данный резервуар :");
            unitId = int.Parse(Console.ReadLine());
            Console.WriteLine("\nВведите максимальный объем резервуара : ");
            maxVolume = int.Parse(Console.ReadLine());
            Console.WriteLine("\nВведите текущую наполненность резервуара :");
            volume = int.Parse(Console.ReadLine());
            CreateNewTank(description, name, id, allObjects, volume, maxVolume, unitId);
            PrintObjectCreated(countOfObjects, allObjects);
        }

    }

    public static void CreateNewFactory(string description, string name, int id, List<AllObjects> allObjects)
        => allObjects.Add(new AllObjects { Description = description, Name = name, Id = id });
    public static void CreateNewUnit(string description, string name, int id, int factoryId, List<AllObjects> allObjects)
        => allObjects.Add(new AllObjects { Description = description, Name = name, Id = id, FactoryId = factoryId });
    public static void CreateNewTank(string description, string name, int id, List<AllObjects> allObjects, int volume, int maxVolume, int unitId)
        => allObjects.Add(new AllObjects { Description = description, Name = name, Id = id, Volume = volume, MaxVolume = maxVolume, FactoryId = unitId });
    public static void PrintObjectCreated(int count, List<AllObjects> allObjects)
    {
        if (count < allObjects.Count)
            Console.WriteLine("\nОбъект успешно добавлен");
    }

    public static void DeleteObject(List<AllObjects> allObjects, int numb) => allObjects.Remove(allObjects[numb]);



    public static void UpdateFactory(List<AllObjects> objects, int numb, List<string>Names)
    {
        Console.WriteLine("\nВведите новое название :");
        string newName = Console.ReadLine();
        if (newName != null && newName != "")
        {
            int nameNumb = Names.IndexOf(objects[numb].Name);
            Names[nameNumb] = newName;
            objects[numb].Name = newName;
        }
        Console.WriteLine("\nВведите новое описание :");
        string newDescription = Console.ReadLine();
        if (newDescription != null && newDescription != "")
            objects[numb].Description = newDescription;
        Console.WriteLine($"\nВведите новый идентификационный номер, если номер не изменяется введите старый ({objects[numb].Id}");
        objects[numb].Id = int.Parse(Console.ReadLine());
        Console.WriteLine("\nДанные успешно обновлены!");
    }

    public static void UpdateUnit(List<AllObjects> objects, int numb, List<string> Names)
    {
        Console.WriteLine("\nВведите новое название :");
        string newName = Console.ReadLine();
        if (newName != null && newName != "")
            objects[numb].Name = newName;
        Console.WriteLine("\nВведите новое описание :");
        string newDescription = Console.ReadLine();
        if (newDescription != null && newDescription != "")
            objects[numb].Description = newDescription;
        Console.WriteLine($"\nВведите новый идентификационный номер, если номер не изменяется, введите текущий ({objects[numb].Id})");
        objects[numb].Id = int.Parse(Console.ReadLine());
        Console.WriteLine($"\nВведите новый идентификационный номер завода, к которому относится данная установка," +
            $"\n если номер не изменяется, введите текущий ({objects[numb].FactoryId})");
        objects[numb].FactoryId = int.Parse(Console.ReadLine());
        Console.WriteLine("\nДанные успешно обновлены!");
    }

    public static void UpdateTank(List<AllObjects> objects, int numb, List<string> Names)
    {
        Console.WriteLine("\nВведите новое название :");
        string newName = Console.ReadLine();
        if (newName != null && newName != "")
            objects[numb].Name = newName;
        Console.WriteLine("\nВведите новое описание :");
        string newDescription = Console.ReadLine();
        if (newDescription != null && newDescription != "")
            objects[numb].Description = newDescription;
        Console.WriteLine($"\nВведите новый идентификационный номер, если номер не изменяется, введите текущий ({objects[numb].Id})");
        objects[numb].Id = int.Parse(Console.ReadLine());
        Console.WriteLine($"\nВведите новый идентификационный номер установки, к которой относится данный резервуар," +
            $"\n если номер не изменяется, введите текущий ({objects[numb].UnitId})");
        objects[numb].UnitId = int.Parse(Console.ReadLine());
        Console.WriteLine($"\nВведите новый максимальный объем резервуара, если объем не изменяется, введите текущий ({objects[numb].MaxVolume})");
        objects[numb].MaxVolume = int.Parse(Console.ReadLine());
        Console.WriteLine($"\nВведите заполненность резервуара, если объем не изменился, введите текущий ({objects[numb].Volume})");
        objects[numb].Volume = int.Parse(Console.ReadLine());
        Console.WriteLine("\nДанные успешно обновлены!");
    }

    public static int JsonSearch(string path, string search)
    {
        int a = 0;
        var fb = new WebClient() { Encoding = Encoding.UTF8 }.DownloadString(path);
        var list = JsonSerializer.Deserialize<List<AllObjects>>(fb);
        foreach (var obj in list)
            if (obj.Name == search)
            {
                a = list.IndexOf(obj);
            }
        return a;
    }
    public static int JsonSearchAndPrint(string path, string search)
    {
        int a = 0;
        var fb = new WebClient() { Encoding = Encoding.UTF8 }.DownloadString(path);
        var list = JsonSerializer.Deserialize<List<AllObjects>>(fb);
        foreach (var obj in list)
            if (obj.Name == search)
            {
                PrintInfo(obj, path);
                a = list.IndexOf(obj);
            }
        return a;
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
    public static List<AllObjects> GetAllObjects()
    {
        List<AllObjects> allObjects = new List<AllObjects>
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
    public static Factory[] GetFactories()
    {
        Factory[] factories = new Factory[]
        {
           new Factory {Description = "Первый нефтеперерабатывающий завод", Name = "НПЗ№1", Id = 1},
           new Factory {Description = "Второй нефтеперерабатывающий завод", Name = "НПЗ№2", Id = 2}
        };
        return factories;

    }
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
    public static int GetTotalVolume(Tank[] units)
    {
        var totalVolume = 0;
        for (int i = 0; i < units.Length; i++)
            totalVolume += units[i].MaxVolume;
        return totalVolume;
    }
}
