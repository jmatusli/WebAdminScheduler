-- APP_SCL_ALTAMIRA.CP_CONEXION definition

CREATE TABLE "APP_SCL_ALTAMIRA"."CP_CONEXION" 
   (	"IDCONEX" NUMBER, 
	"USUARIO" VARCHAR2(25), 
	"PASSWORD" VARCHAR2(25), 
	"SERVICIO" VARCHAR2(25), 
	"DBLINK" VARCHAR2(20), 
	"TIPO" VARCHAR2(10), 
	 CONSTRAINT "CONSTRAINT_IDCONEX" PRIMARY KEY ("IDCONEX")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SCL_DATA"  ENABLE
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SCL_DATA" ;

CREATE SEQUENCE cp_conexion_sequence;

CREATE OR REPLACE TRIGGER "APP_SCL_ALTAMIRA"."CP_CONEXION_ON_INSERT"   BEFORE INSERT ON "APP_SCL_ALTAMIRA"."CP_CONEXION"  FOR EACH ROWBEGIN
  SELECT cp_conexion_sequence.nextval
  INTO :new.IDCONEX
  FROM dual;
END;
/
ALTER TRIGGER "APP_SCL_ALTAMIRA"."CP_CONEXION_ON_INSERT" ENABLE;

CREATE UNIQUE INDEX "APP_SCL_ALTAMIRA"."PK_CONEXION_IDCONEX" ON "APP_SCL_ALTAMIRA"."CP_CONEXION" ("IDCONEX") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SCL_DATA" ;

GRANT SELECT ON "APP_SCL_ALTAMIRA"."CP_CONEXION" TO "URLTUXEDO";
  GRANT SELECT ON "APP_SCL_ALTAMIRA"."CP_CONEXION" TO "APP_APLICACIONES";