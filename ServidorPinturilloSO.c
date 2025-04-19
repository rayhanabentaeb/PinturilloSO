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
	int Socket;
}Conectado;


typedef struct {
	Conectado Conectados [100];
	int num;
}ListaConectados;

ListaConectados JugadoresConectados;
int sockets[100];
int i = 0;

// Estructura necesaria para acceso excluyente
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;


int NuevoConectado(ListaConectados *Lista, char Usuario[50], int Socket){
	// Añade Nuevo Conectado. Retorna 0 si ok y -1 si la lista esstaba llena.
	if (Lista->num == 100)
		return -1;
	else {
		strcpy(Lista->Conectados[Lista->num].Usuario, Usuario);
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
		printf("Numero de conectados: 99/%d\n", Lista->num);
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
		printf("Conectados: %s/%s\n", Conectados, Lista->Conectados[i].Usuario);
		sprintf(Conectados, "%s/%s", Conectados, Lista->Conectados[i].Usuario);
	}
}


void NotificaConectados(ListaConectados *Lista, char notificacion[512]){
	// Envia a todos los usuarios de la lista de conectados el numero total de jugadores conectados.
	printf("Numero de conectados: 99/%d\n", Lista->num);
	for (int j = 0; j < Lista->num; j++)
	{
		sprintf(notificacion, "99/%d", Lista->num);
		write(Lista->Conectados[j].Socket, notificacion, strlen(notificacion));
	}
}


void *AtenderCliente(void *socket) {
    int sock_conn;
    int *s;
    s = (int *)socket;
    sock_conn = *s;

    char peticion[512];
    char respuesta[512];
    int ret;
    int terminar = 0;
	int UsuarioConectado = 0;

    // Conectar a la base de datos
    MYSQL *conn;
    conn = mysql_init(NULL);
    if (conn == NULL) {
        printf("Error al inicializar MySQL\n");
        close(sock_conn);
        pthread_exit(NULL);
    }
    conn = mysql_real_connect(conn, "shiva2.upc.es", "root", "mysql", "T9_DDBBPinturilloSO", 0, NULL, 0);
    if (conn == NULL) {
        printf("Error al conectar a la base de datos: %s\n", mysql_error(conn));
        close(sock_conn);
        pthread_exit(NULL);
    }

    // Bucle para atender al cliente
    while (terminar == 0) {
        ret = read(sock_conn, peticion, sizeof(peticion));
        printf("Recibido\n");

        peticion[ret] = '\0'; // Añadir marca de fin de string
        printf("Peticion: %s\n", peticion);

        // Procesar la petición
        char *p = strtok(peticion, "/");
        int codigo = atoi(p);
		
        char Usuario[80];
        char Correo[80];
        char Contrasena[80];
		

       if (codigo == 0) { // Petición de desconexión
		   terminar = 1;
        } else {
            if (codigo == 1) { // Se recibe el nombre, el correo y la contraseña para crear un nuevo usuario
				// Formato '1/Correo/Usuario/Contraseña'
				p = strtok(NULL, "/");
                strcpy(Correo, p);
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				p = strtok(NULL, "/");
				strcpy(Contrasena, p);
				
				MYSQL_RES *res;
				char query[256];
				
				// Comprobar si existe el correo en la base de datos
				sprintf(query, "SELECT * FROM Jugadores WHERE Correo='%s'", Correo);
				if (mysql_query(conn, query)) {
					printf("1/1/Error en la consulta: %s\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				if (mysql_num_rows(res) > 0) {
					printf("1/2/Error: El correo ya esta registrado.\n");
					sprintf(respuesta, "1/2/Error: El correo ya esta registrado.");
					write(sock_conn, respuesta, strlen(respuesta));
					mysql_free_result(res);
					continue;
				}
				mysql_free_result(res);
				
				// Comprobar si existe el usuario
				sprintf(query, "SELECT * FROM Jugadores WHERE Usuario='%s'", Usuario);
				if (mysql_query(conn, query)) {
					printf("1/1/Error en la consulta: %s\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				res = mysql_store_result(conn);
				if (mysql_num_rows(res) > 0) {
					printf("1/3/Error: El nombre de usuario '%s' ya existe.\n", Usuario);
					sprintf(respuesta, "1/3/Error: El nombre de usuario '%s' ya existe.", Usuario);
					write(sock_conn, respuesta, strlen(respuesta));
					mysql_free_result(res);
					continue;
				}
				mysql_free_result(res);
				
				// Introducir datos a la base de datos
				sprintf(query,
						"INSERT INTO Jugadores (Usuario, Correo, Contrasena) VALUES ('%s', '%s', '%s');",
						Usuario, Correo, Contrasena);
				if (mysql_query(conn, query)) {
					printf("Error al insertar usuario: %s\n", mysql_error(conn));
					mysql_close(conn);
					pthread_exit(NULL);
				}
				
				printf("1/0/Registro completado.\n");
				sprintf(respuesta, "1/0/Registro completado.");
				write(sock_conn, respuesta, strlen(respuesta));
		    }
			else if (codigo == 2) { // Se comprueba que el usuario exista en la BBDD y que la contrasena coincida.
				// Formato '3/Usuario/Contrasena'
				// En caso de estar ya conectado, recibe 0 para desconectar.
				MYSQL_RES *res;
				char query[256];
				
				p = strtok(NULL, "/");
				int X = atoi(p);
				
				if (X == 0)
				{
					pthread_mutex_lock( &mutex );
					printf("PUNTO DE CONTROL ELIMINAR CONECTADOS: %s\n", Usuario);
					int Desconectado = EliminarDesconectados(&JugadoresConectados, Usuario);
					pthread_mutex_unlock( &mutex);
					UsuarioConectado = 0;
					NotificaConectados(&JugadoresConectados, respuesta);
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
						if (yaConectado!= -1)
						{
							printf("2/3/Sesion ya iniciada.\n");
							sprintf(respuesta,"2/3/Sesion ya iniciada.");
							write(sock_conn, respuesta, strlen(respuesta));
							continue;	
						}
						sprintf(query, "SELECT ID FROM Jugadores WHERE Usuario='%s' AND Contrasena='%s'", Usuario, Contrasena);
						if (mysql_query(conn, query)) {
							printf("2/1/Error en la consulta: %s\n", mysql_error(conn));
							mysql_close(conn);
							pthread_exit(NULL);
						}
						res = mysql_store_result(conn);
						if (mysql_num_rows(res) == 0) {
							printf("2/2/El nombre de usuario o la contrasena no son correctos.\n");
							sprintf(respuesta, "2/2/El nombre de usuario o la contrasena no son correctos.");
							write(sock_conn, respuesta, strlen(respuesta));
							mysql_free_result(res);
							continue;
						}
						
						pthread_mutex_lock( &mutex );
						int Conectado = NuevoConectado(&JugadoresConectados, Usuario, sock_conn);
						pthread_mutex_unlock( &mutex );
						NotificaConectados(&JugadoresConectados, respuesta);
						UsuarioConectado = 1;
						
						printf("2/0/Sesion iniciada.\n");
						sprintf(respuesta, "2/0/Sesion iniciada.");
						write(sock_conn, respuesta, strlen(respuesta));
					}
					else
					{
						printf("PUNTO DE CONTROL\n");
						NotificaConectados(&JugadoresConectados, respuesta);
					}
				}
            }
			else if (codigo == 3) { // Se muestran las relaciones de la BBDD (Ranking, Partidas jugadas, Amigos)
				// Formato '3/X/Usuario', siendo X = (1: Ranking, 2: Partidas Jugadas, 3: Amigos)
				p = strtok(NULL, "/");
				int X = atoi(p);
				p = strtok(NULL, "/");
				strcpy(Usuario, p);
				
				MYSQL_RES *res;
				MYSQL_ROW row;
				char query[256];
				
				if (X == 1)
				{
					int ranking = 0;
					sprintf(query,
							"SELECT Ranking.*, (SELECT Jugadores.ID FROM Jugadores WHERE Jugadores.Usuario = '%s') AS JugadorID "
							"FROM Ranking ORDER BY Ranking.Puntuacion DESC;",
							Usuario);
					if (mysql_query(conn, query)) {
						printf("Error en la consulta: %s\n", mysql_error(conn));
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
						printf("Error en la consulta: %s\n", mysql_error(conn));
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
						printf("Error en la consulta: %s\n", mysql_error(conn));
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
        printf("Error creando socket\n");
        return -1;
    }

    // Configurar la dirección del servidor
    memset(&serv_adr, 0, sizeof(serv_adr));
    serv_adr.sin_family = AF_INET;
    serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
    serv_adr.sin_port = htons(50086);

    if (bind(sock_listen, (struct sockaddr *)&serv_adr, sizeof(serv_adr)) < 0) {
        printf("Error en el bind\n");
        return -1;
    }

    if (listen(sock_listen, 3) < 0) {
        printf("Error en el listen\n");
        return -1;
    }

    printf("Escuchando\n");
    pthread_t thread;

    for (;;) {
        printf("Esperando conexión...\n");
        sock_conn = accept(sock_listen, NULL, NULL);
        printf("Conexión recibida\n");

        sockets[i] = sock_conn;

        // Crear un hilo para atender al cliente
        pthread_create(&thread, NULL, AtenderCliente, &sockets[i]);
        i++;
    }
	
    return 0;
}
