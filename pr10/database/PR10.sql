USE shop_system;
GO

CREATE TABLE ProductReservations (
    ID_reservation INT IDENTITY(1,1) CONSTRAINT PK_ProductReservations PRIMARY KEY,
    user_ID INT NOT NULL CONSTRAINT FK_ProductReservations_Users REFERENCES Users(ID_user),
    product_ID INT NOT NULL CONSTRAINT FK_ProductReservations_Products REFERENCES Products(ID_product),
    quantity INT NOT NULL CONSTRAINT CK_ProductReservations_Quantity CHECK (quantity > 0),
    reserved_date DATETIME NOT NULL CONSTRAINT DF_ProductReservations_ReservedDate DEFAULT GETDATE(),
    released_date DATETIME NULL
);
GO

SELECT * FROM ProductReservations;
GO
