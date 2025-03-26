create
database HRMDB;
use
HRMDB;

DROP TABLE IF EXISTS `SalarySheetItem`;
CREATE TABLE `SalarySheetItem`
(
    `Id`         CHAR(36)       NOT NULL,
    `SheetId`    CHAR(36)       NOT NULL,
    `EmployeeId` CHAR(36)       NOT NULL,
    `BaseSalary` DECIMAL(19, 4) NOT NULL,
    `Bonus`      DECIMAL(19, 4) NOT NULL,
    `Fine`       DECIMAL(19, 4) NOT NULL,
    `Other`      DECIMAL(19, 4) NOT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

INSERT INTO `SalarySheetItem` (`Id`, `SheetId`, `EmployeeId`, `BaseSalary`, `Bonus`, `Fine`, `Other`)
VALUES ('d52dc14b-8365-4eb5-8a8a-7c9b8966cfd3', '65c7074c-08b8-4158-ab59-f77cf2a3c3e9',
        '06fac891-19fc-454e-8ca6-f10cdab7540a', 6000.0000, 3000.0000, 1000.0000, 1500.0000);

INSERT INTO `SalarySheetItem` (`Id`, `SheetId`, `EmployeeId`, `BaseSalary`, `Bonus`, `Fine`, `Other`)
VALUES ('273dbb38-5320-4edd-854e-d92acbd8b8e1', '4c4405ec-7ff1-41ae-ba7e-7ec4357931a9',
        '57f2acc6-a412-4556-88ea-ab7e2ec23789', 1500.0000, 500.0000, 200.0000, 600.0000);

DROP TABLE IF EXISTS `SalarySheet`;
CREATE TABLE `SalarySheet`
(
    `Id`           CHAR(36) NOT NULL,
    `Year`         INT      NOT NULL,
    `Month`        INT      NOT NULL,
    `DepartmentId` CHAR(36) NOT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

INSERT INTO `SalarySheet` (`Id`, `Year`, `Month`, `DepartmentId`)
VALUES ('4c4405ec-7ff1-41ae-ba7e-7ec4357931a9', 2015, 7, '6c4bf5ea-4309-41b2-b770-55b3256eb9dd');

INSERT INTO `SalarySheet` (`Id`, `Year`, `Month`, `DepartmentId`)
VALUES ('65c7074c-08b8-4158-ab59-f77cf2a3c3e9', 2015, 7, '7c444dbb-8f93-460f-b113-8ad1df6876d1');

DROP TABLE IF EXISTS `Operator`;
CREATE TABLE `Operator`
(
    `Id`        CHAR(36)    NOT NULL,
    `UserName`  VARCHAR(50) NOT NULL,
    `Password`  VARCHAR(50) NOT NULL,
    `IsDeleted` TINYINT(1)  NOT NULL,
    `RealName`  VARCHAR(50) NOT NULL,
    `IsLocked`  TINYINT(1)  NOT NULL,
    `IsAdmin`   TINYINT(1)  NOT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

INSERT INTO `Operator` (`Id`, `UserName`, `Password`, `IsDeleted`, `RealName`, `IsLocked`, `IsAdmin`)
VALUES ('3a9a2894-f9f6-4ffe-b8e6-a67e949f276c', 'admin', '202cb962ac59075b964b07152d234b70', 0, '张三', 0, 1);

DROP TABLE IF EXISTS `OperationLog`;
CREATE TABLE `OperationLog`
(
    `Id`         CHAR(36) NOT NULL,
    `OperatorId` CHAR(36) NOT NULL,
    `ActionDate` DATETIME NOT NULL,
    `ActionDesc` TEXT     NOT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

DROP TABLE IF EXISTS `Employee`;
CREATE TABLE `Employee`
(
    `Id`           CHAR(36)     NOT NULL,
    `Number`       VARCHAR(50)  NOT NULL,
    `Name`         VARCHAR(50)  NOT NULL,
    `BirthDay`     DATETIME     NOT NULL,
    `InDay`        DATETIME     NOT NULL,
    `MarriageId`   CHAR(36)     NOT NULL,
    `PartyId`      CHAR(36)     NOT NULL,
    `EducationId`  CHAR(36)     NOT NULL,
    `GenderId`     CHAR(36)     NOT NULL,
    `DepartmentId` CHAR(36)     NOT NULL,
    `Telephone`    VARCHAR(50)  NOT NULL,
    `Address`      VARCHAR(250) NOT NULL,
    `Email`        VARCHAR(50)  NOT NULL,
    `Remarks`      TEXT NULL,
    `Resume`       TEXT NULL,
    `Photo`        BLOB NULL,
    `Nation`       VARCHAR(50)  NOT NULL,
    `NativePlace`  VARCHAR(50)  NOT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

DROP TABLE IF EXISTS `Dictionary`;
CREATE TABLE `Dictionary`
(
    `Id`       CHAR(36)    NOT NULL,
    `Name`     VARCHAR(50) NOT NULL,
    `Category` VARCHAR(50) NOT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

INSERT INTO `Dictionary` (`Id`, `Name`, `Category`)
VALUES ('010f2847-dfbe-497a-b0cb-365f3c594264', '专科', '学历');
INSERT INTO `Dictionary` (`Id`, `Name`, `Category`)
VALUES ('f0e73f7a-aae0-49c5-b556-4c198ab54312', '本科', '学历');
INSERT INTO `Dictionary` (`Id`, `Name`, `Category`)
VALUES ('ccd51b95-b8c0-494b-8613-549127777df6', '未婚', '婚姻状况');
INSERT INTO `Dictionary` (`Id`, `Name`, `Category`)
VALUES ('7eed0dac-e4dd-4b61-8d98-6cea02b7d985', '女', '性别');
INSERT INTO `Dictionary` (`Id`, `Name`, `Category`)
VALUES ('5a915954-6815-4541-a606-7cbc0507c19f', '高中及以下', '学历');
INSERT INTO `Dictionary` (`Id`, `Name`, `Category`)
VALUES ('e750dad8-f2ad-4163-b6e2-9b9941e0e8c2', '党员', '政治面貌');
INSERT INTO `Dictionary` (`Id`, `Name`, `Category`)
VALUES ('6e719cf1-96ad-4df5-bc6d-ada0e55be3de', '男', '性别');
INSERT INTO `Dictionary` (`Id`, `Name`, `Category`)
VALUES ('f700b36d-085c-4b02-b6cf-afb76ee1084c', '博士', '学历');
INSERT INTO `Dictionary` (`Id`, `Name`, `Category`)
VALUES ('f68abf06-e57f-44bc-94e6-c2587e867e2f', '团员', '政治面貌');
INSERT INTO `Dictionary` (`Id`, `Name`, `Category`)
VALUES ('31f55cbd-80d7-40d6-89a3-c5f84a9b7a79', '民主党派', '政治面貌');
INSERT INTO `Dictionary` (`Id`, `Name`, `Category`)
VALUES ('4d75ee7f-768c-40b6-a75e-f291152197ca', '硕士', '学历');
INSERT INTO `Dictionary` (`Id`, `Name`, `Category`)
VALUES ('eaeda3b7-9925-403f-b06f-fa87dff22a6d', '群众', '政治面貌');
INSERT INTO `Dictionary` (`Id`, `Name`, `Category`)
VALUES ('f17587a8-f9f4-456f-b044-fb13244b1b91', '已婚', '婚姻状况');
INSERT INTO `Dictionary` (`Id`, `Name`, `Category`)
VALUES ('91b073bf-cc31-4ea1-b42a-ffb39af14102', '未知性别', '性别');

DROP TABLE IF EXISTS `Department`;
CREATE TABLE `Department`
(
    `Id`   CHAR(36)    NOT NULL,
    `Name` VARCHAR(50) NOT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

INSERT INTO `Department` (`Id`, `Name`)
VALUES ('6c4bf5ea-4309-41b2-b770-55b3256eb9dd', '学生工作部');
INSERT INTO `Department` (`Id`, `Name`)
VALUES ('17a56aa9-f5f8-40c4-ad86-55dde2c298a0', '人力资源部');
INSERT INTO `Department` (`Id`, `Name`)
VALUES ('f2fcbdb1-562c-4367-95d9-5d50f0989133', '行政部');
INSERT INTO `Department` (`Id`, `Name`)
VALUES ('7c444dbb-8f93-460f-b113-8ad1df6876d1', '.net教学部');
INSERT INTO `Department` (`Id`, `Name`)
VALUES ('394d8cdc-4adb-4bfd-8f61-cc66eecf386d', '网络营销部');