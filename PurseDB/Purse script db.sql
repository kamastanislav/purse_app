CREATE DATABASE [PurseDB]
GO

USE [PurseDB]

/*CREATE TABLE [CURRENCY] (
	[CODE] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[NAME] NVARCHAR(20) NOT NULL,
	[SYMBOL] NVARCHAR(20) NOT NULL
)*/

CREATE TABLE [CATEGORY_SERVICE] (
	[CODE] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[NAME] NVARCHAR(100) NOT NULL,
	[COLOR] NVARCHAR(10) NOT NULL
)

CREATE TABLE [SERVICE] (
	[CODE] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[NAME] NVARCHAR(100) NOT NULL,
	[CATEGORY_CODE] INT NOT NULL,
	[COLOR] NVARCHAR(10) NOT NULL,
	CONSTRAINT FK_CATEGORY_TO_SERVICE FOREIGN KEY ([CATEGORY_CODE])  REFERENCES [CATEGORY_SERVICE] ([CODE])
)

CREATE TABLE [FAMILY] (
	[CODE] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[CALENDAR_DAY_START] INT NOT NULL,
	[OWNER_CODE] INT NOT NULL/*,
	[CURRCODE] INT NOT NULL,
	CONSTRAINT FK_CLIENT_TO_CURRENCY FOREIGN KEY ([CURRCODE])  REFERENCES [CURRENCY] ([CODE])*/
)

CREATE TABLE [USER] (
	[CODE] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[FIRST_NAME] NVARCHAR(100) NOT NULL,
	[LAST_NAME] NVARCHAR(100) NOT NULL,
	[NAME] NVARCHAR(10) NOT NULL,
	[EMAIL] NVARCHAR(100) NOT NULL,
	[PASSWORD] NVARCHAR(MAX) NOT NULL,
	[CASH] NUMERIC(16, 2) DEFAULT 0,
	[PHONE] NVARCHAR(100) NOT NULL,
	[FAMILY_CODE] INT NULL,
	[BIRTHDAY] BIGINT NOT NULL,
	[CREATE_DATE] BIGINT NOT NULL,
	[LAST_LOGIN] BIGINT NULL,
	[STATUS_USER] INT DEFAULT 0,
	CONSTRAINT UQ_USER_EMAIL UNIQUE ([EMAIL]),
    CONSTRAINT UQ_USER_PHONE UNIQUE ([PHONE]),
    CONSTRAINT UQ_USER_NAME UNIQUE ([NAME]),
    CONSTRAINT FK_USERS_TO_FAMILY FOREIGN KEY ([FAMILY_CODE])  REFERENCES [FAMILY] ([CODE])
)

CREATE TABLE [PLAN] (
	[CODE] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[NAME] VARCHAR(100) NOT NULL,
	[DATE_CREATE] BIGINT NULL,
	[LAST_UPDATE_PLAN] BIGINT NULL,
	[OWNER_CODE] INT NOT NULL,
	[EXECUTOR_CODE] INT NULL,
	[START_DATE_PLAN] BIGINT NOT NULL,
	[END_DATE_PLAN] BIGINT NOT NULL,
/*	[CURRCODE] INT NOT NULL,*/
	[PLANNED_BUDGET_PLAN] NUMERIC(16, 2) DEFAULT 0,
	[ACTUAL_BUDGET_PLAN] NUMERIC(16, 2) DEFAULT 0,
	[FAMILY_CODE] INT NULL,
	[STATUS] INT NOT NULL,
	[IS_PRIVATE] BIT DEFAULT 0,
	[COUNT_FLIGHT] INT DEFAULT 0,
	[CATEGORY_CODE] INT NOT NULL,
	[SERVICE_CODE] INT NOT NULL,
	CONSTRAINT FK_PLAN_TO_OWNER FOREIGN KEY ([OWNER_CODE]) REFERENCES [USER] ([CODE]),
	CONSTRAINT FK_PLAN_TO_EXECUTOR FOREIGN KEY ([EXECUTOR_CODE]) REFERENCES [USER] ([CODE]),
	CONSTRAINT FK_PLAN_TO_FAMILY FOREIGN KEY ([FAMILY_CODE])  REFERENCES [FAMILY] ([CODE]),
	CONSTRAINT FK_PLAN_TO_CATEGORY FOREIGN KEY ([CATEGORY_CODE])  REFERENCES [CATEGORY_SERVICE] ([CODE]),
	CONSTRAINT FK_PLAN_TO_SERVICE FOREIGN KEY ([SERVICE_CODE]) REFERENCES [SERVICE] ([CODE]),
)

CREATE TABLE [FLIGHT] (
	[CODE] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[PLAN_CODE] INT NOT NULL,
	[PLANNED_BUDGET] NUMERIC(16, 2) DEFAULT 0,
	[ACTUAL_BUDGET] NUMERIC(16, 2) DEFAULT 0,
	[OWNER] INT NOT NULL,
	[COMMENT] NVARCHAR(250) NULL,
	/*[CURRCODE] INT NOT NULL,*/
	[STATUS] INT NOT NULL,
	[DATE_CREATE] BIGINT NOT NULL,
	CONSTRAINT FK_FLIGHT_TO_USERS FOREIGN KEY ([OWNER])  REFERENCES [USER] ([CODE]),
	CONSTRAINT FK_FLIGHT_TO_PLAN FOREIGN KEY ([PLAN_CODE])  REFERENCES [PLAN] ([CODE])
)

CREATE TABLE [USER_HISTORY_CASH] (
	[CODE] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[USER_CODE] INT NOT NULL,
	[FAMILY_CODE] INT NOT NULL,
	[DATE_ACTION] BIGINT NOT NULL,
	[CASH] NUMERIC(16, 2) NOT NULL,
	[NAME] VARCHAR(80) NOT NULL,
	[CATEGORY_CODE] INT NULL,
	[PLAN_CODE] INT NULL,
	CONSTRAINT FK_HISTORY_CASH_USERS FOREIGN KEY ([USER_CODE])  REFERENCES [USER] ([CODE]),
	CONSTRAINT FK_HISTORY_CASH_CATEGORY FOREIGN KEY ([CATEGORY_CODE])  REFERENCES [CATEGORY_SERVICE] ([CODE]),
	CONSTRAINT FK_HISTORY_CASH_TO_PLAN FOREIGN KEY ([PLAN_CODE])  REFERENCES [PLAN] ([CODE]),
	CONSTRAINT FK_HISTORY_CASH_TO_FAMILY FOREIGN KEY ([FAMILY_CODE])  REFERENCES [FAMILY] ([CODE])
)

CREATE TABLE [USER_INFORMATION] (
	[CODE] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[USER_CODE] INT NOT NULL,
	[INFO] VARCHAR(100) NOT NULL,
	[PLAN_CODE] INT NULL,
	[DATE_ACTION] BIGINT NOT NULL,
	CONSTRAINT FK_INFO_USERS FOREIGN KEY ([USER_CODE])  REFERENCES [USER] ([CODE]),
	CONSTRAINT FK_INFO_TO_PLAN FOREIGN KEY ([PLAN_CODE])  REFERENCES [PLAN] ([CODE])
)

CREATE TABLE [TEMPLATE_PLAN] (
	[CODE] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[PLAN_CODE] INT NOT NULL,
	[REPEAT] INT NULL,
	[IS_AUTO] BIT DEFAULT 0,
	CONSTRAINT FK_TEMPLATE_TO_PLAN FOREIGN KEY ([PLAN_CODE])  REFERENCES [PLAN] ([CODE])
)

CREATE TABLE [TOTALLOG] (
	[CODE] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[DESCRIPTION] NVARCHAR(MAX) NOT NULL,
	[DATEEROR] DATETIME DEFAULT GETDATE()
)

CREATE TABLE [SENTENCE] (
	[CODE] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[MESSAGE] NVARCHAR(MAX) NOT NULL,
	[DATE_SEND] BIGINT NOT NULL,
	[OWNER] INT NOT NULL,
	CONSTRAINT FK_SENTENCE_TO_USERS FOREIGN KEY ([OWNER])  REFERENCES [USER] ([CODE])
)
GO

ALTER TABLE [FAMILY]
ADD 
CONSTRAINT FK_FAMILY_USER FOREIGN KEY ([OWNER_CODE])  REFERENCES [USER] ([CODE])
GO

INSERT INTO [CATEGORY_SERVICE] ([NAME], [COLOR]) VALUES ('Отдых', '4a76a8'), 
														('Жилье', '4aa85e'), 
														('Транспорт', 'a84a8f'), 
														('Одежда', 'a89c4a'), 
														('Коммуникация', 'a84a4a')
GO

INSERT INTO [SERVICE] ([NAME],[CATEGORY_CODE],[COLOR]) VALUES ('Тренажерный зал', 1, 'edeef0'),
															  ('Поход в театр', 1, '4f89fc'),
															  ('Ночной клуб', 1, 'c0f0e9'),
															  ('Каммунальные платежи', 2, '4d6662'),
															  ('Земельный налог', 2, 'c38cd5'),
															  ('Аренда жилья', 2, '4a76a8'),
															  ('Ремонт', 2, '3d8d47'),
															  ('Общественный транспорт', 3, '4872a3'),
															  ('Топливо', 3, 'fb8a8a'),
															  ('Авиаперелет', 3, 'd60000'),
															  ('ЖД транспорт', 3, '0042d6'),
															  ('Ремонт одежды', 4, 'ffc107'),
															  ('Химчистка', 4, '795548'),
															  ('Покупка новой', 4, 'e6fffb'),
															  ('Услуги интерента', 5, '8a5792'),
															  ('Услуги мобильной связи', 5, 'c2da88')
GO