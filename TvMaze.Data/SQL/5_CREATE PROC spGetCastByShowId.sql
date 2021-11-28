CREATE  PROCEDURE [dbo].[spGetCastByShowId]
	@ShowId INT 
AS
BEGIN

SELECT 
	Id,
	TvMazePersonId,
	ShowId,
	Name
FROM dbo.[Cast]
WHERE ShowId = @ShowId

END