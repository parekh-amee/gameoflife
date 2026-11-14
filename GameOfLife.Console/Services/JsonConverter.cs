using System.Text.Json;
using GameOfLife.ConsoleApp.Configuration;

namespace GameOfLife.ConsoleApp.Services;

public class JsonConverter : System.Text.Json.Serialization.JsonConverter<bool[,]>
{
    public override bool[,] Read(ref Utf8JsonReader reader,Type typeToConvert,JsonSerializerOptions options)
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement rowsElement = document.RootElement;

        if (rowsElement.ValueKind != JsonValueKind.Array)
        {
            throw new JsonException(GameTexts.InvalidSavedGameCells);
        }

        int rows = rowsElement.GetArrayLength();
        int columns = 0;

        if (rows > 0)
        {
            if (rowsElement[0].ValueKind != JsonValueKind.Array)
            {
                throw new JsonException(GameTexts.InvalidSavedGameCells);
            }

            columns = rowsElement[0].GetArrayLength();
        }

        bool[,] cells = new bool[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            JsonElement rowElement = rowsElement[row];

            if (rowElement.ValueKind != JsonValueKind.Array || rowElement.GetArrayLength() != columns)
            {
                throw new JsonException(GameTexts.InvalidSavedGameCells);
            }

            for (int column = 0; column < columns; column++)
            {
                if (rowElement[column].ValueKind is not (JsonValueKind.True or JsonValueKind.False))
                {
                    throw new JsonException(GameTexts.InvalidSavedGameCells);
                }

                cells[row, column] = rowElement[column].GetBoolean();
            }
        }

        return cells;
    }

    public override void Write(Utf8JsonWriter writer,bool[,] value,JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        for (int row = 0; row < value.GetLength(0); row++)
        {
            writer.WriteStartArray();

            for (int column = 0; column < value.GetLength(1); column++)
            {
                writer.WriteBooleanValue(value[row, column]);
            }

            writer.WriteEndArray();
        }

        writer.WriteEndArray();
    }
}