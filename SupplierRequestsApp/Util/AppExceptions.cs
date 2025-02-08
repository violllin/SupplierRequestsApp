namespace SupplierRequestsApp.Util;

public class UndefinedFolderTypeException(string message) : Exception(message);
public class NoFreeSpaceForItemException(string message) : Exception(message);
public class NoMatchingItemOnShelf(string message) : Exception(message);
public class InvalidCapacityValueException(string message) : Exception(message);
public class SupplierNotFoundException(string message) : Exception(message);
public class ShelfNotFoundException(string message) : Exception(message);
