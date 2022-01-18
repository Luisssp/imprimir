using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csharp_Sqlite.Models;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace csharp_Sqlite
{
    class DalHelper
    {
       private static SQLiteConnection sqliteConnection;  //Connection;
       public DalHelper()
        { }
        private static SQLiteConnection DbConnection()
        {
            sqliteConnection = new SQLiteConnection("Data Source=c:\\dados\\sqlhelper.s3db; Version=3;");
            sqliteConnection.Open();
            return sqliteConnection;
        }
        public static void CriarBancoSQLite()
        {
            try
            {
                SQLiteConnection.CreateFile(@"C:\dados\sqlhelper.s3db");
            }
            catch
            {
                throw;
            }
        }
        public static void CriarTabelaSQlite()
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Clientes(id int, Nome Varchar(50), email VarChar(80))";
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetClientes()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Clientes";
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

       public static DataTable GetProdutos()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT CODIGO,PRODUTO,QTPED,CONFERIDA,ABERTO,PESO FROM PRODUTO";
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable GetCliente(int id)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Clientes Where Id=" + id;
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

          public static bool GetProduto(string produto, int count_confe, out int id,out string peso_ret,out int aberto_ret,out int confer_ret,out string peso_parc)
        {
            string cod = "";
            Int32 qt_ped = 0;
            Int32 confer = 0; 
            Int32 aberto = 0; 
            double peso = 0;
            double peso_unit = 0;
            int id_code = 0;
            id = 0;
            //SQLiteDataAdapter da = null;
            //DataTable dt = new DataTable();
             // define um SqlDataReader nulo
            SQLiteDataReader dr = null;
            // fazer a logica o pacote passou pelo leitor corretamente 
            // somar um no campo conferida
            // verifica se a quantidade conferida não é maior que a qtped
            // antes de somar..se sim erro
            // se não somar ao campo conferida e diminui o campo de aberto
            // adiciona o peso de uma no campo peso
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    //String query = "SELECT * FROM PRODUTO INNER JOIN PESO on PESO.PRODUTO = PRODUTO.PRODUTO Where PRODUTO.PRODUTO = ";
                    String query = "SELECT * FROM PRODUTO Where CODIGO = ";
                    query += String.Format("'{0}'", produto);
                    cmd.CommandText = query;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        id_code = Convert.ToInt32(dr["Id"].ToString());
                        cod = dr["CODIGO"].ToString();
                        qt_ped = Convert.ToInt32(dr["QTPED"].ToString());
                        confer = Convert.ToInt32(dr["CONFERIDA"].ToString());
                        aberto = Convert.ToInt32(dr["ABERTO"].ToString());
                        peso = Convert.ToDouble(dr["PESO"].ToString());
                        peso_unit = Convert.ToDouble(dr["PESO_UNIT"].ToString());
                    }
                    peso_parc = peso_unit.ToString();
                    
                    // ok somar os valores e atualizar o banco.
                    id = id_code;
                    count_confe++;
                    confer++;
                    aberto = qt_ped - confer;
                    peso = peso + peso_unit;
                    if (aberto < 0) // erro parar a esteira produto a mais
                    {
                        count_confe--;
                        confer--;
                        aberto++;
                        peso_ret = (peso- peso_unit).ToString();
                        aberto_ret = aberto;
                        confer_ret = confer;
                        return(true); // produto já totalmente conferido
                        // desligar esteira e informar na tela
                        //const string mensagem = "Deseja Encerrar ?";
                        //const string titulo = "Produto Não Confere";
                        //var resultado = MessageBox.Show(mensagem, titulo,
                        //                MessageBoxButtons.YesNo,
                        //                MessageBoxIcon.Question);

                        //if (resultado == DialogResult.Yes)
                        //{
                        //    Application.Exit();
                        //}
                    }
                    else
                    {
                        peso_ret = peso.ToString();
                        aberto_ret = aberto;
                        confer_ret = confer;

                        return(false);
                    }

                }// using
               
             }// try 
              catch(Exception ex)
                {
                    throw ex;
                }
           }
           //fazer query na tabela peso trazendo o valor do peso unitario
           // somar ao peso na tabela produtos
           // atualizar campos confer,aberto e peso també
          //public static DataTable AtualizaProduto(int id,string produto, int confer, int aberto, double peso)
          public static void AtualizaProduto(string produto, int confer, int aberto, string peso) 
          { 
             //SQLiteDataAdapter da = null;
             //DataTable dt = new DataTable();
            
                    try
                    {
                        Produto pro = new Produto();
                        pro.codigo = produto;
                        pro.QtdeConf= confer;
                        pro.QtdeAberto = aberto;
                        pro.Peso = peso;
                        DalHelper.Update(pro);
                        //ExibirDados();
                
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro : " + ex.Message);
                    }                  

                    //da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    //da.Fill(dt);
                    //return dt;              
            }            
   
         public static void Add(Cliente cliente)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Clientes(id, Nome, email ) values (@id, @nome, @email)";
                    cmd.Parameters.AddWithValue("@Id", cliente.Id);
                    cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@Email", cliente.Email);
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


         public static void Add(Produto produto)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO PRODUTO(Id, ORDEM,CODIGO,PRODUTO,QTPED,CONFERIDA,ABERTO,PESO_UNIT,PESO ) values (@id, @ORDEM, @CODIGO,@PRODUTO, @QTPED,@CONFERIDA,@ABERTO,@PESO_UNIT,@PESO)";
                    cmd.Parameters.AddWithValue("@id", produto.Id);
                    cmd.Parameters.AddWithValue("@ORDEM", produto.Carregamento);
                    cmd.Parameters.AddWithValue("@CODIGO", produto.codigo);
                    cmd.Parameters.AddWithValue("@PRODUTO", produto.produto);
                    cmd.Parameters.AddWithValue("@QTPED", produto.QtdePed);
                    cmd.Parameters.AddWithValue("@CONFERIDA", produto.QtdeConf);
                    cmd.Parameters.AddWithValue("@ABERTO", produto.QtdeAberto);
                    cmd.Parameters.AddWithValue("@PESO_UNIT", produto.Peso_Unit);
                    cmd.Parameters.AddWithValue("@PESO", produto.Peso);               

                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public static void Update(Cliente cliente)
        {
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    if (cliente.Id != null )
                    {
                        cmd.CommandText = "UPDATE Clientes SET Nome=@Nome, Email=@Email WHERE Id=@Id";
                        cmd.Parameters.AddWithValue("@Id", cliente.Id);
                        cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                        cmd.Parameters.AddWithValue("@Email", cliente.Email);
                        cmd.ExecuteNonQuery();
                    }
                };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

         public static void Update(Produto produto)
        {
           
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    //if (cliente.Id != null )
                    //{
                        //String query = "SELECT * FROM PRODUTO Where PRODUTO = ";
                        //query += String.Format("'{0}'", produto);
                        String query = "UPDATE PRODUTO SET CONFERIDA=@CONFERIDA, ABERTO=@ABERTO, PESO=@PESO WHERE CODIGO=@CODIGO";
                    //query += String.Format("'{0}'", produto);
                    // cmd.CommandText = "UPDATE PRODUTO SET Nome=@Nome, Email=@Email WHERE Id=@Id";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@CODIGO", produto.codigo);
                    cmd.Parameters.AddWithValue("@CONFERIDA", produto.QtdeConf);
                    cmd.Parameters.AddWithValue("@ABERTO", produto.QtdeAberto);
                    cmd.Parameters.AddWithValue("@PESO", produto.Peso);
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public static void Delete(int Id)
        {
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    cmd.CommandText = "DELETE FROM Clientes Where Id=@Id";
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static int Soma()
        {
           int quantidade = 0;
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    cmd.CommandText = "Select TOTAL(ABERTO) FROM PRODUTO";
                    quantidade = (int)cmd.ExecuteScalar();
                    return (quantidade);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

         public static DataTable LimpaTabProdutos()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM PRODUTO";
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
