CREATE TABLE [dbo].[Post]
(
	[Id] INT NOT NULL IDENTITY (1,1), 
    [Content] VARCHAR(4000) NULL, 
    [UserId] INT NOT NULL ,
	CONSTRAINT Post_PK PRIMARY KEY CLUSTERED ([Id]) ,
    CONSTRAINT Post_User_FK FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]),
)

GO


CREATE INDEX [IX_Post_UserId] ON [dbo].[Post] ([UserId])
