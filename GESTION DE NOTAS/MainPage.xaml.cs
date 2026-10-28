using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace GESTION_DE_NOTAS
{
    public partial class MainPage : ContentPage
    {
        private class EstudianteNotas
        {
            public string Seg1 { get; set; } = "";
            public string Exa1 { get; set; } = "";
            public string Seg2 { get; set; } = "";
            public string Exa2 { get; set; } = "";
        }

        private readonly Dictionary<string, EstudianteNotas> _memoriaEstudiantes = new();
        private string? _estudianteActualKey = null;
        private readonly Random _random = new();
        private bool _modoClaro = false;

        public MainPage()
        {
            InitializeComponent();

            if (PickerEstudiantes.Items.Count > 0)
            {
                PickerEstudiantes.SelectedIndex = 0;
            }
        }

        private void OnCambiarTemaClicked(object? sender, EventArgs e)
        {
            _modoClaro = !_modoClaro;

            if (_modoClaro)
            {
                // APLICAR MODO CLARO
                PageRoot.BackgroundColor = Color.FromArgb("#F1F5F9");
                BtnCambiarTema.Text = "🌙 MODO OSCURO";
                BtnCambiarTema.BackgroundColor = Color.FromArgb("#0F172A");
                BtnCambiarTema.TextColor = Colors.White;

                HeaderBorder.BackgroundColor = Colors.White;
                LblNombreHeader.TextColor = Color.FromArgb("#0F172A");
                LblSaludo.TextColor = Color.FromArgb("#64748B");

                CardGauge.BackgroundColor = Colors.White;
                LblNotaFinalGauge.TextColor = Color.FromArgb("#0F172A");
                LblSubtituloGauge.TextColor = Color.FromArgb("#64748B");
                CircleTrack.Stroke = Color.FromArgb("#E2E8F0");

                CardParciales.BackgroundColor = Colors.White;
                LblP1Titulo.TextColor = Color.FromArgb("#64748B");
                LblP2Titulo.TextColor = Color.FromArgb("#64748B");
                LblParcial1Gauge.TextColor = Color.FromArgb("#0F172A");
                LblParcial2Gauge.TextColor = Color.FromArgb("#0F172A");

                CardEstud.BackgroundColor = Colors.White;
                BoxPicker.BackgroundColor = Color.FromArgb("#F8FAFC");
                PickerEstudiantes.TextColor = Color.FromArgb("#0F172A");

                CardP1.BackgroundColor = Colors.White;
                CardP2.BackgroundColor = Colors.White;
                CardFecha.BackgroundColor = Colors.White;
                DpFecha.TextColor = Color.FromArgb("#0F172A");

                LblSeg1.TextColor = Color.FromArgb("#64748B");
                LblEx1.TextColor = Color.FromArgb("#64748B");
                LblSeg2.TextColor = Color.FromArgb("#64748B");
                LblEx2.TextColor = Color.FromArgb("#64748B");

                TxtSeguimiento1.BackgroundColor = Color.FromArgb("#F8FAFC");
                TxtSeguimiento1.TextColor = Color.FromArgb("#0F172A");
                TxtExamen1.BackgroundColor = Color.FromArgb("#F8FAFC");
                TxtExamen1.TextColor = Color.FromArgb("#0F172A");
                TxtSeguimiento2.BackgroundColor = Color.FromArgb("#F8FAFC");
                TxtSeguimiento2.TextColor = Color.FromArgb("#0F172A");
                TxtExamen2.BackgroundColor = Color.FromArgb("#F8FAFC");
                TxtExamen2.TextColor = Color.FromArgb("#0F172A");

                ModalBorder.BackgroundColor = Colors.White;
                CardContenidoModal.BackgroundColor = Color.FromArgb("#F8FAFC");
                LblModalContenido.TextColor = Color.FromArgb("#0F172A");
            }
            else
            {
                // MODO OSCURO
                PageRoot.BackgroundColor = Color.FromArgb("#0F1322");
                BtnCambiarTema.Text = "☀️ MODO CLARO";
                BtnCambiarTema.BackgroundColor = Color.FromArgb("#222C4A");
                BtnCambiarTema.TextColor = Colors.White;

                HeaderBorder.BackgroundColor = Color.FromArgb("#171D32");
                LblNombreHeader.TextColor = Colors.White;
                LblSaludo.TextColor = Color.FromArgb("#94A3B8");

                CardGauge.BackgroundColor = Color.FromArgb("#171D32");
                LblNotaFinalGauge.TextColor = Colors.White;
                LblSubtituloGauge.TextColor = Color.FromArgb("#94A3B8");
                CircleTrack.Stroke = Color.FromArgb("#222C4A");

                CardParciales.BackgroundColor = Color.FromArgb("#171D32");
                LblP1Titulo.TextColor = Color.FromArgb("#94A3B8");
                LblP2Titulo.TextColor = Color.FromArgb("#94A3B8");
                LblParcial1Gauge.TextColor = Colors.White;
                LblParcial2Gauge.TextColor = Colors.White;

                CardEstud.BackgroundColor = Color.FromArgb("#171D32");
                BoxPicker.BackgroundColor = Color.FromArgb("#0F1322");
                PickerEstudiantes.TextColor = Colors.White;

                CardP1.BackgroundColor = Color.FromArgb("#171D32");
                CardP2.BackgroundColor = Color.FromArgb("#171D32");
                CardFecha.BackgroundColor = Color.FromArgb("#171D32");
                DpFecha.TextColor = Colors.White;

                LblSeg1.TextColor = Color.FromArgb("#94A3B8");
                LblEx1.TextColor = Color.FromArgb("#94A3B8");
                LblSeg2.TextColor = Color.FromArgb("#94A3B8");
                LblEx2.TextColor = Color.FromArgb("#94A3B8");

                TxtSeguimiento1.BackgroundColor = Color.FromArgb("#0F1322");
                TxtSeguimiento1.TextColor = Colors.White;
                TxtExamen1.BackgroundColor = Color.FromArgb("#0F1322");
                TxtExamen1.TextColor = Colors.White;
                TxtSeguimiento2.BackgroundColor = Color.FromArgb("#0F1322");
                TxtSeguimiento2.TextColor = Colors.White;
                TxtExamen2.BackgroundColor = Color.FromArgb("#0F1322");
                TxtExamen2.TextColor = Colors.White;

                ModalBorder.BackgroundColor = Color.FromArgb("#171D32");
                CardContenidoModal.BackgroundColor = Color.FromArgb("#0F1322");
                LblModalContenido.TextColor = Colors.White;
            }
        }

        private void OnEstudianteSelected(object? sender, EventArgs e)
        {
            GuardarNotasEstudianteActual();

            if (PickerEstudiantes.SelectedIndex != -1 && PickerEstudiantes.SelectedItem != null)
            {
                _estudianteActualKey = PickerEstudiantes.SelectedItem.ToString();
                LblNombreHeader.Text = _estudianteActualKey;

                if (!_memoriaEstudiantes.ContainsKey(_estudianteActualKey!))
                {
                    _memoriaEstudiantes[_estudianteActualKey!] = new EstudianteNotas();
                }

                var notas = _memoriaEstudiantes[_estudianteActualKey!];
                TxtSeguimiento1.Text = notas.Seg1;
                TxtExamen1.Text = notas.Exa1;
                TxtSeguimiento2.Text = notas.Seg2;
                TxtExamen2.Text = notas.Exa2;

                CalcularNotasGenerales();
            }
        }

        private void GuardarNotasEstudianteActual()
        {
            if (!string.IsNullOrEmpty(_estudianteActualKey) && _memoriaEstudiantes.ContainsKey(_estudianteActualKey))
            {
                _memoriaEstudiantes[_estudianteActualKey].Seg1 = TxtSeguimiento1.Text;
                _memoriaEstudiantes[_estudianteActualKey].Exa1 = TxtExamen1.Text;
                _memoriaEstudiantes[_estudianteActualKey].Seg2 = TxtSeguimiento2.Text;
                _memoriaEstudiantes[_estudianteActualKey].Exa2 = TxtExamen2.Text;
            }
        }

        private void OnNota1Changed(object? sender, TextChangedEventArgs e)
        {
            CalcularNotasGenerales();
        }

        private void OnNota2Changed(object? sender, TextChangedEventArgs e)
        {
            CalcularNotasGenerales();
        }

        private void CalcularNotasGenerales()
        {
            double seg1 = ValidarYObtenerNota(TxtSeguimiento1.Text);
            double exa1 = ValidarYObtenerNota(TxtExamen1.Text);
            double seg2 = ValidarYObtenerNota(TxtSeguimiento2.Text);
            double exa2 = ValidarYObtenerNota(TxtExamen2.Text);

            double p1 = (seg1 >= 0 ? seg1 : 0) * 0.3 + (exa1 >= 0 ? exa1 : 0) * 0.2;
            double p2 = (seg2 >= 0 ? seg2 : 0) * 0.3 + (exa2 >= 0 ? exa2 : 0) * 0.2;

            LblParcial1Gauge.Text = $"{p1:F2} / 5";
            LblParcial2Gauge.Text = $"{p2:F2} / 5";

            ProgressP1.Progress = Math.Clamp(p1 / 5.0, 0.0, 1.0);
            ProgressP2.Progress = Math.Clamp(p2 / 5.0, 0.0, 1.0);

            double notaFinal = p1 + p2;
            LblNotaFinalGauge.Text = $"{notaFinal:F1}";

            ActualizarAnilloProgreso(notaFinal);
            ActualizarColoresEstado(notaFinal);
        }

        private void ActualizarAnilloProgreso(double notaFinal)
        {
            double fraccion = Math.Clamp(notaFinal / 10.0, 0.0, 1.0);
            double dashVisible = fraccion * 100;
            double dashInvisible = 100 - dashVisible;

            CircleProgress.StrokeDashArray = new DoubleCollection { dashVisible, dashInvisible };
        }

        private void ActualizarColoresEstado(double notaFinal)
        {
            if (notaFinal >= 7.0)
            {
                LblEstadoGauge.Text = "APROBADO";
                BadgeEstadoGauge.BackgroundColor = Color.FromArgb("#10B981");
                CircleProgress.Stroke = Color.FromArgb("#10B981");
            }
            else if (notaFinal >= 5.0 && notaFinal <= 6.9)
            {
                LblEstadoGauge.Text = "COMPLEMENTARIO";
                BadgeEstadoGauge.BackgroundColor = Color.FromArgb("#F59E0B");
                CircleProgress.Stroke = Color.FromArgb("#F59E0B");
            }
            else
            {
                LblEstadoGauge.Text = "REPROBADO";
                BadgeEstadoGauge.BackgroundColor = Color.FromArgb("#EF4444");
                CircleProgress.Stroke = Color.FromArgb("#EF4444");
            }
        }

        private double ValidarYObtenerNota(string? valorTexto)
        {
            if (string.IsNullOrWhiteSpace(valorTexto)) return 0.0;

            string textoNormalizado = valorTexto.Replace(',', '.');

            if (double.TryParse(textoNormalizado, NumberStyles.Any, CultureInfo.InvariantCulture, out double valor))
            {
                if (valor >= 0.0 && valor <= 10.0)
                {
                    return valor;
                }
            }
            return -1.0;
        }

        private async void OnCalcularClicked(object? sender, EventArgs e)
        {
            if (PickerEstudiantes.SelectedIndex == -1)
            {
                await DisplayAlertAsync("Atención", "Por favor, seleccione un estudiante de la lista.", "OK");
                return;
            }

            double seg1 = ValidarYObtenerNota(TxtSeguimiento1.Text);
            double ex1 = ValidarYObtenerNota(TxtExamen1.Text);
            double seg2 = ValidarYObtenerNota(TxtSeguimiento2.Text);
            double ex2 = ValidarYObtenerNota(TxtExamen2.Text);

            if (seg1 < 0 || ex1 < 0 || seg2 < 0 || ex2 < 0)
            {
                await DisplayAlertAsync("Error de Validación", "Todas las notas deben ser numéricas válidas entre 0 y 10.", "OK");
                return;
            }

            string estudiante = PickerEstudiantes.SelectedItem?.ToString() ?? "";
            DateTime fechaSeleccionada = DpFecha.Date ?? DateTime.Now;
            string fecha = fechaSeleccionada.ToString("dd/MM/yyyy");

            double parcial1 = (seg1 * 0.3) + (ex1 * 0.2);
            double parcial2 = (seg2 * 0.3) + (ex2 * 0.2);
            double notaFinal = parcial1 + parcial2;

            string estado = "";
            string[] emojis;

            if (notaFinal >= 7.0)
            {
                estado = "APROBADO";
                ModalBorder.Stroke = Color.FromArgb("#10B981");
                LblModalHeader.TextColor = Color.FromArgb("#10B981");
                BtnCerrarModal.BackgroundColor = Color.FromArgb("#10B981");
                emojis = new string[] { "🎉", "✨", "🥳", "⭐", "🏆", "🎊" };
            }
            else if (notaFinal >= 5.0 && notaFinal <= 6.9)
            {
                estado = "COMPLEMENTARIO";
                ModalBorder.Stroke = Color.FromArgb("#F59E0B");
                LblModalHeader.TextColor = Color.FromArgb("#F59E0B");
                BtnCerrarModal.BackgroundColor = Color.FromArgb("#F59E0B");
                emojis = new string[] { "⚠️", "⚡", "📖", "⏳", "💬" };
            }
            else
            {
                estado = "REPROBADO";
                ModalBorder.Stroke = Color.FromArgb("#EF4444");
                LblModalHeader.TextColor = Color.FromArgb("#EF4444");
                BtnCerrarModal.BackgroundColor = Color.FromArgb("#EF4444");
                emojis = new string[] { "😢", "😭", "🌧️", "💔", "🥀" };
            }

            // FORMATO EXACTO EXIGIDO CON \n
            string mensaje = $"Nombre: {estudiante}\n" +
                             $"Fecha: {fecha}\n" +
                             $"Nota Parcial 1: {parcial1:F2}\n" +
                             $"Nota Parcial 2: {parcial2:F2}\n" +
                             $"Nota Final: {notaFinal:F2}\n" +
                             $"Estado: {estado}";

            LblModalContenido.Text = mensaje;
            ModalOverlay.IsVisible = true;

            _ = ExplotarEfectosUltraLentos15s(emojis, notaFinal >= 7.0);

            ModalBorder.Scale = 0.5;
            await ModalBorder.ScaleToAsync(1.0, 350, Easing.SpringOut);
        }

        private async Task ExplotarEfectosUltraLentos15s(string[] conjuntoEmojis, bool esAprobado)
        {
            ContainerParticulas.Children.Clear();

            double width = RootLayout.Width > 0 ? RootLayout.Width : 1200;
            double height = RootLayout.Height > 0 ? RootLayout.Height : 800;

            double centerX = width / 2;
            double centerY = height / 2;

            List<Task> tareasAnimacion = new List<Task>();

            for (int i = 0; i < 24; i++)
            {
                var labelEmoji = new Label
                {
                    Text = conjuntoEmojis[_random.Next(conjuntoEmojis.Length)],
                    FontSize = _random.Next(55, 80),
                    Opacity = 1.0
                };

                AbsoluteLayout.SetLayoutBounds(labelEmoji, new Rect(centerX - 40, centerY - 40, 80, 80));
                ContainerParticulas.Children.Add(labelEmoji);

                double angulo = _random.NextDouble() * 2 * Math.PI;
                double radio = _random.Next(400, 750);

                double targetX = Math.Cos(angulo) * radio;
                double targetY = esAprobado
                    ? Math.Sin(angulo) * radio - 300
                    : Math.Abs(Math.Sin(angulo) * radio) + 350;

                uint duracion = (uint)_random.Next(14000, 16000);
                uint rotacion = (uint)_random.Next(90, 270);

                var animacionDesplazamiento = labelEmoji.TranslateToAsync(targetX, targetY, duracion, Easing.SinOut);
                var animacionRotacion = labelEmoji.RotateToAsync(rotacion, duracion, Easing.Linear);

                var animacionFade = Task.Run(async () => {
                    await Task.Delay((int)(duracion * 0.75));
                    await labelEmoji.FadeToAsync(0, (uint)(duracion * 0.25), Easing.CubicIn);
                });

                tareasAnimacion.Add(Task.WhenAll(animacionDesplazamiento, animacionRotacion, animacionFade));
            }

            await Task.WhenAll(tareasAnimacion);
        }

        private async void OnCerrarModalClicked(object? sender, EventArgs e)
        {
            ContainerParticulas.Children.Clear();
            await ModalBorder.ScaleToAsync(0.5, 150, Easing.CubicIn);
            ModalOverlay.IsVisible = false;
        }
    }
}