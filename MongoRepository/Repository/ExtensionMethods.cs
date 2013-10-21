using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MongoDB.Driver.Linq;

namespace MongoRepository.Repository
{
    public static class ExtensionMethods
    {
        public static string ToMongoQueryText<TQueryable>(this IQueryable<TQueryable> query)
        {
            return (query as MongoQueryable<TQueryable>).GetMongoQuery().ToString();
        }

        [Conditional("DEBUG")]
        public static void DebugWriteMongoQueryText<TQuerable>(this IQueryable<TQuerable> query)
        {
            Debug.WriteLine("QUERY: " + query.ToMongoQueryText());
        }
    }
}
