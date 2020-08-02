# Servidor Udp 
Prototipo de receptor de mensajes Heartbeat enviados por los clientes.
#### Argumentos de la línea de comando.
        Uso:  svudp  [Ip]  [port]
           
           Argumento           Función
           1  Ip            Direccion Ip del servidor
           2  port          Puerto local, servidor listens
         
            Uso:  svudp  [*] | [D] | [d] | [default] 
           
            Emplea la configuración por defecto, hardcoded.

### Prototipo
El método IniciaServidorInterno() implementa un receptor Udp en código. Se escribió como prototipo.

### Ejemplo de uso
El método  IniciaServidorShared() implementa el receptor definido en la biblioteca Monitor.Shared. Se presenta como ejemplo de código.
    
    