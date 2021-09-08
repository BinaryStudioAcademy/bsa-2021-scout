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
INSERT INTO Secrets ([Key], [Value])
VALUES ('MAIL_ADDRESS', 'notifications.scout@gmail.com')
GO
INSERT INTO Secrets ([Key], [Value])
VALUES ('MAIL_DISPLAY_NAME', 'Scout Notifications')
GO
INSERT INTO Secrets ([Key], [Value])
VALUES ('MAIL_PASSWORD', '_-#SupeR_SCOUT*mail989*')
GO