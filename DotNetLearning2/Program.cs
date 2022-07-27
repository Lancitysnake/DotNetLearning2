using System.Text.Json;
using System.Net;
using System.Text;
using System.Linq;

partial class Program
{

    static void Main(string[] args)
    {
        JsonSerDeser jsonSerDeser = new JsonSerDeser()
        {
            PathToAllObjects = "C:/Users/Alex/source/repos/Lancitysnake/DotNetLearning2/allObjects.json"            
        };

        var objects  = JsonSerDeser.Deserializ(jsonSerDeser);

        var tanks = Tank.GetTanks();

        var units = Unit.GetUnits();

        var factories = Factory.GetFactories();
        
        var _entitesStore = new EntitesStore() { Factorys = factories, Tanks = tanks, Units = units};

        List<EntitesStore> entitesStore = new List<EntitesStore>() { _entitesStore };
        

        var foundUnit = Unit.FindUnit(units, tanks, "Резервуар 2");

        var factory = Factory.FindFactory(factories, foundUnit);

        var totalVolume = Tank.GetTotalVolume(tanks);


        Console.WriteLine($"Количество резервуаров: {tanks.Count}, установок: {units.Count}");

        Console.WriteLine($"Резервуар 2 принадлежит установке {foundUnit.Name} и заводу {factory.Name}");

        Console.WriteLine($"Общий объем резервуаров: {totalVolume}");

        while (true)
        {
            
            Console.WriteLine("\n\n Введите номер команды: " +
                "\n 1 - Найти объект." +
                "\n 2 - Добавить новый." +
                "\n 3 - Изменить данные объекта." +
                "\n 4 - Удалить объект." +
                "\n 5 - Выход из программы с сохранением внесённых изменений." +
                "\n 6 - Выход из программы без сохранения.");
            string answer = Console.ReadLine();
            if (answer == "1")
                EntitesStore.FindObject(objects, true);
            else if (answer == "2")
                EntitesStore.AddObject(objects);
            else if (answer == "3")
            {
                var (numb, word) = EntitesStore.FindObject(objects, false);
                EntitesStore.UpdateObject(objects, numb, word);
            }
            else if (answer == "4")
            {
                var (numb, word) = EntitesStore.FindObject(objects, false);
                EntitesStore.DeleteObject(objects, numb, word);
            }
            else if (answer == "5")
            {
                JsonSerDeser.Serializ(jsonSerDeser, objects);
                break;
            }
            else if (answer == "6")
                break;
            else Console.WriteLine("\nНе верный номер команды!");

        }
       
        
              
}

















   



    
    

}
