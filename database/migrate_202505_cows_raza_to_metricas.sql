-- Migración: vacas — sustituir raza por tamano, peso (kg), color (edad se mantiene).
-- Ejecutar conectado a la base **railway**.

ALTER TABLE cows ADD COLUMN IF NOT EXISTS tamano VARCHAR(100);
ALTER TABLE cows ADD COLUMN IF NOT EXISTS peso NUMERIC(10, 2);
ALTER TABLE cows ADD COLUMN IF NOT EXISTS color VARCHAR(100);

DO $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_schema = 'public' AND table_name = 'cows' AND column_name = 'raza'
    ) THEN
        UPDATE cows SET
            tamano = COALESCE(NULLIF(BTRIM(tamano), ''), raza, 'Mediana'),
            peso = COALESCE(peso, 500.00),
            color = COALESCE(NULLIF(BTRIM(color), ''), 'Sin especificar');
        ALTER TABLE cows DROP COLUMN raza;
    END IF;
END $$;

UPDATE cows SET tamano = 'Mediana' WHERE tamano IS NULL;
UPDATE cows SET color = 'Sin especificar' WHERE color IS NULL;

ALTER TABLE cows DROP CONSTRAINT IF EXISTS chk_cows_peso;
ALTER TABLE cows ADD CONSTRAINT chk_cows_peso CHECK (peso IS NULL OR (peso > 0 AND peso <= 1500));
