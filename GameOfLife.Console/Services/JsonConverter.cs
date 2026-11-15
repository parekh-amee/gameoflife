using System.Text.Json;
using System.Text.Json.Serialization;
using GameOfLife.ConsoleApp.Configuration;

namespace GameOfLife.ConsoleApp.Services;

// Converts bool[,] to JSON nested rows and reads the same structure back.
public class JsonConverter : JsonConverter<bool[,]>
{
    public override bool[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement rowsElement = document.RootElement;

        if (rowsElement.ValueKind != JsonValueKind.Array || rowsElement.GetArrayLength() == 0)
        {
            throw new JsonException(GameTexts.InvalidSavedGameCells);
        }

        int rows = rowsElement.GetArrayLength();

        if (rowsElement[0].ValueKind != JsonValueKind.Array)
        {
            throw new JsonException(GameTexts.InvalidSavedGameCells);
        }

        int columns = rowsElement[0].GetArrayLength();

        if (columns == 0)
        {
            throw new JsonException(GameTexts.InvalidSavedGameCells);
        }

        bool[,] cells = new bool[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            JsonElement rowElement = rowsElement[row];

            // A rectangular 2D array requires every row to have equal length.
            if (rowElement.ValueKind != JsonValueKind.Array || rowElement.GetArrayLength() != columns)
            {
                throw new JsonException(GameTexts.InvalidSavedGameCells);
            }

            for (int column = 0; column < columns; column++)
            {
                JsonElement cell = rowElement[column];

                if (cell.ValueKind is not (JsonValueKind.True or JsonValueKind.False))
                {
                    throw new JsonException(GameTexts.InvalidSavedGameCells);
                }

                cells[row, column] = cell.GetBoolean();
            }
        }

        return cells;
    }

    public override void Write(Utf8JsonWriter writer, bool[,] cells, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        for (int row = 0; row < cells.GetLength(0); row++)
        {
            writer.WriteStartArray();

            for (int column = 0; column < cells.GetLength(1); column++)
            {
                writer.WriteBooleanValue(cells[row, column]);
            }

            writer.WriteEndArray();
        }

        writer.WriteEndArray();
    }
}