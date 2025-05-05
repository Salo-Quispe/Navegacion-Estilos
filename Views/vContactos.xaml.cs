using System.Security.Principal;
using System.Text.RegularExpressions;

namespace squispeT3A.Views;

public partial class vContactos : ContentPage
{
    
    public vContactos()
    {
        InitializeComponent();
        txtNombres.TextChanged += TxtNombres_TextChanged;
        txtApellidos.TextChanged += TxtApellidos_TextChanged;
    }

    private void TxtNombres_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!Regex.IsMatch(e.NewTextValue, @"^[a-zA-Z\s]*$"))
        {
            txtNombres.Text = e.OldTextValue;
        }
    }

    private void TxtApellidos_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!Regex.IsMatch(e.NewTextValue, @"^[a-zA-Z\s]*$"))
        {
            txtApellidos.Text = e.OldTextValue;
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        bool hayErrores = false;
        string tipoId = "";

        if (pickerTipoId.SelectedIndex == -1)
        {
            await DisplayAlert("Error", "Seleccione el tipo de identificación.", "OK");
            hayErrores = true;
        }
        else
        {
            tipoId = pickerTipoId.SelectedItem.ToString();
        }

        string identificacion = txtIdentificacion.Text?.Trim() ?? "";
        string apellidos = txtApellidos.Text?.Trim() ?? "";
        string nombres = txtNombres.Text?.Trim() ?? "";
        string correo = txtCorreo.Text?.Trim() ?? "";
        string salarioTexto = txtSalario.Text?.Trim() ?? "";


        if (string.IsNullOrWhiteSpace(identificacion) ||
            string.IsNullOrWhiteSpace(apellidos) ||
            string.IsNullOrWhiteSpace(nombres) ||
            string.IsNullOrWhiteSpace(correo) ||
            string.IsNullOrWhiteSpace(salarioTexto))
        {
            await DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
            hayErrores = true;
        }

        // Validar formato de salario (coma como decimal)
        if (!decimal.TryParse(salarioTexto.Replace(',', '.'), out decimal salario))
        {
            await DisplayAlert("Error", "El salario debe ser un número válido (usa coma para separar los centavos).", "OK");
            hayErrores = true;
        }

        // Validación de identificación según tipo
        if (tipoId == "CÉDULA")
        {
            if (!Regex.IsMatch(identificacion, @"^\d{10}$"))
            {
                await DisplayAlert("Error", "La cédula debe tener exactamente 10 dígitos numéricos.", "OK");
                hayErrores = true;
            }
        }
        else if (tipoId == "PASAPORTE")
        {
            if (!Regex.IsMatch(identificacion, @"^[a-zA-Z0-9]+$"))
            {
                await DisplayAlert("Error", "El pasaporte debe ser alfanumérico.", "OK");
                hayErrores = true;
            }
        }
        else if (tipoId == "RUC")
        {
            if (!Regex.IsMatch(identificacion, @"^\d{13}$"))
            {
                await DisplayAlert("Error", "El RUC debe tener exactamente 13 dígitos numéricos.", "OK");
                hayErrores = true;
            }
        }

        // Calcular año nacimiento y edad
        DateTime fechaNacimiento = dateFecha.Date;
        int edad = DateTime.Now.Year - fechaNacimiento.Year;
        if (fechaNacimiento > DateTime.Now.AddYears(-edad)) edad--;

        // Ir a vista de resumen
        if (!hayErrores)
        {
            await Navigation.PushAsync(new Views.vPrincipal(tipoId, identificacion, nombres, apellidos, correo, fechaNacimiento, salario));

        }

    }
}
