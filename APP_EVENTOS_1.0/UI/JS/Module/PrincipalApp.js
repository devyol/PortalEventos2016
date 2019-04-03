var app = angular.module('myApp', ['ngRoute', 'ui.bootstrap', 'ngAnimate', 'ngSanitize', 'angular-table'])
.config(function ($routeProvider, $locationProvider) {
    $routeProvider.when('/ListaParticipantes', {
        templateUrl: 'JS/Template/Participantes.html',
        controller: 'CtrlParticipante'
    });
    $routeProvider.when('/MantenimientoEvento', {
        templateUrl: 'JS/Template/MantenimientoEvento.html',
        controller: 'CtrlEvento'
    });
    $routeProvider.when('/AsignacionBusEvento', {
        templateUrl: 'JS/Template/BusInscripcion.html',
        controller: 'CtrlBusInscripcion'
    });
    $routeProvider.when('/Inscripciones', {
        templateUrl: 'JS/Template/Inscripcion.html',
        controller: 'CtrlParticipante'
    });
    $routeProvider.when('/NotaDeCredito', {
        templateUrl: 'JS/Template/NotaCredito.html',
        controller: 'CtrlNotaCredito'
    });
    $routeProvider.when('/MantenimientoOpcionEvento', {
        templateUrl: 'JS/Template/MantOpcEvento/MantenimientoOpcionEvento.html',
        controller: 'CtrlOpcEvento'
    });
    $routeProvider.when('/EliminarOpcionEvento', {
        templateUrl: 'JS/Template/MantOpcEvento/EliminarOpcionEvento.html',
        controller: 'CtrlEvento'
    });
    $routeProvider.when('/MantenimientoParentesco', {
        templateUrl: 'JS/Template/MantenimientoParentesco.html',
        controller: 'CtrlParentesco'
    });
    $routeProvider.when('/ListadoEvento', {
        templateUrl: 'JS/Template/RptListadoEvento.html',
        controller: 'CtrlReportes'
    });
    $routeProvider.when('/ListadoSaldos', {
        templateUrl: 'JS/Template/RptSaldosEvento.html',
        controller: 'CtrlReportes'
    });
    $routeProvider.otherwise({ redirectTo: '/' });

    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
});


