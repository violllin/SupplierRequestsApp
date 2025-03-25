namespace SupplierRequestsApp.Util;

public class UndefinedFolderTypeException(string message) : Exception(message);
public class NoFreeSpaceForItemException(string message) : Exception(message);
public class NoMatchingItemOnShelf(string message) : Exception(message);
public class InvalidCapacityValueException(string message) : Exception(message);
public class SupplierNotFoundException(string message) : Exception(message);
public class ShelfNotFoundException(string message) : Exception(message);
public class OrderNotFoundException(string message) : Exception(message);
public class OrderItemNotFoundException(string message) : Exception(message);
public class PlacingOrderWithEmptyProductsException(string message) : Exception(message);
public class ProductNotFoundException(string message) : Exception(message);
public class OrderNotPaidException(string message) : Exception(message);