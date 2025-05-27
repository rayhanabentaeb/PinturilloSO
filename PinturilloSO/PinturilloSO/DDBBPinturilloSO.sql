DROP DATABASE IF EXISTS DDBBPinturilloSO;
CREATE DATABASE DDBBPinturilloSO;
USE DDBBPinturilloSO;

CREATE TABLE Jugadores (
    ID INTEGER AUTO_INCREMENT PRIMARY KEY,
    Usuario VARCHAR(80) NOT NULL,
    Correo VARCHAR(80) NOT NULL,
    Contrasena VARCHAR(80) NOT NULL,
    FotoPerfil INTEGER NOT NULL
);

CREATE TABLE Partida (
    ID INTEGER AUTO_INCREMENT PRIMARY KEY,
    Codigo INTEGER NOT NULL,
    Rondas INTEGER NOT NULL,
    Privacidad INTEGER NOT NULL
);

CREATE TABLE Dibujos (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Dibujo VARCHAR(80) NOT NULL,
    Dificultad INTEGER NOT NULL,
    Categoria VARCHAR(80) NOT NULL
);

CREATE TABLE Partidas (
    PartidaID INTEGER NOT NULL,
    PrimeroID INTEGER NOT NULL,
    SegundoID INTEGER NOT NULL,
    TerceroID INTEGER,
    CuartoID INTEGER,
    GanadorID INTEGER NOT NULL,
    FOREIGN KEY (PartidaID) REFERENCES Partida(ID),
    FOREIGN KEY (PrimeroID) REFERENCES Jugadores(ID),
    FOREIGN KEY (SegundoID) REFERENCES Jugadores(ID),
    FOREIGN KEY (TerceroID) REFERENCES Jugadores(ID),
    FOREIGN KEY (CuartoID) REFERENCES Jugadores(ID),
    FOREIGN KEY (GanadorID) REFERENCES Jugadores(ID)
);

CREATE TABLE Victorias (
    JugadorID INTEGER NOT NULL,
    PartidaID INTEGER NOT NULL,
    FOREIGN KEY (JugadorID) REFERENCES Jugadores(ID),
    FOREIGN KEY (PartidaID) REFERENCES Partida(ID),
    Puntos INTEGER NOT NULL
);

CREATE TABLE Ranking (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    JugadorID INTEGER NOT NULL,
    Puntuacion INTEGER NOT NULL,
    FOREIGN KEY (JugadorID) REFERENCES Jugadores(ID)
);

CREATE TABLE Amigos (
    JugadorID INTEGER NOT NULL,
    AmigoID INTEGER NOT NULL,
    FOREIGN KEY (JugadorID) REFERENCES Jugadores(ID),
    FOREIGN KEY (AmigoID) REFERENCES Jugadores(ID),
    CHECK (JugadorID <> AmigoID)
);

INSERT INTO Jugadores (Usuario, Correo, Contrasena, FotoPerfil) 
VALUES  ('polfernandez', 'pol.fernandez@gmail.com', 'pol123', 12),
        ('hanabentaeb', 'hana.bentaeb@gmail.com', 'hana123', 11),
        ('noraordonez', 'nora.ordonez@gmail.com', 'nora123', 9);

INSERT INTO Amigos (JugadorID, AmigoID)
VALUES  (1, 2),
        (1, 3),
        (2, 3);