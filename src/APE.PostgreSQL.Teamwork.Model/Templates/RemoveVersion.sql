DELETE FROM "[Schema]"."ExecutedFile" sqlFile WHERE sqlFile."Version" ILIKE '%[LastAppliedVersion]%';