using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Services.Correios
{
    public class CEP
    {
        static public mEndereco Endereco = new mEndereco();

        static public bool Consultar(string cep)
        {

            try
            {
                if (cep == string.Empty)
                    return false;

                WSCorreios.AtendeClienteClient ws = new WSCorreios.AtendeClienteClient();
                WSCorreios.enderecoERP resposta = new WSCorreios.enderecoERP();
                resposta = ws.consultaCEP(cep);
                Endereco.CEP = resposta.cep;
                Endereco.Logradouro = resposta.end.ToUpper();
                Endereco.Bairro = resposta.bairro.ToUpper();
                Endereco.Municipio = resposta.cidade.ToUpper();
                Endereco.UF = resposta.uf.ToUpper();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(string.Format("Erro ao efetuar busca do CEP: {0}", ex.Message));
            }

        }

        //***********Descontinuado***************//
        /*static public bool ConsultarA(string cep)
        {

            string ncep = cep.Replace("-", "");

            bool retornoConsulta = true;

            HttpWebRequest requisicao = (HttpWebRequest)WebRequest.Create(
                string.Format(@"http://www.buscacep.correios.com.br/servicos/dnec/consultaLogradouroAction.do?Metodo=listaLogradouro&CEP={0}&TipoConsulta=cep", cep));

            HttpWebResponse resposta = (HttpWebResponse)requisicao.GetResponse();

            int cont;
            byte[] buffer = new byte[1000];
            StringBuilder sb = new StringBuilder();
            string temp;

            Stream stream = resposta.GetResponseStream();

            do
            {
                cont = stream.Read(buffer, 0, buffer.Length);
                temp = Encoding.Default.GetString(buffer, 0, cont).Trim();
                sb.Append(temp);

            } while (cont > 0);

            string pagina = sb.ToString();

            if (pagina.IndexOf("<font color=\"black\">CEP NAO ENCONTRADO</font>") >= 0)
            {
                throw new Exception("CEP não localizado.");
            }
            else
            {
                string logradouro = Regex.Match(pagina,
                    "<td width=\"268\" style=\"padding: 2px\">(.*)</td>").Groups[1].Value;

                string bairro = Regex.Matches(pagina,
                    "<td width=\"140\" style=\"padding: 2px\">(.*)</td>")[0].Groups[1].Value;

                string cidade = Regex.Matches(pagina,
                    "<td width=\"140\" style=\"padding: 2px\">(.*)</td>")[1].Groups[1].Value;

                string estado = Regex.Match(pagina,
                    "<td width=\"25\" style=\"padding: 2px\">(.*)</td>").Groups[1].Value;

                Endereco.Logradouro = logradouro;
                Endereco.Bairro = bairro;
                Endereco.Municipio = cidade;
                Endereco.UF = estado;
            }

            return retornoConsulta;
        }*/
    }
}
