
class Program
{   

    public class SomethingForGitRefresh
    {
        string someString;
        int someInt;
        double someDouble;
        byte[] someBytes;
    }
    public class Unit
    {
        public string description;
        public string name;
        public int id;
        public int factoryId;
    }


    public class Factory
    {
        public string description;
        public string name;
        public int id;
    }


    public class Tank
    {
        public string name;
        public string description;
        public int id;
        public int unitId;
        public int volume;
        public int maxVolume;
    }
    static void Main(string[] args)
    {
        var tanks = GetTanks();

        var units = GetUnits();

        var factories = GetFactories();
        Console.WriteLine($"Количество резервуаров: {tanks.Length}, установок: {units.Length}");

        var foundUnit = FindUnit(units, tanks, "Резервуар 2");

        var factory = FindFactory(factories, foundUnit);

        Console.WriteLine($"Резервуар 2 принадлежит установке {foundUnit.name} и заводу {factory.name}");

        var totalVolume = GetTotalVolume(tanks);
        Console.WriteLine($"Общий объем резервуаров: {totalVolume}");

        var searching = Console.ReadLine();
        // Поиск информации по описанию объекта //
        FindInfo(factories, units, tanks, searching); 
        Console.WriteLine();
    }
   
    public static void FindInfo(Factory[] factories, Unit[] units, Tank[] tanks, string description)  // Поиск информации по описанию объекта //
    {
        for (int i = 0; i < factories.Length; i++)
            if (factories[i].description == description)
            {
                Console.WriteLine($"По вашему запросу найден {factories[i].name}, это {factories[i].description}," +
                    $"\n ID № {factories[i].id}.");
                for (int j = 0; j < units.Length; j++)
                {
                    int Volume = 0;
                    int maxVolume = 0;
                    if (units[j].factoryId == factories[i].id)
                    {

                        Console.WriteLine($"Оснащен установкой {units[j].name} - {units[j].description} cообщающейся с хранилищем: ");
                        for (int k = 0; k < tanks.Length; k++)
                            if (tanks[k].unitId == units[j].id)
                            {
                                maxVolume += tanks[k].maxVolume;
                                Volume += tanks[k].volume;
                                Console.Write($"{tanks[k].name}, типа установки {tanks[k].description}, наполненностью в {tanks[k].volume} кубических метров" +
                                    $"\n и общим объемом в {tanks[k].maxVolume} кубических метров \n");
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
            if (units[i].description == description)
            {
                Console.WriteLine($"По вашему запросу найдена установка {units[i].name} - {units[i].description} cообщающаяся с хранилищем: ");
                for (int k = 0; k < tanks.Length; k++)
                    if (tanks[k].unitId == units[i].id)
                    {
                        maxVolume += tanks[k].maxVolume;
                        Volume += tanks[k].volume;
                        Console.Write($"{tanks[k].name}, типа установки {tanks[k].description}, наполненностью в {tanks[k].volume} кубических метров" +
                            $"\n и общим объемом в {tanks[k].maxVolume} кубических метров \n");
                    }

            }
            if (Volume != 0)
                Console.WriteLine($"Общая наполненность хранилищ - {Volume} кб.м., максимальный объем - {maxVolume} ");
        }

        for (int i = 0; i < tanks.Length; i++)
        {

            if (tanks[i].description == description)
            {
                Console.WriteLine($"По вашему запросу найден {tanks[i].name} типа установки {tanks[i].description}" +
                    $"\n наполненостью в {tanks[i].volume}кб.м, и общим объемом в {tanks[i].maxVolume} кб.м.");
            }
        }

    } 
    // реализуйте этот метод, чтобы он возвращал массив резервуаров, согласно приложенным таблицам
    // можно использовать создание объектов прямо в C# коде через new, или читать из файла (на своё усмотрение)

    public static Tank[] GetTanks()
    {


        Tank[] tanks = new Tank[]
        {
                new Tank  {description = "Надземный - вертикальный", name = "Резервуар 1", maxVolume = 2000, volume = 1500, id = 1, unitId = 1},
                new Tank  {description = "Надземный - горизонтальный", name = "Резервуар 2", maxVolume = 3000, volume = 2500, id = 2, unitId = 1 },
                new Tank  {description = "Надземный - горизонтальный", name = "Дополнительный резервуар 24", maxVolume = 3000, volume = 3000, id = 3, unitId = 2 },
                new Tank  {description = "Надземный - вертикальный", name = "Резервуар 35", maxVolume = 3000, volume = 3000, id = 4, unitId = 2 },
                new Tank  {description = "Подземный - двустенный", name = "Резервуар 47", maxVolume = 5000, volume = 4000, id = 5, unitId = 2 },
                new Tank  {description = "Подводный", name = "Резервуар 256", maxVolume = 500, volume = 500, id = 6, unitId = 3 }
        };
        return tanks;
    }
    // реализуйте этот метод, чтобы он возвращал массив установок, согласно приложенным таблицам
    public static Unit[] GetUnits()
    {
        Unit[] units = new Unit[]
        {
            new Unit {description = "Газофракционирующая установка", name = "ГФУ-2", id = 1, factoryId = 1},
            new Unit {description = "Атмосферно-вакуумная трубчатка", name = "АВТ-6", id = 2, factoryId = 1 },
            new Unit {description = "Атмосферно-вакуумная трубчатка", name = "АВТ-10", id = 3, factoryId = 2 },
        };
        return units;
    }

    // реализуйте этот метод, чтобы он возвращал массив заводов, согласно приложенным таблицам
    public static Factory[] GetFactories()
    {
        Factory[] factories = new Factory[]
        {
           new Factory {description = "Первый нефтеперерабатывающий завод", name = "НПЗ№1", id = 1},
           new Factory {description = "Второй нефтеперерабатывающий завод", name = "НПЗ№2", id = 2}
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
            if (tanks[i].name == unitName)
            {
                unitId = tanks[i].unitId;
                break;
            }
        }
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i].id == unitId)
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
            if (factories[i].id == unit.factoryId)
                factoryId = i;
        }
        return factories[factoryId];
    }

    // реализуйте этот метод, чтобы он возвращал суммарный объем резервуаров в массиве
    public static int GetTotalVolume(Tank[] units)
    {
        var totalVolume = 0;
        for (int i = 0; i < units.Length; i++)
            totalVolume += units[i].maxVolume;
        return totalVolume;
    }
}
