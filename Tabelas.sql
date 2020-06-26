CREATE TABLE [dbo].[Usuario] (
    [User_Id]       INT           NOT NULL,
    [User_name]     VARCHAR (100) NOT NULL,
    [User_password] VARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([User_Id] ASC)
);


CREATE TABLE [dbo].[Task] (
    [Task_Id]          INT           NOT NULL,
    [Task_description] VARCHAR (100) NOT NULL,
    [Task_concluded]   INT           DEFAULT ((0)) NOT NULL,
    [Task_user]        INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Task_Id] ASC),
    CONSTRAINT [FK_Task_Usuario] FOREIGN KEY ([Task_user]) REFERENCES [dbo].[Usuario] ([User_Id])
);
