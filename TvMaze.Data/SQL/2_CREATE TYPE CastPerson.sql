USE [Test]
GO

/****** Object:  UserDefinedTableType [dbo].[CastPerson]    Script Date: 28/11/2021 10:00:59 am ******/
CREATE TYPE [dbo].[CastPerson] AS TABLE(
	[TvMazePersonId] [int] NULL,
	[Name] [varchar](100) NULL
)
GO


