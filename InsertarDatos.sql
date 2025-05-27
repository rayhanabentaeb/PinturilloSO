INSERT INTO Jugadores (Usuario, Correo, Contrasena, FotoPerfil, Puntos, FraseFavorita) 
VALUES  ('polfernandez', 'pol.fernandez@gmail.com', 'pol123', 12, 0,  'Soy el mejor'),
        ('hanabentaeb', 'hana.bentaeb@gmail.com', 'hana123', 11, 0, 'Soy la mejor'),
        ('noraordonez', 'nora.ordonez@gmail.com', 'nora123', 9, 0, 'Soy la mejor'),
	('nora', 'nora@gmail.com', '123', 11, 0, 'Soy la mejor' ),
        ('pol', 'pol@gmail.com', '123', 12, 0, 'Soy la mejor'),
        ('hana', 'hana@gmail.com', '123', 6, 0, 'Soy la mejor');
        

INSERT INTO Partidas (Codigo, MaximoJugadores, Categoria, Dificultad, Rondas, Privacidad, Fecha)
VALUES  ('4890', 4, 'General', 'Media', 3, 'Publica','2025-05-20'),
        ('9080', 4, 'General', 'Alta', 6, 'Publica','2025-05-21'),
        ('8908', 4, 'General', 'Baja', 9, 'Privada','2025-05-22'),
        ('4021', 4, 'General', 'Baja', 3, 'Privada','2025-05-05'),
        ('2345', 4, 'General', 'Baja', 3, 'Publica','2025-05-06'),
        ('1234', 4, 'General', 'Baja', 3, 'Privada','2025-05-18'),
        ('6732', 4, 'General', 'Baja', 3, 'Privada','2025-05-22');

INSERT INTO PartidasJugadas (PartidaID, PrimeroID, SegundoID, TerceroID, CuartoID, GanadorID)
VALUES  (1, 1, 2, 3, NULL, 1),
        (2, 2, 3, 1, NULL, 1),
        (3, 4, 3, 1, NULL, 4),
        (4, 4, 6, 2, NULL, 6),
        (5, 5, 3, 1, NULL, 3),
        (6, 5, 6, 4, NULL, 4);

INSERT INTO Amigos (JugadorID, AmigoID)
VALUES  (1, 2),
        (1, 3),
        (2, 3),
        (4, 1),
        (4, 2),
        (4, 3);
