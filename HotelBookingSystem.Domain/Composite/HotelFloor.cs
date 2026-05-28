namespace HotelBookingSystem.Domain.Composite;

public class HotelFloor : IHotelComponent
{
    private readonly List<IHotelComponent> _components = new();

    public string FloorName { get; set; }

    public HotelFloor(string floorName)
    {
        FloorName = floorName;
    }

    public void Add(IHotelComponent component)
    {
        _components.Add(component);
    }

    public void Remove(IHotelComponent component)
    {
        _components.Remove(component);
    }

    public string GetDetails()
    {
        var result = $"Floor: {FloorName}\n";

        foreach (var component in _components)
        {
            result += component.GetDetails() + "\n";
        }

        return result;
    }
}