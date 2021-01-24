using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Framework.EventSource.Models
{
    public class EventSourceModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }

        public String AggregateId { get; set; }

        public String EventName { get; set; }

        public String PayLoad { get; set; }
    }
}