USE [books_storeDB]
GO

/****** Object:  StoredProcedure [dbo].[getbooks]    Script Date: 06/04/2024 22.56.22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[getbooks]
    @PageNumber INT,
    @PageSize INT,
    @SortColumn NVARCHAR(50),
    @SortOrder NVARCHAR(4) -- 'ASC' or 'DESC'
AS
BEGIN
    SET NOCOUNT ON;
    IF @SortOrder NOT IN ('ASC', 'DESC')
        SET @SortOrder = 'ASC'
    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    -- Building the dynamic SQL for OFFSET FETCH
    SET @SQL = N'SELECT * FROM [dbo].[Booksstores] 
                 ORDER BY ' + QUOTENAME(@SortColumn) + ' ' + @SortOrder + '
                 OFFSET ' + CAST(@Offset AS NVARCHAR(10)) + ' ROWS
                 FETCH NEXT ' + CAST(@PageSize AS NVARCHAR(10)) + ' ROWS ONLY;';
    EXEC sp_executesql @SQL;
END
GO

