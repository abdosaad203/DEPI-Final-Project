CREATE DATABASE catalogdb;
CREATE DATABASE identitydb;
CREATE DATABASE orderingdb;
CREATE DATABASE webhooksdb;

\c catalogdb
CREATE EXTENSION IF NOT EXISTS vector;
