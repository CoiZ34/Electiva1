erDiagram
  CUSTOMERS ||--o{ ORDERS : "CustomerID"
  ORDERS    ||--o{ ORDERDETAILS : "OrderID"
  PRODUCTS  ||--o{ ORDERDETAILS : "ProductID"

  CUSTOMERS {
    int CustomerID PK
    string FirstName
    string LastName
    string Email
    string City
    string Country
  }

  PRODUCTS {
    int ProductID PK
    string ProductName
    string Category
    decimal Price
  }

  ORDERS {
    int OrderID PK
    int CustomerID FK
    date OrderDate
    string Status
  }

  ORDERDETAILS {
    int OrderID PK
    int ProductID PK
    int Quantity
    decimal UnitPrice
    decimal TotalPrice
  }