using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleDirections;

namespace SuperPrixTeste
{
    class Program
    {

        /// <summary>
        /// Template do HTML que vai comportar o mapa
        /// </summary>
        private const string CaminhoTemplateMapa = @"Template.html";

        /// <summary>
        /// Template do HTML para a otimização de rota
        /// </summary>
        private const string CaminhoTemplateOtimizacao = @"Template2.html";

        /// <summary>
        /// Arquivo em excel com os endereços e configurações
        /// </summary>
        private const string CaminhoExcel = @"enderecos.xlsx";

        /// <summary>
        /// Função inicial do protótipo
        /// </summary>
        /// <remarks>
        /// Ler o excel com as configs, orgiem e desitnos
        /// Desenhar uma rota
        /// Bater conta o tempo máximo (warning)
        /// Montar link com o mapa ou gerar a imagem
        /// gerar os mapas com a rota para cada ponto.
        /// 
        /// </remarks>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var configuracoes = RecuperarConfiguracoes();
            var enderecosOrigem = RecuperarEnderecosOrigem();
            var enderecosDestino = RecuperarEnderecosDestino();

            List<Location> locations = new List<Location>();

            //Sempre um endereço de origem
            locations.Add(new Location(enderecosOrigem.First()));

            //Destinos, entregas
            foreach (var endereco in enderecosDestino)
            {
                locations.Add(new Location(endereco));
            }

            Route route = RouteDirections.GetRoute(true, locations.ToArray());

            //EscreverRota(route);

            //var horas = route.Duration / 3600;
            //var minutos = route.Duration % 3600 / 60;
            //double tempoMaximoEmhoras = 1; // double.Parse(configuracoes["TempoMaximoDeEntregaEmHoras"]);
            //double tempoTotal = double.Parse(string.Format("{0}.{1}", horas, minutos));

            //if (tempoTotal > tempoMaximoEmhoras)
                EscreverOtimizacaoDeRota(route, enderecosOrigem, enderecosDestino);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> RecuperarConfiguracoes()
        {
            var dicionario = new Dictionary<string, string>();

            string con = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path.GetFullPath(CaminhoExcel) + ";Extended Properties=\"Excel 12.0;HDR=YES\"";

            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from [Configuracao$]", connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        dicionario.Add(dr[0] as string, dr[1] as string);
                    }
                }
            }

            return dicionario;
        }

        /// <summary>
        /// Lê o excel e retorna os possiveis enderecos de origem
        /// </summary>
        /// <returns></returns>
        private static List<string> RecuperarEnderecosOrigem()
        {
            var listaRetorno = new List<string>();

            string con = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path.GetFullPath(CaminhoExcel) + ";Extended Properties=\"Excel 12.0;HDR=YES\"";

            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from [Origem$]", connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        listaRetorno.Add(dr[0] as string);
                    }
                }
            }

            return listaRetorno;
        }

        /// <summary>
        /// Lê o excel e retorna os possiveis enderecos de Destino
        /// </summary>
        /// <returns></returns>
        private static List<string> RecuperarEnderecosDestino()
        {
            var listaRetorno = new List<string>();

            string con = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path.GetFullPath(CaminhoExcel) + ";Extended Properties=\"Excel 12.0;HDR=YES\"";

            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from [Destinos$]", connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        listaRetorno.Add(dr[0] as string);
                    }
                }
            }

            return listaRetorno;
        }

        /// <summary>
        /// Gera e abre html para ficar testando as opções de rota
        /// </summary>
        /// <param name="route"></param>
        /// <param name="enderecosOrigem"></param>
        /// <param name="enderecosDestino"></param>
        private static void EscreverOtimizacaoDeRota(Route route, List<string> enderecosOrigem, List<string> enderecosDestino)
        {
            string htmlMapa = File.ReadAllText(CaminhoTemplateOtimizacao, System.Text.Encoding.UTF8);

            //Opção de inicio
            string inicio = string.Format("<option value=\"{0}\">{0}</option>", enderecosOrigem.First());
            string destinos = "";

            foreach (var endereco in enderecosDestino)
            {
                destinos += string.Format("<option value=\"{0}\">{0}</option>", endereco);
            }

            htmlMapa = htmlMapa.Replace("#origem", inicio);
            htmlMapa = htmlMapa.Replace("#destino", destinos);

            File.Delete("otimizacao.html");
            File.AppendAllText("otimizacao.html", htmlMapa, System.Text.Encoding.UTF8);
            Process.Start("otimizacao.html");

        }

        /// <summary>
        /// Gera e abre o html com o mapa
        /// </summary>
        /// <param name="route"></param>
        private static void EscreverRota(Route route)
        {
            string htmlMapa = File.ReadAllText(CaminhoTemplateMapa);

            

            string passos = "";


            var horas = route.Duration / 3600;
            var minutos = route.Duration % 3600 / 60;
            var km = route.Distance/1000;
            var metros = route.Distance % 1000;

            int contador = 0;
            foreach (var routeLeg in route.Legs)
            {
                contador++;
                passos += string.Format("{{\"title\": '{0}', \"lat\": '{1}', \"lng\": '{2}', \"description\": '{3}{0}'}}", contador, routeLeg.StartLocation.Latitude.ToString().Replace(",", "."), routeLeg.StartLocation.Longitude.ToString().Replace(",", "."), "Passo ") + ",";

            }

            htmlMapa = htmlMapa.Replace("#passos", passos.Remove(passos.Length -1, 1));
            
            File.Delete("mapa.html");
            File.AppendAllText("mapa.html", htmlMapa);
            Process.Start("mapa.html");

        }
    }
}
