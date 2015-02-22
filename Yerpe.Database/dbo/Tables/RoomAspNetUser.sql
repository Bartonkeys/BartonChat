CREATE TABLE [dbo].[RoomAspNetUser] (
    [Room_Id]        INT            NOT NULL,
    [AspNetUsers_Id] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_RoomAspNetUser] PRIMARY KEY CLUSTERED ([Room_Id] ASC, [AspNetUsers_Id] ASC),
    CONSTRAINT [FK_RoomAspNetUser_AspNetUser] FOREIGN KEY ([AspNetUsers_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_RoomAspNetUser_Room] FOREIGN KEY ([Room_Id]) REFERENCES [dbo].[Rooms] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_RoomAspNetUser_AspNetUser]
    ON [dbo].[RoomAspNetUser]([AspNetUsers_Id] ASC);

