using Godot;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

public partial class MongoDBManager : Node
{
    private readonly string connectionString = "mongodb+srv://alexnormandi:ZENehvoyWVho0k7w@nalzurin.qd9cicy.mongodb.net/?retryWrites=true&w=majority&appName=Nalzurin";
    private readonly string databaseName = "Statistics";
    private readonly string collectionName = "CharacterStats";
    private IMongoCollection<BsonDocument> collection;
    MongoClient client;
    IMongoDatabase database;
    public override void _Ready()
    {
        client = new MongoClient(connectionString);
        database = client.GetDatabase(databaseName);
        collection = database.GetCollection<BsonDocument>(collectionName);
    }

    public BsonDocument LoadStatistics(string characterName)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("CharacterName", characterName);
        return collection.Find(filter).FirstOrDefault();
    }

    public void SaveStatistics(string characterName, int stepsWalked, int damageDealt, int damageTaken, int healthHealed, int enemiesDefeated)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("CharacterName", characterName);
        var update = Builders<BsonDocument>.Update
            .Set("CharacterName", characterName)
            .Set("StepsWalked", stepsWalked)
            .Set("DamageDealt", damageDealt)
            .Set("DamageTaken", damageTaken)
            .Set("HealthHealed", healthHealed)
            .Set("EnemiesDefeated", enemiesDefeated);
        collection.UpdateOne(filter, update, new UpdateOptions { IsUpsert = true });
    }
}
