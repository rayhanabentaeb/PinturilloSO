DROP DATABASE IF EXISTS DDBBPinturilloSO;
CREATE DATABASE DDBBPinturilloSO;
USE DDBBPinturilloSO;

CREATE TABLE Jugadores (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Usuario VARCHAR(80) NOT NULL,
    Correo VARCHAR(80) NOT NULL,
    Contrasena VARCHAR(80) NOT NULL
);

CREATE TABLE Partidas (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Duracion INTEGER NOT NULL,
    PrimeroID INTEGER NOT NULL,
    SegundoID INTEGER NOT NULL,
    TerceroID INTEGER,
    CuartoID INTEGER,
    FOREIGN KEY (PrimeroID) REFERENCES Jugadores(ID),
    FOREIGN KEY (SegundoID) REFERENCES Jugadores(ID),
    FOREIGN KEY (TerceroID) REFERENCES Jugadores(ID),
    FOREIGN KEY (CuartoID) REFERENCES Jugadores(ID)
);

CREATE TABLE Ranking (
    JugadorID INTEGER NOT NULL,
    PartidaID INTEGER NOT NULL,
    Puntuacion INTEGER NOT NULL,
    FOREIGN KEY (JugadorID) REFERENCES Jugadores(ID),
    FOREIGN KEY (PartidaID) REFERENCES Partidas(ID)
);

CREATE TABLE Amigos (
    JugadorID INTEGER NOT NULL,
    AmigoID INTEGER NOT NULL,
    FOREIGN KEY (JugadorID) REFERENCES Jugadores(ID),
    FOREIGN KEY (AmigoID) REFERENCES Jugadores(ID),
    CHECK (JugadorID <> AmigoID)
);

CREATE TABLE Dibujos (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Dibujo VARCHAR(80) NOT NULL,
    Dificultad INTEGER NOT NULL,
    Categoria VARCHAR(80) NOT NULL
);

INSERT INTO Jugadores (Usuario, Correo, Contrasena) 
VALUES  ('polfernandez', 'pol.fernandez@gmail.com', 'pol123'),
        ('hanabentaeb', 'hana.bentaeb@gmail.com', 'hana123'),
        ('noraordonez', 'nora.ordonez@gmail.com', 'nora123');

INSERT INTO Dibujos (Dibujo, Dificultad, Categoria) 
VALUES  ('Perro', 1, 'Animales'),
        ('Cigarrillo', 3, 'Objetos'),
        ('Paella', 5, 'Culturas');

INSERT INTO Partidas (Duracion, PrimeroID, SegundoID, TerceroID, CuartoID)
VALUES  (120, 1, 2, 3, NULL),
        (180, 2, 3, 1, NULL),
        (240, 3, 1, 2, NULL);

INSERT INTO Ranking (JugadorID, PartidaID, Puntuacion)
VALUES  (1, 1, 100),
        (2, 1, 150),
        (3, 1, 90);

INSERT INTO Amigos (JugadorID, AmigoID)
VALUES  (1, 2),
        (1, 3),
        (2, 3);