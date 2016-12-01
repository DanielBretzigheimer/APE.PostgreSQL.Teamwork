REVOKE CONNECT ON DATABASE "[Database]" FROM public;
ALTER DATABASE "[Database]" CONNECTION LIMIT 0;
SELECT pg_terminate_backend(pid)
  FROM pg_stat_activity
  WHERE pid <> pg_backend_pid()
  AND datname='[Database]';