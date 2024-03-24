CREATE PROC SpUsers(
     @Option   NVARCHAR(20)
    ,@Id       UNIQUEIDENTIFIER = NULL
    ,@Name     NVARCHAR(30)     = NULL
    ,@Pass     NVARCHAR(100)    = NULL
    ,@Created  DATETIME         = NULL
    ,@State    BIT              = NULL
)
AS
BEGIN
        IF @Option = 'Truncate'
    BEGIN
        TRUNCATE TABLE Users
    END

    IF @Option = 'Create'
    BEGIN
        INSERT INTO Users(Name,Pass) VALUES (@Name,@Pass)

        SELECT Id FROM Users WHERE Name = @Name;
    END

    IF @Option = 'Update'
    BEGIN
        UPDATE Users SET Name = @Name, Pass = @Pass WHERE Id = @Id;
    END

    IF @Option = 'Delete'
    BEGIN
        DELETE FROM Users WHERE Id = @Id;

        SELECT 'Deleted user with Id '+ CAST(@id AS NVARCHAR)
    END
END