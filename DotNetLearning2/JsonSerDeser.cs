using System.Text.Json;
using System.Net;
using System.Text;

partial class Program
{
    public class JsonSerDeser
    {
        public string PathToAllObjects { get; set; }

        

        public static List<EntitesStore> Deserializ(JsonSerDeser jsonSerDeser)
        {
            var fb = new WebClient() { Encoding = Encoding.UTF8 }.DownloadString(jsonSerDeser.PathToAllObjects);
            var allObjects = JsonSerializer.Deserialize<List<EntitesStore>>(fb);
            
            return allObjects;
        }

        public static void Serializ(JsonSerDeser jsonSerDeser, List<EntitesStore> allObjects)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IgnoreReadOnlyProperties = true
            };
            string jsonAllObjects = JsonSerializer.Serialize(allObjects, options);            
            File.WriteAllText(jsonSerDeser.PathToAllObjects, jsonAllObjects);            
        }

    }
   
}
