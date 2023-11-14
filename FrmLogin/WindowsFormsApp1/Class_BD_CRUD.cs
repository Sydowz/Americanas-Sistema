﻿using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace WindowsFormsApp1
{

    internal class Class_BD_CRUD
    {   //"server=localhost;port=3307;User Id=root;database=projeto_registro_sql;password=usbw"
        //"server=containers-us-west-156.railway.app;port=6863;User Id=root;database=railway;password=uoNk5WCFgcxKJ1AjalxJ"
        private MySqlConnection conn = new MySqlConnection("server=containers-us-west-156.railway.app;port=6863;User Id=root;database=railway;password=uoNk5WCFgcxKJ1AjalxJ");
        private int id_hora, id_data, id_americanas;
        // váriaveis a baixo para busca dados
        string retorna_nf, retorna_situacao, retorna_cpf_titular, retorna_cpf_entregador, nome_titular, email_titular, telefone_titular, retorna_nome_entregador, retorna_chegada_data, retorna_retirada_data, retorna_chegada_hora, retorna_retirada_hora;
        int retorna_id_data, retorna_id_hora;

        public Class_BD_CRUD()
        {
            /* campo vazio, abertura do BD sera manual
             MessageBox.Show("Banco de Dados public inicializado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);*/
        }
        public void setBD_Open()
        {
            //abre o BD
            conn.Open();
            //MessageBox.Show("Banco de Dados open.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        public void setBD_Close()
        {
            //fecha o BD
            conn.Close();
            //MessageBox.Show("BD fechado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public void setInputBd_titular(string cpf_titular, string nome, string email, string telefone)
        {
            MySqlCommand objcmd_titular = new MySqlCommand("insert into titular (cpf_titular, nome, email, telefone) values (?, ?, ?, ?)", conn);
            // parametros para o sql titular
            objcmd_titular.Parameters.Add("@cpf_titular", MySqlDbType.VarChar, 15).Value = cpf_titular;
            objcmd_titular.Parameters.Add("@nome", MySqlDbType.VarChar, 75).Value = nome;
            objcmd_titular.Parameters.Add("@email", MySqlDbType.VarChar, 30).Value = email;
            objcmd_titular.Parameters.Add("@telefone", MySqlDbType.VarChar, 15).Value = telefone;
            // executando querys            
            objcmd_titular.ExecuteNonQuery();
            MessageBox.Show("envio de dados titular ok.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public void setInputBd_data(string chegada_data, string retirada_data)
        {
            MySqlCommand objcmd_data = new MySqlCommand("insert into tbl_data (id_data, chegada_data, retirada_data) values (NULL, ?, ?)", conn);
            // parametros para o sql data
            objcmd_data.Parameters.Add("@chegada_data", MySqlDbType.Date).Value = chegada_data;
            objcmd_data.Parameters.Add("@retirada_data", MySqlDbType.Date).Value = retirada_data;
            // executando query                       
            objcmd_data.ExecuteNonQuery();
            MessageBox.Show("envio de dados data ok.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            MySqlCommand last_id_data = new MySqlCommand("SELECT LAST_INSERT_ID();", conn);

            id_data = Convert.ToInt32(last_id_data.ExecuteScalar());
            MessageBox.Show("valor id data = " + id_data.ToString(), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public void setInputBd_hora(string chegada_hora, string retirada_hora)
        {
            MySqlCommand objcmd_hora = new MySqlCommand("insert into hora (id_hora, chegada_hora, retirada_hora) values (NULL, ?, ?)", conn);
            // parametros para o sql hora
            objcmd_hora.Parameters.Add("@chegada_hora", MySqlDbType.Time).Value = chegada_hora;
            objcmd_hora.Parameters.Add("@retirada_hora", MySqlDbType.Time).Value = retirada_hora;
            // executando query                       
            objcmd_hora.ExecuteNonQuery();
            MessageBox.Show("envio de dados hora ok.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            MySqlCommand last_id_hora = new MySqlCommand("SELECT LAST_INSERT_ID();", conn);


            id_hora = Convert.ToInt32(last_id_hora.ExecuteScalar());
            MessageBox.Show("valor id hora = " + id_hora.ToString(), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public void setInputBd_americanas(string cep_americanas, string rua_americanas, string bairro_americanas, string numero_americanas)
        {
            MySqlCommand verificaExistenciaCommand = new MySqlCommand("SELECT id_americanas FROM americanas WHERE cep_americanas = @cep_endereco AND rua_americanas = @rua_endereco AND bairro_americanas = @bairro_endereco AND numero_americanas = @numero_endereco;", conn);

            verificaExistenciaCommand.Parameters.AddWithValue("@cep_endereco", cep_americanas);
            verificaExistenciaCommand.Parameters.AddWithValue("@rua_endereco", rua_americanas);
            verificaExistenciaCommand.Parameters.AddWithValue("@bairro_endereco", bairro_americanas);
            verificaExistenciaCommand.Parameters.AddWithValue("@numero_endereco", numero_americanas);

            object resultado = verificaExistenciaCommand.ExecuteScalar();

            if (resultado == null)
            {
                MySqlCommand objcmd_americanas = new MySqlCommand("INSERT INTO americanas (id_americanas, cep_americanas, rua_americanas, bairro_americanas, numero_americanas) VALUES (NULL, ?, ?, ?, ?)", conn);
                // parametros para o sql pacote
                objcmd_americanas.Parameters.Add("@cep_endereco", MySqlDbType.VarChar, 15).Value = cep_americanas;
                objcmd_americanas.Parameters.Add("@rua_endereco", MySqlDbType.VarChar, 45).Value = rua_americanas;
                objcmd_americanas.Parameters.Add("@bairro_endereco", MySqlDbType.VarChar, 15).Value = bairro_americanas;
                objcmd_americanas.Parameters.Add("@numero_endereco", MySqlDbType.VarChar).Value = numero_americanas;
                // executando query                       
                objcmd_americanas.ExecuteNonQuery();
                MessageBox.Show("envio de dados americanas ok.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                objcmd_americanas = new MySqlCommand("SELECT LAST_INSERT_ID();", conn);
                id_americanas = Convert.ToInt32(objcmd_americanas.ExecuteScalar());
            }
            else
            {
                id_americanas = Convert.ToInt32(resultado);
                MessageBox.Show("ID encontrado: " + id_americanas, "Loja encontrada");
            }
        }

        public void setInputBd_funcionario(string email_funcionario, string cpf_funcionario, string nome_funcionario, string telefone_funcionario, string senha_funcionario)
        {
            MySqlCommand objcmd_funcionario = new MySqlCommand("INSERT INTO funcionario (email_americanas_funcionario, cpf_funcionario, nome_funcionario, telefone_funcionario, senha_funcionario, id_americanas) VALUES (?, ?, ?, ?, ?, ?)", conn);
            // parametros para o sql pacote
            objcmd_funcionario.Parameters.Add("@email_americanas_funcionario", MySqlDbType.VarChar, 30).Value = email_funcionario;
            objcmd_funcionario.Parameters.Add("@cpf_funcionario", MySqlDbType.VarChar, 15).Value = cpf_funcionario;
            objcmd_funcionario.Parameters.Add("@nome_funcionario", MySqlDbType.VarChar, 75).Value = nome_funcionario;
            objcmd_funcionario.Parameters.Add("@telefone_funcionario", MySqlDbType.VarChar, 15).Value = telefone_funcionario;
            objcmd_funcionario.Parameters.Add("@senha_funcionario", MySqlDbType.VarChar, 20).Value = senha_funcionario;
            objcmd_funcionario.Parameters.Add("@id_americanas", MySqlDbType.Int32).Value = id_americanas;
            // executando query                       
            objcmd_funcionario.ExecuteNonQuery();
            MessageBox.Show("envio de dados funcionario ok.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public void setInputBd_entregador(string cpf_entregador, string nome_entregador)
        {
            MySqlCommand objcmd_entregador = new MySqlCommand("INSERT INTO entregador (cpf_entregador, nome_entregador) VALUES (?, ?)", conn);
            // parametros para o sql entregador

            objcmd_entregador.Parameters.Add("@cpf_entregador", MySqlDbType.VarChar, 15).Value = cpf_entregador;
            objcmd_entregador.Parameters.Add("@nome_entregador", MySqlDbType.VarChar, 75).Value = nome_entregador;
            // executando query 
            objcmd_entregador.ExecuteNonQuery();
            MessageBox.Show("envio de dados entregador ok.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }


        public void setInputBd_pacote(string nota_fiscal, string situacao_pacote, /*string email_americanas_funcionario,*/ string cpf_titular, string cpf_entregador)
        {
            MySqlCommand objcmd_pacote = new MySqlCommand("INSERT INTO pacote (nota_fiscal_pacote, situacao_pacote, cpf_titular, cpf_entregador, id_data, id_hora) VALUES (?, ?, ?, ?, ?, ?, ?); SELECT LAST_INSERT_ID();", conn);

            objcmd_pacote.Parameters.Add("@nota_fiscal_pacote", MySqlDbType.VarChar, 45).Value = nota_fiscal;
            objcmd_pacote.Parameters.Add("@situacao_pacote", MySqlDbType.VarChar, 20).Value = situacao_pacote;
            //objcmd_pacote.Parameters.Add("@email_americanas_funcionario", MySqlDbType.VarChar, 255).Value = email_americanas_funcionario;
            objcmd_pacote.Parameters.Add("@cpf_titular", MySqlDbType.VarChar, 15).Value = cpf_titular;
            objcmd_pacote.Parameters.Add("@cpf_entregador", MySqlDbType.VarChar, 15).Value = cpf_entregador;
            objcmd_pacote.Parameters.Add("@id_data", MySqlDbType.Int32).Value = id_data;
            objcmd_pacote.Parameters.Add("@id_hora", MySqlDbType.Int32).Value = id_hora;

            // Executar a consulta de inserção e recuperar o ID gerado automaticamente
            objcmd_pacote.ExecuteNonQuery();
            MessageBox.Show("Envio de dados pacote ok.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public bool setReadBd_login(string usuario, string senha)
        {
            // Construa a consulta SQL para verificar as credenciais do usuário
            string query = "SELECT COUNT(*) FROM funcionario WHERE email_americanas_funcionario = @usuario AND senha_funcionario = @senha";

            MySqlCommand objcmd_login = new MySqlCommand(query, conn);
            objcmd_login.Parameters.Add("@usuario", MySqlDbType.VarChar, 255).Value = usuario;
            objcmd_login.Parameters.Add("@senha", MySqlDbType.VarChar, 32).Value = senha; // Certifique-se de usar o mesmo tipo de dados que está armazenado no banco de dados para senhas.

            // Execute a consulta para verificar as credenciais
            int result = Convert.ToInt32(objcmd_login.ExecuteScalar());

            if (result > 0)
            {
                MessageBox.Show("Login bem-sucedido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            else
            {
                MessageBox.Show("Credenciais inválidas. Tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public User setReadBd_funcionario(string usuario, string senha)
        {
            try
            {
                string query = "SELECT * FROM funcionario WHERE email_americanas_funcionario = @usuario AND senha_funcionario = @senha";

                MySqlCommand objcmd_user = new MySqlCommand(query, conn);
                objcmd_user.Parameters.Add("@usuario", MySqlDbType.VarChar, 255).Value = usuario;
                objcmd_user.Parameters.Add("@senha", MySqlDbType.VarChar, 32).Value = senha;

                using (MySqlDataReader reader = objcmd_user.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string email, cpf, nome, telefone;
                        int id_americanas;

                        email = reader["email_americanas_funcionario"].ToString();
                        cpf = reader["cpf_funcionario"].ToString();
                        nome = reader["nome_funcionario"].ToString();
                        telefone = reader["telefone_funcionario"].ToString();
                        id_americanas = Convert.ToInt32(reader["id_americanas"]);

                        User funcionario = new User(nome, email, senha, cpf, id_americanas, telefone);
                        return funcionario;
                    }
                }

                // Se não encontrou um funcionário com as credenciais fornecidas, você pode lançar uma exceção ou retornar um valor padrão.
                // Exemplo lançando uma exceção:
                throw new InvalidOperationException("Funcionário não encontrado com as credenciais fornecidas.");
            }
            catch (Exception ex)
            {
                // Lidar com exceções de banco de dados ou conexão aqui
                // Pode ser útil logar o erro ou tomar outras ações apropriadas.
                Console.WriteLine("Erro: " + ex.Message);
                return null;
            }
        }

        // A BAIXO ESTÃO OS MÉTODOS PARA BUSCAR OS DADOS DO BD**************************************************************************************
        public void setRead_pacote_cpf(string cpf)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT nota_fiscal_pacote, situacao_pacote, cpf_titular, cpf_entregador, id_data, id_hora FROM pacote WHERE cpf_titular = ?", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@cpf_titular", MySqlDbType.VarChar, 15).Value = cpf;

            cmd.CommandType = CommandType.Text;

            //recebe conteudo do banco
            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            //variaveis globais que recebem dados e as chaves estrangeiras da tabela pacote
            retorna_nf = dr.GetString(0);
            retorna_situacao = dr.GetString(1);
            retorna_cpf_titular = dr.GetString(2);
            retorna_cpf_entregador = dr.GetString(3);
            retorna_id_data = dr.GetInt32(4);
            retorna_id_hora = dr.GetInt32(5);
        }
        public void setRead_pacote_nf(string nota_fiscal)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT nota_fiscal_pacote, situacao_pacote, cpf_titular, cpf_entregador, id_data, id_hora FROM pacote WHERE nota_fiscal_pacote = ?", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@nota_fiscal_pacote", MySqlDbType.VarChar, 45).Value = nota_fiscal;

            cmd.CommandType = CommandType.Text;

            //recebe conteudo do banco
            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            //variaveis globais que recebem dados e as chaves estrangeiras da tabela pacote
            retorna_nf = dr.GetString(0);
            retorna_situacao = dr.GetString(1);
            retorna_cpf_titular = dr.GetString(2);
            retorna_cpf_entregador = dr.GetString(3);
            retorna_id_data = dr.GetInt32(4);
            retorna_id_hora = dr.GetInt32(5);
        }


        public void setRead_titular_cpf()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT nome, email, telefone  FROM titular WHERE cpf_titular = ?", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@cpf_titular", MySqlDbType.VarChar, 15).Value = retorna_cpf_titular;

            cmd.CommandType = CommandType.Text;

            //recebe conteudo do banco
            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            //nome_titular, email_titular, telefone_titular;

            nome_titular = dr.GetString(0);
            email_titular = dr.GetString(1);
            telefone_titular = dr.GetString(2);
        }

        public void setRead_entregador()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT nome_entregador FROM entregador WHERE cpf_entregador = ?", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@cpf_entregador", MySqlDbType.VarChar, 15).Value = retorna_cpf_entregador;

            cmd.CommandType = CommandType.Text;

            //recebe conteudo do banco
            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            retorna_nome_entregador = dr.GetString(0);
        }

        public void setRead_data()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT chegada_data, retirada_data  FROM tbl_data WHERE id_data = ?", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@id_data", MySqlDbType.Int32).Value = retorna_id_data;

            cmd.CommandType = CommandType.Text;

            //recebe conteudo do banco
            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            retorna_chegada_data = dr.GetDateTime(0).ToString("dd/MM/yyyy");
            retorna_retirada_data = dr.GetDateTime(1).ToString("dd/MM/yyyy");            
        }

        public void setRead_hora()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT chegada_hora, retirada_hora  FROM hora WHERE id_hora = ?", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@id_hora", MySqlDbType.Int32).Value = retorna_id_hora;

            cmd.CommandType = CommandType.Text;

            //recebe conteudo do banco
            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            retorna_chegada_hora = dr.GetTimeSpan(0).ToString(@"hh\:mm");
            retorna_retirada_hora = dr.GetTimeSpan(1).ToString(@"hh\:mm");
        }

        public string setRead_Presentes()
        {
            string pacotesPresentes = "0";

            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(nota_fiscal_pacote) FROM pacote WHERE pacote.situacao_pacote = \"Presente\"", conn);
            cmd.Parameters.Clear();
            MySqlDataReader select = cmd.ExecuteReader();

            if (select.HasRows)
            {
                select.Read();
                pacotesPresentes = Convert.ToString(select[0]);
            }

            select.Close();
            return pacotesPresentes;
        }

        public string setRead_Retirados()
        {
            string pacotesRetirados = "0";

            DateTime data_atual = DateTime.Now.Date;

            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(nota_fiscal_pacote) FROM pacote INNER JOIN tbl_data ON pacote.id_data = tbl_data.id_data WHERE pacote.situacao_pacote = \"Retirado\" AND tbl_data.retirada_data = @dataAtual", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@dataAtual", MySqlDbType.Date).Value = data_atual;

            MySqlDataReader select = cmd.ExecuteReader();

            if (select.HasRows)
            {
                select.Read();
                pacotesRetirados = Convert.ToString(select[0]);
            }

            select.Close();
            return pacotesRetirados;
        }
        
        public string setRead_Todos()
        {
            string pacotesRetirados = "0";

            DateTime data_atual = DateTime.Now.Date;

            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(nota_fiscal_pacote) FROM pacote INNER JOIN tbl_data ON pacote.id_data = tbl_data.id_data WHERE tbl_data.chegada_data = @dataAtual OR tbl_data.retirada_data = @dataAtual", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@dataAtual", MySqlDbType.Date).Value = data_atual;
            MySqlDataReader select = cmd.ExecuteReader();

            if (select.HasRows)
            {
                select.Read();
                pacotesRetirados = Convert.ToString(select[0]);
            }

            select.Close();
            return pacotesRetirados;
        }



        //GET PARA RECUPERAR TODOS OS DADOS SEPARADAMENTE DO BANCO DE DADOS MENOS FUNCIONARIO*******************************************************
        public string getRetorna_nf() { return retorna_nf; }
        public string getRetorna_situacao() { return retorna_situacao; }
        public string getRetorna_cpf_titular() { return retorna_cpf_titular; }
        public string getRetorna_cpf_entregador() { return retorna_cpf_entregador; }
        public string getNome_titular() { return nome_titular; }
        public string getEmail_titular() { return email_titular; }
        public string getTelefone_titular() { return telefone_titular; }
        public string getRetorna_nome_entregador() { return retorna_nome_entregador; }
        public string getRetorna_chegada_data() { return retorna_chegada_data; }
        public string getRetorna_retirada_data() { return retorna_retirada_data; }
        public string getRetorna_chegada_hora() { return retorna_chegada_hora; }
        public string getRetorna_retirada_hora() { return retorna_retirada_hora; }
        public int getRetorna_id_data() { return retorna_id_data; }
        public int getRetorna_id_hora() { return retorna_id_hora; }

        // MÉTODOS ENVIO DE DADOS EDITADOS PELO USUÁRIO********************************************************************

        public void setEdit_pacote(string nota_fiscal, string situacao, string cpf_titular, string cpf_entregador)
        {
            MySqlConnection conn = new MySqlConnection("server=containers-us-west-156.railway.app;port=6863;User Id=root;database=railway;password=uoNk5WCFgcxKJ1AjalxJ");

            MySqlCommand objEdit = new MySqlCommand("UPDATE pacote SET nota_fiscal_pacote = ?, situacao_pacote = ?, cpf_titular = ?, cpf_entregador = ? WHERE nota_fiscal_pacote = ?", conn);
            objEdit.Parameters.Clear();
            objEdit.Parameters.Add("@nota_fiscal_pacote", MySqlDbType.VarChar, 45).Value = nota_fiscal;
            objEdit.Parameters.Add("@situacao_pacote", MySqlDbType.VarChar, 20).Value = situacao;
            objEdit.Parameters.Add("@cpf_titular", MySqlDbType.VarChar, 15).Value = cpf_titular;
            objEdit.Parameters.Add("@cpf_entregador", MySqlDbType.VarChar, 15).Value = cpf_entregador;
            objEdit.Parameters.Add("@nota_fiscal_pacote", MySqlDbType.VarChar, 45).Value = retorna_nf; // nota fiscal antiga

            objEdit.CommandType = CommandType.Text;
            objEdit.ExecuteNonQuery();
        }

        public void setEdit_titular(string cpf, string nome, string email, string telefone)
        {
            MySqlConnection conn = new MySqlConnection("server=containers-us-west-156.railway.app;port=6863;User Id=root;database=railway;password=uoNk5WCFgcxKJ1AjalxJ");

            MySqlCommand objEdit = new MySqlCommand("UPDATE titular SET cpf_titular = ?, nome = ?, email = ?, telefone = ? WHERE cpf_titular = ?", conn);
            objEdit.Parameters.Clear();
            objEdit.Parameters.Add("@cpf_titular", MySqlDbType.VarChar, 15).Value = cpf;
            objEdit.Parameters.Add("@nome", MySqlDbType.VarChar, 75).Value = nome;
            objEdit.Parameters.Add("@email", MySqlDbType.VarChar, 30).Value = email;
            objEdit.Parameters.Add("@telefone", MySqlDbType.VarChar, 15).Value = telefone;
            objEdit.Parameters.Add("@cpf_titular", MySqlDbType.VarChar, 15).Value = retorna_cpf_titular;

            objEdit.CommandType = CommandType.Text;
            objEdit.ExecuteNonQuery();
        }

        public void setEdit_entregador(string cpf, string nome)
        {
            MySqlConnection conn = new MySqlConnection("server=containers-us-west-156.railway.app;port=6863;User Id=root;database=railway;password=uoNk5WCFgcxKJ1AjalxJ");

            MySqlCommand objEdit = new MySqlCommand("UPDATE entregador SET cpf_entregador = ?, nome_entregador = ? WHERE cpf_entregador = ?", conn);
            objEdit.Parameters.Clear();
            objEdit.Parameters.Add("@cpf_entregador", MySqlDbType.VarChar, 15).Value = cpf;
            objEdit.Parameters.Add("@nome_entregador", MySqlDbType.VarChar, 75).Value = nome;
            objEdit.Parameters.Add("@cpf_entregador", MySqlDbType.VarChar, 15).Value = retorna_cpf_entregador;


            objEdit.CommandType = CommandType.Text;
            objEdit.ExecuteNonQuery();
        }

        public void setEdit_data(string chegada, string retirada)
        {
            MySqlConnection conn = new MySqlConnection("server=containers-us-west-156.railway.app;port=6863;User Id=root;database=railway;password=uoNk5WCFgcxKJ1AjalxJ");

            MySqlCommand objEdit = new MySqlCommand("UPDATE tbl_data SET chegada_data = ?, retirada_data = ? WHERE id_data = ?", conn);
            objEdit.Parameters.Clear();
            objEdit.Parameters.Add("@chegada_data", MySqlDbType.VarChar, 10).Value = chegada;
            objEdit.Parameters.Add("@retirada_data", MySqlDbType.VarChar, 10).Value = retirada;
            objEdit.Parameters.Add("@id_data", MySqlDbType.Int32).Value = retorna_id_data;


            objEdit.CommandType = CommandType.Text;
            objEdit.ExecuteNonQuery();
        }

        public void setEdit_hora(string chegada, string retirada)
        {
            MySqlConnection conn = new MySqlConnection("server=containers-us-west-156.railway.app;port=6863;User Id=root;database=railway;password=uoNk5WCFgcxKJ1AjalxJ");

            MySqlCommand objEdit = new MySqlCommand("UPDATE hora SET chegada_hora = ?, retirada_hora = ? WHERE id_hora = ?", conn);
            objEdit.Parameters.Clear();
            objEdit.Parameters.Add("@chegada_hora", MySqlDbType.VarChar, 10).Value = chegada;
            objEdit.Parameters.Add("@retirada_hora", MySqlDbType.VarChar, 10).Value = retirada;
            objEdit.Parameters.Add("@id_hora", MySqlDbType.Int32).Value = retorna_id_hora;


            objEdit.CommandType = CommandType.Text;
            objEdit.ExecuteNonQuery();
        }

        // MÉTODOS A BAIXO PARA DELETAR DADOS DO BANCO DE DADOS***********************************************************************************************

        public void setDelet_pacote(string notaFiscal) 
        {
            MySqlConnection conn = new MySqlConnection("server=containers-us-west-156.railway.app;port=6863;User Id=root;database=railway;password=uoNk5WCFgcxKJ1AjalxJ");
            
            MySqlCommand cmdDelet = new MySqlCommand("DELET FROM pacote WHERE nota_fiscal_pacote = ?", conn);
            cmdDelet.Parameters.Clear();
            cmdDelet.Parameters.Add("@nota_fiscal_pacote", MySqlDbType.VarChar, 45).Value = notaFiscal;

            cmdDelet.CommandType = CommandType.Text;
            cmdDelet.ExecuteNonQuery();
            
        }

        public void setDelet_titular(string cpf) 
        {
            MySqlConnection conn = new MySqlConnection("server=containers-us-west-156.railway.app;port=6863;User Id=root;database=railway;password=uoNk5WCFgcxKJ1AjalxJ");

            MySqlCommand cmdDelet = new MySqlCommand("DELET FROM titular WHERE cpf_titular = ?", conn);
            cmdDelet.Parameters.Clear();
            cmdDelet.Parameters.Add("@cpf_titular", MySqlDbType.VarChar, 15).Value = cpf;

            cmdDelet.CommandType = CommandType.Text;
            cmdDelet.ExecuteNonQuery();
        }

        public void setDelet_entregador(string cpf_entregador) 
        {
            MySqlConnection conn = new MySqlConnection("server=containers-us-west-156.railway.app;port=6863;User Id=root;database=railway;password=uoNk5WCFgcxKJ1AjalxJ");

            MySqlCommand cmdDelet = new MySqlCommand("DELET FROM entregador WHERE cpf_entregador = ?", conn);
            cmdDelet.Parameters.Clear();
            cmdDelet.Parameters.Add("@cpf_entregador", MySqlDbType.VarChar, 15).Value = cpf_entregador;

            cmdDelet.CommandType = CommandType.Text;
            cmdDelet.ExecuteNonQuery();
        }

        public void setDelet_data(int id_data) 
        {
            MySqlConnection conn = new MySqlConnection("server=containers-us-west-156.railway.app;port=6863;User Id=root;database=railway;password=uoNk5WCFgcxKJ1AjalxJ");

            MySqlCommand cmdDelet = new MySqlCommand("DELET FROM tbl_data WHERE id_data = ?", conn);
            cmdDelet.Parameters.Clear();
            cmdDelet.Parameters.Add("@id_data", MySqlDbType.Int32).Value = id_data;

            cmdDelet.CommandType = CommandType.Text;
            cmdDelet.ExecuteNonQuery();
        }

        public void setDelet_hora(int id_hora) 
        {
            MySqlConnection conn = new MySqlConnection("server=containers-us-west-156.railway.app;port=6863;User Id=root;database=railway;password=uoNk5WCFgcxKJ1AjalxJ");

            MySqlCommand cmdDelet = new MySqlCommand("DELET FROM hora WHERE id_hora = ?", conn);
            cmdDelet.Parameters.Clear();
            cmdDelet.Parameters.Add("@id_hora", MySqlDbType.Int32).Value = id_hora;

            cmdDelet.CommandType = CommandType.Text;
            cmdDelet.ExecuteNonQuery();
        }
    }
}
