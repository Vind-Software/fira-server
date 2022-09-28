CREATE SCHEMA IF NOT EXISTS service;

COMMENT ON SCHEMA service IS 'Holds utilitary tables that dont map directly to domain entities';

CREATE TABLE IF NOT EXISTS service.institutions (
    id SERIAL NOT NULL,
    name character varying(150) NOT NULL,
    "ownerName" character varying(150) DEFAULT 'Not Informed',
    CONSTRAINT institutions_pkey PRIMARY KEY (id)
);

COMMENT ON TABLE service.institutions IS 'Holds information about the entity ''Institution''';

INSERT INTO service.institutions(id, name, "ownerName") VALUES (0, 'FIRA Server Default', 'Vind Software');