using System;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Sim.Services.ReceitaFederal
{
    internal enum Coluna
    {

        RazaoSocial,
        NomeFantasia,
        AtividadeEconomicaPrimaria,
        AtividadesEconomicasSecundarias, // AGORA BUSCA TODAS AS ATIVIDADES SECUNDARIAS E NAO SOMENTE UMA

        NumeroDaInscricao,
        MatrizFilial,
        NaturezaJuridica,

        SituacaoCadastral,
        DataSituacaoCadastral,
        MotivoSituacaoCadastral,

        Endereco,
        Numero,
        Bairro,
        Cidade,
        UF,
        Complemento,
        CEP,

        // PROPRIEDADES ADICIONADAS
        Telefone,
        Email,
        DataAbertura,
        EnteFederativoResponsavel,
        SituacaoEspecial,
        DataSituacaoEspecial,
        Cnae

    }

    public class CNPJ
    {
        static private readonly CookieContainer _cookies = new CookieContainer();
        //static private string urlBaseReceitaFederal = "http://www.receita.fazenda.gov.br/pessoajuridica/cnpj/cnpjreva/";
        static private string urlBaseReceitaFederal = "http://servicos.receita.fazenda.gov.br/Servicos/cnpjreva/";
        static private string paginaValidacao = "valida.asp";
        //static private string paginaPrincipal = "cnpjreva_solicitacao2.asp";
        static private string paginaPrincipal = "Cnpjreva_Solicitacao_CS.asp";
        static private string paginaCaptcha = "captcha/gerarCaptcha.asp";
        static private string paginaHTML = "";
        //metodo para capturar a imagem do site
        static public PessoJuridica Empresa = new PessoJuridica();
        static public string Mensagem = "";

        // PARAMETRO RECEBE O PICTUREBOX INFORMADO NO FORMULARIO. O BITMAP É CRIADO NO PROPRIO MÉTODO
        // RETORNA BOLEANO IGUAL METODO CONSULTA PARA FICAR PADRAO
        static public byte[] Captcha()
        {
            string htmlResult = ""; // PARA TER UM RETORNO VAZIO CASO OCORRA ERRO
            using (var wc = new CookieAwareWeb())
            {
                try
                {
                    wc.SetCookieContainer(_cookies);
                    wc.Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; Synapse)";
                    wc.Headers[HttpRequestHeader.KeepAlive] = "300";
                    htmlResult = wc.DownloadString(urlBaseReceitaFederal + paginaPrincipal); // COLOQUEI TRATAMENTO DE ERRO, O SERVIÇO DA RECEITA FORA DO AR GERA ERRO NESSA LINHA
                }
                catch (Exception)
                {
                    Mensagem = "Não foi possível carregar a imagem de validação.\nServiço da Receita Federal fora do ar ou bloqueado. Tente novamente mais tarde"; // RETORNA MENSAGEM IGUAL O METODO CONSULTA
                    return null;
                }
            }
            if (htmlResult.Length > 0)
            {
                var wc2 = new CookieAwareWeb();
                wc2.SetCookieContainer(_cookies);
                wc2.Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; Synapse)";
                wc2.Headers[HttpRequestHeader.KeepAlive] = "300";
                byte[] data = wc2.DownloadData(urlBaseReceitaFederal + paginaCaptcha);
                //Bitmap bit = new Bitmap(new MemoryStream(data));
                if (data != null)
                {
                    //picLetras = bit;
                    return data;
                }
                else
                {
                    Mensagem = "Não foi possível carregar a imagem de validação. Tente novamente mais tarde"; // RETORNA MENSAGEM IGUAL O METODO CONSULTA
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        // retorna true se conseguiu efetuar a consulta 
        // os dados da consulta vão ser armazenados na variavel paginaHTML
        static public bool Consultar(string aCNPJ, string aCaptcha)
        {
            aCNPJ = aCNPJ.Trim(); // EVITAR QUE QUALQUER ESPAÇO ADICIONAL ATRAPALHE
            aCaptcha = aCaptcha.Trim();

            // VALIDACAO MÍNIMA - EVITAR CONSULTA DESNECESSÁRIA AO SITE
            if (ValidaCampos(aCNPJ, aCaptcha) == false)
                return false;

            bool retornoConsulta = true;
            paginaHTML = "";
            var request = (HttpWebRequest)WebRequest.Create(urlBaseReceitaFederal + paginaValidacao);
            request.ProtocolVersion = HttpVersion.Version10;
            request.CookieContainer = _cookies;
            request.Method = "POST";

            string postData = "";
            postData = postData + "origem=comprovante&";
            postData = postData + "cnpj=" + new Regex(@"[^\d]").Replace(aCNPJ, string.Empty) + "&";
            postData = postData + "txtTexto_captcha_serpro_gov_br=" + aCaptcha + "&";
            postData = postData + "submit1=Consultar&";
            postData = postData + "search_type=cnpj";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            string retorno = "";
            try
            {
                Stream dataStream = request.GetRequestStream(); // PODE OCORRER ERRO AO TENTAR CONECTAR
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse(); // COLOCADO DENTRO DE TRY CATCH - SE O SERVIÇO FICAR FORA DO AR, DA ERRO NESSA LINHA
                StreamReader stHtml = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
                retorno = stHtml.ReadToEnd();
            }
            catch (Exception)
            {
                retornoConsulta = false;
                Mensagem = "Não foi possível consultar o CNPJ.\nServiço da Receita Federal fora do ar ou bloqueado. Tente novamente mais tarde"; // SE O SERVIÇO TIVER FORA, CAI AQUI
            }

            if (retorno.Contains("Verifique se o mesmo foi digitado corretamente"))
            {
                retornoConsulta = false;
                Mensagem = "O número do CNPJ não foi digitado corretamente";
            }

            if (retorno.Contains("Erro na Consulta"))
            {
                retornoConsulta = false;
                Mensagem = "Os caracteres digitados não correspondem com a imagem";
            }


            // EM UM TESTE, QUANDO DIGITADO A IMAGEM ERRADA, A RECEITA RETORNOU UMA PAGINA HTML APENAS COM BOTAO VOLTAR. 
            // ENTAO AQUI ESTOU VERIFICANDO SE RETORNA PELO MENOS A RAZAO SOCIAL PARA VALIDAR O METODO COMO TRUE
            if (retornoConsulta == true && RecuperaColunaValor(retorno, Coluna.RazaoSocial) == "")
            {
                retornoConsulta = false;
                Mensagem = "Não foi possível recuperar os dados. Verifique os dados digitados e Tente novamente";
            }


            if (retornoConsulta == true)
            {
                paginaHTML = retorno; //conteudo do site
                Empresa = new PessoJuridica();

                //Empresa em si
                Empresa.Cnpj = RecuperaColunaValor(paginaHTML, Coluna.NumeroDaInscricao); //aCNPJ;
                Empresa.RazaoSocial = RecuperaColunaValor(paginaHTML, Coluna.RazaoSocial);
                Empresa.NomeFantasia = RecuperaColunaValor(paginaHTML, Coluna.NomeFantasia);
                Empresa.NaturezaJuridica = RecuperaColunaValor(paginaHTML, Coluna.NaturezaJuridica);
                Empresa.AtividadeEconomicaPrimaria = RecuperaColunaValor(paginaHTML, Coluna.AtividadeEconomicaPrimaria);
                Empresa.AtividadeEconomicaSecundaria = RecuperaColunaValor(paginaHTML, Coluna.AtividadesEconomicasSecundarias);
                Empresa.NumeroDaInscricao = RecuperaColunaValor(paginaHTML, Coluna.NumeroDaInscricao);
                Empresa.MatrizFilial = RecuperaColunaValor(paginaHTML, Coluna.MatrizFilial);
                Empresa.SituacaoCadastral = RecuperaColunaValor(paginaHTML, Coluna.SituacaoCadastral);
                Empresa.DataSituacaoCadastral = RecuperaColunaValor(paginaHTML, Coluna.DataSituacaoCadastral);
                Empresa.MotivoSituacaoCadastral = RecuperaColunaValor(paginaHTML, Coluna.MotivoSituacaoCadastral);

                //Endereço
                Empresa.Endereco = RecuperaColunaValor(paginaHTML, Coluna.Endereco);
                Empresa.Numero = RecuperaColunaValor(paginaHTML, Coluna.Numero);
                Empresa.Bairro = RecuperaColunaValor(paginaHTML, Coluna.Bairro);
                Empresa.Cidade = RecuperaColunaValor(paginaHTML, Coluna.Cidade);
                Empresa.CEP = RecuperaColunaValor(paginaHTML, Coluna.CEP);
                Empresa.UF = RecuperaColunaValor(paginaHTML, Coluna.UF);
                Empresa.Complemento = RecuperaColunaValor(paginaHTML, Coluna.Complemento);

                //Contato
                Empresa.Email = RecuperaColunaValor(paginaHTML, Coluna.Email);
                Empresa.Telefones = RecuperaColunaValor(paginaHTML, Coluna.Telefone);

                Empresa.Cnae = RecuperaColunaValor(paginaHTML, Coluna.Cnae);

                Empresa.DataAbertura = RecuperaColunaValor(paginaHTML, Coluna.DataAbertura);
                Empresa.EnteFederativoResponsavel = RecuperaColunaValor(paginaHTML, Coluna.EnteFederativoResponsavel);
                Empresa.SituacaoEspecial = RecuperaColunaValor(paginaHTML, Coluna.SituacaoEspecial);
                Empresa.DataSituacaoEspecial = RecuperaColunaValor(paginaHTML, Coluna.DataSituacaoEspecial);
            }
            return retornoConsulta;
        }

        static public bool ValidaCampos(string aCNPJ, string aCaptcha)
        {
            // VALIDAÇÃO MÍNIMA BASICA.
            // EVITA CONSULTA DESNECESSÁRIA NO SITE DA RECEITA (MUITAS CONSULTAS PODEM LEVAR O BLOQUEIO DO IP)
            string erro = "";

            if (IsCnpj(aCNPJ) == false)
                erro += "CNPJ não informado corretamente\n";

            if (aCaptcha.Length != 6) // SEMPRE 6 CARACTERES
                erro += "Caracteres não informados corretamente\n";

            if (erro.Length > 0)
            {
                Mensagem = erro;
                return false;
            }
            else
                return true;
        }

        //VALIDACAO DE CNPJ
        static private bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma, resto;
            string digito, tempCnpj;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace(" ", "");

            if (cnpj.Length != 14)
            {
                return false;
            }
            else
            {
                tempCnpj = cnpj.Substring(0, 12);

                soma = 0;
                for (int i = 0; i < 12; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

                resto = (soma % 11);
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = resto.ToString();
                tempCnpj = tempCnpj + digito;

                soma = 0;
                for (int i = 0; i < 13; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

                resto = (soma % 11);
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = digito + resto.ToString();
                return cnpj.EndsWith(digito);
            }
        }
        static private string RecuperaColunaValor(String pattern, Coluna col)
        {
            string S = pattern.Replace("\n", "").Replace("\t", "").Replace("\r", "");

            switch (col)
            {
                case Coluna.RazaoSocial:
                    {
                        S = StringEntreString(S, ">NOME EMPRESARIAL<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.NomeFantasia:
                    {
                        S = StringEntreString(S, ">TÍTULO DO ESTABELECIMENTO (NOME DE FANTASIA)<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.NaturezaJuridica:
                    {
                        S = StringEntreString(S, ">CÓDIGO E DESCRIÇÃO DA NATUREZA JURÍDICA<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.AtividadeEconomicaPrimaria:
                    {
                        S = StringEntreString(S, "CÓDIGO E DESCRIÇÃO DA ATIVIDADE ECONÔMICA PRINCIPAL");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.AtividadesEconomicasSecundarias:
                    {
                        S = StringEntreString(S, "ATIVIDADE ECONOMICA SECUNDARIA", "Fim Linha ATIVIDADE ECONOMICA SECUNDARIA");
                        string[] listaAtividades = Regex.Split(S, "<br>");
                        Regex regex = new Regex(@"\s * | * \s", RegexOptions.IgnoreCase | RegexOptions.Compiled); // REGEX PARA REMOVER EXCESSO DE ESPAÇO
                        string atividades = "";

                        foreach (var item in Regex.Split(S, "<br>")) // PERCORRE ENTRE ATIVIDADES
                        {
                            S = StringEntreString(item, "<b>", "</b>"); // PEGA ATIVIDADE
                            if (S != "") // SE EXISTIR ATIVIDADE
                                atividades = atividades + Environment.NewLine + (regex.Replace(S, " ")).Trim(); // ADICIONA LINHA E ADICIONA ATIVIDADE REMOVENDO EXCESSO DE ESPAÇOS
                        }

                        return atividades.Trim();
                    }
                case Coluna.NumeroDaInscricao:
                    {
                        S = StringEntreString(S, ">NÚMERO DE INSCRIÇÃO<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.MatrizFilial:
                    {
                        S = StringEntreString(S, ">NÚMERO DE INSCRIÇÃO<");
                        S = StringEntreString(S, "</b>"); // PULA
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.Endereco:
                    {
                        S = StringEntreString(S, ">LOGRADOURO<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.Numero:
                    {
                        S = StringEntreString(S, ">NÚMERO<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.Complemento:
                    {
                        S = StringEntreString(S, ">COMPLEMENTO<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.CEP:
                    {
                        S = StringEntreString(S, ">CEP<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.Bairro:
                    {
                        S = StringEntreString(S, ">BAIRRO/DISTRITO<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.Cidade:
                    {
                        S = StringEntreString(S, ">MUNICÍPIO<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.UF:
                    {
                        S = StringEntreString(S, ">UF<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.SituacaoCadastral:
                    {
                        S = StringEntreString(S, ">SITUAÇÃO CADASTRAL<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.DataSituacaoCadastral:
                    {
                        S = StringEntreString(S, ">DATA DA SITUAÇÃO CADASTRAL<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.MotivoSituacaoCadastral:
                    {
                        S = StringEntreString(S, ">MOTIVO DE SITUAÇÃO CADASTRAL<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.Email:
                    {
                        S = StringEntreString(S, ">ENDEREÇO ELETRÔNICO<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }

                case Coluna.SituacaoEspecial:
                    {
                        S = StringEntreString(S, ">SITUAÇÃO ESPECIAL<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.DataSituacaoEspecial:
                    {
                        S = StringEntreString(S, ">DATA DA SITUAÇÃO ESPECIAL<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.EnteFederativoResponsavel:
                    {
                        S = StringEntreString(S, ">ENTE FEDERATIVO RESPONSÁVEL (EFR)<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.DataAbertura:
                    {
                        S = StringEntreString(S, ">DATA DE ABERTURA<");
                        S = StringEntreString(S, "<b>", "</b>");
                        return S.Trim();
                    }
                case Coluna.Telefone:
                    {
                        S = StringEntreString(S, ">TELEFONE<");
                        S = StringEntreString(S, "<b>", "</b>"); // APENAS O TELEFONE QUE NAO SEGUE PADRAO POR CONTER ERRO NO HTML
                        return S.Trim();
                    }
                case Coluna.Cnae:
                    {
                        S = StringEntreString(S, ">CÓDIGO E DESCRIÇÃO DA ATIVIDADE ECONÔMICA PRINCIPAL<");
                        S = StringEntreString(S, "<b>", " - ");
                        return S.Trim();
                    }
                default:
                    {
                        return S;
                    }

            }
        }



        /// <summary>
        /// EXTRAI STRING ENTRE STRINGS
        /// </summary>
        /// <param name="Texto">TEXTO</param>
        /// <param name="StrInicio">String Inicial</param>
        /// <param name="StrFinal">String Final</param>
        /// <returns></returns>
        static private string StringEntreString(string Texto, string StrInicio, string StrFinal = "")
        {
            //METODO APRIMORADO. AGORA PEGA O VALOR ENTRE DUAS STRING OU APARTIR DA STRING INICIAL
            try
            {
                int Ini, Fim, Diff;
                Ini = Texto.IndexOf(StrInicio);
                if (Ini == -1) return ""; // SE NAO ENCONTRAR PRIMEIRA PALAVRA, NAO DA ERRO
                Fim = Texto.IndexOf(StrFinal, Ini); // AGORA O FIM SEMPRE COMEÇA APARTIR DA PRIMEIRA STRING
                if (Ini == Fim || Fim == -1) Fim = Texto.Length; // SE NAO TIVER FIM, O FIM SERÁ ULTIMA PALAVRA DO TEXTO
                if (Ini > 0) Ini = Ini + StrInicio.Length;
                if (Fim > 0) Fim = Fim + StrFinal.Length;
                Diff = ((Fim - Ini) - StrFinal.Length);
                if ((Fim > Ini) && (Diff > 0))
                    return Texto.Substring(Ini, Diff);
                else
                    return "";
            }
            catch (Exception)
            {

                return "";
            }

        }
    }
}
