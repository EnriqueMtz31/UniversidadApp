using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using MySql.Data.MySqlClient;
using System.Data;

namespace UniversidadApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private EditText edtMatricula, edtNombre, edtMateria;
        private Button btnInsertar;
        private TextView tvLog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            SetWidgetsFromView();
            SeClickForBtnInsert();

        }

        private void SetWidgetsFromView()
        {
            edtMatricula = FindViewById<EditText>(Resource.Id.edtMatricula);
            edtNombre = FindViewById<EditText>(Resource.Id.edtNombre);
            edtMateria = FindViewById<EditText>(Resource.Id.edtMateria);
            btnInsertar = FindViewById<Button>(Resource.Id.btnInsertar);
            tvLog = FindViewById<TextView>(Resource.Id.tvLog);
        }

        private void SeClickForBtnInsert()
        {
            btnInsertar.Click += BtnInsertar_Click;
        }

        private void BtnInsertar_Click(object sender, System.EventArgs e)
        {
            MySqlConnection con;
            MySqlConnectionStringBuilder conBuilder = new MySqlConnectionStringBuilder
            {
                Port = 3306,
                Server = "db4free.net",
                Database = "univenrique",
                UserID = "enriquemtzp",
                Password = "12345678",
                OldGuids = true
            };

            con = new MySqlConnection(conBuilder.ToString());

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = con,
                        CommandText = "INSERT INTO alumno(matricula,nombre,materia) VALUES(@matri,@nomb,@mater)"
                    };

                    cmd.Parameters.AddWithValue("@matri", int.Parse(edtMatricula.Text));
                    cmd.Parameters.AddWithValue("@nomb", edtNombre.Text);
                    cmd.Parameters.AddWithValue("@mater", edtMateria.Text);

                    cmd.ExecuteNonQuery();
                    
                    tvLog.Text = GetString(Resource.String.data_successfully_inserted);
                    edtMatricula.Text = string.Empty;
                    edtNombre.Text = string.Empty;
                    edtMateria.Text = string.Empty;
                }
            }
            catch (MySqlException ex)
            {
                tvLog.Text = ex.ToString();
            }
            catch (System.TimeoutException)
            {
                tvLog.Text = GetString(Resource.String.timeout_ex_txt);
            }
            finally
            {
                con.Close();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}