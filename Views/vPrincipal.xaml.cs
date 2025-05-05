namespace squispeT3A.Views;

public partial class vPrincipal : ContentPage
{
    private string tipoId, identificacion, nombre, apellido, correo;
    private DateTime fechaNacimiento;
    private double salario, aporteIESS;

    public vPrincipal(string tipoId, string identificacion, string nombre, string apellido, string correo, DateTime fechaNacimiento, decimal salario)
    {
        InitializeComponent();

        this.tipoId = tipoId;
        this.identificacion = identificacion;
        this.nombre = nombre;
        this.apellido = apellido;
        this.correo = correo;
        this.fechaNacimiento = fechaNacimiento;
        this.salario = (double)salario;
        this.aporteIESS = this.salario * 0.0945;

        int edad = DateTime.Now.Year - fechaNacimiento.Year;
        if (DateTime.Now.Date < fechaNacimiento.Date.AddYears(edad)) edad--;

        // Mostrar en los Labels
        lblTipoId.Text = $"Tipo ID: {tipoId}";
        lblIdentificacion.Text = $"Identificaci�n: {identificacion}";
        lblNombre.Text = $"Nombres: {nombre}";
        lblApellido.Text = $"Apellidos: {apellido}";
        lblCorreo.Text = $"Correo: {correo}";
        lblFecha.Text = $"Fecha Nacimiento: {fechaNacimiento:dd/MM/yyyy}";
        lblEdad.Text = $"Edad: {edad} a�os";
        lblSalario.Text = $"Salario: ${salario:F2}";
        lblAporte.Text = $"Aporte IESS: ${aporteIESS:F2}";
    }
    private async void btnExportar_Clicked(object sender, EventArgs e)
    {
        // Obtener ruta al Escritorio
        string carpeta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


        // Generar nombre de archivo �nico
        string nombreContacto = nombre.Replace(" ", "_");
        string fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string nombreArchivo = $"{nombreContacto}_{fecha}.txt";
        string ruta = Path.Combine(carpeta, nombreArchivo);

        int edad = DateTime.Now.Year - fechaNacimiento.Year;
        if (DateTime.Now.Date < fechaNacimiento.Date.AddYears(edad)) edad--;

        string contenido = $"Tipo ID: {tipoId}\n" +
                           $"Identificaci�n: {identificacion}\n" +
                           $"Nombre: {nombre}\n" +
                           $"Apellido: {apellido}\n" +
                           $"Correo: {correo}\n" +
                           $"Fecha Nacimiento: {fechaNacimiento:dd/MM/yyyy}\n" +
                           $"Edad: {edad} a�os\n" +
                           $"Salario: {this.salario:F2}\n" +
                           $"Aporte IESS: {this.aporteIESS:F2}";

        File.WriteAllText(ruta, contenido);
        string mensajeExito = $"Su inscripci�n ha sido guardada con �xito.\n\nDatos guardados:\n\n{contenido}";
        await DisplayAlert("Inscripci�n Exitosa", mensajeExito, "Aceptar");
        await Navigation.PushAsync(new Views.vContactos());
    }
}
