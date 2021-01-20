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

                dataAccess.AddParameters("@InscricaoMunicipal", obj.InscricaoMunicipal);
                dataAccess.AddParameters("@Autorizacao", obj.Autorizacao);
                dataAccess.AddParameters("@Titular", _titular);
                dataAccess.AddParameters("@Auxiliar", _auxiliar);
                dataAccess.AddParameters("@Atividade", obj.Atividade);
                dataAccess.AddParameters("@FormaAtuacao", obj.FormaAtuacao);
                dataAccess.AddParameters("@Veiculo.Modelo", obj.Veiculo.Modelo);
                dataAccess.AddParameters("@Veiculo.Placa", obj.Veiculo.Placa);
                dataAccess.AddParameters("@Veiculo.Cor", obj.Veiculo.Cor);
                dataAccess.AddParameters("@Validade", obj.Validade);
                dataAccess.AddParameters("@Processo", obj.Processo);
                dataAccess.AddParameters("@Situacao", obj.Situacao);

                if (dataAccess.Write(""))
                    return 1;
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

    }
}
