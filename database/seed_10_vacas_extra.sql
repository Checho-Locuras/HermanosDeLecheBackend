-- 10 vacas adicionales (Hermanos de Leche)
-- Requiere lecheros del script seed_3_lecheros_5_vacas.sql (milkmen d1000001..d1000003).

INSERT INTO cows (id, milkman_id, nombre, foto_url, tamano, peso, color, edad, ciudad, descripcion, fecha_registro)
VALUES
(
    'd2000006-0000-4000-8000-010000000006',
    'd1000001-0000-4000-8000-000000000001',
    'Nube',
    NULL,
    'Mediana',
    498.00,
    'Gris perla',
    3,
    'Villahermosa',
    'Si la dejas sola con el balde, ya sabes qué pasa.',
    NOW()
),
(
    'd2000007-0000-4000-8000-010000000007',
    'd1000001-0000-4000-8000-000000000001',
    'Tostada',
    NULL,
    'Pequeña',
    360.75,
    'Marrón claro',
    2,
    'Villahermosa',
    'La experta en choreography hacia el bebedero.',
    NOW()
),
(
    'd2000008-0000-4000-8000-010000000008',
    'd1000001-0000-4000-8000-000000000001',
    'Brisa',
    NULL,
    'Grande',
    640.00,
    'Negro brillante',
    5,
    'Villahermosa',
    'No negocia: o hay leche o hay mirada de reproche.',
    NOW()
),
(
    'd2000009-0000-4000-8000-010000000009',
    'd1000001-0000-4000-8000-000000000001',
    'Galleta',
    NULL,
    'Mediana',
    520.20,
    'Café con leche',
    4,
    'Villahermosa',
    'Firma autógrafos con el rabo. Mal, pero firma.',
    NOW()
),
(
    'd2000010-0000-4000-8000-010000000010',
    'd1000002-0000-4000-8000-000000000002',
    'Neblina',
    NULL,
    'Grande',
    680.50,
    'Blanco humo',
    5,
    'El Cocuy',
    'Vive a 3000 m de altitud y de drama.',
    NOW()
),
(
    'd2000011-0000-4000-8000-010000000011',
    'd1000002-0000-4000-8000-000000000002',
    'Lluvia',
    NULL,
    'Mediana',
    540.00,
    'Azul acero',
    3,
    'El Cocuy',
    'Solo bebe si el agua está fría como su carácter.',
    NOW()
),
(
    'd2000012-0000-4000-8000-010000000012',
    'd1000002-0000-4000-8000-000000000002',
    'Trueno',
    NULL,
    'Pequeña',
    395.00,
    'Negro azabache',
    2,
    'El Cocuy',
    'Pequeña pero con agenda llena de mugidos.',
    NOW()
),
(
    'd2000013-0000-4000-8000-010000000013',
    'd1000003-0000-4000-8000-000000000003',
    'Bombón',
    NULL,
    'Mediana',
    575.25,
    'Chocolate',
    4,
    'Soatá',
    'Dulce por fuera, estratega por dentro.',
    NOW()
),
(
    'd2000014-0000-4000-8000-010000000014',
    'd1000003-0000-4000-8000-000000000003',
    'Copito',
    NULL,
    'Grande',
    715.80,
    'Blanco nieve',
    6,
    'Soatá',
    'La reina del silo; el silo opina lo mismo.',
    NOW()
),
(
    'd2000015-0000-4000-8000-010000000015',
    'd1000003-0000-4000-8000-000000000003',
    'Sombrita',
    NULL,
    'Mediana',
    505.00,
    'Gris ratón',
    3,
    'Soatá',
    'Sigue el consejo del abuelo: siempre al lado del comedero.',
    NOW()
)
ON CONFLICT (id) DO NOTHING;
