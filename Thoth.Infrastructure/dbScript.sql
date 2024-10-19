-- sudo -u postgres psql
CREATE USER thoth WITH PASSWORD 'thoth' CREATEDB;
\c postgres thoth
CREATE DATABASE thothdb;