-- ===========================================================
-- 建立 OrderMealNotice 資料表
-- 用途：使用者設定是否接收通知 (Mail / DingTalk)
-- Type = 1 => Mail
-- Type = 2 => DingTalk
-- ===========================================================
CREATE TABLE OrderMealNotice (
    UserNo    VARCHAR2(50)  NOT NULL,
    Type      VARCHAR2(30)  NOT NULL,
    isEnable  NUMBER(1)     DEFAULT 1 NOT NULL,
    CONSTRAINT PK_OrderMealNotice PRIMARY KEY (UserNo, Type)
);

-- 初始化說明（可選）
COMMENT ON TABLE OrderMealNotice IS '使用者通知設定表';
COMMENT ON COLUMN OrderMealNotice.UserNo   IS '使用者編號';
COMMENT ON COLUMN OrderMealNotice.Type     IS '通知類型: 1=Mail, 2=DingTalk';
COMMENT ON COLUMN OrderMealNotice.isEnable IS '是否啟用: 1=啟用, 0=停用';
