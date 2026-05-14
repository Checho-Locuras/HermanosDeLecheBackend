-- Hermanos de Leche - datos de ejemplo (opcional)
-- Contraseña de los lecheros sembrados: Lechero123!

INSERT INTO milkmen (id, nombre, username, email, password_hash, ciudad, foto_url, fecha_registro)
VALUES
(
    'a0000001-0000-4000-8000-000000000001',
    'Don Tiple',
    'don_tiple',
    'don.tiple@hermanos.test',
    '$2a$11$mvRbYJRW2swCTeBOs5scm.5/RL/Jq4imRhlOL8FYP8zoCY9vX/5UC',
    'Sogamoso',
    'https://example.com/fotos/don-tiple.png',
    NOW() - INTERVAL '40 days'
),
(
    'a0000002-0000-4000-8000-000000000002',
    'María La Lechera',
    'mlechera',
    'maria@hermanos.test',
    '$2a$11$mvRbYJRW2swCTeBOs5scm.5/RL/Jq4imRhlOL8FYP8zoCY9vX/5UC',
    'Duitama',
    NULL,
    NOW() - INTERVAL '12 days'
),
(
    'a0000003-0000-4000-8000-000000000003',
    'Pepe Pastuso',
    'pepe_p',
    'pepe@hermanos.test',
    '$2a$11$mvRbYJRW2swCTeBOs5scm.5/RL/Jq4imRhlOL8FYP8zoCY9vX/5UC',
    'Paipa',
    NULL,
    NOW() - INTERVAL '5 days'
);

INSERT INTO cows (id, milkman_id, nombre, foto_url, raza, edad, ciudad, descripcion, fecha_registro)
VALUES
(
    'b0000001-0000-4000-8000-510000000001',
    'a0000001-0000-4000-8000-000000000001',
    'Lola',
    NULL,
    'Holstein',
    4,
    'Sogamoso',
    'Le encanta el brebaje directo del balde.',
    NOW() - INTERVAL '30 days'
),
(
    'b0000002-0000-4000-8000-520000000002',
    'a0000001-0000-4000-8000-000000000001',
    'Brisa',
    NULL,
    'Jersey',
    3,
    'Sogamoso',
    'Sedienta nivel campeonato.',
    NOW() - INTERVAL '28 days'
),
(
    'b0000003-0000-4000-8000-530000000003',
    'a0000002-0000-4000-8000-000000000002',
    'Nube',
    NULL,
    'Normanda',
    5,
    'Duitama',
    'Prefiere leche tibia y buena charla.',
    NOW() - INTERVAL '10 days'
),
(
    'b0000004-0000-4000-8000-540000000004',
    'a0000003-0000-4000-8000-000000000003',
    'Tormenta',
    NULL,
    'Angus',
    2,
    'Paipa',
    'Apodo cariñoso; es mimada.',
    NOW() - INTERVAL '3 days'
);

INSERT INTO cow_milk_intakes (id, cow_id, milkman_id, cantidad_litros, fecha, observaciones)
VALUES
(
    'c0000001-0000-4000-8000-000000000001',
    'b0000001-0000-4000-8000-510000000001',
    'a0000001-0000-4000-8000-000000000001',
    2.5,
    NOW() - INTERVAL '20 days',
    'Primera toma del día.'
),
(
    'c0000002-0000-4000-8000-000000000002',
    'b0000001-0000-4000-8000-510000000001',
    'a0000001-0000-4000-8000-000000000001',
    1.2,
    NOW() - INTERVAL '19 days',
    NULL
),
(
    'c0000003-0000-4000-8000-000000000003',
    'b0000001-0000-4000-8000-510000000001',
    'a0000002-0000-4000-8000-000000000002',
    3.0,
    NOW() - INTERVAL '8 days',
    'Visitó el puesto ambulante.'
),
(
    'c0000004-0000-4000-8000-000000000004',
    'b0000002-0000-4000-8000-520000000002',
    'a0000001-0000-4000-8000-000000000001',
    4.0,
    NOW() - INTERVAL '18 days',
    'Récord personal; Lola y Brisa compiten.'
),
(
    'c0000005-0000-4000-8000-000000000005',
    'b0000002-0000-4000-8000-520000000002',
    'a0000001-0000-4000-8000-000000000001',
    2.0,
    NOW() - INTERVAL '17 days',
    NULL
),
(
    'c0000006-0000-4000-8000-000000000006',
    'b0000002-0000-4000-8000-520000000002',
    'a0000003-0000-4000-8000-000000000003',
    0.75,
    NOW() - INTERVAL '2 days',
    'Degustación rápida.'
),
(
    'c0000007-0000-4000-8000-000000000007',
    'b0000003-0000-4000-8000-530000000003',
    'a0000002-0000-4000-8000-000000000002',
    5.5,
    NOW() - INTERVAL '6 days',
    'Sesión campeona.'
),
(
    'c0000008-0000-4000-8000-000000000008',
    'b0000004-0000-4000-8000-540000000004',
    'a0000003-0000-4000-8000-000000000003',
    1.0,
    NOW() - INTERVAL '1 days',
    NULL
);
