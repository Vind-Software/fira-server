CREATE SCHEMA application; 

COMMENT ON SCHEMA application IS 'Holds application level configuration and utility tables';

CREATE TABLE application.schema_change_log (
    "id" SERIAL PRIMARY KEY,
    "databaseVersion" VARCHAR(10) UNIQUE NOT NULL,
    "applicationVersion" VARCHAR(9) NOT NULL,
    "script" VARCHAR(18) UNIQUE NOT NULL,
    "author" VARCHAR(50),
    "description" VARCHAR(200),
    "dateApplied" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);