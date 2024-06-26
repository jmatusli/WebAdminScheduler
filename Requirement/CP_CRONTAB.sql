-- APP_SCL_ALTAMIRA.CP_CRONTAB definition

CREATE TABLE "APP_SCL_ALTAMIRA"."CP_CRONTAB" 
   (	"IDCRONTAB" NUMBER NOT NULL ENABLE, 
	"FECHA" VARCHAR2(8) DEFAULT '00000000' NOT NULL ENABLE, 
	"HORA_INICIO" VARCHAR2(4) DEFAULT '0000' NOT NULL ENABLE, 
	"RECURRENCIA" VARCHAR2(4) DEFAULT '0000' NOT NULL ENABLE, 
	"HORA_FIN" VARCHAR2(4) DEFAULT '2359' NOT NULL ENABLE, 
	"WDAY_M2S_EX" VARCHAR2(7) DEFAULT 1111111 NOT NULL ENABLE, 
	"DAY_EX" VARCHAR2(500) DEFAULT 0 NOT NULL ENABLE, 
	"MONTH_EX" VARCHAR2(70) DEFAULT 0 NOT NULL ENABLE, 
	"REPEAT_EVERY_MINS" NUMBER(*,0) DEFAULT 0, 
	"REPEAT_AFTER_FINISH" NUMBER(*,0) DEFAULT 1, 
	 CONSTRAINT "CP_CRONTAB_PK" PRIMARY KEY ("IDCRONTAB")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SCL_DATA"  ENABLE
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SCL_DATA" ;


CREATE SEQUENCE cp_crontab_sequence;

CREATE OR REPLACE TRIGGER "APP_SCL_ALTAMIRA"."CP_CRONTAB_ON_INSERT"   BEFORE INSERT ON "APP_SCL_ALTAMIRA"."CP_CRONTAB"  FOR EACH ROWBEGIN
  SELECT cp_crontab_sequence.nextval
  INTO :new.IDCRONTAB
  FROM dual;
END;
/
ALTER TRIGGER "APP_SCL_ALTAMIRA"."CP_CRONTAB_ON_INSERT" ENABLE;

CREATE UNIQUE INDEX "APP_SCL_ALTAMIRA"."PK_CRONTAB_IDCRONTAB" ON "APP_SCL_ALTAMIRA"."CP_CRONTAB" ("IDCRONTAB") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SCL_DATA" ;

COMMENT ON COLUMN APP_SCL_ALTAMIRA.CP_CRONTAB.REPEAT_AFTER_FINISH IS ' NO:0 - YES:1 ';