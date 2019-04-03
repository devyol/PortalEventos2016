app.service('ServicioEvento', function ($http, $q, SendBox) {

    var CatalogosServices = new Object();

    //-----------------------------------------------------------------------
    //  LISTA DE EVENTOS
    //----------------------------------------------------------------------

    this.ObtenerEventos = function () {
        var deferred = $q.defer();

        try {
            SendBox.post("", 'api/GestionEvento/ListarEventos')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.eventos = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.eventos);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.eventos);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // INFORMACION DE UN EVENTO
    //----------------------------------------------------------------------

    this.ObtenerEvento = function (objevento) {
        var deferred = $q.defer();

        try {
            SendBox.post(objevento, 'api/GestionEvento/ListarEvento')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.evento = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.evento);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.evento);
        };
        return deferred.promise;
    };
    

    //-----------------------------------------------------------------------
    //  LISTA EVENTOS ACTIVOS
    //----------------------------------------------------------------------

    this.ListaEventoActivo = function () {
        var deferred = $q.defer();

        try {
            SendBox.post("", 'api/OpcionEvento/ListaEventoActivo')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.eventosactivos = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.eventosactivos);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.eventosactivos);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    //  LISTA DE OPCIONES DE EVENTO
    //----------------------------------------------------------------------

    this.ObtenerOpcionesEventos = function (objIDevento) {
        var deferred = $q.defer();
                

        try {
            SendBox.post(objIDevento, 'api/OpcionEvento/ListaOpcionesEvento')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.opcionesEventos = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.opcionesEventos);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.opcionesEventos);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    //  INFORMACION DE LA OPCION DE UN EVENTO
    //----------------------------------------------------------------------

    this.ObtenerOpcionEvento = function (objIdOpcion) {
        var deferred = $q.defer();


        try {
            SendBox.post(objIdOpcion, 'api/OpcionEvento/InformacionOpcionEvento')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.opcionEvento = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.opcionEvento);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.opcionEvento);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    //  LISTAR PARTICIPANTES
    //----------------------------------------------------------------------

    this.ListarParticipates = function () {
        var deferred = $q.defer();

        try {
            SendBox.post("", 'api/GestionParticipante/ListaParticipantes')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.participantes = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.participantes);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.participantes);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // INFORMACION DE UN PARTICIPANTE
    //----------------------------------------------------------------------

    this.ObtenerParticipante = function (objParticipante) {
        var deferred = $q.defer();

        try {
            SendBox.post(objParticipante, 'api/GestionParticipante/ListarParticipante')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.participante = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.participante);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.participante);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // EVENTOS DISPONIBLES PARA QUE PUEDA INSCRIBIRSE EL PARTICIPANTE
    //----------------------------------------------------------------------

    this.ObtenerEventoInscripcion = function (objParticipante) {
        var deferred = $q.defer();
        
        try {
            SendBox.post(objParticipante, 'api/GestionInscripcion/EventosParaInscripcion')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.eventoinscripcion = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.eventoinscripcion);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.eventoinscripcion);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // LISTA EVENTOS EN LOS QUE YA ESTA INSCRITO EL PARTICIPANTE
    //----------------------------------------------------------------------

    this.EventosInscritosParticipante = function (objParticipante) {
        var deferred = $q.defer();

        try {
            SendBox.post(objParticipante, 'api/GestionInscripcion/EventosInscritos')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.eventoinscrito = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.eventoinscrito);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.eventoinscrito);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // LISTA OPCIONES DEL PARTICIPANTE INSCRITO
    //----------------------------------------------------------------------

    this.OpcionesDelInscrito = function (objInscripcion) {
        var deferred = $q.defer();

        try {
            SendBox.post(objInscripcion, 'api/GestionInscripcion/OpcionesDelInscrito')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.opcionesInscrito = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.opcionesInscrito);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.opcionesInscrito);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // LISTA DE SALDOS DE EVENTOS DONDE SE ENCUENTRA INSCRITO EL PARTICIPANTE
    //----------------------------------------------------------------------

    this.SaldosEventosInscritos = function (objParticipante) {
        var deferred = $q.defer();

        try {
            SendBox.post(objParticipante, 'api/GestionCobro/SaldosEventosInscritos')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.saldosEventos = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.saldosEventos);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.saldosEventos);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // LISTA SALDO DE EVENTO DONDE SE ENCUENTRA INSCRITO EL PARTICIPANTE
    //----------------------------------------------------------------------

    this.SaldoEventoInscrito = function (objSaldo) {
        var deferred = $q.defer();

        try {
            SendBox.post(objSaldo, 'api/GestionCobro/SaldoEventoInscrito')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.saldoEvento = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.saldoEvento);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.saldoEvento);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    // LISTA DE MOVIMIENTOS DE UN EVENTO DONDE SE ENCUENTRA INSCRITO EL PARTICIPANTE
    //----------------------------------------------------------------------

    this.ListadoMovimientosEventoInscrito = function (objSaldo) {
        var deferred = $q.defer();

        try {
            SendBox.post(objSaldo, 'api/GestionCobro/ListadoMovimientosEventoInscrito')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.movimientosEvento = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.movimientosEvento);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.movimientosEvento);
        };
        return deferred.promise;
    };


    //-----------------------------------------------------------------------
    //  LISTA DE EVENTOS CON BUS
    //----------------------------------------------------------------------

    this.ObtenerEventosConBus = function () {
        var deferred = $q.defer();


        try {
            SendBox.post("", 'api/GestionBusInscripcion/EventosConBus')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.eventosconbus = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.eventosconbus);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.eventosconbus);
        };
        return deferred.promise;
    };
    
    //-----------------------------------------------------------------------
    //  LISTA DE PARTICIPANTES POR EVENTO
    //----------------------------------------------------------------------

    this.ObtenerParticipantesEvento = function (idevento) {
        var deferred = $q.defer();


        try {
            SendBox.post(idevento, 'api/GestionBusInscripcion/ListarParticipantes')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.participantes = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.participantes);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.participantes);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    //  LISTA EL PARTICIPANTES CON TRASPORTE
    //----------------------------------------------------------------------

    this.ParticipanteConTransporte = function (ObjParticipante) {
        var deferred = $q.defer();


        try {
            SendBox.post(ObjParticipante, 'api/GestionBusInscripcion/ParticipanteConTransporte')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.participanteconTransporte = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.participanteconTransporte);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.participanteconTransporte);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    //  LISTA DE BUSES POR EVENTO
    //----------------------------------------------------------------------

    this.ObtenerBusesPorEvento = function (idevento) {
        var deferred = $q.defer();


        try {
            SendBox.post(idevento, 'api/GestionBusInscripcion/BusesEvento')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.buses = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.buses);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.buses);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    //  LISTA DE BUSES DISPONIBLES PARA ASIGNAR AL PARTICIPANTE
    //----------------------------------------------------------------------

    this.BusesDisponiblesAsignar = function (objParticipante) {
        var deferred = $q.defer();


        try {
            SendBox.post(objParticipante, 'api/GestionBusInscripcion/BusesDisponibles')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.busesDisponibles = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.busesDisponibles);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.busesDisponibles);
        };
        return deferred.promise;
    };


    //-----------------------------------------------------------------------
    //  BUS ASIGNADO AL PARTICIPANTE
    //----------------------------------------------------------------------

    this.BusAsignado = function (objParticipante) {
        var deferred = $q.defer();


        try {
            SendBox.post(objParticipante, 'api/GestionBusInscripcion/BusAsignado')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.busasignado = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.busasignado);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.busasignado);
        };
        return deferred.promise;
    };


    //-----------------------------------------------------------------------
    //  BUS ASIGNADO AL PARTICIPANTE (DATO UNICO)
    //----------------------------------------------------------------------

    this.BusAsignadoDatoUnico = function (objParticipante) {
        var deferred = $q.defer();


        try {
            SendBox.post(objParticipante, 'api/GestionBusInscripcion/BusAsignadoDataUnico')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.busasignadoUnico = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.busasignadoUnico);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.busasignadoUnico);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    //  LISTA DE BUSES DISPONIBLES PARA MODIFICAR BUS AL PARTICIPANTE
    //----------------------------------------------------------------------

    this.BusesDisponiblesModificar = function (objParticipante) {
        var deferred = $q.defer();


        try {
            SendBox.post(objParticipante, 'api/GestionBusInscripcion/BusesDisponiblesModificar')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.busesModificar = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.busesModificar);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.busesModificar);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    //  LISTA DE EVENTOS ACTIVOS PARA GENERAR NOTA DE CREDITO
    //----------------------------------------------------------------------

    this.ListarEventosNota = function () {
        var deferred = $q.defer();


        try {
            SendBox.post("", 'api/GestionNotaDeCredito/ListarEventosNota')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.EventosActivosNota = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.EventosActivosNota);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.EventosActivosNota);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    //  LISTA DE PARTICIPANTES POR EVENTO PARA GENERARLES NOTA DE CREDITO
    //----------------------------------------------------------------------

    this.ListarParticipantesNota = function (idevento) {
        var deferred = $q.defer();


        try {
            SendBox.post(idevento, 'api/GestionNotaDeCredito/ListarParticipantesNota')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.ParticipantesNota = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.ParticipantesNota);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.ParticipantesNota);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    //  OBTIENE SALDO DEL EVENTO DEL PARTICIPANTE
    //----------------------------------------------------------------------

    this.SaldoEventoNota = function (objParticipante) {
        var deferred = $q.defer();


        try {
            SendBox.post(objParticipante, 'api/GestionNotaDeCredito/SaldoEventoNota')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.SaldoNota = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.SaldoNota);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.SaldoNota);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    //  LISTADO DE CARGOS POR NOTA DE CREDITO PARA UN PARTICIPANTE
    //----------------------------------------------------------------------

    this.ListarCargosNota = function (objParticipante) {
        var deferred = $q.defer();


        try {
            SendBox.post(objParticipante, 'api/GestionNotaDeCredito/ListarCargosNota')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.CargosNota = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.CargosNota);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.CargosNota);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    //  LISTADO DE PARTICIPANTES INSCRITOS POR EVENTO
    //----------------------------------------------------------------------

    this.inscritosPorEvento = function (idEvento) {
        var deferred = $q.defer();


        try {
            SendBox.post(idEvento, 'api/ReportesEvento/ListarInscritosEvento')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.inscritosEvento = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.inscritosEvento);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.inscritosEvento);
        };
        return deferred.promise;
    };

    //-----------------------------------------------------------------------
    //  LISTADO DE PARTICIPANTES INSCRITOS POR EVENTO
    //----------------------------------------------------------------------

    this.listaSaldosDiarios = function (idEvento) {
        var deferred = $q.defer();


        try {
            SendBox.post(idEvento, 'api/ReportesEvento/ListarSaldosDiarios')
            .then(function (data) {
                if (data.code == 0) {
                    CatalogosServices.saldosEvento = data.data;
                } else {
                    alert(data.message);
                }
                deferred.resolve(CatalogosServices.saldosEvento);
            }),
            function (data) { };

        } catch (e) {
            deferred.reject(CatalogosServices.saldosEvento);
        };
        return deferred.promise;
    };

});