namespace RGO_LIPA
{
    public partial class WELCOME : Form
    {
        public WELCOME()
        {
            InitializeComponent();
        }

        private void btnStudent_Click(object sender, EventArgs e)
        {
            var newform = new STUDENT_PORTAL();
            newform.ShowDialog();
            this.Close();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            var newform = new ADMIN_LOGIN();
            newform.ShowDialog(); 
            this.Close();
        }
    }
}