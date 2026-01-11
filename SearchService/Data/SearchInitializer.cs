using Typesense;

namespace SearchService.Data;

public static class SearchInitializer
{
    public static async Task EnsureIndexExistsAsync(ITypesenseClient client, int timeoutSeconds = 60)
    {
        const string schemaName = "questions";
        var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);

        while (DateTime.UtcNow < deadline)
        {
            try
            {
                await client.RetrieveCollection(schemaName);
                Console.WriteLine($"Collection {schemaName} has been created already.");
                return;
            }
            catch (TypesenseApiNotFoundException)
            {
                Console.WriteLine($"Collection {schemaName} has not been created yet.");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error checking Collection {schemaName} has not been created yet. {ex.Message}. Retrying in {timeoutSeconds} seconds...");
                await Task.Delay(1000);
            }
        }

        var schema = new Schema(schemaName, new List<Field>
        {
            new("id", FieldType.String),
            new("title", FieldType.String),
            new("content", FieldType.String),
            new("tags", FieldType.StringArray),
            new("createdAt", FieldType.Int64),
            new("answerCount", FieldType.Int32),
            new("hasAcceptedAnswer", FieldType.Bool)
        })
        {
            DefaultSortingField = "createdAt"
        };
        
        await client.CreateCollection(schema);
        Console.WriteLine($"Typesense Collection {schemaName} has been created.");
    }
}