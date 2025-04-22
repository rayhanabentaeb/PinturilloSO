#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <pthread.h>
#include <mysql.h> 


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
	int MaximoJugadores;
	int Rondas;
	char CodigoSala[5];
	char Dificultad[20];
	char Privacidad[20];
	char Categoria[80];
	char Ultimos20Mensajes[1000];
	ListaConectados ConectadosPartida;
	int PartidaComenzada;
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


int DamePosicion(ListaConectados *Lista, char Usuario[50]){
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


int EliminarDesconectados(ListaConectados *Lista, char Usuario[50]){
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
	// Primero pone el número de conectados.
	// Ejemplo: "3/Juan/Maria/Pedro"
	sprintf(Conectados, "%d", Lista->num);
	for (int i = 0; i < Lista->num; i++)
	{
		printf(">> Conectados: %s/%s\n", Conectados, Lista->Conectados[i].Usuario);
		sprintf(Conectados, "%s/%s", Conectados, Lista->Conectados[i].Usuario);
	}
}


int NuevoConectadoPartida(ListaConectados *Lista, char Usuario[50], int FotoPerfil, int Socket){
	// Añade Nuevo Conectado. Retorna 0 si ok y -1 si la lista esstaba llena.
	if (Lista->num == 4)
		return -1;
	else {
		strcpy(Lista->Conectados[Lista->num].Usuario, Usuario);
		Lista->Conectados[Lista->num].FotoPerfil = FotoPerfil;
		Lista->Conectados[Lista->num].Socket = Socket;
		Lista->num++;
		return 0;
	}
}


int NuevaPartida(ListaPartidas *Lista, Partida SalaPartida){
	// Añade Nueva Partida. Retorna 0 si ok y -1 si la lista estaba llena.
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
	// Devuelve la posicion en la lista o -1 si no está en la lista.
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
		srand(time(NULL));
		PosibleCodigo = rand() % 10000;
		for (int i = 0; i < sizeof(CodigosSala)/sizeof(CodigosSala[0]); i++)
		{
			if (CodigosSala[i] == PosibleCodigo)
			{
				break;
			}
			else if (CodigosSala[i] == 0)
			{
				CodigosSala[i] = PosibleCodigo;
				CodigoValido = 1;
				break;
			}
			// Añadir control de errores por si las salas estan llenas
		}
	}
	return PosibleCodigo;
}

	
void QuitarCodigoSala(int CodigoDeSala) {
	int PosicionCodigo;
	for (int i = 0; i < sizeof(CodigosSala)/sizeof(CodigosSala[0]); i++)
	{
		if (CodigosSala[i] == CodigoDeSala)
		{
			PosicionCodigo = i;
		}
		else if (CodigosSala[i] == 0)
		{
			break;
		}
		
		if (i >= PosicionCodigo)
		{
			CodigosSala[i] = CodigosSala[i+1];
		}
	}
}


int PartidaCerrada(ListaPartidas *Lista, char CodigoSala[5]){
	// Retorna 0 si elimina y -1 si la partida no está en la lista.
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





void NotificaConectados(ListaConectados *Lista, char notificacion[512]){
	// Envia a todos los usuarios de la lista de conectados el numero total de jugadores conectados.
	for (int j = 0; j < Lista->num; j++)
	{
		write(Lista->Conectados[j].Socket, notificacion, strlen(notificacion));
	}
}


void *AtenderCliente(void *socket) {
    int sock_conn;
    int *s;
    s = (int *)socket;
    sock_conn = *s;

    char peticion[2048];
    char respuesta[2048];
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
    conn = mysql_real_connect(conn, "localhost", "root", "mysql", "DDBBPinturilloSO", 0, NULL, 0);
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
					sprintf(respuesta, "-1/El correo '%s' ya esta registrado.", Correo);
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
					sprintf(respuesta, "-1/El nombre de usuario '%s' ya existe.", Usuario);
					write(sock_conn, respuesta, strlen(respuesta));
					mysql_free_result(res);
					continue;
				}
				mysql_free_result(res);
				
				// Introducir datos a la base de datos
				sprintf(query,
						"INSERT INTO Jugadores (Usuario, Correo, Contrasena, FotoPerfil) VALUES ('%s', '%s', '%s', '%d');",
						Usuario, Correo, Contrasena, FotoPerfil);
				if (mysql_query(conn, query)) {
					printf(">> [Error al insertar usuario: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				
				printf(">> 1/0/Registro completado.\n");
				sprintf(respuesta, "1/0/Registro completado.");
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
					sprintf(respuesta, "-1/El correo no esta registrado.");
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
				
				sprintf(respuesta, "1/1/%s/%s", row[0], row[1]);
				write(sock_conn, respuesta, strlen(respuesta));
			}
			else if (X == 2)
			{					
				char NuevoUsuario[80];
				p = strtok(NULL, "/");
				strcpy(NuevoUsuario, p);
				p = strtok(NULL, "/");
				FotoPerfil = atoi(p);
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				
				sprintf(query,
						"UPDATE Jugadores SET Usuario = '%s', FotoPerfil = %d WHERE Usuario = '%s';",
						NuevoUsuario, FotoPerfil, Usuario);
				if (mysql_query(conn, query)) {
					printf(">> [Error al insertar usuario: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				
				sprintf(respuesta, "1/2/%s/%d", NuevoUsuario, FotoPerfil);
				write(sock_conn, respuesta, strlen(respuesta));
			}
		}
		else if (codigo == 2) 
	   {    // Se comprueba que el usuario exista en la BBDD y que la contrasena coincida.
			// Formato '3/Usuario/Contrasena'
			// En caso de estar ya conectado, recibe 0 para desconectar.
			MYSQL_RES *res;
			char query[2048];
			
			p = strtok(NULL, "/");
			int X = atoi(p);
			
			if (X == 0)
			{
				pthread_mutex_lock( &mutex );
				int Desconectado = EliminarDesconectados(&JugadoresConectados, Usuario);
				pthread_mutex_unlock( &mutex);
				UsuarioConectado = 0;
				sprintf(respuesta, "99/%d", JugadoresConectados.num);
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
						sprintf(respuesta,"-1/Sesion ya iniciada.");
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
						sprintf(respuesta, "-1/El nombre de usuario o la contrasena no son correctos.");
						write(sock_conn, respuesta, strlen(respuesta));
						mysql_free_result(res);
						continue;
					}
					
					pthread_mutex_lock( &mutex );
					int Conectado = NuevoConectado(&JugadoresConectados, Usuario, FotoPerfil, sock_conn);
					pthread_mutex_unlock( &mutex );
					sprintf(respuesta, "99/%d", JugadoresConectados.num);
					pthread_mutex_lock( &mutex );
					NotificaConectados(&JugadoresConectados, respuesta);
					pthread_mutex_unlock( &mutex );
					UsuarioConectado = 1;
					
					sprintf(query,
							"SELECT FotoPerfil FROM Jugadores WHERE Usuario='%s';",
							Usuario);
					if (mysql_query(conn, query)) {
						printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
						mysql_close(conn);
						pthread_exit(NULL);
					}
					
					res = mysql_store_result(conn);
					MYSQL_ROW row = mysql_fetch_row(res);
					FotoPerfil = atoi(row[0]);
					
					printf(">> 2/0/Sesion iniciada.\n");
					sprintf(respuesta, "2/0/%d", FotoPerfil);
					write(sock_conn, respuesta, strlen(respuesta));
				}
				else
				{
					sprintf(respuesta, "99/%d", JugadoresConectados.num);
					pthread_mutex_lock( &mutex );
					NotificaConectados(&JugadoresConectados, respuesta);
					pthread_mutex_unlock( &mutex );
				}
			}
		}
		else if (codigo == 3)
		{   // Se muestran las relaciones de la BBDD (Ranking, Partidas jugadas, Amigos)
			// Formato '3/X/Usuario', siendo X = (1: Ranking, 2: Partidas Jugadas, 3: Amigos)
			p = strtok(NULL, "/");
			int X = atoi(p);
			p = strtok(NULL, "/");
			strcpy(Usuario, p);
			
			MYSQL_RES *res;
			MYSQL_ROW row;
			char query[2048];
			
			if (X == 1)
			{
				int ranking = 0;
				sprintf(query,
						"SELECT Ranking.*, (SELECT Jugadores.ID FROM Jugadores WHERE Jugadores.Usuario = '%s') AS JugadorID "
						"FROM Ranking ORDER BY Ranking.Puntuacion DESC;",
						Usuario);
				if (mysql_query(conn, query)) {
					printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				
				while ((row = mysql_fetch_row(res)) != NULL)
				{
					ranking++;
					int jugadorID = atoi(row[mysql_num_fields(res) - 1]);
					int rankingJugadorID = atoi(row[0]);
					
					if (rankingJugadorID == jugadorID)
					{
						printf("3/1/%d\n", ranking);
						sprintf(respuesta, "3/1/%d", ranking);
						break;
					}
				}
				write(sock_conn, respuesta, strlen(respuesta));
			}
			else if (X == 2)
			{
				sprintf(query,
						"SELECT COUNT(*) AS NumeroDePartidas "
						"FROM Partidas "
						"WHERE PrimeroID = (SELECT ID FROM Jugadores WHERE Usuario='%s') "
						"OR SegundoID = (SELECT ID FROM Jugadores WHERE Usuario='%s') "
						"OR TerceroID = (SELECT ID FROM Jugadores WHERE Usuario='%s') "
						"OR CuartoID = (SELECT ID FROM Jugadores WHERE Usuario='%s');", 
						Usuario, Usuario, Usuario, Usuario);
				if (mysql_query(conn, query)) {
					printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				
				res = mysql_store_result(conn);
				row = mysql_fetch_row(res);
				printf("3/2/%s\n", row[0]);
				sprintf(respuesta, "3/2/%s", row[0]);
				write(sock_conn, respuesta, strlen(respuesta));
			}
			else if (X == 3)
			{
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
						Usuario, Usuario, Usuario, Usuario);
				if (mysql_query(conn, query)) {
					printf(">> [Error en la consulta: %s]\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				row = mysql_fetch_row(res);
				sprintf(respuesta, "3/3/%s", row[0]);
				while (row != NULL)
				{
					row = mysql_fetch_row(res);
					if (row != NULL)
						sprintf(respuesta, "%s/%s", respuesta, row[0]);
				}
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
				
				pthread_mutex_lock( &mutex );
				QuitarCodigoSala(CodigoDeSala);
				pthread_mutex_unlock( &mutex );
			}
			else if (X == 1)
			{
				pthread_mutex_lock( &mutex );
				int PosibleCodigo = NuevoCodigoSala();
				pthread_mutex_unlock( &mutex );
				sprintf(CodigoDeSala, "%04d", PosibleCodigo);
				printf(">> 4/%s\n", CodigoDeSala);
				sprintf(respuesta, "4/%s", CodigoDeSala);
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
				
				pthread_mutex_lock( &mutex );
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				pthread_mutex_unlock( &mutex );
				
				if (Posicion != -1)
				{
					pthread_mutex_lock( &mutex );
					int EliminaJugadorPartida = EliminarDesconectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, Usuario);
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
						sprintf(respuesta, "5/3/%s", SalaPartida);
						
						pthread_mutex_lock( &mutex );
						NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
						pthread_mutex_unlock( &mutex );

						printf(">> 5/4/%s/El jugador '%s' ha abandonado la sala.\n", MensajeTodaSala, Usuario);
						
						sprintf(respuesta, "5/4/%s/El jugador '%s' ha abandonado la sala.", MensajeTodaSala, Usuario);
						sprintf(PartidasActivas.Partidas[Posicion].Ultimos20Mensajes, 
								"%s/El jugador '%s' ha abandonado la sala.", 
								PartidasActivas.Partidas[Posicion].Ultimos20Mensajes, Usuario);
						
						pthread_mutex_lock( &mutex );
						NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
						pthread_mutex_unlock( &mutex );
					}
				}
				else
				{
					printf(">> [-1/Error: Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.");
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
				NuevoConectadoPartida(&PartidaNueva.ConectadosPartida, Usuario, FotoPerfil, sock_conn);
				NuevaPartida(&PartidasActivas, PartidaNueva);
				pthread_mutex_unlock( &mutex );
				printf(">> 5/4/%s/El jugador '%s' ha creado la sala.\n", MensajeTodaSala, Usuario);
				sprintf(respuesta, "5/4/%s/El jugador '%s' ha creado la sala.", MensajeTodaSala, Usuario);
				pthread_mutex_lock( &mutex );
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
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
					sprintf(respuesta, "5/2/%s", SalaPartida);
					write(sock_conn, respuesta, strlen(respuesta));
					printf(">> 5/4/%s/El jugador '%s' se ha unido a la sala.\n", MensajeTodaSala, Usuario);
					sprintf(respuesta, "5/4/%s/El jugador '%s' se ha unido a la sala.", MensajeTodaSala, Usuario);
					pthread_mutex_lock( &mutex );
					NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
					pthread_mutex_unlock( &mutex );
				}
				else
				{
					printf(">> [-1/Error: Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.");
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
				int NuevoJugadorPartida = NuevoConectadoPartida(&PartidasActivas.Partidas[Posicion].ConectadosPartida, 
														 Usuario, FotoPerfil, sock_conn);
				pthread_mutex_unlock( &mutex );
				
				if (Posicion != -1)
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
					
					printf(">> 5/3/%s\n", SalaPartida);
					sprintf(respuesta, "5/3/%s", SalaPartida);
					pthread_mutex_lock( &mutex );
					NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
					pthread_mutex_unlock( &mutex );
				}
				else
				{
					printf(">> [-1/Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.");
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
				
				pthread_mutex_lock( &mutex );
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				pthread_mutex_unlock( &mutex );
				
				if (Posicion != -1)
				{				
					printf(">> 5/4/%s/%s\n", Usuario, MensajeChat);
					sprintf(respuesta, "5/4/%s/%s", Usuario, MensajeChat);
					pthread_mutex_lock( &mutex );
					NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
					pthread_mutex_unlock( &mutex );
				}
				else
				{
					printf(">> [-1/Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.");
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
				
				pthread_mutex_lock( &mutex );
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				pthread_mutex_unlock( &mutex );
				
				if (Posicion != -1)
				{				
					//printf(">> 5/5/%d/%d/%d\n", CodigoDibujo, DibujoX, DibujoY);
					sprintf(respuesta, "5/5/%d/%d/%d", CodigoDibujo, DibujoX, DibujoY);
					pthread_mutex_lock( &mutex );
					NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
					pthread_mutex_unlock( &mutex );
				}
				else
				{
					printf(">> [-1/Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.");
					write(sock_conn, respuesta, strlen(respuesta));
				}
			}
			else if (X == 6)
			{
				p = strtok(NULL, "/");
				strcpy(CodigoDeSala, p);
				
				pthread_mutex_lock( &mutex );
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				pthread_mutex_unlock( &mutex );
				
				if (Posicion != -1)
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
					char Palabra[500];
					strcpy(Palabra, row[0]);
					
					printf(">> 5/6/%s\n", Palabra);
					sprintf(respuesta, "5/6/%s", Palabra);
					write(sock_conn, respuesta, strlen(respuesta));
				}
				else
				{
					printf(">> [-1/Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.");
					write(sock_conn, respuesta, strlen(respuesta));
				}
			}
			else if (X == 7)
			{
				p = strtok(NULL, "/");
				strcpy(CodigoDeSala, p);
				p = strtok(NULL, "/");
				char Palabra[200];
				strcpy(Palabra, p);
				p = strtok(NULL, "/");
				char Pintor[200];
				strcpy(Pintor, p);
				p = strtok(NULL, "/");
				int NumeroIndices = atoi(p);
				
				pthread_mutex_lock( &mutex );
				int Posicion = DamePosicionPartida(&PartidasActivas, CodigoDeSala);
				pthread_mutex_unlock( &mutex );
				
				if (Posicion != -1)
				{				
					printf(">> 5/7/%s/%s/%d\n", Palabra, Pintor, NumeroIndices);
					sprintf(respuesta, "5/7/%s/%s/%d", Palabra, Pintor, NumeroIndices);
					for (int i = 0; i < NumeroIndices; i++)
					{
						p = strtok(NULL, "/");
						sprintf(respuesta, "%s/%d", respuesta, atoi(p));
					}
					pthread_mutex_lock( &mutex );
					NotificaConectados(&PartidasActivas.Partidas[Posicion].ConectadosPartida, respuesta);
					pthread_mutex_unlock( &mutex );
				}
				else
				{
					printf(">> [-1/Partida no encontrada.]\n");
					sprintf(respuesta, "-1/Partida no encontrada.");
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

    // INICIALIZACIONES
    // Abrimos el socket
    if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
        printf(">> [Error creando socket.]\n");
        return -1;
    }

    // Configurar la dirección del servidor
    memset(&serv_adr, 0, sizeof(serv_adr));
    serv_adr.sin_family = AF_INET;
    serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
    serv_adr.sin_port = htons(9070);

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
        printf(">> Esperando conexión...\n");
        sock_conn = accept(sock_listen, NULL, NULL);
        printf(">> Conexión recibida\n");

        sockets[i] = sock_conn;

        // Crear un hilo para atender al cliente
        pthread_create(&thread, NULL, AtenderCliente, &sockets[i]);
        i++;
    }
	
    return 0;
}
