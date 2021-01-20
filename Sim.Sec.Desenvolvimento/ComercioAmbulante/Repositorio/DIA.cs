using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.Repositorio
{

    using Mvvm.Observers;
    using Shared.Model;

    public class DIA: NotifyProperty
    {
        public int Gravar(Model.DIA obj)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {
                dataAccess.ClearParameters();

                string _titular = string.Format(@"{0};{1};",
                    obj.Titular.Nome,
                    obj.Titular.RG);

                string _auxiliar = string.Format(@"{0};{1};",
                    obj.Auxiliar.Nome,
                    obj.Auxiliar.RG);

                string _veiculo = string.Format(@"{0};{1};{2};",
                    obj.Veiculo.Modelo,
                    obj.Veiculo.Placa,
                    obj.Veiculo.Cor);

                dataAccess.AddParameters("@InscricaoMunicipal", obj.InscricaoMunicipal);                
                dataAccess.AddParameters("@Autorizacao", obj.Autorizacao);
                dataAccess.AddParameters("@Titular", _titular);
                dataAccess.AddParameters("@Auxiliar", _auxiliar);
                dataAccess.AddParameters("@Atividade", obj.Atividade);
                dataAccess.AddParameters("@FormaAtuacao", obj.FormaAtuacao);
                dataAccess.AddParameters("@Veiculo", _veiculo);
                dataAccess.AddParameters("@Emissao", obj.Emissao.ToShortDateString());
                dataAccess.AddParameters("@Validade", obj.Validade);
                dataAccess.AddParameters("@Processo", obj.Processo);
                dataAccess.AddParameters("@Situacao", obj.Situacao);

                string _novo = @"INSERT INTO SDT_CAmbulante_DIA 
([InscricaoMunicipal], [Autorizacao], [Titular], [Auxiliar], [Atividade], [FormaAtuacao], [Veiculo], [Emissao], [Validade], [Processo], [Situacao]) 
VALUES 
(@InscricaoMunicipal, @Autorizacao, @Titular, @Auxiliar, @Atividade, @FormaAtuacao, @Veiculo, @Emissao, @Validade, @Processo, @Situacao)";


                if (dataAccess.Write(_novo))
                    return 1;
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        public Model.DIA Consulta(Model.DIA obj)
        {

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Desenvolvimento);

            try
            {


                return null;
            }
            catch
            {
                return null;
            }
               
        }

    }
}
