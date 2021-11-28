CREATE  PROCEDURE [dbo].[spGetShow]
	@Skip INT = NULL,
	@Take INT = NULL,
	@ShowId INT = NULL
AS
BEGIN

IF (@Skip IS NULL AND @Take IS NULL)
BEGIN
 SET @Skip = 1
 SET @Take = 10
END

SELECT 
	Id,
	TvMazeShowId,
	Name
FROM dbo.Show 
WHERE (@ShowId IS NULL OR Id = @ShowId)
ORDER BY Name 
OFFSET @Skip rows
FETCH NEXT @Take rows only

END