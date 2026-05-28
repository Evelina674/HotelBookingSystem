using System.Diagnostics;
using HotelBookingSystem.Domain.Models;

namespace HotelBookingSystem.Domain.Services;

public class CollectionBenchmarkService
{
    public void CompareCollections()
    {
        var roomsList = new List<Room>();
        var roomsDictionary = new Dictionary<int, Room>();
        var roomsHashSet = new HashSet<Room>();

        for (int i = 1; i <= 10000; i++)
        {
            var room = new Room(i, $"Room {i}", 100 + i);

            roomsList.Add(room);
            roomsDictionary.Add(room.Number, room);
            roomsHashSet.Add(room);
        }

        var stopwatch = Stopwatch.StartNew();

        var foundInList = roomsList.FirstOrDefault(room => room.Number == 9999);

        stopwatch.Stop();
        Console.WriteLine($"List search time: {stopwatch.ElapsedTicks} ticks");

        stopwatch.Restart();

        var foundInDictionary = roomsDictionary.TryGetValue(9999, out var dictionaryRoom);

        stopwatch.Stop();
        Console.WriteLine($"Dictionary search time: {stopwatch.ElapsedTicks} ticks");

        stopwatch.Restart();

        var foundInHashSet = roomsHashSet.Contains(new Room(9999, "Room 9999", 10099));

        stopwatch.Stop();
        Console.WriteLine($"HashSet search time: {stopwatch.ElapsedTicks} ticks");
    }
}