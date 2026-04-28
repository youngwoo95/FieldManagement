namespace PlantManagement.ViewItems;

public class EquipmentViewItems
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public override string ToString()
    {
        return Name;
    }
}
