app.controller('CtrlBusInscripcion', ['$scope','SendBox', 'ServicioEvento', function ($scope,SendBox, ServicioEvento) {
    $scope.vista = "Asignacion de Bus por Evento"
    $scope.VerParticipantes = false;
    $scope.disabledID = true;
    $scope.ViewAsignacionBus = true;
    $scope.ViewModificarBus = true;

    $scope.usuarios = [
        /*{ usuario: 'JVM10', nombre: 'SISTEMA' },*/
		{ usuario: 'RFLORES', nombre: 'RUT FLORES' },
		{ usuario: 'PROSALES', nombre: 'PAOLA ROSALES' },
		{ usuario: 'SSIAN', nombre: 'SARA SIAN' },
		{ usuario: 'EDEFAJARDO', nombre: 'EMMA DE FAJARDO' },
        { usuario: 'EYOL', nombre: 'ERIK YOL' },
        { usuario: 'OLEMUS', nombre: 'OSMAN LEMUS' }
    ];

       
    $scope.configList = {
        itemsPerPage: 5,
        fillLastPage: false,
        maxPages: 10
    };

    $scope.ListaParticipantes = new Array();
    $scope.ListaBuses = new Array();
    $scope.ListaBusesDisponibles = new Array();
    $scope.InfoParticipante = {};
    $scope.busAsignadoParticipante = {};
    $scope.idBus = 0;

    var limpiarCamposAsignacion = function () {
        $scope.ObjBus = "";
        $scope.busModificar = "";
        $scope.usgestion = "";
    };


    ServicioEvento.ObtenerEventosConBus().then(function (data) {
        $scope.ListaEventos = data;
        $scope.idEvento = data.id_evento;

    });

    $scope.ObtenerParticipantes = function (evento) {

        ServicioEvento.ObtenerParticipantesEvento(evento.id_evento).then(function (data) {
            $scope.ListaParticipantes = data;

            ServicioEvento.ObtenerBusesPorEvento(evento.id_evento).then(function (data) {
                $scope.ListaBuses = data;
            });

            $scope.VerParticipantes = true;
        });
    };

    $scope.irAsignarBus = function (objParticipante) {

        limpiarCamposAsignacion();
        $scope.ViewAsignacionBus = true;
        $scope.ViewModificarBus = false;

        ServicioEvento.BusesDisponiblesAsignar(objParticipante).then(function (data) {
            $scope.ListaBusesDisponibles = data;            
        });

        ServicioEvento.ParticipanteConTransporte(objParticipante).then(function (data) {
            $scope.InfoParticipante = data;
        });

        ServicioEvento.BusAsignado(objParticipante).then(function (data) {
            $scope.busAsignadoParticipante = data;
        });

        ServicioEvento.BusesDisponiblesModificar(objParticipante).then(function (data) {
            $scope.ListaBusesModificar = data;
        });        

        $('#ModalBus').modal('show');
    };

    $scope.AsignarBus = function (id,idevento,id_bus,no_bus,usuario) {

        limpiarCamposAsignacion();
        var objBusInscripcion = new Object();
        var objParticipante = new Object();

        objBusInscripcion.id_participante = id;
        objBusInscripcion.id_evento = idevento;
        objBusInscripcion.id_bus = id_bus;
        objBusInscripcion.no_bus = no_bus;
        objBusInscripcion.usuario = usuario;

        objParticipante.id = id;
        objParticipante.idevento = idevento;


        SendBox.post(objBusInscripcion, 'api/GestionBusInscripcion/AsignarBusParticipante')
        .then(function (data) {
            if (data.code == 0) {
                alert(data.message);
                //SE EJECUTA PETICION PARA ACTUALIZAR REGISTRO DE BUS AL CUAL FUE ASIGNADO
                ServicioEvento.BusAsignado(objParticipante).then(function (data) {
                    $scope.busAsignadoParticipante = data;
                });
                //SE EJECUTA PETICION PARA ACTUALIZAR LISTADO DE BUSES EN EL SELECT
                ServicioEvento.BusesDisponiblesAsignar(objParticipante).then(function (data) {
                    $scope.ListaBusesDisponibles = data;
                });
                //SE EJECUTA PETICION PARA QUE ACTUALICE LISTADO DE BUSES EN LA PANTALLA PRINCIPAL
                ServicioEvento.ObtenerBusesPorEvento(idevento).then(function (data) {
                    $scope.ListaBuses = data;
                });
                //SE EJECUTA PETICION PARA QUE ACTUALICE LISTADO DE BUSES DISPONIBLES PARA MODIFICAR EN LA MISMA SOLICITUD
                ServicioEvento.BusesDisponiblesModificar(objParticipante).then(function (data) {
                    $scope.ListaBusesModificar = data;
                });

            } else {
                alert(data.message);
            }
        });

    };

    $scope.irModificarBus = function (ObjBusInscripcion) {
        
        limpiarCamposAsignacion();
        $scope.ViewAsignacionBus = false;
        $scope.ViewModificarBus = true;

        var objParticipante = new Object();

        objParticipante.id = ObjBusInscripcion.id_participante;
        objParticipante.idevento = ObjBusInscripcion.id_evento;

        ServicioEvento.BusAsignadoDatoUnico(objParticipante).then(function (data) {
            $scope.busAsignadoDatoUnico = data;
        });

    };

    $scope.ModificarBus = function (id_bus,id_evento,id_participante,id_bus_new,no_bus_new,usuario) {

        var ObjBusInscripcion = new Object();
        var objParticipante = new Object();

        ObjBusInscripcion.id_bus = id_bus;
        ObjBusInscripcion.id_evento = id_evento;
        ObjBusInscripcion.id_participante = id_participante;
        ObjBusInscripcion.id_bus_new = id_bus_new;
        ObjBusInscripcion.no_bus_new = no_bus_new;
        ObjBusInscripcion.usuario = usuario;

        objParticipante.id = id_participante;
        objParticipante.idevento = id_evento;



        SendBox.post(ObjBusInscripcion, 'api/GestionBusInscripcion/ModificarBusParticipante')
        .then(function (data) {
            if (data.code == 0) {

                /* SE EJECUTAN LAS SIGUIENTES 4 PETICIONES PARA QUE ACTUALICE DATOS EN PANTALLA */
                ServicioEvento.BusesDisponiblesAsignar(objParticipante).then(function (data) {
                    $scope.ListaBusesDisponibles = data;
                });

                ServicioEvento.ParticipanteConTransporte(objParticipante).then(function (data) {
                    $scope.InfoParticipante = data;
                });

                ServicioEvento.BusAsignado(objParticipante).then(function (data) {
                    $scope.busAsignadoParticipante = data;
                });

                ServicioEvento.ObtenerBusesPorEvento(id_evento).then(function (data) {
                    $scope.ListaBuses = data;
                });

                ServicioEvento.BusesDisponiblesModificar(objParticipante).then(function (data) {
                    $scope.ListaBusesModificar = data;
                });
                /* SE EJECUTA EL METODO "VOLVERASIGNACIONBUS" PARA REGRESAR A LA PANTALLA PRINCIPAL DE ASIGANCION*/
                $scope.volverAsignacionBus();
                alert(data.message);
            } else {
                alert(data.message);
            }
        });

    };

    $scope.volverAsignacionBus = function () {

        limpiarCamposAsignacion();
        $scope.ViewAsignacionBus = true;
        $scope.ViewModificarBus = false;

    };

    $scope.LimpiarFiltro = function () {
        $scope.buscar = {};
    };

}]);