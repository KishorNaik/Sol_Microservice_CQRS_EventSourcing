CREATE PROCEDURE [dbo].[uspSetMovie]
	@Command Varchar(100),

	@MovieIdentity uniqueidentifier=NULL,

	@Title Varchar(100),
	@ReleaseDate DateTime,

	@IsDelete bit
AS
	
	DECLARE @ErrorMessage Varchar(100);
	DECLARE @IsMovieExists bit;
	DECLARE @AggregateId uniqueidentifier=NULL;

	IF @Command='Add'
		BEGIN
			
			BEGIN TRY
				
				BEGIN TRANSACTION

					SET @IsMovieExists=dbo.IsMovieExists(@MovieIdentity,@Title);

					IF @IsMovieExists=0
						BEGIN
							
							SET @MovieIdentity=NEWID();

							SET @AggregateId=NEWID();

							INSERT INTO tblMovie
							(
								MovieIdentity,
								Title,
								ReleaseDate,
								IsDelete,
								AggregateId
								
							)
							VALUES
							(
								@MovieIdentity,
								@Title,
								@ReleaseDate,
								0,
								@AggregateId
							)

							SELECT CAST(@AggregateId AS VARCHAR(MAX)) As 'Message';
							
							
						END
					ELSE
						BEGIN
							SELECT 'Exist' As 'Message';
						END

				COMMIT TRANSACTION

			END TRY

			BEGIN CATCH 
				SET @ErrorMessage=ERROR_MESSAGE();
				RAISERROR(@ErrorMessage,16,1);
				ROLLBACK TRANSACTION
			END CATCH



		END
	IF @Command='Update'
		BEGIN
			
			BEGIN TRY
				
				BEGIN TRANSACTION

					SET @IsMovieExists=dbo.IsMovieExists(@MovieIdentity,@Title);

					IF @IsMovieExists=0
						BEGIN

						DECLARE @TempTable Table
						(

							MovieIdentity UNIQUEIDENTIFIER,
							Title Varchar(50),
							ReleaseDate datetime,
							AggregateId UNIQUEIDENTIFIER
						)

						INSERT INTO @TempTable(MovieIdentity,Title,ReleaseDate,AggregateId) 
						SELECT M.MovieIdentity,M.Title,M.ReleaseDate,M.AggregateId 
							FROM tblMovie AS M WITH(NOLOCK)
							WHERE M.MovieIdentity=@MovieIdentity
							
							SELECT 
								@Title=CASE WHEN @Title IS NULL THEN M.Title ELSE @Title END,
								@ReleaseDate=CASE WHEN @ReleaseDate IS NULL THEN M.ReleaseDate ELSE @ReleaseDate END,
								@AggregateId=CASE WHEN @AggregateId IS NULL THEN M.AggregateId ELSE @AggregateId END
							FROM 
								@TempTable AS M
							WHERE
								M.MovieIdentity=@MovieIdentity

								-- Get Old Values for Event Publishing
							SELECT
								*
							FROM 
								@TempTable AS M 
							WHERE
								M.MovieIdentity=@MovieIdentity								
								
						-- Update New Values
							UPDATE tblMovie
								SET 
									Title=@Title,
									ReleaseDate=@ReleaseDate
								WHERE
									MovieIdentity=@MovieIdentity

								
							
						END
					ELSE
						BEGIN
							SELECT 'Exist' As 'Message';
						END

				COMMIT TRANSACTION

			END TRY

			BEGIN CATCH 
				SET @ErrorMessage=ERROR_MESSAGE();
				RAISERROR(@ErrorMessage,16,1);
				ROLLBACK TRANSACTION
			END CATCH

		END
		IF @Command='Delete'
		BEGIN
			
			BEGIN TRY
				
				BEGIN TRANSACTION

					UPDATE tblMovie
						SET IsDelete=1
					WHERE
						MovieIdentity=@MovieIdentity

					SELECT 
						@AggregateId=M.AggregateId
					FROM 
						tblMovie AS M WITH(NOLOCK)
					WHERE
						M.MovieIdentity=@MovieIdentity

				SELECT CAST(@AggregateId AS VARCHAR(MAX)) As 'Message';
					
				COMMIT TRANSACTION

			END TRY

			BEGIN CATCH 
				SET @ErrorMessage=ERROR_MESSAGE();
				RAISERROR(@ErrorMessage,16,1);
				ROLLBACK TRANSACTION
			END CATCH



		END

GO
	