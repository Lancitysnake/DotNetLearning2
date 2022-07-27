partial class Program
{
    public class EntitesStore
    {

        public List<Unit>? Units { get; set; }
        public List<Tank>? Tanks { get; set; }
        public List<Factory>? Factorys { get; set; }



        public static void DeleteObject(List<EntitesStore> objects, byte numbOfObject, string search)
        {
            if (numbOfObject == 1)
            {
                var foundFactory = objects[0].Factorys.FirstOrDefault(p => p.Name == search);
                objects[0].Factorys.Remove(foundFactory);
            }
            else if (numbOfObject == 2)
            {
                var foundUnit = objects[0].Units.FirstOrDefault(p => p.Name == search);
                objects[0].Units.Remove(foundUnit);
            }
            else
            {
                var foundTank = objects[0].Tanks.FirstOrDefault(p => p.Name == search);
                objects[0].Tanks.Remove(foundTank);
            }
        }

        public static void PrintObjectCreated(int count, List<EntitesStore> allObjects)
        {
            if (count < allObjects.Count)
                Console.WriteLine("\nОбъект успешно добавлен");
        }

        public static (byte, string) FindObject(List<EntitesStore> objects, bool printInfo)
        {
            byte numbOfObj = 0;
            Console.WriteLine("\nВведите название объекта : ");
            string search = Console.ReadLine();
            var foundFactory = objects[0].Factorys.FirstOrDefault(p => p.Name == search);
            if (foundFactory != null)
                numbOfObj = 1;
            var foundUnit = objects[0].Units.FirstOrDefault(p => p.Name == search);
            if (foundUnit != null)
                numbOfObj = 2;
            var foundTank = objects[0].Tanks.FirstOrDefault(p => p.Name == search);
            if (foundTank != null)
                numbOfObj = 3;
            if (printInfo)
            {
                if (foundFactory != null)
                    Console.WriteLine($"\nНайден объект : {foundFactory.Name} это {foundFactory.Description}. Идентификационный номер {foundFactory.Id}");
                else if (foundUnit != null)
                {
                    foundFactory = objects[0].Factorys.FirstOrDefault(p => p.Id == foundUnit.FactoryId);
                    Console.WriteLine($"\nНайден объект : {foundUnit.Name} это {foundUnit.Description}. Идентификационный номер {foundUnit.Id}, ");
                        if (foundFactory != null)
                        Console.Write($" установленa на заводе {foundFactory.Name} ");
                    else Console.Write(" Завод, владелец установки не найден");
                }
                else if (foundTank != null)
                {
                    foundUnit = objects[0].Units.FirstOrDefault(p => p.Id == foundTank.UnitId);
                    Console.WriteLine($"\nНайден объект : {foundTank.Name} типа размещения {foundTank.Description}. Идентификационный номер {foundTank.Id}," +
                        $"\n заполненость хранилища : {foundTank.Volume}, максимальным объемом {foundTank.MaxVolume} ");
                    if (foundUnit != null)
                        Console.Write($", сообщается с установкой {foundUnit.Name}");
                    else Console.Write(" Сообщающихся с резервуаром установок не найдено!");
                }
                else Console.WriteLine("\nК сожалению объект не найден! Проверьте правильность написания названия объекта!");
                
            }
            return (numbOfObj, search);
        }
        public static void UpdateObject (List<EntitesStore> objects, byte numbOfObject, string search)
        {
            if (numbOfObject == 1) 
            {
                var foundFactory = objects[0].Factorys.FirstOrDefault(p => p.Name == search);
                Factory.UpdateFactory(foundFactory);
            }
            else if (numbOfObject == 2)
            {
                var foundUnit = objects[0].Units.FirstOrDefault(p => p.Name == search);
                Unit.UpdateUnit(foundUnit);
            }
            else 
            {
                var foundTank = objects[0].Tanks.FirstOrDefault(p => p.Name == search);
                Tank.UpdateTank(foundTank);
            }

        }

       

       

        

        public static void AddObject(List<EntitesStore> allObjects)
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
                Factory.CreateNewFactory(description, name, id, allObjects);
                PrintObjectCreated(countOfObjects, allObjects);
            }
            else if (answer == "2")
            {
                Console.WriteLine("\nВведите идентификационный номер завода, которому принадлежит данная утсановка : ");
                factoryId = int.Parse(Console.ReadLine());
                Unit.CreateNewUnit(description, name, id, factoryId, allObjects);
                PrintObjectCreated(countOfObjects, allObjects);
            }
            else
            {
                Console.WriteLine("\nВведите идентификационный номер установки, которой принадлежит данный резервуар :");
                unitId = int.Parse(Console.ReadLine());
                Console.WriteLine("\nВведите максимальный объем резервуара (кб.м) : ");
                maxVolume = int.Parse(Console.ReadLine());
                Console.WriteLine("\nВведите текущую наполненность резервуара (кб.м):");
                volume = int.Parse(Console.ReadLine());
                Tank.CreateNewTank(description, name, id, allObjects, volume, maxVolume, unitId);
                PrintObjectCreated(countOfObjects, allObjects);
            }
        }
    }
}
