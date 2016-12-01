SET datestyle = dmy;
INSERT INTO "[Schema]"."ExecutionHistory" ("Version", "ExecutionDate", "FileType", "Message") VALUES ('[Version]', '[Time]', '[FileType]', '[Message]');
[Ignore] INSERT INTO "[Schema]"."ExecutedFile" VALUES ('[Version]', '[Time]', '[Message]');