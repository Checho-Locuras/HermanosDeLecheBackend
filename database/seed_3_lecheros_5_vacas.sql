-- 3 lecheros + 5 vacas (Hermanos de Leche)
-- Contraseña de todos los lecheros: Lechero123!
-- Hash BCrypt generado para esa contraseña.

INSERT INTO milkmen (id, nombre, username, email, password_hash, ciudad, foto_url, fecha_registro)
VALUES
(
    'd1000001-0000-4000-8000-000000000001',
    'Lechero Demo Uno',
    'lechero_demo_uno',
    'lechero.demo1@hermanos.test',
    '$2a$11$mvRbYJRW2swCTeBOs5scm.5/RL/Jq4imRhlOL8FYP8zoCY9vX/5UC',
    'Villahermosa',
    NULL,
    NOW()
),
(
    'd1000002-0000-4000-8000-000000000002',
    'Lechero Demo Dos',
    'lechero_demo_dos',
    'lechero.demo2@hermanos.test',
    '$2a$11$mvRbYJRW2swCTeBOs5scm.5/RL/Jq4imRhlOL8FYP8zoCY9vX/5UC',
    'El Cocuy',
    NULL,
    NOW()
),
(
    'd1000003-0000-4000-8000-000000000003',
    'Lechero Demo Tres',
    'lechero_demo_tres',
    'lechero.demo3@hermanos.test',
    '$2a$11$mvRbYJRW2swCTeBOs5scm.5/RL/Jq4imRhlOL8FYP8zoCY9vX/5UC',
    'Soatá',
    NULL,
    NOW()
)
ON CONFLICT (id) DO NOTHING;

INSERT INTO cows (id, milkman_id, nombre, foto_url, tamano, peso, color, edad, ciudad, descripcion, fecha_registro)
VALUES
(
    'd2000001-0000-4000-8000-010000000001',
    'd1000001-0000-4000-8000-000000000001',
    'Canela',
    NULL,
    'Grande',
    620.50,
    'Bayo',
    4,
    'Villahermosa',
    'Fan número uno del recipiente metálico.',
    NOW()
),
(
    'd2000002-0000-4000-8000-010000000002',
    'd1000001-0000-4000-8000-000000000001',
    'Mantequilla',
    NULL,
    'Mediana',
    455.00,
    'Crema',
    3,
    'Villahermosa',
    'Rápida y sin remordimientos.',
    NOW()
),
(
    'd2000003-0000-4000-8000-010000000003',
    'd1000002-0000-4000-8000-000000000002',
    'Eclipse',
    NULL,
    'Mediana',
    610.00,
    'Gris oscuro',
    5,
    'El Cocuy',
    'Prefiere leche fría de altura.',
    NOW()
),
(
    'd2000004-0000-4000-8000-010000000004',
    'd1000002-0000-4000-8000-000000000002',
    'Ciruela',
    NULL,
    'Pequeña',
    380.25,
    'Rojo cereza',
    2,
    'El Cocuy',
    'Todavía aprende el ritual del balde.',
    NOW()
),
(
    'd2000005-0000-4000-8000-010000000005',
    'd1000003-0000-4000-8000-000000000003',
    'Motita',
    NULL,
    'Grande',
    705.00,
    'Blanco con manchas',
    6,
    'Soatá',
    'La que manda en el potrero.',
    NOW()
)
ON CONFLICT (id) DO NOTHING;
