using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoRepository
{
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// Entity interface.
    /// </summary>
    public interface IMongoEntity
    {
        /// <summary>
        /// Gets or sets the Id of the Entity.
        /// </summary>
        /// <value>Id of the Entity.</value>
        [BsonId]
        string Id { get; set; }
    }
}
