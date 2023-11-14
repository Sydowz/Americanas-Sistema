﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WindowsFormsApp1.RegistrosPac
{
    internal partial class Form5_Registro_Pac : Form
    {
        private User usuario;
        Class_CadastroPac cadastroPacote = new Class_CadastroPac();
        Class_BD_CRUD Bd = new Class_BD_CRUD();
        public Form5_Registro_Pac(User usuarioAtual)
        {
            usuario = usuarioAtual;
            InitializeComponent();
        }

        private void button_Cadastrar_Click(object sender, EventArgs e)
        {
            string funcionario = comboBox1.Text;
            string notaFiscal = textBox_NotaFiscal.Text;
            string titular = textBox_Titular.Text;
            string CPF = maskedTextBox_CPF.Text;
            string situacao = maskedTextBoxSituacao.Text;
            string email = maskedTextBox_email.Text;
            string telefone = maskedTextBox_telefone.Text;
            string cpf_entregador = txtbox_cpf_entregador.Text;
            string nome_entregador = txtbox_nome_entregador.Text;


            if (funcionario != "" && notaFiscal != "" && titular != "" & CPF != "" && situacao != ""
                && email != "" && telefone != "" && cpf_entregador != "" && nome_entregador != "")
            {

                bool dadosOk = cadastroPacote.setValid_dados(funcionario, titular, situacao, email, notaFiscal, telefone, CPF, cpf_entregador, nome_entregador);

                if (dadosOk == true)
                {
                    //DÚVIDA EM QUESTÃO DE BOAS PRÁTICAS?
                    string dadosValidos_funcionario = cadastroPacote.getCad_Funcionario();//avaliar se vai integrar realmente no cadastro do pacote
                    string dadosValidos_nomeTitular = cadastroPacote.getCad_Titular();
                    string dadosValidos_situacao = cadastroPacote.getCad_Situacao();
                    string dadosValidos_email = cadastroPacote.getCad_Email();
                    string dadosValidos_notaFiscal = cadastroPacote.getCad_NotaFiscal();
                    string dadosValidos_telefone = cadastroPacote.getCad_Telefone();
                    string dadosValidos_CPF = cadastroPacote.getCad_Cpf();
                    string dadosValidos_cpf_entregador = cadastroPacote.getCpf_entregador();
                    string dadosValidos_nome_entregador = cadastroPacote.getNome_entregador();


                    string hour = (DateTime.Now).ToString("HH:mm:ss");
                    string date = DateTime.Now.Date.ToString("yyyy-MM-dd");

                    try
                    {
                        Bd.setBD_Open();
                        Bd.setInputBd_hora(hour);
                        Bd.setInputBd_data(date);
                        Bd.setInputBd_titular(dadosValidos_CPF, dadosValidos_nomeTitular, dadosValidos_email, dadosValidos_telefone);
                        Bd.setInputBd_entregador(dadosValidos_cpf_entregador, dadosValidos_nome_entregador);
                        Bd.setInputBd_pacote(dadosValidos_notaFiscal, dadosValidos_situacao, dadosValidos_funcionario, dadosValidos_CPF, dadosValidos_cpf_entregador);
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show("Erro ao conectar dados no banco de dados.\n\n" + erro, "Erro de conexão");
                    }
                    finally
                    {
                        Bd.setBD_Close();
                    }
                }
                else { return; }

            }
            else
            {
                MessageBox.Show("Todos os campos de cadastro precisam ser preenchidos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }


        }

        private void button_deletar_Click(object sender, EventArgs e)
        {
            string funcionario = comboBox1.Text;
            string notaFiscal = textBox_NotaFiscal.Text;
            string titular = textBox_Titular.Text;
            string CPF = (maskedTextBox_CPF.Text).Replace("-", "").Replace(".", "");
            string situacao = maskedTextBoxSituacao.Text;
            string email = maskedTextBox_email.Text;
            string telefone = maskedTextBox_telefone.Text.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            string cpf_entregador = txtbox_cpf_entregador.Text;
            string nome_entregador = txtbox_nome_entregador.Text;
            int id_data = Bd.getRetorna_id_data();
            int id_hora = Bd.getRetorna_id_hora();



            if (funcionario != "" && notaFiscal != "" && titular != "" & CPF != "" && situacao != ""
                && email != "" && telefone != "" && cpf_entregador != "" && nome_entregador != "")
            {
                try
                {
                    Bd.setBD_Open();
                    Bd.setDelet_data(id_data);
                    Bd.setDelet_hora(id_hora);
                    Bd.setDelet_entregador(cpf_entregador);
                    Bd.setDelet_titular(CPF);
                    Bd.setDelet_pacote(notaFiscal);
                    Bd.setBD_Close();
                }
                catch (Exception erro)
                {
                    MessageBox.Show("Erro ao deletar dados do pacote no banco de dados.\n\n" + erro, "Erro de conexão");
                }               

            }
            else
            {
                MessageBox.Show("Todos os campos de cadastro precisam estar corretos para localizar e deletar o pacote.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void button_Buscar_Click(object sender, EventArgs e)
        {
            string CPF, nota_fiscal;
            bool dadosOk, rdb_cpf_checked = rdb_Titular_cpf.Checked, rdb_nf_checked = rdb_NotaFiscal.Checked;


            if (rdb_cpf_checked)
            {
                CPF = rdb_Titular_cpf.Text;
                if (CPF != "")
                {
                    dadosOk = cadastroPacote.setValid_cpf_buscar(CPF);
                    if (dadosOk)
                    {
                        Bd.setBD_Open();
                        Bd.setRead_pacote_cpf(CPF);
                        Bd.setRead_titular_cpf();
                        Bd.setRead_entregador();
                        Bd.setRead_data();
                        Bd.setRead_hora();
                    }
                    else { MessageBox.Show("CPF Titular if dadosok.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); }                    
                }
                else
                { MessageBox.Show("CPF Titular vazio em opções do buscar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); }
            }
            else if (rdb_nf_checked)
            {
                nota_fiscal = rdb_NotaFiscal.Text;
                if (nota_fiscal != "")
                {
                    dadosOk = cadastroPacote.setValid_nf_buscar(nota_fiscal);
                    if (dadosOk)
                    {
                        Bd.setBD_Open();
                        Bd.setRead_pacote_nf(nota_fiscal);
                        Bd.setRead_titular_cpf();
                        Bd.setRead_entregador();
                        Bd.setRead_data();
                        Bd.setRead_hora();

                    }
                    else { MessageBox.Show("NF  if dadosok.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); }                    
                }
                else
                { MessageBox.Show("Nota Fiscal vazio em opções do buscar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); }
            }
            else { MessageBox.Show("Selecione uma das opções do buscar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); return; }

            //recebendo dados para enviar pros text box nos form registro pacote
            textBox_NotaFiscal.Text = Bd.getRetorna_nf();
            maskedTextBoxSituacao.Text = Bd.getRetorna_situacao();
            maskedTextBox_CPF.Text = Bd.getRetorna_cpf_titular();
            txtbox_cpf_entregador.Text = Bd.getRetorna_cpf_entregador();
            textBox_Titular.Text = Bd.getNome_titular();
            maskedTextBox_email.Text = Bd.getEmail_titular();
            txtbox_nome_entregador.Text = Bd.getRetorna_nome_entregador();
            Bd.setBD_Close();

        }

        private void button_Editar_Click(object sender, EventArgs e)
        {
            string funcionario = comboBox1.Text;
            string notaFiscal = textBox_NotaFiscal.Text;
            string titular = textBox_Titular.Text;
            string CPF = maskedTextBox_CPF.Text;
            string situacao = maskedTextBoxSituacao.Text;
            string email = maskedTextBox_email.Text;
            string telefone = maskedTextBox_telefone.Text;
            string cpf_entregador = txtbox_cpf_entregador.Text;
            string nome_entregador = txtbox_nome_entregador.Text;


            if (funcionario != "" && notaFiscal != "" && titular != "" & CPF != "" && situacao != ""
                && email != "" && telefone != "" && cpf_entregador != "" && nome_entregador != "")
            {

                bool dadosOk = cadastroPacote.setValid_dados(funcionario, titular, situacao, email, notaFiscal, telefone, CPF, cpf_entregador, nome_entregador);

                if (dadosOk == true)
                {
                    string dadosValidos_funcionario = cadastroPacote.getCad_Funcionario();//avaliar se vai integrar realmente no cadastro do pacote
                    string dadosValidos_nomeTitular = cadastroPacote.getCad_Titular();
                    string dadosValidos_situacao = cadastroPacote.getCad_Situacao();
                    string dadosValidos_email = cadastroPacote.getCad_Email();
                    string dadosValidos_notaFiscal = cadastroPacote.getCad_NotaFiscal();
                    string dadosValidos_telefone = cadastroPacote.getCad_Telefone();
                    string dadosValidos_CPF = cadastroPacote.getCad_Cpf();
                    string dadosValidos_cpf_entregador = cadastroPacote.getCpf_entregador();
                    string dadosValidos_nome_entregador = cadastroPacote.getNome_entregador();

                    string hour = (DateTime.Now).ToString("HH:mm:ss");
                    string date = DateTime.Now.Date.ToString("yyyy-MM-dd");

                    try
                    {
                        Bd.setBD_Open();
                        Bd.setInputBd_hora(hour);
                        Bd.setInputBd_data(date);
                        Bd.setInputBd_titular(dadosValidos_CPF, dadosValidos_nomeTitular, dadosValidos_email, dadosValidos_telefone);
                        Bd.setInputBd_entregador(dadosValidos_cpf_entregador, dadosValidos_nome_entregador);
                        Bd.setInputBd_pacote(dadosValidos_notaFiscal, dadosValidos_situacao, dadosValidos_funcionario, dadosValidos_CPF, dadosValidos_cpf_entregador);
                        Bd.setBD_Close();

                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show("Erro ao editar dados no banco de dados.\n\n" + erro, "Erro de conexão");
                    }
                }
                else { return; }

            }
            else
            {
                MessageBox.Show("Todos os campos de cadastro precisam ser preenchidos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void Form5_Registro_Pac_Load(object sender, EventArgs e)
        {
            int id = int.Parse(usuario.GetUserData(6));
            Bd.setBD_Open();
            List<string> emails = Bd.setRead_email_funcionarios(id);
            Bd.setBD_Close();
            
            comboBox1.Items.Clear();
            
            foreach (string email in emails)
            {
                comboBox1.Items.Add(email);
            }
        }
    }
}
