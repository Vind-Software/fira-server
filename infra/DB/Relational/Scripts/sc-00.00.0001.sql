-- SCHEMAS CREATION
CREATE SCHEMA auth;
COMMENT ON SCHEMA auth IS 'Holds relevant structures to authentication and authorization processes';

CREATE SCHEMA service;
COMMENT ON SCHEMA service IS 'Holds utilitary tables that dont map directly to domain entities';

-- EXTENSIONS CREATION
CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;
COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';

-- TYPES CREATION
CREATE TYPE application.valid_data_types AS ENUM (
    'STRING',
    'INT',
    'TIMESTAMP',
    'FLOAT',
    'DOUBLE',
    'BOOLEAN'
);

CREATE TYPE auth.resource_action AS ENUM (
    'CREATE',
    'READ',
    'UPDATE',
    'DELETE'
);

CREATE TYPE auth.resource_auth_level AS ENUM (
    'PUBLIC',
    'REAUTHENTICATE',
    'AUTHENTICATED'
);

CREATE TYPE auth.scope_visibility_level AS ENUM (
    'ADMIN',
    'INSTITUTION',
    'SELF',
    'PUBLIC'
);

-- TABLES CREATION
CREATE TABLE application.system_variables (
    id integer NOT NULL,
    name character varying(150) NOT NULL,
    code character varying(200) NOT NULL,
    type application.valid_data_types DEFAULT 'STRING'::application.valid_data_types,
    value character varying,
    description character varying
);
CREATE SEQUENCE application.system_variables_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER SEQUENCE application.system_variables_id_seq OWNED BY application.system_variables.id;

CREATE TABLE auth.application_clients (
    id integer NOT NULL,
    client_id uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    client_secret uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    name character varying(250)
);
CREATE SEQUENCE auth.application_client_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER SEQUENCE auth.application_client_id_seq OWNED BY auth.application_clients.id;

CREATE TABLE auth.application_clients_application_scopes_grants (
    id bigint NOT NULL,
    scope_id integer NOT NULL,
    client_id integer NOT NULL,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    expiration_date timestamp with time zone,
    visibility_level auth.scope_visibility_level DEFAULT 'PUBLIC'::auth.scope_visibility_level NOT NULL
);
CREATE SEQUENCE auth.application_clients_application_scopes_grants_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER SEQUENCE auth.application_clients_application_scopes_grants_id_seq OWNED BY auth.application_clients_application_scopes_grants.id;

CREATE TABLE auth.application_resources (
    id integer NOT NULL,
    uri character varying(255) NOT NULL,
    type_id integer NOT NULL,
    name character varying(255) NOT NULL,
    action auth.resource_action NOT NULL,
    auth_level auth.resource_auth_level DEFAULT 'AUTHENTICATED'::auth.resource_auth_level NOT NULL
);
CREATE SEQUENCE auth.application_resource_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER SEQUENCE auth.application_resource_id_seq OWNED BY auth.application_resources.id;

CREATE TABLE auth.application_resource_types (
    id smallint NOT NULL,
    name character varying(100) NOT NULL
);
CREATE SEQUENCE auth.application_resource_types_id_seq
    AS smallint
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER SEQUENCE auth.application_resource_types_id_seq OWNED BY auth.application_resource_types.id;

CREATE TABLE auth.application_scopes (
    id integer NOT NULL,
    name character varying(250) DEFAULT 'Not Informed'::character varying,
    path character varying(500) NOT NULL,
    description text,
    visibility_levels auth.scope_visibility_level[] DEFAULT '{ADMIN}'::auth.scope_visibility_level[] NOT NULL
);
COMMENT ON TABLE auth.application_scopes IS 'Holds the list of functionality scopes provided by the application';
CREATE SEQUENCE auth.application_scopes_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER SEQUENCE auth.application_scopes_id_seq OWNED BY auth.application_scopes.id;

CREATE TABLE auth.institution_application_clients (
    id integer NOT NULL,
    institution_id integer NOT NULL,
    application_client_id integer DEFAULT 1 NOT NULL
);
COMMENT ON TABLE auth.institution_application_clients IS 'Holds information about api clients that belongs directly to institutions';
CREATE SEQUENCE auth.institutional_api_clients_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER SEQUENCE auth.institutional_api_clients_id_seq OWNED BY auth.institution_application_clients.id;

CREATE TABLE auth.institutions (
    id integer NOT NULL,
    name character varying(150) NOT NULL,
    owner_name character varying(150) DEFAULT 'Not Informed'::character varying
);
COMMENT ON TABLE auth.institutions IS 'Holds information about the entity ''Institution''';
CREATE SEQUENCE auth.institutions_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER SEQUENCE auth.institutions_id_seq OWNED BY auth.institutions.id;

CREATE TABLE auth.map__application_resources__application_scopes (
    id bigint NOT NULL,
    application_resource_id integer NOT NULL,
    application_scope_id integer NOT NULL
);
CREATE SEQUENCE auth.map__application_resources__application_scopes_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER SEQUENCE auth.map__application_resources__application_scopes_id_seq OWNED BY auth.map__application_resources__application_scopes.id;

-- SETTING DEFAULTS
ALTER TABLE ONLY application.system_variables ALTER COLUMN id SET DEFAULT nextval('application.system_variables_id_seq'::regclass);
ALTER TABLE ONLY auth.application_clients ALTER COLUMN id SET DEFAULT nextval('auth.application_client_id_seq'::regclass);
ALTER TABLE ONLY auth.application_clients_application_scopes_grants ALTER COLUMN id SET DEFAULT nextval('auth.application_clients_application_scopes_grants_id_seq'::regclass);
ALTER TABLE ONLY auth.application_resource_types ALTER COLUMN id SET DEFAULT nextval('auth.application_resource_types_id_seq'::regclass);
ALTER TABLE ONLY auth.application_resources ALTER COLUMN id SET DEFAULT nextval('auth.application_resource_id_seq'::regclass);
ALTER TABLE ONLY auth.application_scopes ALTER COLUMN id SET DEFAULT nextval('auth.application_scopes_id_seq'::regclass);
ALTER TABLE ONLY auth.institution_application_clients ALTER COLUMN id SET DEFAULT nextval('auth.institutional_api_clients_id_seq'::regclass);
ALTER TABLE ONLY auth.institutions ALTER COLUMN id SET DEFAULT nextval('auth.institutions_id_seq'::regclass);
ALTER TABLE ONLY auth.map__application_resources__application_scopes ALTER COLUMN id SET DEFAULT nextval('auth.map__application_resources__application_scopes_id_seq'::regclass);

-- DATA INSERTS
INSERT INTO auth.application_clients (id, client_id, client_secret, name) VALUES (1, '4a2bae7f-54ae-48f9-a772-29fd52795f06', '56a72ac5-c345-431a-b878-0e6fe0e798d3', 'Default Admin Client');
INSERT INTO auth.application_clients_application_scopes_grants (id, scope_id, client_id, created_on, expiration_date, visibility_level) VALUES (1, 3, 1, '2023-01-27 00:51:07.429346+00', NULL, 'PUBLIC');
INSERT INTO auth.application_resource_types (id, name) VALUES (1, 'FUNCTIONALITY');
INSERT INTO auth.application_resources (id, uri, type_id, name, action, auth_level) VALUES (1, '/auth/access-token', 1, 'Get Current Access-Token', 'READ', 'REAUTHENTICATE');
INSERT INTO auth.application_scopes (id, name, path, description, visibility_levels) VALUES 
(1, 'Access Token - All', 'auth.access_token', 'Grants access to all access token related functionalities', '{"ADMIN"}'),
(2, 'Access Token - Create', 'auth.access_token:create', 'Allow clients to request new access tokens', '{"ADMIN"}'),
(3, 'Access Token - Read', 'auth.access_token:read', 'Allow clients to read their current active access token', '{"ADMIN", "SELF"}');
INSERT INTO auth.institution_application_clients (id, institution_id, application_client_id) VALUES (1, 0, 1);
INSERT INTO auth.institutions (id, name, owner_name) VALUES (0, 'FIRA Server Default', 'Vind Software');
INSERT INTO auth.map__application_resources__application_scopes (id, application_resource_id, application_scope_id) VALUES (1, 1, 5);

-- SEQUENCES NEXT VALUE UPDATE 
SELECT pg_catalog.setval('application.system_variables_id_seq', 1, false);
SELECT pg_catalog.setval('auth.application_client_id_seq', 1, true);
SELECT pg_catalog.setval('auth.application_resource_id_seq', 1, true);
SELECT pg_catalog.setval('auth.application_resource_types_id_seq', 1, true);
SELECT pg_catalog.setval('auth.application_scopes_id_seq', 3, true);
SELECT pg_catalog.setval('auth.institutional_api_clients_id_seq', 1, false);
SELECT pg_catalog.setval('auth.application_clients_application_scopes_grants_id_seq', 1, true);
SELECT pg_catalog.setval('auth.institutions_id_seq', 1, false);
SELECT pg_catalog.setval('auth.map__application_resources__application_scopes_id_seq', 1, true);

-- CONSTRAINTS CREATION
ALTER TABLE ONLY application.system_variables
    ADD CONSTRAINT system_variables_code_key UNIQUE (code);

ALTER TABLE ONLY application.system_variables
    ADD CONSTRAINT system_variables_pkey PRIMARY KEY (id);

ALTER TABLE ONLY auth.application_clients
    ADD CONSTRAINT application_client_pkey PRIMARY KEY (id);

ALTER TABLE ONLY auth.application_resources
    ADD CONSTRAINT application_resource_pkey PRIMARY KEY (id);

ALTER TABLE ONLY auth.application_resource_types
    ADD CONSTRAINT application_resource_types_pkey PRIMARY KEY (id);

ALTER TABLE ONLY auth.application_scopes
    ADD CONSTRAINT application_scopes_pkey PRIMARY KEY (id);

ALTER TABLE ONLY auth.application_clients
    ADD CONSTRAINT auth__application_clients__client_id__uq UNIQUE (client_id);

ALTER TABLE ONLY auth.application_scopes
    ADD CONSTRAINT auth__application_scopes_path_uq UNIQUE (path);

ALTER TABLE ONLY auth.institution_application_clients
    ADD CONSTRAINT institutional_api_clients_pkey PRIMARY KEY (id);

ALTER TABLE ONLY auth.application_clients_application_scopes_grants
    ADD CONSTRAINT institutional_clients_scopes_pkey PRIMARY KEY (id);

ALTER TABLE ONLY auth.institutions
    ADD CONSTRAINT institutions_pkey PRIMARY KEY (id);

CREATE INDEX fki_fk__application_resources__application_resources_types ON auth.application_resources USING btree (type_id);

ALTER TABLE ONLY auth.application_clients_application_scopes_grants
    ADD CONSTRAINT auth__application_clients_scopes__application_clients__fk FOREIGN KEY (client_id) REFERENCES auth.application_clients(id) NOT VALID;

ALTER TABLE ONLY auth.institution_application_clients
    ADD CONSTRAINT auth__institution_application_clients_fk_application_client FOREIGN KEY (application_client_id) REFERENCES auth.application_clients(id) ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;

ALTER TABLE ONLY auth.institution_application_clients
    ADD CONSTRAINT auth__institutional_api_clients__service__institutions_fk FOREIGN KEY (institution_id) REFERENCES auth.institutions(id) ON DELETE RESTRICT;

ALTER TABLE ONLY auth.application_clients_application_scopes_grants
    ADD CONSTRAINT auth__institutional_clients_scopes__auth__application_scopes_fk FOREIGN KEY (scope_id) REFERENCES auth.application_scopes(id);

ALTER TABLE ONLY auth.application_resources
    ADD CONSTRAINT fk__application_resources__application_resources_types FOREIGN KEY (type_id) REFERENCES auth.application_resource_types(id) ON UPDATE CASCADE ON DELETE RESTRICT NOT VALID;

ALTER TABLE ONLY auth.map__application_resources__application_scopes
    ADD CONSTRAINT fk__map_resources_scopes__application_resources FOREIGN KEY (application_resource_id) REFERENCES auth.application_resources(id) NOT VALID;

ALTER TABLE ONLY auth.map__application_resources__application_scopes
    ADD CONSTRAINT fk__map_resources_scopes__application_scopes FOREIGN KEY (application_scope_id) REFERENCES auth.application_scopes(id) NOT VALID;


--REVOKE USAGE ON SCHEMA public FROM PUBLIC;
--GRANT ALL ON SCHEMA public TO PUBLIC;