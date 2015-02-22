CREATE TABLE [dbo].[Messages] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      NVARCHAR (128) NOT NULL,
    [Text]        NVARCHAR (MAX) NOT NULL,
    [Room_Id]     INT            NOT NULL,
    [DateCreated] DATETIME       NOT NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Messages_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_RoomMessage] FOREIGN KEY ([Room_Id]) REFERENCES [dbo].[Rooms] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_RoomMessage]
    ON [dbo].[Messages]([Room_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_Messages_AspNetUsers]
    ON [dbo].[Messages]([UserId] ASC);

