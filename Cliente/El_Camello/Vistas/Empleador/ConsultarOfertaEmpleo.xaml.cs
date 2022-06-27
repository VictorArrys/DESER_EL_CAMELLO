﻿using El_Camello.Aspirante;
using El_Camello.Assets.utilerias;
using El_Camello.Empleador;
using El_Camello.Modelo.clases;
using El_Camello.Modelo.dao;
using System;
using System.Collections.Generic;
using System.Windows;

namespace El_Camello.Vistas.Empleador
{
    /// <summary>
    /// Interaction logic for ConsultarOfertaEmpleo.xaml
    /// </summary>
    public partial class ConsultarOfertaEmpleo : Window
    {
        int idOfertaEmpleo;
        private string token = "";
        OfertaEmpleo ofertaEmpleoConsulta;
        private List<ContratacionEmpleoAspirante> contratados;

        MensajesSistema error;

        public ConsultarOfertaEmpleo(int idOfertaEmpleo, string token)
        {
            this.token = token;
            this.idOfertaEmpleo = idOfertaEmpleo;
            InitializeComponent();

            cargarOfertaEmpleo();
        }

        private async void cargarOfertaEmpleo()
        {
            try
            {
                string tokenString = "" + token;

                ofertaEmpleoConsulta = await OfertaEmpleoDAO.GetOfertaEmpleoCompleta(idOfertaEmpleo, tokenString);

                lbNombreEmpleo.Text = ofertaEmpleoConsulta.Nombre;
                lbTipoPago.Text = ofertaEmpleoConsulta.TipoPago;
                lbCategoria.Text = ofertaEmpleoConsulta.CategoriaEmpleo;
                lbPago.Text = "$" + ofertaEmpleoConsulta.CantidadPago;
                string fechaFin = string.Format("{0:yyyy-MM-dd}", ofertaEmpleoConsulta.FechaFinalizacion);
                lbFechaFinalizacion.Text = fechaFin;
                string fechaContratacion = string.Format("{0:yyyy-MM-dd}", ofertaEmpleoConsulta.ContratacionEmpleo.FechaContratacion);

                if (ofertaEmpleoConsulta.FechaInicio > DateTime.Now)
                {
                    btnEvaluar.IsEnabled = false;
                    lbEstado.Text = "Por empezar";
                }
                if (ofertaEmpleoConsulta.FechaFinalizacion < DateTime.Now)
                {
                    btnEvaluar.IsEnabled = true;
                    lbEstado.Text = "Terminada";
                }

                if (fechaContratacion == "0001-01-01")
                {
                    lbFechaContratacion.Text = "Sin contratación";
                    btnEvaluar.IsEnabled = false;
                    lbEstado.Text = "Sin contratacion";
                }
                else
                {

                    lbFechaContratacion.Text = fechaContratacion;

                    /*if (ofertaEmpleoConsulta.ContratacionEmpleo.Estatus == 1)
                    {
                        lbEstado.Text = "En proceso";
                        btnEvaluar.IsEnabled = false;
                    }
                    else
                    {
                        lbEstado.Text = "Terminada";
                    }*/
                }


                contratados = ofertaEmpleoConsulta.ContratacionEmpleo.ContratacionesAspirantes;
                cargarEmpleados(ofertaEmpleoConsulta.ContratacionEmpleo.ContratacionesAspirantes);                

                if(contratados.Count == 0)
                {
                    btnConsultar.IsEnabled = false;
                }
            }
            catch (Exception exception)
            {
                error = new MensajesSistema("Error", "Hubo un error al cargar la oferta de empleo, favor de intentar más tarde", exception.StackTrace, exception.Message);
                error.ShowDialog();
            }

        }

        private async void cargarEmpleados(List<ContratacionEmpleoAspirante> listaEmpleados)
        {

            if (listaEmpleados.Count > 0)
            {

                dgEmpleados.ItemsSource = listaEmpleados;
            }

        }

        private void evaluarAspirante(object sender, RoutedEventArgs e)
        {
            int indiceSeleccion = dgEmpleados.SelectedIndex;
            ContratacionEmpleoAspirante aspiranteEvaluar = new ContratacionEmpleoAspirante();
            int valoracion = -1;
            if (indiceSeleccion >= 0)
            {
                int posicion = 0;
                foreach (var contratado in ofertaEmpleoConsulta.ContratacionEmpleo.ContratacionesAspirantes)
                {
                    if (contratados[posicion].IdUsuario == contratado.IdUsuario)
                    {
                        valoracion = contratado.ValoracionAspirante;
                        aspiranteEvaluar = contratados[indiceSeleccion];
                       
                    }

                }
                if (valoracion == 0)
                {

                    EvaluarApirante ventanaEvaluar = new EvaluarApirante(aspiranteEvaluar, aspiranteEvaluar.IdAspirante, idOfertaEmpleo, token);
                    ventanaEvaluar.ShowDialog();
                    cargarOfertaEmpleo();
                }
                else
                {
                    error = new MensajesSistema("AccionInvalida", "La acción que ha realizado es invalida", "Intento de evaluar un empleado", "No puedes evaluar a un aspirante 2 veces");
                    error.ShowDialog();
                }


            }
            else
            {
                error = new MensajesSistema("AccionInvalida", "La acción que ha realizado es invalida", "Intento de evaluar un empleado", "Selecciona un empleado para evaluarlo posteriormente");
                error.ShowDialog();
            }

        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            int indiceSeleccion = dgEmpleados.SelectedIndex;

            if (indiceSeleccion >= 0)
            {
                ContratacionEmpleoAspirante aspiranteConsultar = contratados[indiceSeleccion];

                PerfilAspirante perfilAspirante = new PerfilAspirante(aspiranteConsultar.IdUsuario, token);
                perfilAspirante.ShowDialog();
                /*EvaluarApirante ventanaEvaluar = new EvaluarApirante(aspiranteEvaluar, token);
                ventanaEvaluar.ShowDialog();*/
            }
            else
            {
                error = new MensajesSistema("AccionInvalida", "La acción que ha realizado es invalida", "Intento de consultar un aspirante", "Selecciona un aspirante para evaluarlo posteriormente");
                error.ShowDialog();
            }

        }


        private void cerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
