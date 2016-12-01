
CREATE SCHEMA IF NOT EXISTS "[Schema]";

DO
-- needs to be in one line, else Npgsql splits the statement wrong!
$$ BEGIN CREATE TYPE "[Schema]"."SqlFileType" AS ENUM ('UndoDiff', 'Diff', 'Dump'); EXCEPTION WHEN others THEN /* do nothing because its already existing */ END $$;
ALTER TYPE "[Schema]"."SqlFileType"
	OWNER TO postgres;
COMMENT ON TYPE "[Schema]"."SqlFileType"
	IS 'the type of an sql file which was executed';

CREATE TABLE IF NOT EXISTS "[Schema]"."ExecutedFile"
(
	"Version" text NOT NULL, -- the version of the sql file (is used for the execution order)
	"ExecutionDate" timestamp(0) with time zone NOT NULL, -- time when the file was executed (start)
	"Message" text, -- Contains an optional message if one occured
	CONSTRAINT "ExecutedFile_pkey" PRIMARY KEY ("Version")
)
WITH (
	OIDS=FALSE
);
ALTER TABLE "[Schema]"."ExecutedFile"
	OWNER TO postgres;
COMMENT ON TABLE "[Schema]"."ExecutedFile"
	IS 'Contains all diffs, dumps and undo diffs which were executed';
COMMENT ON COLUMN "[Schema]"."ExecutedFile"."Version" IS 'the version of the sql file (is used for the execution order)';
COMMENT ON COLUMN "[Schema]"."ExecutedFile"."ExecutionDate" IS 'time when the file was executed (start)';
COMMENT ON COLUMN "[Schema]"."ExecutedFile"."Message" IS 'Contains an optional message if one occured';

CREATE TABLE "[Schema]"."ExecutionHistory"
(
  "Id" bigserial NOT NULL, -- Identifier for the history entry
  "Version" text NOT NULL, -- the version of the sql file (is used for the execution order)
  "ExecutionDate" timestamp(0) with time zone NOT NULL, -- time when the file was executed (start)
  "FileType" "[Schema]"."SqlFileType", -- the type of the file e.g. dump, diff
  "Message" text, -- Contains an optional message if one occured
  CONSTRAINT "ExecutionHistory_pkey" PRIMARY KEY ("Id")
)
WITH (
  OIDS=FALSE
);
ALTER TABLE "[Schema]"."ExecutionHistory"
  OWNER TO postgres;
COMMENT ON TABLE "[Schema]"."ExecutionHistory"
  IS 'Contains all diffs, dumps and undo diffs which were executed';
COMMENT ON COLUMN "[Schema]"."ExecutionHistory"."Id" IS 'Identifier for the history entry';
COMMENT ON COLUMN "[Schema]"."ExecutionHistory"."Version" IS 'the version of the sql file (is used for the execution order)';
COMMENT ON COLUMN "[Schema]"."ExecutionHistory"."ExecutionDate" IS 'time when the file was executed (start)';
COMMENT ON COLUMN "[Schema]"."ExecutionHistory"."FileType" IS 'the type of the file e.g. dump, diff';
COMMENT ON COLUMN "[Schema]"."ExecutionHistory"."Message" IS 'Contains an optional message if one occured';

CREATE TABLE "[Schema]"."[Schema].Version"
(
  "Version" bigint NOT NULL, -- the version of the [Schema] tool
  "Time" timestamp(0) with time zone NOT NULL, -- time when the database upgraded to the version
  CONSTRAINT "[Schema].Version_pkey" PRIMARY KEY ("Version")
)
WITH (
  OIDS=FALSE
);
ALTER TABLE "[Schema]"."[Schema].Version"
  OWNER TO postgres;
COMMENT ON TABLE "[Schema]"."[Schema].Version"
  IS 'Contains the version of the [Schema] tool';
COMMENT ON COLUMN "[Schema]"."[Schema].Version"."Version" IS 'the version of the [Schema] tool';
COMMENT ON COLUMN "[Schema]"."[Schema].Version"."Time" IS 'time when the database upgraded to the version';
