namespace FieldManagement.Models;

public class MarkerPosition
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public double X { get; set; }
    public double Y { get; set; }

    public override string ToString()
    {
        return $"id: {Id} " +
               $"Text: {Text} " +
               $"X : {X} " +
               $"Y : {Y}";
    }
}