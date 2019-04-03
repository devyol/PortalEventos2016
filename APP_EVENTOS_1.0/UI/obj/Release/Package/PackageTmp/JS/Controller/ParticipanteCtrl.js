app.controller('CtrlParticipante', ['$scope', 'SendBox', '$location','ServicioEvento','$window', function ($scope, SendBox, $location,ServicioEvento,$window) {
    $scope.vista = 'Listado de Participantes';
    $scope.controlador = 'CtrlParticipante';
    $scope.vistaInscripcion = 'Eventos para Inscribir';
    $scope.operacion = "";
    

    $scope.mostrarIdParticipante = true;
    $scope.mostrarEstado = false;
    $scope.disabledID = true;
    $scope.participante = [];
    $scope.nombreParticipante = "";
    $scope.valor = null;

    $scope.response = {};
    $scope.response.id = 0;
    $scope.response.nombre = "";
    $scope.response.apellido = "";
    $scope.response.direccion = "";
    $scope.response.telefono = "";
    $scope.response.correo = "";
    $scope.response.genero_ = {};
    $scope.response.fecha_nacimiento = "";
    $scope.response.alerjico_ = {};
    $scope.response.talla = "";
    $scope.response.observaciones = "";
    $scope.response.estado_ = {};

    $scope.ListaParticipantes = new Array();

    $scope.configList = {
        itemsPerPage: 10,
        fillLastPage: false,
        maxPages: 10
    };
    

    $scope.ViewInscripcion = true;
    $scope.ViewOpcionesInscripcion = true;
    $scope.ViewCobros = true;
    $scope.listaEventosInscritos = true;
    $scope.ViewDetalleCobro = true;
    $scope.listaMovimientosEventos = true;
   

    $scope.generos = [
    { genero: 'M', descripcion: 'MASCULINO' },
    { genero: 'F', descripcion: 'FEMENINO' }
    ];

    $scope.esalergico = [
        { alerjico: 'S', descripcion: 'SI' },
        { alerjico: 'N', descripcion: 'NO' }
    ];

    $scope.estadoregistro = [
        { estado: 'A', descripcion: 'ACTIVO' },
        { estado: 'B', descripcion: 'INACTIVO' }
    ];
    
    $scope.usuarios = [
        /*{ usuario: 'JVM10', nombre: 'SISTEMA' },*/
		{ usuario: 'RFLORES', nombre: 'RUT FLORES' },
		{ usuario: 'PROSALES', nombre: 'PAOLA ROSALES' },
		{ usuario: 'SSIAN', nombre: 'SARA SIAN' },
		{ usuario: 'EDEFAJARDO', nombre: 'EMMA DE FAJARDO' },
        { usuario: 'EYOL', nombre: 'ERIK YOL' },
        { usuario: 'OLEMUS', nombre: 'OSMAN LEMUS' }
    ];

    $scope.tipopago = [        
        { tipo_pago: 1, descripcion: 'EFECTIVO' },
        { tipo_pago: 2, descripcion: 'CHEQUE' }
    ];

    var limpiarCamposCobro = function () {
        $scope.tpago = "";
        $scope.usgestion = "";
        $scope.valor = null;
    };
    

    ServicioEvento.ListarParticipates().then(function (data) {
        $scope.ListaParticipantes = data;
    });

    $scope.nuevo = function () {
        limpiarCamposCobro();
        $scope.response = {};
        $scope.mostrarIdParticipante = false;
        $scope.mostrarEstado = false;
        $scope.operacion = "Nuevo";
        $('#ModalNuevoRegistro').modal('show');
    };

    $scope.volverListadoInscritos = function () {
        $scope.ViewInscripcion = true;
        $scope.ViewOpcionesInscripcion = false;

    };



    $scope.inscribir = function (objParticipante, idParticipante, idEvento, usuario) {
        var objInscripcion = new Object();
                
        objInscripcion.id_evento = idEvento;
        objInscripcion.id_participante = idParticipante;
        objInscripcion.usuario = usuario;
        SendBox.post(objInscripcion, 'api/GestionInscripcion/InscribirParticipante')
        .then(function (data) {
            if (data.code == 0) {
                alert(data.message);
                //CARGA LISTADO DE EVENTOS DONDE YA ESTA INSCRITO EL PARTICIPANTE Y PODERLO LISTAR
                ServicioEvento.EventosInscritosParticipante(objParticipante).then(function (data) {
                    $scope.eventoinscrito = data;
                    //SE LLAMA EL METODO QUE AGREGA LAS OPCIONES DE LA INSCRIPCION
                    $scope.irOpcionesInscripciones(objInscripcion);
                });
            } else {
                alert(data.message);
            }
        });

    };


    $scope.irInscripcion = function (objparticipante) {
        $scope.ViewInscripcion = true;
        $scope.ViewOpcionesInscripcion = false;
        limpiarCamposCobro();

        //CARGA LA INFORMACION DEL PARTICIPANTE PARA TENER DISPONIBLE EL ID PARA PROXIMAS PETICIONES API
        ServicioEvento.ObtenerParticipante(objparticipante).then(function (data) {
            $scope.response = data;
        });
        
        //CARGA LISTADO DE EVENTOS DISPONIBLES PARA INSCRIBIR AL PARTICIPANTES - SE MUESTRAN EN UN COMBO SELECCIONABLE
        ServicioEvento.ObtenerEventoInscripcion(objparticipante).then(function (data) {
            $scope.eventoinscripcion = data;
        });

        //CARGA LISTADO DE EVENTOS DONDE YA ESTA INSCRITO EL PARTICIPANTE Y PODERLO LISTAR
        ServicioEvento.EventosInscritosParticipante(objparticipante).then(function (data) {
            $scope.eventoinscrito = data;
        });

        //SE CARGA EL LISTADO DE PARTICIPANTES ANTES DE CERRAR EL MODAL, ESTO PARA TENER DISPONIBLE LA BUSQUEDA POR FILTRO
        ServicioEvento.ListarParticipates().then(function (data) {
            $scope.ListaParticipantes = data;
        });

        $('#ModalInscribir').modal('show');
    };


    $scope.irOpcionesInscripciones = function (objInscripcion) {
        $scope.ViewInscripcion = false;
        $scope.ViewOpcionesInscripcion = true;

        ServicioEvento.OpcionesDelInscrito(objInscripcion).then(function (data) {
            $scope.ListaOpcionesInscrito = data;
        });

    };

    $scope.ModificarOpcionInscripcion = function (objOpcInsc) {

        var objInscripcion = new Object();
        objInscripcion.id_participante = objOpcInsc.id_participante;
        objInscripcion.id_evento = objOpcInsc.id_evento;

        SendBox.post(objOpcInsc, 'api/GestionInscripcion/ModificarOpcionInscripcion')
        .then(function (data) {
            if (data.code == 0) {
                console.log(data.message);
                ServicioEvento.OpcionesDelInscrito(objInscripcion).then(function (data) {
                    $scope.ListaOpcionesInscrito = data;
                });
            } else {
                alert(data.message);
            }
        });

    };

    $scope.editar = function (objparticipante) {        
        $scope.mostrarIdParticipante = true;
        $scope.disabledID = true;
        $scope.mostrarEstado = true;
        $scope.operacion = "Editar";
        limpiarCamposCobro();
        ServicioEvento.ObtenerParticipante(objparticipante).then(function (data) {
            $scope.response = data;
            if (data.estado_.estado == "A") {
                $scope.response.estado_ = $scope.estadoregistro[0];
            } else {
                $scope.response.estado_ = $scope.estadoregistro[1];
            }
            //if (data.alerjico_.alerjico == "S") {
            //    $scope.response.alerjico_ = $scope.esalergico[0];
            //} else {
            //    $scope.response.alerjico_ = $scope.esalergico[1];
            //}
            if (data.genero_.genero == "M") {
                $scope.response.genero_ = $scope.generos[0];
            } else {
                $scope.response.genero_ = $scope.generos[1];
            }
        });

        $('#ModalNuevoRegistro').modal('show');
    };

    $scope.guardar = function (participante, ge, ea, us, est) {
        participante.operacion = $scope.operacion;
        participante.usuario = us;
        SendBox.post(participante, 'api/GestionParticipante/GuardarParticipante')
        .then(function (data) {
            if (data.code == 0) {
                alert(data.message);
                $scope.actualizar();
                $('#ModalNuevoRegistro').modal('hide');
            } else {
                alert(data.message);
            }
        });
    };

    $scope.irCobro = function (objparticipante) {

        $scope.ViewCobros = true;
        $scope.ViewDetalleCobro = false;
        

        //CARGA LA INFORMACION DEL PARTICIPANTE PARA TENER DISPONIBLE EL ID PARA PROXIMAS PETICIONES API
        ServicioEvento.ObtenerParticipante(objparticipante).then(function (data) {
            $scope.response = data;
        });

        //CARGA LISTADO DE SALDOS DE EVENTOS DONDE SE ENCUENTRA INSCRITO EL PARTICIPANTE
        ServicioEvento.SaldosEventosInscritos(objparticipante).then(function (data) {
            $scope.saldoeventoinscrito = data;
        });
        
        $('#ModalCobrar').modal('show');
        
    };

    $scope.irDetalleCobro = function (objSaldo) {

        $scope.ViewCobros = false;
        $scope.ViewDetalleCobro = true;
        $scope.listaMovimientosEventos = true;
        limpiarCamposCobro();

        //CARGA INFORMACION DE UN REGISTRO DE SALDO DE EVENTO INSCRITO POR PARTICIPANTE
        ServicioEvento.SaldoEventoInscrito(objSaldo).then(function (data) {
            $scope.saldoEventoInscrito = data;
        });

        //CARGA LISTADO DE MOVIMIENTOS DE SALDO DE EVENTO INSCRITO POR PARTICIPANTE
        ServicioEvento.ListadoMovimientosEventoInscrito(objSaldo).then(function (data) {
            $scope.movimientosEvento = data;
        });

    };

    $scope.realizarCobro = function (objMovimiento,valor,tipopago,usuario) {

        var objParticipante = new Object();
        var objSaldo = new Object();
        var respuesta = confirm("¿Esta Seguro de Realizar el Cobro con \n\n Valor de Q " + valor +" \n\n Y tipo de pago " + tipopago.descripcion + "?");

        objParticipante.id = objMovimiento.id_participante;

        objSaldo.id_participante = objMovimiento.id_participante;
        objSaldo.id_evento = objMovimiento.id_evento;

        objMovimiento.tipo_pago = tipopago.tipo_pago;
        objMovimiento.usuario = usuario;
        objMovimiento.valor = valor;

        if (respuesta == true) {
            SendBox.post(objMovimiento, 'api/GestionCobro/AbonarSaldo')
            .then(function (data) {
                if (data.code == 0) {
                    alert(data.message);
                    //CARGA LISTADO DE MOVIMIENTOS DE SALDO DE EVENTO INSCRITO POR PARTICIPANTE
                    ServicioEvento.ListadoMovimientosEventoInscrito(objMovimiento).then(function (data) {
                        $scope.movimientosEvento = data;
                        $scope.listaMovimientosEventos = true;
                    });
                    //ACTUALIZA INFORMACION DEL LISTADO DE SALDOS DE LOS EVENTOS INSCRITOS
                    ServicioEvento.SaldosEventosInscritos(objParticipante).then(function (data) {
                        $scope.saldoeventoinscrito = data;
                    });
                    //CARGA INFORMACION DE UN REGISTRO DE SALDO DE EVENTO INSCRITO POR PARTICIPANTE
                    ServicioEvento.SaldoEventoInscrito(objSaldo).then(function (data) {
                        $scope.saldoEventoInscrito = data;                        
                    });
                    limpiarCamposCobro();
                    

                } else {
                    alert(data.message);
                    limpiarCamposCobro();
                }
            });
        } else {
            limpiarCamposCobro();
        }


    };

    $scope.imprimirRecibo = function (objMOv) {
        $window.open("http://localhost:15872/home/obtenerRecibo?idMov=" + objMOv.id_movimiento );
    };

    $scope.volverListadoEventosInscritos = function () {

        $scope.ViewCobros = true;
        $scope.ViewDetalleCobro = false;

    };

    $scope.actualizar = function () {
        ServicioEvento.ListarParticipates().then(function (data) {
            $scope.ListaParticipantes = data;
        });
    };

    $scope.LimpiarFiltro = function () {
        $scope.buscar = {};
    };


}]);