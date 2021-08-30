CREATE DATABASE HashiCorp;
GO
USE HashiCorp;
GO
DROP TABLE IF EXISTS Secrets;
GO
CREATE TABLE Secrets(
  [Key] [varchar](255),
  [Value] [varchar](255)
);
GO