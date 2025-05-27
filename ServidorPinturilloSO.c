#include <string.h>
#include <strings.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>

#include <pthread.h>
#include <mysql.h> 
#include <stdbool.h>


typedef struct {
	char Usuario[50];
	int FotoPerfil;
	int Socket;
}Conectado;


typedef struct {
	Conectado Conectados[100];
	int num;
}ListaConectados;


typedef struct {
	Conectado Conectados[4];
	int num;
	int Puntos;
	int VotacionesExpulsion;
}ListaJugadores;


typedef struct {
	Conectado Jugador;
	int Puntos;
}PuntosJugador;


typedef struct {
	char Votante[80];
	char Votado[80];
}VotacionesExpulsion;


typedef struct {
	int MaximoJugadores;
	int Rondas;
	int Palabras;
	int TotalPintores;
	int TotalVotaciones;
	char CodigoSala[5];
	char Dificultad[20];
	char Privacidad[20];
	char Categoria[80];
	char Ultimos20Mensajes[40][1000];
	char PalabrasPorAcertar[50][100];
	char Pintores[10][80];
	char Acertantes[5][80];
	VotacionesExpulsion VotacionesDeExpulsion[20];
	ListaConectados ConectadosPartida;
	PuntosJugador Puntos[4];
	int PartidaIniciada;
    int NumAcertantesActuales;
	time_t FechaInicio;
}Partida;


typedef struct {
	Partida Partidas[100];
	int num;
}ListaPartidas;


ListaConectados JugadoresConectados;
ListaPartidas PartidasActivas;
int CodigosSala[100];
int sockets[100];
int i = 0;

// Estructura necesaria para acceso excluyente
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;


int NuevoConectado(ListaConectados *Lista, char Usuario[50], int FotoPerfil, int Socket){
	// Añade Nuevo Conectado. Retorna 0 si ok y -1 si la lista esstaba llena.
	if (Lista->num == 100)
		return -1;
	else {
		strcpy(Lista->Conectados[Lista->num].Usuario, Usuario);
		Lista->Conectados[Lista->num].FotoPerfil = FotoPerfil;
		Lista->Conectados[Lista->num].Socket = Socket;
		Lista->num++;
		return 0;
	}
}


int DamePosicion(ListaConectados *Lista, char Usuario[80]){
	// Devuelve la posicion en la lista o -1 si no está en la lista.
	int i = 0;
	int encontrado = 0;
	while ((i < Lista->num) && !encontrado)
	{
		if (strcmp(Lista->Conectados[i].Usuario, Usuario) == 0)
			encontrado = 1;
		else
			i++;
	}
	if (encontrado)
		return i;
	else
		return -1;
}


int EliminarDesconectados(ListaConectados *Lista, char Usuario[80]){
	// Retorna 0 si elimina y -1 si el usuario no está en la lista.
	int posicion = DamePosicion(Lista, Usuario);
	if (posicion == -1)
		return -1;
	else {
		for (int i = posicion; i < Lista->num - 1; i++)
		{
			Lista->Conectados[i] = Lista->Conectados[i+1];
		}
		Lista->num--;
		return 0;
	}
}


void DameConectados(ListaConectados *Lista, char Conectados[1000]){
	// Pone en conectados los nombres de todos los conectados separados por /.
	
	Conectados[0] = '\0';	// Recorre la lista de conectados y concatena los nombres
	for (int i = 0; i < Lista->num; i++) {
		// Agrega el separador solo si no es el primer elemento
		if (i > 0) {
			strcat(Conectados, "/");
		}
		// Concatenar el nombre del usuario
		strcat(Conectados, Lista->Conectados[i].Usuario);
	}
}


void DamePartidas(ListaPartidas *Lista, char Partidas[1000]){
	// Pone en conectados los nombres de todos los conectados separados por /.
	
	Partidas[0] = '\0';	// Recorre la lista de conectados y concatena los nombres
	for (int i = 0; i < Lista->num; i++) {
		// Agrega el separador solo si no es el primer elemento
		if (i > 0) {
			strcat(Partidas, "/");
		}
		// Concatenar el nombre del usuario
		strcat(Partidas, Lista->Partidas[i].CodigoSala);
		strcat(Partidas, "/");
		char PartidaIniciada[10];
		sprintf(PartidaIniciada, "%d", Lista->Partidas[i].PartidaIniciada);
		strcat(Partidas, PartidaIniciada);
	}
}


int NuevoConectadoPartida(Partida *Partida, char Usuario[80], int FotoPerfil, int Socket) {
	// A�ade Nuevo Conectado. Retorna 0 si ok y -1 si la lista estaba llena.
	int NumeroConectados = Partida->ConectadosPartida.num;
	
	if (NumeroConectados == 4)
		return -1;
	else {
		strcpy(Partida->ConectadosPartida.Conectados[NumeroConectados].Usuario, Usuario);
		Partida->ConectadosPartida.Conectados[NumeroConectados].FotoPerfil = FotoPerfil;
		Partida->ConectadosPartida.Conectados[NumeroConectados].Socket = Socket;
		strcpy(Partida->Puntos[NumeroConectados].Jugador.Usuario, Usuario);
		Partida->Puntos[NumeroConectados].Jugador.FotoPerfil = FotoPerfil;
		Partida->Puntos[NumeroConectados].Jugador.Socket = Socket;
		Partida->ConectadosPartida.num++;
		return 0;
	}
}


int EliminaDesconectadoPartida(Partida *Partida, char Usuario[80]) {
	int posicion = DamePosicion(&Partida->ConectadosPartida, Usuario);
	if (posicion == -1)
		return -1;
	else {
		for (int i = posicion; i < Partida->ConectadosPartida.num - 1; i++)
		{
			Partida->ConectadosPartida.Conectados[i] = Partida->ConectadosPartida.Conectados[i+1];
			Partida->Puntos[i].Jugador = Partida->Puntos[i+1].Jugador;
		}
		Partida->ConectadosPartida.num--;
		return 0;
	}
}


int NuevaPartida(ListaPartidas *Lista, Partida SalaPartida){
	// A�ade Nueva Partida. Retorna 0 si ok y -1 si la lista estaba llena.
	if (Lista->num == 100)
		return -1;
	else {
		Lista->Partidas[Lista->num] = SalaPartida;
		Lista->num++;
		printf(">> Numero de partidas: %d\n", Lista->num);
		return 0;
	}
}


int DamePosicionPartida(ListaPartidas *Lista, char CodigoSala[5]){
	// Devuelve la posicion en la lista o -1 si no est� en la lista.
	int i = 0;
	int encontrado = 0;
	while ((i < Lista->num) && !encontrado)
	{
		if (strcmp(Lista->Partidas[i].CodigoSala, CodigoSala) == 0)
			encontrado = 1;
		else
			i++;
	}
	if (encontrado)
		return i;
	else
		return -1;
}


int NuevoCodigoSala() {
	int CodigoValido = 0;
	int PosibleCodigo;
	
	while (!CodigoValido)
	{
		PosibleCodigo = rand() % 10000;
		int encontrado = 0;
		int i;
		for (i = 0; i < sizeof(CodigosSala)/sizeof(CodigosSala[0]); i++)
		{
			if (CodigosSala[i] == PosibleCodigo)
			{
				encontrado = 1;
				break;
			}
		}
		if (!encontrado) {
			for (i = 0; i < sizeof(CodigosSala)/sizeof(CodigosSala[0]); i++)
			{
				if (CodigosSala[i] == 0)
				{
					CodigosSala[i] = PosibleCodigo;
					CodigoValido = 1;
					break;
				}
			}
			if (!CodigoValido) {
				printf("ERROR: No hay espacio para nuevos códigos de sala.\n");
				return -1;
			}
		}
	}
	return PosibleCodigo;
}

	
void QuitarCodigoSala(int CodigoDeSala) {
	int PosicionCodigo = -1;
	int length = sizeof(CodigosSala)/sizeof(CodigosSala[0]);

	for (int i = 0; i < length; i++)
	{
		if (CodigosSala[i] == CodigoDeSala)
		{
			PosicionCodigo = i;
			break;
		}
		if (CodigosSala[i] == 0)
			break;
	}
	if (PosicionCodigo == -1)
		return;
	for (int i = PosicionCodigo; i < length - 1; i++)
	{
		CodigosSala[i] = CodigosSala[i + 1];
	}
	CodigosSala[length - 1] = 0;
}



int PartidaCerrada(ListaPartidas *Lista, char CodigoSala[5]){
	// Retorna 0 si elimina y -1 si la partida no est� en la lista.
	int posicion = DamePosicionPartida(Lista, CodigoSala);
	if (posicion == -1)
		return -1;
	else {
		for (int i = posicion; i < Lista->num - 1; i++)
		{
			Lista->Partidas[i] = Lista->Partidas[i+1];
		}
		Lista->num--;
		QuitarCodigoSala(atoi(CodigoSala));
		printf(">> Numero de partidas: %d\n", Lista->num);
		return 0;
	}
}


int contarPintor(Partida *Partida, char Usuario[80]) {
	int count = 0;
	for (int i = 0; i < Partida->Palabras; i++) {
		if (strcmp(Partida->Pintores[i], Usuario) == 0) {
			count++;
		}
	}
	return count;
}


int contarCaracteresUTF8(const char* str) {
	int count = 0;
	while (*str) {
		if ((*str & 0xC0) != 0x80) {
			count++;
		}
		str++;
	}
	return count;
}


int BuscaConectado(ListaConectados *Lista, char Usuario[50]) {
	for (int i = 0; i < Lista->num; i++) {
		if (strcmp(Lista->Conectados[i].Usuario, Usuario) == 0) {
			return 0;
		}
	}
	return -1;
}


int obtener_socket_invitado(ListaConectados *jugadores, char invitado[80]) {
	// Recorre todos los jugadores conectados
	for (int i = 0; i < jugadores->num; i++) {
		// Compara el nombre del jugador conectado con el nombre del invitado
		if (strcmp(jugadores->Conectados[i].Usuario, invitado) == 0) {
			return jugadores->Conectados[i].Socket;  // Retorna el socket si se encuentra
		}
	}
	return -1;  // Si no se encuentra el invitado, retorna -1
}


void NotificaConectados(ListaConectados *Lista, char notificacion[512]){
	// Envia a todos los usuarios de la lista de conectados el numero total de jugadores conectados.
	for (int j = 0; j < Lista->num; j++)
	{
		write(Lista->Conectados[j].Socket, notificacion, strlen(notificacion));
	}
}

void NotificaConectadosExcepto(ListaConectados *Lista, char* usuarioExcluir, char notificacion[512]) {
	for (int j = 0; j < Lista->num; j++) {
		printf(">> Evaluando: %s vs %s\n", Lista->Conectados[j].Usuario, usuarioExcluir);
		if (strcmp(Lista->Conectados[j].Usuario, usuarioExcluir) != 0) {
			printf(">> Enviando a %s (socket %d): %s\n", Lista->Conectados[j].Usuario, Lista->Conectados[j].Socket, notificacion);
			write(Lista->Conectados[j].Socket, notificacion, strlen(notificacion));
		}
	}
}


void *AtenderCliente(void *socket) {
    int sock_conn;
    int *s;
    s = (int *)socket;
    sock_conn = *s;

    char peticion[2048];
    char respuesta[10000];
	char conectados[1000];
	char Partidas[1000];
    int ret;
    int terminar = 0;
	int UsuarioConectado = 0;

    // Conectar a la base de datos
    MYSQL *conn;
    conn = mysql_init(NULL);
	mysql_set_character_set(conn, "utf8");
    if (conn == NULL) {
        printf(">> [Error al inicializar MySQL]\n");
        close(sock_conn);
        pthread_exit(NULL);
    }
	// con maquina virtual
    //conn = mysql_real_connect(conn, "localhost", "root", "mysql", "T9_DDBBPinturilloSO", 0, NULL, 0);
	// conn shiva
   	conn = mysql_real_connect(conn, "shiva2.upc.es", "root", "mysql", "T9_DDBBPinturilloSO", 0, NULL, 0);
    if (conn == NULL) {
        printf(">> [Error al conectar a la base de datos: %s]\n", mysql_error(conn));
        close(sock_conn);
        pthread_exit(NULL);
    }

    // Bucle para atender al cliente
    while (terminar == 0) {
        ret = read(sock_conn, peticion, sizeof(peticion));
        printf(">> Recibido\n");

        peticion[ret] = '\0'; // Añadir marca de fin de string
        printf(">> Peticion: %s\n", peticion);

        // Procesar la petición
        char *p = strtok(peticion, "/");
        int codigo = atoi(p);
		
        char Usuario[80];
        char Correo[80];
        char Contrasena[80];
		int FotoPerfil;
		char CodigoDeSala[5];
		char SalaPartida[5000];
		char MensajeTodaSala[15];
		strcpy(MensajeTodaSala, "ParaTodos");
		

       if (codigo == 0) 
	   { // Petición de desconexión
		   terminar = 1;
	   }
	   else if (codigo == 1)
	   {    // Se recibe el nombre, el correo y la contraseña para crear un nuevo usuario
			// Formato '1/Correo/Usuario/Contraseña'
			
			MYSQL_RES *res;
			MYSQL_ROW row;
			char query[2048];
			
			p = strtok(NULL, "/");
			int X = atoi(p);
			
			if (X == 0)
			{
				p = strtok(NULL, "/");
				strcpy(Correo, p);
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				p = strtok(NULL, "/");
				strcpy(Contrasena, p);
				p = strtok(NULL, "/");
				FotoPerfil = atoi(p);
				
				// Comprobar si existe el correo en la base de datos
				sprintf(query, "SELECT * FROM Jugadores WHERE Correo='%s'", Correo);
				if (mysql_query(conn, query)) {
					printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				if (mysql_num_rows(res) > 0) {
					printf(">> [-1/Error: El correo ya esta registrado.]\n");
					sprintf(respuesta, "-1/El correo '%s' ya esta registrado.|", Correo);
					write(sock_conn, respuesta, strlen(respuesta));
					mysql_free_result(res);
					continue;
				}
				mysql_free_result(res);
				
				// Comprobar si existe el usuario
				sprintf(query, "SELECT * FROM Jugadores WHERE Usuario='%s'", Usuario);
				if (mysql_query(conn, query)) {
					printf("Error en la consulta: %s\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				if (mysql_num_rows(res) > 0) {
					printf(">> [-1/Error: El nombre de usuario '%s' ya existe.]\n", Usuario);
					sprintf(respuesta, "-1/El nombre de usuario '%s' ya existe.|", Usuario);
					write(sock_conn, respuesta, strlen(respuesta));
					mysql_free_result(res);
					continue;
				}
				mysql_free_result(res);
				
				// Introducir datos a la base de datos
				sprintf(query,
						"INSERT INTO Jugadores (Usuario, Correo, Contrasena, FotoPerfil) VALUES ('%s', '%s', '%s', %d);",
						Usuario, Correo, Contrasena, FotoPerfil);
				if (mysql_query(conn, query)) {
					printf(">> [Error al insertar usuario: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				
				
				printf(">> 1/0/Registro completado.\n");
				sprintf(respuesta, "1/0/Registro completado.|");
				write(sock_conn, respuesta, strlen(respuesta));
			}
			else if (X == 1)
			{
				p = strtok(NULL, "/");
				strcpy(Correo, p);
				
				// Comprobar si existe el correo en la base de datos
				sprintf(query, "SELECT * FROM Jugadores WHERE Correo='%s'", Correo);
				if (mysql_query(conn, query)) {
					printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				if (mysql_num_rows(res) == 0) {
					printf(">> [-1/Error: El correo no esta registrado.]\n");
					sprintf(respuesta, "-1/El correo no esta registrado.|");
					write(sock_conn, respuesta, strlen(respuesta));
					mysql_free_result(res);
					continue;
				}
				mysql_free_result(res);
				
				sprintf(query,
						"SELECT Usuario, Contrasena FROM Jugadores WHERE Correo = '%s';",
						Correo);
				if (mysql_query(conn, query)) {
					printf(">> [Error al insertar usuario: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				row = mysql_fetch_row(res);
				
				printf(">> 1/1/%s/%s\n", row[0], row[1]);
				sprintf(respuesta, "1/1/%s/%s|", row[0], row[1]);
				write(sock_conn, respuesta, strlen(respuesta));
			}
			else if (X == 2)
			{					
				char NuevoUsuario[80];
				char FraseFavorita[256];
				p = strtok(NULL, "/");
				strcpy(NuevoUsuario, p);
				p = strtok(NULL, "/");
				FotoPerfil = atoi(p);
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				p = strtok(NULL, "/");
				strcpy(FraseFavorita, p);
				
				sprintf(query,
						"UPDATE Jugadores SET Usuario = '%s', FotoPerfil = %d, FraseFavorita = '%s' WHERE Usuario = '%s';",
						NuevoUsuario, FotoPerfil, FraseFavorita, Usuario);
				if (mysql_query(conn, query)) {
					printf(">> [Error al insertar usuario: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				
				printf(">> 1/2/%s/%d/%s\n", NuevoUsuario, FotoPerfil, FraseFavorita	);
				sprintf(respuesta, "1/2/%s/%d/%s|", NuevoUsuario, FotoPerfil, FraseFavorita);
				write(sock_conn, respuesta, strlen(respuesta));
			}
			else if (X == 3)
			{
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				char NuevoAmigo[80];
				p = strtok(NULL, "/");
				strcpy(NuevoAmigo, p);
				
				if (strcmp(Usuario, NuevoAmigo) == 0)
				{
					printf(">> [-1/Error: Solicitud de amistad enviada a uno mismo.]\n");
					sprintf(respuesta, "-1/Solicitud de amistad enviada a uno mismo.|");
				}
				else
				{
					sprintf(query, "SELECT ID FROM Jugadores WHERE Usuario = '%s';", Usuario);
					if (mysql_query(conn, query)) {
						printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
						mysql_close(conn);
						pthread_exit(NULL);
					}
					res = mysql_store_result(conn);
					row = mysql_fetch_row(res);
					int UsuarioID = atoi(row[0]);
					mysql_free_result(res); 
					
					sprintf(query, "SELECT ID FROM Jugadores WHERE Usuario = '%s';", NuevoAmigo);
					if (mysql_query(conn, query)) {
						printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
						mysql_close(conn);
						pthread_exit(NULL);
					}
					res = mysql_store_result(conn);
					row = mysql_fetch_row(res);
					int NuevoAmigoID = atoi(row[0]);
					mysql_free_result(res); 
					
					sprintf(query,
							"SELECT JugadorID FROM Amigos WHERE (JugadorID = %d AND AmigoID = %d) OR (JugadorID = %d AND AmigoID = %d);",
							UsuarioID, NuevoAmigoID, NuevoAmigoID, UsuarioID);
					if (mysql_query(conn, query)) {
						printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
						mysql_close(conn);
						pthread_exit(NULL);
					}
					
					res = mysql_store_result(conn);
					if (mysql_num_rows(res) > 0) {
						printf(">> [-1/Error: '%s' y '%s' ya son amigos.]\n", Usuario, NuevoAmigo);
						sprintf(respuesta, "-1/%s y tu ya sois amigos.\n", NuevoAmigo);
						write(sock_conn, respuesta, strlen(respuesta));
						mysql_free_result(res);
						continue;
					}
					else {
						int socket_invitado = obtener_socket_invitado(&JugadoresConectados, NuevoAmigo);
						printf(">> 1/3/%s\n", Usuario);
						sprintf(respuesta, "1/3/%s|", Usuario);
						write(socket_invitado, respuesta, strlen(respuesta));
					}
					mysql_free_result(res);
				}
			}
			else if (X == 4)
			{
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				char NuevoAmigo[80];
				p = strtok(NULL, "/");
				strcpy(NuevoAmigo, p);

				int UsuarioID = -1, NuevoAmigoID = -1;

				// Obtener ID del Usuario
				sprintf(query, "SELECT ID FROM Jugadores WHERE Usuario = '%s';", Usuario);
				if (mysql_query(conn, query)) {
					printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				if ((row = mysql_fetch_row(res)) != NULL)
					UsuarioID = atoi(row[0]);
				mysql_free_result(res);

				if (UsuarioID == -1) {
					printf(">> [-1/Error: Usuario '%s' no encontrado.]\n", Usuario);
					sprintf(respuesta, "-1/Usuario '%s' no encontrado.|", Usuario);
					write(sock_conn, respuesta, strlen(respuesta));
					continue;
				}

				// Obtener ID del NuevoAmigo
				sprintf(query, "SELECT ID FROM Jugadores WHERE Usuario = '%s';", NuevoAmigo);
				if (mysql_query(conn, query)) {
					printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				if ((row = mysql_fetch_row(res)) != NULL)
					NuevoAmigoID = atoi(row[0]);
				mysql_free_result(res);

				if (NuevoAmigoID == -1) {
					printf(">> [-1/Error: Usuario '%s' no encontrado.]\n", NuevoAmigo);
					sprintf(respuesta, "-1/Usuario '%s' no encontrado.|", NuevoAmigo);
					write(sock_conn, respuesta, strlen(respuesta));
					continue;
				}

				// Comprobar si ya existe la amistad
				sprintf(query,
						"SELECT * FROM Amigos WHERE (JugadorID = %d AND AmigoID = %d) OR (JugadorID = %d AND AmigoID = %d);",
						UsuarioID, NuevoAmigoID, NuevoAmigoID, UsuarioID);
				if (mysql_query(conn, query)) {
					printf(">> [Error al verificar amistad existente: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				if (mysql_num_rows(res) > 0) {
					printf(">> [-1/Error: '%s' y '%s' ya son amigos.]\n", Usuario, NuevoAmigo);
					sprintf(respuesta, "-1/%s y tú ya sois amigos.|", NuevoAmigo);
					write(sock_conn, respuesta, strlen(respuesta));
					mysql_free_result(res);
					continue;
				}
				mysql_free_result(res);

				// Insertar amistad
				sprintf(query, "INSERT INTO Amigos (JugadorID, AmigoID) VALUES (%d, %d);", UsuarioID, NuevoAmigoID);
				if (mysql_query(conn, query)) {
					printf(">> [Error al insertar amistad: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				sprintf(query, "INSERT INTO Amigos (JugadorID, AmigoID) VALUES (%d, %d);", NuevoAmigoID, UsuarioID);
				if (mysql_query(conn, query)) {
					printf(">> [Error al insertar amistad (inversa): %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}

				printf(">> 1/4/%s y %s ahora son amigos.\n", Usuario, NuevoAmigo);
				sprintf(respuesta, "1/4/%s y %s ahora sois amigos.|", Usuario, NuevoAmigo);
				write(sock_conn, respuesta, strlen(respuesta));
			}

			else if (X == 5) 
			{
				p = strtok(NULL, "/");
				if (p == NULL) {
					sprintf(respuesta, "-1/Falta nombre de usuario para eliminar.|");
					write(sock_conn, respuesta, strlen(respuesta));
					break;
				}
				char Usuario[80];
				strcpy(Usuario, p);
				int UsuarioID = -1;

				// Obtener ID de jugador
				sprintf(query, "SELECT ID FROM Jugadores WHERE Usuario = '%s';", Usuario);
				if (mysql_query(conn, query)) {
					printf(">> [Error al obtener ID de jugador: %s]\n", mysql_error(conn));
					sprintf(respuesta, "-1/Error al obtener ID del jugador.|");
					write(sock_conn, respuesta, strlen(respuesta));
					break;
				}
				res = mysql_store_result(conn);
				row = mysql_fetch_row(res);
				if (row)
					UsuarioID = atoi(row[0]);
				mysql_free_result(res);

				if (UsuarioID == -1) {
					printf(">> [-1/Error: Jugador '%s' no encontrado.]\n", Usuario);
					sprintf(respuesta, "-1/Jugador '%s' no encontrado.|", Usuario);
					write(sock_conn, respuesta, strlen(respuesta));
					break;
				}

				// Cerrar sesión del usuario antes de eliminarlo
				pthread_mutex_lock(&mutex);
				int desconectado = EliminarDesconectados(&JugadoresConectados, Usuario);
				pthread_mutex_unlock(&mutex);
				if (desconectado)
					printf(">> [Sesión cerrada para usuario %s antes de eliminar]\n", Usuario);

				// Eliminar jugador
				sprintf(query, "DELETE FROM Jugadores WHERE ID = %d;", UsuarioID);
				if (mysql_query(conn, query)) {
					printf(">> [Error al eliminar jugador %s: %s]\n", Usuario, mysql_error(conn));
					sprintf(respuesta, "-1/Error al eliminar jugador.|");
					write(sock_conn, respuesta, strlen(respuesta));
				} else {
					printf(">> [Jugador %s eliminado exitosamente]\n", Usuario);
					sprintf(respuesta, "5/5/%s/Eliminacion exitosa.|", Usuario);
					write(sock_conn, respuesta, strlen(respuesta));
				}
			}

		}
		else if (codigo == 2) 
	   {    // Se comprueba que el usuario exista en la BBDD y que la contrasena coincida.
			// Formato '3/Usuario/Contrasena'
			// En caso de estar ya conectado, recibe 0 para desconectar.
			MYSQL_RES *res;
			char query[2048];
			char FraseFavorita[256];
			
			p = strtok(NULL, "/");
			int X = atoi(p);
			
			if (X == 0)
			{
				pthread_mutex_lock( &mutex );
				int Desconectado = EliminarDesconectados(&JugadoresConectados, Usuario);
				pthread_mutex_unlock( &mutex);
				UsuarioConectado = 0;
				printf(">> 99/1/%d\n", JugadoresConectados.num);
				sprintf(respuesta, "99/1/%d|", JugadoresConectados.num);
				pthread_mutex_lock( &mutex );
				NotificaConectados(&JugadoresConectados, respuesta);
				pthread_mutex_unlock( &mutex );
			}
			else if (X == 1)
			{
				if (!UsuarioConectado)
				{
					p = strtok(NULL, "/");
					strcpy(Usuario, p);
					p = strtok(NULL, "/");
					strcpy(Contrasena, p);
					
					
					pthread_mutex_lock(&mutex);
					int yaConectado = DamePosicion(&JugadoresConectados, Usuario);
					pthread_mutex_unlock(&mutex);
					if (yaConectado != -1)
					{
						printf(">> [-1/Error: Sesion ya iniciada.]\n");
						sprintf(respuesta,"-1/Sesion ya iniciada.|");
						write(sock_conn, respuesta, strlen(respuesta));
						continue;	
					}
					sprintf(query, "SELECT ID FROM Jugadores WHERE Usuario='%s' AND Contrasena='%s'", Usuario, Contrasena);
					if (mysql_query(conn, query)) {
						printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
						mysql_close(conn);
						pthread_exit(NULL);
					}
					res = mysql_store_result(conn);
					if (mysql_num_rows(res) == 0) {
						printf(">> [-1/Error: El nombre de usuario o la contrasena no son correctos.]\n");
						sprintf(respuesta, "-1/El nombre de usuario o la contrasena no son correctos.|");
						write(sock_conn, respuesta, strlen(respuesta));
						mysql_free_result(res);
						continue;
					}
					
					pthread_mutex_lock( &mutex );
					int Conectado = NuevoConectado(&JugadoresConectados, Usuario, FotoPerfil, sock_conn);
					pthread_mutex_unlock( &mutex );
					DameConectados(&JugadoresConectados, conectados);
					printf(">> 99/1/%d/%s\n", JugadoresConectados.num, conectados);
					sprintf(respuesta, "99/1/%d/%s|", JugadoresConectados.num, conectados);
					pthread_mutex_lock( &mutex );
					NotificaConectados(&JugadoresConectados, respuesta);
					pthread_mutex_unlock( &mutex );
					UsuarioConectado = 1;
					
					sprintf(query,
							"SELECT FotoPerfil, FraseFavorita FROM Jugadores WHERE Usuario='%s';",
							Usuario);
					if (mysql_query(conn, query)) {
						printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
						mysql_close(conn);
						pthread_exit(NULL);
					}
					
					res = mysql_store_result(conn);
					MYSQL_ROW row = mysql_fetch_row(res);
					FotoPerfil = atoi(row[0]);
					strcpy(FraseFavorita, row[1]); 
					
					printf(">> 2/0/Sesion iniciada.\n");
					sprintf(respuesta, "2/0/%d/%s|", FotoPerfil, FraseFavorita);
					write(sock_conn, respuesta, strlen(respuesta));
				}
				else
				{
					DameConectados(&JugadoresConectados, conectados);
					printf(">> 99/1/%d/%s\n", JugadoresConectados.num, conectados);
					sprintf(respuesta, "99/1/%d/%s|", JugadoresConectados.num, conectados);
					pthread_mutex_lock( &mutex );
					NotificaConectados(&JugadoresConectados, respuesta);
					pthread_mutex_unlock( &mutex );
					DamePartidas(&PartidasActivas, Partidas);
					printf(">> 99/2/%d/%s\n", PartidasActivas.num, Partidas);
					sprintf(respuesta, "99/2/%d/%s|", PartidasActivas.num, Partidas);
					write(sock_conn, respuesta, strlen(respuesta));
				}
			}
		}
		else if (codigo == 3)
		{   // Se muestran las relaciones de la BBDD (Ranking, Partidas jugadas, Amigos)
			// Formato '3/X/Usuario', siendo X = (1: Ranking, 2: Partidas Jugadas, 3: Amigos)
			
			p = strtok(NULL, "/");
			int X = atoi(p);
			
			MYSQL_RES *res;
			MYSQL_ROW row;
			char query[2048];
			
			if (X == 1)
			{
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				
				sprintf(query,
					"SELECT ID, Usuario, Puntos "
					"FROM Jugadores "
					"ORDER BY Puntos DESC;");
				
				if (mysql_query(conn, query)) {
					printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				
				char Jugador[80];
				sprintf(respuesta, "3/1/");
				while ((row = mysql_fetch_row(res)) != NULL)
				{					
					int jugadorID = atoi(row[0]);
					strcpy(Jugador, row[1]);
					int Puntos = atoi(row[2]);
					
					sprintf(respuesta, "%s*%s/%d/%d", respuesta, Jugador, Puntos, jugadorID);
				}
				printf(">> %s\n", respuesta);
				sprintf(respuesta, "%s|", respuesta);
				mysql_free_result(res);
				write(sock_conn, respuesta, strlen(respuesta));
			}
			else if (X == 2)
			{
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				
				sprintf(query,
					"SELECT Partidas.Codigo, Partidas.Fecha, Partidas.Privacidad, Partidas.Rondas, "
					"JugadorGanador.Usuario AS NombreGanador, "
					"JugadorPrimero.Usuario AS NombrePrimero, "
					"JugadorSegundo.Usuario AS NombreSegundo, "
					"JugadorTercero.Usuario AS NombreTercero, "
					"JugadorCuarto.Usuario AS NombreCuarto "
					"FROM PartidasJugadas "
					"JOIN Partidas ON PartidasJugadas.PartidaID = Partidas.ID "
					"LEFT JOIN Jugadores AS JugadorPrimero ON PartidasJugadas.PrimeroID = JugadorPrimero.ID "
					"LEFT JOIN Jugadores AS JugadorSegundo ON PartidasJugadas.SegundoID = JugadorSegundo.ID "
					"LEFT JOIN Jugadores AS JugadorTercero ON PartidasJugadas.TerceroID = JugadorTercero.ID "
					"LEFT JOIN Jugadores AS JugadorCuarto ON PartidasJugadas.CuartoID = JugadorCuarto.ID "
					"LEFT JOIN Jugadores AS JugadorGanador ON PartidasJugadas.GanadorID = JugadorGanador.ID "
					"WHERE PartidasJugadas.PrimeroID = (SELECT ID FROM Jugadores WHERE Usuario = '%s') "
					"OR PartidasJugadas.SegundoID = (SELECT ID FROM Jugadores WHERE Usuario = '%s') "
					"OR PartidasJugadas.TerceroID = (SELECT ID FROM Jugadores WHERE Usuario = '%s') "
					"OR PartidasJugadas.CuartoID = (SELECT ID FROM Jugadores WHERE Usuario = '%s');",
					Usuario, Usuario, Usuario, Usuario
				);


				if (mysql_query(conn, query)) {
					printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				
				char CodigoPartida[5];
				char Fecha[20]; 
				char Privacidad[20];
				char Jugador1[80];
				char Jugador2[80];
				char Jugador3[80];
				char Jugador4[80];
				char Ganador[80];
				int Rondas;
				
				sprintf(respuesta, "3/1/");
				while ((row = mysql_fetch_row(res)) != NULL)
				{					
					
					strcpy(CodigoPartida, row[0]);
					strcpy(Fecha, row[1]);
					strcpy(Privacidad, row[2]);
					Rondas = atoi(row[3]);
					if (row[4])
						strcpy(Ganador, row[4]);
					else
						strcpy(Ganador, "-");

					if (row[5])
						strcpy(Jugador1, row[5]);
					else
						strcpy(Jugador1, "-");

					if (row[6])
						strcpy(Jugador2, row[6]);
					else
						strcpy(Jugador2, "-");
					
					if (row[7])
						strcpy(Jugador3, row[7]);
					else
						strcpy(Jugador3, "-");
					
					if (row[8])
						strcpy(Jugador4, row[8]);
					else
						strcpy(Jugador4, "-");

					sprintf(respuesta, "%s*%s/%s/%s/%d/%s/%s/%s/%s/%s", respuesta, CodigoPartida, Fecha, Privacidad, Rondas,
							Ganador, Jugador1, Jugador2, Jugador3, Jugador4);
				}
				mysql_free_result(res);
				printf(">> %s\n", respuesta);
				sprintf(respuesta, "%s|", respuesta);
				write(sock_conn, respuesta, strlen(respuesta));
			}
			else if (X == 3)
			{
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				
				sprintf(query, 
					"SELECT Jugadores.Usuario "
					"FROM Jugadores "
					"JOIN Amigos ON Jugadores.ID = Amigos.AmigoID "
					"WHERE Amigos.JugadorID = (SELECT ID FROM Jugadores WHERE Usuario = '%s') "
					"AND Jugadores.ID != (SELECT ID FROM Jugadores WHERE Usuario = '%s') "
					"UNION "
					"SELECT Jugadores.Usuario "
					"FROM Jugadores "
					"JOIN Amigos ON Jugadores.ID = Amigos.JugadorID "
					"WHERE Amigos.AmigoID = (SELECT ID FROM Jugadores WHERE Usuario = '%s') "
					"AND Jugadores.ID != (SELECT ID FROM Jugadores WHERE Usuario = '%s');",
					Usuario, Usuario, Usuario, Usuario
				);
				
				if (mysql_query(conn, query)) {
					printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				
				res = mysql_store_result(conn);
				sprintf(respuesta, "3/1");
				
				while ((row = mysql_fetch_row(res)) != NULL)
				{
					sprintf(respuesta, "%s/%s", respuesta, row[0]);
				}
				
				printf(">> %s\n", respuesta);
				sprintf(respuesta, "%s|", respuesta);
				mysql_free_result(res);
				write(sock_conn, respuesta, strlen(respuesta));
			}
			else if (X == 4)
			{
				p = strtok(NULL, "/");
				strcpy(CodigoDeSala, p);
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				
				int ranking = 0;
				int jugadorID = 0;
				char FraseFavorita[256] = "";

				sprintf(query, "SELECT ID, FraseFavorita FROM Jugadores WHERE Usuario='%s';", Usuario);
				if (mysql_query(conn, query)) {
					printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				row = mysql_fetch_row(res);
				
				if (row != NULL) {
					jugadorID = atoi(row[0]);
					strcpy(FraseFavorita, row[1]);
				} else {
					printf(">> [Jugador '%s' no encontrado]\n", Usuario);
					sprintf(respuesta, "-1/Jugador no encontrado|");
					write(sock_conn, respuesta, strlen(respuesta));
					mysql_free_result(res);
					continue;
				}
				mysql_free_result(res);
				
				sprintf(query, "SELECT ID FROM Jugadores ORDER BY Puntos DESC;");
				if (mysql_query(conn, query)) {
					printf(">> [Error al obtener ranking: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				
				ranking = 1;
				bool encontrado = false;
				while ((row = mysql_fetch_row(res)) != NULL) {
					int idActual = atoi(row[0]);
					if (idActual == jugadorID) {
						encontrado = true;
						break;
					}
					ranking++;
				}
				mysql_free_result(res);
				
				if (!encontrado)
					ranking = -1;
				if (ranking == -1 || jugadorID == 0) {
					sprintf(respuesta, "-1/Jugador no válido o eliminado|");
					write(sock_conn, respuesta, strlen(respuesta));
					break;
				}
				sprintf(query,
					"SELECT COUNT(*) FROM PartidasJugadas WHERE GanadorID = %d;", jugadorID);
				if (mysql_query(conn, query)) {
					printf(">> [Error al contar partidas ganadas: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				row = mysql_fetch_row(res);
				int partidasGanadas = atoi(row[0]);
				mysql_free_result(res);

				printf(">> 3/4/%s/%s/%d/%d/%s\n", CodigoDeSala, Usuario, ranking, partidasGanadas, FraseFavorita);
				sprintf(respuesta, "3/4/%s/%s/%d/%d/%s|", CodigoDeSala, Usuario, ranking, partidasGanadas, FraseFavorita);
				write(sock_conn, respuesta, strlen(respuesta));
			}

			else if (X == 5)
			{
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				
				int ranking = 0;
				int jugadorID = 0;
				int FotoPerfil;
				char FraseFavorita [256] = "";

				sprintf(query,
					"SELECT ID, FotoPerfil, FraseFavorita FROM Jugadores WHERE Usuario='%s';", Usuario);
				if (mysql_query(conn, query)) {
					printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				row = mysql_fetch_row(res);
				
				if (row != NULL) {
					jugadorID = atoi(row[0]);
					FotoPerfil = atoi(row[1]);
					if (row[2] != NULL)
						strcpy(FraseFavorita, row[2]);
				} else {
					printf(">> [Jugador no encontrado]\n");
					sprintf(respuesta, "-1/Jugador no encontrado|");
					write(sock_conn, respuesta, strlen(respuesta));
					mysql_free_result(res);
					continue;
				}
				mysql_free_result(res);

				sprintf(query, "SELECT ID FROM Jugadores ORDER BY Puntos DESC;");
				if (mysql_query(conn, query)) {
					printf(">> [Error en la consulta de ranking: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				
				ranking = 1;
				bool encontrado = false;
				while ((row = mysql_fetch_row(res)) != NULL) {
					int idActual = atoi(row[0]);
					if (idActual == jugadorID) {
						encontrado = true;
						break;
					}
					ranking++;
				}
				mysql_free_result(res);
				if (!encontrado)
					ranking = -1;

				if (ranking == -1 || jugadorID == 0) {
					sprintf(respuesta, "-1/Jugador no válido o eliminado|");
					write(sock_conn, respuesta, strlen(respuesta));
					break;
				}


				sprintf(query,
					"SELECT COUNT(*) FROM PartidasJugadas WHERE GanadorID = %d;", jugadorID);
				if (mysql_query(conn, query)) {
					printf(">> [Error al contar partidas ganadas: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				row = mysql_fetch_row(res);
				int partidasGanadas = atoi(row[0]);
				mysql_free_result(res);

				int EstadoConectado = BuscaConectado(&JugadoresConectados, Usuario);

				printf(">> 3/5/%d/%d/%d/%d\n", FotoPerfil, ranking, partidasGanadas, EstadoConectado);
				sprintf(respuesta, "3/5/%d/%d/%d/%d/%s|", FotoPerfil, ranking, partidasGanadas, EstadoConectado, FraseFavorita);
				write(sock_conn, respuesta, strlen(respuesta));
			}

			else if (X == 6)
			{
				// 3/6/Usuario/fechaInicio/fechaFin/jugador
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				p = strtok(NULL, "/");
				char fechaInicio[20];
				strcpy(fechaInicio, p);
				p = strtok(NULL, "/");
				char fechaFin[20];
				strcpy(fechaFin, p);
				p = strtok(NULL, "/");
				char jugadorFiltro[80];
				strcpy(jugadorFiltro, p);

				char query[4096];
				char whereClause[2048] = "";
				char condiciones[3][1024];
				int numCondiciones = 0;

				// Siempre filtrar por partidas en las que participó el usuario
				sprintf(condiciones[numCondiciones++],
					"(PartidasJugadas.PrimeroID = (SELECT ID FROM Jugadores WHERE Usuario = '%s') OR "
					"PartidasJugadas.SegundoID = (SELECT ID FROM Jugadores WHERE Usuario = '%s') OR "
					"PartidasJugadas.TerceroID = (SELECT ID FROM Jugadores WHERE Usuario = '%s') OR "
					"PartidasJugadas.CuartoID = (SELECT ID FROM Jugadores WHERE Usuario = '%s'))",
					Usuario, Usuario, Usuario, Usuario);

				// Filtro por fecha
				if (strcmp(fechaInicio, "-") != 0 && strcmp(fechaFin, "-") != 0) {
					sprintf(condiciones[numCondiciones++], "Partidas.Fecha BETWEEN '%s' AND '%s'", fechaInicio, fechaFin);
				}

				// Filtro por jugador
				if (strcmp(jugadorFiltro, "-") != 0) {
					sprintf(condiciones[numCondiciones++],
						"(PartidasJugadas.PrimeroID = (SELECT ID FROM Jugadores WHERE Usuario = '%s') OR "
						"PartidasJugadas.SegundoID = (SELECT ID FROM Jugadores WHERE Usuario = '%s') OR "
						"PartidasJugadas.TerceroID = (SELECT ID FROM Jugadores WHERE Usuario = '%s') OR "
						"PartidasJugadas.CuartoID = (SELECT ID FROM Jugadores WHERE Usuario = '%s'))",
						jugadorFiltro, jugadorFiltro, jugadorFiltro, jugadorFiltro);
				}

				// Construir cláusula WHERE final
				strcpy(whereClause, condiciones[0]);
				for (int i = 1; i < numCondiciones; i++) {
					strcat(whereClause, " AND ");
					strcat(whereClause, condiciones[i]);
				}

				sprintf(query,
					"SELECT Partidas.Codigo, Partidas.Fecha, Partidas.Privacidad, Partidas.Rondas, "
					"JugadorGanador.Usuario AS NombreGanador, "
					"JugadorPrimero.Usuario AS NombrePrimero, "
					"JugadorSegundo.Usuario AS NombreSegundo, "
					"JugadorTercero.Usuario AS NombreTercero, "
					"JugadorCuarto.Usuario AS NombreCuarto "
					"FROM PartidasJugadas "
					"JOIN Partidas ON PartidasJugadas.PartidaID = Partidas.ID "
					"LEFT JOIN Jugadores AS JugadorPrimero ON PartidasJugadas.PrimeroID = JugadorPrimero.ID "
					"LEFT JOIN Jugadores AS JugadorSegundo ON PartidasJugadas.SegundoID = JugadorSegundo.ID "
					"LEFT JOIN Jugadores AS JugadorTercero ON PartidasJugadas.TerceroID = JugadorTercero.ID "
					"LEFT JOIN Jugadores AS JugadorCuarto ON PartidasJugadas.CuartoID = JugadorCuarto.ID "
					"LEFT JOIN Jugadores AS JugadorGanador ON PartidasJugadas.GanadorID = JugadorGanador.ID "
					"WHERE %s;", whereClause);

				if (mysql_query(conn, query)) {
					printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}

				res = mysql_store_result(conn);

				char CodigoPartida[6];
				char Fecha[20];
				char Privacidad[20];
				char Jugador1[80];
				char Jugador2[80];
				char Jugador3[80];
				char Jugador4[80];
				char Ganador[80];
				int Rondas;

				sprintf(respuesta, "3/6/");
				while ((row = mysql_fetch_row(res)) != NULL)
				{
					strcpy(CodigoPartida, row[0]);
					strcpy(Fecha, row[1]);
					strcpy(Privacidad, row[2]);
					Rondas = atoi(row[3]);

					if (row[4]) strcpy(Ganador, row[4]); else strcpy(Ganador, "-");
					if (row[5]) strcpy(Jugador1, row[5]); else strcpy(Jugador1, "-");
					if (row[6]) strcpy(Jugador2, row[6]); else strcpy(Jugador2, "-");
					if (row[7]) strcpy(Jugador3, row[7]); else strcpy(Jugador3, "-");
					if (row[8]) strcpy(Jugador4, row[8]); else strcpy(Jugador4, "-");

					sprintf(respuesta, "%s*%s/%s/%s/%d/%s/%s/%s/%s/%s", respuesta, CodigoPartida, Fecha, Privacidad, Rondas,
							Ganador, Jugador1, Jugador2, Jugador3, Jugador4);
				}

				mysql_free_result(res);
				printf(">> %s\n", respuesta);
				sprintf(respuesta, "%s|", respuesta);
				write(sock_conn, respuesta, strlen(respuesta));
			}


		}
		else if (codigo == 4)
		{
			p = strtok(NULL, "/");
			int X = atoi(p);
			
			if (X == 0)
			{
				p = strtok(NULL, "/");
				int CodigoDeSala = atoi(p);
				
				pthread_mutex_lock(&mutex);
				QuitarCodigoSala(CodigoDeSala);
				pthread_mutex_unlock(&mutex);
			}
			else if (X == 1)
			{
				pthread_mutex_lock( &mutex );
				int PosibleCodigo = NuevoCodigoSala();
				pthread_mutex_unlock( &mutex );
				sprintf(CodigoDeSala, "%04d", PosibleCodigo);
				printf(">> 4/%s\n", CodigoDeSala);
				sprintf(respuesta, "4/%s|", CodigoDeSala);
				write(sock_conn, respuesta, strlen(respuesta));
			}
		}
		else if (codigo == 5)
		{
			p = strtok(NULL, "/");
			int X = atoi(p);
			
			MYSQL_RES *res;
			MYSQL_ROW row;
			char query[2048];
			
			if (X == 0)
			{
				p = strtok(NULL, "/");
				strcpy(CodigoDeSala, p);
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				
				pthread_mutex_lock(&mutex);
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				pthread_mutex_unlock(&mutex);
				
				if (Posicion != -1)
				{
					pthread_mutex_lock( &mutex );
					int EliminaJugadorPartida = EliminaDesconectadoPartida(&PartidasActivas.Partidas[Posicion], Usuario);
					pthread_mutex_unlock( &mutex );
					if (EliminaJugadorPartida != -1)
					{
						sprintf(SalaPartida, "%d/%d/%d/%s/%s/%s/%s", 
								PartidasActivas.Partidas[Posicion].MaximoJugadores,
								PartidasActivas.Partidas[Posicion].ConectadosPartida.num,
								PartidasActivas.Partidas[Posicion].Rondas,
								PartidasActivas.Partidas[Posicion].CodigoSala,
								PartidasActivas.Partidas[Posicion].Categoria,
								PartidasActivas.Partidas[Posicion].Dificultad,
								PartidasActivas.Partidas[Posicion].Privacidad);
						
						for (int i = 0; i < PartidasActivas.Partidas[Posicion].ConectadosPartida.num; i++)
						{
							sprintf(SalaPartida, "%s/%s/%d", SalaPartida, 
									PartidasActivas.Partidas[Posicion].ConectadosPartida.Conectados[i].Usuario,
									PartidasActivas.Partidas[Posicion].ConectadosPartida.Conectados[i].FotoPerfil);
						}
						
						if (PartidasActivas.Partidas[Posicion].ConectadosPartida.num == 0)
						{
							pthread_mutex_lock( &mutex );
							PartidaCerrada(&PartidasActivas, CodigoDeSala);
							pthread_mutex_unlock( &mutex );
						}
						
						printf(">> 5/3/%s\n", SalaPartida);
						sprintf(respuesta, "5/3/%s|", SalaPartida);
						
						pthread_mutex_lock( &mutex );
						NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
						pthread_mutex_unlock( &mutex );

						printf(">> 5/4/%s/%s/EL JUGADOR %s HA ABANDONADO LA SALA./%d\n", CodigoDeSala, MensajeTodaSala, Usuario, 0);
						sprintf(respuesta, "5/4/%s/%s/EL JUGADOR %s HA ABANDONADO LA SALA./%d|", CodigoDeSala, MensajeTodaSala, Usuario, 0);
						
						for (int i = 19; i > 0; i--) {
							strcpy(PartidasActivas.Partidas[Posicion].Ultimos20Mensajes[i], 
								   PartidasActivas.Partidas[Posicion].Ultimos20Mensajes[i - 1]);
						}
						
						char MensajeUsuario[1080];
						sprintf(MensajeUsuario, "%s/%s/EL JUGADOR %s HA ABANDONADO LA SALA./%d", CodigoDeSala, MensajeTodaSala, Usuario, 0);
						strcpy(PartidasActivas.Partidas[Posicion].Ultimos20Mensajes[0], MensajeUsuario);
						
						pthread_mutex_lock( &mutex );
						NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
						pthread_mutex_unlock( &mutex );

						DamePartidas(&PartidasActivas, Partidas);
						printf(">> 99/2/%d/%s\n", PartidasActivas.num, Partidas);
						sprintf(respuesta, "99/2/%d/%s|", PartidasActivas.num, Partidas);
						pthread_mutex_lock( &mutex );
						NotificaConectados(&JugadoresConectados, respuesta);
						pthread_mutex_unlock( &mutex );
					}
				}
				else
				{
					printf(">> [-1/Error: Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.|");
					write(sock_conn, respuesta, strlen(respuesta));
				}
			}
			else if (X == 1)
			{
				Partida PartidaNueva;
				p = strtok(NULL, "/");
				PartidaNueva.MaximoJugadores = atoi(p);
				p = strtok(NULL, "/");
				PartidaNueva.Rondas = atoi(p);
				p = strtok(NULL, "/");
				strcpy(PartidaNueva.CodigoSala, p);
				p = strtok(NULL, "/");
				strcpy(PartidaNueva.Categoria, p);
				p = strtok(NULL, "/");
				strcpy(PartidaNueva.Dificultad, p);
				p = strtok(NULL, "/");
				strcpy(PartidaNueva.Privacidad, p);
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				p = strtok(NULL, "/");
				FotoPerfil = atoi(p);
				
				pthread_mutex_lock( &mutex );
				NuevoConectadoPartida(&PartidaNueva, Usuario, FotoPerfil, sock_conn);
				NuevaPartida(&PartidasActivas, PartidaNueva);
				pthread_mutex_unlock( &mutex );

				printf(">> 5/4/%s/%s/EL JUGADOR %s HA CREADO LA SALA./%d\n", PartidaNueva.CodigoSala, MensajeTodaSala, Usuario, 0);
				sprintf(respuesta, "5/4/%s/%s/EL JUGADOR %s HA CREADO LA SALA./%d|", PartidaNueva.CodigoSala, MensajeTodaSala, Usuario, 0);
				
				pthread_mutex_lock( &mutex );
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
				
				
				PartidasActivas.Partidas[Posicion].Palabras = 0;
				PartidasActivas.Partidas[Posicion].TotalPintores = 0;
				PartidasActivas.Partidas[Posicion].TotalVotaciones = 0;
				PartidasActivas.Partidas[Posicion].PartidaIniciada = 0;
				pthread_mutex_unlock( &mutex );

				for (int i = 0; i < 20; i++) {
					PartidasActivas.Partidas[Posicion].Ultimos20Mensajes[i][0] = '\0';					
				}
				
				for (int i = 19; i > 0; i--) {
					strcpy(PartidasActivas.Partidas[Posicion].Ultimos20Mensajes[i], 
						   PartidasActivas.Partidas[Posicion].Ultimos20Mensajes[i - 1]);
				}
				
				char MensajeUsuario[1080];
				sprintf(MensajeUsuario, "%s/%s/EL JUGADOR %s HA CREADO LA SALA./%d", PartidaNueva.CodigoSala, MensajeTodaSala, Usuario, 0);
				strcpy(PartidasActivas.Partidas[Posicion].Ultimos20Mensajes[0], MensajeUsuario);
				
				DamePartidas(&PartidasActivas, Partidas);
				sprintf(respuesta, "99/2/%d/%s|", PartidasActivas.num, Partidas);
				printf(">> %s", respuesta);
				pthread_mutex_lock( &mutex );
				NotificaConectados(&JugadoresConectados, respuesta);
				pthread_mutex_unlock( &mutex );
			}
			else if (X == 2)
			{
				p = strtok(NULL, "/");
				strcpy(CodigoDeSala, p);
				
				pthread_mutex_lock( &mutex );
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				pthread_mutex_unlock( &mutex );
				
				if (Posicion != -1)
				{
					int yaEstaEnSala = 0;
					for (int i = 0; i < PartidasActivas.Partidas[Posicion].ConectadosPartida.num; i++) {
						if (strcmp(PartidasActivas.Partidas[Posicion].ConectadosPartida.Conectados[i].Usuario, Usuario) == 0) {
							yaEstaEnSala = 1;
							break;
						}
					}
					if (yaEstaEnSala) {
						printf(">> [-1/Error: El usuario ya esta en la sala.]\n");
						sprintf(respuesta, "-1/El usuario ya esta en la sala.|");
						write(sock_conn, respuesta, strlen(respuesta));
					}
					else if (PartidasActivas.Partidas[Posicion].PartidaIniciada == 1)
					{
						printf(">> [-1/Error: Partida ya iniciada.]\n");
						sprintf(respuesta, "-1/Partida ya iniciada.|");
						write(sock_conn, respuesta, strlen(respuesta));
					}
					else if (PartidasActivas.Partidas[Posicion].ConectadosPartida.num == PartidasActivas.Partidas[Posicion].MaximoJugadores)
					{
						printf(">> [-1/Error: Sala de partida llena.]\n");
						sprintf(respuesta, "-1/Sala de partida llena.|");
						write(sock_conn, respuesta, strlen(respuesta));
					}
					else
					{
						sprintf(SalaPartida, "%d/%d/%d/%s/%s/%s/%s", 
								PartidasActivas.Partidas[Posicion].MaximoJugadores,
								PartidasActivas.Partidas[Posicion].ConectadosPartida.num,
								PartidasActivas.Partidas[Posicion].Rondas,
								PartidasActivas.Partidas[Posicion].CodigoSala,
								PartidasActivas.Partidas[Posicion].Categoria,
								PartidasActivas.Partidas[Posicion].Dificultad,
								PartidasActivas.Partidas[Posicion].Privacidad);
						
						for (int i = 0; i < PartidasActivas.Partidas[Posicion].ConectadosPartida.num; i++)
						{
							sprintf(SalaPartida, "%s/%s/%d", SalaPartida, 
									PartidasActivas.Partidas[Posicion].ConectadosPartida.Conectados[i].Usuario,
									PartidasActivas.Partidas[Posicion].ConectadosPartida.Conectados[i].FotoPerfil);
						}
						
						printf(">> 5/2/%s\n", SalaPartida);
						sprintf(respuesta, "5/2/%s|", SalaPartida);
						write(sock_conn, respuesta, strlen(respuesta));
						printf(">> 5/4/%s/%s/EL JUGADOR %s SE HA UNIDO A LA SALA./%d\n", CodigoDeSala, MensajeTodaSala, Usuario, 0);
						sprintf(respuesta, "5/4/%s/%s/EL JUGADOR %s SE HA UNIDO A LA SALA./%d|", CodigoDeSala, MensajeTodaSala, Usuario, 0);
						pthread_mutex_lock( &mutex );
						NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
						pthread_mutex_unlock( &mutex );
						
						for (int i = 19; i > 0; i--) {
							strcpy(PartidasActivas.Partidas[Posicion].Ultimos20Mensajes[i], 
								   PartidasActivas.Partidas[Posicion].Ultimos20Mensajes[i - 1]);
						}
						
						char MensajeUsuario[10000];
						sprintf(MensajeUsuario, "%s/%s/EL JUGADOR %s SE HA UNIDO A LA SALA./%d", CodigoDeSala, MensajeTodaSala, Usuario, 0);
						strcpy(PartidasActivas.Partidas[Posicion].Ultimos20Mensajes[0], MensajeUsuario);
					};
				}
				else
				{
					printf(">> [-1/Error: Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.|");
					write(sock_conn, respuesta, strlen(respuesta));
				}
			}
			else if (X == 3)
			{
				p = strtok(NULL, "/");
				strcpy(CodigoDeSala, p);
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				p = strtok(NULL, "/");
				FotoPerfil = atoi(p);
				
				pthread_mutex_lock( &mutex );
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				int NuevoJugadorPartida = NuevoConectadoPartida(&PartidasActivas.Partidas[Posicion], 
														 Usuario, FotoPerfil, sock_conn);
				pthread_mutex_unlock( &mutex );
				
				if (Posicion != -1)
				{
					char MensajeUsuario[10000];
					strcpy(respuesta, "5/4");
					for (int i = 19; i >= 0; i--)
					{
						strcpy(MensajeUsuario, PartidasActivas.Partidas[Posicion].Ultimos20Mensajes[i]);
						if (MensajeUsuario[0] != '\0')	
						{				
							sprintf(respuesta, "%s/%s", respuesta, MensajeUsuario);
						}
					}
					sprintf(respuesta, "%s|", respuesta);
					printf(">> %s\n", respuesta);
					int socket_jugador = obtener_socket_invitado(&JugadoresConectados, Usuario);
					write(socket_jugador, respuesta, strlen(respuesta));
				
					sprintf(SalaPartida, "%d/%d/%d/%s/%s/%s/%s", 
							PartidasActivas.Partidas[Posicion].MaximoJugadores,
							PartidasActivas.Partidas[Posicion].ConectadosPartida.num,
							PartidasActivas.Partidas[Posicion].Rondas,
							PartidasActivas.Partidas[Posicion].CodigoSala,
							PartidasActivas.Partidas[Posicion].Categoria,
							PartidasActivas.Partidas[Posicion].Dificultad,
							PartidasActivas.Partidas[Posicion].Privacidad);
					
					for (int i = 0; i < PartidasActivas.Partidas[Posicion].ConectadosPartida.num; i++)
					{
						sprintf(SalaPartida, "%s/%s/%d", SalaPartida, 
								PartidasActivas.Partidas[Posicion].ConectadosPartida.Conectados[i].Usuario,
								PartidasActivas.Partidas[Posicion].ConectadosPartida.Conectados[i].FotoPerfil);
					}
					
					printf(">> 5/3/%s\n", SalaPartida);
					sprintf(respuesta, "5/3/%s|", SalaPartida);
					pthread_mutex_lock( &mutex );
					NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
					pthread_mutex_unlock( &mutex );
				}
				else
				{
					printf(">> [-1/Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.|");
					write(sock_conn, respuesta, strlen(respuesta));
				}
			}
			else if (X == 4)
			{
				char MensajeChat[5000];
				
				p = strtok(NULL, "/");
				strcpy(CodigoDeSala, p);
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				p = strtok(NULL, "/");
				strcpy(MensajeChat, p);
				p = strtok(NULL, "/");
				int PalabraAcertada = atoi(p);
				
				pthread_mutex_lock( &mutex );
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				pthread_mutex_unlock( &mutex );
				
				if (Posicion != -1)
				{				
					char Palabra[80];
					int TotalPalabras = PartidasActivas.Partidas[Posicion].Palabras;
					strcpy(Palabra, PartidasActivas.Partidas[Posicion].PalabrasPorAcertar[TotalPalabras - 1]);
					char Pintor[80];
					int TotalPintores = PartidasActivas.Partidas[Posicion].TotalPintores;
					strcpy(Pintor, PartidasActivas.Partidas[Posicion].Pintores[TotalPintores - 1]);
					int PartidaIniciada = PartidasActivas.Partidas[Posicion].PartidaIniciada;
					
					int Acertada = 0;
					if (strcasecmp(MensajeChat, Palabra) == 0){
						Acertada = 1;
					}
					
					if (Acertada == 1 && PartidaIniciada && PalabraAcertada || strcmp(Pintor, Usuario) == 0 && Acertada == 1){
						strcpy(MensajeChat, Palabra);
						for (int i = 0; i < strlen(Palabra); i++){
							MensajeChat[i] = '*';
						}
						Acertada = 0;
					}
					
					for (int i = 20; i > 0; i--) {
						strcpy(PartidasActivas.Partidas[Posicion].Ultimos20Mensajes[i], 
							   PartidasActivas.Partidas[Posicion].Ultimos20Mensajes[i - 1]);
					}
					
					char MensajeUsuario[1080];
					sprintf(MensajeUsuario, "%s/%s/%s/%d", CodigoDeSala, Usuario, MensajeChat, Acertada);
					strcpy(PartidasActivas.Partidas[Posicion].Ultimos20Mensajes[0], MensajeUsuario);
					
					printf(">> 5/4/%s/%s/%s/%d\n", CodigoDeSala, Usuario, MensajeChat, Acertada);
					sprintf(respuesta, "5/4/%s/%s/%s/%d|", CodigoDeSala, Usuario, MensajeChat, Acertada);
					pthread_mutex_lock( &mutex );
					NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
					pthread_mutex_unlock( &mutex );
				}
				else
				{
					printf(">> [-1/Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.|");
					write(sock_conn, respuesta, strlen(respuesta));
				}
			}
				
			else if (X == 5)
			{
				p = strtok(NULL, "/");
				strcpy(CodigoDeSala, p);
				p = strtok(NULL, "/");
				int CodigoDibujo = atoi(p);
				p = strtok(NULL, "/");
				int DibujoX = atoi(p);
				p = strtok(NULL, "/");
				int DibujoY = atoi(p);
				p = strtok(NULL, "/");
				int Color = atoi(p);
				p = strtok(NULL, "/");
				char Grosor[20];
				strcpy(Grosor, p);
				
				pthread_mutex_lock( &mutex );
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				pthread_mutex_unlock( &mutex );
				
				if (Posicion != -1)
				{				
					//printf(">> 5/5/%d/%d/%d\n", CodigoDibujo, DibujoX, DibujoY);
					sprintf(respuesta, "5/5/%s/%d/%d/%d/%d/%s|", CodigoDeSala, CodigoDibujo, DibujoX, DibujoY, Color, Grosor);
					pthread_mutex_lock( &mutex );
					NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
					pthread_mutex_unlock( &mutex );
				}
				else
				{
					printf(">> [-1/Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.|");
					write(sock_conn, respuesta, strlen(respuesta));
				}
			}
			else if (X == 6)
			{
				
				p = strtok(NULL, "/");
				strcpy(CodigoDeSala, p);
			
				pthread_mutex_lock(&mutex);
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				
				pthread_mutex_unlock(&mutex);
				
				
				
				
				if (Posicion != -1)
				{
					
					PartidasActivas.Partidas[Posicion].NumAcertantesActuales = 0;
					for (int i = 0; i < 5; i++) {
						strcpy(PartidasActivas.Partidas[Posicion].Acertantes[i], "");
					}
					int TotalPintores = PartidasActivas.Partidas[Posicion].TotalPintores;
					char Pintor[80];
					
					char Palabra[500];
					char PalabrasRonda[3][500];
					int TotalPalabrasRondas = 0;
					int TotalPalabras = PartidasActivas.Partidas[Posicion].Palabras;
					
					if (PartidasActivas.Partidas[Posicion].PartidaIniciada == 0){
						PartidasActivas.Partidas[Posicion].PartidaIniciada = 1;
					}
				
					srand(time(NULL));
					int NumeroJugadores = PartidasActivas.Partidas[Posicion].ConectadosPartida.num;
					int numeroRandom = rand() % NumeroJugadores;
					
					if (PartidasActivas.Partidas[Posicion].Pintores > 0 &&
						(strcmp(PartidasActivas.Partidas[Posicion].ConectadosPartida.Conectados[numeroRandom].Usuario, 
								PartidasActivas.Partidas[Posicion].Pintores[PartidasActivas.Partidas[Posicion].Palabras]) == 0 ||
						 contarPintor(&PartidasActivas.Partidas[Posicion], PartidasActivas.Partidas[Posicion].ConectadosPartida.Conectados[numeroRandom].Usuario) >= PartidasActivas.Partidas[Posicion].Rondas)) {
						
						do {
							numeroRandom = rand() % NumeroJugadores;
						} 
						while (strcmp(PartidasActivas.Partidas[Posicion].ConectadosPartida.Conectados[numeroRandom].Usuario, PartidasActivas.Partidas[Posicion].Pintores[PartidasActivas.Partidas[Posicion].Palabras]) == 0||
							   contarPintor(&PartidasActivas.Partidas[Posicion], PartidasActivas.Partidas[Posicion].ConectadosPartida.Conectados[numeroRandom].Usuario) > PartidasActivas.Partidas[Posicion].Rondas);
					}
					strcpy(Pintor, PartidasActivas.Partidas[Posicion].ConectadosPartida.Conectados[numeroRandom].Usuario);
					strcpy(PartidasActivas.Partidas[Posicion].Pintores[TotalPintores], Pintor);
					PartidasActivas.Partidas[Posicion].TotalPintores++;
					
					
					while (TotalPalabrasRondas < 3)
					{
						
						int PalabraEncontrada = 0;
						while (!PalabraEncontrada)
						{
							int Dificultad;
							
							if (strcmp(PartidasActivas.Partidas[Posicion].Dificultad, "Cualquiera") == 0)
							{
								Dificultad = 0;
							}
							else if (strcmp(PartidasActivas.Partidas[Posicion].Dificultad, "Facil") == 0)
							{
								Dificultad = 1;
							}
							else if (strcmp(PartidasActivas.Partidas[Posicion].Dificultad, "Normal") == 0)
							{
								Dificultad = 2;
							}
							else if (strcmp(PartidasActivas.Partidas[Posicion].Dificultad, "Dificil") == 0)
							{
								Dificultad = 3;
							}
							
							if (strcmp(PartidasActivas.Partidas[Posicion].Categoria, "Cualquiera") != 0 && Dificultad != 0)
							{
								sprintf(query,
										"SELECT Dibujo FROM Dibujos WHERE Categoria = '%s' AND Dificultad = %d ORDER BY RAND();",
										PartidasActivas.Partidas[Posicion].Categoria, Dificultad);
							}
							else if (strcmp(PartidasActivas.Partidas[Posicion].Categoria, "Cualquiera") != 0)
							{
								sprintf(query,
										"SELECT Dibujo FROM Dibujos WHERE Categoria = '%s' ORDER BY RAND();",
										PartidasActivas.Partidas[Posicion].Categoria);
							}
							else if (Dificultad != 0)
							{
								sprintf(query,
										"SELECT Dibujo FROM Dibujos WHERE Dificultad = %d ORDER BY RAND();",
										Dificultad);
							}
							else{
								sprintf(query,
										"SELECT Dibujo FROM Dibujos ORDER BY RAND();");
							}
							
							if (mysql_query(conn, query)) {
								printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
								mysql_close(conn);
								pthread_exit(NULL);
							}
							
							res = mysql_store_result(conn);
							MYSQL_ROW row = mysql_fetch_row(res);
							strcpy(Palabra, row[0]);
							
							int repetida = 0;
							for (int i = 0; i < TotalPalabras; i++) {
								if (strcmp(PartidasActivas.Partidas[Posicion].PalabrasPorAcertar[i], Palabra) == 0) {
									repetida = 1;
									break;
								}
							}
							
							if (!repetida) {
								for (int j = 0; j < TotalPalabrasRondas; j++) {
									if (strcmp(PalabrasRonda[j], Palabra) == 0) {
										repetida = 1;
										break;
									}
								}
							}
							
							if (!repetida) {
								strcpy(PalabrasRonda[TotalPalabrasRondas++], Palabra);
								PalabraEncontrada = 1;
							}
						}
					}
					
					printf(">> 5/6/%s/%s/%s/%s/%s\n", CodigoDeSala, Pintor, PalabrasRonda[0], PalabrasRonda[1], PalabrasRonda[2]);
					sprintf(respuesta, "5/6/%s/%s/%s/%s/%s|", CodigoDeSala, Pintor, PalabrasRonda[0], PalabrasRonda[1], PalabrasRonda[2]);
					pthread_mutex_lock( &mutex );
					NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
					pthread_mutex_unlock( &mutex );
				}
				else
				{
					printf(">> [-1/Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.|");
					write(sock_conn, respuesta, strlen(respuesta));
				}
			
			}
			else if (X == 7)
			{
				p = strtok(NULL, "/");
				strcpy(CodigoDeSala, p);
				char Palabra[500];
				p = strtok(NULL, "/");
				strcpy(Palabra, p);
				
				pthread_mutex_lock( &mutex );
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				pthread_mutex_unlock( &mutex );
		
				if (Posicion != -1)
				{			
					PartidasActivas.Partidas[Posicion].NumAcertantesActuales = 0;
					int TotalPalabras = PartidasActivas.Partidas[Posicion].Palabras;
					strcpy(PartidasActivas.Partidas[Posicion].PalabrasPorAcertar[TotalPalabras++], Palabra);
					PartidasActivas.Partidas[Posicion].Palabras++;
				
					int totalIndices = contarCaracteresUTF8(Palabra);
					int indicesAleatorios[totalIndices];
					for (int j = 0; j < totalIndices; j++) {
						indicesAleatorios[j] = j;
					}
					
					for (int j = totalIndices - 1; j > 0; j--) {
						int k = rand() % (j + 1);
						int temp = indicesAleatorios[j];
						indicesAleatorios[j] = indicesAleatorios[k];
						indicesAleatorios[k] = temp;
					}
					
					sprintf(respuesta, "5/7/%s/%s/%d", CodigoDeSala, Palabra, totalIndices);
					for (int i = 0; i < totalIndices; i++)
					{
						sprintf(respuesta, "%s/%d", respuesta, indicesAleatorios[i]);
					}
					printf(">> %s\n", respuesta);
					sprintf(respuesta, "%s|", respuesta);
					pthread_mutex_lock( &mutex );
					NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
					pthread_mutex_unlock( &mutex );
				}
				else
				{
					printf(">> [-1/Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.|");
					write(sock_conn, respuesta, strlen(respuesta));
				}
			}
			else if (X == 8)
			{
				p = strtok(NULL, "/");
				char invitador[80];
				strcpy(invitador, p);
				p = strtok(NULL, "/");
				char invitado[80];
				strcpy(invitado, p);
				p = strtok(NULL, "/");
				strcpy(CodigoDeSala, p);
				
				int socket_invitado = obtener_socket_invitado(&JugadoresConectados, invitado);
				
				if (socket_invitado == -1) 
				{
					printf("Error: El jugador %s no esta conectado.\n", invitado);
				} 
				else 
				{
					char respuesta[100];
					sprintf(respuesta, "5/8/%s/%s|", invitador, CodigoDeSala); 
					write(socket_invitado, respuesta, strlen(respuesta));
					printf(">> Invitacion enviada de %s a %s.\n", invitador, invitado);
				}
			}
			else if (X == 9)
			{
				p = strtok(NULL, "/");
				strcpy(CodigoDeSala, p);
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				p = strtok(NULL, "/");
				int TiempoRonda = atoi(p);

				pthread_mutex_lock(&mutex);
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				pthread_mutex_unlock(&mutex);

				if (Posicion != -1)
				{
					pthread_mutex_lock(&mutex);

					int Puntos = (TiempoRonda >= 60) ? 60 : TiempoRonda;

					int NumeroJugador = DamePosicion(&PartidasActivas.Partidas[Posicion].ConectadosPartida, Usuario);
					pthread_mutex_unlock(&mutex);
					if (NumeroJugador == -1) {
						
						sprintf(respuesta, "-1/Usuario no encontrado.|");
						write(sock_conn, respuesta, strlen(respuesta));
						return;
					}

					PartidasActivas.Partidas[Posicion].Puntos[NumeroJugador].Puntos += Puntos;
					int PuntosUsuario = PartidasActivas.Partidas[Posicion].Puntos[NumeroJugador].Puntos;

					int PintoresPartida = PartidasActivas.Partidas[Posicion].TotalPintores;
					char Pintor[80];
					strcpy(Pintor, PartidasActivas.Partidas[Posicion].Pintores[PintoresPartida - 1]);

					int NumeroPintor = DamePosicion(&PartidasActivas.Partidas[Posicion].ConectadosPartida, Pintor);
					if (NumeroPintor != -1) {
						PartidasActivas.Partidas[Posicion].Puntos[NumeroPintor].Puntos += 5;
					}

					int PuntosPintor = (NumeroPintor != -1) ? PartidasActivas.Partidas[Posicion].Puntos[NumeroPintor].Puntos : 0;

					printf(">> 5/9/%s/%s/%d/%s/%d\n", CodigoDeSala, Usuario, PuntosUsuario, Pintor, PuntosPintor);
					sprintf(respuesta, "5/9/%s/%s/%d/%s/%d|", CodigoDeSala, Usuario, PuntosUsuario, Pintor, PuntosPintor);
					NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);

				}
				else
				{
					printf(">> [-1/Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.|");
					write(sock_conn, respuesta, strlen(respuesta));
				}
			}

			else if (X == 10)
			{
				printf(">> [DEBUG] Entrando en el bloque X == 10\n");
				p = strtok(NULL, "/");
				strcpy(CodigoDeSala, p);
				char Votante[80];
				p = strtok(NULL, "/");
				strcpy(Votante, p);
				char Votado[80];
				p = strtok(NULL, "/");
				strcpy(Votado, p);
				p = strtok(NULL, "/");
				int Host = atoi(p);
				
				pthread_mutex_lock(&mutex);
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				pthread_mutex_unlock(&mutex);
				
				if (Posicion != -1) 
				{
					if (Host == 0) {
						if (PartidasActivas.Partidas[Posicion].ConectadosPartida.num > 2) {
							int votoRepetido = 0;
							for (int i = 0; i < PartidasActivas.Partidas[Posicion].TotalVotaciones; i++) {
								if (strcmp(PartidasActivas.Partidas[Posicion].VotacionesDeExpulsion[i].Votante, Votante) == 0) {
									votoRepetido = 1;
									printf(">> [-1/Votacion repetida.]\n");
									sprintf(respuesta, "-1/Ya has votado para expulsar a este jugador.|");
									
									write(sock_conn, respuesta, strlen(respuesta));
									break;
								}
							}
							if (!votoRepetido) {
								strcpy(PartidasActivas.Partidas[Posicion].VotacionesDeExpulsion[PartidasActivas.Partidas[Posicion].TotalVotaciones].Votante, Votante);
								strcpy(PartidasActivas.Partidas[Posicion].VotacionesDeExpulsion[PartidasActivas.Partidas[Posicion].TotalVotaciones].Votado, Votado);
								PartidasActivas.Partidas[Posicion].TotalVotaciones++;
				
								
								int votos = 0;
								for (int i = 0; i < PartidasActivas.Partidas[Posicion].TotalVotaciones; i++) {
									if (strcmp(PartidasActivas.Partidas[Posicion].VotacionesDeExpulsion[i].Votado, Votado) == 0)
										votos++;
								}
								
								
								if (votos >= PartidasActivas.Partidas[Posicion].ConectadosPartida.num - 1) {
									printf(">> 5/10/%s/%s\n", CodigoDeSala, Votado);
									sprintf(respuesta, "5/10/%s/%s|", CodigoDeSala, Votado);
									int socket_expulsado = obtener_socket_invitado(&JugadoresConectados, Votado);
									write(socket_expulsado, respuesta, strlen(respuesta));
									printf(">> 5/4/%s/%s/EL JUGADOR %s HA SIDO EXPULSADO DE LA SALA./%d\n", CodigoDeSala, MensajeTodaSala, Votado, 0);
									sprintf(respuesta, "5/4/%s/%s/EL JUGADOR %s HA SIDO EXPULSADO DE LA SALA./%d|", CodigoDeSala, MensajeTodaSala, Votado, 0);
									pthread_mutex_lock( &mutex );
									NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
									pthread_mutex_unlock( &mutex );
							}
							}
						}
						else {
							printf(">> [-1/No hay suficientes jugadores para votar para expulsar.]\n");
							sprintf(respuesta, "-1/No hay suficientes jugadores para votar para expulsar.|");
							write(sock_conn, respuesta, strlen(respuesta));
							break;
						}
					}
					else {
						printf("5/10/%s/%s\n", CodigoDeSala, Votado);
						sprintf(respuesta, "5/10/%s/%s|", CodigoDeSala, Votado);
						int socket_expulsado = obtener_socket_invitado(&JugadoresConectados, Votado);
						write(socket_expulsado, respuesta, strlen(respuesta));
						printf(">> 5/4/%s/%s/EL JUGADOR %s HA SIDO EXPULSADO DE LA SALA./%d\n", CodigoDeSala, MensajeTodaSala, Votado, 0);
						sprintf(respuesta, "5/4/%s/%s/EL JUGADOR %s HA SIDO EXPULSADO DE LA SALA./%d|", CodigoDeSala, MensajeTodaSala, Votado, 0);
						pthread_mutex_lock( &mutex );
						NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
						pthread_mutex_unlock( &mutex );
					}
				}
				else
				{
					printf(">> [-1/Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.|");
					write(sock_conn, respuesta, strlen(respuesta));
				}
			}
			else if (X == 11)
			{
				p = strtok(NULL, "/");
				strcpy(CodigoDeSala, p);
				
				pthread_mutex_lock(&mutex);
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				pthread_mutex_unlock(&mutex);
				
				if (Posicion != -1)
				{

					time_t t = time(NULL);
					struct tm tm = *localtime(&t);
					char fechaActual[11]; 
					sprintf(fechaActual, "%04d-%02d-%02d",
							tm.tm_year + 1900, tm.tm_mon + 1, tm.tm_mday);
					
					sprintf(query,
						"INSERT INTO Partidas (Codigo, MaximoJugadores, Categoria, Dificultad, Rondas, Privacidad, Fecha) "
						"VALUES ('%s', %d, '%s', '%s', %d, '%s', '%s');",
						PartidasActivas.Partidas[Posicion].CodigoSala,
						PartidasActivas.Partidas[Posicion].MaximoJugadores,
						PartidasActivas.Partidas[Posicion].Categoria,
						PartidasActivas.Partidas[Posicion].Dificultad,
						PartidasActivas.Partidas[Posicion].Rondas,
						PartidasActivas.Partidas[Posicion].Privacidad,
						fechaActual);

					if (mysql_query(conn, query)) {
						printf(">> [Error al insertar en Partidas: %s]\n", mysql_error(conn));
						mysql_close(conn);
						pthread_exit(NULL);
					}

					int PartidaID = (int)mysql_insert_id(conn);
					int PrimeroID = -1;
					int SegundoID = -1;
					int TerceroID = -1;
					int CuartoID = -1;
					int GanadorID = -1;
					int PuntosGanador = 0;

					for (int i = 0; i < PartidasActivas.Partidas[Posicion].ConectadosPartida.num; i++)
					{
						sprintf(query, "SELECT ID FROM Jugadores WHERE Usuario = '%s';", 
							PartidasActivas.Partidas[Posicion].ConectadosPartida.Conectados[i].Usuario);
						if (mysql_query(conn, query)) {
							printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
							mysql_close(conn);
							pthread_exit(NULL);
						}
						res = mysql_store_result(conn);
						row = mysql_fetch_row(res);

						int jugadorID = atoi(row[0]);

						switch (i)
						{
							case 0: PrimeroID = jugadorID; break;
							case 1: SegundoID = jugadorID; break;
							case 2: TerceroID = jugadorID; break;
							case 3: CuartoID = jugadorID; break;
						}

						sprintf(query,
							"UPDATE Jugadores SET Puntos = Puntos + %d WHERE ID = %d;",
							PartidasActivas.Partidas[Posicion].Puntos[i].Puntos, jugadorID);

						if (mysql_query(conn, query)) {
							printf(">> [Error al actualizar puntos: %s]\n", mysql_error(conn));
							mysql_close(conn);
							pthread_exit(NULL);
						}

						if (PartidasActivas.Partidas[Posicion].Puntos[i].Puntos > PuntosGanador)
						{
							GanadorID = jugadorID;
							PuntosGanador = PartidasActivas.Partidas[Posicion].Puntos[i].Puntos;
						}

						if (res != NULL)
							mysql_free_result(res);
					}

					char IDTercero[12], IDCuarto[12];
					if (TerceroID != -1) sprintf(IDTercero, "%d", TerceroID); else strcpy(IDTercero, "NULL");
					if (CuartoID != -1) sprintf(IDCuarto, "%d", CuartoID); else strcpy(IDCuarto, "NULL");

					sprintf(query,
						"INSERT INTO PartidasJugadas (PartidaID, PrimeroID, SegundoID, TerceroID, CuartoID, GanadorID) "
						"VALUES (%d, %d, %d, %s, %s, %d);",
						PartidaID, PrimeroID, SegundoID, IDTercero, IDCuarto, GanadorID);

					if (mysql_query(conn, query)) {
						printf(">> [Error al insertar en PartidasJugadas: %s]\n", mysql_error(conn));
						mysql_close(conn);
						pthread_exit(NULL);
					}
				}
				else
				{
					printf(">> [-1/Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.|");
					write(sock_conn, respuesta, strlen(respuesta));
				}
			}

			else if (X == 12)
			{
				p = strtok(NULL, "/");
				strcpy(CodigoDeSala, p);
				
				pthread_mutex_lock( &mutex );
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				pthread_mutex_unlock( &mutex );
				
				if (Posicion != -1)
				{
					for (int i = 0; i < 50; i++) {
						for (int j = 0; j < 100; j++) {
							PartidasActivas.Partidas[Posicion].PalabrasPorAcertar[i][j] = '\0';
						}
					}
					
					for (int i = 0; i < 10; i++) {
						for (int j = 0; j < 80; j++) {
							PartidasActivas.Partidas[Posicion].Pintores[i][j] = '\0';
						}
					}
					
					for (int i = 0; i < 20; i++) {
						strcpy(PartidasActivas.Partidas[Posicion].VotacionesDeExpulsion[i].Votante, "");
						strcpy(PartidasActivas.Partidas[Posicion].VotacionesDeExpulsion[i].Votado, "");
					}
					
					for (int i = 0; i < 4; i++) {
						PartidasActivas.Partidas[Posicion].Puntos[i] = (PuntosJugador){0};
					}
				}
				else
				{
					printf(">> [-1/Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.|");
					write(sock_conn, respuesta, strlen(respuesta));
				}
			}
			else if (X == 13)
			{
				Partida PartidaEditada;
				p = strtok(NULL, "/");
				PartidaEditada.MaximoJugadores = atoi(p);
				p = strtok(NULL, "/");
				PartidaEditada.Rondas = atoi(p);
				p = strtok(NULL, "/");
				strcpy(PartidaEditada.CodigoSala, p);
				p = strtok(NULL, "/");
				strcpy(PartidaEditada.Categoria, p);
				p = strtok(NULL, "/");
				strcpy(PartidaEditada.Dificultad, p);
				p = strtok(NULL, "/");
				strcpy(PartidaEditada.Privacidad, p);
				
				pthread_mutex_lock( &mutex );
				int Posicion = DamePosicionPartida(&PartidasActivas, PartidaEditada.CodigoSala);
				pthread_mutex_unlock( &mutex );
				
				if (Posicion != -1)
				{
					PartidasActivas.Partidas[Posicion].MaximoJugadores = PartidaEditada.MaximoJugadores;
					PartidasActivas.Partidas[Posicion].Rondas = PartidaEditada.Rondas;
					strcpy(PartidasActivas.Partidas[Posicion].Dificultad, PartidaEditada.Dificultad);
					strcpy(PartidasActivas.Partidas[Posicion].Categoria, PartidaEditada.Categoria);
					strcpy(PartidasActivas.Partidas[Posicion].Privacidad, PartidaEditada.Privacidad);
				}
				else
				{
					printf(">> [-1/Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.|");
					write(sock_conn, respuesta, strlen(respuesta));
				}
			}
		}
    }
	
    mysql_close(conn);
    close(sock_conn);
    pthread_exit(NULL);
}


int main() {
    int sock_conn, sock_listen;
    struct sockaddr_in serv_adr;
	
	JugadoresConectados.num = 0;
	srand(time(NULL));

    // INICIALIZACIONES
    // Abrimos el socket
    if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
        printf(">> [Error creando socket.]\n");
        return -1;
    }

    // Configurar la direcci�n del servidor
    memset(&serv_adr, 0, sizeof(serv_adr));
    serv_adr.sin_family = AF_INET;
    serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// maquina virtual
	//serv_adr.sin_port = htons(9070);
	// shiva
   	serv_adr.sin_port = htons(50086);

    if (bind(sock_listen, (struct sockaddr *)&serv_adr, sizeof(serv_adr)) < 0) {
        printf(">> [Error en el bind.]\n");
        return -1;
    }

    if (listen(sock_listen, 3) < 0) {
        printf(">> [Error en el listen.]\n");
        return -1;
    }

    printf(">> Escuchando\n");
    pthread_t thread;

    for (;;) {
        printf(">> Esperando conexion...\n");
        sock_conn = accept(sock_listen, NULL, NULL);
        printf(">> Conexion recibida\n");

        sockets[i] = sock_conn;

        // Crear un hilo para atender al cliente
        pthread_create(&thread, NULL, AtenderCliente, &sockets[i]);
        i++;
    }
	
    return 0;

}


