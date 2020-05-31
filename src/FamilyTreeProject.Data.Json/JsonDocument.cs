using System.Collections.Generic;
using System.IO;
using System.Linq;
using FamilyTreeProject.Core.Data;
using FamilyTreeProject.Data.Common.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace FamilyTreeProject.Data.Json
{
    public class JsonDocument : IDocument
    {

        public TreeModel Tree { get;  set; }
        public IList<FamilyModel> Families { get; private set; }
        public IList<IndividualModel> Individuals { get; private set; }
        public IList<RepositoryModel> Repositories { get; private set; }
        public IList<SourceModel> Sources { get; private set; }

        public JsonDocument()
        {
            Tree = new TreeModel();
            Families = new List<FamilyModel>();
            Individuals = new List<IndividualModel>();
            Repositories = new List<RepositoryModel>();
            Sources = new List<SourceModel>();
        }
        
        /// <summary>
        ///   Loads the Json Document from a Stream
        /// </summary>
        /// <param name = "stream">The stream to load</param>
        public void Load(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    if (jsonReader.TokenType != JsonToken.None)
                    {
                        string jsonText = jsonReader.ReadAsString();
                        if (jsonText != null)
                        {
                            JObject json = (JObject)JToken.Parse(jsonText);

                            Individuals = Parse<IndividualModel>(json, "individuals");
                            Families = Parse<FamilyModel>(json, "families");
                            Sources = Parse<SourceModel>(json, "sources");
                            Repositories = Parse<RepositoryModel>(json, "repositories");
                        }                       
                    }
                }
            }
        }

        private IList<T> Parse<T>(JObject json, string nodeName)
        {
            IList<JToken> results = json["familyTree"][nodeName].Children().ToList();

            // deserialize JSON results into .NET objects
            var list = new List<T>();
            foreach (JToken result in results)
            {
                // JToken.ToObject is a helper method that uses JsonSerializer internally
                T item = result.ToObject<T>();
                list.Add(item);
            }

            return list;
        }
        
        /// <summary>
        ///   Save the Json Document to a Stream.
        /// </summary>
        /// <param name = "stream">The streanm to save to.</param>
        public void Save(Stream stream)
        {
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings 
            { 
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });
            
            using (JsonWriter writer = new JsonTextWriter(new StreamWriter(stream)))
            {
                var familyTree = new
                {
                    Tree,
                    Individuals,
                    Families,
                    Sources,
                    Repositories
                };
                
                serializer.Serialize(writer, new
                {
                    FamilyTree =  familyTree
                });
            }
        }
    }
}