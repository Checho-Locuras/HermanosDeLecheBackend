-- Comprueba que estás en la base correcta (Railway: railway, no postgres).
SELECT current_database() AS base_actual;

SELECT table_schema, table_name
FROM information_schema.tables
WHERE table_schema = 'public'
  AND table_type = 'BASE TABLE'
ORDER BY table_name;
