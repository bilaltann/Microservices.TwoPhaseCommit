using MongoDB.Bson.Serialization.Attributes;

namespace Stock.API.Models.Entities
{
    public class Stock
    {
        [BsonId]
        [BsonGuidRepresentation(MongoDB.Bson.GuidRepresentation.CSharpLegacy)]
        [BsonElement(Order =0)] //C# sınıfınız MongoDB’ye çevrilirken, bu öznitelik sayesinde alanların BSON içindeki sıralamasını belirleyebilirsiniz.
        public Guid Id { get; set; }

        [BsonElement(Order = 1)]
        public string ProductId { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]
        [BsonElement(Order = 2)]
        public int Count { get; set; }
    }
}

// bson = binary json
//Bu satırdaki [BsonId] attribute’u, MongoDB koleksiyonundaki bu alanın _id alanı olacağını belirtir.

//MongoDB'de her dökümanın bir _id alanı olur. Bu alan benzersizdir ve dökümanı tanımlamak için kullanılır.
//[BsonId] attribute’u, hangi özelliğin bu _id alanı olarak kullanılacağını belirlemenizi sağlar.