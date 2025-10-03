
CREATE TABLE [dbo].[Documents] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [FileName] NVARCHAR(260) NOT NULL,
    [ContentType] NVARCHAR(100) NOT NULL,
    [Size] BIGINT NOT NULL,
    [BlobName] NVARCHAR(500) NOT NULL,
    [Metadata] NVARCHAR(MAX) NULL,
    [CreatedAt] DATETIME2 NOT NULL,
    [UpdatedAt] DATETIME2 NULL
);

-- Seed data
INSERT INTO [dbo].[Documents] (Id, FileName, ContentType, Size, BlobName, Metadata, CreatedAt, UpdatedAt)
VALUES 
(NEWID(), 'example1.pdf', 'application/pdf', 123456, 'blob-example1.pdf', '{"author":"Alice","category":"Reports"}', SYSUTCDATETIME(), NULL),
(NEWID(), 'example2.docx', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', 456789, 'blob-example2.docx', '{"author":"Bob","category":"Specs"}', SYSUTCDATETIME(), NULL),
(NEWID(), 'image1.png', 'image/png', 98765, 'blob-image1.png', '{"author":"Charlie","category":"Images"}', SYSUTCDATETIME(), NULL),
(NEWID(), 'notes.rtf', 'application/rtf', 54321, 'blob-notes.rtf', '{"author":"Dana","category":"Rich Text"}', SYSUTCDATETIME(), NULL),
(NEWID(), 'financials.xlsx', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 234567, 'blob-financials.xlsx', '{"author":"Eve","category":"Finance"}', SYSUTCDATETIME(), NULL);
