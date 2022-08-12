CREATE TYPE application.valid_data_types AS ENUM ('STRING', 'INT', 'TIMESTAMP', 'FLOAT', 'DOUBLE', 'BOOLEAN');

CREATE TABLE application.system_variables (
	"id" SERIAL PRIMARY KEY,
	"name" VARCHAR(150) NOT NULL,
	"code" VARCHAR(200) UNIQUE NOT NULL,
	"type" application.valid_data_types DEFAULT 'STRING'::application.valid_data_types,
	"value" VARCHAR DEFAULT NULL,
    "description" VARCHAR DEFAULT NULL
);