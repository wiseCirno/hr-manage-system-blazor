CREATE DATABASE HRMDB;
USE HRMDB;

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
    `Remarks`      TEXT         NULL,
    `Resume`       TEXT         NULL,
    `Photo`        BLOB         NULL,
    `Nation`       VARCHAR(50)  NOT NULL,
    `NativePlace`  VARCHAR(50)  NOT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

-- 示例 1：男员工，未婚，党员，本科，所属部门：行政部
INSERT INTO `Employee`
(
    `Id`,
    `Number`,
    `Name`,
    `BirthDay`,
    `InDay`,
    `MarriageId`,
    `PartyId`,
    `EducationId`,
    `GenderId`,
    `DepartmentId`,
    `Telephone`,
    `Address`,
    `Email`,
    `Remarks`,
    `Resume`,
    `Photo`,
    `Nation`,
    `NativePlace`
)
VALUES
    (
        'a1f41795-3e9f-4a4a-918e-1c1f1d3e8c12',
        'EMP001',
        '刘建国',
        '1985-03-15 00:00:00',
        '2010-06-01 00:00:00',
        'ccd51b95-b8c0-494b-8613-549127777df6',   -- 婚姻状况：未婚
        'e750dad8-f2ad-4163-b6e2-9b9941e0e8c2',   -- 政治面貌：党员
        'f0e73f7a-aae0-49c5-b556-4c198ab54312',   -- 学历：本科
        '6e719cf1-96ad-4df5-bc6d-ada0e55be3de',   -- 性别：男
        'f2fcbdb1-562c-4367-95d9-5d50f0989133',   -- 部门：行政部
        '13800138000',
        '北京市海淀区XX路10号',
        'liujg@example.com',
        '工作积极，责任心强。',
        '曾在多家公司担任中层管理岗位，经验丰富。',
        NULL,
        '汉',
        '北京'
    );

-- 示例 2：女员工，已婚，团员，硕士，所属部门：人力资源部
INSERT INTO `Employee`
(
    `Id`,
    `Number`,
    `Name`,
    `BirthDay`,
    `InDay`,
    `MarriageId`,
    `PartyId`,
    `EducationId`,
    `GenderId`,
    `DepartmentId`,
    `Telephone`,
    `Address`,
    `Email`,
    `Remarks`,
    `Resume`,
    `Photo`,
    `Nation`,
    `NativePlace`
)
VALUES
    (
        'b2d52736-9f5d-41a8-a3a0-1d3da8f39e77',
        'EMP002',
        '张美丽',
        '1990-08-20 00:00:00',
        '2015-09-15 00:00:00',
        'f17587a8-f9f4-456f-b044-fb13244b1b91',   -- 婚姻状况：已婚
        'f68abf06-e57f-44bc-94e6-c2587e867e2f',   -- 政治面貌：团员
        '4d75ee7f-768c-40b6-a75e-f291152197ca',   -- 学历：硕士
        '7eed0dac-e4dd-4b61-8d98-6cea02b7d985',   -- 性别：女
        '17a56aa9-f5f8-40c4-ad86-55dde2c298a0',   -- 部门：人力资源部
        '13900139000',
        '上海市浦东新区XX路18号',
        'zhangml@example.com',
        '善于沟通，团队合作精神强。',
        '拥有多年人力资源管理工作经验，擅长招聘与培训。',
        NULL,
        '汉',
        '上海'
    );

-- 示例 3：男员工，已婚，群众，高中及以下，所属部门：网络营销部
INSERT INTO `Employee`
(
    `Id`,
    `Number`,
    `Name`,
    `BirthDay`,
    `InDay`,
    `MarriageId`,
    `PartyId`,
    `EducationId`,
    `GenderId`,
    `DepartmentId`,
    `Telephone`,
    `Address`,
    `Email`,
    `Remarks`,
    `Resume`,
    `Photo`,
    `Nation`,
    `NativePlace`
)
VALUES
    (
        'c3e638ac-bd8d-4c0f-9f72-53bc31d672e4',
        'EMP003',
        '王大勇',
        '1978-12-05 00:00:00',
        '2005-04-20 00:00:00',
        'f17587a8-f9f4-456f-b044-fb13244b1b91',   -- 婚姻状况：已婚
        'eaeda3b7-9925-403f-b06f-fa87dff22a6d',   -- 政治面貌：群众
        '5a915954-6815-4541-a606-7cbc0507c19f',   -- 学历：高中及以下
        '6e719cf1-96ad-4df5-bc6d-ada0e55be3de',   -- 性别：男
        '394d8cdc-4adb-4bfd-8f61-cc66eecf386d',   -- 部门：网络营销部
        '13700137000',
        '广州市天河区XX街道26号',
        'wangdy@example.com',
        '勤奋踏实，工作经验丰富。',
        '曾在多个营销团队中担任重要职务，负责市场推广与客户关系维护。',
        NULL,
        '汉',
        '广州'
    );

-- 示例 4：女员工，未婚，民主党派，专科，所属部门：.net教学部
INSERT INTO `Employee`
(
    `Id`,
    `Number`,
    `Name`,
    `BirthDay`,
    `InDay`,
    `MarriageId`,
    `PartyId`,
    `EducationId`,
    `GenderId`,
    `DepartmentId`,
    `Telephone`,
    `Address`,
    `Email`,
    `Remarks`,
    `Resume`,
    `Photo`,
    `Nation`,
    `NativePlace`
)
VALUES
    (
        'd4c7b3f2-8a3a-4fbd-9ed6-3a9d1a4b7c41',
        'EMP004',
        '陈佳怡',
        '1992-05-10 00:00:00',
        '2018-03-01 00:00:00',
        'ccd51b95-b8c0-494b-8613-549127777df6',   -- 婚姻状况：未婚
        '31f55cbd-80d7-40d6-89a3-c5f84a9b7a79',   -- 政治面貌：民主党派
        '010f2847-dfbe-497a-b0cb-365f3c594264',   -- 学历：专科
        '7eed0dac-e4dd-4b61-8d98-6cea02b7d985',   -- 性别：女
        '7c444dbb-8f93-460f-b113-8ad1df6876d1',   -- 部门：.net教学部
        '13600136000',
        '浙江省杭州市西湖区XX街道15号',
        'chenjy@example.com',
        '性格开朗，富有团队精神。',
        '毕业于地方高职院校，后通过努力不断提升自我。',
        NULL,
        '汉',
        '杭州'
    );

-- 示例 5：男员工，已婚，党员，博士，所属部门：学生工作部
INSERT INTO `Employee`
(
    `Id`,
    `Number`,
    `Name`,
    `BirthDay`,
    `InDay`,
    `MarriageId`,
    `PartyId`,
    `EducationId`,
    `GenderId`,
    `DepartmentId`,
    `Telephone`,
    `Address`,
    `Email`,
    `Remarks`,
    `Resume`,
    `Photo`,
    `Nation`,
    `NativePlace`
)
VALUES
    (
        'e5b8cfc1-2cef-4f90-b2a0-8a6d4f4233e9',
        'EMP005',
        '黄志远',
        '1980-11-22 00:00:00',
        '2008-07-15 00:00:00',
        'f17587a8-f9f4-456f-b044-fb13244b1b91',   -- 婚姻状况：已婚
        'e750dad8-f2ad-4163-b6e2-9b9941e0e8c2',   -- 政治面貌：党员
        'f700b36d-085c-4b02-b6cf-afb76ee1084c',   -- 学历：博士
        '6e719cf1-96ad-4df5-bc6d-ada0e55be3de',   -- 性别：男
        '6c4bf5ea-4309-41b2-b770-55b3256eb9dd',   -- 部门：学生工作部
        '13700237000',
        '江苏省南京市玄武区XX大道88号',
        'huangzy@example.com',
        '具备丰富的专业知识和管理经验。',
        '曾在高校学生事务管理中发挥重要作用，多次组织大型活动。',
        NULL,
        '汉',
        '南京'
    );

-- 示例 6：女员工，已婚，群众，硕士，所属部门：行政部
INSERT INTO `Employee`
(
    `Id`,
    `Number`,
    `Name`,
    `BirthDay`,
    `InDay`,
    `MarriageId`,
    `PartyId`,
    `EducationId`,
    `GenderId`,
    `DepartmentId`,
    `Telephone`,
    `Address`,
    `Email`,
    `Remarks`,
    `Resume`,
    `Photo`,
    `Nation`,
    `NativePlace`
)
VALUES
    (
        'f6d9a4e8-3c57-4c0b-84a7-9a2a56dfe8e3',
        'EMP006',
        '孙丽华',
        '1987-02-28 00:00:00',
        '2012-10-05 00:00:00',
        'f17587a8-f9f4-456f-b044-fb13244b1b91',   -- 婚姻状况：已婚
        'eaeda3b7-9925-403f-b06f-fa87dff22a6d',   -- 政治面貌：群众
        '4d75ee7f-768c-40b6-a75e-f291152197ca',   -- 学历：硕士
        '7eed0dac-e4dd-4b61-8d98-6cea02b7d985',   -- 性别：女
        'f2fcbdb1-562c-4367-95d9-5d50f0989133',   -- 部门：行政部
        '13800998000',
        '四川省成都市锦江区XX巷20号',
        'sunlh@example.com',
        '做事认真，有较强的协调能力。',
        '曾在多家国企和民营企业任职，熟悉行政管理流程。',
        NULL,
        '汉',
        '成都'
    );

-- 示例 7：男员工，未婚，团员，本科，所属部门：行政部
INSERT INTO `Employee`
(
    `Id`,
    `Number`,
    `Name`,
    `BirthDay`,
    `InDay`,
    `MarriageId`,
    `PartyId`,
    `EducationId`,
    `GenderId`,
    `DepartmentId`,
    `Telephone`,
    `Address`,
    `Email`,
    `Remarks`,
    `Resume`,
    `Photo`,
    `Nation`,
    `NativePlace`
)
VALUES
    (
        '071fce76-3b0d-4e0f-9caa-10291c6e3bf1',
        'EMP007',
        '赵启明',
        '1988-04-12 00:00:00',
        '2013-09-01 00:00:00',
        'ccd51b95-b8c0-494b-8613-549127777df6',   -- 婚姻状况：未婚
        'f68abf06-e57f-44bc-94e6-c2587e867e2f',   -- 政治面貌：团员
        'f0e73f7a-aae0-49c5-b556-4c198ab54312',   -- 学历：本科
        '6e719cf1-96ad-4df5-bc6d-ada0e55be3de',   -- 性别：男
        'f2fcbdb1-562c-4367-95d9-5d50f0989133',   -- 部门：行政部
        '13912341234',
        '湖北省武汉市武昌区XX路20号',
        'zhaoqm@example.com',
        '性格开朗，积极负责。',
        '参与过多个项目管理，具备良好的领导能力。',
        NULL,
        '汉',
        '武汉'
    );

-- 示例 8：女员工，已婚，民主党派，硕士，所属部门：人力资源部
INSERT INTO `Employee`
(
    `Id`,
    `Number`,
    `Name`,
    `BirthDay`,
    `InDay`,
    `MarriageId`,
    `PartyId`,
    `EducationId`,
    `GenderId`,
    `DepartmentId`,
    `Telephone`,
    `Address`,
    `Email`,
    `Remarks`,
    `Resume`,
    `Photo`,
    `Nation`,
    `NativePlace`
)
VALUES
    (
        '3872a9d2-2d2d-4311-bc68-f94b8f9b17cd',
        'EMP008',
        '李婷婷',
        '1993-07-18 00:00:00',
        '2017-06-12 00:00:00',
        'f17587a8-f9f4-456f-b044-fb13244b1b91',   -- 婚姻状况：已婚
        '31f55cbd-80d7-40d6-89a3-c5f84a9b7a79',   -- 政治面貌：民主党派
        '4d75ee7f-768c-40b6-a75e-f291152197ca',   -- 学历：硕士
        '7eed0dac-e4dd-4b61-8d98-6cea02b7d985',   -- 性别：女
        '17a56aa9-f5f8-40c4-ad86-55dde2c298a0',   -- 部门：人力资源部
        '13823452345',
        '福建省厦门市思明区XX小区12栋',
        'litt@example.com',
        '工作认真，沟通能力强。',
        '曾在外企参与人力资源战略项目，擅长人才招聘和培训。',
        NULL,
        '汉',
        '厦门'
    );

-- 示例 9：男员工，已婚，群众，高中及以下，所属部门：网络营销部
INSERT INTO `Employee`
(
    `Id`,
    `Number`,
    `Name`,
    `BirthDay`,
    `InDay`,
    `MarriageId`,
    `PartyId`,
    `EducationId`,
    `GenderId`,
    `DepartmentId`,
    `Telephone`,
    `Address`,
    `Email`,
    `Remarks`,
    `Resume`,
    `Photo`,
    `Nation`,
    `NativePlace`
)
VALUES
    (
        '492e55b6-5c7b-48f5-bcc1-9b16efb7e2a3',
        'EMP009',
        '周文斌',
        '1975-09-30 00:00:00',
        '2000-02-15 00:00:00',
        'f17587a8-f9f4-456f-b044-fb13244b1b91',   -- 婚姻状况：已婚
        'eaeda3b7-9925-403f-b06f-fa87dff22a6d',   -- 政治面貌：群众
        '5a915954-6815-4541-a606-7cbc0507c19f',   -- 学历：高中及以下
        '6e719cf1-96ad-4df5-bc6d-ada0e55be3de',   -- 性别：男
        '394d8cdc-4adb-4bfd-8f61-cc66eecf386d',   -- 部门：网络营销部
        '13787654321',
        '重庆市渝中区解放碑附近XX街5号',
        'zhouwb@example.com',
        '工作踏实，有较强的执行力。',
        '长期从事市场推广工作，熟悉网络营销策略。',
        NULL,
        '汉',
        '重庆'
    );

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