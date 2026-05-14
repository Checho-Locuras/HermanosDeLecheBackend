-- Hermanos de Leche - esquema inicial (PostgreSQL)
-- Ejecutar contra la base de datos objetivo (ej. Railway: railway)

CREATE EXTENSION IF NOT EXISTS "pgcrypto";

CREATE TABLE milkmen (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nombre VARCHAR(200) NOT NULL,
    username VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL,
    password_hash VARCHAR(500) NOT NULL,
    ciudad VARCHAR(200),
    foto_url TEXT,
    fecha_registro TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CONSTRAINT uq_milkmen_username UNIQUE (username),
    CONSTRAINT uq_milkmen_email UNIQUE (email)
);

CREATE INDEX idx_milkmen_fecha_registro ON milkmen (fecha_registro DESC);

CREATE TABLE cows (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    milkman_id UUID NOT NULL REFERENCES milkmen (id) ON DELETE CASCADE,
    nombre VARCHAR(200) NOT NULL,
    foto_url TEXT,
    raza VARCHAR(100),
    edad INT,
    ciudad VARCHAR(200),
    descripcion TEXT,
    fecha_registro TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CONSTRAINT chk_cows_edad CHECK (edad IS NULL OR (edad >= 0 AND edad <= 50))
);

CREATE INDEX idx_cows_milkman_id ON cows (milkman_id);
CREATE INDEX idx_cows_fecha_registro ON cows (fecha_registro DESC);

-- Tabla de negocio: historial de ingestas (CowMilkIntakes)
CREATE TABLE cow_milk_intakes (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    cow_id UUID NOT NULL REFERENCES cows (id) ON DELETE CASCADE,
    milkman_id UUID NOT NULL REFERENCES milkmen (id) ON DELETE CASCADE,
    cantidad_litros NUMERIC(10, 2) NOT NULL,
    fecha TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    observaciones TEXT,
    CONSTRAINT chk_intakes_litros_positivos CHECK (cantidad_litros > 0)
);

CREATE INDEX idx_intakes_cow_id_fecha ON cow_milk_intakes (cow_id, fecha DESC);
CREATE INDEX idx_intakes_milkman_id_fecha ON cow_milk_intakes (milkman_id, fecha DESC);
CREATE INDEX idx_intakes_fecha ON cow_milk_intakes (fecha DESC);
