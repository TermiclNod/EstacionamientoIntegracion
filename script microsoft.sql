create database appEstacionamiento;
go
use appEstacionamiento;
go

CREATE TABLE denuncia (
    id_denuncia            int identity NOT NULL,
    descripcion_denuncia   varchar(200) NOT NULL,
    id_usuario             int NOT NULL
);

ALTER TABLE denuncia ADD CONSTRAINT denuncia_pk PRIMARY KEY ( id_denuncia );

CREATE TABLE estacionamiento (
    id_estacionamiento             int identity NOT NULL,
    comuna_estacionamiento         varchar(100) NOT NULL,
    direccion_estacionamiento      varchar(200) NOT NULL,
    comentario_estacionamiento     varchar(200) NOT NULL,
    id_usuario             int NOT NULL,
	arrendatario int,
    valor_estacionamiento          int NOT NULL,
    tipovehiculo_estacionamiento   varchar(200) NOT NULL,
    imagen_estacionamiento         varchar(500),
    estado_estacionamiento         CHAR(1) NOT NULL
);

ALTER TABLE estacionamiento ADD CONSTRAINT estacionamiento_pk PRIMARY KEY ( id_estacionamiento );

CREATE TABLE resena (
    id_resena            int identity NOT NULL,
    puntaje_resena       int NOT NULL,
    comentario_resena    varchar(200) NOT NULL,
    id_usuario   int NOT NULL
);

ALTER TABLE resena
    ADD CHECK ( puntaje_resena IN (
        1,
        2,
        3,
        4,
        5,
        6,
        7
    ) );

ALTER TABLE resena ADD CONSTRAINT resena_pk PRIMARY KEY ( id_resena );

create TABLE tipo (
    id_tipo       int NOT NULL,
    tipo          varchar(20) NOT NULL
);

ALTER TABLE tipo ADD CONSTRAINT tipo_pk PRIMARY KEY ( id_tipo );

CREATE TABLE usuario (
    id_usuario           int identity NOT NULL,
    nombre_usuario       varchar(100) NOT NULL,
    apellido_usuario     varchar(100) NOT NULL,
    rut_usuario          varchar(10) NOT NULL,
    contrasena_usuario   varchar(50) NOT NULL,
    estado_usuario       int NOT NULL,
    correo_usuario       varchar(50) NOT NULL,
    imagen_usuario       varchar(255)
);

ALTER TABLE usuario ADD CONSTRAINT usuario_pk PRIMARY KEY ( id_usuario );

CREATE TABLE usuario_tipo (
    id_tipo         int NOT NULL,
    id_usuario   int NOT NULL
);

ALTER TABLE usuario_tipo ADD CONSTRAINT usuario_tipo_pk PRIMARY KEY ( id_tipo, id_usuario );

ALTER TABLE denuncia
    ADD CONSTRAINT denuncia_usuario_fk FOREIGN KEY ( id_usuario )
        REFERENCES usuario ( id_usuario );

ALTER TABLE estacionamiento
    ADD CONSTRAINT estacionamiento_usuario_fk FOREIGN KEY ( id_usuario )
        REFERENCES usuario ( id_usuario );

ALTER TABLE resena
    ADD CONSTRAINT resena_usuario_fk FOREIGN KEY ( id_usuario )
        REFERENCES usuario ( id_usuario );

ALTER TABLE usuario_tipo
    ADD CONSTRAINT usuario_tipo_tipo_fk FOREIGN KEY ( id_tipo )
        REFERENCES tipo ( id_tipo );

ALTER TABLE usuario_tipo
    ADD CONSTRAINT usuario_tipo_usuario_fk FOREIGN KEY ( id_usuario )
        REFERENCES usuario ( id_usuario );

-- inserts
--usuarios prueba
insert into usuario (nombre_usuario,apellido_usuario,rut_usuario,contrasena_usuario,estado_usuario,correo_usuario) values('José Miguel','Reyes','19733215-8','admin',1,'termiclnod@gmail.com');
insert into usuario (nombre_usuario,apellido_usuario,rut_usuario,contrasena_usuario,estado_usuario,correo_usuario) values('Sebastián','Palma','1979999-9','admin',1,'elsoly@gmail.com');
--tipo
insert into tipo values(1,'Administrador');
insert into tipo values(2,'Dueño');
insert into tipo values(3,'Arrendador');
--tipo_usuario
insert into usuario_tipo values(3,1);
insert into usuario_tipo values(2,1);
insert into usuario_tipo values(2,2);

select * from usuario_tipo order by id_usuario;

delete from estacionamiento
insert into estacionamiento (comuna_estacionamiento,direccion_estacionamiento,comentario_estacionamiento,id_usuario,valor_estacionamiento,tipovehiculo_estacionamiento,estado_estacionamiento) values ('Puente Alto','porahí 123','Recinto piola',2,950,'Sedan',1);

select u.id_usuario,u.nombre_usuario,u.apellido_usuario,u.rut_usuario,u.contrasena_usuario,u.estado_usuario,u.correo_usuario
from usuario u 


select u.id_usuario,u.nombre_usuario,u.apellido_usuario,u.rut_usuario,u.contrasena_usuario,u.estado_usuario,u.correo_usuario,ut.id_tipo
from usuario u inner join usuario_tipo ut on u.id_usuario=ut.id_usuario order by u.id_usuario

delete from usuario_tipo where id_usuario = 2 and id_tipo=2


select id_estacionamiento,comuna_estacionamiento,direccion_estacionamiento,comentario_estacionamiento,id_usuario, arrendatario,valor_estacionamiento,tipovehiculo_estacionamiento,imagen_estacionamiento,estado_estacionamiento from estacionamiento where id_usuario=2

select u.id_usuario from usuario u inner join usuario_tipo ut on u.id_usuario=ut.id_usuario where u.id_usuario=15 and ut.id_tipo=3
select u.id_usuario from usuario u inner join usuario_tipo ut on u.id_usuario=ut.id_usuario where u.correo_usuario='elsoly@gmail.com' and ut.id_tipo=2