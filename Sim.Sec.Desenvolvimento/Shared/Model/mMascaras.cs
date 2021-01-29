using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    public class mMascaras
    {
        public string Remove(string valor)
        {
            var str = valor;

            if (str != string.Empty || str != null)

                return str = new string((from c in str
                                         where char.IsWhiteSpace(c) || char.IsLetterOrDigit(c)
                                         select c
                       ).ToArray());

            return str;
        }

        public string CNPJ(string valor)
        {
            try
            {
                string v = Remove(valor);

                return Convert.ToUInt64(v).ToString(@"00\.000\.000\/0000\-00");
            }
            catch { return valor; }
        }

        public string CPF(string valor)
        {
            try
            {
                string v = Remove(valor);

                return Convert.ToUInt64(v).ToString(@"000\.000\.000\-00");
            }
            catch { return valor; }
        }

        public string CEP(string valor)
        {
            try
            {
                string v = Remove(valor);

                return Convert.ToUInt64(v).ToString(@"00\.000\-000");
            }
            catch { return valor; }
        }

        public string Phone(string valor)
        {
            try
            {

                string v = Remove(valor);

                if (v.Length >= 11)
                    return Convert.ToUInt64(v).ToString(@"00\.00000\-0000");
                else
                    return Convert.ToUInt64(v).ToString(@"00\.0000\-0000");
            }catch
            { return valor; }
        }

        public string CNAE(string valor)
        {
            try
            {
                string v = Remove(valor);

                return Convert.ToUInt64(v).ToString(@"00\.00\-0\-00");
            }
            catch { return valor; }
        }

        public string CNAE_V(string valor)
        {
            try
            {
                string v = Remove(valor);

                return Convert.ToUInt64(v).ToString(@"0000\-0\/00");
            }
            catch { return valor; }
        }

    }
}
