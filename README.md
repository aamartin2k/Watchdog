# Watchdog Suite

WatchDog es una aplicación que monitorea el funcionamiento de varias aplicaciones o servicios mediante un mecanismo de transmisión de señal periódica (hearbeat).

Típico proyecto de watchdog con mejoras usando arquitectura cliente/servidor.
Se implementa un servicio de Windows que verifica la ejecución de varias
aplicaciones que deben ejecutarse de forma permanente (como servicios, pero
que por diversos motivos no las hicieron como tal). Al detectar la falta de
respuesta de las mismas, las reinicia y emite alertas. Se adiciona un módulo
cliente que permite a los administradores verificar el estado de las aplicaciones
desde otras estaciones remotas.