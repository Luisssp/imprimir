using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using System.Data;
using System.Text;
using SQLitestudo;
using System.Windows.Forms;

namespace EmpresaCedcon
{
    class BdAcesso
    {


        public bool checaUsuarioPassword(string pass, string login)
        {
            try
            {

                SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");
                string retorno;

                String query = "select Login from Users where Login = ";
                //query += "Login LIKE " + login;
                query += String.Format("'{0}'", login);


                retorno = db.ExecuteScalar(query);

                //verifica se houve retorno e e se o usuario e o mesmo
                if ((retorno == "") || (retorno != login))
                {
                    return (false); // não bateu a senha.
                }

                query = "select Senha from Users Where Login = ";
                //query1 += "Login = " + "login";
                query += String.Format("'{0}'", login);


                retorno = db.ExecuteScalar(query);

                if ((retorno == "") || (retorno != pass))
                {
                    return (false);
                }
                return (true);
            }

            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                return (false);
            }
        }         

        public string DevolveCampoUser(string campo,string login)
        {
            try
            {

                SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");

                //String query = "select Nome from Users Where Login = ";
                String query = String.Format("select {0} from Users Where Login = ", campo);
                // query += "Login = " + "login";
                query += String.Format("'{0}'", login);

                return (db.ExecuteScalar(query));

            }

            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                return ("");
            }
        }

        public string DevolveCampoConfig(string campo, string id)
        {
            try
            {

                SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");

                //String query = "select Nome from Users Where Login = ";
                String query = String.Format("select {0} from Propr Where ProprId = ", campo);
                // query += "Login = " + "login";
                query += String.Format("{0}", id);

                return (db.ExecuteScalar(query));

            }

            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                return ("");
            }
        }

        public void delId(int Id)
        {
            try
            {
                SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");
                // String recipeID = "4";
                db.Delete("Users", String.Format("UserId = {0}", Id));
            }
            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                //return ("");
            }
        }

         public void delIdProd(int Id)
        {
            try
            {
                SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");
                // String recipeID = "4";
                db.Delete("prod", String.Format("seq_id = {0}", Id));
            }
            catch(Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                //return ("");
            }
        }

         public void delOrdem(string ordem) // Id
         {
             try
             {
                 SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");
                 // String recipeID = "4";
                 db.Delete("ordem", String.Format("ordem = '{0}'", ordem));
                 //query += String.Format("'{0}'", login);
             }
             catch (Exception fail)
             {
                 String error = "The following error has occurred:\n\n";
                 error += fail.Message.ToString() + "\n\n";
                 MessageBox.Show(error);
                 //this.Close();
                 //return ("");
             }
         }


         public void delIdProdSubst(int Id)
         {
             try
             {
                 SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");
                 // String recipeID = "4";
                 db.Delete("replacement", String.Format("repId = {0}", Id));
             }
             catch (Exception fail)
             {
                 String error = "The following error has occurred:\n\n";
                 error += fail.Message.ToString() + "\n\n";
                 MessageBox.Show(error);
                 //this.Close();
                 //return ("");
             }
         }

        public void upConfId(Dictionary<String, String> data, int Id)
        {
           
            SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");
            try
            {
                db.Update("Propr", data, String.Format("ProprId = {0}", Id)); // this.RecipeID
            }
            catch (Exception crap)
            {
                //MessageBox.Show(crap.Message);
                String error = "The following error has occurred:\n\n";
                error += crap.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                //return ("");
            }
        }

        public void insUser(Dictionary<String, String> data)
        {

            SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");
            try
            {
                db.Insert("Users", data);
            }
            catch (Exception crap)
            {
                //MessageBox.Show(crap.Message);
                String error = "The following error has occurred:\n\n";
                error += crap.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                //return ("");
            }
           
         }

        public DataTable DatatableUser(string query)
        {
          
            SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");
            return(db.GetDataTable(query));
               
        }

        public string DevolveCodSubst(string codigo, string campo)
        {
            try
            {

                SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");

                String query = String.Format("select {0} from replacement Where codprod = ", codigo);
                 query += String.Format("'{0}'", campo);

                return (db.ExecuteScalar(query));

            }
            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                return ("");
            }
        }
        

        public string DevolveCod(string codigo, string campo)
        {
            try
            {

                SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");

                //String query = "select Nome from Users Where Login = ";
                String query = String.Format("select {0} from prod Where seq_id = ", codigo); // codigo
                // query += "Login = " + "login";
                query += String.Format("'{0}'", campo);

                return (db.ExecuteScalar(query));

            }
            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                return ("");
            }
        }

        public string DevolveCodProd(string codigo, string campo)
        {
            try
            {

                SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");

                //String query = "select Nome from Users Where Login = ";
                String query = String.Format("select {0} from prod Where cod = ", codigo); // codigo
                // query += "Login = " + "login";
                query += String.Format("'{0}'", campo);

                return (db.ExecuteScalar(query));

            }
            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                return ("");
            }
        }


        public string DevolveSeqidProd(string codigo, string campo)
        {
            try
            {

                SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");

                //String query = "select Nome from Users Where Login = ";
                String query = String.Format("select {0} from prod Where cod = ", codigo); // codigo
                // query += "Login = " + "login";
                query += String.Format("'{0}'", campo);

                return (db.ExecuteScalar(query));

            }
            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                return ("");
            }
        }



        public string VerificaOrdem(string codigo, string campo)
        {
            try
            {

                SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");

                //String query = "select Nome from Users Where Login = ";
                String query = String.Format("select {0} from ordem Where ordem = ", codigo);
                // query += "Login = " + "login";
                query += String.Format("'{0}'", campo);

                return (db.ExecuteScalar(query));

            }
            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                return ("");
            }
        }

        //SELECT MIN (nivel) from montagem where ordem = '737910-001'
        
        public string AcessaNivelMin(string campo)
        {
            try
            {

                SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");

                //String query = "select Nome from Users Where Login = ";
                String query = "select min (seq) from ordem Where ordem = ";
                // query += "Login = " + "login";
                query += String.Format("'{0}'", campo);

                return (db.ExecuteScalar(query));

            }
            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                return ("");
            }
        }

        public string AcessaNivelMax(string campo)
        {
            try
            {

                SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");

                //String query = "select Nome from Users Where Login = ";
                String query = "select max (seq) from ordem Where ordem = ";
                // query += "Login = " + "login";
                query += String.Format("'{0}'", campo);

                return (db.ExecuteScalar(query));

            }
            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                return ("");
            }
        }


        public string DevolveCod1(string codigo, string campo, string campo1)
        {
            try
            {

                SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");

                //String query = "select Nome from Users Where Login = ";
                String query = String.Format("select {0} from ordem Where ordem = ", codigo);
                // query += "Login = " + "login";
                query += String.Format("'{0}'", campo);
                query += " AND seq = ";
                query += String.Format("'{0}'", campo1);

                return (db.ExecuteScalar(query));

            }
            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                return ("");
            }
        }

        public string DevolveCodDesenho(string codigo, string campo)
        {
            try
            {

                SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");

                //String query = "select Nome from Users Where Login = ";
                String query = String.Format("select {0} from produtos Where coddesenho = ", codigo);
                // query += "Login = " + "login";
                query += String.Format("'{0}'", campo);

                return (db.ExecuteScalar(query));

            }
            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                return ("");
            }
        }

        public void InsProdutoSubst(Dictionary<String, String> data)
        {

            SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");
            try
            {
                db.Insert("replacement", data);
            }
            catch (Exception crap)
            {
                //MessageBox.Show(crap.Message);
                String error = "The following error has occurred:\n\n";
                error += crap.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                //return ("");
            }

        }



        public void upProdIdSubst(Dictionary<String, String> data, int Id)
        {

            SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");
            try
            {
                db.Update("replacement", data, String.Format("repId = {0}", Id)); 
            }
            catch (Exception crap)
            {
                //MessageBox.Show(crap.Message);
                String error = "The following error has occurred:\n\n";
                error += crap.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                //return ("");
            }
        }



        public void InsOrdem(Dictionary<String, String> data)
        {

            SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");
            try
            {
                db.Insert("ordem", data);
            }
            catch (Exception crap)
            {
                //MessageBox.Show(crap.Message);
                String error = "The following error has occurred:\n\n";
                error += crap.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                //return ("");
            }

        }

        public void InsDesdobra(Dictionary<String, String> data)
        {

            SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");
            try
            {
                db.Insert("desmembra", data);
            }
            catch (Exception crap)
            {
                //MessageBox.Show(crap.Message);
                String error = "The following error has occurred:\n\n";
                error += crap.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                //return ("");
            }

        }


        public void InsProd(Dictionary<String, String> data)
        {

            SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");
            try
            {
                db.Insert("prod", data);
            }
            catch (Exception crap)
            {
                //MessageBox.Show(crap.Message);
                String error = "The following error has occurred:\n\n";
                error += crap.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                //return ("");
            }

        }

        public void InsOrdemValor(int valor)
        {

            SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");
            try
            {
                db.InsertNumero("ordem", valor);
            }
            catch (Exception crap)
            {
                //MessageBox.Show(crap.Message);
                String error = "The following error has occurred:\n\n";
                error += crap.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                //return ("");
            }

        }


        public void upProdId(Dictionary<String, String> data, int Id)
        {

            SQLiteDatabase db = new SQLiteDatabase(@"C:\dados\otis.s3db");
            try
            {
                db.Update("prod", data, String.Format("seq_id = {0}", Id)); // this.RecipeID
            }
            catch (Exception crap)
            {
                //MessageBox.Show(crap.Message);
                String error = "The following error has occurred:\n\n";
                error += crap.Message.ToString() + "\n\n";
                MessageBox.Show(error);
                //this.Close();
                //return ("");
            }
        }

       
    }

 }

