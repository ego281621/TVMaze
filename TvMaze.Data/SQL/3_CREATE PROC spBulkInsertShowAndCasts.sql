
CREATE  PROCEDURE [dbo].[spBulkInsertShowAndCasts]
@TvMazeShowId INT,
@Name VARCHAR(500),
@CastPersons CastPerson READONLY
AS
BEGIN
	
	DECLARE @ShowId INT = 0

	IF NOT EXISTS(SELECT TOP 1 Id FROM dbo.Show WHERE TvMazeShowId = @TvMazeShowId)
	BEGIN

		INSERT INTO dbo.Show(TvMazeShowId, Name)
		SELECT @TvMazeShowId, @Name
		SET @ShowId = SCOPE_IDENTITY()

	END
	ELSE
	BEGIN
		SELECT TOP 1 @ShowId = Id FROM dbo.Show WHERE TvMazeShowId = @TvMazeShowId

		UPDATE dbo.Show
		SET Name = @Name
		WHERE TvMazeShowId = @TvMazeShowId
	END


	IF @ShowId > 0
	BEGIN
		INSERT INTO dbo.[Cast](TvMazePersonId, ShowId, Name)
		SELECT 
		cp.TvMazePersonId,
		@ShowId,
		cp.Name
		FROM @CastPersons cp
		LEFT JOIN dbo.[Cast] c ON c.TvMazePersonId = cp.TvMazePersonId
		WHERE
			c.Id IS NULL

		UPDATE c
			SET Name = cp.Name
		FROM @CastPersons cp
	    JOIN dbo.[Cast] c ON c.TvMazePersonId = cp.TvMazePersonId
	END
END
GO


