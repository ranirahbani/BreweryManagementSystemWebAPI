-- Create Brewery Table
CREATE TABLE Brewery (
    br_Id INT IDENTITY(1,1) PRIMARY KEY, -- Auto-incrementing integer
    br_Name NVARCHAR(255) NOT NULL UNIQUE -- Brewery names are unique
);
GO

----------------------------------------------------------------------------------------------------------------------------------------

-- Create Beer Table
CREATE TABLE Beer (
    be_Id INT IDENTITY(1,1) PRIMARY KEY, -- Auto-incrementing integer
    be_Name NVARCHAR(255) NOT NULL UNIQUE, -- Beer names are unique
    be_AlcoholContent DECIMAL(5, 2) NOT NULL,
    be_Price DECIMAL(10, 2) NOT NULL,
    fk_br_Id INT NOT NULL, -- Foreign key referencing Brewery
    FOREIGN KEY (fk_br_Id) REFERENCES Brewery(br_Id)
);
GO
----------------------------------------------------------------------------------------------------------------------------------------

-- Create Wholesaler Table
CREATE TABLE Wholesaler (
    wh_Id INT IDENTITY(1,1) PRIMARY KEY, -- Auto-incrementing integer
    wh_Name NVARCHAR(255) NOT NULL UNIQUE -- Wholesaler names are unique
);
GO

----------------------------------------------------------------------------------------------------------------------------------------

-- Create WholesalerStock Table (Wholesaler's stock)
CREATE TABLE WholesalerStock (
    fk_wh_Id INT NOT NULL, -- Foreign key referencing Wholesaler
    fk_be_Id INT NOT NULL, -- Foreign key referencing Beer
    ws_Quantity INT NOT NULL CHECK (ws_Quantity >= 0),
    PRIMARY KEY (fk_wh_Id, fk_be_Id),
    FOREIGN KEY (fk_wh_Id) REFERENCES Wholesaler(wh_Id),
    FOREIGN KEY (fk_be_Id) REFERENCES Beer(be_Id)
);
GO

----------------------------------------------------------------------------------------------------------------------------------------

--Add New Brewery
CREATE OR ALTER PROCEDURE spBE_AddBrewery 
	@br_Name NVARCHAR(255)
As
Begin
	SET NOCOUNT ON;

	DECLARE @InsertedBrewery TABLE (br_Id INTEGER);

    INSERT INTO Brewery (br_Name)
    OUTPUT INSERTED.br_Id INTO @InsertedBrewery
    VALUES (@br_Name);

    SELECT br_Id, @br_Name as br_Name FROM @InsertedBrewery;
End;

GO
----------------------------------------------------------------------------------------------------------------------------------------

--Add New Wholesaler
CREATE OR ALTER PROCEDURE spBE_AddWholesaler
	@wh_Name NVARCHAR(255)
As
Begin
	SET NOCOUNT ON;

	DECLARE @InsertedWholesaler TABLE (wh_Id INTEGER);
	
	INSERT INTO Wholesaler(wh_Name)
	OUTPUT INSERTED.wh_Id INTO @InsertedWholesaler
	VALUES(@wh_Name);

	SELECT wh_Id, @wh_Name as wh_name FROM @InsertedWholesaler;
End;

GO
----------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------

--List All Beers by Brewery (FR1)
CREATE OR ALTER PROCEDURE spBE_ListBeersByBrewery
    @fk_br_Id INTEGER = NULL,
    @br_Name NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- If fk_br_Id is NULL or 0, retrieve the br_Id from the brewery name
    IF @fk_br_Id IS NULL OR @fk_br_Id = 0
    BEGIN
        SELECT @fk_br_Id = br_Id FROM Brewery WHERE br_Name = @br_Name;
    END
	Else
	BEGIN
        SELECT @fk_br_Id = (Select br_Id FROM Brewery WHERE br_Id = @fk_br_Id);
    END

    IF @fk_br_Id IS NULL OR @fk_br_Id = 0
    BEGIN
        RAISERROR('Brewery not found.', 16, 1);
        RETURN;
    END

    -- List all beers by the brewery
    SELECT be_id, be_Name, be_AlcoholContent, be_Price, br_id, br_Name
    FROM Beer
	inner join Brewery on br_Id = fk_br_Id
    WHERE fk_br_Id = @fk_br_Id;
END


GO
----------------------------------------------------------------------------------------------------------------------------------------

--Add New Beer (FR2)
CREATE OR ALTER PROCEDURE spBE_AddBeer
    @be_Name NVARCHAR(255),
    @be_AlcoholContent FLOAT,
    @be_Price DECIMAL(10, 2),
    @fk_br_Id INTEGER = NULL,
    @br_Name NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- If fk_br_Id is NULL or 0, retrieve the br_Id from the brewery name
    IF @fk_br_Id IS NULL OR @fk_br_Id = 0
    BEGIN
        SELECT @fk_br_Id = br_Id FROM Brewery WHERE br_Name = @br_Name;
    END
	Else
	BEGIN
        SELECT @fk_br_Id = (Select br_Id FROM Brewery WHERE br_Id = @fk_br_Id);
    END

    IF @fk_br_Id IS NULL OR @fk_br_Id = 0
    BEGIN
        RAISERROR('Brewery not found.', 16, 1);
        RETURN;
    END

	if Exists (Select Top 1 1 from Beer where be_Name = @be_Name)
	Begin
		SELECT Beer.be_Id, be_Name, be_AlcoholContent, be_Price, fk_br_Id
		FROM  Beer
		where be_Name = @be_Name;

		return;
	End

	DECLARE @InsertedBeer TABLE (be_Id INTEGER);
	
	INSERT INTO Beer (be_Name, be_AlcoholContent, be_Price, fk_br_Id)
	OUTPUT INSERTED.be_Id INTO @InsertedBeer
	VALUES (@be_Name, @be_AlcoholContent, @be_Price, @fk_br_Id);

	SELECT Beer.be_id, be_Name, be_AlcoholContent, be_Price, br_Id, br_Name
    FROM Beer
	inner join Brewery on br_Id = fk_br_Id
	inner join @InsertedBeer NB on NB.be_id = Beer.be_Id;
END


GO
----------------------------------------------------------------------------------------------------------------------------------------

--Delete a Beer (FR3)
CREATE OR ALTER PROCEDURE spBE_DeleteBeer
    @be_Id INTEGER = NULL,
    @be_Name NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- If be_Id is NULL or 0, retrieve the be_Id from the beer name
    IF @be_Id IS NULL OR @be_Id = 0
    BEGIN
        SELECT @be_Id = be_Id FROM Beer WHERE be_Name = @be_Name;
    END
	Else
	BEGIN
        SELECT @be_Id = (Select be_Id FROM Beer WHERE be_Id = @be_Id);
    END


    IF @be_Id IS NULL OR @be_Id = 0
    BEGIN
        RAISERROR('Beer not found.', 16, 1);
        RETURN;
    END

    DELETE FROM Beer WHERE be_Id = @be_Id;


	SELECT @be_Id As be_id, @be_Name As be_Name, 0.0 As be_AlcoholContent, 0.0 As be_Price, 0 As br_Id, '' As br_Name
END


GO
----------------------------------------------------------------------------------------------------------------------------------------

--Add Sale of an Existing Beer to an Existing Wholesaler (FR4)
CREATE OR ALTER PROCEDURE spBE_AddSaleToWholesaler
    @fk_wh_Id INTEGER = NULL,
    @wh_Name NVARCHAR(255) = NULL,
    @fk_be_Id INTEGER = NULL,
    @be_Name NVARCHAR(255) = NULL,
    @ws_Quantity INT
AS
BEGIN
    SET NOCOUNT ON;

    -- If fk_wh_Id is NULL or 0, retrieve the wh_Id from the wholesaler name
    IF @fk_wh_Id IS NULL OR @fk_wh_Id = 0
    BEGIN
        SELECT @fk_wh_Id = wh_Id FROM Wholesaler WHERE wh_Name = @wh_Name;
    END
	Else
	BEGIN
        SELECT @fk_wh_Id = (Select wh_Id FROM Wholesaler WHERE wh_Id = @fk_wh_Id);
    END

    IF @fk_wh_Id IS NULL OR @fk_wh_Id = 0
    BEGIN
        RAISERROR('Wholesaler not found.', 16, 1);
        RETURN;
    END

    -- If fk_be_Id is NULL or 0, retrieve the be_Id from the beer name
    IF @fk_be_Id IS NULL OR @fk_be_Id = 0
    BEGIN
        SELECT @fk_be_Id = be_Id FROM Beer WHERE be_Name = @be_Name;
    END
	Else
	BEGIN
        SELECT @fk_be_Id = (Select be_Id FROM Beer WHERE be_Id = @fk_be_Id);
    END

    IF @fk_be_Id IS NULL OR @fk_be_Id = 0
    BEGIN
        RAISERROR('Beer not found.', 16, 1);
        RETURN;
    END

	IF EXISTS (SELECT Top 1 1 FROM WholesalerStock WHERE fk_wh_Id = @fk_wh_Id AND fk_be_Id = @fk_be_Id)
    BEGIN
        UPDATE WholesalerStock
        SET ws_Quantity = ws_Quantity + @ws_Quantity
        WHERE fk_wh_Id = @fk_wh_Id 
		AND fk_be_Id = @fk_be_Id;
    END
    ELSE
    BEGIN
        INSERT INTO WholesalerStock (fk_wh_Id, fk_be_Id, ws_Quantity)
		VALUES (@fk_wh_Id, @fk_be_Id, @ws_Quantity);
    END

	Select wh_Id, wh_name, be_Id, be_Name, ws_Quantity
	from WholesalerStock
	inner join Wholesaler on wh_id = fk_wh_id
	inner join Beer on fk_be_id = be_id
	WHERE fk_wh_Id = @fk_wh_Id 
	AND fk_be_Id = @fk_be_Id;
END



GO
----------------------------------------------------------------------------------------------------------------------------------------

-- Update Remaining Quantity of a Beer in Wholesaler’s Stock (FR5)

CREATE OR ALTER PROCEDURE spBE_UpdateWholesalerStock
    @fk_wh_Id INTEGER = NULL,
    @wh_Name NVARCHAR(255) = NULL,
    @fk_be_Id INTEGER = NULL,
    @be_Name NVARCHAR(255) = NULL,
    @ws_Quantity INT
AS
BEGIN
    SET NOCOUNT ON;

    -- If fk_wh_Id is NULL or 0, retrieve the wh_Id from the wholesaler name
    IF @fk_wh_Id IS NULL OR @fk_wh_Id = 0
    BEGIN
        SELECT @fk_wh_Id = (Select wh_Id FROM Wholesaler WHERE wh_Name = @wh_Name);
    END
	Else
	BEGIN
        SELECT @fk_wh_Id = (Select wh_Id FROM Wholesaler WHERE wh_Id = @fk_wh_Id);
    END

    IF @fk_wh_Id IS NULL OR @fk_wh_Id = 0
    BEGIN
        RAISERROR('Wholesaler not found.', 16, 1);
        RETURN;
    END

    -- If fk_be_Id is NULL or 0, retrieve the be_Id from the beer name
    IF @fk_be_Id IS NULL OR @fk_be_Id = 0
    BEGIN
        SELECT @fk_be_Id = be_Id FROM Beer WHERE be_Name = @be_Name;
    END
	Else
	BEGIN
        SELECT @fk_be_Id = (Select be_Id FROM Beer WHERE be_Id = @fk_be_Id);
    END

    IF @fk_be_Id IS NULL OR @fk_be_Id = 0
    BEGIN
        RAISERROR('Beer not found.', 16, 1);
        RETURN;
    END

    UPDATE WholesalerStock
    SET ws_Quantity = @ws_Quantity
    WHERE fk_wh_Id = @fk_wh_Id AND fk_be_Id = @fk_be_Id;


	Select wh_Id, wh_name, be_Id, be_Name, ws_Quantity
	from WholesalerStock
	inner join Wholesaler on wh_id = fk_wh_id
	inner join Beer on fk_be_id = be_id
	WHERE fk_wh_Id = @fk_wh_Id 
	AND fk_be_Id = @fk_be_Id;
END



GO
----------------------------------------------------------------------------------------------------------------------------------------

--Beer Order List Table Type (For @tblBeerOrderListType Parameter in spBE_GenerateQuote)
CREATE TYPE BeerOrderListType AS TABLE
(
    BeerId INTEGER,
    Quantity INT
);

GO
----------------------------------------------------------------------------------------------------------------------------------------

--Generate a Quote (FR6)
CREATE OR ALTER PROCEDURE spBE_GenerateQuote
    @fk_wh_Id INTEGER = NULL,
    @wh_Name NVARCHAR(255) = NULL,
    @tblBeerOrderListType BeerOrderListType READONLY
AS
BEGIN
    -- Initialize variables
    DECLARE @TotalQuantity INT = 0;
    DECLARE @TotalAmount DECIMAL(10, 2) = 0.00;
    DECLARE @Discount DECIMAL(10, 2) = 0.00;
    DECLARE @AvailableStock INT;
    DECLARE @BeerId INTEGER;
    DECLARE @Quantity INT;
    DECLARE @Price DECIMAL(10, 2);
    DECLARE @Message NVARCHAR(MAX) = '';
    DECLARE @HasError integer = 0;
    
    -- Error: Empty Order List
    IF NOT EXISTS (SELECT Top 1 1 FROM @tblBeerOrderListType)
    BEGIN
        SET @Message += 'The order cannot be empty.
';
        SET @HasError = 1;
		RAISERROR(@Message, 16, 1);
		return;
    END

    -- Get wholesaler ID if not provided
    IF isNull(@fk_wh_Id, 0) = 0
    BEGIN
        SET @fk_wh_Id = (SELECT wh_Id FROM Wholesaler WHERE wh_Name = @wh_Name);
    END
    ELSE
    BEGIN
        SET @fk_wh_Id = (SELECT wh_Id FROM Wholesaler WHERE wh_id = @fk_wh_Id);
    END

	IF isNull(@fk_wh_Id, 0) = 0
    BEGIN
        SET @Message += 'The wholesaler must exist.
';
        SET @HasError = 1;
		RAISERROR(@Message, 16, 1);
		return;
    END

    -- Error: Duplicate Items in Order List
    IF EXISTS (
        SELECT BeerId
        FROM @tblBeerOrderListType
        GROUP BY BeerId
        HAVING COUNT(BeerId) > 1
    )
    BEGIN
        SET @Message += 'There can''t be any duplicate in the order.
';
        SET @HasError = 1;
    END
		
	-- Error: Beer Not Sold by Wholesaler
    IF EXISTS (
        SELECT o.BeerId
        FROM @tblBeerOrderListType o
        LEFT JOIN WholesalerStock ws
        ON o.BeerId = ws.fk_be_Id AND ws.fk_wh_Id = @fk_wh_Id
        WHERE ws.fk_be_Id IS NULL
    )
    BEGIN
        SET @Message += 'The beer must be sold by the wholesaler.
';
        SET @HasError = 1;
    END

    -- Error: Quantity Greater Than Stock
    IF EXISTS (
        SELECT o.BeerId
        FROM @tblBeerOrderListType o
        JOIN WholesalerStock ws
        ON o.BeerId = ws.fk_be_Id AND ws.fk_wh_Id = @fk_wh_Id
        WHERE o.Quantity > ws.ws_Quantity
    )
    BEGIN
            SET @Message += 'The number of beers ordered cannot be greater than the wholesaler''s stock.
';
            SET @HasError = 1;
    END

    -- Calculate Total Quantity and Amount
    IF @HasError = 0
    BEGIN
        SELECT 
            @TotalQuantity = SUM(o.Quantity),
            @TotalAmount = SUM(o.Quantity * b.be_Price)
        FROM @tblBeerOrderListType o
        JOIN Beer b ON o.BeerId = b.be_Id
        JOIN WholesalerStock ws ON o.BeerId = ws.fk_be_Id AND ws.fk_wh_Id = @fk_wh_Id;

        -- Apply Discount based on the total quantity
        IF @TotalQuantity > 20
        BEGIN
            SET @Discount = 0.20; -- 20% Discount
        END
        ELSE IF @TotalQuantity > 10
        BEGIN
            SET @Discount = 0.10; -- 10% Discount
        END

        SET @TotalAmount = @TotalAmount - (@TotalAmount * @Discount);

		SELECT 
            @TotalQuantity AS TotalQuantity,
            @Discount * 100 AS DiscountPercentage,
            @TotalAmount AS TotalAmount;
    END
    -- Return results or error message
    ELSE
    BEGIN
        RAISERROR(@Message, 16, 1);
    END
END;


GO
----------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------
-- Add Samples Data

-- 1) Insert Data into Brewery Table

-- Insert Brewery 'Abbaye de Leffe'
EXEC spBE_AddBrewery @br_Name = 'Abbaye de Leffe';

-- Insert additional Breweries
EXEC spBE_AddBrewery @br_Name = 'Brasserie Dupont';
EXEC spBE_AddBrewery @br_Name = 'Chimay';

-- 2) Insert Data into Beer Table

-- Insert Beer 'Leffe Blonde'
EXEC spBE_AddBeer 
    @be_Name = 'Leffe Blonde',
    @be_AlcoholContent = 6.6,
    @be_Price = 2.20,
    @fk_br_Id = NULL,  
    @br_Name = 'Abbaye de Leffe';  

-- Insert additional Beers
EXEC spBE_AddBeer 
    @be_Name = 'Saison Dupont',
    @be_AlcoholContent = 6.5,
    @be_Price = 3.00,
    @fk_br_Id = NULL,  
    @br_Name = 'Brasserie Dupont';  

EXEC spBE_AddBeer 
    @be_Name = 'Chimay Blue',
    @be_AlcoholContent = 9.0,
    @be_Price = 4.50,
    @fk_br_Id = NULL,  
    @br_Name = 'Chimay';  

-- 3. Insert Data into Wholesaler Table

-- Insert Wholesaler 'GeneDrinks'
EXEC spBE_AddWholesaler @wh_Name = 'GeneDrinks';

-- Insert additional Wholesalers
EXEC spBE_AddWholesaler @wh_Name = 'Belgian Beers Inc';
EXEC spBE_AddWholesaler @wh_Name = 'BeerWorld';

-- 4. Insert Data into WholesalerStock Table

-- Insert WholesalerStock for 'GeneDrinks' and 'Leffe Blonde'
EXEC spBE_AddSaleToWholesaler 
    @fk_wh_Id = NULL,  
    @wh_Name = 'GeneDrinks',  
    @fk_be_Id = NULL,  
    @be_Name = 'Leffe Blonde',  
    @ws_Quantity = 10;

-- Insert additional WholesalerStock
EXEC spBE_AddSaleToWholesaler 
    @fk_wh_Id = NULL,  
    @wh_Name = 'Belgian Beers Inc',  
    @fk_be_Id = NULL,  
    @be_Name = 'Saison Dupont',  
    @ws_Quantity = 20;

EXEC spBE_AddSaleToWholesaler 
    @fk_wh_Id = NULL,  
    @wh_Name = 'BeerWorld',  
    @fk_be_Id = NULL,  
    @be_Name = 'Chimay Blue',  
    @ws_Quantity = 15;

GO

EXEC spBE_AddSaleToWholesaler 
    @fk_wh_Id = NULL,  
    @wh_Name = 'BeerWorld',  
    @fk_be_Id = NULL,  
    @be_Name = 'Leffe Blonde',  
    @ws_Quantity = 10;

GO



Exec spBE_ListBeersByBrewery
    @fk_br_Id = 4,
    @br_Name = NULL
GO


EXEC spBE_AddBeer 
    @be_Name = 'Leffe Blonde 2',
    @be_AlcoholContent = 6.6,
    @be_Price = 2.20,
    @fk_br_Id = NULL,  
    @br_Name = 'Abbaye de Leffe'; 
GO


EXEC spBE_DeleteBeer
    @be_Id = NULL,
    @be_Name  = 'Leffe Blonde 2'


Exec spBE_UpdateWholesalerStock
    @fk_wh_Id = NULL,  
    @wh_Name = 'BeerWorld',  
    @fk_be_Id = NULL,  
    @be_Name = 'Leffe Blonde',  
    @ws_Quantity = 17;
GO



Select * from Brewery
Select * from Beer
Select * from Wholesaler
Select * from WholesalerStock

GO


----------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------
--Generate Quote Scenario:

-- Scenario 1: Successful Quote with Less than 10 Drinks
DECLARE @tbl AS BeerOrderListType;
INSERT INTO @tbl (BeerId, Quantity) VALUES 
((SELECT be_Id FROM Beer WHERE be_Name = 'Leffe Blonde'), 5);
EXEC spBE_GenerateQuote 
    @fk_wh_Id = NULL,
    @wh_Name = 'GeneDrinks',
    @tblBeerOrderListType = @tbl;
GO

-- Scenario 2: Successful Quote with 10-20 Drinks (10% Discount)
DECLARE @tbl AS BeerOrderListType;
INSERT INTO @tbl (BeerId, Quantity) VALUES 
((SELECT be_Id FROM Beer WHERE be_Name = 'Chimay Blue'), 15);
EXEC spBE_GenerateQuote 
    @fk_wh_Id = NULL,
    @wh_Name = 'BeerWorld',
    @tblBeerOrderListType = @tbl;
GO

-- Scenario 3: Successful Quote with More than 20 Drinks (20% Discount) with multiple lines
DECLARE @tbl AS BeerOrderListType;
INSERT INTO @tbl (BeerId, Quantity) VALUES 
((SELECT be_Id FROM Beer WHERE be_Name = 'Chimay Blue'), 15),
((SELECT be_Id FROM Beer WHERE be_Name = 'Leffe Blonde'), 8);
EXEC spBE_GenerateQuote 
    @fk_wh_Id = NULL,
    @wh_Name = 'BeerWorld',
    @tblBeerOrderListType = @tbl;
GO

-- Scenario 4: Error - Empty Order List
DECLARE @tbl AS BeerOrderListType;
EXEC spBE_GenerateQuote 
    @fk_wh_Id = NULL,
    @wh_Name = 'GeneDrinks',
    @tblBeerOrderListType = @tbl;
GO

-- Scenario 5: Error - Wholesaler Does Not Exist
DECLARE @tbl AS BeerOrderListType;
INSERT INTO @tbl (BeerId, Quantity) VALUES 
((SELECT be_Id FROM Beer WHERE be_Name = 'Leffe Blonde'), 5);
EXEC spBE_GenerateQuote 
    @fk_wh_Id = NULL, 
    @wh_Name = 'TESTT', -- Non-existent wholesaler
    @tblBeerOrderListType = @tbl;
GO

-- Scenario 6: Error - Duplicate Items in Order List
DECLARE @tbl AS BeerOrderListType;
INSERT INTO @tbl (BeerId, Quantity) VALUES 
((SELECT be_Id FROM Beer WHERE be_Name = 'Leffe Blonde'), 5),
((SELECT be_Id FROM Beer WHERE be_Name = 'Leffe Blonde'), 5);
EXEC spBE_GenerateQuote 
    @fk_wh_Id = NULL,
    @wh_Name = 'GeneDrinks',
    @tblBeerOrderListType = @tbl;
GO

-- Scenario 7: Error - Beer Not Sold by Wholesaler and Duplicate Items in Order List
DECLARE @tbl AS BeerOrderListType;
INSERT INTO @tbl (BeerId, Quantity) VALUES 
((SELECT be_Id FROM Beer WHERE be_Name = 'Chimay Blue'), 5),
((SELECT be_Id FROM Beer WHERE be_Name = 'Chimay Blue'), 5);
EXEC spBE_GenerateQuote 
    @fk_wh_Id = NULL,
    @wh_Name = 'GeneDrinks',
    @tblBeerOrderListType = @tbl;

GO
----------------------------------------------------------------------------------------------------------------------------------------
