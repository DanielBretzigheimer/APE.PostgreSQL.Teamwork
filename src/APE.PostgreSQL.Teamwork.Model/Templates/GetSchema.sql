SELECT 	COUNT(*) 
FROM 	information_schema.schemata
WHERE	schema_name = '[Schema]'
	AND	catalog_name = '[Database]';