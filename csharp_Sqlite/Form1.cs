using System;
using csharp_Sqlite.Models;
using DGVPrinterHelper;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace csharp_Sqlite
{

    public partial class Form1 : Form
    {
        int conta_parcial=0;
        double peso_parcial = 0;
        int conta_confer = 0;
        List<Produto> Produtos;
        public Form1()
        {
            InitializeComponent();
        }
        private void btnCriarBancoDados_Click(object sender, EventArgs e)
        {
            try
            {
                DalHelper.CriarBancoSQLite();
                btnCriarBancoDados.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void btnCriarTabela_Click(object sender, EventArgs e)
        {
            try
            {
                DalHelper.CriarTabelaSQlite();
                btnCriarTabela.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void btnIncluirDados_Click(object sender, EventArgs e)
        {
            if (!Valida())
            {
                MessageBox.Show("Informe os dados cliente a incluir");
                return;
            }

            try
            {
                Cliente cli = new Cliente();
                cli.Id = Convert.ToInt32(txtID.Text);
                cli.Nome = txtNome.Text;
                cli.Email = txtEmail.Text;
                DalHelper.Add(cli);
                ExibirDados();
                LimpaDados();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void btnExibirDados_Click(object sender, EventArgs e)
        {
            ExibirDados();
        }

        private void ExibirDados()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DalHelper.GetProdutos();
                dgvDados.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

          private void LimpaDadosProd()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DalHelper.LimpaTabProdutos();
                dgvDados.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

         private void btnAtualizarDados_Click(object sender, EventArgs e)
        {
            if(!Valida())
            {
                MessageBox.Show("Informe os dados cliente a atualizar");
                return;
            }

            try
            {
                Cliente cli = new Cliente();
                cli.Id = Convert.ToInt32(txtID.Text);
                cli.Nome = txtNome.Text;
                cli.Email = txtEmail.Text;
                DalHelper.Update(cli);
                ExibirDados();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void btnExcluirDados_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Informe o ID do cliente a ser Excluído");
                return;
            }

            try
            {
                int codigo = Convert.ToInt32(txtID.Text);
                DalHelper.Delete(codigo);
                ExibirDados();
                LimpaDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void btnLocalizarDados_Click(object sender, EventArgs e)
        {
            string peso_parc;
            int id;
            string peso_ret = "0";
            int aberto_ret = 0;
            int confer_ret = 0;
            bool erro_conf;
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Informe o ID do cliente a ser Localizado");
                return;
            }
            try
            {
                DataTable dt = new DataTable();
                //DataTable dt1 = new DataTable();
                //int codigo = Convert.ToInt32(txtID.Text);           

                string codigo = txtID.Text;
                
                erro_conf = DalHelper.GetProduto(codigo, conta_confer,out id,out peso_ret,out aberto_ret,out confer_ret,out peso_parc);
                if (erro_conf == false)
                {
                    conta_parcial = conta_parcial + 1;
                    //peso_parcial = ((Convert.ToDouble(peso_parc)*confer_ret) + Convert.ToDouble(peso_parcial)).ToString();
                    peso_parcial = Convert.ToDouble(peso_parc)+ Convert.ToDouble(peso_parcial);
                    DalHelper.AtualizaProduto(codigo, confer_ret, aberto_ret, peso_ret);
                    dt = DalHelper.GetProdutos();
                    dgvDados.DataSource = dt;
                    // Me.Datagrid.Rows(i).Selected = True
                    dgvDados.CurrentCell = dgvDados.Rows[id - 1].Cells[dgvDados.CurrentCellAddress.X];
                    //soma_peso = DalHelper.Soma();
                    txtPesoTot.Text = peso_parcial.ToString();
                    txtQtdParc.Text = conta_parcial.ToString();
                }
                else
                {
                     const string mensagem = "Erro Conf. Terminada ?";
                     const string titulo = "Encerrar";
                    var resultado = MessageBox.Show(mensagem, titulo,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                }
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void dgvDados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvDados.Rows[e.RowIndex];
                txtID.Text = row.Cells["Id"].Value.ToString();
                txtNome.Text = row.Cells["Nome"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
            }
        }

        private bool Valida()
        {
            if(string.IsNullOrEmpty(txtID.Text) && string.IsNullOrEmpty(txtNome.Text) && string.IsNullOrEmpty(txtEmail.Text))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void LimpaDados()
        {
            txtID.Text = "";
            txtNome.Text = "";
            txtEmail.Text = "";
        }

        private void btnEncerrar_Click(object sender, EventArgs e)
        {
            const string mensagem = "Deseja Encerrar ?";
            const string titulo = "Encerrar";
            var resultado = MessageBox.Show(mensagem, titulo,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void BtnPrintDados_Click(object sender, EventArgs e)
        {
            PrintDados();
                       
        }

        private void PrintDados()
        {
            string data_hoje = DateTime.Now.ToString("dd/MM/yy");
            string hora = DateTime.Now.ToString("HH:mm:ss");
            string placa = "EAB 1648";
           
            DGVPrinter printer = new DGVPrinter();
            try
            {
                DataTable dt = new DataTable();
                dt = DalHelper.GetProdutos();
                dgvDados.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
            printer.Title = "Relatorio Carga"; //Header
            printer.SubTitle = data_hoje + " "+ hora+ " "+ "Placa " + placa ;                               //string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = System.Drawing.StringFormatFlags.LineLimit | System.Drawing.StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = System.Drawing.StringAlignment.Near;
            printer.Footer = "Imbramil";//Footer
            printer.FooterSpacing = 15;
            printer.PrintNoDisplay(dgvDados);
            //printer.PrintPreviewNoDisplay(dgvDados);
            //printer.PrintPreviewDataGridView(dgvDados); = 


        }

        private void btnImp_Click(object sender, EventArgs e)
        {
            bool resp;
            string carregamento = "6";
            string path = @"c:\carregamento\" + carregamento + ".txt";
            Stopwatch stopwatch = Stopwatch.StartNew();
                while (true)
                {
                    //some other processing to do possible
                    if (stopwatch.ElapsedMilliseconds >= 60000)//millisecondsToWait
                    {
                        break;
                    }
                }

            resp = LearquivoProd(path);
            ExibirDados();


        }

        private bool LearquivoProd(string path)
        {
            int qtd_prod = 0;
            Produtos = new List<Produto>();
            //BdAcesso bdo = new BdAcesso();


            if (System.IO.File.Exists(path))
            {
                //label2.Text = "Iniciando a Carga do Arquivo de Ordem";
                //considerando que o arquivo existe
                using (StreamReader sr = new StreamReader(path))
                {
                   
                    while (sr.Peek() >= 0)
                    {
                        string str = null;
                        string[] strArray = null;
                        str = sr.ReadLine();

                        strArray = str.Split(';');
                        Produto currentApp = new Produto();
                        // currentApp.ordem = int.Parse(strArray[1]);

                        currentApp.Id = Convert.ToInt32(strArray[0]);
                        currentApp.Carregamento= strArray[1];
                        currentApp.codigo = strArray[2];
                        currentApp.produto = strArray[3];
                        currentApp.QtdePed = Convert.ToInt32(strArray[4]);                        
                        currentApp.Peso_Unit= strArray[5];
                        currentApp.QtdeAberto = 0;
                        currentApp.QtdeConf = 0;
                        currentApp.Peso = "0";
                                       
                        DalHelper.Add(currentApp);
                        qtd_prod = qtd_prod + currentApp.QtdePed;
                        //txtQtdTotal.Text = qtd_prod.ToString();
                        //Produtos.Add(currentApp);
                        currentApp.QtdeTotal = qtd_prod;
                        txtQtdTotal.Text = currentApp.QtdeTotal.ToString();
                    }
                    
                }
                return true;
            }
            else
            {
                label2.Text = "Arquivo não Existe";
                return false;
            }

        }

        private void btnExcluiDados_Click(object sender, EventArgs e)
        {
            LimpaDadosProd();
        }
    }
}
