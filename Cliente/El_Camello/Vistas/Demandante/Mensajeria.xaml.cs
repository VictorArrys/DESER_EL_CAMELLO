﻿using El_Camello.Modelo.clases;
using El_Camello.Modelo.dao;
using El_Camello.Vistas.Demandante.controles;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace El_Camello.Vistas.Demandante
{
    /// <summary>
    /// Lógica de interacción para Mensajeria.xaml
    /// </summary>
    public partial class Mensajeria : Window
    {
        Modelo.clases.Demandante perfilDemandante;
        Modelo.clases.Conversacion conversacionSeleccionada;

        public Mensajeria(Modelo.clases.Demandante perfilDemandante)
        {
            InitializeComponent();
            this.perfilDemandante = perfilDemandante;
            CargarListaConversaciones();
        }

        private async void CtrlConversacion_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Cargar conversacion y habilitar botones
            int idConversacion = ((ConversacionControl)e.Source).Conversacion.IdConversacion;
            conversacionSeleccionada = await ConversacionesDAO.GetConversacionDemandante(idConversacion, perfilDemandante.IdDemandante, perfilDemandante.Token);
            CargarConversacion();
        }

        private void btnEnviarMensaje_Click(object sender, RoutedEventArgs e)
        {
            btnEnviarMensajeAsync();
        }

        private async void CargarListaConversaciones()
        {
            List<Conversacion> listaConversaciones = await ConversacionesDAO.GetConversacionesDemandante(perfilDemandante.IdDemandante, perfilDemandante.Token);
            foreach (Conversacion conversacion in listaConversaciones)
            {
                ConversacionControl ctrlConversacion = new ConversacionControl();
                ctrlConversacion.Conversacion = conversacion;
                ctrlConversacion.MouseLeftButtonUp += CtrlConversacion_MouseDown;
                pnlConversaciones.Children.Add(ctrlConversacion);
            }
        }

        private void CargarConversacion()
        {
            this.lblNombreConversacion.Content = conversacionSeleccionada.Titulo;
            pnl_Chat.Children.Clear();
            txtMensaje.Text = "";
            txtMensaje.IsEnabled = true;
            btnEnviarMensaje.IsEnabled = true;
            foreach (var mensaje in conversacionSeleccionada.Mensajes)
            {
                MostrarMensaje(mensaje);
            }
        }

        private void MostrarMensaje(Mensaje mensaje)
        {
            bool esRemitente = (mensaje.IdUsuarioRemitente == perfilDemandante.IdPerfilusuario) ? true : false;
            CuadroMensaje nuevoMensaje = new CuadroMensaje(mensaje, esRemitente);
            pnl_Chat.Children.Add(nuevoMensaje);
            scrollConversacion.ScrollToBottom();
        }



        private async void btnEnviarMensajeAsync()
        {
            string contenidoMensaje = txtMensaje.Text;
            if (contenidoMensaje.Length > 0)
            {
                Mensaje mensaje = await ConversacionesDAO.PostMensajeDemandante(
                    conversacionSeleccionada.IdConversacion,
                    perfilDemandante.IdDemandante,
                    contenidoMensaje,
                    perfilDemandante.Token);
                txtMensaje.Text = "";
                if(mensaje.IdMensaje > 0)
                {
                    MostrarMensaje(mensaje);
                }
            }
        }
    }

    /// <summary>
    /// Contenedor WPF para los mensaje recibidos del chat
    /// </summary>
    class CuadroMensaje : StackPanel
    {
        TextBlock mensaje;
        Label remitente;
        Label fecha;

        /// <summary>
        /// Constructor del contenedor WPF de un mensaje
        /// </summary>
        /// <param name="mensajeRecibido">Mensaje recibido del servidor de la sala de chat</param>
        /// <param name="esRemitente">True: El usuario conectado es remitente, False: El usuario conectado es destinatario</param>
        public CuadroMensaje(Mensaje mensajeRecibido, bool esRemitente)
        {
            remitente = new Label();
            fecha = new Label();
            mensaje = new TextBlock();

            remitente.Content = $"{mensajeRecibido.Remitente} ({mensajeRecibido.TipoUsuario})";
            fecha.Content = mensajeRecibido.FechaRegistro;
            mensaje.Text = mensajeRecibido.ContenidoMensaje;

            ConstruirCuadro(esRemitente);
        }

        /// <summary>
        /// Da formato al coneteneder del mensaje
        /// </summary>
        /// <param name="esRemitente">True: El usuario conectado es remitente, False: El usuario conectado es destinatario</param>
        private void ConstruirCuadro(bool esRemitente)
        {
            Thickness padding = new Thickness(10, 5, 10, 5);
            remitente.Padding = padding;
            remitente.HorizontalContentAlignment = HorizontalAlignment.Right;
            fecha.Padding = padding;
            mensaje.Padding = padding;
            mensaje.TextWrapping = TextWrapping.Wrap;
            mensaje.FontSize = 15;

            if (esRemitente)
            {
                this.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                Margin = new Thickness(60, 10, 20, 10);
            }
            else
            {
                this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                Margin = new Thickness(20, 10, 60, 10);
            }

            StackPanel panel = new StackPanel();
            panel.Children.Add(fecha);
            panel.Children.Add(mensaje);
            panel.Children.Add(remitente);

            Border marco = new Border();
            marco.Background = Brushes.White;
            marco.CornerRadius = new CornerRadius(10, 10, 10, 10);
            marco.Child = panel;
            this.Children.Add(marco);
        }
    }
}
