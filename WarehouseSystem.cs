using System;
using System.Collections.Generic;
using System.Linq;

// Question 3: Warehouse Inventory with Generics and Exception Handling

// Marker interface
public interface IInventoryItem
{
    int Id { get; set; }
    string Name { get; set; }
    int Quantity { get; set; }
}

// Custom exceptions
public class DuplicateItemException : Exception
{
    public DuplicateItemException(string message) : base(message) { }
}

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string message) : base(message) { }
}

public class InvalidQuantityException : Exception
{
    public InvalidQuantityException(string message) : base(message) { }
}

// Electronic Item class
public class ElectronicItem : IInventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }

    public ElectronicItem(int id, string name, int quantity, string brand, string model)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Brand = brand;
        Model = model;
    }

    public override string ToString()
    {
        return $"Electronic - ID: {Id}, Name: {Name}, Quantity: {Quantity}, Brand: {Brand}, Model: {Model}";
    }
}

// Grocery Item class
public class GroceryItem : IInventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Category { get; set; }

    public GroceryItem(int id, string name, int quantity, DateTime expiryDate, string category)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        ExpiryDate = expiryDate;
        Category = category;
    }

    public override string ToString()
    {
        return $"Grocery - ID: {Id}, Name: {Name}, Quantity: {Quantity}, Expiry: {ExpiryDate:yyyy-MM-dd}, Category: {Category}";
    }
}

// Generic Inventory Repository
public class InventoryRepository<T> where T : IInventoryItem
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        if (item.Quantity < 0)
            throw new InvalidQuantityException("Quantity cannot be negative");

        if (items.Any(i => i.Id == item.Id))
            throw new DuplicateItemException($"Item with ID {item.Id} already exists");

        items.Add(item);
    }

    public void Remove(int id)
    {
        var item = items.FirstOrDefault(i => i.Id == id);
        if (item == null)
            throw new ItemNotFoundException($"Item with ID {id} not found");

        items.Remove(item);
    }

    public void Update(int id, int newQuantity)
    {
        if (newQuantity < 0)
            throw new InvalidQuantityException("Quantity cannot be negative");

        var item = items.FirstOrDefault(i => i.Id == id);
        if (item == null)
            throw new ItemNotFoundException($"Item with ID {id} not found");

        item.Quantity = newQuantity;
    }

    public T GetById(int id)
    {
        var item = items.FirstOrDefault(i => i.Id == id);
        if (item == null)
            throw new ItemNotFoundException($"Item with ID {id} not found");

        return item;
    }

    public List<T> GetAll()
    {
        return new List<T>(items);
    }
}

// Warehouse Manager
public class WarehouseManager
{
    private InventoryRepository<ElectronicItem> electronicRepository;
    private InventoryRepository<GroceryItem> groceryRepository;

    public WarehouseManager()
    {
        electronicRepository = new InventoryRepository<ElectronicItem>();
        groceryRepository = new InventoryRepository<GroceryItem>();
    }

    public void DemonstrateElectronics()
    {
        Console.WriteLine("=== Electronic Items Management ===");
        
        try
        {
            // Add electronic items
            electronicRepository.Add(new ElectronicItem(1, "Laptop", 10, "Dell", "XPS 13"));
            electronicRepository.Add(new ElectronicItem(2, "Phone", 25, "Apple", "iPhone 14"));
            
            Console.WriteLine("Added electronic items successfully");
            
            // Display all items
            foreach (var item in electronicRepository.GetAll())
            {
                Console.WriteLine(item);
            }
            
            // Update quantity
            electronicRepository.Update(1, 8);
            Console.WriteLine("Updated laptop quantity to 8");
            
            // Try to add duplicate (should throw exception)
            electronicRepository.Add(new ElectronicItem(1, "Tablet", 5, "Samsung", "Galaxy Tab"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
        Console.WriteLine();
    }

    public void DemonstrateGroceries()
    {
        Console.WriteLine("=== Grocery Items Management ===");
        
        try
        {
            // Add grocery items
            groceryRepository.Add(new GroceryItem(1, "Milk", 20, DateTime.Now.AddDays(7), "Dairy"));
            groceryRepository.Add(new GroceryItem(2, "Bread", 15, DateTime.Now.AddDays(3), "Bakery"));
            
            Console.WriteLine("Added grocery items successfully");
            
            // Display all items
            foreach (var item in groceryRepository.GetAll())
            {
                Console.WriteLine(item);
            }
            
            // Try invalid quantity (should throw exception)
            groceryRepository.Update(1, -5);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
        Console.WriteLine();
    }

    public static void Main()
    {
        Console.WriteLine("=== Warehouse Inventory System ===\n");
        
        var manager = new WarehouseManager();
        manager.DemonstrateElectronics();
        manager.DemonstrateGroceries();
    }
}