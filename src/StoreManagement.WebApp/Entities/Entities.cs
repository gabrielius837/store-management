namespace StoreManagement.WebApp;

public record ProductType(int Id, string Name);
public record ProductSubtype(int Id, string Name, int TypeId);
public record Product(int Id, string Name, int SubtypeId);