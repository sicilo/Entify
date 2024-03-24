CREATE TABLE [dbo].[Users] (
    [Id]      UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Name]    NVARCHAR (30)    NOT NULL,
    [Pass]    NVARCHAR (100)   NOT NULL,
    [Created] DATETIME         DEFAULT (getdate()) NULL,
    [State]   BIT              DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
);

