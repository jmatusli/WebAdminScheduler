-- APP_SCL_ALTAMIRA.CP_DEPENDENCIAS definition

CREATE TABLE "APP_SCL_ALTAMIRA"."CP_DEPENDENCIAS" 
   (	"IDDEP" NUMBER, 
	"IDPROC" NUMBER, 
	"IDPROC_DEP" NUMBER
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SCL_DATA" ;

CREATE UNIQUE INDEX "APP_SCL_ALTAMIRA"."PK_DEPENDENCIAS" ON "APP_SCL_ALTAMIRA"."CP_DEPENDENCIAS" ("IDPROC", "IDPROC_DEP") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SCL_DATA" ;
  CREATE UNIQUE INDEX "APP_SCL_ALTAMIRA"."PK_DEPENDENCIAS_001X" ON "APP_SCL_ALTAMIRA"."CP_DEPENDENCIAS" ("IDDEP") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SCL_DATA" ;