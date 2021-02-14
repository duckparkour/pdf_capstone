CREATE TABLE [dbo].[Table]
(
	[FileID] INT NOT NULL PRIMARY KEY,
	[FileExtension] VARCHAR(10) NOT NULL ,
	[FileName] VARCHAR(200) NOT NUll,
	[FileSize] INT NOT NULL,
	[ContentType] VARCHAR(200) NOT NULL,
	[FileContent] VARBINARY(MAX) NOT NULL,
)
