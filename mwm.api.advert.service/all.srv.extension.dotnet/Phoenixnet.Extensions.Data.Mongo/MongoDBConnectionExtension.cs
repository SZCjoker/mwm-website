using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Phoenixnet.Extensions.Data.Mongo
{
    public static class MongoDBConnectionExtension
    {
        /// <summary>
        ///MongoDB 連線單一實例
        ///請參考官方說明Reuse章節
        /// http://mongodb.github.io/mongo-csharp-driver/2.0/reference/driver/connecting/#re-use
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="config"></param>
        
        public static void UseMongoDB(this IServiceCollection serviceCollection, string config)
        {
            serviceCollection.AddSingleton(s =>
                {
                    var _mongoClient =  new MongoClient(config);
                    return _mongoClient;
                }
            );
        }

    }
}